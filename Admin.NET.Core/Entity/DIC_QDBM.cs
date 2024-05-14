using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Admin.NET.Core
{
    ///<summary>
    ///统计区段
    ///</summary>
    [SugarTable("DIC_QDBM")]
    public partial class DIC_QDBM
    {
           public DIC_QDBM(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public decimal ID {get;set;}

           /// <summary>
           /// Desc:区段代码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string QDDM {get;set;}

           /// <summary>
           /// Desc:区段名称(客)
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? QDMCK {get;set;}

           /// <summary>
           /// Desc:区段公里(客)
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? QDGLK {get;set;}

           /// <summary>
           /// Desc:区段名称(货)
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? QDMCH {get;set;}

           /// <summary>
           /// Desc:区段公里(货)
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? QDGLH {get;set;}

           /// <summary>
           /// Desc:分线标识
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? FXBS {get;set;}

           /// <summary>
           /// Desc:枢纽标识
           /// Default:
           /// Nullable:True
           /// </summary>           
           public bool SNBS {get;set;}

           /// <summary>
           /// Desc:线路等级
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? XLDJ {get;set;}

           /// <summary>
           /// Desc:铁路局代码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string TLJDM {get;set;}

           /// <summary>
           /// Desc:企业代码
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? QYDM {get;set;}

           /// <summary>
           /// Desc:顺序
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? ORD {get;set;}

           /// <summary>
           /// Desc:线路代码
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? XLDM {get;set;}

           /// <summary>
           /// Desc:线路性质
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? XLXZ {get;set;}

    }
}
