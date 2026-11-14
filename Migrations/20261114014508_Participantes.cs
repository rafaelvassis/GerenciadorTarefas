using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GerenciadorTarefas.Migrations
{
    /// <inheritdoc />
    public partial class Participantes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Participantes",
                columns: table => new
                {
                    IdParticipante = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Nome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participantes", x => x.IdParticipante);
                });

            migrationBuilder.CreateTable(
                name: "TarefasParticipantes",
                columns: table => new
                {
                    FK_IdTarefa = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FK_IdParticipante = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TarefasParticipantes", x => new { x.FK_IdTarefa, x.FK_IdParticipante });
                    table.ForeignKey(
                        name: "FK_TarefasParticipantes_Participantes_FK_IdParticipante",
                        column: x => x.FK_IdParticipante,
                        principalTable: "Participantes",
                        principalColumn: "IdParticipante",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TarefasParticipantes_Tarefas_FK_IdTarefa",
                        column: x => x.FK_IdTarefa,
                        principalTable: "Tarefas",
                        principalColumn: "IdTarefa",
                        onDelete: ReferentialAction.Cascade);
                });

            // Usuários cadastrados viram participantes e mantêm suas tarefas.
            // O "Usuário Padrão" era só um marcador e não é copiado.
            migrationBuilder.Sql(
                "INSERT INTO Participantes (IdParticipante, Nome, Email) " +
                "SELECT IdUsuario, Nome, Email FROM Usuarios WHERE Email <> 'usuario@exemplo.com';");

            migrationBuilder.Sql(
                "INSERT INTO TarefasParticipantes (FK_IdTarefa, FK_IdParticipante) " +
                "SELECT t.IdTarefa, t.FK_IdUsuario FROM Tarefas t " +
                "INNER JOIN Participantes p ON p.IdParticipante = t.FK_IdUsuario;");

            migrationBuilder.DropForeignKey(
                name: "FK_Tarefas_Usuarios_FK_IdUsuario",
                table: "Tarefas");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Tarefas_FK_IdUsuario",
                table: "Tarefas");

            migrationBuilder.DropColumn(
                name: "FK_IdUsuario",
                table: "Tarefas");

            migrationBuilder.InsertData(
                table: "Participantes",
                columns: new[] { "IdParticipante", "Email", "Nome" },
                values: new object[,]
                {
                    { new Guid("3b7c9e4a-5d2f-4e8b-a1c6-000000000001"), "ana@exemplo.com", "Ana Souza" },
                    { new Guid("3b7c9e4a-5d2f-4e8b-a1c6-000000000002"), "bruno@exemplo.com", "Bruno Lima" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Participantes_Email",
                table: "Participantes",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TarefasParticipantes_FK_IdParticipante",
                table: "TarefasParticipantes",
                column: "FK_IdParticipante");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TarefasParticipantes");

            migrationBuilder.DropTable(
                name: "Participantes");

            migrationBuilder.AddColumn<Guid>(
                name: "FK_IdUsuario",
                table: "Tarefas",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IdUsuario = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IdUsuario);
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "IdUsuario", "Email", "Nome" },
                values: new object[] { new Guid("3b7c9e4a-5d2f-4e8b-a1c6-000000000001"), "usuario@exemplo.com", "Usuário Padrão" });

            migrationBuilder.CreateIndex(
                name: "IX_Tarefas_FK_IdUsuario",
                table: "Tarefas",
                column: "FK_IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tarefas_Usuarios_FK_IdUsuario",
                table: "Tarefas",
                column: "FK_IdUsuario",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
