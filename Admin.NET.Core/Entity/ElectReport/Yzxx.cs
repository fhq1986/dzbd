using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// 油脂信息
/// </summary>
namespace DzbdJwd.Domain.Models.Zdxx
{
    [SugarTable("Dic_yzxx")]
    public class Yzxx
    {
        [SugarColumn(IsPrimaryKey = true)]
        public int Id { get; set; }
        /// <summary>
        /// 油脂名称
        /// </summary>
        public string Yzmc{ get; set; }
        /// <summary>
        /// 机务段代码
        /// </summary>
        public string Jwddm { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Ord { get; set; }
    }
}
