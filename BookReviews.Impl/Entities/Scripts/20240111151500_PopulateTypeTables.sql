USE [BookReviews];
GO

INSERT INTO [dbo].[BookFormat] ([Id], [Name]) VALUES 
(1, 'Physical'),
(2, 'Ebook'),
(3, 'Audiobook');

INSERT INTO [dbo].[Genre] ([Name], [IsFiction]) VALUES
('Classic', 1),
('Contemporary', 1),
('Fantasy', 1),
('Science Fiction', 1),
('Historical Fiction', 1),
('Romance', 1),
('Mystery', 1),
('Horror', 1),
('Thriller', 1),
('Western', 1),
('Drama', 1),
('Poetry', 1),
('Essays', 0),
('Biography', 0),
('Self-Help', 0),
('History', 0),
('Science', 0),
('Politics', 0),
('Economics', 0),
('Sociology', 0);

GO