using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Admin.NET.Core
{
    ///<summary>
    ///自定义线别
    ///</summary>
    [SugarTable("DIC_ZDYXB")]
    public partial class DIC_ZDYXB
    {
           public DIC_ZDYXB(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public decimal ID {get;set;}

           /// <summary>
           /// Desc:自定义线别编码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string ZDYXBDM {get;set;}

           /// <summary>
           /// Desc:自定义线别名称
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string ZDYXBMC {get;set;}

           /// <summary>
           /// Desc:自定义编码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string ZDYBM {get;set;}

           /// <summary>
           /// Desc:铁路局编码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string TLJDM {get;set;}

           /// <summary>
           /// Desc:排序
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? ORD {get;set;}

           /// <summary>
           /// Desc:企业代码
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? QYDM {get;set;}

    }
}
