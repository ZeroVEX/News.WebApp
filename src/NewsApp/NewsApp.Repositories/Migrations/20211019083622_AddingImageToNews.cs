using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsApp.Repositories.Migrations
{
    public partial class AddingImageToNews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "News",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "News");
        }
    }
}
