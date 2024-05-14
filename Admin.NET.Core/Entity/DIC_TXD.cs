using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Admin.NET.Core
{
    ///<summary>
    ///通讯段
    ///</summary>
    [SugarTable("DIC_TXD")]
    public partial class DIC_TXD
    {
           public DIC_TXD(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public decimal ID {get;set;}

           /// <summary>
           /// Desc:通讯段编码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string TXDDM {get;set;}

           /// <summary>
           /// Desc:通讯段名称
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string TXDMC {get;set;}

           /// <summary>
           /// Desc:通讯编码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string TXBM {get;set;}

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
