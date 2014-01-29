-- Script Date: 1/30/2014 1:26 AM  - ErikEJ.SqlCeScripting version 3.5.2.35
-- Database information:
-- Locale Identifier: 1033
-- Encryption Mode: 
-- Case Sensitive: False
-- Database: C:\Users\johnson\Documents\Projects\Orchard\Orchard.Platform\src\Orchard.Web\App_Data\Sites\Default\Orchard.sdf
-- ServerVersion: 4.0.8876.1
-- DatabaseSize: 320 KB
-- SpaceAvailable: 3.999 GB
-- Created: 1/30/2014 1:17 AM

-- User Table information:
-- Number of tables: 14
-- Orchard_Framework_ContentItemRecord: 0 row(s)
-- Orchard_Framework_ContentItemVersionRecord: 0 row(s)
-- Orchard_Framework_ContentTypeRecord: 0 row(s)
-- Orchard_Roles_PermissionRecord: 0 row(s)
-- Orchard_Roles_RoleRecord: 0 row(s)
-- Orchard_Roles_RolesPermissionsRecord: 0 row(s)
-- Orchard_Roles_UserRolesPartRecord: 0 row(s)
-- Orchard_Users_RegistrationSettingsPartRecord: 0 row(s)
-- Orchard_Users_UserPartRecord: 0 row(s)
-- Settings_ContentFieldRecord: 0 row(s)
-- Settings_ContentPartDefinitionRecord: 0 row(s)
-- Settings_ContentPartFieldDefinitionRecord: 0 row(s)
-- Settings_ContentTypeDefinitionRecord: 2 row(s)
-- Settings_ContentTypePartDefinitionRecord: 0 row(s)

