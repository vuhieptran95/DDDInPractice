using Microsoft.EntityFrameworkCore.Migrations;

namespace DDDInPractice.Persistence.Migrations
{
    public partial class RemoveIsInTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInTransaction",
                table: "VendingMachines");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsInTransaction",
                table: "VendingMachines",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
