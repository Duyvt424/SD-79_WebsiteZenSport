using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    public partial class _12_06_2023_16_33 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoesDetails_Size_SizeID",
                table: "ShoesDetails");

            migrationBuilder.AlterColumn<Guid>(
                name: "SizeID",
                table: "ShoesDetails",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoesDetails_Size_SizeID",
                table: "ShoesDetails",
                column: "SizeID",
                principalTable: "Size",
                principalColumn: "SizeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoesDetails_Size_SizeID",
                table: "ShoesDetails");

            migrationBuilder.AlterColumn<Guid>(
                name: "SizeID",
                table: "ShoesDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoesDetails_Size_SizeID",
                table: "ShoesDetails",
                column: "SizeID",
                principalTable: "Size",
                principalColumn: "SizeID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
