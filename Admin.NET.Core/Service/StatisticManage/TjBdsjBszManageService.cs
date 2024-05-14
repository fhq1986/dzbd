// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Core.Entity;
using Admin.NET.Core.Entity.ElectReport;
using Admin.NET.Core.Util;
using Newtonsoft.Json.Linq;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service.StatisticManage;
/// <summary>
/// 
/// </summary>
[NonUnify]
[ApiDescriptionSettings(Order = 390)]
public class TjBdsjBszManageService : IDynamicApiController, ITransient
{
    private readonly ISqlSugarClient _db;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public TjBdsjBszManageService(ISqlSugarClient db)
    {
        _db = db;
    }

    [HttpPost]
    [DisplayName("")]
    [Route("/api/TjBdsjBsz/Upload")]
    public dynamic Upload([FromBody] string param)
    {
        try
        {
            JObject jParam = JObject.Parse(param);
            string tableName = "TJ_DAT_BDSJ_BSZ";
            string ddm = jParam.Value<string>("Ddm");
            string xtrq = jParam.Value<string>("Xtrq");
            string data = jParam.Value<string>("Data");
            //提交报单数据
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
    [DisplayName("")]
    [Route("/api/TjBdsjBsz/FindAll")]
    public dynamic FindAll(string param = "")
    {
        return null;
    }
    [HttpGet]
    [DisplayName("")]
    [Route("/api/TjBdsjBsz/GetBjJwdxx")]
    public dynamic GetList(string param = "")
    {
        try
        {
            if (string.IsNullOrEmpty(param)) param = "{}";
            JObject jParam = JObject.Parse(param);

            string ddm = "";
            //机务段
            if (jParam.ContainsKey("Ddm") && !string.IsNullOrEmpty(jParam.Value<string>("Ddm")))
            {
                ddm = jParam.Value<string>("Ddm");
            }

            string cjdm = "";
            //车间
            if (jParam.ContainsKey("Cjdm") && !string.IsNullOrEmpty(jParam.Value<string>("Cjdm")))
            {
                cjdm = jParam.Value<string>("Cjdm");
            }

            string timeType = "";
            //车间
            if (jParam.ContainsKey("timeType") && !string.IsNullOrEmpty(jParam.Value<string>("timeType")))
            {
                timeType = jParam.Value<string>("timeType");
            }

            string sTime = "";
            //车间
            if (jParam.ContainsKey("sTime") && !string.IsNullOrEmpty(jParam.Value<string>("sTime")))
            {
                sTime = jParam.Value<string>("sTime");
            }

            string zdbm = "";
            //车间
            if (jParam.ContainsKey("Zdbm") && !string.IsNullOrEmpty(jParam.Value<string>("Zdbm")))
            {
                zdbm = jParam.Value<string>("Zdbm");
            }

            var result = GetList(ddm, cjdm, zdbm, sTime, timeType);

            return new { success = true, message = "", data = result };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }

    }

    [HttpGet]
    [DisplayName("")]
    [Route("/api/TjBdsjBsz/TJ")]
    public dynamic TJ(string param = "")
    {
        try
        {
            if (string.IsNullOrEmpty(param)) param = "{}";
            JObject jParam = JObject.Parse(param);

            string ddm = "";
            //机务段
            if (jParam.ContainsKey("Ddm") && !string.IsNullOrEmpty(jParam.Value<string>("Ddm")))
            {
                ddm = jParam.Value<string>("Ddm");
            }

            string cjdm = "";
            //车间
            if (jParam.ContainsKey("Cjdm") && !string.IsNullOrEmpty(jParam.Value<string>("Cjdm")))
            {
                cjdm = jParam.Value<string>("Cjdm");
            }

            string timeType = "";
            //车间
            if (jParam.ContainsKey("timeType") && !string.IsNullOrEmpty(jParam.Value<string>("timeType")))
            {
                timeType = jParam.Value<string>("timeType");
            }

            string StartTime = "1970-12-12";
            //车间
            if (jParam.ContainsKey("StartTime") && !string.IsNullOrEmpty(jParam.Value<string>("StartTime")))
            {
                StartTime = jParam.Value<string>("StartTime");
            }

            string EndTime = "2099-12-12";
            //车间
            if (jParam.ContainsKey("EndTime") && !string.IsNullOrEmpty(jParam.Value<string>("EndTime")))
            {
                EndTime = jParam.Value<string>("EndTime");
            }

            var result = TJ(ddm, cjdm, StartTime, EndTime, timeType);

            return new { success = true, message = "", data = result };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }

    }

    private bool BulkInsert3(string xtrq, string ddm, string data)
    {
        var dt = DataUtil.TsvToDataTable<TjBdsjBSZ>(data);
        //var conn = (TjBdsjBSZRepository.Context as DapperRepositoryContext).Connection;
        try
        {
            _db.Ado.BeginTran();
            //TjBdsjBSZRepository.Context.ExecuteSqlCommand($"delete TJ_DAT_BDSJ_BSZ where xtrq = '{xtrq}' and psd = '{ddm}'");
            _db.Deleteable<TjBdsjBSZ>().Where(x => x.XTRQ == xtrq && x.PSD == ddm).ExecuteCommand();
            //using (var bulk = new Z.BulkOperations.BulkOperation(conn as System.Data.Common.DbConnection))
            //{
            //    bulk.DestinationTableName = "TJ_DAT_BDSJ_BSZ";
            //    bulk.BulkInsert(dt, DataRowState.Added);
            //}
            _db.Insertable<TjBdsjBSZ>(dt).ExecuteCommand();
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

    private List<TjBdsjBSZ_SumView> TJ(string ddm, string cjdm, string startTime, string endTime, string timeType)
    {
        string rq = "";
        string rqFlagText = "";
        if (timeType == "1")
        {
            rq = "XTRQ";
            rqFlagText = "系统日期";
        }
        else if (timeType == "2")
        {
            rq = "BDRQ";
            rqFlagText = "报单日期";
        }
        else if (timeType == "3")
        {
            rq = "SJRQ";
            rqFlagText = "实际日期";
        }
        else
        {
            rq = "XTRQ";
            rqFlagText = "系统日期";
        }


        StringBuilder sb = new StringBuilder();
        sb.AppendFormat(@"select t3.cjdm ,t3.{0} rq, t3.zdbm, t4.czmc ,  sum(case fdbz when 'Cf' then 1 else 0 end) cfsum ,sum(case fdbz when 'Dd' then 1 else 0 end) ddsum from 
(select t1.*, (case fdbz when 'Cf' then t1.cfzdbm else t1.ddzdbm end) zdbm , t2.cjmc  from TJ_DAT_BDSJ_BSZ t1 left join dic_cjxx t2 on t1.cjdm = t2.cjdm
where 1=1 ", rq);
        if (!string.IsNullOrEmpty(ddm))
        {
            sb.AppendFormat(" and t2.ddm = '{0}' ", ddm);
        }
        if (!string.IsNullOrEmpty(cjdm))
        {
            sb.AppendFormat(" and t2.cjdm = '{0}' ", cjdm);
        }
        sb.AppendFormat("and t1.{0} between '{1}' and '{2}') t3 left join DIC_CZDBM t4 on t3.zdbm = t4.dblm group by t3.cjdm ,t3.{0} , t3.zdbm , t4.czmc order by rq ", rq, startTime, endTime);

        string s = sb.ToString();

        //var list = this.TjBdsjBSZRepository.QuerySql<TjBdsjBSZ_SumView>(s);
        var list = _db.Ado.SqlQuery<TjBdsjBSZ_SumView>(s).ToList();

        foreach (var item in list)
        {
            item.RQFlag = rqFlagText;
        }


        return list;
    }

    private List<TjBdsjBSZ_View> GetList(string ddm, string cjdm, string zdbm, string sTime, string timeType)
    {
        string rq = "";
        if (timeType == "1")
        {
            rq = "XTRQ";

        }
        else if (timeType == "2")
        {
            rq = "BDRQ";

        }
        else if (timeType == "3")
        {
            rq = "SJRQ";
        }
        else
        {
            rq = "XTRQ";

        }


        StringBuilder sb = new StringBuilder();
        sb.AppendFormat(@"select t3.* , t4.czmc from 
(select t1.* ,(case fdbz when 'Cf' then t1.cfzdbm else t1.ddzdbm end) zdbm from TJ_DAT_BDSJ_BSZ t1 left join dic_cjxx t2 on t1.cjdm = t2.cjdm where 1=1 and t1.{0} = '{1}' ", rq, sTime);
        if (!string.IsNullOrEmpty(ddm))
        {
            sb.AppendFormat(" and t2.ddm = '{0}' ", ddm);
        }
        if (!string.IsNullOrEmpty(cjdm))
        {
            sb.AppendFormat(" and t2.cjdm = '{0}' ", cjdm);
        }
        sb.AppendFormat(") t3 left join DIC_CZDBM t4 on t3.zdbm = t4.dblm where t3.zdbm = '{0}' order by t3.fdbz", zdbm);
        string s = sb.ToString();
        //var list = this.TjBdsjBSZRepository.QuerySql<TjBdsjBSZ_View>(s);
        var list = _db.Ado.SqlQuery<TjBdsjBSZ_View>(s).ToList();
        foreach (var item in list)
        {
            item.CFDZ = item.FDBZ.ToLower() == "cf" ? "出发" : "到达";
        }

        return list;
    }
}
