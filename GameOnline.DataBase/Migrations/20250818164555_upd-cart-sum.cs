using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOnline.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class updcartsum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SumOrder",
                table: "Carts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SumScore",
                table: "Carts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SumOrder",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "SumScore",
                table: "Carts");
        }
    }
}
