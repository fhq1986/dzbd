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
using DapperExtensions.Predicate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service.DictionaryManage;

/// <summary>
/// 车队字典信息管理接口
/// </summary>
[NonUnify]
[ApiDescriptionSettings(Order = 390)]
public class CheDuixxManageService : IDynamicApiController, ITransient
{
    private readonly ISqlSugarClient _db;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public CheDuixxManageService(ISqlSugarClient db)
    {
        _db = db;
    }

    [Route("/api/CheDuixx/FindAll")]
    [HttpGet]
    [Authorize(Roles = "cheduixx_base")]
    public dynamic FindAll(string param="")
    {
        try
        {
            //PredicateGroup predicateGroup;
            List<IConditionalModel> predicateGroup = new List<IConditionalModel>();
            IList<ISort> sorts;
            //DapperHelper.GeneratePredicateAndSort<CheDuixx>(param, out predicateGroup, out sorts);
            SqlSugarHelper.GeneratePredicateAndSort<CheDuixx>(param, out predicateGroup, out sorts);
            //var cheduixxs = ICheDuixxService.FindAll(predicateGroup, sorts);
            var query = _db.Queryable<CheDuixx>();
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
    /// <summary>
    /// 添加车间信息
    /// </summary>
    /// <param name="cheduixxs"></param>
    /// <returns></returns>
    [Route("/api/CheDuixx/Add")]
    [HttpPost]
    [Authorize(Roles = "cheduixx_add")]
    public dynamic Add([FromBody] CheDuixx cheduixxs)
    {
        try
        {
            //int result = ICheDuixxService.Add(cheduixxs);
            //ICheDuixxService.Commit();
            //return new { success = true, data = result };
            _db.Insertable(cheduixxs).ExecuteCommand();
            CheDuixx result = _db.Queryable<CheDuixx>()
                                .Where(x => x.CheDuiName == cheduixxs.CheDuiName
                                            && x.CheDuiCode == cheduixxs.CheDuiCode
                                            && x.CheJianCode == cheduixxs.CheJianCode
                                            && x.CdRemarks == cheduixxs.CdRemarks
                                            && x.Ddm == cheduixxs.Ddm).Single();
            return new { success = true, data = (result==null?0:result.Id) };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 修改车间信息
    /// </summary>
    /// <param name="cheduixxs"></param>
    /// <returns></returns>
    [Route("/api/CheDuixx/Update")]
    [HttpPost]
    [Authorize(Roles = "cheduixx_edit")]
    public dynamic Update([FromBody] CheDuixx cheduixxs)
    {
        try
        {
            //ICheDuixxService.Update(cheduixxs);
            //ICheDuixxService.Commit();
            _db.Updateable(cheduixxs).ExecuteCommand();
            return new { success = true };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }

    }

    /// <summary>
    /// 删除车间信息
    /// </summary>
    /// <param name="cheduixxs"></param>
    /// <returns></returns>
    [Route("/api/CheDuixx/Remove")]
    [HttpPost]
    [Authorize(Roles = "cheduixx_delete")]
    public dynamic Remove([FromBody] CheDuixx cheduixxs)
    {
        try
        {
            //ICheDuixxService.Remove(cheduixxs);
            //ICheDuixxService.Commit();
            _db.Deleteable<CheDuixx>().Where(x => x.Id == cheduixxs.Id).ExecuteCommand();
            return new { success = true };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
}
