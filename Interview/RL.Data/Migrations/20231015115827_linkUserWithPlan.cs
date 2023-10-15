using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RL.Data.Migrations
{
    public partial class linkUserWithPlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserPlanProcedureMappings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProcedureId = table.Column<int>(type: "INTEGER", nullable: false),
                    PlanId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPlanProcedureMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPlanProcedureMappings_PlanProcedures_PlanId_ProcedureId",
                        columns: x => new { x.PlanId, x.ProcedureId },
                        principalTable: "PlanProcedures",
                        principalColumns: new[] { "PlanId", "ProcedureId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPlanProcedureMappings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPlanProcedureMappings_PlanId_ProcedureId",
                table: "UserPlanProcedureMappings",
                columns: new[] { "PlanId", "ProcedureId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserPlanProcedureMappings_UserId",
                table: "UserPlanProcedureMappings",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPlanProcedureMappings");
        }
    }
}
