using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Admin.NET.Core
{
    ///<summary>
    ///电务段
    ///</summary>
    [SugarTable("DIC_DWD")]
    public partial class DIC_DWD
    {
           public DIC_DWD(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public decimal ID {get;set;}

           /// <summary>
           /// Desc:电务段编码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string DWDDM {get;set;}

           /// <summary>
           /// Desc:电务段名称
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string DWDMC {get;set;}

           /// <summary>
           /// Desc:电务编码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string DWBM {get;set;}

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
