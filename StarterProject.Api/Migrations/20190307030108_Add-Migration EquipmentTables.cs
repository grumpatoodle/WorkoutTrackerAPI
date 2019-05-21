using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StarterProject.Api.Migrations
{
    public partial class AddMigrationEquipmentTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equipments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipments", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 150, 77, 222, 115, 155, 204, 253, 154, 126, 78, 139, 248, 67, 150, 32, 25, 191, 111, 86, 59 }, new byte[] { 111, 18, 212, 114, 88, 25, 193, 171, 132, 59, 100, 145, 181, 226, 75, 76 } });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Equipments");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 75, 17, 111, 11, 95, 102, 220, 35, 129, 238, 128, 87, 5, 70, 216, 64, 103, 44, 216, 173 }, new byte[] { 36, 144, 75, 66, 140, 150, 73, 186, 168, 239, 41, 141, 181, 2, 5, 99 } });
        }
    }
}
