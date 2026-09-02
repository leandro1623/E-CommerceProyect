using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_CommerceSite.Datos.Migrations
{
    /// <inheritdoc />
    public partial class SehaeliminadoelcampoPhonedelaentidadcustomerdebidoaqueyaesteestaincluidoenUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Customer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Customer",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}
