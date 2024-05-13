using Stark.Starter.Core.Enum;
using Stark.Starter.DDD.Domain.Entities;

namespace Stark.Module.System.Domain
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class SysMenu : AggregateRoot
    {
        #region 构造函数

        public SysMenu()
        {
            Hidden = false;
            Icon = string.Empty;
            Link = string.Empty;
            Sort = 0;
        }

        /// <summary>
        /// 添加查询按钮
        /// </summary>
        public SysMenu(string code, string parentId) : this()
        {
            Code = code + "_query";
            Type = MenuTypeEnum.Button;
            Name = "查询";
            ParentId = parentId;
        }


        public SysMenu(string name, string parentId, int sort, string link, MenuTypeEnum menuType,
            string code, string icon, bool hidden) : this()
        {
            Name = name;
            ParentId = parentId;
            Sort = sort;
            Link = link;
            Type = menuType;
            Code = code;
            Icon = icon;
            Hidden = hidden;
        }

        #endregion

        #region 实体类

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 父菜单ID
        /// </summary>
        public string ParentId { get; private set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int Sort { get; private set; }

        /// <summary>
        /// 外链 是否为外链（有值则是外链）
        /// </summary>
        public string Link { get; private set; }

        /// <summary>
        ///菜单类型；0：目录，1：菜单，2：按钮
        /// </summary>
        public MenuTypeEnum Type { get; private set; }

        /// <summary>
        /// 权限字符串
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// 菜单图标
        /// </summary>
        public string Icon { get; private set; }

        /// <summary>
        /// 是否隐藏
        /// </summary>
        public bool Hidden { get; private set; } = false;

        #endregion

        #region 操作


        public SysMenu SetBaseInfo(string name, string parentId, int sortNumber,string link, MenuTypeEnum menuType,
            string code, string icon, bool hidden)
        {
            Name = name;
            ParentId = parentId;
            Sort = sortNumber;
            Link = link;
            Type = menuType;
            Code = code;
            Icon = icon;
            Hidden = hidden;
            return this;
        }
        #endregion
    }
}