using Microsoft.EntityFrameworkCore.Migrations;

namespace DDDInPractice.Persistence.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VendingMachines",
                columns: table => new
                {
                    MachineId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AmountOf5K = table.Column<int>(nullable: true),
                    AmountOf10K = table.Column<int>(nullable: true),
                    AmountOf20K = table.Column<int>(nullable: true),
                    AmountOf50K = table.Column<int>(nullable: true),
                    AmountOf100K = table.Column<int>(nullable: true),
                    AmountOf200K = table.Column<int>(nullable: true),
                    AmountOf500K = table.Column<int>(nullable: true),
                    IsInTransaction = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendingMachines", x => x.MachineId);
                });

            migrationBuilder.CreateTable(
                name: "Slots",
                columns: table => new
                {
                    SlotId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(nullable: true),
                    ProductCount = table.Column<int>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    Position = table.Column<string>(nullable: true),
                    VendingMachineId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slots", x => x.SlotId);
                    table.ForeignKey(
                        name: "FK_Slots_VendingMachines_VendingMachineId",
                        column: x => x.VendingMachineId,
                        principalTable: "VendingMachines",
                        principalColumn: "MachineId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Slots_VendingMachineId",
                table: "Slots",
                column: "VendingMachineId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Slots");

            migrationBuilder.DropTable(
                name: "VendingMachines");
        }
    }
}
