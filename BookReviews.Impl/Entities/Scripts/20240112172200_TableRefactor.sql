USE [BookReviews]
GO

DROP TABLE [dbo].[Review];
DROP TABLE [dbo].[Book];

CREATE TABLE [dbo].[User] (
    [Id] INT NOT NULL IDENTITY,
    [FirstName] VARCHAR(50) NOT NULL,
    [LastName] VARCHAR(50) NOT NULL,
    [DateAdded] DATETIME NOT NULL,
    [DateUpdated] DATETIME NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY ([Id])
);

CREATE TABLE [dbo].[Review] (
    [Id] INT NOT NULL IDENTITY,
    [Rating] FLOAT(2) NOT NULL,
    [Notes] VARCHAR(1000) NULL,
	[DateAdded] DATETIME NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[BookId] BIGINT NOT NULL,
    [UserId] INT NOT NULL,
    [GenreId] SMALLINT NOT NULL,
    [BookFormatId] SMALLINT NOT NULL
    CONSTRAINT [PK_Review] PRIMARY KEY ([Id])
);

ALTER TABLE [dbo].[Review] ADD CONSTRAINT [FK_Review_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]);
ALTER TABLE [dbo].[Review] ADD CONSTRAINT [FK_Review_GenreId] FOREIGN KEY ([GenreId]) REFERENCES [dbo].[Genre] ([Id]);
ALTER TABLE [dbo].[Review] ADD CONSTRAINT [FK_Review_BookFormatId] FOREIGN KEY ([BookFormatId]) REFERENCES [dbo].[BookFormat] ([Id]);

GO
