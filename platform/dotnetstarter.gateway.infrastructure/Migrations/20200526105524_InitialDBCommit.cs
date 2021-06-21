using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace dotnetstarter.gateway.infrastructure.Migrations
{
    public partial class InitialDBCommit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateSequence(
                name: "buyerseq",
                schema: "dbo",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true),
                    JWTId = table.Column<string>(maxLength: 200, nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    MobileNumber = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    Email = table.Column<string>(nullable: true),
                    Verified = table.Column<bool>(nullable: false),
                    IdentityNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "dbo",
                table: "Users",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Users_JWTId",
                schema: "dbo",
                table: "Users",
                column: "JWTId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users",
                schema: "dbo");

            migrationBuilder.DropSequence(
                name: "buyerseq",
                schema: "dbo");
        }
    }
}
