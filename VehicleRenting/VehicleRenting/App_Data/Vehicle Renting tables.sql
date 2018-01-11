CREATE TABLE [dbo].[VehicleConditions]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Code] INT NOT NULL, 
    [Title] VARCHAR(128) NOT NULL, 
    CONSTRAINT [Unique_VehicleConditions_Code] UNIQUE ([Code])
);

CREATE TABLE [dbo].[Proprietors]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(128) NOT NULL
);

CREATE TABLE [dbo].[Vehicles]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [RegistrationNo] VARCHAR(128) NOT NULL, 
    [VehicleColor] VARCHAR(128) NULL, 
    [AvailableToDouble] BIT NOT NULL, 
    [RentPerWeek] DECIMAL(18, 2) NOT NULL, 
    [VehicleConditionId] INT NOT NULL, 
    [Active] BIT NOT NULL, 
    [AvailableDate] DATETIME NULL, 
    [UnderOffer] BIT NULL, 
    [ProprietorId] INT NOT NULL, 
    CONSTRAINT [Unique_Vehicles_Registration] UNIQUE ([RegistrationNo]), 
    CONSTRAINT [FK_Vehicles_VehicleCondition] FOREIGN KEY ([VehicleConditionId]) REFERENCES [VehicleConditions]([Id]), 
    CONSTRAINT [FK_Vehicles_Proprietor] FOREIGN KEY ([ProprietorId]) REFERENCES [Proprietor]([Id])
);

CREATE TABLE [dbo].[IssueTypes]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Code] INT NOT NULL, 
    [Title] VARCHAR(128) NOT NULL, 
    CONSTRAINT [Unique_IssueTypes_Code] UNIQUE ([Code])
);

CREATE TABLE [dbo].[Issues]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [IssueTypeId] INT NOT NULL, 
    [Subject] NVARCHAR(128) NOT NULL, 
    [Description] NVARCHAR(2048) NOT NULL, 
    CONSTRAINT [FK_Issues_IssueType] FOREIGN KEY ([IssueTypeId]) REFERENCES [IssueTypes]([id])
);

CREATE TABLE [dbo].[Notices]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [NoticeDate] DATETIME NOT NULL, 
    [CheckoutDate] DATETIME NOT NULL, 
    [Reason] NVARCHAR(2048) NOT NULL
);

CREATE TABLE [dbo].[Appointments]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(128) NOT NULL, 
    [Email] NVARCHAR(128) NOT NULL, 
    [Phone] NVARCHAR(128) NOT NULL, 
    [Date] DATETIME NOT NULL, 
    [Comments] NVARCHAR(1024) NOT NULL
);

CREATE TABLE [dbo].[Documents]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Code] INT NOT NULL, 
    [Title] NVARCHAR(128) NOT NULL, 
    [Path] NVARCHAR(512) NOT NULL, 
    CONSTRAINT [Unique_Documents_Code] UNIQUE ([Code])
);

CREATE TABLE [dbo].[Salutations]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Code] INT NOT NULL, 
    [Title] NVARCHAR(128) NOT NULL, 
    CONSTRAINT [Unique_Salutations_Code] UNIQUE ([Code])
);

CREATE TABLE [dbo].[SourceTypes]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Code] INT NOT NULL, 
    [Title] NVARCHAR(128) NOT NULL, 
    CONSTRAINT [AK_SourceTypes_Code] UNIQUE ([Code])
);

CREATE TABLE [dbo].[ReferenceTypes]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Code] INT NOT NULL, 
    [Title] NVARCHAR(128) NOT NULL, 
    CONSTRAINT [AK_ReferenceTypes_Code] UNIQUE ([Code])
);

CREATE TABLE [dbo].[IdentityTypes]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Code] INT NOT NULL, 
    [Title] NVARCHAR(128) NOT NULL, 
    CONSTRAINT [Unique_IdentityTypes_Code] UNIQUE ([Code])
);

CREATE TABLE [dbo].[Nationalities]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Code] INT NOT NULL, 
    [Title] NVARCHAR(128) NOT NULL, 
    CONSTRAINT [Unique_Nationalities_Code] UNIQUE ([Code])
);

CREATE TABLE [dbo].[Contracts]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [VehicleId] INT NOT NULL, 
    [StartDate] DATETIME NOT NULL, 
    [EndDate] DATETIME NULL, 
    [DocumentPath] VARCHAR(512) NOT NULL, 
    [Notes] VARCHAR(2048) NULL, 
    CONSTRAINT [FK_Contracts_Vehicle] FOREIGN KEY ([VehicleId]) REFERENCES [Vehicles]([Id])
);

CREATE TABLE [dbo].[Drivers]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [SalutationId] INT NOT NULL, 
    [FirstName] VARCHAR(128) NOT NULL, 
    [LastName] VARCHAR(128) NOT NULL, 
    [RentDueDate] INT NULL, 
    [RentDate] DATETIME NULL, 
    [ContractFrom] DATETIME NULL, 
    [ContractTo] DATETIME NULL, 
    [ContractLength] INT NULL, 
    [SourceId] INT NULL, 
    [UserId] NVARCHAR(128) NOT NULL, 
    [SecurityDepositAmount] DECIMAL(18, 2) NULL, 
    [AdvancedRentAmount] DECIMAL(18, 2) NULL, 
    [HoldingDepositAmount] DECIMAL(18, 2) NULL, 
    [AdminFee] DECIMAL(18, 2) NULL, 
    [ReferenceId] INT NULL, 
    [ReferenceDocumentPath] NVARCHAR(512) NULL, 
    [IdentityId] INT NULL, 
    [IdentityDocuementPath] NVARCHAR(512) NULL, 
    [NationalityId] INT NULL, 
    [SpecialConditions] NVARCHAR(2048) NULL, 
    CONSTRAINT [FK_Drivers_Salutation] FOREIGN KEY ([SalutationId]) REFERENCES [Salutations]([Id]), 
    CONSTRAINT [CK_Drivers_RentDueDate] CHECK (RentDueDate >= 1 AND RentDueDate <= 31), 
    CONSTRAINT [CK_Drivers_ContractLength] CHECK (ContractLength >= 1), 
    CONSTRAINT [FK_Drivers_Source] FOREIGN KEY ([SourceId]) REFERENCES [SourceTypes]([Id]), 
    CONSTRAINT [FK_Drivers_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers]([Id]), 
    CONSTRAINT [FK_Drivers_ReferenceType] FOREIGN KEY ([ReferenceId]) REFERENCES [ReferenceTypes]([Id]), 
    CONSTRAINT [FK_Drivers_Identity] FOREIGN KEY ([IdentityId]) REFERENCES [IdentityTypes]([Id]), 
    CONSTRAINT [FK_Drivers_Nationality] FOREIGN KEY ([NationalityId]) REFERENCES [Nationalities]([Id])
);


