using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace voteStuff.Migrations
{
    public partial class VotingDbInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserVotingDbs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LastTimeVotesGifted = table.Column<DateTime>(nullable: false),
                    LastTotallVotesGifted = table.Column<int>(nullable: false),
                    TotallCastedVotes = table.Column<int>(nullable: false),
                    TotallVotingRights = table.Column<int>(nullable: false),
                    UserID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserVotingDbs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserVotingDbs_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DuoVotedByUserDbs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DuoID = table.Column<int>(nullable: false),
                    UserVotingDbID = table.Column<int>(nullable: false),
                    VotingTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DuoVotedByUserDbs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DuoVotedByUserDbs_VotesDb_DuoID",
                        column: x => x.DuoID,
                        principalTable: "VotesDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DuoVotedByUserDbs_UserVotingDbs_UserVotingDbID",
                        column: x => x.UserVotingDbID,
                        principalTable: "UserVotingDbs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DuoVotedByUserDbs_DuoID",
                table: "DuoVotedByUserDbs",
                column: "DuoID");

            migrationBuilder.CreateIndex(
                name: "IX_DuoVotedByUserDbs_UserVotingDbID",
                table: "DuoVotedByUserDbs",
                column: "UserVotingDbID");

            migrationBuilder.CreateIndex(
                name: "IX_UserVotingDbs_UserID",
                table: "UserVotingDbs",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DuoVotedByUserDbs");

            migrationBuilder.DropTable(
                name: "UserVotingDbs");
        }
    }
}
