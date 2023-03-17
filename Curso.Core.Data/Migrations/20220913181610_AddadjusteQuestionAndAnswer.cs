using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Curso.Core.Data.Migrations
{
    public partial class AddadjusteQuestionAndAnswer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Querys_QuestionId",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "AnswerId",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Answers");

            migrationBuilder.AddColumn<Guid>(
                name: "PermitionAccessId",
                table: "Querys",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "QuestionId",
                table: "Answers",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "PermitionAccessId",
                table: "Answers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Question",
                table: "Answers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Response",
                table: "Answers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Querys_PermitionAccessId",
                table: "Querys",
                column: "PermitionAccessId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_PermitionAccessId",
                table: "Answers",
                column: "PermitionAccessId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_PermitionAccesses_PermitionAccessId",
                table: "Answers",
                column: "PermitionAccessId",
                principalTable: "PermitionAccesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Querys_QuestionId",
                table: "Answers",
                column: "QuestionId",
                principalTable: "Querys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Querys_PermitionAccesses_PermitionAccessId",
                table: "Querys",
                column: "PermitionAccessId",
                principalTable: "PermitionAccesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_PermitionAccesses_PermitionAccessId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Querys_QuestionId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Querys_PermitionAccesses_PermitionAccessId",
                table: "Querys");

            migrationBuilder.DropIndex(
                name: "IX_Querys_PermitionAccessId",
                table: "Querys");

            migrationBuilder.DropIndex(
                name: "IX_Answers_PermitionAccessId",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "PermitionAccessId",
                table: "Querys");

            migrationBuilder.DropColumn(
                name: "PermitionAccessId",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "Question",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "Response",
                table: "Answers");

            migrationBuilder.AlterColumn<Guid>(
                name: "QuestionId",
                table: "Answers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnswerId",
                table: "Answers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Answers",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Answers",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Querys_QuestionId",
                table: "Answers",
                column: "QuestionId",
                principalTable: "Querys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
