using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Admin.NET.Core
{
    ///<summary>
    ///出入段地点权限
    ///</summary>
    [SugarTable("DIC_USER_CRDDD")]
    public partial class DIC_USER_CRDDD
    {
           public DIC_USER_CRDDD(){


           }
           /// <summary>
           /// Desc:用户ID
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public decimal USERID {get;set;}

           /// <summary>
           /// Desc:地点ID
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public decimal DDID {get;set;}

    }
}
