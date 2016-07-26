-- Organization roles
INSERT INTO [Auth].[OrgRole] (OrgRoleId, Name) VALUES (1, 'Member');
INSERT INTO [Auth].[OrgRole] (OrgRoleId, Name) VALUES (2, 'Owner');

-- Product roles -- Time Tracker
INSERT INTO [Auth].[ProductRole] (ProductRoleId, ProductId, Name) VALUES (1, 1, 'User');
INSERT INTO [Auth].[ProductRole] (ProductRoleId, ProductId, Name, PermissionAdmin) VALUES (2, 1, 'Manager', 1);
INSERT INTO [Auth].[ProductRole] (ProductRoleId, ProductId, Name, PermissionAdmin) VALUES (3, 1, 'Admin', 1);

-- Product roles -- Consulting
INSERT INTO [Auth].[ProductRole] (ProductRoleId, ProductId, Name) VALUES (4, 2, 'User');
INSERT INTO [Auth].[ProductRole] (ProductRoleId, ProductId, Name, PermissionAdmin) VALUES (5, 2, 'Manager', 1);
INSERT INTO [Auth].[ProductRole] (ProductRoleId, ProductId, Name, PermissionAdmin) VALUES (6, 2, 'Admin', 1);
