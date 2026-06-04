using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialProjectManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "project_management");

            migrationBuilder.CreateTable(
                name: "Projects",
                schema: "project_management",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Is_Public = table.Column<bool>(type: "boolean", nullable: false),
                    Allow_Public_Comments = table.Column<bool>(type: "boolean", nullable: false),
                    Default_Access_Level = table.Column<int>(type: "integer", nullable: false),
                    Invitation_Validity_Period = table.Column<long>(type: "bigint", nullable: false),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_projects", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectInvitations",
                schema: "project_management",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    invite_code = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    Suggested_Role_Level = table.Column<int>(type: "integer", nullable: false),
                    Suggested_Role_Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_used = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project_invitations", x => x.id);
                    table.ForeignKey(
                        name: "fk_project_invitations_projects_project_id",
                        column: x => x.project_id,
                        principalSchema: "project_management",
                        principalTable: "Projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectMembers",
                schema: "project_management",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    member_id = table.Column<Guid>(type: "uuid", nullable: false),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    Role_Level = table.Column<int>(type: "integer", nullable: false),
                    Role_Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    invited_by = table.Column<Guid>(type: "uuid", nullable: true),
                    joined_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project_members", x => x.id);
                    table.ForeignKey(
                        name: "fk_project_members_projects_project_id",
                        column: x => x.project_id,
                        principalSchema: "project_management",
                        principalTable: "Projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_project_invitations_expires_at",
                schema: "project_management",
                table: "ProjectInvitations",
                column: "expires_at");

            migrationBuilder.CreateIndex(
                name: "ix_project_invitations_invite_code",
                schema: "project_management",
                table: "ProjectInvitations",
                column: "invite_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_project_invitations_is_used",
                schema: "project_management",
                table: "ProjectInvitations",
                column: "is_used");

            migrationBuilder.CreateIndex(
                name: "ix_project_invitations_project_id",
                schema: "project_management",
                table: "ProjectInvitations",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "ix_project_members_joined_at",
                schema: "project_management",
                table: "ProjectMembers",
                column: "joined_at");

            migrationBuilder.CreateIndex(
                name: "ix_project_members_member_id",
                schema: "project_management",
                table: "ProjectMembers",
                column: "member_id");

            migrationBuilder.CreateIndex(
                name: "ix_project_members_project_id_member_id",
                schema: "project_management",
                table: "ProjectMembers",
                columns: new[] { "project_id", "member_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_RoleLevel",
                schema: "project_management",
                table: "ProjectMembers",
                column: "Role_Level");

            migrationBuilder.CreateIndex(
                name: "ix_projects_name",
                schema: "project_management",
                table: "Projects",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_projects_status",
                schema: "project_management",
                table: "Projects",
                column: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectInvitations",
                schema: "project_management");

            migrationBuilder.DropTable(
                name: "ProjectMembers",
                schema: "project_management");

            migrationBuilder.DropTable(
                name: "Projects",
                schema: "project_management");
        }
    }
}
