using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Admin.NET.Core
{
    ///<summary>
    ///子编组数据
    ///</summary>
    [SugarTable("DAT_ZBZSJ")]
    public partial class DAT_ZBZSJ
    {
           public DAT_ZBZSJ(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public int ID {get;set;}

           /// <summary>
           /// Desc:编组类型
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? BZLX {get;set;}

           /// <summary>
           /// Desc:总重
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? ZZ {get;set;}

           /// <summary>
           /// Desc:载重
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? ZAIZ {get;set;}

           /// <summary>
           /// Desc:客车
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? KC {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? DDQY {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string? QZJG {get;set;}

           /// <summary>
           /// Desc:重车
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? ZC {get;set;}

           /// <summary>
           /// Desc:空车
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? KONGC {get;set;}

           /// <summary>
           /// Desc:非运用
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? FYYC {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? DKC {get;set;}

           /// <summary>
           /// Desc:其它
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? QT {get;set;}

           /// <summary>
           /// Desc:辆数合计
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? LS {get;set;}

           /// <summary>
           /// Desc:换长
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? HC {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int BZSJID {get;set;}

    }
}
