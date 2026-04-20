using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationAcademy.Migrations
{
    /// <inheritdoc />
    public partial class UpdateQuizAndUserProgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProgresses_Materials_MaterialId",
                table: "UserProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProgresses_Quizzes_QuizId",
                table: "UserProgresses");

            migrationBuilder.DropIndex(
                name: "IX_UserProgresses_MaterialId",
                table: "UserProgresses");

            migrationBuilder.DropIndex(
                name: "IX_UserProgresses_QuizId",
                table: "UserProgresses");

            migrationBuilder.DropColumn(
                name: "MaterialId",
                table: "UserProgresses");

            migrationBuilder.DropColumn(
                name: "QuizId",
                table: "UserProgresses");

            migrationBuilder.RenameColumn(
                name: "CompletedAt",
                table: "UserProgresses",
                newName: "CompletionDate");

            migrationBuilder.AddColumn<int>(
                name: "LevelId",
                table: "UserProgresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxLevelReached",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LevelId",
                table: "UserProgresses");

            migrationBuilder.DropColumn(
                name: "MaxLevelReached",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "CompletionDate",
                table: "UserProgresses",
                newName: "CompletedAt");

            migrationBuilder.AddColumn<int>(
                name: "MaterialId",
                table: "UserProgresses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuizId",
                table: "UserProgresses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProgresses_MaterialId",
                table: "UserProgresses",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProgresses_QuizId",
                table: "UserProgresses",
                column: "QuizId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgresses_Materials_MaterialId",
                table: "UserProgresses",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgresses_Quizzes_QuizId",
                table: "UserProgresses",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id");
        }
    }
}
