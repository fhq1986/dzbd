using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace Admin.NET.Core
{
    ///<summary>
    ///机务段信息
    ///</summary>
    [SugarTable("DIC_JWD")]
    public partial class DIC_JWD
    {
        public DIC_JWD()
        {


        }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        [SugarColumn(IsPrimaryKey = true)]
        public int ID { get; set; }

        /// <summary>
        /// Desc:机务段代码
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string JWDDM { get; set; }

        /// <summary>
        /// Desc:机务段名称
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string JWDMC { get; set; }

        /// <summary>
        /// Desc:机务编码
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string JWBM { get; set; }

        /// <summary>
        /// Desc:铁路局代码
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string TLJDM { get; set; }

        /// <summary>
        /// Desc:排序
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? ORD { get; set; }

        /// <summary>
        /// Desc:企业代码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? QYDM { get; set; }

        /// <summary>
        /// Desc:配属段码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? JK_PSDH { get; set; }

        /// <summary>
        /// Desc:电子报单是否走终端：1是 0否
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? ZDBS { get; set; }

        /// <summary>
        /// Desc:是否获取运统1数据：1是 0否
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? YT1BS { get; set; }

        /// <summary>
        /// Desc:列车供电是否启用:1启用 0或null不启用
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? LCGD { get; set; }

        /// <summary>
        /// Desc:打印标识:1打印 0或null不打印
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? DYBS { get; set; }

        /// <summary>
        /// Desc:IC卡转储标识:1启用 0或null不启用
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string? ICKBS { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? BFNHBS { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? AEIBS { get; set; }
    }
}
