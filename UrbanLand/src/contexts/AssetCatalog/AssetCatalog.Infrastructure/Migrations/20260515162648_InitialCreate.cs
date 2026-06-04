using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "asset_catalog");

            migrationBuilder.CreateTable(
                name: "AssetTemplates",
                schema: "asset_catalog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_custom = table.Column<bool>(type: "boolean", nullable: false),
                    uploader_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Topographic_Symbol_Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Topographic_Symbol_Color = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Topographic_Symbol_Size = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asset_templates", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AssetVersions",
                schema: "asset_catalog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    version_number = table.Column<int>(type: "integer", nullable: false),
                    Model_File_Url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Model_File_Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Model_File_Size = table.Column<long>(type: "bigint", nullable: false),
                    Model_Format = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Model_Width = table.Column<double>(type: "double precision", nullable: false),
                    Model_Height = table.Column<double>(type: "double precision", nullable: false),
                    Model_Depth = table.Column<double>(type: "double precision", nullable: false),
                    Model_Polygon_Count = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    asset_template_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asset_versions", x => x.id);
                    table.ForeignKey(
                        name: "fk_asset_versions_asset_templates_asset_template_id",
                        column: x => x.asset_template_id,
                        principalSchema: "asset_catalog",
                        principalTable: "AssetTemplates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_asset_templates_category",
                schema: "asset_catalog",
                table: "AssetTemplates",
                column: "category");

            migrationBuilder.CreateIndex(
                name: "ix_asset_templates_name",
                schema: "asset_catalog",
                table: "AssetTemplates",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_asset_versions_asset_template_id",
                schema: "asset_catalog",
                table: "AssetVersions",
                column: "asset_template_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetVersions",
                schema: "asset_catalog");

            migrationBuilder.DropTable(
                name: "AssetTemplates",
                schema: "asset_catalog");
        }
    }
}
