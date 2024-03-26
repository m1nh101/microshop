using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Order.API.Infrastructure.Migrations
{
  /// <inheritdoc />
  public partial class InitialOrderDb : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "Order",
          columns: table => new
          {
            Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
            Status = table.Column<int>(type: "int", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Order", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "OrderItem",
          columns: table => new
          {
            OrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            ProductId = table.Column<string>(type: "nvarchar(max)", nullable: false),
            ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
            PictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
            Price = table.Column<double>(type: "float", nullable: false),
            Quantity = table.Column<int>(type: "int", nullable: false),
            Status = table.Column<int>(type: "int", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_OrderItem", x => new { x.OrderId, x.Id });
            table.ForeignKey(
                      name: "FK_OrderItem_Order_OrderId",
                      column: x => x.OrderId,
                      principalTable: "Order",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "OrderItem");

      migrationBuilder.DropTable(
          name: "Order");
    }
  }
}
