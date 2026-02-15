using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MindShield.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RealityProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentJobTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Employer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomeLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrustedGuardianEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsGuardianActive = table.Column<bool>(type: "bit", nullable: false),
                    LinkedInPasswordEncrypted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealityProfiles", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "RealityProfiles",
                columns: new[] { "Id", "CreatedAt", "CurrentJobTitle", "Employer", "FullName", "HomeLocation", "IsGuardianActive", "LinkedInPasswordEncrypted", "TrustedGuardianEmail", "UserId" },
                values: new object[] { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Senior Project Manager", "TechFlow Solutions", "Alex Rivera", "San Jose, CA", true, null, null, "user_01" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RealityProfiles");
        }
    }
}
