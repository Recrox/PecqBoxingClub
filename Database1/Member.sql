CREATE TABLE [dbo].[Member]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100),
    FirstName NVARCHAR(100),
    Age INT,
    LicenseId INT NULL,
    Solde DECIMAL(10,2),
)

ALTER TABLE dbo.Member
ADD CONSTRAINT FK_Member_License FOREIGN KEY (LicenseId)
REFERENCES dbo.License(Id);
