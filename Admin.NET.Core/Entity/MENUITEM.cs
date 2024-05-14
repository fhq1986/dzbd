using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Admin.NET.Core
{
    ///<summary>
    ///菜单
    ///</summary>
    [SugarTable("MENUITEM")]
    public partial class MENUITEM
    {
           public MENUITEM(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public decimal ID {get;set;}

           /// <summary>
           /// Desc:标题
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string TITLE {get;set;}

           /// <summary>
           /// Desc:名称
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string NAME {get;set;}

           /// <summary>
           /// Desc:组件
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? COMPONENT {get;set;}

           /// <summary>
           /// Desc:图标
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? ICON {get;set;}

           /// <summary>
           /// Desc:路径
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? PATH {get;set;}

           /// <summary>
           /// Desc:权限
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? ROLES {get;set;}

           /// <summary>
           /// Desc:序号
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? ORD {get;set;}

           /// <summary>
           /// Desc:父类ID
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? PARENTID {get;set;}

           /// <summary>
           /// Desc:启用状态
           /// Default:
           /// Nullable:False
           /// </summary>           
           public bool ENABLED {get;set;}

           /// <summary>
           /// Desc:页面是否缓存
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? NOCACHE {get;set;}
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public List<MENUITEM> children { get; set; }


        /// <summary>
        /// 是否启用
        /// </summary>
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public List<MENUITEM> SubMenus { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public List<ROLES> MenuRoles { get; set; }

        /// <summary>
        /// 数据状态
        /// </summary>
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public string Status { get; set; }
    }
}
