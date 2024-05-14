// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Core.Entity.ElectReport;
using Admin.NET.Core.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service.BdglManage;
/// <summary>
/// 
/// </summary>
[NonUnify]
[ApiDescriptionSettings(Order = 390)]
public class CqxxManageService : IDynamicApiController, ITransient
{
    private readonly ISqlSugarClient _db;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public CqxxManageService(ISqlSugarClient db)
    {
        _db = db;
    }

    /// <summary>
    /// 报单
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("/api/Cqxx/FindCqxxByCwydm")]
    public dynamic FindCqxxByCwydm(string param)
    {
        try
        {
            //Cqxx cqxx = CqxxService.FindCqxxByCwydm(param);
            #region Cqxx cqxx = CqxxService.FindCqxxByCwydm(param);
            //var pgMain = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            JObject jo = JObject.Parse(param);
            ////乘务员作业地点
            //if (!string.IsNullOrWhiteSpace(jo["cwzydddm"].ToString()))
            //{
            //    pgMain.Predicates.Add(Predicates.Field<Cqxx>(p => p.Cwzydddm, Operator.Eq, jo["cwzydddm"].ToString()));
            //}
            ////乘务员代码
            //if (!string.IsNullOrWhiteSpace(jo["cwydm1"].ToString()))
            //{
            //    pgMain.Predicates.Add(Predicates.Field<Cqxx>(p => p.Cwydm1, Operator.Eq, jo["cwydm1"].ToString()));
            //}
            ////没有发放二维码
            //pgMain.Predicates.Add(Predicates.Field<Cqxx>(p => p.Qrcode, Operator.Eq, null));

            //Cqxx cqxx = CqxxRepository.FindAll(pgMain).FirstOrDefault();


            var query = _db.Queryable<Cqxx>()
                            .Where(x => SqlFunc.IsNullOrEmpty(x.Qrcode))
                            .WhereIF(!SqlFunc.IsNullOrEmpty(jo["cwzydddm"] + ""), x => x.Cwzydddm == jo["cwzydddm"].ToString())
                            .WhereIF(!SqlFunc.IsNullOrEmpty(jo["cwydm1"] + ""), x => x.Cwydm1 == jo["cwydm1"].ToString());
            Cqxx cqxx = query.Single();
            #endregion
            return new { success = true, data = cqxx };
        }
        catch (Exception e)
        {
            return new { success = false };
        }
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="cqxx"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("/api/Cqxx/Add")]
    public dynamic Add([FromBody] Cqxx cqxx)
    {
        try
        {
            //int result = CqxxService.Add(cqxx);
            //CqxxService.Commit();
            int result = _db.Insertable(cqxx).ExecuteCommand();
            return new { success = true, data = result };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 更改
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("/api/Cqxx/Update")]
    public dynamic Update([FromBody]Cqxx Cqxx)
    {
        try
        {
            //CqxxService.Update(Cqxx);
            //CqxxService.Commit();
            int result = _db.Updateable(Cqxx).ExecuteCommand();
            return new { success = true, data = 1 };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
}
