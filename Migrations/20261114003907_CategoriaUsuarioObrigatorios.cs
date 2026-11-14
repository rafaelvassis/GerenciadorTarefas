using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GerenciadorTarefas.Migrations
{
    /// <inheritdoc />
    public partial class CategoriaUsuarioObrigatorios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tarefas_Categorias_FK_IdCategoria",
                table: "Tarefas");

            migrationBuilder.DropForeignKey(
                name: "FK_Tarefas_Usuarios_FK_IdUsuario",
                table: "Tarefas");

            // Tarefas antigas sem categoria/usuário passam para "Geral" e "Usuário Padrão"
            migrationBuilder.Sql(
                "UPDATE Tarefas SET FK_IdCategoria = '8f1d6a2e-1c3b-4a5d-9e7f-000000000001' WHERE FK_IdCategoria IS NULL; " +
                "UPDATE Tarefas SET FK_IdUsuario = '3b7c9e4a-5d2f-4e8b-a1c6-000000000001' WHERE FK_IdUsuario IS NULL;");

            migrationBuilder.AlterColumn<Guid>(
                name: "FK_IdUsuario",
                table: "Tarefas",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "FK_IdCategoria",
                table: "Tarefas",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "IdCategoria", "Nome" },
                values: new object[,]
                {
                    { new Guid("8f1d6a2e-1c3b-4a5d-9e7f-000000000001"), "Geral" },
                    { new Guid("8f1d6a2e-1c3b-4a5d-9e7f-000000000002"), "Trabalho" },
                    { new Guid("8f1d6a2e-1c3b-4a5d-9e7f-000000000003"), "Estudos" },
                    { new Guid("8f1d6a2e-1c3b-4a5d-9e7f-000000000004"), "Pessoal" }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "IdUsuario", "Email", "Nome" },
                values: new object[] { new Guid("3b7c9e4a-5d2f-4e8b-a1c6-000000000001"), "usuario@exemplo.com", "Usuário Padrão" });

            migrationBuilder.AddForeignKey(
                name: "FK_Tarefas_Categorias_FK_IdCategoria",
                table: "Tarefas",
                column: "FK_IdCategoria",
                principalTable: "Categorias",
                principalColumn: "IdCategoria",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tarefas_Usuarios_FK_IdUsuario",
                table: "Tarefas",
                column: "FK_IdUsuario",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tarefas_Categorias_FK_IdCategoria",
                table: "Tarefas");

            migrationBuilder.DropForeignKey(
                name: "FK_Tarefas_Usuarios_FK_IdUsuario",
                table: "Tarefas");

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "IdCategoria",
                keyValue: new Guid("8f1d6a2e-1c3b-4a5d-9e7f-000000000001"));

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "IdCategoria",
                keyValue: new Guid("8f1d6a2e-1c3b-4a5d-9e7f-000000000002"));

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "IdCategoria",
                keyValue: new Guid("8f1d6a2e-1c3b-4a5d-9e7f-000000000003"));

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "IdCategoria",
                keyValue: new Guid("8f1d6a2e-1c3b-4a5d-9e7f-000000000004"));

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "IdUsuario",
                keyValue: new Guid("3b7c9e4a-5d2f-4e8b-a1c6-000000000001"));

            migrationBuilder.AlterColumn<Guid>(
                name: "FK_IdUsuario",
                table: "Tarefas",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "FK_IdCategoria",
                table: "Tarefas",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Tarefas_Categorias_FK_IdCategoria",
                table: "Tarefas",
                column: "FK_IdCategoria",
                principalTable: "Categorias",
                principalColumn: "IdCategoria");

            migrationBuilder.AddForeignKey(
                name: "FK_Tarefas_Usuarios_FK_IdUsuario",
                table: "Tarefas",
                column: "FK_IdUsuario",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario");
        }
    }
}
