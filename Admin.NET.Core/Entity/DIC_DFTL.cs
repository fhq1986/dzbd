using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Admin.NET.Core
{
    ///<summary>
    ///地方铁路
    ///</summary>
    [SugarTable("DIC_DFTL")]
    public partial class DIC_DFTL
    {
           public DIC_DFTL(){


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
           /// Desc:顺序
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? ORD {get;set;}

    }
}
