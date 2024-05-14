using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Admin.NET.Core
{
    ///<summary>
    ///权限
    ///</summary>
    [SugarTable("ROLES")]
    public partial class ROLES
    {
           public ROLES(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public int Id {get;set;}

           /// <summary>
           /// Desc:权限代码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Name {get;set;}

        /// <summary>
        /// Desc:权限名称
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string DisplayName { get; set; }

           /// <summary>
           /// Desc:依赖权限
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? Dependency { get;set;}

           /// <summary>
           /// Desc:菜单ID
           /// Default:
           /// Nullable:False
           /// </summary>           
           public decimal MenuId { get;set;}

           /// <summary>
           /// Desc:排序
           /// Default:
           /// Nullable:False
           /// </summary>           
           public decimal Ord { get;set; }
        /// <summary>
        /// 数据状态
        /// </summary>
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public string Status { get; set; }

    }
}
