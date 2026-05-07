using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WareHouseApp.Dal.Migrations
{
    /// <inheritdoc />
    public partial class RenameInventoryProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_Warehouses_WarehouseId",
                table: "InventoryItems");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Products",
                newName: "UnitPrice");

            migrationBuilder.RenameColumn(
                name: "WarehouseId",
                table: "InventoryItems",
                newName: "WareHouseId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryItems_WarehouseId",
                table: "InventoryItems",
                newName: "IX_InventoryItems_WareHouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_Warehouses_WareHouseId",
                table: "InventoryItems",
                column: "WareHouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_Warehouses_WareHouseId",
                table: "InventoryItems");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "Products",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "WareHouseId",
                table: "InventoryItems",
                newName: "WarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryItems_WareHouseId",
                table: "InventoryItems",
                newName: "IX_InventoryItems_WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_Warehouses_WarehouseId",
                table: "InventoryItems",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
