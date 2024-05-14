using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Admin.NET.Core
{
    ///<summary>
    ///监控交路单位
    ///</summary>
    [SugarTable("DIC_JKJLDW")]
    public partial class DIC_JKJLDW
    {
           public DIC_JKJLDW(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public decimal ID {get;set;}

           /// <summary>
           /// Desc:监控交路单位代码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string JKJLDWDM {get;set;}

           /// <summary>
           /// Desc:监控交路单位名称
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string JKJLDWMC {get;set;}

           /// <summary>
           /// Desc:排序
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? ORD {get;set;}

           /// <summary>
           /// Desc:机务段代码
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? DDM {get;set;}

    }
}
