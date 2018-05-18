﻿using AllyisApps.DBModel.Auth;
using AllyisApps.Lib;
using AllyisApps.Services.Auth;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllyisApps.Services
{
	public partial class AppService : BaseService
	{
		public async Task<UserOld> AddUserToOrganizaion(string email, string firstName, string lastName, int organizaionId, OrganizationRoleEnum roleType, string empolyeeId, int? employeeTypeId)
		{
			UserOld user = await GetUserByEmailAsync(email);
			int employeeType = 0;

			if (employeeTypeId == null)
			{
				var orgEmployeeTypes = (await GetEmployeeTypeByOrganization(organizaionId)).FirstOrDefault();
				employeeType = orgEmployeeTypes.EmployeeTypeId;
			}
			else
			{
				employeeType = employeeTypeId.Value;
			}

			int userId = 0;
			if (user == null)
			{
				//User is completly new does not exist in any fashion
				DateTime now = DateTime.Now;
				DateTime years18 = now.AddYears(-18);

				int userid = await DBHelper.CreateUserAsync(email, Crypto.GetPasswordHash("Welcome1"), firstName, lastName, Guid.NewGuid(), years18, null, null, null, (int)LoginProviderEnum.AllyisApps);
				user = await GetUserByEmailAsync(email);
			}

			OrganizationUserDBEntity orgUser = new OrganizationUserDBEntity()
			{
				CreatedUtc = DateTime.Now,
				Email = email,
				EmployeeTypeId = employeeType,
				EmployeeId = empolyeeId,
				FirstName = firstName,
				LastName = lastName,
				MaxAmount = 0,
				OrganizationId = organizaionId,
				OrganizationRoleId = (int)roleType,
				UserId = userId
			};

			DBHelper.CreateOrganizationUser(orgUser);
			return user;
		}
	}
}
