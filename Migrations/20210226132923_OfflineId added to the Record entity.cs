using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ground_Storage_WebAPI.Migrations
{
    public partial class OfflineIdaddedtotheRecordentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Records_Entity",
                table: "Records");

            migrationBuilder.AddColumn<Guid>(
                name: "OfflineId",
                table: "Records",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Records_Entity_Rev_OfflineId_CreatedAt_UpdatedAt_CreatorId",
                table: "Records",
                columns: new[] { "Entity", "Rev", "OfflineId", "CreatedAt", "UpdatedAt", "CreatorId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Records_Entity_Rev_OfflineId_CreatedAt_UpdatedAt_CreatorId",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "OfflineId",
                table: "Records");

            migrationBuilder.CreateIndex(
                name: "IX_Records_Entity",
                table: "Records",
                column: "Entity");
        }
    }
}
