-- Organization roles
INSERT INTO [Auth].[OrgRole] (OrgRoleId, Name) VALUES (1, 'Member');
INSERT INTO [Auth].[OrgRole] (OrgRoleId, Name) VALUES (2, 'Owner');

-- Employee types
INSERT INTO [Auth].[EmployeeType] (EmployeeTypeId, Name) VALUES (1, 'Salaried'); 
INSERT INTO [Auth].[EmployeeType] (EmployeeTypeId, Name) VALUES (2, 'Hourly');

-- Product roles -- Time Tracker
INSERT INTO [Auth].[ProductRole] (ProductRoleId, ProductId, Name) VALUES (1, 200000, 'User');
INSERT INTO [Auth].[ProductRole] (ProductRoleId, ProductId, Name) VALUES (2, 200000, 'Manager');

-- Product roles -- Expense Tracker
INSERT INTO [Auth].[ProductRole] (ProductRoleId, ProductId, Name) VALUES (1, 300000, 'User');
INSERT INTO [Auth].[ProductRole] (ProductRoleId, ProductId, Name) VALUES (2, 300000, 'Manager');

-- Product roles -- Staffing Manager
INSERT INTO [Auth].[ProductRole] (ProductRoleId, ProductId, Name) VALUES (1, 400000, 'User');
INSERT INTO [Auth].[ProductRole] (ProductRoleId, ProductId, Name) VALUES (2, 400000, 'Manager');
