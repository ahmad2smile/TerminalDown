using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace voteStuff.Migrations
{
    public partial class initialMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VotesDb",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DuoFirst = table.Column<string>(nullable: true),
                    DuoFirstVotes = table.Column<int>(nullable: false),
                    DuoSecond = table.Column<string>(nullable: true),
                    DuoSecondVotes = table.Column<int>(nullable: false),
                    DuoTotalVotes = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VotesDb", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VotesDb");
        }
    }
}
