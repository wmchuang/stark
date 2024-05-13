using System.ComponentModel;

namespace Stark.Starter.Core.Enum
{
    public enum MenuTypeEnum
    {
        [Description("目录")] Catalog=0,
        [Description("菜单")] Menu=1,
        [Description("按钮")] Button=2
    }
}
