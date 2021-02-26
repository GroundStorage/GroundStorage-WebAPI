using Microsoft.EntityFrameworkCore.Migrations;

namespace Ground_Storage_WebAPI.Migrations
{
    public partial class grouppropertyremovedfromRecordentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Records_Entity_Group",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "Group",
                table: "Records");

            migrationBuilder.CreateIndex(
                name: "IX_Records_Entity",
                table: "Records",
                column: "Entity");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Records_Entity",
                table: "Records");

            migrationBuilder.AddColumn<string>(
                name: "Group",
                table: "Records",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Records_Entity_Group",
                table: "Records",
                columns: new[] { "Entity", "Group" });
        }
    }
}
