﻿INSERT INTO [Auth].[Organization] (OrganizationId, Name, CreatedUtc, ModifiedUtc, IsActive) VALUES (112559, 'Test Organization', '7/13/2017 6:03:40 PM', '7/13/2017 6:03:40 PM', 1);

INSERT INTO [Auth].[OrganizationUser] (UserId, OrganizationId, EmployeeId, [OrganizationRoleId], EmployeeTypeId, CreatedUtc, ModifiedUtc) VALUES (111119, 112559, 'qwe', 2, 1, '7/13/2017 6:03:40 PM', '7/13/2017 6:03:40 PM');
INSERT INTO [Auth].[OrganizationUser] (UserId, OrganizationId, EmployeeId, [OrganizationRoleId], EmployeeTypeId, CreatedUtc, ModifiedUtc) VALUES (111122, 112559, 'qwert', 1, 2, '7/13/2017 6:03:40 PM', '7/13/2017 6:03:40 PM');

INSERT INTO [Auth].[User] (UserId, FirstName, LastName, Email, PasswordHash, EmailConfirmed, PhoneNumberConfirmed, TwoFactorEnabled, AccessFailedCount, LockoutEnabled, CreatedUtc, ModifiedUtc, DateOfBirth, EmailConfirmationCode, LanguagePreference) VALUES (111122, 'Bob', 'Tester', 'asd@asd.asd', '20000:n+D9dpuLRkhKx1YXf6vFQVaigbCdS3+T:5meLnihk8hWEKn0FQic//lGcLyqTyTmY1+9DJYcJV2M=', 1, 1, 0, 0, 0, '7/13/2017 6:22:59 PM', '7/13/2017 6:22:59 PM', '7/13/1999 12:00:00 AM', '57a94b45-27ce-4879-b16c-1b164b8067fc', 1);
INSERT INTO [Auth].[User] (UserId, FirstName, LastName, Email, PasswordHash, EmailConfirmed, PhoneNumberConfirmed, TwoFactorEnabled, AccessFailedCount, LockoutEnabled, CreatedUtc, ModifiedUtc, DateOfBirth, ActiveOrganizationId, EmailConfirmationCode, LanguagePreference) VALUES (111119, 'Luke', 'Kook', 'test@allyis.com', '20000:Et1PVCUpAfLBi+0lyqzsCmhHR7DvMLgf:y30SZkNCtm6RpOJs24blDObcxD8jLTAMowiIoBfsDS4=', 1, 1, 0, 0, 0, '7/13/2017 5:48:50 PM', '7/13/2017 6:03:40 PM', '7/13/1999 12:00:00 AM', 112559, 'bfff06e9-c1b1-4a1b-8a4d-06443d75b97d', 1);
