using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOnline.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class updtabals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPrices_Guarantees_GuaranteeId",
                table: "ProductPrices");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPrices_Guarantees_GuaranteeId",
                table: "ProductPrices",
                column: "GuaranteeId",
                principalTable: "Guarantees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPrices_Guarantees_GuaranteeId",
                table: "ProductPrices");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPrices_Guarantees_GuaranteeId",
                table: "ProductPrices",
                column: "GuaranteeId",
                principalTable: "Guarantees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
