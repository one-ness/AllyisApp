using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.Services.Auth
{
	//Leaving commented I kinda liked this class.
    ///// <summary
    ///// User account service 
    /////// </summary>
    ////public class UserAccount
    //{
    //    public User userInfo { get; set; }

    //    public List<OrganizationAndRole> Organizations { get; set; }

    //    public List<Tuple<InvitationInfo, string>> InviatationInfoWithName { get; set; }

    //    /// <summary>
    //    /// Creats user Account from results from GetUser.sql 
    //    /// </summary>
    //    public UserAccount(
    //        dynamic UserInfo)
    //    {
    //        userInfo = AppService.InitializeUser(UserInfo.User);
    //        Organizations = new List<OrganizationAndRole>();
    //        foreach(dynamic org in UserInfo.Organizaions)
    //        {
    //            var orgInfo = new OrganizationAndRole(AppService.InitializeOrganization(org), (OrganizationRole)org.OrganizationRoleId);
				//IEnumerable<dynamic> subscriptions = UserInfo.Subscriptions;
    //            var orgSubs = subscriptions.Where(sub => sub.OrganizationId == org.OrganizationId);
                
    //            foreach (dynamic sub in orgSubs)
    //            {
    //                    orgInfo.OrganizationSubscriptions.Add(
    //                    new UserSubscription()
    //                    {
				//			Subscription = new Subscription()
				//			{ 
				//				AreaUrl = sub.AreaUrl,
				//				OrganizationId = sub.OrganizationId,
				//				ProductId = (ProductIdEnum)sub.ProductId,
				//				ProductName = sub.ProductName,
				//				SkuId = (SkuIdEnum)sub.SkuId,
				//				SubscriptionId = sub.SubscriptionId,
				//				SubscriptionName = sub.SubscriptionName
				//			},
    //                        ProductRoleId = sub.ProductRoleId,
    //                    });
    //            }
    //            Organizations.Add(orgInfo);
    //        }
            

    //        InviatationInfoWithName = new List<Tuple<InvitationInfo,string>>();
    //        foreach (dynamic invite in UserInfo.Invitations)
    //        {
    //            InviatationInfoWithName.Add(new Tuple<InvitationInfo, string>(
    //            new InvitationInfo() {
    //                CompressedEmail = AppService.GetCompressedEmail(invite.Email),
    //                DateOfBirth = invite.DateOfBirth,
    //                Email = invite.Email,
    //                EmployeeId = invite.EmployeeId,
    //                FirstName = invite.FirstName,
    //                LastName = invite.LastName,
    //                InvitationId = invite.InvitationId,
    //                OrganizationId = invite.OrganizationId,
    //                OrganizationRole = (OrganizationRole) invite.OrganizationRoleId,
    //            },
    //            invite.OrganizationName
    //            ));
    //        }
    //    }

    //    /// <summary>
    //    /// Get organizion and thier role in that organization;
    //    /// </summary>
    //    public class OrganizationAndRole
    //    {
    //        public Organization organization { get; set; }
    //        public OrganizationRole role { get; set; }
    //        public OrganizationAndRole(Organization org, OrganizationRole role)
    //        {
    //            organization = org;
    //            this.role = role;
    //            OrganizationSubscriptions = new List<UserSubscription>();
    //        }

    //        public List<UserSubscription> OrganizationSubscriptions;
    //    }

        
    //}
}
