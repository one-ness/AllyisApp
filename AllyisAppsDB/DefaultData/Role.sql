-- Organization roles
INSERT INTO [Auth].[OrganizationRole] (OrganizationRoleId, Name) VALUES (1, 'Member');
INSERT INTO [Auth].[OrganizationRole] (OrganizationRoleId, Name) VALUES (2, 'Owner');


-- Product roles -- Time Tracker
INSERT INTO [Auth].[ProductRole] (ProductRoleId, ProductId, Name) VALUES (1, 200000, 'User');
INSERT INTO [Auth].[ProductRole] (ProductRoleId, ProductId, Name) VALUES (2, 200000, 'Manager');

-- Product roles -- Expense Tracker
INSERT INTO [Auth].[ProductRole] (ProductRoleId, ProductId, Name) VALUES (1, 300000, 'User');
INSERT INTO [Auth].[ProductRole] (ProductRoleId, ProductId, Name) VALUES (2, 300000, 'Manager');

-- Product roles -- Staffing Manager
INSERT INTO [Auth].[ProductRole] (ProductRoleId, ProductId, Name) VALUES (1, 400000, 'User');
INSERT INTO [Auth].[ProductRole] (ProductRoleId, ProductId, Name) VALUES (2, 400000, 'Manager');
