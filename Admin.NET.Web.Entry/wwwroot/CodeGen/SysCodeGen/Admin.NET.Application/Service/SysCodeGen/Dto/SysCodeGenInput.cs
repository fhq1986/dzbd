using Admin.NET.Core;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// 测试基础输入参数
    /// </summary>
    public class SysCodeGenBaseInput
    {
        /// <summary>
        /// AuthorName
        /// </summary>
        public virtual string? AuthorName { get; set; }
        
        /// <summary>
        /// TablePrefix
        /// </summary>
        public virtual string? TablePrefix { get; set; }
        
        /// <summary>
        /// GenerateType
        /// </summary>
        public virtual string? GenerateType { get; set; }
        
        /// <summary>
        /// ConfigId
        /// </summary>
        public virtual string? ConfigId { get; set; }
        
        /// <summary>
        /// DbName
        /// </summary>
        public virtual string? DbName { get; set; }
        
        /// <summary>
        /// DbType
        /// </summary>
        public virtual string? DbType { get; set; }
        
        /// <summary>
        /// ConnectionString
        /// </summary>
        public virtual string? ConnectionString { get; set; }
        
        /// <summary>
        /// TableName
        /// </summary>
        public virtual string? TableName { get; set; }
        
        /// <summary>
        /// NameSpace
        /// </summary>
        public virtual string? NameSpace { get; set; }
        
        /// <summary>
        /// BusName
        /// </summary>
        public virtual string? BusName { get; set; }
        
        /// <summary>
        /// MenuPid
        /// </summary>
        public virtual long MenuPid { get; set; }
        
        /// <summary>
        /// CreateUserName
        /// </summary>
        public virtual string? CreateUserName { get; set; }
        
        /// <summary>
        /// UpdateUserName
        /// </summary>
        public virtual string? UpdateUserName { get; set; }
        
    }

    /// <summary>
    /// 测试分页查询输入参数
    /// </summary>
    public class SysCodeGenInput : BasePageInput
    {
        /// <summary>
        /// 关键字查询
        /// </summary>
        public string? SearchKey { get; set; }

        /// <summary>
        /// AuthorName
        /// </summary>
        public string? AuthorName { get; set; }
        
        /// <summary>
        /// TablePrefix
        /// </summary>
        public string? TablePrefix { get; set; }
        
        /// <summary>
        /// GenerateType
        /// </summary>
        public string? GenerateType { get; set; }
        
        /// <summary>
        /// ConfigId
        /// </summary>
        public string? ConfigId { get; set; }
        
        /// <summary>
        /// DbName
        /// </summary>
        public string? DbName { get; set; }
        
        /// <summary>
        /// DbType
        /// </summary>
        public string? DbType { get; set; }
        
        /// <summary>
        /// ConnectionString
        /// </summary>
        public string? ConnectionString { get; set; }
        
        /// <summary>
        /// TableName
        /// </summary>
        public string? TableName { get; set; }
        
        /// <summary>
        /// NameSpace
        /// </summary>
        public string? NameSpace { get; set; }
        
        /// <summary>
        /// BusName
        /// </summary>
        public string? BusName { get; set; }
        
        /// <summary>
        /// MenuPid
        /// </summary>
        public long? MenuPid { get; set; }
        
        /// <summary>
        /// CreateUserName
        /// </summary>
        public string? CreateUserName { get; set; }
        
        /// <summary>
        /// UpdateUserName
        /// </summary>
        public string? UpdateUserName { get; set; }
        
    }

    /// <summary>
    /// 测试增加输入参数
    /// </summary>
    public class AddSysCodeGenInput : SysCodeGenBaseInput
    {
        /// <summary>
        /// MenuPid
        /// </summary>
        [Required(ErrorMessage = "MenuPid不能为空")]
        public override long MenuPid { get; set; }
        
    }

    /// <summary>
    /// 测试删除输入参数
    /// </summary>
    public class DeleteSysCodeGenInput : BaseIdInput
    {
    }

    /// <summary>
    /// 测试更新输入参数
    /// </summary>
    public class UpdateSysCodeGenInput : SysCodeGenBaseInput
    {
        /// <summary>
        /// Id
        /// </summary>
        [Required(ErrorMessage = "Id不能为空")]
        public long Id { get; set; }
        
    }

    /// <summary>
    /// 测试主键查询输入参数
    /// </summary>
    public class QueryByIdSysCodeGenInput : DeleteSysCodeGenInput
    {

    }
