using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using voteStuff.Entities;

namespace voteStuff.Migrations
{
    [DbContext(typeof(VoteDbContext))]
    [Migration("20170103170042_initialMigrations")]
    partial class initialMigrations
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("voteStuff.Models.VoteDuo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DuoFirst");

                    b.Property<int>("DuoFirstVotes");

                    b.Property<string>("DuoSecond");

                    b.Property<int>("DuoSecondVotes");

                    b.Property<int>("DuoTotalVotes");

                    b.HasKey("Id");

                    b.ToTable("VotesDb");
                });
        }
    }
}
