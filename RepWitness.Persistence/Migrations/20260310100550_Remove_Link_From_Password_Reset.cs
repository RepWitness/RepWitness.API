using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepWitness.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Remove_Link_From_Password_Reset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Link",
                table: "PasswordResets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "PasswordResets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
