using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StarterProject.Api.Migrations
{
    public partial class AddRoutinesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Routines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routines", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 16, 7, 228, 165, 165, 235, 116, 129, 13, 58, 215, 149, 102, 180, 234, 150, 97, 117, 72, 54 }, new byte[] { 72, 27, 226, 167, 209, 167, 183, 165, 43, 25, 243, 13, 178, 170, 239, 193 } });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Routines");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 75, 17, 111, 11, 95, 102, 220, 35, 129, 238, 128, 87, 5, 70, 216, 64, 103, 44, 216, 173 }, new byte[] { 36, 144, 75, 66, 140, 150, 73, 186, 168, 239, 41, 141, 181, 2, 5, 99 } });
        }
    }
}
