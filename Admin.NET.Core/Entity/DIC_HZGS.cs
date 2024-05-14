using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Admin.NET.Core
{
    ///<summary>
    ///合资公司
    ///</summary>
    [SugarTable("DIC_HZGS")]
    public partial class DIC_HZGS
    {
           public DIC_HZGS(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public decimal ID {get;set;}

           /// <summary>
           /// Desc:代码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string DM {get;set;}

           /// <summary>
           /// Desc:名称
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string MC {get;set;}

           /// <summary>
           /// Desc:铁路局代码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string TLJDM {get;set;}

           /// <summary>
           /// Desc:控股
           /// Default:
           /// Nullable:False
           /// </summary>           
           public bool KGBS {get;set;}

           /// <summary>
           /// Desc:顺序
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? ORD {get;set;}

    }
}
