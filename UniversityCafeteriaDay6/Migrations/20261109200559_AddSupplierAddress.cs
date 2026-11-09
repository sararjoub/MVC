using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityCafeteriaDay6.Migrations
{
    /// <inheritdoc />
    public partial class AddSupplierAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Addrees",
                table: "Suppliers",
                newName: "Address");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Suppliers",
                newName: "Addrees");
        }
    }
}
