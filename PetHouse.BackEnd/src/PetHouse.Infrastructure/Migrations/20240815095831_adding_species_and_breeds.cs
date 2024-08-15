﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetHouse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class adding_species_and_breeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "breed",
                table: "pets");

            migrationBuilder.DropColumn(
                name: "specie",
                table: "pets");

            migrationBuilder.AddColumn<Guid>(
                name: "pet_identifier_breed_id",
                table: "pets",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "pet_identifier_species_id",
                table: "pets",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "species",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_species", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "breed",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    species_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_breed", x => new { x.species_id, x.id });
                    table.ForeignKey(
                        name: "fk_breed_species_species_id",
                        column: x => x.species_id,
                        principalTable: "species",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "breed");

            migrationBuilder.DropTable(
                name: "species");

            migrationBuilder.DropColumn(
                name: "pet_identifier_breed_id",
                table: "pets");

            migrationBuilder.DropColumn(
                name: "pet_identifier_species_id",
                table: "pets");

            migrationBuilder.AddColumn<string>(
                name: "breed",
                table: "pets",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "specie",
                table: "pets",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
