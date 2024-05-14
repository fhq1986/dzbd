using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Admin.NET.Core
{
    ///<summary>
    ///供电段
    ///</summary>
    [SugarTable("DIC_GDD")]
    public partial class DIC_GDD
    {
           public DIC_GDD(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public decimal ID {get;set;}

           /// <summary>
           /// Desc:供电段编码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string GDDDM {get;set;}

           /// <summary>
           /// Desc:供电段名称
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string GDDMC {get;set;}

           /// <summary>
           /// Desc:供电编码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string GDBM {get;set;}

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
