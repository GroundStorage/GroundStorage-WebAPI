using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ground_Storage_WebAPI.Migrations
{
    public partial class somecolumnsadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Records",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "Records",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Entity",
                table: "Records",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Group",
                table: "Records",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Rev",
                table: "Records",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Records",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Records_Entity_Group",
                table: "Records",
                columns: new[] { "Entity", "Group" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Records_Entity_Group",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "Entity",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "Group",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "Rev",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Records");
        }
    }
}
