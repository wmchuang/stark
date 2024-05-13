using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stark.Module.AI.Migrations
{
    /// <inheritdoc />
    public partial class update0320 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AiBot",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false, comment: "主键")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, comment: "描述")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ChatModelId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "AI模型")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Avatar = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, comment: "知识库头像")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Prompting = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false, comment: "提示词")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Temperature = table.Column<decimal>(type: "decimal(2,1)", precision: 2, scale: 1, nullable: false, comment: "温度"),
                    Opening = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, comment: "开场白")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartPrologues = table.Column<string>(type: "longtext", nullable: false, comment: "推荐问题")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WikiIds = table.Column<string>(type: "longtext", nullable: false, comment: "标识集合")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "创建人")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, comment: "创建人名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间"),
                    UpdateBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "最后修改人")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdateName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, comment: "修改人名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "最后修改时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiBot", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AiModel",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false, comment: "主键")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "模型描述")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ModelType = table.Column<int>(type: "int", maxLength: 50, nullable: false, comment: "模型类型"),
                    EndPoint = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "模型地址")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModelName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "模型名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModelKey = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false, comment: "秘钥")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "创建人")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, comment: "创建人名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间"),
                    UpdateBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "最后修改人")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdateName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, comment: "修改人名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "最后修改时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiModel", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AiWiki",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false, comment: "主键")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WikiName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "知识库名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "描述")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ChatModelId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "会话模型ID")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmbeddingModelId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "向量模型ID")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DbType = table.Column<int>(type: "int", maxLength: 5, nullable: false, comment: "保存位置"),
                    ConnectionString = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, comment: "数据库连接地址")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "创建人")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, comment: "创建人名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间"),
                    UpdateBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "最后修改人")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdateName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, comment: "修改人名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "最后修改时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiWiki", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AiWikiDocument",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false, comment: "主键")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WikiId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false, comment: "知识库标识")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FileName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, comment: "文件名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Path = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, comment: "地址")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Text = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false, comment: "文本")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<int>(type: "int", nullable: false, comment: "类型 文件、网页"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "状态"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiWikiDocument", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AiBot");

            migrationBuilder.DropTable(
                name: "AiModel");

            migrationBuilder.DropTable(
                name: "AiWiki");

            migrationBuilder.DropTable(
                name: "AiWikiDocument");
        }
    }
}
