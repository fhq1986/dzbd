using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Admin.NET.Core
{
    ///<summary>
    ///用户权限
    ///</summary>
    [SugarTable("USERROLES")]
    public partial class USERROLES
    {
           public USERROLES(){


           }
           /// <summary>
           /// Desc:用户ID
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public decimal USERID {get;set;}

           /// <summary>
           /// Desc:权限ID
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public decimal ROLEID {get;set;}

    }
}
