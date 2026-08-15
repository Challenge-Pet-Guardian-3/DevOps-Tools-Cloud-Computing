using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetGuardian.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "estado",
                columns: table => new
                {
                    id_estado = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    nome_estado = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_estado", x => x.id_estado);
                });

            migrationBuilder.CreateTable(
                name: "raca",
                columns: table => new
                {
                    id_raca = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    nome_raca = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_raca", x => x.id_raca);
                });

            migrationBuilder.CreateTable(
                name: "status",
                columns: table => new
                {
                    id_status = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    nome_status = table.Column<string>(type: "NVARCHAR2(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_status", x => x.id_status);
                });

            migrationBuilder.CreateTable(
                name: "telefone",
                columns: table => new
                {
                    id_telefone = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    num_ddd = table.Column<string>(type: "NVARCHAR2(2)", maxLength: 2, nullable: false),
                    num_tel = table.Column<string>(type: "NVARCHAR2(9)", maxLength: 9, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_telefone", x => x.id_telefone);
                });

            migrationBuilder.CreateTable(
                name: "tipo_atend",
                columns: table => new
                {
                    id_tipo_atend = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    tipo = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipo_atend", x => x.id_tipo_atend);
                });

            migrationBuilder.CreateTable(
                name: "cidade",
                columns: table => new
                {
                    id_cidade = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    nome_cidade = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    estado_id_estado = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cidade", x => x.id_cidade);
                    table.ForeignKey(
                        name: "FK_cidade_estado_estado_id_estado",
                        column: x => x.estado_id_estado,
                        principalTable: "estado",
                        principalColumn: "id_estado",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "pet",
                columns: table => new
                {
                    id_pet = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    nome = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    idade = table.Column<byte>(type: "NUMBER(2)", nullable: false),
                    sexo = table.Column<string>(type: "NVARCHAR2(1)", maxLength: 1, nullable: false),
                    porte = table.Column<string>(type: "NVARCHAR2(10)", maxLength: 10, nullable: false),
                    castrado = table.Column<string>(type: "NVARCHAR2(1)", maxLength: 1, nullable: false),
                    raca_id_raca = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pet", x => x.id_pet);
                    table.ForeignKey(
                        name: "FK_pet_raca_raca_id_raca",
                        column: x => x.raca_id_raca,
                        principalTable: "raca",
                        principalColumn: "id_raca",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    id_usuario = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    senha = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    telefone_id_telefone = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario", x => x.id_usuario);
                    table.ForeignKey(
                        name: "FK_usuario_telefone_telefone_id_telefone",
                        column: x => x.telefone_id_telefone,
                        principalTable: "telefone",
                        principalColumn: "id_telefone",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bairro",
                columns: table => new
                {
                    id_bairro = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    nome_bairro = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    cidade_id_cidade = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bairro", x => x.id_bairro);
                    table.ForeignKey(
                        name: "FK_bairro_cidade_cidade_id_cidade",
                        column: x => x.cidade_id_cidade,
                        principalTable: "cidade",
                        principalColumn: "id_cidade",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "usuario_pet",
                columns: table => new
                {
                    usuario_id_usuario = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    pet_id_pet = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    respon_princ = table.Column<string>(type: "NVARCHAR2(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario_pet", x => new { x.usuario_id_usuario, x.pet_id_pet });
                    table.ForeignKey(
                        name: "FK_usuario_pet_pet_pet_id_pet",
                        column: x => x.pet_id_pet,
                        principalTable: "pet",
                        principalColumn: "id_pet",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_usuario_pet_usuario_usuario_id_usuario",
                        column: x => x.usuario_id_usuario,
                        principalTable: "usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "endereco",
                columns: table => new
                {
                    id_endereco = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    cep = table.Column<string>(type: "NVARCHAR2(8)", maxLength: 8, nullable: false),
                    rua = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    numero = table.Column<string>(type: "NVARCHAR2(5)", maxLength: 5, nullable: false),
                    bairro_id_bairro = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_endereco", x => x.id_endereco);
                    table.ForeignKey(
                        name: "FK_endereco_bairro_bairro_id_bairro",
                        column: x => x.bairro_id_bairro,
                        principalTable: "bairro",
                        principalColumn: "id_bairro",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "clinica",
                columns: table => new
                {
                    id_clinica = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    nome = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    telefone_id_telefone = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    endereco_id_endereco = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clinica", x => x.id_clinica);
                    table.ForeignKey(
                        name: "FK_clinica_endereco_endereco_id_endereco",
                        column: x => x.endereco_id_endereco,
                        principalTable: "endereco",
                        principalColumn: "id_endereco",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_clinica_telefone_telefone_id_telefone",
                        column: x => x.telefone_id_telefone,
                        principalTable: "telefone",
                        principalColumn: "id_telefone",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usuario_endereco",
                columns: table => new
                {
                    usuario_id_usuario = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    endereco_id_endereco = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario_endereco", x => new { x.usuario_id_usuario, x.endereco_id_endereco });
                    table.ForeignKey(
                        name: "FK_usuario_endereco_endereco_endereco_id_endereco",
                        column: x => x.endereco_id_endereco,
                        principalTable: "endereco",
                        principalColumn: "id_endereco",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_usuario_endereco_usuario_usuario_id_usuario",
                        column: x => x.usuario_id_usuario,
                        principalTable: "usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "veterinario",
                columns: table => new
                {
                    id_veterinario = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    senha = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    telefone_id_telefone = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    clinica_id_clinica = table.Column<Guid>(type: "RAW(16)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_veterinario", x => x.id_veterinario);
                    table.ForeignKey(
                        name: "FK_veterinario_clinica_clinica_id_clinica",
                        column: x => x.clinica_id_clinica,
                        principalTable: "clinica",
                        principalColumn: "id_clinica",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_veterinario_telefone_telefone_id_telefone",
                        column: x => x.telefone_id_telefone,
                        principalTable: "telefone",
                        principalColumn: "id_telefone",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "atendimento",
                columns: table => new
                {
                    id_atendimento = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    data = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    anotacoes = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: false),
                    valor = table.Column<decimal>(type: "NUMBER(10,2)", nullable: false),
                    pet_id_pet = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    veterinario_id_veterinario = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    tipo_atend_id_tipo_atend = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    status_id_status = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_atendimento", x => x.id_atendimento);
                    table.ForeignKey(
                        name: "FK_atendimento_pet_pet_id_pet",
                        column: x => x.pet_id_pet,
                        principalTable: "pet",
                        principalColumn: "id_pet",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_atendimento_status_status_id_status",
                        column: x => x.status_id_status,
                        principalTable: "status",
                        principalColumn: "id_status",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_atendimento_tipo_atend_tipo_atend_id_tipo_atend",
                        column: x => x.tipo_atend_id_tipo_atend,
                        principalTable: "tipo_atend",
                        principalColumn: "id_tipo_atend",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_atendimento_veterinario_veterinario_id_veterinario",
                        column: x => x.veterinario_id_veterinario,
                        principalTable: "veterinario",
                        principalColumn: "id_veterinario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tarefa",
                columns: table => new
                {
                    id_tarefa = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    titulo = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    pontos_tarefa = table.Column<byte>(type: "NUMBER(3)", nullable: false),
                    descricao = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    criacao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    prazo = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    conclusao = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    usuario_id_usuario = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    pet_id_pet = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    status_id_status = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    veterinario_id_veterinario = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tarefa", x => x.id_tarefa);
                    table.ForeignKey(
                        name: "FK_tarefa_pet_pet_id_pet",
                        column: x => x.pet_id_pet,
                        principalTable: "pet",
                        principalColumn: "id_pet",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tarefa_status_status_id_status",
                        column: x => x.status_id_status,
                        principalTable: "status",
                        principalColumn: "id_status",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tarefa_usuario_usuario_id_usuario",
                        column: x => x.usuario_id_usuario,
                        principalTable: "usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_tarefa_veterinario_veterinario_id_veterinario",
                        column: x => x.veterinario_id_veterinario,
                        principalTable: "veterinario",
                        principalColumn: "id_veterinario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_atendimento_pet_id_pet",
                table: "atendimento",
                column: "pet_id_pet");

            migrationBuilder.CreateIndex(
                name: "IX_atendimento_status_id_status",
                table: "atendimento",
                column: "status_id_status");

            migrationBuilder.CreateIndex(
                name: "IX_atendimento_tipo_atend_id_tipo_atend",
                table: "atendimento",
                column: "tipo_atend_id_tipo_atend");

            migrationBuilder.CreateIndex(
                name: "IX_atendimento_veterinario_id_veterinario",
                table: "atendimento",
                column: "veterinario_id_veterinario");

            migrationBuilder.CreateIndex(
                name: "IX_bairro_cidade_id_cidade",
                table: "bairro",
                column: "cidade_id_cidade");

            migrationBuilder.CreateIndex(
                name: "IX_cidade_estado_id_estado",
                table: "cidade",
                column: "estado_id_estado");

            migrationBuilder.CreateIndex(
                name: "IX_clinica_endereco_id_endereco",
                table: "clinica",
                column: "endereco_id_endereco",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_clinica_telefone_id_telefone",
                table: "clinica",
                column: "telefone_id_telefone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_endereco_bairro_id_bairro",
                table: "endereco",
                column: "bairro_id_bairro");

            migrationBuilder.CreateIndex(
                name: "IX_pet_raca_id_raca",
                table: "pet",
                column: "raca_id_raca");

            migrationBuilder.CreateIndex(
                name: "IX_tarefa_pet_id_pet",
                table: "tarefa",
                column: "pet_id_pet");

            migrationBuilder.CreateIndex(
                name: "IX_tarefa_status_id_status",
                table: "tarefa",
                column: "status_id_status");

            migrationBuilder.CreateIndex(
                name: "IX_tarefa_usuario_id_usuario",
                table: "tarefa",
                column: "usuario_id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_tarefa_veterinario_id_veterinario",
                table: "tarefa",
                column: "veterinario_id_veterinario");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_email",
                table: "usuario",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_usuario_telefone_id_telefone",
                table: "usuario",
                column: "telefone_id_telefone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_usuario_endereco_endereco_id_endereco",
                table: "usuario_endereco",
                column: "endereco_id_endereco");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_pet_pet_id_pet",
                table: "usuario_pet",
                column: "pet_id_pet");

            migrationBuilder.CreateIndex(
                name: "IX_veterinario_clinica_id_clinica",
                table: "veterinario",
                column: "clinica_id_clinica");

            migrationBuilder.CreateIndex(
                name: "IX_veterinario_email",
                table: "veterinario",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_veterinario_telefone_id_telefone",
                table: "veterinario",
                column: "telefone_id_telefone",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "atendimento");

            migrationBuilder.DropTable(
                name: "tarefa");

            migrationBuilder.DropTable(
                name: "usuario_endereco");

            migrationBuilder.DropTable(
                name: "usuario_pet");

            migrationBuilder.DropTable(
                name: "tipo_atend");

            migrationBuilder.DropTable(
                name: "status");

            migrationBuilder.DropTable(
                name: "veterinario");

            migrationBuilder.DropTable(
                name: "pet");

            migrationBuilder.DropTable(
                name: "usuario");

            migrationBuilder.DropTable(
                name: "clinica");

            migrationBuilder.DropTable(
                name: "raca");

            migrationBuilder.DropTable(
                name: "endereco");

            migrationBuilder.DropTable(
                name: "telefone");

            migrationBuilder.DropTable(
                name: "bairro");

            migrationBuilder.DropTable(
                name: "cidade");

            migrationBuilder.DropTable(
                name: "estado");
        }
    }
}
