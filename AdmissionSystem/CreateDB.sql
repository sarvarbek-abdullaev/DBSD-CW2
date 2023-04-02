USE [DBSD_CW];
GO
set language english;
GO

/*******************************************************************************
   Create Tables
********************************************************************************/
CREATE TABLE [dbo].[Teacher]
(
    [TeacherId] INT NOT NULL IDENTITY,
    [FirstName] NVARCHAR(40) NOT NULL,
    [LastName] NVARCHAR(20) NOT NULL,
    [BirthDate] DATETIME NOT NULL,
    [IsMarried] BIT,
    [Address] NVARCHAR(70),
    Salary INT,
    [Phone] NVARCHAR(24),
    [Email] NVARCHAR(60) NOT NULL,
    [Photo] VARBINARY(MAX), 
    CONSTRAINT [PK_Teacher] PRIMARY KEY CLUSTERED ([TeacherId])
);
GO

GO

CREATE TABLE [dbo].[Course]
(
    [CourseId] INT NOT NULL IDENTITY,
    [Name] NVARCHAR(20) NOT NULL,
    [Description] NVARCHAR(100) NOT NULL,
    [Photo] VARBINARY(MAX),
    CONSTRAINT [PK_Course] PRIMARY KEY CLUSTERED ([CourseId])
);

CREATE TABLE [dbo].[Class]
(
    [ClassId] INT NOT NULL IDENTITY,
    [Name] NVARCHAR(20) NOT NULL,
    [Description] NVARCHAR(100) NOT NULL,
    [Photo] VARBINARY(MAX),
    [CourseId] INT,
    [TeacherId] INT,

    CONSTRAINT [PK_Class] PRIMARY KEY CLUSTERED ([ClassId])
);

GO
CREATE TABLE [dbo].[Student]
(
    [StudentId] INT NOT NULL IDENTITY,
    [FirstName] NVARCHAR(40) NOT NULL,
    [LastName] NVARCHAR(20) NOT NULL,
    [BirthDate] DATETIME NOT NULL,
    [Address] NVARCHAR(70),
    [Phone] NVARCHAR(24),
    [Email] NVARCHAR(60) NOT NULL,
    [HasDebt] BIT,
    [Level] INT NOT NULL,
    [Photo] VARBINARY(MAX),
    [ClassId] INT,

    CONSTRAINT [PK_Student] PRIMARY KEY CLUSTERED ([StudentId])
);
GO


ALTER TABLE [dbo].[Class] ADD CONSTRAINT [FK_ClassTeacherId]
    FOREIGN KEY ([TeacherId]) REFERENCES [dbo].[Teacher] ([TeacherId]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[Class] ADD CONSTRAINT [FK_ClassCourseId]
    FOREIGN KEY ([CourseId]) REFERENCES [dbo].[Course] ([CourseId]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

ALTER TABLE [dbo].[Student] ADD CONSTRAINT [FK_StudentClassId]
    FOREIGN KEY ([ClassId]) REFERENCES [dbo].[Class] ([ClassId]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

