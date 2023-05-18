using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnchorNews_AspNet.Migrations
{
    /// <inheritdoc />
    public partial class addViewCountToPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "NewsPosts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "NewsPosts");
        }
    }
}
