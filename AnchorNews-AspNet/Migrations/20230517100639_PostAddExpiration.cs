using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnchorNews_AspNet.Migrations
{
    /// <inheritdoc />
    public partial class PostAddExpiration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BreakingNewsExpiration",
                table: "NewsPosts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BreakingNewsExpiration",
                table: "NewsPosts");
        }
    }
}
