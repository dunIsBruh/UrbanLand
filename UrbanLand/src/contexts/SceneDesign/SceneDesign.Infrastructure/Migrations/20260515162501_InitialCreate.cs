using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SceneDesign.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "scene_design");

            migrationBuilder.CreateTable(
                name: "Scenes",
                schema: "scene_design",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    Max_Object_Limit = table.Column<int>(type: "integer", nullable: false),
                    Detect_Collisions = table.Column<bool>(type: "boolean", nullable: false),
                    current_view_mode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_locked = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_scenes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ObjectLayers",
                schema: "scene_design",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false),
                    is_visible = table.Column<bool>(type: "boolean", nullable: false),
                    is_locked = table.Column<bool>(type: "boolean", nullable: false),
                    scene_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_object_layers", x => x.id);
                    table.ForeignKey(
                        name: "fk_object_layers_scenes_scene_id",
                        column: x => x.scene_id,
                        principalSchema: "scene_design",
                        principalTable: "Scenes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SceneObjects",
                schema: "scene_design",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    asset_id = table.Column<Guid>(type: "uuid", nullable: false),
                    Position_X = table.Column<double>(type: "double precision", nullable: false),
                    Position_Y = table.Column<double>(type: "double precision", nullable: false),
                    Position_Z = table.Column<double>(type: "double precision", nullable: false),
                    rotation_Yaw = table.Column<double>(type: "double precision", nullable: false),
                    rotation_Pitch = table.Column<double>(type: "double precision", nullable: false),
                    rotation_Roll = table.Column<double>(type: "double precision", nullable: false),
                    scale_X = table.Column<double>(type: "double precision", nullable: false),
                    scale_Y = table.Column<double>(type: "double precision", nullable: false),
                    scale_Z = table.Column<double>(type: "double precision", nullable: false),
                    layer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    BoundingBox_Min_X = table.Column<double>(type: "double precision", nullable: false),
                    BoundingBox_Min_Y = table.Column<double>(type: "double precision", nullable: false),
                    BoundingBox_Min_Z = table.Column<double>(type: "double precision", nullable: false),
                    BoundingBox_Max_X = table.Column<double>(type: "double precision", nullable: false),
                    BoundingBox_Max_Y = table.Column<double>(type: "double precision", nullable: false),
                    BoundingBox_Max_Z = table.Column<double>(type: "double precision", nullable: false),
                    scene_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_scene_objects", x => x.id);
                    table.ForeignKey(
                        name: "fk_scene_objects_scenes_scene_id",
                        column: x => x.scene_id,
                        principalSchema: "scene_design",
                        principalTable: "Scenes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_object_layers_scene_id",
                schema: "scene_design",
                table: "ObjectLayers",
                column: "scene_id");

            migrationBuilder.CreateIndex(
                name: "ix_scene_objects_scene_id",
                schema: "scene_design",
                table: "SceneObjects",
                column: "scene_id");

            migrationBuilder.CreateIndex(
                name: "ix_scenes_project_id",
                schema: "scene_design",
                table: "Scenes",
                column: "project_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ObjectLayers",
                schema: "scene_design");

            migrationBuilder.DropTable(
                name: "SceneObjects",
                schema: "scene_design");

            migrationBuilder.DropTable(
                name: "Scenes",
                schema: "scene_design");
        }
    }
}