CREATE TABLE [Settings_ContentTypePartDefinitionRecord] (
  [Id] int IDENTITY (1,1) NOT NULL
, [Settings] ntext NULL
, [ContentPartDefinitionRecord_id] int NULL
, [ContentTypeDefinitionRecord_Id] int NULL
);
GO
CREATE TABLE [Settings_ContentTypeDefinitionRecord] (
  [Id] int IDENTITY (3,1) NOT NULL
, [Name] nvarchar(255) NULL
, [DisplayName] nvarchar(255) NULL
, [Hidden] bit NULL
, [Settings] ntext NULL
);
GO
CREATE TABLE [Settings_ContentPartFieldDefinitionRecord] (
  [Id] int IDENTITY (1,1) NOT NULL
, [Name] nvarchar(255) NULL
, [Settings] ntext NULL
, [ContentFieldRecord_id] int NULL
, [ContentPartDefinitionRecord_Id] int NULL
);
GO
CREATE TABLE [Settings_ContentPartDefinitionRecord] (
  [Id] int IDENTITY (1,1) NOT NULL
, [Name] nvarchar(255) NULL
, [Hidden] bit NULL
, [Settings] ntext NULL
);
GO
CREATE TABLE [Settings_ContentFieldRecord] (
  [Id] int IDENTITY (1,1) NOT NULL
, [Name] nvarchar(255) NULL
);
GO
CREATE TABLE [Orchard_Users_UserPartRecord] (
  [Id] int NOT NULL
, [UserName] nvarchar(255) NULL
, [Email] nvarchar(255) NULL
, [NormalizedUserName] nvarchar(255) NULL
, [Password] nvarchar(255) NULL
, [PasswordFormat] nvarchar(255) NULL
, [HashAlgorithm] nvarchar(255) NULL
, [PasswordSalt] nvarchar(255) NULL
, [RegistrationStatus] nvarchar(255) DEFAULT 'Approved' NULL
, [EmailStatus] nvarchar(255) DEFAULT 'Approved' NULL
, [EmailChallengeToken] nvarchar(255) NULL
);
GO
CREATE TABLE [Orchard_Users_RegistrationSettingsPartRecord] (
  [Id] int NOT NULL
, [UsersCanRegister] bit DEFAULT 0 NULL
, [UsersMustValidateEmail] bit DEFAULT 0 NULL
, [ValidateEmailRegisteredWebsite] nvarchar(255) NULL
, [ValidateEmailContactEMail] nvarchar(255) NULL
, [UsersAreModerated] bit DEFAULT 0 NULL
, [NotifyModeration] bit DEFAULT 0 NULL
, [NotificationsRecipients] ntext NULL
, [EnableLostPassword] bit DEFAULT 0 NULL
);
GO
CREATE TABLE [Orchard_Roles_UserRolesPartRecord] (
  [Id] int IDENTITY (1,1) NOT NULL
, [UserId] int NULL
, [Role_id] int NULL
);
GO
CREATE TABLE [Orchard_Roles_RolesPermissionsRecord] (
  [Id] int IDENTITY (1,1) NOT NULL
, [Role_id] int NULL
, [Permission_id] int NULL
, [RoleRecord_Id] int NULL
);
GO
CREATE TABLE [Orchard_Roles_RoleRecord] (
  [Id] int IDENTITY (1,1) NOT NULL
, [Name] nvarchar(255) NULL
);
GO
CREATE TABLE [Orchard_Roles_PermissionRecord] (
  [Id] int IDENTITY (1,1) NOT NULL
, [Name] nvarchar(255) NULL
, [FeatureName] nvarchar(255) NULL
, [Description] nvarchar(255) NULL
);
GO
CREATE TABLE [Orchard_Framework_ContentTypeRecord] (
  [Id] int IDENTITY (1,1) NOT NULL
, [Name] nvarchar(255) NULL
);
GO
CREATE TABLE [Orchard_Framework_ContentItemVersionRecord] (
  [Id] int IDENTITY (1,1) NOT NULL
, [VersionNumber] int NULL
, [Published] bit NULL
, [Latest] bit NULL
, [Data] ntext NULL
, [ContentItemRecord_id] int NOT NULL
);
GO
CREATE TABLE [Orchard_Framework_ContentItemRecord] (
  [Id] int IDENTITY (1,1) NOT NULL
, [Data] ntext NULL
, [ContentType_id] int NULL
);
GO
SET IDENTITY_INSERT [Settings_ContentTypePartDefinitionRecord] OFF;
GO
SET IDENTITY_INSERT [Settings_ContentTypeDefinitionRecord] ON;
GO
INSERT INTO [Settings_ContentTypeDefinitionRecord] ([Id],[Name],[DisplayName],[Hidden],[Settings]) VALUES (1,N'Site',N'Site',0,N'<settings />');
GO
INSERT INTO [Settings_ContentTypeDefinitionRecord] ([Id],[Name],[DisplayName],[Hidden],[Settings]) VALUES (2,N'User',N'User',0,N'<settings ContentTypeSettings.Creatable="False" />');
GO
SET IDENTITY_INSERT [Settings_ContentTypeDefinitionRecord] OFF;
GO
SET IDENTITY_INSERT [Settings_ContentPartFieldDefinitionRecord] OFF;
GO
SET IDENTITY_INSERT [Settings_ContentPartDefinitionRecord] OFF;
GO
SET IDENTITY_INSERT [Settings_ContentFieldRecord] OFF;
GO
SET IDENTITY_INSERT [Orchard_Roles_UserRolesPartRecord] OFF;
GO
SET IDENTITY_INSERT [Orchard_Roles_RolesPermissionsRecord] OFF;
GO
SET IDENTITY_INSERT [Orchard_Roles_RoleRecord] OFF;
GO
SET IDENTITY_INSERT [Orchard_Roles_PermissionRecord] OFF;
GO
SET IDENTITY_INSERT [Orchard_Framework_ContentTypeRecord] OFF;
GO
SET IDENTITY_INSERT [Orchard_Framework_ContentItemVersionRecord] OFF;
GO
SET IDENTITY_INSERT [Orchard_Framework_ContentItemRecord] OFF;
GO
ALTER TABLE [Settings_ContentTypePartDefinitionRecord] ADD CONSTRAINT [PK__Settings_ContentTypePartDefinitionRecord__0000000000000042] PRIMARY KEY ([Id]);
GO
ALTER TABLE [Settings_ContentTypeDefinitionRecord] ADD CONSTRAINT [PK__Settings_ContentTypeDefinitionRecord__0000000000000034] PRIMARY KEY ([Id]);
GO
ALTER TABLE [Settings_ContentPartFieldDefinitionRecord] ADD CONSTRAINT [PK__Settings_ContentPartFieldDefinitionRecord__0000000000000024] PRIMARY KEY ([Id]);
GO
ALTER TABLE [Settings_ContentPartDefinitionRecord] ADD CONSTRAINT [PK__Settings_ContentPartDefinitionRecord__0000000000000014] PRIMARY KEY ([Id]);
GO
ALTER TABLE [Settings_ContentFieldRecord] ADD CONSTRAINT [PK__Settings_ContentFieldRecord__0000000000000006] PRIMARY KEY ([Id]);
GO
ALTER TABLE [Orchard_Users_UserPartRecord] ADD CONSTRAINT [PK__Orchard_Users_UserPartRecord__00000000000000CB] PRIMARY KEY ([Id]);
GO
ALTER TABLE [Orchard_Users_RegistrationSettingsPartRecord] ADD CONSTRAINT [PK__Orchard_Users_RegistrationSettingsPartRecord__00000000000000F2] PRIMARY KEY ([Id]);
GO
ALTER TABLE [Orchard_Roles_UserRolesPartRecord] ADD CONSTRAINT [PK__Orchard_Roles_UserRolesPartRecord__0000000000000124] PRIMARY KEY ([Id]);
GO
ALTER TABLE [Orchard_Roles_RolesPermissionsRecord] ADD CONSTRAINT [PK__Orchard_Roles_RolesPermissionsRecord__0000000000000118] PRIMARY KEY ([Id]);
GO
ALTER TABLE [Orchard_Roles_RoleRecord] ADD CONSTRAINT [PK__Orchard_Roles_RoleRecord__000000000000010A] PRIMARY KEY ([Id]);
GO
ALTER TABLE [Orchard_Roles_PermissionRecord] ADD CONSTRAINT [PK__Orchard_Roles_PermissionRecord__0000000000000100] PRIMARY KEY ([Id]);
GO
ALTER TABLE [Orchard_Framework_ContentTypeRecord] ADD CONSTRAINT [PK__Orchard_Framework_ContentTypeRecord__000000000000014C] PRIMARY KEY ([Id]);
GO
ALTER TABLE [Orchard_Framework_ContentItemVersionRecord] ADD CONSTRAINT [PK__Orchard_Framework_ContentItemVersionRecord__0000000000000142] PRIMARY KEY ([Id]);
GO
ALTER TABLE [Orchard_Framework_ContentItemRecord] ADD CONSTRAINT [PK__Orchard_Framework_ContentItemRecord__0000000000000130] PRIMARY KEY ([Id]);
GO
CREATE INDEX [IDX_ContentItemRecord_id] ON [Orchard_Framework_ContentItemVersionRecord] ([ContentItemRecord_id] ASC);
GO
CREATE INDEX [IDX_ContentType_id] ON [Orchard_Framework_ContentItemRecord] ([ContentType_id] ASC);
GO

