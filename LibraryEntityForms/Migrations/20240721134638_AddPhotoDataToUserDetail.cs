using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryEntityForms.Migrations
{
    /// <inheritdoc />
    public partial class AddPhotoDataToUserDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserDetail_Id_User",
                table: "UserDetail");

            migrationBuilder.AddColumn<byte[]>(
                name: "PhotoData",
                table: "UserDetail",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.CreateIndex(
                name: "IX_UserDetail_Id_User",
                table: "UserDetail",
                column: "Id_User",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserDetail_Id_User",
                table: "UserDetail");

            migrationBuilder.DropColumn(
                name: "PhotoData",
                table: "UserDetail");

            migrationBuilder.CreateIndex(
                name: "IX_UserDetail_Id_User",
                table: "UserDetail",
                column: "Id_User");
        }
    }
}
