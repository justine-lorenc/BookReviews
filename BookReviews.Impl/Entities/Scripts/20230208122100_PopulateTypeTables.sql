USE [BookReviews];
GO

INSERT INTO [dbo].[BookFormat] ([Id], [Name]) VALUES 
(1, 'Paper'),
(2, 'Ebook'),
(3, 'Audiobook');

INSERT INTO [dbo].[Genre] ([Name], [IsFiction]) VALUES
('Classic', 1),
('Contemporary', 1),
('Drama', 1),
('Fantasy', 1),
('Historical Fiction', 1),
('Horror', 1),
('Mystery', 1),
('Romance', 1),
('Science Fiction', 1),
('Thriller', 1),
('Western', 1),
('History', 0),
('Politics', 0),
('Science', 0),
('Self-Help', 0),
('Sociology', 0);

GO