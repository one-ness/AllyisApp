using AllyisApps.Core;
using AllyisApps.DBModel;
using AllyisApps.Services;
using AllyisApps.Services.Auth;
using AllyisApps.Services.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UploadDataDirect
{
	class Program
	{
		static ServiceSettings settings;
		public static AppService appService;

		/// <summary>
		/// Expected args 
		/// </summary>
		/// <param name="args">email, password, pathToFile, optional=organizaionID, optional=</param>
		public static void Main(string[] args)
		{
			GlobalSettings.Init();
			settings = new ServiceSettings(GlobalSettings.SqlConnectionString, GlobalSettings.SupportEmail, GlobalSettings.SendGridApiKey);
			Console.WriteLine(GlobalSettings.SqlConnectionString);
			if (args.Length < 3)
			{
				throw new ArgumentException($"Expected email, password, upload_file_path, " +
				$"(organizaionId empty if create new organizaion, subscriptoinID empty if create new subcriptoin)");
			}
			
			appService = new AppService(settings);
			CacheContainer.Init(settings.SqlConnectionString);
			
			//Expected Args  email, password, file, organizationId=optional, subscriptionId optional 

			User user = appService.ValidateLogin(args[0],args[1]).Result;
			var existingUser = appService.GetUserByEmailAsync(args[0]).Result;
			DateTime today = DateTime.Now;
			
			if(user == null)
			{
				if(existingUser != null)
				{
					throw new ArgumentException("Password is incorrect");
				}
				var emailCode = Guid.NewGuid();
				int userId = appService.SetupNewUser
					(args[0], args[1], "Uploader", "Owner", emailCode, today.AddYears(-18), 
					null, null, null, null, null, null, null,
					"Added Manually", "You ran the manual uploader please go to allyis.com to sign in")
					.Result;
				user = appService.ValidateLogin(args[0], args[1]).Result;
			}

			appService.PopulateUserContext(user.UserId);
			int orgID = 0;
			if(args.Length < 4)
			{
				//No organizaion ID create org
				OrganizaionUploader orgUpload = new OrganizaionUploader(appService);
				orgID = orgUpload.CreateOrganizaion().Result;
				appService.PopulateUserContext(user.UserId);
			}
			else
			{
				orgID = int.Parse(args[3]);
			}
			bool isNew = false;
			int subID = 0;
			if(args.Length < 5)
			{
				SubscriptionCreate subscriptionCreater = new SubscriptionCreate(appService,orgID);
				subID = subscriptionCreater.CreateTimeTrackerSubscription().Result;
				appService.PopulateUserContext(user.UserId);
				isNew = true;
			}
			else
			{
				subID = int.Parse(args[4]);
			};

			//Begin reading Data
			DataUploader data = new DataUploader(args[2], appService, orgID, subID,isNew);
			data.UploadData().Wait();
		}
	}
}
