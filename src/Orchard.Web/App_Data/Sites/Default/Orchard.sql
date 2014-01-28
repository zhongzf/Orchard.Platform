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

