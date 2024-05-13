
using Stark.Starter.DDD.Domain.Entities;

namespace Stark.Module.System.Domain
{
    /// <summary>
    /// 角色和菜单关联表
    /// </summary>
    public class SysRoleMenu : Entity
    {
        #region 构造函数

        public SysRoleMenu()
        {
        }

        public SysRoleMenu(string roleId, string menuId) : this()
        {
            RoleId = roleId;
            MenuId = menuId;
        }

        #endregion

        #region 实体类

        /// <summary>
        /// 角色ID
        /// </summary>
        public string RoleId { get; private set; }

        /// <summary>
        /// 菜单ID
        /// </summary>
        public string MenuId { get; private set; }

        #endregion
    }
}