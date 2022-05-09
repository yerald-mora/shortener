using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;

namespace nativoshortener.api.Migrations
{
    public partial class SeedUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var hasher = new PasswordHasher<IdentityUser>();
            IdentityUser user = new IdentityUser();
            var pass = hasher.HashPassword(user, "Nativos user testing password #1");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "UserName","NormalizedUserName" ,"Email","NormalizedEmail", "PasswordHash","SecurityStamp", "ConcurrencyStamp","EmailConfirmed", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnabled", "AccessFailedCount" },
                values: new object[] { user.Id, "nativo", "NATIVO","nativo@nativo.la", "NATIVO@NATIVO.LA", pass, "5CBF8FA280C64142A1E2F97AA05568E4", "72ec0482-21d3-4e8b-9298-767bcbe21695", true, true, false, false, 0 },
                schema: null);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE AspNetUsers WHERE UserName = 'nativo' and Email = 'nativo@nativo.la'");
        }
    }
}
