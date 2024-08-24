using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Prescription.Migrations
{
    /// <inheritdoc />
    public partial class TesCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StockReceivedId",
                table: "StockOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockOrders_StockReceivedId",
                table: "StockOrders",
                column: "StockReceivedId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockOrders_StockReceived_StockReceivedId",
                table: "StockOrders",
                column: "StockReceivedId",
                principalTable: "StockReceived",
                principalColumn: "StockReceivedId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockOrders_StockReceived_StockReceivedId",
                table: "StockOrders");

            migrationBuilder.DropIndex(
                name: "IX_StockOrders_StockReceivedId",
                table: "StockOrders");

            migrationBuilder.DropColumn(
                name: "StockReceivedId",
                table: "StockOrders");
        }
    }
}
