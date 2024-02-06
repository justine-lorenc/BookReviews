USE [BookReviews]
GO

CREATE TABLE [dbo].[Role] (
	[Id] SMALLINT NOT NULL,
	[Name] VARCHAR(50) NOT NULL
	CONSTRAINT PK_Role PRIMARY KEY ([Id])
);

CREATE TABLE [dbo].[UserRole] (
	[UserId] INT NOT NULL,
	[RoleId] SMALLINT NOT NULL
);

ALTER TABLE [dbo].[UserRole] ADD CONSTRAINT FK_UserRole_User FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]);
ALTER TABLE [dbo].[UserRole] ADD CONSTRAINT FK_UserRole_Role FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id]);

INSERT INTO [dbo].[Role] ([Id], [Name]) VALUES
(0, 'Admin'),
(1, 'Add Review'),
(2, 'Edit Review'),
(3, 'Delete Review');

GO
