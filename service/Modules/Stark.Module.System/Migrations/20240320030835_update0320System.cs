using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stark.Module.System.Migrations
{
    /// <inheritdoc />
    public partial class update0320System : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SySDept",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false, comment: "主键")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DeptName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "部门名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentDeptId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false, comment: "父级部门id")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TreePath = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false, comment: "节点路径")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sort = table.Column<int>(type: "int", nullable: false, comment: "排序"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false, comment: "是否删除"),
                    Status = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: false, defaultValue: "Y", comment: "状态 Y:启用 N:禁用")
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
                    table.PrimaryKey("PK_SySDept", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SysLogEx",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false, comment: "主键")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Message = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false, comment: "日志消息")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StackTrace = table.Column<string>(type: "longtext", nullable: false, comment: "堆栈信息")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RequestUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false, comment: "请求地址")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Source = table.Column<string>(type: "longtext", nullable: false, comment: "来源")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LoginName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "登录账号")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "用户名称")
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
                    table.PrimaryKey("PK_SysLogEx", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SysLogVisit",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false, comment: "主键")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ActionName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "方法名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RemoteIp = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "IP地址")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Location = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "登录地点")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Browser = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "浏览器")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Os = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "操作系统")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Elapsed = table.Column<long>(type: "bigint", nullable: false, comment: "操作用时"),
                    LoginName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "登录账号")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "用户名称")
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
                    table.PrimaryKey("PK_SysLogVisit", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SysMenu",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false, comment: "主键")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "角色名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false, comment: "父菜单ID")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sort = table.Column<int>(type: "int", nullable: false, comment: "角色排序"),
                    Link = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, comment: "是否为外链(有值则为外链)")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<int>(type: "int", nullable: false, comment: "菜单类型 0：目录，1：菜单，2：按钮"),
                    Code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "权限字符串")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Icon = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "菜单图标")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Hidden = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false, comment: "是否隐藏"),
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
                    table.PrimaryKey("PK_SysMenu", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SysRole",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false, comment: "主键")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "角色名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Key = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "角色权限")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sort = table.Column<int>(type: "int", nullable: false, comment: "角色排序"),
                    DataScope = table.Column<int>(type: "int", nullable: false, comment: "数据范围"),
                    Remark = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, comment: "备注")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: false, defaultValue: "Y", comment: "状态 Y:启用 N:禁用")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false, comment: "是否删除"),
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
                    table.PrimaryKey("PK_SysRole", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SysRoleMenu",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false, comment: "主键")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RoleId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false, comment: "角色ID")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MenuId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false, comment: "菜单ID")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysRoleMenu", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SysUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false, comment: "主键")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DeptId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false, comment: "部门Id")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LoginName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "登录账号")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "用户名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Avatar = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false, comment: "用户头像")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "密码")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Salt = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, comment: "加密盐")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false, comment: "手机号码")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false, comment: "是否删除"),
                    Status = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: false, defaultValue: "Y", comment: "状态 Y:启用 N:禁用")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Remark = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, comment: "备注")
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
                    table.PrimaryKey("PK_SysUser", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SysUserRole",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false, comment: "主键")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false, comment: "用户ID")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RoleId = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false, comment: "角色ID")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysUserRole", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SySDept");

            migrationBuilder.DropTable(
                name: "SysLogEx");

            migrationBuilder.DropTable(
                name: "SysLogVisit");

            migrationBuilder.DropTable(
                name: "SysMenu");

            migrationBuilder.DropTable(
                name: "SysRole");

            migrationBuilder.DropTable(
                name: "SysRoleMenu");

            migrationBuilder.DropTable(
                name: "SysUser");

            migrationBuilder.DropTable(
                name: "SysUserRole");
        }
    }
}
