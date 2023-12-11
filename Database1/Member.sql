CREATE TABLE [dbo].[Member]
(
	[Id] INT NOT NULL PRIMARY KEY,
    Name NVARCHAR(100),
    FirstName NVARCHAR(100),
    Age INT,
    LicenseId INT NULL,
    Solde DECIMAL(10,2),
)