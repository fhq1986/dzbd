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
using AlibabaCloud.OpenApiClient.Models;
using DapperExtensions;
using DapperExtensions.Predicate;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service.StatisticManage;
/// <summary>
/// 
/// </summary>
[NonUnify]
[ApiDescriptionSettings(Order = 390)]
public class TjBdsjManageService : IDynamicApiController, ITransient
{

    private readonly ISqlSugarClient _db;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public TjBdsjManageService(ISqlSugarClient db)
    {
        _db = db;
    }
    //[Dependency]
    //public ICacheProvider CacheManager { get; set; }
    [HttpPost]
    [Route("/api/TjBdsj/Upload")]
    public dynamic Upload([FromBody] JObject param)
    {
        try
        {
            string tableName = "TJ_DAT_BDSJ";
            string ddm = param.Value<string>("Ddm");
            string xtrq = param.Value<string>("Xtrq");
            //查询日志是否锁定了
            //List<Log_sjsb> logs = LogsjsbService.Find(xtrq.Substring(0, 7), ddm).ToList();
            List<Log_sjsb> logs = _db.Queryable<Log_sjsb>()
                                    .Where(x => x.Xtrq == xtrq
                                                && x.Ddm == ddm)
                                    .ToList();
            if (logs.Count > 0)
            {
                Log_sjsb queryLog = logs.FirstOrDefault();
                if (queryLog.IsLock == "Y")
                {
                    //CacheManager.Remove("JwdUpload");
                    //CacheManager.Add("JwdUpload","Y", "SjUpload");
                    return new { success = false, message = "数据已锁定" };
                }
            }
            string data = param.Value<string>("Data");
            //提交报单数据
            //if (TjBdsjService.BulkInsert3(xtrq, ddm, data))
            if (BulkInsert3(xtrq, ddm, data))
            {
                return new { success = true };
            }
            else
            {
                return new { success = false, message = "传输失败" };
            }
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    private bool BulkInsert3(string xtrq, string ddm, string data)
    {
        var dt = DataUtil.TsvToDataTable<TjBdsj>(data);
        //var conn = (TjBdsjRepository.Context as DapperRepositoryContext).Connection;
        try
        {
            xtrq = DateTime.Parse(xtrq).ToString("yyyy-MM-dd");
            _db.Ado.BeginTran();
            //TjBdsjRepository.Context.ExecuteSqlCommand($"delete TJ_DAT_BDSJ where fqbs = to_date('{xtrq}','YYYY-MM-DD') and ddm = '{ddm}'");
            _db.Deleteable<TjBdsj>().Where(x => x.Fqbs.ToString("yyyy-MM-dd") == xtrq && x.Ddm == ddm).ExecuteCommand();
            //using (var bulk = new Z.BulkOperations.BulkOperation(conn as System.Data.Common.DbConnection))
            //{
            //    bulk.DestinationTableName = "TJ_DAT_BDSJ";
            //    bulk.BulkInsert(dt, DataRowState.Added);
            //}
            _db.Insertable<TjBdsj>(dt).ExecuteCommand();
            //Commit();
            _db.Ado.CommitTran();
        }
        catch (Exception e)
        {
            //Rollback();
            _db.Ado.RollbackTran();
            throw e;
        }
        return true;
    }

    [HttpGet]
    [DisplayName("")]
    [Route("/api/TjBdsj/FindAll")]
    public dynamic FindAll(string param)
    {
        try
        {
            //IEnumerable<TjBdsj> result = TjBdsjService.FindAll(param);
            List<TjBdsj> result;
            #region IEnumerable<TjBdsj> result = TjBdsjService.FindAll(param);
            //PredicateGroup pg;
            //IList<ISort> sorts;
            int page = 0, limit = 0;
            //if (!parseParam(param, out pg, out sorts, out page, out limit))
            //    return null;
            ////分页模式
            //if (page > 0 && limit > 0)
            //{
            //    IEnumerable<TjBdsj> aa = TjBdsjRepository.FindAll(pg, sorts, page, limit);
            //    return aa;
            //}
            //else
            //{
            //    return TjBdsjRepository.FindAll(pg, sorts);
            //}
            var query = _db.Queryable<TjBdsj>();
            if (!string.IsNullOrWhiteSpace(param + ""))
            {
                JObject jParam = JObject.Parse(param);
                if (jParam.ContainsKey("Ddm") && !string.IsNullOrEmpty(jParam.Value<string>("Ddm")))
                {
                    //pg.Predicates.Add(Predicates.Field<TjBdsj>(p => p.Ddm, Operator.Eq, jParam.Value<string>("Ddm")));
                    query = query.Where(x => x.Ddm == jParam.Value<string>("Ddm"));
                }
                if (jParam.ContainsKey("Sort") && !string.IsNullOrEmpty(jParam.Value<string>("Sort")))
                {
                    string[] ssorts = jParam.Value<string>("Sort").Split(',');
                    foreach (string ssort in ssorts)
                    {
                        string sortName = ssort.Split(' ')[0];
                        string sortType = "ASC";
                        if (ssort.Split(' ').Count() == 2)
                        {
                            sortType = ssort.Split(' ')[1];
                        }
                        //sorts.Add(new Sort { PropertyName = sortName, Ascending = sortType.Equals("ASC") });
                        query = query.OrderBy(sortName + (sortType.EqualIgnoreCase("ASC") ? " asc " : " desc "));
                    }
                }
                //分页模式
                page = 0;
                limit = 0;
                if (jParam.ContainsKey("page") && !string.IsNullOrEmpty(jParam.Value<string>("page")) &&
                    jParam.ContainsKey("limit") && !string.IsNullOrEmpty(jParam.Value<string>("limit")))
                {
                    page = jParam.Value<int>("page");
                    limit = jParam.Value<int>("limit");
                }
            }
            int totalNumber = 0;
            if (page > 0 && limit > 0)
            {
                result = query.ToPageList(page, limit, ref totalNumber);
            }
            else
            {
                result = query.ToList();
            }
            #endregion
            //if (result is PagedResult<TjBdsj>)
            //{
            //    var pagedResult = result as PagedResult<TjBdsj>;
            //    return new { success = true, message = "", data = result, total = pagedResult.TotalRecords, pages = pagedResult.TotalPages };
            //}
            //else
            //{
            //    return new { success = true, message = "", data = result, total = result.Count() };
            //}
            if (page > 0 && limit > 0)
            {
                return new { success = true, message = "", data = result, total = totalNumber, pages = (totalNumber + 10 - 1) / 10 };
            }
            else
            {
                return new { success = true, message = "", data = result, total = result.Count() };
            }
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
}
