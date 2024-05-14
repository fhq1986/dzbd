using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Admin.NET.Core
{
    ///<summary>
    ///铁路局信息
    ///</summary>
    [SugarTable("DIC_TLJ")]
    public partial class DIC_TLJ
    {
           public DIC_TLJ(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public decimal ID {get;set;}

           /// <summary>
           /// Desc:铁路局代码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string TLJDM {get;set;}

           /// <summary>
           /// Desc:铁路局名称
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string TLJMC {get;set;}

           /// <summary>
           /// Desc:机务编码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string JWBM {get;set;}

           /// <summary>
           /// Desc:排序
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? ORD {get;set;}

    }
}
