using System.ComponentModel.DataAnnotations;

namespace Stark.Module.System.Models.Requests.SysRoles
{
    public class AddRoleRequest
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 角色权限
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 角色排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [MinLength(1, ErrorMessage = "状态不能为空")]
        public string Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 菜单集合
        /// </summary>
        public List<string> MenuIds { get; set; } = new List<string>();
    }
}