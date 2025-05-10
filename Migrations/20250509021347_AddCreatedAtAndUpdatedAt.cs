using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace warsztaty_blazor.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedAtAndUpdatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Person",
                type: "TEXT",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Person",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Person",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Person",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateOfBirth",
                table: "Person",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Person",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Person",
                type: "TEXT",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Person",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Person",
                type: "TEXT",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Person",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Person",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "Person",
                type: "TEXT",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AdminUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PersonId = table.Column<int>(type: "INTEGER", nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminUser_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminUser_PersonId",
                table: "AdminUser",
                column: "PersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminUser");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "Person");
        }
    }
}
