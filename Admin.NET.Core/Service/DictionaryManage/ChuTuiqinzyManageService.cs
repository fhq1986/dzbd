// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Core.DapperEx;
using Admin.NET.Core.Entity.ElectReport;
using Admin.NET.Core.Util;
using Dapper;
using DapperExtensions.Predicate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service.DictionaryManage;
/// <summary>
/// 出退勤作业信息管理接口
/// </summary>
[NonUnify]
[ApiDescriptionSettings(Order = 390)]
public class ChuTuiqinzyManageService : IDynamicApiController, ITransient
{
    private readonly ISqlSugarClient _db;
    private readonly IDapperRepository _dap;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public ChuTuiqinzyManageService(ISqlSugarClient db, IDapperRepository dap)
    {
        _db = db;
        _dap = dap;
    }

    [Route("/api/ChuTuiqinzy/FindAll")]
    [HttpGet]
    [Authorize(Roles = "chutuiqinzy_base")]
    public dynamic FindAll(string param="")
    {
        try
        {
            //PredicateGroup predicateGroup;
            List<IConditionalModel> predicateGroup = new List<IConditionalModel>();
            IList<ISort> sorts;
            //DapperHelper.GeneratePredicateAndSort<ChuTuiqinzy>(param, out predicateGroup, out sorts);
            SqlSugarHelper.GeneratePredicateAndSort<ChuTuiqinzy>(param,out predicateGroup,out sorts);
            //var cheduixxs = IChuTuiqinzyService.FindAll(predicateGroup, sorts);
            var query = _db.Queryable<ChuTuiqinzy>();
            if (predicateGroup != null
                && predicateGroup.Count > 0)
            {
                query = query.Where(predicateGroup);
            }
            if (sorts != null
                && sorts.Count > 0)
            {
                foreach (var item in sorts)
                {
                    if (item.Ascending)
                        query = query.OrderBy($"{item.PropertyName} asc");
                    else
                        query = query.OrderBy($"{item.PropertyName} desc");
                }
            }
            var cheduixxs = query.ToList();
            return new { success = true, data = new { total = cheduixxs.Count(), items = cheduixxs } };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    //[HttpGet]
    //public dynamic GetJxByChuTuiqinzy(string dm)
    //{
    //    try
    //    {
    //        var xsxxs = IChuTuiqinzyService.GetJxByChuqinzy(dm);
    //        return new { success = true, data = xsxxs, total = xsxxs.Count() };
    //    }
    //    catch (Exception e)
    //    {
    //        return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
    //    }
    //}
    /// <summary>
    /// 添加出退勤作业信息信息
    /// </summary>
    /// <param name="cheduixx"></param>
    /// <returns></returns>
    [Route("/api/ChuTuiqinzy/Add")]
    [HttpPost]
    [Authorize(Roles = "chutuiqinzy_add")]
    public dynamic Add([FromBody] ChuTuiqinzy chuTuiqinzy)
    {
        try
        {
            //int result = IChuTuiqinzyService.Add(chuTuiqinzy);
            //IChuTuiqinzyService.Commit();
            _db.Insertable(chuTuiqinzy).ExecuteCommand();
            ChuTuiqinzy result = _db.Queryable<ChuTuiqinzy>()
                                    .Where(x => x.ZhanMingmc == chuTuiqinzy.ZhanMingmc
                                                && x.ZhanMingbm == chuTuiqinzy.ZhanMingbm
                                                && x.Ddm == chuTuiqinzy.Ddm
                                                && x.Remark == chuTuiqinzy.Remark
                                                && x.ShiJicq == chuTuiqinzy.ShiJicq
                                                && x.JiChengcq == chuTuiqinzy.JiChengcq
                                                && x.ChuKucq == chuTuiqinzy.ChuKucq
                                                && x.ShiJitq == chuTuiqinzy.ShiJitq
                                                && x.JiChengtq == chuTuiqinzy.JiChengtq
                                                && x.RuKutq == chuTuiqinzy.RuKutq
                                                && x.Cqjisuanfs == chuTuiqinzy.Cqjisuanfs
                                                && x.Tqjisuanfs == chuTuiqinzy.Tqjisuanfs).Single();
            return new { success = true, data = (result==null?0:result.Id) };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    /// <summary>
    /// 修改出退勤作业信息
    /// </summary>
    /// <param name="jxxx"></param>
    /// <returns></returns>
    [Route("/api/ChuTuiqinzy/Update")]
    [HttpPost]
    [Authorize(Roles = "chutuiqinzy_update")]
    public dynamic Update([FromBody] ChuTuiqinzy chuTuiqinzy)
    {
        try
        {
            //IChuTuiqinzyService.Update(chuTuiqinzy);
            //IChuTuiqinzyService.Commit();
            _db.Updateable(chuTuiqinzy).ExecuteCommand();
            return new { success = true };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }

    }
    /// <summary>
    /// 删除消息
    /// </summary>
    /// <param name="jxxx"></param>
    /// <returns></returns>
    [Route("/api/ChuTuiqinzy/Remove")]
    [HttpPost]
    [Authorize(Roles = "chutuiqinzy_remove")]
    public dynamic Remove([FromBody] ChuTuiqinzy chuTuiqinzy)
    {
        try
        {
            //IChuTuiqinzyService.Remove(chuTuiqinzy);
            //IChuTuiqinzyService.Commit();
            _db.Deleteable<ChuTuiqinzy>().Where(x=>x.Id==chuTuiqinzy.Id).ExecuteCommand();
            return new { success = true };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    [Route("/api/ChuTuiqinzy/Chuqinzy")]
    [HttpPost]
    [Authorize(Roles = "chutuiqines_update")]
    public dynamic Chuqinzy(string Ddm, string Cqjisuanfs, string Tqjisuanfs)
    {
        try
        {
            //var xsxxs = IChuTuiqinzyService.GetJxByChuqinzy(Ddm, Cqjisuanfs, Tqjisuanfs);
            string sql = "";
            if (Cqjisuanfs == "0")
            {
                sql = "update DIC_CHUTUIQINZY set CQJISUANFS=" + Cqjisuanfs + " where DDM='" + Ddm + "'";
            }
            else if (Cqjisuanfs == "1")
            {
                sql = "update DIC_CHUTUIQINZY set CQJISUANFS=" + Cqjisuanfs + " where DDM='" + Ddm + "'";
            }
            if (Tqjisuanfs == "0")
            {
                sql = "update DIC_CHUTUIQINZY set TQJISUANFS=" + Tqjisuanfs + " where DDM='" + Ddm + "'";
            }
            else if (Tqjisuanfs == "1")
            {
                sql = "update DIC_CHUTUIQINZY set TQJISUANFS=" + Tqjisuanfs + " where DDM='" + Ddm + "'";
            }
            //IList<ChuTuiqinzy> chuTuiqinzy = ChuTuiqinzyRepository.GetGroupByChuTuiqinzy(sql).ToList();
            IList<ChuTuiqinzy> xsxxs = _dap.Context.Query<ChuTuiqinzy>(sql).ToList();
            return new { success = true, data = xsxxs, total = xsxxs.Count() };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
}
