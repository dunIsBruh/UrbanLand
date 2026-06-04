using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SceneDesign.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixColumnNamesAndConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_scenes_project_id",
                schema: "scene_design",
                table: "Scenes");

            migrationBuilder.CreateIndex(
                name: "ix_scenes_project_id",
                schema: "scene_design",
                table: "Scenes",
                column: "project_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_scenes_project_id",
                schema: "scene_design",
                table: "Scenes");

            migrationBuilder.CreateIndex(
                name: "ix_scenes_project_id",
                schema: "scene_design",
                table: "Scenes",
                column: "project_id",
                unique: true);
        }
    }
}
