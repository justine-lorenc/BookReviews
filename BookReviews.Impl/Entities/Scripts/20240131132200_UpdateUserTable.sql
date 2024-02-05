USE [BookReviews]
GO

ALTER TABLE [dbo].[User] ADD [EmailAddress] VARCHAR(100) NOT NULL,
[PasswordHash] VARCHAR(128) NOT NULL,
[IsActive] BIT NOT NULL DEFAULT 0;

GO
