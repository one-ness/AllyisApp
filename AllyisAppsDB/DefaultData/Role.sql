-- Organization roles
INSERT INTO [Auth].[OrganizationRole] (OrganizationRoleId, [OrganizationRoleName]) VALUES (1, 'Member');
INSERT INTO [Auth].[OrganizationRole] (OrganizationRoleId, [OrganizationRoleName]) VALUES (2, 'Owner');

-- Product roles aka Organization roles -- AllyisApps
INSERT INTO [Auth].[ProductRole] (ProductRoleId, ProductId, [ProductRoleName]) VALUES (1, 100000, 'Member');
INSERT INTO [Auth].[ProductRole] (ProductRoleId, ProductId, [ProductRoleName]) VALUES (2, 100000, 'Owner');

-- Product roles -- Time Tracker
INSERT INTO [Auth].[ProductRole] (ProductRoleId, ProductId, [ProductRoleName]) VALUES (1, 200000, 'User');
INSERT INTO [Auth].[ProductRole] (ProductRoleId, ProductId, [ProductRoleName]) VALUES (2, 200000, 'Manager');

-- Product roles -- Expense Tracker
INSERT INTO [Auth].[ProductRole] (ProductRoleId, ProductId, [ProductRoleName]) VALUES (1, 300000, 'User');
INSERT INTO [Auth].[ProductRole] (ProductRoleId, ProductId, [ProductRoleName]) VALUES (2, 300000, 'Manager');
INSERT INTO [Auth].[ProductRole] (ProductRoleId, ProductId, [ProductRoleName]) VALUES (3, 300000, 'Admin');

-- Product roles -- Staffing Manager
INSERT INTO [Auth].[ProductRole] (ProductRoleId, ProductId, [ProductRoleName]) VALUES (1, 400000, 'User');
INSERT INTO [Auth].[ProductRole] (ProductRoleId, ProductId, [ProductRoleName]) VALUES (2, 400000, 'Manager');
