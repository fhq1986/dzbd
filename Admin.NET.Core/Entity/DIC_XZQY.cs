using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Admin.NET.Core
{
    ///<summary>
    ///行政区域
    ///</summary>
    [SugarTable("DIC_XZQY")]
    public partial class DIC_XZQY
    {
           public DIC_XZQY(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public decimal ID {get;set;}

           /// <summary>
           /// Desc:行政区域编码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string XZQYDM {get;set;}

           /// <summary>
           /// Desc:行政区域名称
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string XZQYMC {get;set;}

           /// <summary>
           /// Desc:工务编码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string XZBM {get;set;}

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
