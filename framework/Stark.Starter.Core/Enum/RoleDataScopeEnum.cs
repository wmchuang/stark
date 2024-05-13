using System.ComponentModel;

namespace Stark.Starter.Core.Enum;

public enum RoleDataScopeEnum
{
    [Description("无权限")] None = 0,
    [Description("全部数据权限")] All = 1,
    [Description("本部门及以下数据权限")] DeptAndBelow = 2,
    [Description("本部门数据权限")] Dept = 3,
    [Description("仅本人数据权限")] Personal = 4,
}