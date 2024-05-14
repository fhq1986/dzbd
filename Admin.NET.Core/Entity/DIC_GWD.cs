using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Admin.NET.Core
{
    ///<summary>
    ///工务段
    ///</summary>
    [SugarTable("DIC_GWD")]
    public partial class DIC_GWD
    {
           public DIC_GWD(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public decimal ID {get;set;}

           /// <summary>
           /// Desc:工务段编码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string GWDDM {get;set;}

           /// <summary>
           /// Desc:工务段名称
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string GWDMC {get;set;}

           /// <summary>
           /// Desc:工务编码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string GWBM {get;set;}

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
