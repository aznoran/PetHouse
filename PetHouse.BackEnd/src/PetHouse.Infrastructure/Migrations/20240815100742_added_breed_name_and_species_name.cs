using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetHouse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class added_breed_name_and_species_name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "pet_identifier_species_id",
                table: "pets",
                newName: "species_id");

            migrationBuilder.RenameColumn(
                name: "pet_identifier_breed_id",
                table: "pets",
                newName: "breed_id");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "species",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "breed",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                table: "species");

            migrationBuilder.DropColumn(
                name: "name",
                table: "breed");

            migrationBuilder.RenameColumn(
                name: "species_id",
                table: "pets",
                newName: "pet_identifier_species_id");

            migrationBuilder.RenameColumn(
                name: "breed_id",
                table: "pets",
                newName: "pet_identifier_breed_id");
        }
    }
}
