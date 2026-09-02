using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_CommerceSite.Datos.Migrations
{
    /// <inheritdoc />
    public partial class Sehacambiadolarelacionentrecustomeryuserentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_AspNetUsers_UserEntityId",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_UserEntityId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "UserEntityId",
                table: "Customer");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CustomerId",
                table: "AspNetUsers",
                column: "CustomerId",
                unique: true,
                filter: "[CustomerId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Customer_CustomerId",
                table: "AspNetUsers",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Customer_CustomerId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CustomerId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "UserEntityId",
                table: "Customer",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_UserEntityId",
                table: "Customer",
                column: "UserEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_AspNetUsers_UserEntityId",
                table: "Customer",
                column: "UserEntityId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
