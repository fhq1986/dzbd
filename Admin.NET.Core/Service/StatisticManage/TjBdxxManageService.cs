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
using DapperExtensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Core.DapperEx;
using Magicodes.ExporterAndImporter.Excel.Utility;
using NewLife.Xml;
using System.Net;
using System.Net.Http.Headers;

namespace Admin.NET.Core.Service.StatisticManage;
/// <summary>
/// 统计报单信息管理接口
/// </summary>
[AllowAnonymous]
[NonUnify]
[ApiDescriptionSettings(Order = 390)]
public class TjBdxxManageService : IDynamicApiController, ITransient
{
    private readonly ISqlSugarClient _db;
    private readonly IDapperRepository _dap;
    /// <summary>
    /// 统计报单信息管理接口
    /// </summary>
    /// <param name="db"></param>
    public TjBdxxManageService(ISqlSugarClient db, IDapperRepository dap)
    {
        _db = db;
        _dap = dap;
    }

    [HttpPost]
    [DisplayName("导入")]
    [Route("/api/TjBdxx/Upload")]
    public dynamic Upload([FromForm]dynamic param)
    {
        try
        {
            string tableName = "TJ_DAT_BDXX";
            string ddm = param.Value<string>("Ddm");
            string xtrq = param.Value<string>("Xtrq");
            //查询日志是否锁定了
            //List<Log_sjsb> logs = LogsjsbService.Find(xtrq.Substring(0, 7), ddm).ToList();
            List<Log_sjsb> logs = _db.Queryable<Log_sjsb>().Where(x => x.Xtrq == SqlFunc.Substring(xtrq, 0, 7)
                                                                    && x.Ddm == ddm).ToList();
            if (logs.Count > 0)
            {
                Log_sjsb queryLog = logs.FirstOrDefault();
                if (queryLog.IsLock == "Y")
                {
                    return new { success = false, message = "数据已锁定" };
                }
            }
            string data = param.Value<string>("Data");
            //提交报单数据
            //if (TjBdxxService.BulkInsert3(xtrq, ddm, data))
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
    [HttpGet]
    [Route("/api/TjBdxx/QueryCzjcgzl")]
    public dynamic QueryCzjcgzl(string param="")
    {
        try
        {
            int total, totalPage;
            //var bdxxs = TjBdxxService.QueryCzjcgzl(param, out total, out totalPage);
            IList<TjBdxx> bdxxs = QueryCzjcgzl(param, out total, out totalPage);
            return new { success = true, data = new { total = total, totalPage = totalPage, items = bdxxs } };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    [HttpGet]
    [Route("/api/TjBdxx/FindAll")]
    public dynamic FindAll(string param="")
    {
        try
        {
            //var result = TjBdxxService.FindAll(param);
            int total = 0;
            if (string.IsNullOrEmpty(param)) param = "{}";
            JObject jParam = JObject.Parse(param);
            //var conn = (TjBdxxRepository.Context as IDapperRepositoryContext).Connection;
            string sql = "select * from (select t.*,rownum rn from tj_dat_bdxx t where 1=1";
            string swhere = null;
            if (jParam.ContainsKey("Tljdm") && !string.IsNullOrEmpty(jParam.Value<string>("Tljdm")) && string.IsNullOrEmpty(jParam.Value<string>("Ddm")))
            {
                //var jwd = JwdxxService.FindAll(Predicates.Field<Jwdxx>(p => p.Tljdm, Operator.Eq, jParam.Value<string>("Tljdm")));
                List<Jwdxx> jwd = _db.Queryable<Jwdxx>().Where(x => x.Tljdm == jParam.Value<string>("Tljdm")).ToList();
                string ddm = "";
                foreach (var jwdxx in jwd)
                {
                    ddm += "'" + jwdxx.Jwddm + "',";
                }
                ddm = ddm.Substring(0, ddm.Length - 1);
                swhere += " and ddm in (" + ddm + ")";
            }
            if (jParam.ContainsKey("Jclx") && !string.IsNullOrEmpty(jParam.Value<string>("Jclx")))
            {
                string[] jclx = jParam.Value<string>("Jclx").Split(',');
                if (jclx.Count() > 1)
                {
                    swhere += " and jczl in ('" + jclx[0] + "','" + jclx[1] + "')";
                }
                else
                {
                    swhere += " and jczl='" + jParam.Value<string>("Jclx") + "'";
                }
            }
            if (jParam.ContainsKey("Jch") && !string.IsNullOrEmpty(jParam.Value<string>("Jch")))
            {
                swhere += " and Jch='" + jParam.Value<string>("Jch") + "'";
            }
            if (jParam.ContainsKey("Jx") && !string.IsNullOrEmpty(jParam.Value<string>("Jx")))
            {
                swhere += " and Jx='" + jParam.Value<string>("Jx") + "'";
            }
            if (jParam.ContainsKey("Cwy") && !string.IsNullOrEmpty(jParam.Value<string>("Cwy")))
            {
                swhere += " and (sj1='" + jParam.Value<string>("Cwy") + "' or fsj1='" + jParam.Value<string>("Cwy") + "')";
            }
            if (jParam.ContainsKey("Cc") && !string.IsNullOrEmpty(jParam.Value<string>("Cc")))
            {
                swhere += " and Cc='" + jParam.Value<string>("Cc") + "'";
            }
            if (jParam.ContainsKey("Cjdm") && !string.IsNullOrEmpty(jParam.Value<string>("Cjdm")))
            {
                swhere += " and Cjdm='" + jParam.Value<string>("Cjdm") + "'";
            }
            if (jParam.ContainsKey("Ddm") && !string.IsNullOrEmpty(jParam.Value<string>("Ddm")))
            {
                swhere += " and ddm='" + jParam.Value<string>("Ddm") + "'";
            }
            //分区标识
            if (jParam.ContainsKey("StartTime") && !string.IsNullOrEmpty(jParam.Value<string>("StartTime")))
            {
                swhere += " and fqbs between to_date('" + jParam.Value<string>("StartTime") + "','yyyy/mm/dd')";
            }
            if (jParam.ContainsKey("EndTime") && !string.IsNullOrEmpty(jParam.Value<string>("EndTime")))
            {
                swhere += " and to_date('" + jParam.Value<string>("EndTime") + "','yyyy/mm/dd')";
            }
            if (jParam.ContainsKey("Sort") && !string.IsNullOrEmpty(jParam.Value<string>("Sort")))
            {
                swhere += " order by " + jParam.Value<string>("Sort");
            }
            string sqltotal = "select count(1) from tj_dat_bdxx where 1=1" + swhere;
            //total = conn.ExecuteScalar<int>(sqltotal);
            total=_db.Ado.SqlQuery<int>(sqltotal).SingleOrDefault();
            int page = 0;
            int limit = 0;
            if (jParam.ContainsKey("page") && !string.IsNullOrEmpty(jParam.Value<string>("page")) &&
                jParam.ContainsKey("limit") && !string.IsNullOrEmpty(jParam.Value<string>("limit")))
            {
                page = jParam.Value<int>("page");
                limit = jParam.Value<int>("limit");
                swhere += ") where rn between " + ((page - 1) * limit) + " and " + (page * limit);
            }
            //var bdxxs = conn.Query<TjBdxx>(sql + swhere);
            var bdxxs = _db.Ado.SqlQuery<TjBdxx>(sql + swhere);
            //return new { success = true, message = "", data = result.bdxxs, total = result.total, pages = result.page };
            return new { success = true, message = "", data = bdxxs, total = total, pages = page };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    [HttpGet]
    [Route("/api/TjBdxx/ExportToExcel")]
    public HttpResponseMessage ExportToExcel(string param = "")
    {
        try
        {
            int allCount = 0;
            int total, totalPage;
            //var bdxxs = TjBdxxService.QueryCzjcgzl(param, out total, out totalPage);
            var bdxxs=QueryCzjcgzl(param, out total, out totalPage);
            Stream stream = new MemoryStream();
            if (Util.ExcelHelper.SaveModelDataToExcelNPOI(bdxxs.ToList<dynamic>(), JObject.Parse(HeaderCfg), stream))
            {
                stream.Seek(0, SeekOrigin.Begin);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(stream);
                response.Content.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = HttpUtility.UrlEncode("出租机车工作量" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls", System.Text.Encoding.UTF8)
                };
                response.Content.Headers.ContentLength = stream.Length;
                return response;
            }
            else
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent(JsonConvert.SerializeObject(
                            new { success = false, message = "数据导出失败！", now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }))
                };
            }
        }
        catch (Exception e)
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(
                        new { success = false, msg = ExceptionUtil.GetExceptionMsg(e), now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }))
            };
        }
    }

    private IList<TjBdxx> QueryCzjcgzl(string param, out int total, out int totalPage)
    {
        if (string.IsNullOrWhiteSpace(param))
        {
            param = "{}";
        }
        JObject jParam = JObject.Parse(param);
        string startTime = DateTime.Now.ToString("yyyy-MM-dd");
        string endTime = startTime;

        if (jParam.ContainsKey("StartTime") && !string.IsNullOrEmpty(jParam.Value<string>("StartTime")) &&
            jParam.ContainsKey("EndTime") && !string.IsNullOrEmpty(jParam.Value<string>("EndTime")))
        {
            startTime = jParam.Value<string>("StartTime");
            endTime = jParam.Value<string>("EndTime");

        }
        string predicate = $"where fqbs between to_date('{startTime}', 'yyyy-mm-dd') and to_date('{endTime}', 'yyyy-mm-dd') and sx is null";
        if (jParam.ContainsKey("Jclx") && !string.IsNullOrEmpty(jParam.Value<string>("Jclx")))
        {
            predicate += $" and jczl = '{jParam.Value<string>("Jclx")}'";
        }
        if (jParam.ContainsKey("Jx") && !string.IsNullOrEmpty(jParam.Value<string>("Jx")))
        {
            predicate += $" and jx = '{jParam.Value<string>("Jx")}'";
        }
        if (jParam.ContainsKey("Ddm") && !string.IsNullOrEmpty(jParam.Value<string>("Ddm")))
        {
            predicate += $" and ddm = '{jParam.Value<string>("Ddm")}'";

            //PredicateGroup predicateGroup = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            //predicateGroup.Predicates.Add(Predicates.Field<Cjxx>(p => p.Ddm, Operator.Eq, jParam.Value<string>("Ddm")));
            //predicateGroup.Predicates.Add(Predicates.Field<Cjxx>(p => p.Xz, Operator.Eq, "CZ"));
            //var cjxxs = CjxxService.FindAll(predicateGroup);
            var cjxxs = _db.Queryable<Cjxx>()
                            .Where(x => x.Ddm == jParam.Value<string>("Ddm")
                                        && x.Xz == "CZ").ToList();
            if (cjxxs.Count() == 0)
            {

                //如果不存在出租车间，就同时等于别外一个段代码
                predicate += $" and ddm = '{jParam.Value<string>("Ddm") + "2"}'";

            }
            else if (cjxxs.Count() == 1)
            {
                predicate += $" and cjdm = '{cjxxs.FirstOrDefault().Cjdm}'";
            }
            else
            {
                string predicateCjxx = "";
                foreach (var cjxx in cjxxs)
                {
                    if (string.IsNullOrEmpty(predicateCjxx))
                    {
                        predicateCjxx = $"cjdm = '{cjxx.Cjdm}'";
                    }
                    else
                    {
                        predicateCjxx += $" or cjdm = '{cjxx.Cjdm}'";
                    }
                }
                predicate += $" and ({predicateCjxx})";
            }
        }
        else
        {
            if (jParam.ContainsKey("Tljdm") && !string.IsNullOrEmpty(jParam.Value<string>("Tljdm")))
            {
                //var jwdxxs = JwdxxService.FindAll(Predicates.Field<Jwdxx>(p => p.Tljdm, Operator.Eq, jParam.Value<string>("Tljdm")));
                var jwdxxs = _db.Queryable<Jwdxx>().Where(x => x.Tljdm == jParam.Value<string>("Tljdm")).ToList();
                string predicateJwdxx = "";
                foreach (var jwdxx in jwdxxs)
                {
                    //PredicateGroup predicateGroup = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
                    //predicateGroup.Predicates.Add(Predicates.Field<Cjxx>(p => p.Ddm, Operator.Eq, jwdxx.Jwddm));
                    //predicateGroup.Predicates.Add(Predicates.Field<Cjxx>(p => p.Xz, Operator.Eq, "CZ"));
                    //var cjxxs = CjxxService.FindAll(predicateGroup);
                    var cjxxs = _db.Queryable<Cjxx>().Where(x => x.Ddm == jwdxx.Jwddm
                                                                && x.Xz == "CZ").ToList();
                    if (cjxxs.Count() == 0)
                    {
                    }
                    else if (cjxxs.Count() == 1)
                    {
                        if (string.IsNullOrEmpty(predicateJwdxx))
                        {
                            predicateJwdxx = $"(ddm = '{jwdxx.Jwddm}' and cjdm = '{cjxxs.FirstOrDefault().Cjdm}')";
                        }
                        else
                        {
                            predicateJwdxx += $" or (ddm = '{jwdxx.Jwddm}' and cjdm = '{cjxxs.FirstOrDefault().Cjdm}')";

                        }
                    }
                    else
                    {
                        string predicateCjxx = "";
                        foreach (var cjxx in cjxxs)
                        {
                            if (string.IsNullOrEmpty(predicateCjxx))
                            {
                                predicateCjxx = $"cjdm = '{cjxx.Cjdm}'";
                            }
                            else
                            {
                                predicateCjxx += $" or cjdm = '{cjxx.Cjdm}'";
                            }
                        }


                        if (string.IsNullOrEmpty(predicateJwdxx))
                        {
                            predicateJwdxx = $"(ddm = '{jwdxx.Jwddm}' and ({predicateCjxx}))";
                        }
                        else
                        {
                            predicateJwdxx += $" or (ddm = '{jwdxx.Jwddm}' and ({predicateCjxx}))";

                        }
                    }

                }

                predicate += $" and ({predicateJwdxx})";

            }

        }


        int page = 0;
        int limit = 0;
        if (jParam.ContainsKey("page") && !string.IsNullOrEmpty(jParam.Value<string>("page")) &&
            jParam.ContainsKey("limit") && !string.IsNullOrEmpty(jParam.Value<string>("limit")))
        {
            page = jParam.Value<int>("page");
            limit = jParam.Value<int>("limit");
        }
        string sql = $@"select count(1) cnt from (select distinct fqbs, ddm, bdh from tj_dat_bdxx {predicate})";
        //var conn = (TjBdxxRepository.Context as IDapperRepositoryContext).Connection;
        //total = conn.ExecuteScalar<int>(sql);
        total = _db.Queryable<int>(sql).Single();
        totalPage = total / limit;
        if (total % limit > 0)
        {
            totalPage += 1;
        }
        sql = $@"select * from (select t.*, rownum rn from (select 
                min(xtrq) xtrq, min(bdh) bdh, min(bdrq) bdrq, min(jczl) jczl, min(jx) jx, min (jch) jch, min(jiec) jiec, min(jiaoc) jiaoc,
                min(psd) psd, sum(rlbz) rlbz, sum(rlyl) rlyl, sum(zxgl) zxgl, sum(yhgl) yhgl, sum(dcgl) dcgl, sum(zzdglfp) zzdglfp,
                sum(zaizdglfp) zaizdglfp, sum(zzcs) zzcs, sum(qzzsj) qzzsj, sum(cyzsj) cyzsj, sum(bdtsj) bdtsj,
                sum(bztsj) bztsj, sum(wztsj) wztsj, sum(zjtsj) zjtsj, sum(dcsj) dcsj, 
                sum(czc) czc, sum(case when czc = 1 then czt else 0 end) czt, sum(czgl) czgl, sum(czt * czgl) cztgl, 
                sum(qzc) qzc, sum(case when qzc = 1 then qzt else 0 end) qzt, sum(qzgl) qzgl, sum(qzt * czgl) qztgl,
                sum(jwtc) jwtc, sum(jwts) jwts 
                from tj_dat_bdxx {predicate}
                group by fqbs, ddm, bdh order by ddm, jx, jch, jiec) t) where rn > {(page - 1) * limit} and rn <= {page * limit}";
        //var bdxx = conn.Query<TjBdxx>(sql).AsList();
        IList<TjBdxx> bdxx = _db.Queryable<TjBdxx>(sql).ToList();
        return bdxx;
    }

    private bool BulkInsert3(string xtrq, string ddm, string data)
    {
        var dt = DataUtil.TsvToDataTable<TjBdxx>(data);
        //var conn = (TjBdxxRepository.Context as DapperRepositoryContext).Connection;
        try
        {
            _db.Ado.BeginTran();
            //TjBdxxRepository.Context.ExecuteSqlCommand($"delete TJ_DAT_BDXX where fqbs = to_date('{xtrq}','YYYY-MM-DD') and ddm = '{ddm}'");
            _db.Deleteable<TjBdxx>().Where(x => x.Fqbs.ToString("yyyy-MM-dd") == SqlFunc.ToDate(xtrq).ToString("yyyy-MM-dd") && x.Ddm == ddm).ExecuteCommand();
            //using (var bulk = new Z.BulkOperations.BulkOperation(conn as System.Data.Common.DbConnection))
            //{
            //    bulk.DestinationTableName = "TJ_DAT_BDXX";
            //    bulk.BulkInsert(dt, System.Data.DataRowState.Added);
            //}
            _db.Insertable<TjBdxx>(dt).ExecuteCommand();
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

    public static string HeaderCfg
    {
        get
        {
            return @"{""header"":""出租机车工作量"",filedcount:8,rowspan:1,
	                        ""cols"":[
		                        {
			                        ""filedName"":""Xtrq"",""caption"":""系统日期"",""row"":1,""colspan"":1,""rowspan"":1
		                        },		
		                        {
			                        ""filedName"":""Bdrq"",""caption"":""报单日期"",""row"":1,""colspan"":1,""rowspan"":1
		                        },
		                        {
			                        ""filedName"":""Jx"",""caption"":""机型"",""row"":1,""colspan"":1,""rowspan"":1
		                        },			
		                        {
			                        ""filedName"":""Jch"",""caption"":""机车号"",""row"":1,""colspan"":1,""rowspan"":1
		                        },
		                        {
			                        ""filedName"":""Psd"",""caption"":""配属段"",""row"":1,""colspan"":1,""rowspan"":1
		                        },
		                        {
			                        ""filedName"":""Rlbz"",""caption"":""标准能耗"",""row"":1,""colspan"":1,""rowspan"":1
		                        },
		                        {
			                        ""filedName"":""Rlyl"",""caption"":""实际能耗"",""row"":1,""colspan"":1,""rowspan"":1
		                        },	
		                        {
			                        ""filedName"":""Zzdglfp"",""caption"":""总重吨公里"",""row"":1,""colspan"":1,""rowspan"":1
		                        },	
		                        {
			                        ""filedName"":""Zaizdglfp"",""caption"":""载重吨公里"",""row"":1,""colspan"":1,""rowspan"":1
		                        }
	                        ]
                        }";
        }
    }
}
