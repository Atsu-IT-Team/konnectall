IF NOT EXISTS(SELECT 1 FROM SYS.COLUMNS WHERE Name = 'ShowAsBanner' AND Object_ID = Object_ID('Category'))
BEGIN
    ALTER TABLE [Category] 
	ADD ShowAsBanner bit NOT NULL
	CONSTRAINT ShowAsBanner_Default_Standard DEFAULT 0
	WITH VALUES
END

IF NOT EXISTS(SELECT 1 FROM SYS.COLUMNS WHERE Name = 'IconId' AND Object_ID = Object_ID('Category'))
BEGIN
    ALTER TABLE [Category] 
	ADD IconId INT NOT NULL
	CONSTRAINT IconId_Default_Standard DEFAULT 0
	WITH VALUES
END

IF NOT EXISTS(SELECT 1 FROM SYS.COLUMNS WHERE Name = 'BannerId' AND Object_ID = Object_ID('Category'))
BEGIN
    ALTER TABLE [Category] 
	ADD BannerId INT NOT NULL
	CONSTRAINT BannerId_Default_Standard DEFAULT 0
	WITH VALUES
END

IF NOT EXISTS(SELECT 1 FROM SYS.COLUMNS WHERE Name = 'IsMostPopular' AND Object_ID = Object_ID('Category'))
BEGIN
    ALTER TABLE [Category] 
	ADD IsMostPopular BIT NOT NULL
	CONSTRAINT IsMostPopular_Default_Standard DEFAULT 0
	WITH VALUES
END

--Changes on 01-11-2023

GO
/****** Object:  Table [dbo].[KA_ApplicationDocuments]    Script Date: 01-11-2023 13:04:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KA_ApplicationDocuments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationId] [int] NULL,
	[DocumentName] [nvarchar](max) NULL,
 CONSTRAINT [PK_KA_ApplicationDocuments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KA_ApplicationRequest]    Script Date: 01-11-2023 13:04:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KA_ApplicationRequest](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](200) NOT NULL,
	[LastName] [nvarchar](200) NOT NULL,
	[Email] [nvarchar](300) NOT NULL,
	[Phone] [nvarchar](50) NOT NULL,
	[Resume] [nvarchar](200) NULL,
	[CreatedOnUtc] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_KA_ApplicationRequest] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[KA_ApplicationDocuments]  WITH CHECK ADD  CONSTRAINT [FK_KA_ApplicationDocuments_ApplicationId_KA_ApplicationRequest_Id] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[KA_ApplicationRequest] ([Id])
GO
ALTER TABLE [dbo].[KA_ApplicationDocuments] CHECK CONSTRAINT [FK_KA_ApplicationDocuments_ApplicationId_KA_ApplicationRequest_Id]
GO
--Changes on 01-11-2023