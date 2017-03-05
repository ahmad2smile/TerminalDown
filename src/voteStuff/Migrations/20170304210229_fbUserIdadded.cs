using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace voteStuff.Migrations
{
    public partial class fbUserIdadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FbUserId",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FbUserId",
                table: "AspNetUsers");
        }
    }
}
