using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StarterProject.Api.Migrations
{
    public partial class AddEquipmentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 242, 236, 92, 65, 79, 43, 196, 126, 254, 5, 188, 120, 250, 192, 65, 73, 182, 104, 51, 5 }, new byte[] { 29, 223, 94, 117, 145, 158, 190, 164, 15, 37, 60, 169, 174, 58, 209, 180 } });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 150, 77, 222, 115, 155, 204, 253, 154, 126, 78, 139, 248, 67, 150, 32, 25, 191, 111, 86, 59 }, new byte[] { 111, 18, 212, 114, 88, 25, 193, 171, 132, 59, 100, 145, 181, 226, 75, 76 } });
        }
    }
}
