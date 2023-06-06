using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryManagementSystem.Migrations
{
    public partial class ChangedColumnNamePurchaseOrderHeaderId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderDetails_PurchaseOrderHeaders_PoId",
                table: "PurchaseOrderDetails");

            migrationBuilder.RenameColumn(
                name: "PoId",
                table: "PurchaseOrderDetails",
                newName: "PurchaseOrderHeaderId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseOrderDetails_PoId",
                table: "PurchaseOrderDetails",
                newName: "IX_PurchaseOrderDetails_PurchaseOrderHeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderDetails_PurchaseOrderHeaders_PurchaseOrderHeaderId",
                table: "PurchaseOrderDetails",
                column: "PurchaseOrderHeaderId",
                principalTable: "PurchaseOrderHeaders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderDetails_PurchaseOrderHeaders_PurchaseOrderHeaderId",
                table: "PurchaseOrderDetails");

            migrationBuilder.RenameColumn(
                name: "PurchaseOrderHeaderId",
                table: "PurchaseOrderDetails",
                newName: "PoId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseOrderDetails_PurchaseOrderHeaderId",
                table: "PurchaseOrderDetails",
                newName: "IX_PurchaseOrderDetails_PoId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderDetails_PurchaseOrderHeaders_PoId",
                table: "PurchaseOrderDetails",
                column: "PoId",
                principalTable: "PurchaseOrderHeaders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
