using Microsoft.EntityFrameworkCore.Migrations;

namespace nativoshortener.api.Migrations
{
    public partial class UpdateColumnSpecificationShortenedUrlsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                    BEGIN TRANSACTION
                    SET QUOTED_IDENTIFIER ON
                    SET ARITHABORT ON
                    SET NUMERIC_ROUNDABORT OFF
                    SET CONCAT_NULL_YIELDS_NULL ON
                    SET ANSI_NULLS ON
                    SET ANSI_PADDING ON
                    SET ANSI_WARNINGS ON
                    COMMIT
                    BEGIN TRANSACTION
                    GO
                    CREATE TABLE dbo.Tmp_ShortenedUrls
	                    (
	                    Id int NOT NULL,
	                    CreationDate datetime2(7) NOT NULL,
	                    URL nvarchar(2000) NOT NULL,
	                    ShortCode nvarchar(150) NOT NULL
	                    )  ON [PRIMARY]
                    GO
                    ALTER TABLE dbo.Tmp_ShortenedUrls SET (LOCK_ESCALATION = TABLE)
                    GO
                    IF EXISTS(SELECT * FROM dbo.ShortenedUrls)
	                     EXEC('INSERT INTO dbo.Tmp_ShortenedUrls (Id, CreationDate, URL, ShortCode)
		                    SELECT Id, CreationDate, URL, ShortCode FROM dbo.ShortenedUrls WITH (HOLDLOCK TABLOCKX)')
                    GO
                    ALTER TABLE dbo.Visits
	                    DROP CONSTRAINT FK_Visits_ShortenedUrls_ShortenedUrlId
                    GO
                    DROP TABLE dbo.ShortenedUrls
                    GO
                    EXECUTE sp_rename N'dbo.Tmp_ShortenedUrls', N'ShortenedUrls', 'OBJECT' 
                    GO
                    ALTER TABLE dbo.ShortenedUrls ADD CONSTRAINT
	                    PK_ShortenedUrls PRIMARY KEY CLUSTERED 
	                    (
	                    Id
	                    ) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

                    GO
                    COMMIT
                    BEGIN TRANSACTION
                    GO
                    ALTER TABLE dbo.Visits ADD CONSTRAINT
	                    FK_Visits_ShortenedUrls_ShortenedUrlId FOREIGN KEY
	                    (
	                    ShortenedUrlId
	                    ) REFERENCES dbo.ShortenedUrls
	                    (
	                    Id
	                    ) ON UPDATE  NO ACTION 
	                     ON DELETE  CASCADE 
	
                    GO
                    ALTER TABLE dbo.Visits SET (LOCK_ESCALATION = TABLE)
                    GO
                    COMMIT");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                    BEGIN TRANSACTION
                    SET QUOTED_IDENTIFIER ON
                    SET ARITHABORT ON
                    SET NUMERIC_ROUNDABORT OFF
                    SET CONCAT_NULL_YIELDS_NULL ON
                    SET ANSI_NULLS ON
                    SET ANSI_PADDING ON
                    SET ANSI_WARNINGS ON
                    COMMIT
                    BEGIN TRANSACTION
                    GO
                    CREATE TABLE dbo.Tmp_ShortenedUrls
	                    (
	                    Id int NOT NULL IDENTITY (1, 1),
	                    CreationDate datetime2(7) NOT NULL,
	                    URL nvarchar(MAX) NOT NULL,
	                    ShortCode nvarchar(MAX) NOT NULL
	                    )  ON [PRIMARY]
                    GO
                    ALTER TABLE dbo.Tmp_ShortenedUrls SET (LOCK_ESCALATION = TABLE)
                    GO
                    SET IDENTITY_INSERT dbo.Tmp_ShortenedUrls ON
                    GO
                    IF EXISTS(SELECT * FROM dbo.ShortenedUrls)
	                     EXEC('INSERT INTO dbo.Tmp_ShortenedUrls (Id, CreationDate, URL, ShortCode)
		                    SELECT Id, CreationDate, URL, ShortCode FROM dbo.ShortenedUrls WITH (HOLDLOCK TABLOCKX)')
                    GO
                    SET IDENTITY_INSERT dbo.Tmp_ShortenedUrls OFF
                    GO
                    DROP TABLE dbo.ShortenedUrls
                    GO
                    EXECUTE sp_rename N'dbo.Tmp_ShortenedUrls', N'ShortenedUrls', 'OBJECT' 
                    GO
                    ALTER TABLE dbo.ShortenedUrls ADD CONSTRAINT
	                    PK_ShortenedUrls PRIMARY KEY CLUSTERED 
	                    (
	                    Id
	                    ) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

                    GO
                    COMMIT");
        }
    }
}
