using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Order.API.Infrastructure.Migrations
{
  /// <inheritdoc />
  public partial class AlterTable : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<string>(
          name: "BuyerName",
          table: "Orders",
          type: "nvarchar(max)",
          nullable: false,
          defaultValue: "");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
          name: "BuyerName",
          table: "Orders");
    }
  }
}
