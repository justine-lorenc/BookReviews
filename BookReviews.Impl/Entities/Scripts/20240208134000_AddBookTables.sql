USE [BookReviews]
GO

CREATE TABLE [dbo].[Book] (
	[Id] BIGINT NOT NULL,
	[Title] VARCHAR(250) NOT NULL,
	[SubTitle] VARCHAR(250) NULL,
	[Pages] SMALLINT NOT NULL,
	[DatePublished] DATETIME NOT NULL,
	[Description] VARCHAR(MAX) NULL,
	[CoverUrl] VARCHAR(1000) NULL
	CONSTRAINT PK_Book PRIMARY KEY ([Id])
);

CREATE TABLE [dbo].[Author] (
	[Id] INT NOT NULL IDENTITY,
	[Name] VARCHAR(200) NOT NULL
	CONSTRAINT PK_Author PRIMARY KEY ([Id])
);

CREATE TABLE [dbo].[BookAuthor] (
	[BookId] INT NOT NULL,
	[AuthorId] BIGINT NOT NULL
);

ALTER TABLE [dbo].[BookAuthor] ADD CONSTRAINT FK_BookAuthor_Book FOREIGN KEY ([BookId]) REFERENCES [dbo].[Book] ([Id]);
ALTER TABLE [dbo].[BookAuthor] ADD CONSTRAINT FK_BookAuthor_Author FOREIGN KEY ([AuthorId]) REFERENCES [dbo].[Author] ([Id]);

CREATE TYPE [dbo].[TVP_Author] AS TABLE
(
	[Id] INT NOT NULL,
	[Name] VARCHAR(200) NOT NULL
);

CREATE TYPE [dbo].[TVP_BookAuthor] AS TABLE
(
	[BookId] BIGINT NOT NULL,
	[AuthorId] INT NOT NULL
);

GO
