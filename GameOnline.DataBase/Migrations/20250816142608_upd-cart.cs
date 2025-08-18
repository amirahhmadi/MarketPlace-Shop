using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOnline.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class updcart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDisCount",
                table: "ProductPrices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDisCount",
                table: "ProductPrices",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDisCount",
                table: "ProductPrices");

            migrationBuilder.DropColumn(
                name: "StartDisCount",
                table: "ProductPrices");
        }
    }
}
