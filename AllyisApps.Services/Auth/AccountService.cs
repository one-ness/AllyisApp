//------------------------------------------------------------------------------
// <copyright file="AccountService.cs" company="Allyis, Inc.">
//     Copyright (c) Allyis, Inc.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using AllyisApps.DBModel;
using AllyisApps.DBModel.Auth;
using AllyisApps.DBModel.Billing;
using AllyisApps.DBModel.Shared;
using AllyisApps.Lib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.Services
{
	/// <summary>
	/// Business logic for all account related operations.
	/// </summary>
	public partial class AppService : BaseService
	{
		#region public static

		/// <summary>
		/// Returns a compressed version of the given email address if it is too long.
		/// </summary>
		/// <param name="fullEmail">Full email address.</param>
		/// <returns>Compressed email address, or the full address if it is short enough (or null).</returns>
		public static string GetCompressedEmail(string fullEmail)
		{
			if (!string.IsNullOrEmpty(fullEmail) && fullEmail.Length > 50)
			{
				string cemail = string.Format("{0}...{1}", fullEmail.Substring(0, 20), fullEmail.Substring(fullEmail.Length - 15));
				return cemail;
			}
			else
			{
				return fullEmail;
			}
		}

		#endregion public static

		#region public

		/// <summary>
		/// Returns a collection of valid states/provinces for the given country.
		/// </summary>
		/// <param name="countryName">Country name.</param>
		/// <returns><see cref="IEnumerable"/> of valid states/provinces.</returns>
		public IEnumerable ValidStates(string countryName)
		{
			if (string.IsNullOrWhiteSpace(countryName)) throw new ArgumentException("countryName");
			return DBHelper.ValidStates(countryName);
		}

		/// <summary>
		/// Gets the list of valid countries.
		/// </summary>
		/// <returns>A collection of valid countries.</returns>
		public IEnumerable<string> ValidCountries()
		{
			return DBHelper.ValidCountries();
		}

		/// <summary>
		/// Gets the list of valid languages selections.
		/// </summary>
		/// <returns>A list of languages.</returns>
		public IEnumerable<Language> ValidLanguages()
		{
			return DBHelper.ValidLanguages().Select(s => new Language
			{
				LanguageId = s.LanguageID,
				LanguageName = s.LanguageName,
				CultureName = s.CultureName
			});
		}

		/// <summary>
		/// Retrieves a list of all pending invitations for a given user by the user's email.
		/// </summary>
		/// <param name="userEmail">The user's email address.</param>
		/// <returns>The invitation associated with the user.</returns>
		public List<InvitationInfo> GetInvitationsByUser(string userEmail)
		{
			#region Validation

			if (!Utility.IsValidEmail(userEmail))
			{
				throw new FormatException("Email address must be in a valid format.");
			}

			#endregion Validation

			var invitationsDB = DBHelper.GetUserInvitationsByUserData(new UserDBEntity()
			{
				Email = userEmail
			});

			return invitationsDB.Select(idb => InitializeInvitationInfo(idb)).ToList();
		}

		/// <summary>
		/// Accepts an invitation, adding the user to the invitation's organization, subscriptions, and projects, then deletes the invitations.
		/// </summary>
		/// <param name="invitationId">The invitationId.</param>
		/// <returns>The resulting action message if succeed, null if fail.</returns>
		public string AcceptUserInvitation(int invitationId)
		{
			#region Validation

			if (invitationId <= 0) throw new ArgumentOutOfRangeException("invitationId", "The invitation id cannot be zero or negative.");

			#endregion Validation

			var results = DBHelper.AcceptInvitation(invitationId, UserContext.UserId);

			if (results == null) return null;

			return string.Format("You have successfully joined {0} in the role of {1}.", results.Item1, results.Item2);
		}

		/// <summary>
		/// Rejects an invitation, deleting it from a user's pending invites.
		/// </summary>
		/// <param name="invitationId">The id of the invitation to reject.</param>
		/// <returns>The resulting message.</returns>
		public string RejectUserInvitation(int invitationId)
		{
			if (DBHelper.RemoveInvitation(invitationId, UserContext.UserId))
			{
				return "The invitation has been rejected.";
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Setup a new user.
		/// </summary>
		/// <param name="email">The new user's email.</param>
		/// <param name="firstName">The new user's first name.</param>
		/// <param name="lastName">The new user's last name.</param>
		/// <param name="dateOfBirth">The new user's date of birth.</param>
		/// <param name="address">The new user's street address.</param>
		/// <param name="city">The new user's city portion of their address.</param>
		/// <param name="state">The new user's state portion of their address.</param>
		/// <param name="country">The new user's country portion of their address.</param>
		/// <param name="postalCode">The new user's postal code.</param>
		/// <param name="phone">The new user's phone number.</param>
		/// <param name="password">The new user's password.</param>
		/// <param name="confirmUrl">The template of the Url for the confirm email link, with userId and code replaced by "{userId}" and "{code}".</param>
		/// <param name="languagePreference">The new user's language preference.</param>
		/// <param name="twoFactorEnabled">A boolean representing whether the new user has enabled 2FA. Defaults to false.</param>
		/// <param name="lockOutEnabled">A boolean representing whether the new user is locked out from failed access attempts.  Defaults to false.</param>
		/// <param name="lockOutEndDateUtc">The time whent the user's lockout will end.  Defaults to null.</param>
		/// <returns>Returns 0 if user already exists, 0 if there is failure.</returns>
		public async Task<Tuple<int, int>> SetupNewUser(
			string email,
			string firstName,
			string lastName,
			DateTime? dateOfBirth,
			string address,
			string city,
			string state,
			string country,
			string postalCode,
			string phone,
			string password,
			int languagePreference,
			string confirmUrl,
			bool twoFactorEnabled = false,
			bool lockOutEnabled = false,
			DateTime? lockOutEndDateUtc = null)
		{
			#region Validation

			if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
			{
				throw new ArgumentException("User name must have a value.");
			}

			if (string.IsNullOrEmpty(email))
			{
				throw new ArgumentNullException("email", "Email address must have a value.");
			}
			else if (!Utility.IsValidEmail(email))
			{
				throw new FormatException("Email address must be in a valid format.");
			}

			if (string.IsNullOrEmpty(password))
			{
				throw new ArgumentNullException("password", "Password must have a value.");
			}

			#endregion Validation

			//int result = 0;
			Tuple<int, int> result = null;
			try
			{
				UserDBEntity entity = new UserDBEntity()
				{
					Email = email,
					FirstName = firstName,
					LastName = lastName,
					DateOfBirth = dateOfBirth,
					Address = address,
					City = city,
					State = state,
					Country = country,
					PostalCode = postalCode,
					PhoneNumber = phone,
					PasswordHash = Crypto.GetPasswordHash(password),
					TwoFactorEnabled = twoFactorEnabled,
					LockoutEnabled = lockOutEnabled,
					LockoutEndDateUtc = lockOutEndDateUtc,
					LanguagePreference = languagePreference
				};
				Guid emailConfirmCode = Guid.NewGuid();
				result = this.DBHelper.CreateUser(entity, emailConfirmCode);

				// send confirmation email
				string url = confirmUrl.Replace("%7BuserId%7D", result.Item1.ToString()).Replace("%7Bcode%7D", emailConfirmCode.ToString());
				await SendConfirmationEmail("support@allyisapps.com", email, url);
			}
			catch (SqlException ex)
			{
				if (ex.Message.ToLower().Contains("unique"))
				{
					// unique constraint violation of email
					result = null;
				}
				else
				{
					throw;
				}
			}

			return result;
		}

		/// <summary>
		/// Validates the given email and password with the database.
		/// </summary>
		/// <param name="email">The login email.</param>
		/// <param name="password">The login password.</param>
		public UserContext ValidateLogin(string email, string password)
		{
			if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("email");
			if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("password");

			UserContext result = null;
			var user = this.DBHelper.GetUserByEmail(email);
			if (user != null)
			{
				// email exists, hash the given password and compare with hash in db
				Tuple<bool, string> passwordValidation = Crypto.ValidateAndUpdate(password, user.PasswordHash);
				if (passwordValidation.Item1)
				{
					result = new UserContext(user.UserId, email, user.FirstName, user.LastName);

					// Store updated password hash if needed
					if (passwordValidation.Item2 != null)
					{
						DBHelper.UpdateUserPassword(user.UserId, passwordValidation.Item2);
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Uses the database to return a fully populated UserContext from the userId.
		/// </summary>
		/// <param name="userId">The user Id to look up.</param>
		/// <returns>The User context after population.</returns>
		public UserContext PopulateUserContext(int userId)
		{
			if (userId <= 0) throw new ArgumentException("userId");

			UserContext result = null;
			List<UserContextDBEntity> contextInfo = this.DBHelper.GetUserContextInfo(userId);
			if (contextInfo != null && contextInfo.Count > 0)
			{
				// user exists in db
				UserContextDBEntity firstRow = contextInfo[0];
				result = new UserContext(userId, firstRow.Email, firstRow.FirstName, firstRow.LastName, firstRow.LastSubscriptionId == null ? 0 : firstRow.LastSubscriptionId.Value, firstRow.LanguagePreference.Value);
				// set result to self
				this.SetUserContext(result);

				// note: if contextInfo.Count > 0, user is part of at least one organization
				bool chosenSubscriptionFound = false;
				foreach (var item in contextInfo)
				{
					// user is part of at least one organization, do we have org id?
					if (item.OrganizationId.HasValue)
					{
						// yes, was it already added to the list?
						UserOrganization orgInfo = null;
						if (!result.UserOrganizations.TryGetValue(item.OrganizationId.Value, out orgInfo))
						{
							// no, add it now
							orgInfo = new UserOrganization
							{
								OrganizationId = item.OrganizationId.Value,
								OrganizationName = item.OrganizationName,
								OrganizationRole = (OrganizationRole)item.OrgRoleId.Value,
							};

							result.UserOrganizations.Add(item.OrganizationId.Value, orgInfo);
						}

						// is there a subscription id?
						if (item.SubscriptionId.HasValue)
						{
							UserSubscription subInfo = new UserSubscription()
							{
								SubscriptionId = item.SubscriptionId.Value,
								OrganizationId = orgInfo.OrganizationId,
								OrganizationName = orgInfo.OrganizationName,
								ProductId = (ProductIdEnum)item.ProductId.Value,
								ProductName = item.ProductName,
								ProductRoleName = item.ProductRoleName,
								ProductRoleId = item.ProductRoleId.Value,
								SkuId = item.SkuId.Value,
								AreaUrl = item.AreaUrl
							};

							// add it to the list of subscriptions for this organization
							orgInfo.UserSubscriptions.Add(item.SubscriptionId.Value, subInfo);

							// also add it to the result
							result.UserSubscriptions.Add(item.SubscriptionId.Value, subInfo);

							// compare with chosen subscription? is user still a member of it?
							if (result.ChosenSubscriptionId == item.SubscriptionId.Value)
							{
								chosenSubscriptionFound = true;
							}
						}
					}
				}

				// was chosen subscription found?
				if (!chosenSubscriptionFound && result.ChosenSubscriptionId > 0)
				{
					// no, set it to 0
					result.ChosenSubscriptionId = 0;

					// update database
					this.UpdateActiveSubscription(result.ChosenSubscriptionId);
				}
			}

			return result;
		}

		/// <summary>
		/// Updates the active subsciption for the current user.
		/// </summary>
		/// <param name="subscriptionId">Subscription Id.</param>
		public void UpdateActiveSubscription(int? subscriptionId)
		{
			if (subscriptionId.HasValue && subscriptionId.Value <= 0) throw new ArgumentException("subscriptionId");
			DBHelper.UpdateActiveSubscription(UserContext.UserId, subscriptionId);
		}

		/// <summary>
		/// Gets the user info for the current user.
		/// </summary>
		/// <returns>A User instance with the current user's info.</returns>
		public User GetUser()
		{
			return GetUser(UserContext.UserId);
		}

		/// <summary>
		/// Gets the User for the current user, along with Organizations for each organization the
		/// user is a member of, and InvitationInfos for any invitations for the user.
		/// </summary>
		/// <returns></returns>
		public Tuple<User, List<Organization>, List<InvitationInfo>> GetUserOrgsAndInvitationInfo()
		{
			var spResults = DBHelper.GetUserOrgsAndInvitations(UserContext.UserId);
			return Tuple.Create<User, List<Organization>, List<InvitationInfo>>(
				InitializeUser(spResults.Item1),
				spResults.Item2.Select(odb => InitializeOrganization(odb)).ToList(),
				spResults.Item3.Select(idb => InitializeInvitationInfo(idb)).ToList());
		}

		/// <summary>
		/// Gets the user info for a specific user.
		/// </summary>
		/// <param name="userId">User Id.</param>
		/// <returns>A User instance with the current user's info.</returns>
		public User GetUser(int userId)
		{
			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException("userId", "User ID cannot be 0 or negative.");
			}

			return InitializeUser(DBHelper.GetUserInfo(userId));
		}

		/// <summary>
		/// Gets the user info from an email address.
		/// </summary>
		/// <param name="email">Email address.</param>
		/// <returns>A UserInfo instance with the user's info.</returns>
		public User GetUserByEmail(string email)
		{
			if (!Utility.IsValidEmail(email)) throw new ArgumentException("email");

			return InitializeUser(DBHelper.GetUserByEmail(email));
		}

		/// <summary>
		/// Saves the user info in the database.
		/// </summary>
		/// <param name="model">UserInfo containing updated info.</param>
		public void SaveUserInfo(User model)
		{
			if (model == null)
			{
				throw new ArgumentNullException("model", "UserInfo object must not be null.");
			}

			// TODO: Add UserInfo->UserDBEntity conversion at bottom
			DBHelper.UpdateUser(new UserDBEntity
			{
				AccessFailedCount = model.AccessFailedCount,
				ActiveOrganizationId = model.ActiveOrganizationId,
				Address = model.Address,
				City = model.City,
				Country = model.Country,
				DateOfBirth = model.DateOfBirth,
				Email = model.Email,
				EmailConfirmed = model.EmailConfirmed,
				FirstName = model.FirstName,
				LastName = model.LastName,
				LastSubscriptionId = model.LastSubscriptionId,
				LockoutEnabled = model.LockoutEnabled,
				LockoutEndDateUtc = model.LockoutEndDateUtc,
				PasswordHash = model.PasswordHash,
				PasswordResetCode = model.PasswordResetCode,
				PhoneExtension = model.PhoneExtension,
				PhoneNumber = model.PhoneNumber,
				PhoneNumberConfirmed = model.PhoneNumberConfirmed,
				State = model.State,
				TwoFactorEnabled = model.TwoFactorEnabled,
				UserId = model.UserId,
				PostalCode = model.PostalCode
			});
		}

		/// <summary>
		/// Sets the language preference for the current user.
		/// </summary>
		/// <param name="languageID">The language ID.</param>
		public void SetLanguage(int languageID)
		{
			if (languageID < 0)
			{
				throw new ArgumentOutOfRangeException("languageID", "Language ID cannot be negative.");
			}

			DBHelper.UpdateUserLanguagePreference(UserContext.UserId, languageID);
		}

		/// <summary>
		/// Gets the browser-compatible universal culture language string (e.g. "en-US") based on language ID.
		/// </summary>
		/// <param name="languageID">The language ID. May use 0 to indicate no language setting.</param>
		/// <returns>Culture string.</returns>
		public Language GetLanguage(int languageID)
		{
			if (languageID < 0)
			{
				throw new ArgumentOutOfRangeException("languageID", "Language ID cannot be negative.");
			}

			// No language setting, use browser setting. May return null if browser culture is unsupported in this app's database.
			if (languageID == 0)
			{
				return this.ValidLanguages().Where(c => System.Globalization.CultureInfo.CurrentCulture.Name.Equals(c.CultureName)).SingleOrDefault();
			}

			LanguageDBEntity language = DBHelper.GetLanguage(languageID);
			return new Language
			{
				LanguageId = language.LanguageID,
				LanguageName = language.LanguageName,
				CultureName = language.CultureName
			};
		}

		/// <summary>
		/// Sends an email with password reset link to the given email address.
		/// </summary>
		/// <param name="email">The user email address.</param>
		/// <param name="code">the password reset code that is </param>
		/// <param name="callbackUrl">The Url to include as the "click here" link, with stand-ins for userid and code (as "{userid}" and "{code}".</param>
		/// <returns>A value indicating whether the given email address matched with an existing user.</returns>
		public async Task<bool> SendPasswordResetMessage(string email, string code, string callbackUrl)
		{
			if (!Utility.IsValidEmail(email)) throw new ArgumentException("email");
			if (string.IsNullOrWhiteSpace(code)) throw new ArgumentNullException("code");
			if (string.IsNullOrWhiteSpace(callbackUrl)) throw new ArgumentNullException("callbackUrl");

			bool result = false;
			int userId = this.DBHelper.UpdateUserPasswordResetCode(email, code);
			if (userId > 0)
			{
				// Send reset email
				StringBuilder sb = new StringBuilder();
				sb.AppendFormat("Please reset your password by clicking <a href=\"{0}\">this reset link</a>.", callbackUrl);
				string msgbody = new System.Web.HtmlString(sb.ToString()).ToString();
				await Mailer.SendEmailAsync("noreply@allyisapps.com", email, "Reset password", msgbody);
				result = true;
			}

			return result;
		}

		/// <summary>
		/// Reset password.
		/// </summary>
		/// <param name="userId">The user ID.</param>
		/// <param name="code">The code.</param>
		/// <param name="password">The password.</param>
		/// <returns>The reset password task.</returns>
		public async Task<bool> ResetPassword(int userId, Guid code, string password)
		{
			if (userId <= 0) throw new ArgumentNullException("userId");
			if (code == null) throw new ArgumentNullException("code");
			if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException("password");
			return await Task.Run(() =>
			{
				// update password for user and reset password code to null
				return this.DBHelper.UpdateUserPasswordUsingCode(userId, Crypto.GetPasswordHash(password), code) == userId;
			});
		}

		/// <summary>
		/// Changes a user's password.
		/// </summary>
		/// <param name="oldPassword">Old password, for verification.</param>
		/// <param name="newPassword">New password to change it to.</param>
		/// <returns>True for a successful change, false if anything fails.</returns>
		public bool ChangePassword(string oldPassword, string newPassword)
		{
			if (string.IsNullOrWhiteSpace(oldPassword)) throw new ArgumentNullException("oldPassword");
			if (string.IsNullOrWhiteSpace(newPassword)) throw new ArgumentNullException("newPassword");

			bool result = false;
			string passwordHash = this.DBHelper.GetPasswordHashById(UserContext.UserId);
			if (!string.IsNullOrWhiteSpace(passwordHash))
			{
				Tuple<bool, string> validation = Crypto.ValidateAndUpdate(oldPassword, passwordHash);
				if (validation.Item1)
				{
					// old password is correct.
					result = true;
					this.DBHelper.UpdateUserPassword(UserContext.UserId, Crypto.GetPasswordHash(newPassword));
				}
			}

			return result;
		}

		/// <summary>
		/// Gets a password hash.
		/// </summary>
		/// <param name = "password" >The password entered by user.</param>
		/// <returns>Password hash.</returns>
		public string GetPasswordHash(string password)
		{
			if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException("password");
			return Crypto.GetPasswordHash(password);
		}

		/// <summary>
		/// Send confirmation email after registration.
		/// </summary>
		/// <param name="userId">The user id.</param>
		/// <returns>The async confirmation email code task.</returns>
		public async Task<string> GetConfirmEmailCode(int userId)
		{
			if (userId <= 0)
			{
				throw new ArgumentOutOfRangeException("userId", "User ID cannot be 0 or negative.");
			}

			string result = Guid.NewGuid().ToString();
			await Task.Run(() =>
			{
				return;
			});

			return result;
		}

		/// <summary>
		/// Send confirmation email.
		/// </summary>
		/// <param name="from">Who the message is from.</param>
		/// <param name="to">Who the message is to.</param>
		/// <param name="confirmEmailUrl">The URL for email confirmation.</param>
		/// <returns>The async confirmation email task.</returns>
		public async Task SendConfirmationEmail(string from, string to, string confirmEmailUrl)
		{
			if (!Utility.IsValidEmail(from)) throw new ArgumentException("from");
			if (!Utility.IsValidEmail(to)) throw new ArgumentException("to");
			if (string.IsNullOrWhiteSpace(confirmEmailUrl)) throw new ArgumentNullException("confirmEmailUrl");

			string bodyHtml = string.Format("Please confirm your email by clicking <a href=\"{0}\">here</a>", confirmEmailUrl);
			await Mailer.SendEmailAsync(from, to, "Confirm Email", bodyHtml);
		}

		/// <summary>
		/// Confirms the users email.
		/// </summary>
		/// <param name="userId">User Id.</param>
		/// <param name="code">The confirmation code.</param>
		/// <returns>True on success, false if the email is already confirmed or some other failure occurs.</returns>
		public bool ConfirmUserEmail(int userId, Guid code)
		{
			if (userId <= 0) throw new ArgumentException("userId");
			if (code == null) throw new ArgumentNullException("code");

			bool result = false;
			UserDBEntity userDbEntity = DBHelper.GetUserInfo(userId);
			if (!userDbEntity.EmailConfirmed)
			{
				string guidstr = code.ToString();
				if (string.Compare(userDbEntity.EmailConfirmationCode.ToString(), guidstr, true) == 0)
				{
					result = DBHelper.UpdateEmailConfirmed(userId, guidstr);
				}
			}

			return result;
		}

		/// <summary>
		/// Gets a list of <see cref="Organization"/>s for all organizations the user is a part of.
		/// </summary>
		/// <returns>Collection of Organizations.</returns>
		public IEnumerable<Organization> GetOrganizationsByUserId()
		{
			return DBHelper.GetOrganizationsByUserId(UserContext.UserId).Select(o => InitializeOrganization(o));
		}

		#endregion public

		#region Info-DBEntity Conversions

		/// <summary>
		/// Translates a UserDBEntity into a User business object.
		/// </summary>
		/// <param name="user">UserDBEntity instance.</param>
		/// <returns>User instance.</returns>
		public static User InitializeUser(UserDBEntity user)
		{
			if (user == null)
			{
				return null;
			}

			return new User
			{
				AccessFailedCount = user.AccessFailedCount,
				ActiveOrganizationId = user.ActiveOrganizationId,
				Address = user.Address,
				City = user.City,
				Country = user.Country,
				DateOfBirth = user.DateOfBirth,
				Email = user.Email,
				EmailConfirmed = user.EmailConfirmed,
				FirstName = user.FirstName,
				LastName = user.LastName,
				LastSubscriptionId = user.LastSubscriptionId,
				LockoutEnabled = user.LockoutEnabled,
				LockoutEndDateUtc = user.LockoutEndDateUtc,
				PasswordHash = user.PasswordHash,
				PasswordResetCode = user.PasswordResetCode,
				PhoneExtension = user.PhoneExtension,
				PhoneNumber = user.PhoneNumber,
				PhoneNumberConfirmed = user.PhoneNumberConfirmed,
				State = user.State,
				TwoFactorEnabled = user.TwoFactorEnabled,
				UserId = user.UserId,
				PostalCode = user.PostalCode
			};
		}

		/// <summary>
		/// Translates a User into a UserDBEntity.
		/// </summary>
		/// <param name="user">User instance.</param>
		/// <returns>UserDBEntity instance.</returns>
		public static UserDBEntity GetDBEntityFromUser(User user)
		{
			if (user == null)
			{
				return null;
			}

			return new UserDBEntity
			{
				AccessFailedCount = user.AccessFailedCount,
				ActiveOrganizationId = user.ActiveOrganizationId,
				Address = user.Address,
				City = user.City,
				Country = user.Country,
				DateOfBirth = user.DateOfBirth,
				Email = user.Email,
				EmailConfirmed = user.EmailConfirmed,
				FirstName = user.FirstName,
				LastName = user.LastName,
				LastSubscriptionId = user.LastSubscriptionId,
				LockoutEnabled = user.LockoutEnabled,
				LockoutEndDateUtc = user.LockoutEndDateUtc,
				PasswordHash = user.PasswordHash,
				PasswordResetCode = user.PasswordResetCode,
				PhoneExtension = user.PhoneExtension,
				PhoneNumber = user.PhoneNumber,
				PhoneNumberConfirmed = user.PhoneNumberConfirmed,
				State = user.State,
				TwoFactorEnabled = user.TwoFactorEnabled,
				UserId = user.UserId,
				PostalCode = user.PostalCode,
				LanguagePreference = 1          // TODO: Put this into UserInfo and do proper lookup
			};
		}

		/// <summary>
		/// Translates a <see cref="UserRolesDBEntity"/> into a <see cref="UserRolesInfo"/>.
		/// </summary>
		/// <param name="userRoles">UserRolesDBEntity instance.</param>
		/// <returns>UserRolesInfo instance.</returns>
		public static UserRolesInfo InitializeUserRolesInfo(UserRolesDBEntity userRoles)
		{
			if (userRoles == null)
			{
				return null;
			}

			return new UserRolesInfo
			{
				Email = userRoles.Email,
				FirstName = userRoles.FirstName,
				LastName = userRoles.LastName,
				Name = userRoles.Name,
				OrgRoleId = userRoles.OrgRoleId,
				ProductRoleId = userRoles.ProductRoleId == null ? -1 : userRoles.ProductRoleId.Value,
				SubscriptionId = userRoles.SubscriptionId == null ? -1 : userRoles.SubscriptionId.Value,
				UserId = userRoles.UserId
			};
		}

		/// <summary>
		/// Translates a <see cref="SubscriptionUserDBEntity"/> into a <see cref="SubscriptionUserInfo"/>"/>.
		/// </summary>
		/// <param name="subUser">SubscriptionUserDBEntity instance.</param>
		/// <returns>SubscriptionUserInfo instance.</returns>
		public static SubscriptionUserInfo InitializeSubscriptionUserInfo(SubscriptionUserDBEntity subUser)
		{
			if (subUser == null)
			{
				return null;
			}

			return new SubscriptionUserInfo
			{
				CreatedUTC = subUser.CreatedUTC,
				FirstName = subUser.FirstName,
				LastName = subUser.LastName,
				ProductRoleId = subUser.ProductRoleId,
				ProductRoleName = subUser.ProductRoleName,
				SkuId = subUser.SkuId,
				SubscriptionId = subUser.SubscriptionId,
				UserId = subUser.UserId
			};
		}

		#endregion Info-DBEntity Conversions
	}
}
