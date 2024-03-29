/****** Object:  StoredProcedure [dbo].[getSignatures]    Script Date: 6/21/2015 4:12:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[getSignatures]
		
AS

BEGIN

	SELECT * FROM [Signatures] ORDER BY [SignID] DESC
		
END


GO
/****** Object:  StoredProcedure [dbo].[newSignature]    Script Date: 6/21/2015 4:12:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[newSignature]

	@SiteID INT,
	@PetID INT,
	@Email VARCHAR(255),
	@FirstName NVARCHAR(50),
	@LastName NVARCHAR(50),
	@Address NVARCHAR(255),
	@City NVARCHAR(50),
	@State NVARCHAR(50),
	@ZipCode NVARCHAR(10),
	@Country NVARCHAR(50),
	@AgeGroup TINYINT,
	@Gender TINYINT,
	@PoliticalId TINYINT,
	@OptIn BIT,

	@SignID INT = NULL OUTPUT
		
AS

BEGIN

	INSERT INTO
		[Signatures]
	
	(
		[SiteID],
		[PetID],
		[Email],
		[FirstName],
		[LastName],
		[Address],
		[City],
		[State],
		[ZipCode],
		[Country],
		[AgeGroup],
		[Gender],
		[PoliticalId],
		[OptIn]
	)	
	
	VALUES
	
	(
		@SiteID,
		@PetID,
		LOWER(@Email),
		UPPER(@FirstName),
		UPPER(@LastName),
		UPPER(@Address),
		UPPER(@City),
		UPPER(@State),
		UPPER(@ZipCode),
		UPPER(@Country),
		@AgeGroup,
		@Gender,
		@PoliticalId,
		@OptIn
	)

	SELECT @SignID = SCOPE_IDENTITY()
		
END


GO

/****** Object:  Table [dbo].[Signatures]    Script Date: 6/21/2015 4:12:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Signatures](
	[SignID] [int] IDENTITY(1,1) NOT NULL,
	[SignDate] [smalldatetime] NOT NULL,
	[SiteID] [int] NOT NULL,
	[PetID] [int] NOT NULL,
	[Email] [varchar](255) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](255) NULL,
	[City] [nvarchar](50) NULL,
	[State] [nvarchar](50) NULL,
	[ZipCode] [nvarchar](10) NULL,
	[Country] [nvarchar](50) NULL,
	[AgeGroup] [tinyint] NULL,
	[Gender] [tinyint] NULL,
	[PoliticalId] [tinyint] NULL,
	[OptIn] [bit] NULL,
 CONSTRAINT [PK_Signatures] PRIMARY KEY CLUSTERED 
(
	[SignID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Signatures] ADD  CONSTRAINT [DF_Signatures_SignDate]  DEFAULT (sysdatetime()) FOR [SignDate]
GO
