using Stark.Starter.Core;
using Stark.Starter.Core.Const;
using Stark.Starter.DDD.Domain;
using Stark.Starter.DDD.Domain.Entities;
using Volo.Abp;

namespace Stark.Module.System.Domain
{
    /// <summary>
    /// 用户对象
    /// </summary>
    public class SysUser : AggregateRoot, ISoftDelete, IEntityStatus
    {
        #region 构造函数

        public SysUser()
        {
            DeptId = string.Empty;
            PhoneNumber = string.Empty;
            Password = string.Empty;
            Salt = string.Empty;
            Avatar = string.Empty;
        }

        #endregion

        #region 实体类

        /// <summary>
        ///  部门ID
        /// </summary>
        public string DeptId { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 加密盐
        /// </summary>
        public string Salt { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 状态 Y:正常 N:禁用
        /// </summary>
        public string Status { get; set; } = StarkConst.StatusYes;

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; private set; }

        #endregion

        #region 操作

        public SysUser SetBaseInfo(string deptId, string loginName, string userName, string phoneNumber, string remark)
        {
            DeptId = deptId;
            LoginName = loginName;
            UserName = userName;
            PhoneNumber = phoneNumber;
            Remark = remark;
            return this;
        }

        /// <summary>
        /// 修改密码，每次修改密码后都更换加密盐值
        /// </summary>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public SysUser SetPassword(string password)
        {
            Salt = GetSalt();
            Password = CreatePassword(password, Salt);
            return this;
        }

        /// <summary>
        /// 重置密码，重置修改密码后更换加密盐值
        /// </summary>
        /// <returns></returns>
        public SysUser RestPassword()
        {
            Salt = GetSalt();
            Password = CreatePassword("123456", Salt);
            return this;
        }

        /// <summary>
        ///  验证密码
        /// </summary>
        /// <returns>true: 密码正确；false:密码错误</returns>
        public bool VerityPassword(string password)
        {
            password = string.Concat(password ?? "", Salt ?? "").ToMd5();
            return Password.Equals(password);
        }

        /// <summary>
        /// 生成密码
        /// </summary>
        /// <param name="password">用户输入密码</param>
        /// <param name="salt">加密盐</param>
        /// <returns></returns>
        private string CreatePassword(string password, string salt)
        {
            return string.Concat(password, salt ?? "").ToMd5();
        }

        private string GetSalt()
        {
            return RandomHelper.GetRandom(1000, 9999).ToString();
        }

        #endregion
    }
}