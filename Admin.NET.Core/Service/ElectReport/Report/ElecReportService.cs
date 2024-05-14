// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Core.Entity.ElectReport;
using DapperExtensions.Predicate;
using DapperExtensions;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Html;
using FastReport.Web;
using FastReport;
using FastReport.Barcode;
using Admin.NET.Core.Util;
using Dapper;
using Admin.NET.Core.DapperEx;
using Microsoft.AspNetCore.SignalR.Client;
using System.Globalization;
using AutoMapper;
using Furion.Logging.Extensions;

namespace Admin.NET.Core.Service.ElectReport.Report;
[AllowAnonymous]
/// <summary>
/// 电子报单
/// </summary>
[Route("")]
public class ElecReportService : IDynamicApiController, ITransient
{
    SqlSugar.ISqlSugarClient _db;
    private readonly IDapperRepository _dapperRepository;
    IEnumerable<Xtcs> xtcss;
    public static double yt1datagettime;
    public static int yt1datagetcounts;
    public ElecReportService(SqlSugar.ISqlSugarClient db, IDapperRepository dapperRepository)
    {
        _db = db;
        _dapperRepository = dapperRepository;
    }
    #region 电子报单查询
    [NonAction]
    private bool parseParam(string param, out PredicateGroup pg, out IList<ISort> sorts, out int page, out int limit)
    {

        // 解析字符串
        if (string.IsNullOrEmpty(param)) param = "{}";
        JObject jParam = JObject.Parse(param);
        pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
        //司机代码查询，6个司机位都可以查询
        if (jParam.ContainsKey("Sjdm1") && !string.IsNullOrEmpty(jParam.Value<string>("Sjdm1")))
        {
            var pga = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
            pga.Predicates.Add(Predicates.Field<Bdsj>(p => p.Sjdm1, Operator.Eq, jParam.Value<string>("Sjdm1")));
            pga.Predicates.Add(Predicates.Field<Bdsj>(p => p.Fsjdm1, Operator.Eq, jParam.Value<string>("Sjdm1")));
            pga.Predicates.Add(Predicates.Field<Bdsj>(p => p.Xydm1, Operator.Eq, jParam.Value<string>("Sjdm1")));
            pga.Predicates.Add(Predicates.Field<Bdsj>(p => p.Sjdm2, Operator.Eq, jParam.Value<string>("Sjdm1")));
            pga.Predicates.Add(Predicates.Field<Bdsj>(p => p.Fsjdm2, Operator.Eq, jParam.Value<string>("Sjdm1")));
            pga.Predicates.Add(Predicates.Field<Bdsj>(p => p.Xydm2, Operator.Eq, jParam.Value<string>("Sjdm1")));
            pg.Predicates.Add(pga);
        }
        //车间
        if (jParam.ContainsKey("Cjdm") && !string.IsNullOrEmpty(jParam.Value<string>("Cjdm")))
        {
            pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Cjdm, Operator.Eq, jParam.Value<string>("Cjdm")));
        }
        //二维码
        if (jParam.ContainsKey("qrcode") && !string.IsNullOrEmpty(jParam.Value<string>("qrcode")))
        {
            pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Qrcode, Operator.Eq, jParam.Value<string>("qrcode")));
        }
        //处理流程
        if (jParam.ContainsKey("Proceeding") && !string.IsNullOrEmpty(jParam.Value<string>("Proceeding")))
        {
            var pga = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
            string[] sproceedings = jParam.Value<string>("Proceeding").Split(',');
            foreach (string sproceeding in sproceedings)
            {
                pga.Predicates.Add(Predicates.Field<Bdsj>(p => p.Proceeding, Operator.Eq, sproceeding));
            }
            pg.Predicates.Add(pga);
        }

        //机车统计程序完善
        if (jParam.ContainsKey("Jx") && !string.IsNullOrEmpty(jParam.Value<string>("Jx")) &&
            jParam.ContainsKey("Jch") && !string.IsNullOrEmpty(jParam.Value<string>("Jch")))
        {
            //如果机型和机车号都不为空，则查询本务或无人重联机车的报单
            var pgOr = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
            //本务
            var pgAnd1 = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            pgAnd1.Predicates.Add(Predicates.Field<Bdsj>(p => p.Jch, Operator.Eq, jParam.Value<string>("Jch").PadLeft(4, '0')));
            //无人重联
            var pgAnd2 = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            pgAnd2.Predicates.Add(Predicates.Field<Bdsj>(p => p.Jch_Wrcl, Operator.Eq, jParam.Value<string>("Jch").PadLeft(4, '0')));

            if (jParam.Value<string>("Jx") == "HXD3C" || jParam.Value<string>("Jx") == "HXD3CA")
            {
                //如果运统1运回的HXD3C或者HXD3CA错误，则这2个机型都可以查出报单数据。
                var pgJxOr1 = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
                pgJxOr1.Predicates.Add(Predicates.Field<Bdsj>(p => p.Jx, Operator.Eq, "HXD3C"));
                pgJxOr1.Predicates.Add(Predicates.Field<Bdsj>(p => p.Jx, Operator.Eq, "HXD3CA"));
                pgAnd1.Predicates.Add(pgJxOr1);

                var pgJxOr2 = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
                pgJxOr2.Predicates.Add(Predicates.Field<Bdsj>(p => p.Jx_Wrcl, Operator.Eq, "HXD3C"));
                pgJxOr2.Predicates.Add(Predicates.Field<Bdsj>(p => p.Jx_Wrcl, Operator.Eq, "HXD3CA"));
                pgAnd2.Predicates.Add(pgJxOr2);
            }
            else
            {
                pgAnd1.Predicates.Add(Predicates.Field<Bdsj>(p => p.Jx, Operator.Eq, jParam.Value<string>("Jx")));
                pgAnd2.Predicates.Add(Predicates.Field<Bdsj>(p => p.Jx_Wrcl, Operator.Eq, jParam.Value<string>("Jx")));
            }
            pgOr.Predicates.Add(pgAnd1);
            pgOr.Predicates.Add(pgAnd2);

            pg.Predicates.Add(pgOr);
        }
        else
        {
            //机型
            if (jParam.ContainsKey("Jx") && !string.IsNullOrEmpty(jParam.Value<string>("Jx")))
            {
                pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Jx, Operator.Eq, jParam.Value<string>("Jx")));
            }
            //机车号
            if (jParam.ContainsKey("Jch") && !string.IsNullOrEmpty(jParam.Value<string>("Jch")))
            {
                pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Jch, Operator.Eq, jParam.Value<string>("Jch").PadLeft(4, '0')));
            }
        }
        PredicateGroup pg2 = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
        //车次
        if ((jParam.ContainsKey("Zdcc") && !string.IsNullOrEmpty(jParam.Value<string>("Zdcc"))))
        {
            pg2.Predicates.Add(Predicates.Field<Bdsj>(p => p.Zdcc, Operator.Eq, jParam.Value<string>("Zdcc")));
            pg2.Predicates.Add(Predicates.Field<Bdsj>(p => p.Sfcc, Operator.Eq, jParam.Value<string>("Zdcc")));
            pg.Predicates.Add(pg2);
        }
        //乘务出勤地点
        if (jParam.ContainsKey("Cwcqdd") && !string.IsNullOrEmpty(jParam.Value<string>("Cwcqdd")))
        {
            pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Cwcqdddm, Operator.Eq, jParam.Value<string>("Cwcqdd")));
        }
        //乘务提交地点
        if (jParam.ContainsKey("Cwtjdd") && !string.IsNullOrEmpty(jParam.Value<string>("Cwtjdd")))
        {
            pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Bdtjdddm, Operator.Eq, jParam.Value<string>("Cwtjdd")));
        }
        //报单日期
        if (jParam.ContainsKey("StartTime") && !string.IsNullOrEmpty(jParam.Value<string>("StartTime")) &&
            jParam.ContainsKey("EndTime") && !string.IsNullOrEmpty(jParam.Value<string>("EndTime")))
        {
            pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Bdrq, Operator.Ge, jParam.Value<string>("StartTime")));
            pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Bdrq, Operator.Le, jParam.Value<string>("EndTime")));
        }
        //报单发放时间
        if (jParam.ContainsKey("BdffStartTime") && !string.IsNullOrEmpty(jParam.Value<string>("BdffStartTime")) &&
            jParam.ContainsKey("BdffEndTime") && !string.IsNullOrEmpty(jParam.Value<string>("BdffEndTime")))
        {
            pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Bdffsj, Operator.Ge, jParam.Value<string>("BdffStartTime")));
            pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Bdffsj, Operator.Le, jParam.Value<string>("BdffEndTime")));
        }
        //报单提交时间
        if (jParam.ContainsKey("BdtjStartTime") && !string.IsNullOrEmpty(jParam.Value<string>("BdtjStartTime")) &&
            jParam.ContainsKey("BdtjEndTime") && !string.IsNullOrEmpty(jParam.Value<string>("BdtjEndTime")))
        {
            pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Bdtjsj, Operator.Ge, jParam.Value<string>("BdtjStartTime")));
            if (jParam.Value<string>("BdtjEndTime").Length == 10)
            {
                pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Bdtjsj, Operator.Le, jParam.Value<string>("BdtjEndTime") + " 59:59"));
            }
            else
            {
                pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Bdtjsj, Operator.Le, jParam.Value<string>("BdtjEndTime")));
            }
        }
        ////报单回收时间
        //if (jParam.ContainsKey("BdhsStartTime") && !string.IsNullOrEmpty(jParam.Value<string>("BdhsStartTime")) &&
        //    jParam.ContainsKey("BdhsEndTime") && !string.IsNullOrEmpty(jParam.Value<string>("BdhsEndTime")))
        //{
        //    pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Bdhssj, Operator.Ge, jParam.Value<string>("BdhsStartTime")));
        //    if (jParam.Value<string>("BdhsEndTime").Length == 10)
        //    {
        //        pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Bdhssj, Operator.Le, jParam.Value<string>("BdhsEndTime") + " 59:59"));
        //    }
        //    else
        //    {
        //        pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Bdhssj, Operator.Le, jParam.Value<string>("BdhsEndTime")));
        //    }
        //}
        //报单回收时间
        if (jParam.ContainsKey("HsStartTime") && !string.IsNullOrEmpty(jParam.Value<string>("HsStartTime")))
        {
            if (jParam.ContainsKey("HsEndTime") && !string.IsNullOrEmpty(jParam.Value<string>("HsEndTime")))
            {
                pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Bdhssj, Operator.Ge, jParam.Value<string>("HsStartTime")));
                pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Bdhssj, Operator.Le, jParam.Value<string>("HsEndTime")));
            }
            else
            {
                pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Bdhssj, Operator.Eq, jParam.Value<string>("HsStartTime")));
            }
        }
        else if (jParam.ContainsKey("HsEndTime") && !string.IsNullOrEmpty(jParam.Value<string>("HsEndTime")))
        {
            pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Bdhssj, Operator.Eq, jParam.Value<string>("HsEndtTime")));
        }
        //报单回收人
        if ((jParam.ContainsKey("Bdhsr") && !string.IsNullOrEmpty(jParam.Value<string>("Bdhsr"))))
        {
            pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Bdhsr, Operator.Like, "%" + jParam.Value<string>("Bdhsr") + "%"));
            //pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Bdhsr, Operator.Like, "%" + jParam.Value<string>("Bdhsr") + "%"));
        }

        //录入段代码
        if (jParam.ContainsKey("Lrddm") && !string.IsNullOrEmpty(jParam.Value<string>("Lrddm")))
        {
            pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Lrddm, Operator.Eq, jParam.Value<string>("Lrddm")));
        }
        if (jParam.ContainsKey("Jldm") && !string.IsNullOrEmpty(jParam.Value<string>("Jldm")))
        {
            pg.Predicates.Add(Predicates.Field<CwJlxx>(p => p.Jldm, Operator.Like, "%" + jParam.Value<string>("Jldm") + "%"));
        }

        if (jParam.ContainsKey("Id") && !string.IsNullOrEmpty(jParam.Value<string>("Id")))
        {
            pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Id, Operator.Eq, jParam.Value<int>("Id")));
        }
        //统计状态
        if (jParam.ContainsKey("Tjzt"))
        {
            if (jParam.Value<string>("Tjzt") == null)
            {
                pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Tjzt, Operator.Eq, null));
            }
            if (!string.IsNullOrEmpty(jParam.Value<string>("Tjzt")))
            {
                pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Tjzt, Operator.Eq, jParam.Value<string>("Tjzt")));
            }
        }
        //统计账号
        if (jParam.ContainsKey("Tjzh"))
        {
            if (!string.IsNullOrEmpty(jParam.Value<string>("Tjzh")))
            {
                pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Tjzh, Operator.Eq, jParam.Value<string>("Tjzh")));
            }
        }
        //能耗是否异常
        if (jParam.ContainsKey("Nhsfyc"))
        {
            if (jParam.Value<string>("Nhsfyc") == null)
            {
                pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Nhsfyc, Operator.Eq, null));
            }
            if (!string.IsNullOrEmpty(jParam.Value<string>("Nhsfyc")))
            {
                pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Nhsfyc, Operator.Eq, jParam.Value<string>("Nhsfyc")));
            }
        }
        //支配段
        if (jParam.ContainsKey("Zpddm") && !string.IsNullOrEmpty(jParam.Value<string>("Zpddm")))
        {
            pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Zpddm, Operator.Eq, jParam.Value<string>("Zpddm")));
        }
        sorts = new List<ISort>();
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
                sorts.Add(new DapperExtensions.Predicate.Sort { PropertyName = sortName, Ascending = sortType.Equals("ASC") });
            }
        }
        if (sorts.Count() == 0)
        {
            sorts.Add(new DapperExtensions.Predicate.Sort { PropertyName = "Tq", Ascending = false });
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
        return true;
    }
    [NonUnify]
    [HttpGet("api/Bdcx/FindAll")]
    [Authorize(Roles = "bdcx_base")]
    public dynamic FindBdcx([FromQuery] string param)
    {
        if (string.IsNullOrEmpty(param)) param = "{}";
        JObject jParam = JObject.Parse(param);
        int page, limit;
        List<OrderByModel> orderList = OrderByModel.Create();
        page = 0;
        limit = 0;
        if (jParam.ContainsKey("page") && !string.IsNullOrEmpty(jParam.Value<string>("page")) &&
            jParam.ContainsKey("limit") && !string.IsNullOrEmpty(jParam.Value<string>("limit")))
        {
            page = jParam.Value<int>("page");
            limit = jParam.Value<int>("limit");
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
                orderList.Add(new OrderByModel { FieldName = sortName, OrderByType = sortType.Equals("ASC") ? OrderByType.Asc : OrderByType.Desc });
            }
        }
        if (orderList.Count() == 0)
        {
            orderList.Add(new OrderByModel { FieldName = "ID", OrderByType = OrderByType.Asc });
        }
        var expable = parseParam(param).ToExpression();
        IEnumerable<Bdsj> result = Enumerable.Empty<Bdsj>();
        int count = 0;
        //分页模式
        if (page > 0 && limit > 0)
        {
            var data = _db.Queryable<Bdsj>().Where(expable).OrderBy(orderList).ToPagedList(page, limit);
            count = data.Total;
            //机车统计程序调用，湖东段（V53），过滤机车号异常的
            if (!jParam.ContainsKey("Sjdm1") && jParam.ContainsKey("Zpddm") && jParam.Value<string>("Zpddm") == "V53")
            {
                result = data.Items.Where(x => x.Jchsfyc == "0" || string.IsNullOrEmpty(x.Jchsfyc));
                count = result.Count();
            }
            else
            {
                result = data.Items;

            }
        }
        else
        {
            result = _db.Queryable<Bdsj>().AS("dat_bdsj").Where(expable).OrderBy(orderList).ToList();
            count = result.Count();
        }
        return new { success = true, data = result, total = count };

    }
    [HttpGet("api/Xtcs/FindJdm")]
    //[Authorize(Roles = "xtcs_jdm")]
    public dynamic FindJdm()
    {
        var xtcses = _db.Ado.SqlQuery<Xtcs>("select cs from dic_xtcs where csdm='tljdm'");
        return xtcses[0].Cs;
    }
    [AllowAnonymous]
    [NonUnify]
    [HttpGet("api/Bdcx/Stamp")]
    public async Task<IActionResult> Stamp([FromQuery] string param)
    {
        Bdsj result = BdcxFind(param);
        foreach (var item in result.Bzsjs)
        {
            if (item.Cf != null)
            {
                item.Cf = item.Cf.Substring(11, 5);
            }
            if (item.Dd != null)
            {
                item.Dd = item.Dd.Substring(11, 5);
            }
        }
        FastReport.Utils.Config.WebMode = true;
        WebReport webReport = new WebReport();
        webReport.Toolbar.Sticky = true;
        webReport.LocalizationFile = FastReport.Utils.Config.ApplicationFolder + @"FastReports/Localization/Chinese (Simplified).frl";
        webReport.Report.Load(FastReport.Utils.Config.ApplicationFolder + @"Reports/sjbd.frx");
        webReport.Report.RegisterData(new Bdsj[] { result }, "Bdsjs", FastReport.Data.BOConverterFlags.AllowFields, 3);
        var maindata = webReport.Report.GetDataSource("Bdsjs");
        maindata.Enabled = true;
        (webReport.Report.FindObject("DataBdsj") as DataBand).DataSource = maindata;

        var bzsjdata = webReport.Report.GetDataSource("Bdsjs.Bzsjs");
        bzsjdata.Enabled = true;
        (webReport.Report.FindObject("DataBzsj") as DataBand).DataSource = bzsjdata;

        var zbzsjdata = webReport.Report.GetDataSource("Bdsjs.Bzsjs.Zbzsjs");
        zbzsjdata.Enabled = true;
        (webReport.Report.FindObject("DataZbzsj") as DataBand).DataSource = zbzsjdata;
        (webReport.Report.FindObject("BarcodeBdbh") as BarcodeObject).Text = string.IsNullOrEmpty(result.Qrcode) ? "" : result.Qrcode;
        webReport.Inline = true;
        webReport.Report.Prepare();
        HtmlString reportHtml = await webReport.Render();
        return new ContentResult { Content = reportHtml.ToString(), ContentType = "text/html" };
    }
    [NonUnify]
    [HttpGet("api/Bdcx/Find")]
    [Authorize(Roles = "bdcx_querymx")]
    public dynamic Find(string param)
    {
        Bdsj result;
        if (string.IsNullOrEmpty(param)) param = "{}";
        JObject jParam = JObject.Parse(param);
        if (jParam.ContainsKey("IsFull") && (jParam.Value<string>("IsFull") == "N"))
        {
            //专用线合并显示
            result = BdcxFind3(param);
        }
        else
        {
            //非合并显示
            result = BdcxFind(param);
        }

        if (null == result)
        {
            return new { success = false, message = $"参数{param}无法找到相应的数据！" };
        }
        else
        {
            var resultView = new BdsjView(result);
            return new { success = true, message = "", data = resultView, total = 1 };
        }
    }
    private Expressionable<Bdsj> parseParam(string param)
    {

        // 解析字符串
        if (string.IsNullOrEmpty(param)) param = "{}";
        JObject jParam = JObject.Parse(param);
        var expable = Expressionable.Create<Bdsj>();
        //司机代码查询，6个司机位都可以查询
        if (jParam.ContainsKey("Sjdm1") && !string.IsNullOrEmpty(jParam.Value<string>("Sjdm1")))
        {

            expable.And(p => p.Sjdm1 == jParam.Value<string>("Sjdm1") || p.Fsjdm1 == jParam.Value<string>("Sjdm1") || p.Xydm1 == jParam.Value<string>("Sjdm1") || p.Sjdm2 == jParam.Value<string>("Sjdm1")
            || p.Fsjdm2 == jParam.Value<string>("Sjdm1") || p.Xydm2 == jParam.Value<string>("Sjdm1"));
        }
        //车间
        if (jParam.ContainsKey("Cjdm") && !string.IsNullOrEmpty(jParam.Value<string>("Cjdm")))
        {
            expable.And(p => p.Cjdm == jParam.Value<string>("Cjdm"));
        }
        //二维码
        if (jParam.ContainsKey("qrcode") && !string.IsNullOrEmpty(jParam.Value<string>("qrcode")))
        {
            expable.And(p => p.Qrcode == jParam.Value<string>("qrcode"));
        }
        //处理流程
        if (jParam.ContainsKey("Proceeding") && !string.IsNullOrEmpty(jParam.Value<string>("Proceeding")))
        {
            var sproceedings = jParam.Value<string>("Proceeding").Split(',').Select(t => int.Parse(t)).ToList();
            expable.And(p => SqlFunc.ContainsArray(sproceedings, p.Proceeding));
        }
        //机车统计程序完善
        if (jParam.ContainsKey("Jx") && !string.IsNullOrEmpty(jParam.Value<string>("Jx")) &&
            jParam.ContainsKey("Jch") && !string.IsNullOrEmpty(jParam.Value<string>("Jch")))
        {
            string jch = jParam.Value<string>("Jch").PadLeft(4, '0');
            //如果机型和机车号都不为空，则查询本务或无人重联机车的报单
            var pgOr = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
            //本务
            var pgAnd1 = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            expable.And(p => p.Jch == jch);
            var pgAnd2 = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            expable.And(p => p.Jch_Wrcl == jch);
            if (jParam.Value<string>("Jx") == "HXD3C" || jParam.Value<string>("Jx") == "HXD3CA")
            {
                //如果运统1运回的HXD3C或者HXD3CA错误，则这2个机型都可以查出报单数据。
                expable.And(p => p.Jx == "HXD3C" || p.Jx == "HXD3CA");
                expable.And(p => p.Jx_Wrcl == "HXD3C" || p.Jx_Wrcl == "HXD3CA");
            }
            else
            {
                expable.And(p => p.Jx == jParam.Value<string>("Jx"));
                expable.And(p => p.Jx_Wrcl == jParam.Value<string>("Jx"));
            }
        }
        else
        {
            //机型
            if (jParam.ContainsKey("Jx") && !string.IsNullOrEmpty(jParam.Value<string>("Jx")))
            {
                expable.And(p => p.Jx == jParam.Value<string>("Jx"));
            }
            //机车号
            if (jParam.ContainsKey("Jch") && !string.IsNullOrEmpty(jParam.Value<string>("Jch")))
            {
                string jch = jParam.Value<string>("Jch").PadLeft(4, '0');
                expable.And(p => p.Jch == jch);
            }
        }
        PredicateGroup pg2 = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
        //车次
        if ((jParam.ContainsKey("Zdcc") && !string.IsNullOrEmpty(jParam.Value<string>("Zdcc"))))
        {
            expable.And(p => p.Zdcc == jParam.Value<string>("Zdcc"));
            expable.And(p => p.Sfcc == jParam.Value<string>("Zdcc"));
        }
        //乘务出勤地点
        if (jParam.ContainsKey("Cwcqdd") && !string.IsNullOrEmpty(jParam.Value<string>("Cwcqdd")))
        {
            expable.And(p => p.Cwcqdddm == jParam.Value<string>("Cwcqdd"));
        }
        //乘务提交地点
        if (jParam.ContainsKey("Cwtjdd") && !string.IsNullOrEmpty(jParam.Value<string>("Cwtjdd")))
        {
            expable.And(p => p.Bdtjdddm == jParam.Value<string>("Cwtjdd"));
        }
        //报单日期
        if (jParam.ContainsKey("StartTime") && !string.IsNullOrEmpty(jParam.Value<string>("StartTime")) &&
            jParam.ContainsKey("EndTime") && !string.IsNullOrEmpty(jParam.Value<string>("EndTime")))
        {
            expable.And(p => SqlFunc.ToDate(p.Bdrq) >= SqlFunc.ToDate(jParam.Value<string>("StartTime")) && SqlFunc.ToDate(p.Bdrq) <= SqlFunc.ToDate(jParam.Value<string>("EndTime")));
        }
        //报单发放时间
        if (jParam.ContainsKey("BdffStartTime") && !string.IsNullOrEmpty(jParam.Value<string>("BdffStartTime")) &&
            jParam.ContainsKey("BdffEndTime") && !string.IsNullOrEmpty(jParam.Value<string>("BdffEndTime")))
        {
            expable.And(p => SqlFunc.ToDate(p.Bdffsj) >= SqlFunc.ToDate(jParam.Value<string>("BdffStartTime")) && SqlFunc.ToDate(p.Bdffsj) <= SqlFunc.ToDate(jParam.Value<string>("BdffEndTime")));
        }
        //报单提交时间
        if (jParam.ContainsKey("BdtjStartTime") && !string.IsNullOrEmpty(jParam.Value<string>("BdtjStartTime")) &&
            jParam.ContainsKey("BdtjEndTime") && !string.IsNullOrEmpty(jParam.Value<string>("BdtjEndTime")))
        {
            expable.And(p => SqlFunc.ToDate(p.Bdtjsj) >= SqlFunc.ToDate(jParam.Value<string>("BdtjStartTime")));
            if (jParam.Value<string>("BdtjEndTime").Length == 10)
            {
                expable.And(p => SqlFunc.ToDate(p.Bdtjsj) <= SqlFunc.ToDate(jParam.Value<string>("BdtjEndTime") + " 59:59"));
            }
            else
            {
                expable.And(p => SqlFunc.ToDate(p.Bdtjsj) <= SqlFunc.ToDate(jParam.Value<string>("BdtjEndTime")));
            }
        }
        //报单回收时间
        if (jParam.ContainsKey("HsStartTime") && !string.IsNullOrEmpty(jParam.Value<string>("HsStartTime")))
        {
            if (jParam.ContainsKey("HsEndTime") && !string.IsNullOrEmpty(jParam.Value<string>("HsEndTime")))
            {
                expable.And(p => SqlFunc.ToDate(p.Bdhssj) >= SqlFunc.ToDate(jParam.Value<string>("HsStartTime")));
                expable.And(p => SqlFunc.ToDate(p.Bdhssj) <= SqlFunc.ToDate(jParam.Value<string>("HsEndTime")));
            }
            else
            {
                expable.And(p => p.Bdhssj == jParam.Value<string>("HsStartTime"));
            }
        }
        else if (jParam.ContainsKey("HsEndTime") && !string.IsNullOrEmpty(jParam.Value<string>("HsEndTime")))
        {
            expable.And(p => p.Bdhssj == jParam.Value<string>("HsEndtTime"));
        }
        //报单回收人
        if ((jParam.ContainsKey("Bdhsr") && !string.IsNullOrEmpty(jParam.Value<string>("Bdhsr"))))
        {
            expable.And(p => p.Bdhsr.Contains(jParam.Value<string>("Bdhsr")));
        }

        //录入段代码
        if (jParam.ContainsKey("Lrddm") && !string.IsNullOrEmpty(jParam.Value<string>("Lrddm")))
        {
            expable.And(p => p.Lrddm == jParam.Value<string>("Lrddm"));
        }
        //if (jParam.ContainsKey("Jldm") && !string.IsNullOrEmpty(jParam.Value<string>("Jldm")))
        //{
        //    pg.Predicates.Add(Predicates.Field<CwJlxx>(p => p.Jldm, Operator.Like, "%" + jParam.Value<string>("Jldm") + "%"));
        //    expable.And(p => p.Jldm == jParam.Value<string>("Lrddm"));
        //}

        if (jParam.ContainsKey("Id") && !string.IsNullOrEmpty(jParam.Value<string>("Id")))
        {
            expable.And(p => SqlFunc.ToString(p.Id) == jParam.Value<string>("Id"));
        }
        //统计状态
        if (jParam.ContainsKey("Tjzt"))
        {
            if (jParam.Value<string>("Tjzt") == null)
            {
                expable.And(p => SqlFunc.IsNullOrEmpty(p.Tjzt));
            }
            if (!string.IsNullOrEmpty(jParam.Value<string>("Tjzt")))
            {
                expable.And(p => p.Tjzt == jParam.Value<string>("Tjzt"));
            }
        }
        //统计账号
        if (jParam.ContainsKey("Tjzh"))
        {
            if (!string.IsNullOrEmpty(jParam.Value<string>("Tjzh")))
            {
                expable.And(p => p.Tjzh == jParam.Value<string>("Tjzh"));
            }
        }
        //能耗是否异常
        if (jParam.ContainsKey("Nhsfyc"))
        {
            if (jParam.Value<string>("Nhsfyc") == null)
            {
                expable.And(p => SqlFunc.IsNullOrEmpty(p.Nhsfyc));
            }
            if (!string.IsNullOrEmpty(jParam.Value<string>("Nhsfyc")))
            {
                expable.And(p => p.Nhsfyc == jParam.Value<string>("Nhsfyc"));
            }
        }
        //支配段
        if (jParam.ContainsKey("Zpddm") && !string.IsNullOrEmpty(jParam.Value<string>("Zpddm")))
        {
            expable.And(p => p.Zpddm == jParam.Value<string>("Zpddm"));
        }
        return expable;
    }
    [NonAction]
    public Bdsj BdcxFind(string param)
    {
        Bdsj result = null;
        IList<Bdsj> bdsjs = new List<Bdsj>();
        // 解析字符串
        if (string.IsNullOrEmpty(param)) param = "{}";
        JObject jParam = JObject.Parse(param);
        if (jParam.ContainsKey("Id") && !string.IsNullOrEmpty(jParam.Value<string>("Id")))
        {
            string sql = $@"select *
                from dat_bdsj t
                left outer join dat_clbdsj c on t.id = c.bdsjid 
                left outer join dat_bzsj b on t.id = b.bdsjid 
                left outer join dat_zbzsj z on b.id = z.bzsjid 
                left outer join dic_jkcz jk on t.zpddm = jk.ddm and t.jkjldwdm = jk.jkjldwdm and b.sjjlh=jk.jkjlh and b.czh=jk.jkczh
                left outer join tj_dic_czxx cz on jk.ddm = cz.ddm and jk.czdm=cz.tjczdm 
                where t.id = {jParam.Value<int>("Id")} order by b.bzcs";
            var list = _dapperRepository.Context.Query<Bdsj, Clbdsj, Bzsj, Zbzsj, Jkcz, TjCzxx, Bdsj>(
                       sql, (bdsj, clbdsj, bzsj, zbzsj, jkcz, czxx) =>
                       {
                           //看报单数据有没有在现有的列表中，没有则加入
                           Bdsj bdsjTmp = bdsjs.Where(m => m.Id == bdsj.Id).FirstOrDefault();
                           if (null == bdsjTmp)
                           {
                               bdsj.Clbdsjs = new List<Clbdsj>();
                               bdsj.Bzsjs = new List<Bzsj>();
                               bdsjs.Add(bdsj);
                               bdsjTmp = bdsj;
                           }
                           //看报单数据中有没有
                           if (bzsj != null)
                           {
                               Bzsj bzsjTmp = bdsjTmp.Bzsjs.Where(m => m.Id == bzsj.Id).FirstOrDefault();
                               if (null == bzsjTmp)
                               {
                                   if (jkcz != null)
                                   {
                                       bzsj.Czmc = jkcz.Jkczmc;  //得到计统车站代码
                                       bzsj.Tjczdm = jkcz.Czdm;  //得到计统车站代码
                                   }
                                   if (czxx != null) bzsj.Tjczmc = czxx.Tjczmc;    //得到计统车站名称
                                   //----之前和全前沟通的条件---
                                   //if (bzsj.Ls != null && bzsj.Zz != null && bzsj.Ls <= 1 && bzsj.Zz <= 200)
                                   //----陈燕双：2024-04-30和滑科以及全科沟通后定下来的条件----
                                   //----重车为1、总辆数为1、载重为空时我们就认为是单机----
                                   if (bzsj.Ls == 1 && bzsj.Zc == 1 && bzsj.Zaiz == null)
                                   {
                                       bzsj.Zz = null;
                                       bzsj.Zaiz = null;
                                       bzsj.Kc = null;
                                       bzsj.Ddqy = null;
                                       bzsj.Qzjg = null;
                                       bzsj.Zc = null;
                                       bzsj.Kongc = null;
                                       bzsj.Fyyc = null;
                                       bzsj.Dkc = null;
                                       bzsj.Qt = null;
                                       bzsj.Ls = null;
                                       bzsj.Hc = null;
                                   }
                                   bzsj.Zbzsjs = new List<Zbzsj>();
                                   bdsjTmp.Bzsjs.Add(bzsj);
                                   bzsjTmp = bzsj;
                               }
                               if (zbzsj != null)
                               {
                                   if (!bzsjTmp.Zbzsjs.Any(m => m.Id == zbzsj.Id))
                                   {
                                       bzsjTmp.Zbzsjs.Add(zbzsj);
                                   }
                               }
                           }
                           //看报单数据中有没有
                           if (clbdsj != null)
                           {
                               Clbdsj clbdsjTmp = bdsjTmp.Clbdsjs.Where(m => m.Id == clbdsj.Id).FirstOrDefault();
                               if (null == clbdsjTmp)
                               {
                                   bdsjTmp.Clbdsjs.Add(clbdsj);
                                   clbdsjTmp = clbdsj;
                               }
                           }
                           return null;
                       });
            foreach (Bdsj bdsj in bdsjs)
            {
                (bdsj.Bzsjs as List<Bzsj>).Sort((l, r) =>
                {
                    if (l.Bzcs > r.Bzcs)
                        return 1;
                    else if (l.Bzcs < r.Bzcs)
                        return -1;
                    else
                        return 0;
                });
                //bdsj.Bzsjs = AddZjgls(bdsj.Zpddm,bdsj.Bzsjs);
                foreach (Bzsj bzsj in bdsj.Bzsjs)
                {
                    if (bzsj.Cljc == "-")
                    {
                        bzsj.Cljczb = "摘补";
                    }
                    else
                    {
                        bzsj.Cljczb = bzsj.Cljc;
                    }
                    (bzsj.Zbzsjs as List<Zbzsj>).Sort((l, r) =>
                    {
                        if (l.Bzlx > r.Bzlx)
                            return 1;
                        else if (l.Bzlx < r.Bzlx)
                            return -1;
                        else
                            return 0;
                    });
                }
                //机车统计程序完善
                if (bdsj.Clbdsjs != null && bdsj.Clbdsjs.Count == 1)
                {
                    bdsj.Jch_Wrcl = bdsj.Clbdsjs[0].Jch;
                    bdsj.Jx_Wrcl = bdsj.Clbdsjs[0].Jx;
                    //如果本务机车和统计员录入机车不同，而且和无人重联机车号相同，则将本务和无人互换
                    if (jParam.ContainsKey("WrclJch") && !string.IsNullOrEmpty(jParam.Value<string>("WrclJch")))
                    {
                        string tjjc = jParam.Value<string>("WrclJch");
                        string bwjc = bdsj.Jx + "-" + bdsj.Jch;
                        string wrjc = bdsj.Clbdsjs[0].Jx + "-" + bdsj.Clbdsjs[0].Jch;

                        if (bwjc != tjjc && wrjc == tjjc)
                        {
                            Clbdsj cl = new Clbdsj();

                            #region 无人能耗处理
                            cl.Id = bdsj.Clbdsjs[0].Id;
                            cl.Bdsjid = bdsj.Clbdsjs[0].Bdsjid;
                            cl.Jx = bdsj.Jx;
                            cl.Jch = bdsj.Jch;
                            cl.Jsl1 = bdsj.Jsl1;
                            cl.Jrl1 = bdsj.Jrl1;
                            cl.Jcl1 = bdsj.Jcl1;
                            cl.Zsdjsl1 = bdsj.Zsdjsl1;
                            cl.Zsdjcl1 = bdsj.Zsdjcl1;
                            cl.Lcgdjsl1 = bdsj.Lcgdjsl1;
                            cl.Lcgdjcl1 = bdsj.Lcgdjcl1;
                            cl.Jsl2 = bdsj.Jsl2;
                            cl.Jrl2 = bdsj.Jrl2;
                            cl.Jcl2 = bdsj.Jcl2;
                            cl.Zsdjsl2 = bdsj.Zsdjsl2;
                            cl.Zsdjcl2 = bdsj.Zsdjcl2;
                            cl.Lcgdjsl2 = bdsj.Lcgdjsl2;
                            cl.Lcgdjcl2 = bdsj.Lcgdjcl2;
                            cl.Dwsj = bdsj.Dwsj;
                            cl.Dwyl = bdsj.Dwyl;
                            cl.Lcgdjsl1_2 = bdsj.Lcgdjsl1_2;
                            cl.Lcgdjcl1_2 = bdsj.Lcgdjcl1_2;
                            cl.Lcgdjsl2_2 = bdsj.Lcgdjsl2_2;
                            cl.Lcgdjcl2_2 = bdsj.Lcgdjcl2_2;
                            #endregion

                            #region 本务能耗处理
                            bdsj.Jx = bdsj.Clbdsjs[0].Jx;
                            bdsj.Jch = bdsj.Clbdsjs[0].Jch;
                            bdsj.Jsl1 = bdsj.Clbdsjs[0].Jsl1;
                            bdsj.Jrl1 = bdsj.Clbdsjs[0].Jrl1;
                            bdsj.Jcl1 = bdsj.Clbdsjs[0].Jcl1;
                            bdsj.Zsdjsl1 = bdsj.Clbdsjs[0].Zsdjsl1;
                            bdsj.Zsdjcl1 = bdsj.Clbdsjs[0].Zsdjcl1;
                            bdsj.Lcgdjsl1 = bdsj.Clbdsjs[0].Lcgdjsl1;
                            bdsj.Lcgdjcl1 = bdsj.Clbdsjs[0].Lcgdjcl1;
                            bdsj.Jsl2 = bdsj.Clbdsjs[0].Jsl2;
                            bdsj.Jrl2 = bdsj.Clbdsjs[0].Jrl2;
                            bdsj.Jcl2 = bdsj.Clbdsjs[0].Jcl2;
                            bdsj.Zsdjsl2 = bdsj.Clbdsjs[0].Zsdjsl2;
                            bdsj.Zsdjcl2 = bdsj.Clbdsjs[0].Zsdjcl2;
                            bdsj.Lcgdjsl2 = bdsj.Clbdsjs[0].Lcgdjsl2;
                            bdsj.Lcgdjcl2 = bdsj.Clbdsjs[0].Lcgdjcl2;
                            bdsj.Dwsj = bdsj.Clbdsjs[0].Dwsj;
                            bdsj.Dwyl = bdsj.Clbdsjs[0].Dwyl;
                            bdsj.Lcgdjsl1_2 = bdsj.Clbdsjs[0].Lcgdjsl1_2;
                            bdsj.Lcgdjcl1_2 = bdsj.Clbdsjs[0].Lcgdjcl1_2;
                            bdsj.Lcgdjsl2_2 = bdsj.Clbdsjs[0].Lcgdjsl2_2;
                            bdsj.Lcgdjcl2_2 = bdsj.Clbdsjs[0].Lcgdjcl2_2;
                            #endregion

                            bdsj.Clbdsjs[0] = cl;
                        }
                    }
                }
            }
            result = bdsjs.FirstOrDefault();

            //机车统计调用的话，更新为已提取。
            //if (jParam.ContainsKey("IsFull"))
            //{
            //    BdsjRepository.ExecuteSqlCommand("update dat_bdsj set Sfytq = '1' where id =" + jParam.Value<int>("Id"));
            //    this.Commit();
            //}

            //湖东段不要学员 ， 湖东段电表单位都为度，要除以100
            if (null != result && result.Zpddm == "V53")
            {
                result.Xyxm1 = null;
                result.Xydm1 = null;
                result.Xyxm2 = null;
                result.Xydm2 = null;

                /*if (result.Jcl1 != null)
                {
                    result.Jcl1 = Convert.ToInt32(Math.Round((result.Jcl1.Value + 0.00d) / 100));
                }
                if (result.Jcl2 != null)
                {
                    result.Jcl2 = Convert.ToInt32(Math.Round((result.Jcl2.Value + 0.00d) / 100));
                }
                if (result.Jsl1 != null)
                {
                    result.Jsl1 = Convert.ToInt32(Math.Round((result.Jsl1.Value + 0.00d) / 100));
                }
                if (result.Jsl2 != null)
                {
                    result.Jsl2 = Convert.ToInt32(Math.Round((result.Jsl2.Value + 0.00d) / 100));
                }
                if (result.Zsdjcl1 != null)
                {
                    result.Zsdjcl1 = Convert.ToInt32(Math.Round((result.Zsdjcl1.Value + 0.00d) / 100));
                }
                if (result.Zsdjcl2 != null)
                {
                    result.Zsdjcl2 = Convert.ToInt32(Math.Round((result.Zsdjcl2.Value + 0.00d) / 100));
                }
                if (result.Zsdjsl1 != null)
                {
                    result.Zsdjsl1 = Convert.ToInt32(Math.Round((result.Zsdjsl1.Value + 0.00d) / 100));
                }
                if (result.Zsdjsl2 != null)
                {
                    result.Zsdjsl2 = Convert.ToInt32(Math.Round((result.Zsdjsl2.Value + 0.00d) / 100));
                }*/

            }

            if (null != result && (result.Zpddm == "V46" || result.Zpddm == "V53"))
            {
                result.Lcgdjcl1 = null;
                result.Lcgdjsl1 = null;
                result.Lcgdjsl2 = null;
                result.Lcgdjcl2 = null;
                result.Lcgdjcl1_2 = null;
                result.Lcgdjsl1_2 = null;
                result.Lcgdjcl2_2 = null;
                result.Lcgdjsl2_2 = null;
            }
        }
        return result;
    }
    [NonAction]
    public Bdsj BdcxFind3(string param)
    {

        var bdsj = BdcxFind(param);

        //找到编组数据的监控车站
        string sql = string.Format(@"select jk.* from dat_bdsj t inner join  dat_bzsj b on t.id = b.bdsjid inner join dic_jkcz jk on t.zpddm = jk.ddm and t.jkjldwdm = jk.jkjldwdm and b.sjjlh=jk.jkjlh and b.czh=jk.jkczh
            where t.id = {0}", bdsj.Id);
        var jkList = _db.Ado.SqlQuery<Jkcz>(sql).ToList();

        //过滤监控车站为空的编组
        bdsj.Bzsjs = bdsj.Bzsjs.Where(x => jkList.FirstOrDefault(y => y.Ddm == bdsj.Zpddm && y.Jkjldwdm == bdsj.Jkjldwdm && y.Jkjlh == x.Sjjlh && y.Jkczh == x.Czh) != null).ToList();

        var bzList = bdsj.Bzsjs;    //编组数据
                                    //找到第一个专用线车站
        var firstZyxIndex = -1;
        for (var i = 0; i < bzList.Count; i++)
        {
            var bz = bzList[i];
            var jkcz = jkList.FirstOrDefault(x => x.Ddm == bdsj.Zpddm && x.Jkjldwdm == bdsj.Jkjldwdm && x.Jkjlh == bz.Sjjlh && x.Jkczh == bz.Czh);
            if (jkcz.Zyxbs == "1")  //是专用线车站
            {
                firstZyxIndex = i;
                break;
            }
        }

        //找到最后一个专用线车站
        var lastZyxIndex = -1;
        for (var j = bzList.Count - 1; j >= 0; j--)
        {
            var bz = bzList[j];
            var jkcz = jkList.FirstOrDefault(x => x.Ddm == bdsj.Zpddm && x.Jkjldwdm == bdsj.Jkjldwdm && x.Jkjlh == bz.Sjjlh && x.Jkczh == bz.Czh);
            if (jkcz == null || jkcz.Zyxbs == "1")  //是专用线车站，或者没找到（没找到的认为是专用线车站）
            {
                lastZyxIndex = j;
                break;
            }
        }

        if (firstZyxIndex == -1 && lastZyxIndex == -1) return bdsj;     //没有专用线，返回
        if (firstZyxIndex == 0 && lastZyxIndex == bzList.Count - 1) return bdsj;    //全是专用线，返回

        var bzsjZyx = new Bzsj();   //构造新的编组数据   
        Bzsj bzGy = null;
        if (firstZyxIndex == 0)
        {
            //如果第一个车站就是专用线车站（新高山调-新高山-湖东）
            bzGy = bzList[lastZyxIndex + 1];    //下面的第一个国有站
                                                //bzsjZyx.Tjczmc = bzGy.Tjczmc + "调";
        }
        else
        {
            //最后一个车站是专用线车站（湖东-新高山-新高山调）
            //或者 （湖东-新高山-新高山调-新高山-湖东）
            bzGy = bzList[firstZyxIndex - 1];    //上面的第一个国有站
                                                 //bzsjZyx.Tjczmc = bzGy.Tjczmc + "调";
        }

        //国有站点bzGy的统计车站信息
        var tjcz_bzGy = _db.Ado.SqlQuery<TjCzxx>(string.Format(@" select * from TJ_DIC_CZXX where ddm = '{0}' and tjczdm = '{1}'", bdsj.Zpddm, bzGy.Tjczdm)).FirstOrDefault();
        if (tjcz_bzGy == null) tjcz_bzGy = new TjCzxx();
        bzsjZyx.Tjczmc = tjcz_bzGy.Tjczmc + "调";

        //找到调车地点，更新编组数据
        //var dcdd = Dcdd2Repository.FindAll(bdsj.Zpddm).FirstOrDefault(x => x.Dcddmc == bzsjZyx.Czmc);
        var dcdd = _db.Ado.SqlQuery<TjCzxx>(string.Format(@" select * from TJ_DIC_CZXX where ddm = '{0}' and tjczmc = '{1}'", bdsj.Zpddm, bzsjZyx.Tjczmc)).FirstOrDefault();
        if (dcdd != null)
        {
            bzsjZyx.Tjczdm = dcdd.Tjczdm;
        }

        //出发，到达，调车时间
        if (firstZyxIndex == 0)
        {
            //（新高山调-新高山-湖东）
            //第一站专用线的出发时间作为到达时间（第一站没有到达时间），下一个国有站的出发时间作为出发时间                
            bzsjZyx.Dd = !string.IsNullOrEmpty(bzList[firstZyxIndex].Cf) ? bzList[firstZyxIndex].Cf : bzList[firstZyxIndex].Dd;
            bzsjZyx.Cf = !string.IsNullOrEmpty(bzList[lastZyxIndex + 1].Cf) ? bzList[lastZyxIndex + 1].Cf : bzList[lastZyxIndex + 1].Dd;
        }
        else if (lastZyxIndex == bzList.Count - 1)
        {
            //（湖东-新高山-新高山调）                
            //第一站专用线的上一个国有站的到达时间作为到达时间，最后一站专用线的到达时间作为出发时间（最后一站没有出发时间）
            bzsjZyx.Dd = !string.IsNullOrEmpty(bzList[firstZyxIndex - 1].Dd) ? bzList[firstZyxIndex - 1].Dd : bzList[firstZyxIndex - 1].Cf;
            bzsjZyx.Cf = !string.IsNullOrEmpty(bzList[lastZyxIndex].Dd) ? bzList[lastZyxIndex].Dd : bzList[lastZyxIndex].Cf;
        }
        else
        {
            //（湖东-新高山-新高山调-新高山-湖东）
            //第一站专用线的上一个国有站的到达时间作为到达时间,最后一站专用线的下一个国有站的出发时间作为出发时间
            bzsjZyx.Dd = !string.IsNullOrEmpty(bzList[firstZyxIndex - 1].Dd) ? bzList[firstZyxIndex - 1].Dd : bzList[firstZyxIndex - 1].Cf;
            bzsjZyx.Cf = !string.IsNullOrEmpty(bzList[lastZyxIndex + 1].Cf) ? bzList[lastZyxIndex + 1].Cf : bzList[lastZyxIndex + 1].Dd;
        }

        if (!(string.IsNullOrEmpty(bzsjZyx.Dd) || string.IsNullOrEmpty(bzsjZyx.Cf)))
        {
            //出发到达都不为空，计算调车时间
            var tSpan = Convert.ToDateTime(bzsjZyx.Cf).Subtract(Convert.ToDateTime(bzsjZyx.Dd));
            bzsjZyx.Dcsj = tSpan.Hours.ToString().PadLeft(2, '0') + ":" + tSpan.Minutes.ToString().PadLeft(2, '0');
        }


        //修改专用线下一个国有线站点车次等数据
        if (!(lastZyxIndex == bzList.Count - 1))
        {
            //（湖东-新高山-新高山调-新高山-湖东）
            //或者 （新高山调-新高山-湖东）
            //需要修改下一个国有站点车次，总重等数据（车次，总重已经在专用线站点里更改了）
            Bzsj bzChange = null;
            for (var i = lastZyxIndex; i >= firstZyxIndex; i--)
            {
                var temp = bzList[i];
                if (!string.IsNullOrEmpty(temp.Cc) && temp.Zaiz != null)    //有车次 和 载重的
                {
                    bzChange = temp;
                    break;
                }
            }

            if (bzChange != null)
            {
                //更新载重，车次等
                bzList[lastZyxIndex + 1].Cc = bzChange.Cc;
                bzList[lastZyxIndex + 1].Zz = bzChange.Zz;
                bzList[lastZyxIndex + 1].Zaiz = bzChange.Zaiz;
                bzList[lastZyxIndex + 1].Zc = bzChange.Zc;
                bzList[lastZyxIndex + 1].Ls = bzChange.Ls;
                bzList[lastZyxIndex + 1].Hc = bzChange.Hc;
            }
        }

        //删除专用线编组，插入新编组
        for (var i = lastZyxIndex; i >= firstZyxIndex; i--)
        {
            bzList.RemoveAt(i);
        }
        bzList.Insert(firstZyxIndex, bzsjZyx);

        //清空新编组上一站国有站的出发时间，下一站国有站的到达时间（？？？）
        if (firstZyxIndex > 0)
        {
            if (string.IsNullOrEmpty(bzList[firstZyxIndex - 1].Dd))
            {
                bzList[firstZyxIndex - 1].Dd = bzList[firstZyxIndex - 1].Cf;
            }

            bzList[firstZyxIndex - 1].Cf = null;
        }
        if (firstZyxIndex < bzList.Count - 1)
        {
            if (string.IsNullOrEmpty(bzList[firstZyxIndex + 1].Cf))
            {
                bzList[firstZyxIndex + 1].Cf = bzList[firstZyxIndex + 1].Dd;
            }

            bzList[firstZyxIndex + 1].Dd = null;
        }

        return bdsj;
    }
    #endregion

    #region 电子报单回收
    /// <summary>
    /// 查询需要回收的报单数据
    /// </summary>
    /// <returns></returns>
    [HttpGet("api/Bdhs/FindAll")]
    [Authorize(Roles = "bdhs_queryneedhs")]
    [NonUnify]
    public dynamic FindBdhs(string param)
    {
        // 解析字符串
        if (string.IsNullOrEmpty(param)) param = "{}";
        JObject jParam = JObject.Parse(param);
        string shzt = jParam["SHZT"].ToString();  //0 未回收 1已回收
        var userInfo = App.User.Claims;
        var userName = App.User?.FindFirst(ClaimConst.Account)?.Value;
        var user = _db.Ado.SqlQuery<SysUserInfo>("select Jwddm from users where username=@UserName", new { UserName = userName }).FirstOrDefault();
        string strWhere = "";
        if (shzt == "0")
        {
            Jwdxx jwdxx = _db.Queryable<Jwdxx>().First(t => t.Jwddm == user.Jwddm);
            if (null != jwdxx)
            {
                var cwyzydds = _db.Ado.SqlQuery<Dmmc>(@"select u.cwzydddm dm,dd.cwzyddmc mc from usercwzydd u 
                                                left join dic_cwzydd dd on u.cwzydddm=dd.cwzydddm where u.username=@UserName", new { UserName = userName });
                if (cwyzydds == null || cwyzydds.Count == 0) return null;

                if (!string.IsNullOrEmpty(jParam.Value<string>("StartTime")))
                {
                    if (!string.IsNullOrEmpty(jParam.Value<string>("EndTime")))
                    {
                        strWhere += string.Format(" and to_char(to_date(bdrq,'yyyy/mm/dd hh24:mi:ss'),'yyyy-mm-dd') between '{0}' and '{1}'", jParam.Value<string>("StartTime").ToString(), jParam.Value<string>("EndTime").ToString());
                    }
                    else
                    {
                        strWhere += string.Format(" and to_char(to_date(bdrq,'yyyy/mm/dd hh24:mi:ss'),'yyyy-mm-dd')='{0}'", jParam.Value<string>("StartTime").ToString());
                    }
                }
                else if (!string.IsNullOrEmpty(jParam.Value<string>("EndTime")))
                {
                    strWhere += string.Format(" and to_char(to_date(bdrq,'yyyy/mm/dd hh24:mi:ss'),'yyyy-mm-dd')='{0}'", jParam.Value<string>("EndTime").ToString());
                }

                if (jParam.ContainsKey("Zdcc") && !string.IsNullOrEmpty(jParam.Value<string>("Zdcc")))
                    strWhere += " and (Zdcc='" + jParam.Value<string>("Zdcc") + "' or Sfcc='" + jParam.Value<string>("Zdcc") + "')";
                if (jParam.ContainsKey("Jch") && !string.IsNullOrEmpty(jParam.Value<string>("Jch")))
                    strWhere += " and Jch='" + jParam.Value<string>("Jch") + "'";

                if (jParam.ContainsKey("Zpddm") && !string.IsNullOrEmpty(jParam.Value<string>("Zpddm")))
                    strWhere += " and Zpddm='" + jParam.Value<string>("Zpddm") + "'";

                if (jParam.ContainsKey("Sjdm") && !string.IsNullOrEmpty(jParam.Value<string>("Sjdm")))
                {
                    //只查询当前搜索乘务员所有未回收报单。
                    strWhere += " and (sjdm1='" + jParam.Value<string>("Sjdm") + "' or sjdm2='" + jParam.Value<string>("Sjdm") + "' or fsjdm1='" + jParam.Value<string>("Sjdm") + "'or fsjdm2='" + jParam.Value<string>("Sjdm") + "'or xydm1='" + jParam.Value<string>("Sjdm") + "'or xydm2='" + jParam.Value<string>("Sjdm") + "')";
                }
                else
                {
                    //走终端，必须是乘务员在终端提交的报单，才能够回收。
                    //if (jwdxx.Zdbs == "1")
                    //{
                    string dms = "";
                    foreach (var cwzydd in cwyzydds)
                    {
                        if (dms == "")
                            dms = "'" + cwzydd.Dm + "'";
                        else
                            dms += ",'" + cwzydd.Dm + "'";
                    }

                    if (!string.IsNullOrWhiteSpace(dms))
                    {
                        strWhere += " and Bdtjdddm in (" + dms + ")";
                    }
                }
                string sql = @"select * from dat_bdsj where Proceeding=3 " + strWhere + " order by bdtjsj desc";
                IList<Bdsj> bdsjs = _db.Ado.SqlQuery<Bdsj>(sql);
                return bdsjs.Count() == 0 ? "null" : new { success = true, data = bdsjs, total = bdsjs.Count() };
            }
            else
                return "null";
        }
        else
        {
            //只查询当前搜索乘务员所有未回收报单
            if (jParam.ContainsKey("StartSHSJ") && !string.IsNullOrEmpty(jParam.Value<string>("StartSHSJ")))
            {
                var val = DateTime.Parse(jParam.Value<string>("StartSHSJ")).ToString("yyyy-MM-dd HH:mm:00");
                strWhere = " and bdhssj >= '" + val + "'";
            }
            string sql = @"select * from dat_bdsj where Proceeding=4 and bdhsr='" + userName + "'" + strWhere + " order by bdhssj desc";
            IList<Bdsj> bdsjs = _db.Ado.SqlQuery<Bdsj>(sql);
            return bdsjs.Count() == 0 ? "null" : new { success = true, data = bdsjs, total = bdsjs.Count() };
        }
    }
    #endregion

    #region 电子报单回收台账
    /// <summary>
    /// 报表类型获取开始结束时间
    /// </summary>
    /// <param name="start">开始时间</param>
    /// <param name="end">结束时间</param>
    /// <param name="sType">报表类型</param>
    /// <returns></returns>
    [NonAction]
    public string GeTimeByBblx(ref string sStart, ref string sEnd, ref string sType)
    {
        DateTime time = DateTime.Parse(sStart);
        string sBbrq = string.Empty;
        switch (sType.ToUpper())
        {
            case "RB":
                sEnd = sStart;
                sBbrq = time.ToString("yyyy年MM月dd日");
                sType = "日报";
                break;
            case "XB":
                if (time.Day < 11)
                {
                    sStart = time.AddDays(1 - time.Day).ToString("yyyy-MM-dd");
                    sEnd = new DateTime(time.Year, time.Month, 10).ToString("yyyy-MM-dd");
                    sBbrq = string.Concat(time.ToString("yyyy年MM月"), "上旬");
                }
                else if (time.Day >= 11 && time.Day < 21)
                {
                    sStart = new DateTime(time.Year, time.Month, 11).ToString("yyyy-MM-dd");
                    sEnd = new DateTime(time.Year, time.Month, 20).ToString("yyyy-MM-dd");
                    sBbrq = string.Concat(time.ToString("yyyy年MM月"), "中旬");
                }
                else
                {
                    sBbrq = string.Concat(time.ToString("yyyy年MM月"), "下旬");
                    sStart = new DateTime(time.Year, time.Month, 21).ToString("yyyy-MM-dd");
                    sEnd = new DateTime(time.AddMonths(1).Year, time.AddMonths(1).Month, 1).AddDays(-1).ToString("yyyy-MM-dd");
                }
                sType = "旬报";
                break;
            case "YB":
                sStart = time.AddDays(1 - time.Day).ToString("yyyy-MM-dd");
                sEnd = Convert.ToDateTime(sStart).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                sBbrq = time.ToString("yyyy年MM月");
                sType = "月报";
                break;
            case "JB":
                sStart = time.AddMonths(0 - (time.Month - 1) % 3).AddDays(1 - time.Day).ToString("yyyy-MM-dd");
                sEnd = Convert.ToDateTime(sStart).AddMonths(3).AddDays(-1).ToString("yyyy-MM-dd");
                sBbrq = string.Concat(time.ToString("yyyy年"), time.Month / 3 + 1, "季");
                sType = "季报";
                break;
            case "NB":
                sStart = sStart.Substring(0, 4) + "-01-01";
                sEnd = sStart.Substring(0, 4) + "-12-31";
                sBbrq = time.ToString("yyyy年");
                sType = "年报";
                break;
            case "DZ":
                sType = "定制";
                DateTime time1 = DateTime.Parse(sEnd);
                sBbrq = time.ToString("yyyy年MM月dd日") + "-" + time1.ToString("yyyy年MM月dd日");
                break;
            case "DZ2":
                sType = "定制";
                sBbrq = time.ToString("yyyy年MM月dd日HH时mm分") + "-" + DateTime.Parse(sEnd).ToString("yyyy年MM月dd日HH时mm分");
                break;
            case "RL":
                sBbrq = string.Concat(time.ToString("yyyy年MM月dd日"), "～", Convert.ToDateTime(sEnd).ToString("yyyy年MM月dd日"));
                sType = "日累";
                break;
            case "YL":
                sBbrq = string.Concat(time.ToString("yyyy年MM月dd日"), "～", Convert.ToDateTime(sEnd).ToString("yyyy年MM月dd日"));
                sType = "月累";
                break;
            default:
                sBbrq = string.Concat(time.ToString("yyyy年MM月dd日"), "～", Convert.ToDateTime(sEnd).ToString("yyyy年MM月dd日"));
                sType = "年累";
                break;
        }
        return sBbrq;
    }
    [NonAction]
    public IList<ReportBdsj> GetHsReport(string zpddm, string cjdm, string hsr, string tjzh, string tjzt, string sDate, string eDate)
    {
        var list = new List<ReportBdsj>();
        try
        {

            if (string.IsNullOrWhiteSpace(hsr))
                hsr = " and t1.bdhsr is not null ";
            else
                hsr = " and (t1.bdhsr = '" + hsr + "' or  t2.DISPLAYNAME = '" + hsr + "') ";


            if (string.IsNullOrWhiteSpace(zpddm))
                zpddm = "";
            else
                zpddm = " and t1.zpddm = '" + zpddm + "' ";

            if (string.IsNullOrWhiteSpace(cjdm))
                cjdm = "";
            else
                cjdm = " and upper(t4.cjdm) = upper('" + cjdm + "') ";

            if (null == tjzt)
                tjzt = " and t1.tjzh is null ";
            else if (tjzt == "")
                tjzt = "";
            else
                tjzt = " and t1.tjzt = '" + tjzt + "' ";

            if (string.IsNullOrWhiteSpace(tjzh))
                tjzh = "";
            else
                tjzh = " and t1.tjzh = '" + tjzh + "' ";

            string sHsTime = "";
            if (!string.IsNullOrWhiteSpace(sDate))
            {
                if (!string.IsNullOrWhiteSpace(eDate))
                {
                    sHsTime = string.Format(" and to_char(to_date(t1.bdhssj,'yyyy-MM-dd hh24:mi:ss'),'yyyy-MM-dd hh24:mi') between '{0}' and '{1}'", sDate, eDate);
                }
                else
                {
                    sHsTime = string.Format(" and to_char(to_date(t1.bdhssj,'yyyy-MM-dd hh24:mi:ss'),'yyyy-MM-dd hh24:mi')='{0}'", sDate);
                }
            }
            else
            {
                sHsTime = string.Format(" and to_char(to_date(t1.bdhssj,'yyyy-MM-dd hh24:mi:ss'),'yyyy-MM-dd hh24:mi')='{0}'", eDate);
            }

            //                string upsql = string.Format(@"SELECT (CASE WHEN SHZT = '1' THEN '通过' WHEN SHZT='0' THEN '不通过' END) SHZT,
            //ROWNUM XH,SJDM1,SJXM1,JX || '-' || JCH JCH,BDRQ,'' QRCODE FROM YT1_DAT_BDSJ
            //WHERE SHSJ BETWEEN '{2}:00' AND '{3}:59' and shzt in ({4}) {0} {1} order by SHZT DESC,JX,JCH,BDRQ desc,Cq desc", auditUser , zpddm, sDate , eDate , shzt);

            //string upsql = string.Format(@"SELECT 
            //            ROWNUM XH,t1.SJDM1,t1.SJXM1,t1.XYDM1,t1.XYXM1,t1.JX || '-' || t1.JCH JCH,t1.BDRQ,'' QRCODE ,t2.DISPLAYNAME,
            //            (case when t1.FSJDM1 is null then t1.sjdm2 else t1.FSJDM1 end) FSJDM1, 
            //            (case when t1.FSJXM1 is null then t1.sjxm2 else t1.FSJXM1 end) FSJXM1 , 
            //            (case when t1.CBD is null then substr(t1.CWD,6) else substr(t1.CBD,6) end) CK,
            //            (case when t1.RBD is null then substr(t1.RWD,6) else substr(t1.RBD,6) end) RK,
            //            substr(t1.CQ,6) CQ,substr(t1.JIEC,6) JIEC,substr(t1.TQ,6) TQ,t1.SFCZMC , t1.ZDCZMC ,t1.bdhssj,t4.cjmc,
            //            (case when t1.TJZT = '2' then '已统计' when t1.TJZT = '1' then '收集' when t1.TJZT = '3' then '暂缓统计'  when t1.TJZT = '4' then '手动统计' else '未处理' end) TJMC
            //            FROM dat_bdsj t1 left join users t2 on t1.bdhsr = t2.username 
            //            left join dic_cwyxx t3 on t1.sjdm1=t3.cwydm left join DIC_CJXX t4 on t3.ddm = t4.ddm and t3.cjdm = t4.cjdm 
            //        WHERE t1.proceeding = 4 and t1.bdhssj BETWEEN '{2}:00' AND '{3}:59' {0} {1} {4} {5} {6} 
            //            order by t1.bdhssj,t1.bdhsr,t1.sjxm1,t1.bdrq", hsr , zpddm, sDate, eDate, cjdm, tjzh, tjzt);

            string upsql = string.Format(@"select ROWNUM XH,t.* from (SELECT 
                            t1.SJDM1,t1.SJXM1,t1.XYDM1,t1.XYXM1,t1.JX || '-' || t1.JCH JCH,t1.BDRQ,'' QRCODE ,t2.DISPLAYNAME,
                            (case when t1.FSJDM1 is null then t1.sjdm2 else t1.FSJDM1 end) FSJDM1, 
                            (case when t1.FSJXM1 is null then t1.sjxm2 else t1.FSJXM1 end) FSJXM1 , 
                            (case when t1.CBD is null then substr(t1.CWD,6) else substr(t1.CBD,6) end) CK,
                            (case when t1.RBD is null then substr(t1.RWD,6) else substr(t1.RBD,6) end) RK,
                            substr(t1.CQ,6) CQ,substr(t1.JIEC,6) JIEC,substr(t1.JIAOC,6) JIAOC,substr(t1.TQ,6) TQ,t1.SFCZMC , t1.ZDCZMC ,t1.bdhssj,t4.cjmc,
                            (case when t1.TJZT = '2' then '已统计' when t1.TJZT = '1' then '收集' when t1.TJZT = '3' then '暂缓统计'  when t1.TJZT = '4' then '手动统计' else '未处理' end) TJMC
                            FROM dat_bdsj t1 left join users t2 on t1.bdhsr = t2.username 
                            left join dic_cwyxx t3 on t1.sjdm1=t3.cwydm left join DIC_CJXX t4 on t3.ddm = t4.ddm and t3.cjdm = t4.cjdm 
                        WHERE t1.proceeding = 4 {2} {0} {1} {3} {4} {5} 
                            order by t1.bdhssj,t1.bdhsr,t1.sjxm1,t1.bdrq) t", hsr, zpddm, sHsTime, cjdm, tjzh, tjzt);
            list = _db.Ado.SqlQuery<ReportBdsj>(upsql).ToList();
            return list;
        }
        catch (Exception e)
        {
            return list;
        }
    }
    /// <summary>
    /// 打印审核后的报单
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpGet("api/bdcx/ShowReport")]
    [AllowAnonymous]
    [NonUnify]
    public async Task<IActionResult> ShowReport([FromQuery] string param)
    {
        JObject joWhere = JObject.Parse(param);
        string sStartDate = joWhere.Value<string>("StartTime");
        string sEndDate = joWhere.Value<string>("EndTime");

        string Hsr = joWhere.Value<string>("Hsr");
        string Zpddm = joWhere.Value<string>("Zpddm");
        string Cjdm = joWhere.Value<string>("Cjdm");
        string Tjzt = joWhere.Value<string>("Tjzt");
        string Tjzh = joWhere.Value<string>("Tjzh");

        FastReport.Utils.Config.WebMode = true;
        WebReport webReport = new WebReport();
        webReport.LocalizationFile = FastReport.Utils.Config.ApplicationFolder + @"FastReports/Localization/Chinese (Simplified).frl";
        webReport.Report.Load(FastReport.Utils.Config.ApplicationFolder + @"FastReports/Bdcx/fx1.frx");
        string sbblx = "DZ2";
        string s = sStartDate;
        string e = sEndDate;
        string sRq = GeTimeByBblx(ref s, ref e, ref sbblx);
        /*
        if (!string.IsNullOrEmpty(sEndDate))
        {
            sEndDate = Convert.ToDateTime(sEndDate).AddHours(24d).ToString("yyyy-MM-dd HH:mm");
        }*/
        PredicateGroup pg;
        IList<ISort> sorts;
        var data = GetHsReport(Zpddm, Cjdm, Hsr, Tjzh, Tjzt, sStartDate, sEndDate);

        webReport.Report.RegisterData(data, "qry");

        var userInfo = App.User.Claims;
        var userName = App.User?.FindFirst(ClaimConst.Account)?.Value;

        (webReport.Report.FindObject("txtRq") as TextObject).Text = sRq;
        if (!string.IsNullOrWhiteSpace(Hsr) && userName == Hsr)
        {
            var user = await _db.Queryable<SysUserInfo>().FirstAsync(t => t.UserName == Hsr);

            var zyddList = await _db.Ado.SqlQueryAsync<Dmmc>(@"select u.cwzydddm dm,dd.cwzyddmc mc from usercwzydd u 
                                                left join dic_cwzydd dd on u.cwzydddm=dd.cwzydddm where u.username=@UserName", new { UserName = userName });
            var zydd = zyddList.FirstOrDefault();
            if (zydd == null) zydd = new Dmmc();
            (webReport.Report.FindObject("txtJclx") as TextObject).Text = string.IsNullOrWhiteSpace(zydd.Mc) ? "值班员姓名：" + user.DisplayName : "派班室：" + zydd.Mc + " 值班员姓名：" + user.DisplayName;
        }
        else
        {
            (webReport.Report.FindObject("txtJclx") as TextObject).Text = "";
        }
        FastReport.Data.DataSourceBase DataAll = webReport.Report.GetDataSource("qry");
        (webReport.Report.FindObject("DataAll") as DataBand).DataSource = DataAll;
        DataAll.Enabled = true;
        webReport.Inline = true;
        webReport.Report.Prepare();
        HtmlString reportHtml = await webReport.Render();
        return new ContentResult { Content = reportHtml.ToString(), ContentType = "text/html" };

    }
    #endregion

    #region 报单查询(运统1)
    [NonAction]
    private BdsjYt1 FindYt1(string param)
    {
        BdsjYt1 result = null;
        IList<BdsjYt1> bdsjs = new List<BdsjYt1>();
        // 解析字符串
        if (string.IsNullOrEmpty(param)) param = "{}";
        JObject jParam = JObject.Parse(param);
        if (jParam.ContainsKey("Id") && !string.IsNullOrEmpty(jParam.Value<string>("Id")))
        {
            string sql = $@"select * from yt1_dat_bdsj t
                                left outer join yt1_dat_clbdsj c on t.id = c.bdsjid 
                                left outer join yt1_dat_bzsj b on t.id = b.bdsjid 
                                left outer join yt1_dat_zbzsj z on b.id = z.bzsjid 
                                where t.id = {jParam.Value<int>("Id")}";
            var list = _dapperRepository.Context.Query<BdsjYt1, ClbdsjYt1, BzsjYt1, ZbzsjYt1, BdsjYt1>(
                       sql, (bdsj, clbdsj, bzsj, zbzsj) =>
                       {
                           //看报单数据有没有在现有的列表中，没有则加入
                           BdsjYt1 bdsjTmp = bdsjs.Where(m => m.Id == bdsj.Id).FirstOrDefault();
                           if (null == bdsjTmp)
                           {
                               bdsj.Clbdsjs = new List<ClbdsjYt1>();
                               bdsj.Bzsjs = new List<BzsjYt1>();
                               bdsjs.Add(bdsj);
                               bdsjTmp = bdsj;
                           }
                           //看报单数据中有没有
                           if (bzsj != null)
                           {
                               BzsjYt1 bzsjTmp = bdsjTmp.Bzsjs.Where(m => m.Id == bzsj.Id).FirstOrDefault();
                               if (null == bzsjTmp)
                               {
                                   bzsj.Zbzsjs = new List<ZbzsjYt1>();
                                   bdsjTmp.Bzsjs.Add(bzsj);
                                   bzsjTmp = bzsj;
                               }
                               if (zbzsj != null)
                               {
                                   if (!bzsjTmp.Zbzsjs.Any(m => m.Id == zbzsj.Id))
                                   {
                                       bzsjTmp.Zbzsjs.Add(zbzsj);
                                   }
                               }
                           }
                           //看报单数据中有没有
                           if (clbdsj != null)
                           {
                               ClbdsjYt1 clbdsjTmp = bdsjTmp.Clbdsjs.Where(m => m.Id == clbdsj.Id).FirstOrDefault();
                               if (null == clbdsjTmp)
                               {
                                   bdsjTmp.Clbdsjs.Add(clbdsj);
                                   clbdsjTmp = clbdsj;
                               }
                           }
                           return null;
                       });
            foreach (BdsjYt1 bdsj in bdsjs)
            {
                (bdsj.Bzsjs as List<BzsjYt1>).Sort((l, r) =>
                {
                    if (l.Bzcs > r.Bzcs)
                        return 1;
                    else if (l.Bzcs < r.Bzcs)
                        return -1;
                    else
                        return 0;
                });
                foreach (BzsjYt1 bzsj in bdsj.Bzsjs)
                {
                    if (bzsj.Cljc == "-")
                    {
                        bzsj.Cljczb = "摘补";
                    }
                    else
                    {
                        bzsj.Cljczb = bzsj.Cljc;
                    }

                    (bzsj.Zbzsjs as List<ZbzsjYt1>).Sort((l, r) =>
                    {
                        if (l.Bzlx > r.Bzlx)
                            return 1;
                        else if (l.Bzlx < r.Bzlx)
                            return -1;
                        else
                            return 0;
                    });
                }
            }
            result = bdsjs.FirstOrDefault();
        }
        //Mapper.CreateMap<BdsjYt1, Bdsj>().ForAllMembers(opt => opt.Condition(srs => !(new string[] {"Id"}).Contains(srs.MemberName)));
        //Bdsj bdsj = Mapper.Map<Bdsj>(result);
        return result;

    }
    private async Task<Expressionable<BdsjYt1>> parseYt1Param(string param)
    {
        // 解析字符串
        if (string.IsNullOrEmpty(param)) param = "{}";
        JObject jParam = JObject.Parse(param);
        var expable = Expressionable.Create<BdsjYt1>();
        if (jParam.ContainsKey("TqNull"))
        {
            //退勤时间不为空
            expable.And(p => SqlFunc.IsNullOrEmpty(p.Tq));
        }
        if (jParam.ContainsKey("JchNull"))
        {
            //机车号不为空
            expable.And(p => SqlFunc.IsNullOrEmpty(p.Jch));
        }
        //司机代码查询，6个司机位都可以查询
        if (jParam.ContainsKey("Sjdm1") && !string.IsNullOrEmpty(jParam.Value<string>("Sjdm1")))
        {
            //1.有司机查询，则查询指定乘务员报单
            //var pga = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
            //pga.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.Sjdm1, Operator.Eq, jParam.Value<string>("Sjdm1")));
            //pga.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.Fsjdm1, Operator.Eq, jParam.Value<string>("Sjdm1")));
            //pga.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.Xydm1, Operator.Eq, jParam.Value<string>("Sjdm1")));
            //pga.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.Sjdm2, Operator.Eq, jParam.Value<string>("Sjdm1")));
            //pga.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.Fsjdm2, Operator.Eq, jParam.Value<string>("Sjdm1")));
            //pga.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.Xydm2, Operator.Eq, jParam.Value<string>("Sjdm1")));
            //pg.Predicates.Add(pga);

            expable.And(p => p.Sjdm1 == jParam.Value<string>("Sjdm1") || p.Fsjdm1 == jParam.Value<string>("Sjdm1")
            || p.Xydm1 == jParam.Value<string>("Sjdm1") || p.Sjdm2 == jParam.Value<string>("Sjdm1") || p.Fsjdm2 == jParam.Value<string>("Sjdm1")
            || p.Xydm2 == jParam.Value<string>("Sjdm1")
            );
        }
        else if (jParam.ContainsKey("AuditUser") && !string.IsNullOrEmpty(jParam.Value<string>("AuditUser")) && jParam.Value<string>("AuditUser") != "system")
        {
            //2.如果没有司机查询条件，得到当前审核人所在的派班室退勤的报单 
            List<Dmmc> zyddList = await _db.Ado.SqlQueryAsync<Dmmc>(@"select u.cwzydddm dm,dd.cwzyddmc mc from usercwzydd u 
                                                left join dic_cwzydd dd on u.cwzydddm=dd.cwzydddm where u.username=@UserName", new { UserName = jParam.Value<string>("AuditUser") });
            if (zyddList.Count > 0)
            {
                //var pgZydd = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
                foreach (var cwzydd in zyddList)
                {
                    //pgZydd.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.Cwtqdddm, Operator.Eq, cwzydd.Mc)); //运统一返回的是名称，所以要和名称核对
                    expable.And(p => p.Cwtqdddm == cwzydd.Mc);
                }

            }
            else
            {
                //pg.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.Cwtqdddm, Operator.Eq, "NotRole"));
                expable.And(p => p.Cwtqdddm == "NotRole");
            }
        }
        //机型
        if (jParam.ContainsKey("Jx") && !string.IsNullOrEmpty(jParam.Value<string>("Jx")))
        {
            //pg.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.Jx, Operator.Eq, jParam.Value<string>("Jx")));
            expable.And(p => p.Jx == jParam.Value<string>("Jx"));
        }
        //机车号
        if (jParam.ContainsKey("Jch") && !string.IsNullOrEmpty(jParam.Value<string>("Jch")))
        {
            //pg.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.Jch, Operator.Eq, ReturnJch(jParam.Value<string>("Jch"))));
            expable.And(p => p.Jch == jParam.Value<string>("Jch").PadLeft(4, '0'));
        }

        PredicateGroup pg2 = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
        //车次
        if ((jParam.ContainsKey("Zdcc") && !string.IsNullOrEmpty(jParam.Value<string>("Zdcc"))))
        {
            //pg2.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.Zdcc, Operator.Eq, jParam.Value<string>("Zdcc")));
            //pg2.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.Sfcc, Operator.Eq, jParam.Value<string>("Zdcc")));
            expable.And(p => p.Zdcc == jParam.Value<string>("Zdcc") || p.Sfcc == jParam.Value<string>("Zdcc"));
        }
        //报单日期
        if (jParam.ContainsKey("StartTime") && !string.IsNullOrEmpty(jParam.Value<string>("StartTime")) &&
            jParam.ContainsKey("EndTime") && !string.IsNullOrEmpty(jParam.Value<string>("EndTime")))
        {
            //pg.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.Bdrq, Operator.Ge, jParam.Value<string>("StartTime")));
            //pg.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.Bdrq, Operator.Le, jParam.Value<string>("EndTime")));
            expable.And(p => SqlFunc.ToDate(p.Bdrq) >= SqlFunc.ToDate(jParam.Value<string>("StartTime")));
            expable.And(p => SqlFunc.ToDate(p.Bdrq) <= SqlFunc.ToDate(jParam.Value<string>("EndTime")));
        }

        //机务段
        if (jParam.ContainsKey("Zpddm") && !string.IsNullOrEmpty(jParam.Value<string>("Zpddm")))
        {
            //pg.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.Zpddm, Operator.Eq, jParam.Value<string>("Zpddm")));
            expable.And(p => p.Zpddm == jParam.Value<string>("Zpddm"));
        }

        //是否生成电子报单
        if (jParam.ContainsKey("SCZT") && !string.IsNullOrEmpty(jParam.Value<string>("SCZT")))
        {
            //pg.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.Proceeding, Operator.Eq, jParam.Value<string>("SCZT")));
            expable.And(p => p.Proceeding == jParam.Value<int>("SCZT"));
        }
        //退勤状态
        if (jParam.ContainsKey("TQZT") && !string.IsNullOrEmpty(jParam.Value<string>("TQZT")))
        {
            if (jParam.Value<string>("TQZT") == "1")  //未退勤
                //pg.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.Tq, Operator.Eq, null, false));
                expable.And(p => SqlFunc.IsNullOrEmpty(p.Tq));
            else if (jParam.Value<string>("TQZT") == "3")  //已退勤
                //pg.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.Tq, Operator.Eq, null, true));
                expable.And(p => !SqlFunc.IsNullOrEmpty(p.Tq));
        }

        //审核人
        if (jParam.ContainsKey("SHR") && !string.IsNullOrEmpty(jParam.Value<string>("SHR")))
        {
            var val = jParam.Value<string>("SHR");
            //pg.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.SHR, Operator.Eq, val));
            expable.And(p => p.SHR == val);
        }

        //审核时间
        if (jParam.ContainsKey("StartSHSJ") && !string.IsNullOrEmpty(jParam.Value<string>("StartSHSJ")))
        {
            var val = DateTime.Parse(jParam.Value<string>("StartSHSJ")).ToString("yyyy-MM-dd HH:mm:00");
            //pg.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.SHSJ, Operator.Ge, val));
            expable.And(p => SqlFunc.ToDate(p.SHSJ) >= SqlFunc.ToDate(val));
        }

        if (jParam.ContainsKey("EndSHSJ") && !string.IsNullOrEmpty(jParam.Value<string>("EndSHSJ")))
        {
            var val = DateTime.Parse(jParam.Value<string>("EndSHSJ")).ToString("yyyy-MM-dd HH:mm:00");
            expable.And(p => SqlFunc.ToDate(p.SHSJ) <= SqlFunc.ToDate(val));
            //pg.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.SHSJ, Operator.Le, val));
        }
        return expable;
    }
    [NonUnify]
    [HttpGet("api/Yt1/FindAll")]
    [Authorize(Roles = "yt1bdsh_base")]
    public async Task<object> FindYtq(string param)
    {
        try
        {
            JObject jParam = JObject.Parse(param);
            List<OrderByModel> orderList = OrderByModel.Create();
            int page = 0;
            int limit = 0;
            if (jParam.ContainsKey("page") && !string.IsNullOrEmpty(jParam.Value<string>("page")) &&
                jParam.ContainsKey("limit") && !string.IsNullOrEmpty(jParam.Value<string>("limit")))
            {
                page = jParam.Value<int>("page");
                limit = jParam.Value<int>("limit");
            }
            if (jParam.ContainsKey("Sort"))
            {
                var ssorts = jParam.Value<JArray>("Sort");
                foreach (var ssort in ssorts)
                {
                    orderList.Add(new OrderByModel { FieldName = ssort.Value<string>("PropertyName"), OrderByType = ssort.Value<bool>("Ascending") ? OrderByType.Asc : OrderByType.Desc });
                }
            }
            if (orderList.Count() == 0)
            {
                orderList.Add(new OrderByModel { FieldName = "Tq", OrderByType = OrderByType.Asc });
            }
            var expable = await parseYt1Param(param);
            //分页模式
            if (page > 0 && limit > 0)
            {
                var pagedResult = _db.Queryable<BdsjYt1>().Where(expable.ToExpression()).OrderBy(orderList).ToPagedList(page, limit);
                return new { success = true, message = "", data = pagedResult.Items, total = pagedResult.Total, pages = pagedResult.TotalPages };
            }
            else
            {
                return _db.Queryable<BdsjYt1>().Where(expable.ToExpression()).OrderBy(orderList).ToList();
            }
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    [NonUnify]
    [HttpGet("api/Yt1/Find")]
    [Authorize(Roles = "yt1bdsh_querymx")]
    public dynamic FindYt1Details(string param)
    {
        try
        {
            BdsjYt1 result = FindYt1(param);

            if (null == result)
            {
                return new { success = false, message = $"参数{param}无法找到相应的数据！" };
            }
            else
            {
                object ext = null;
                //额外输出信息
                if (!string.IsNullOrEmpty(result.SHR))
                {
                    var shr = _db.Queryable<SysUserInfo>().First(t => t.UserName == result.SHR);
                    ext = new { SHRDisplayName = shr.DisplayName };
                }

                return new { success = true, message = "", data = result, extData = ext, total = 1 };
            }
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    [AllowAnonymous]
    [NonUnify]
    [HttpGet("api/Yt1/Stamp")]
    public async Task<IActionResult> Yt1Stamp([FromQuery] string param)
    {
        BdsjYt1 result = FindYt1(param);
        foreach (var item in result.Bzsjs)
        {
            if (item.Cf != null)
            {
                item.Cf = item.Cf.Substring(11, 5);
            }
            if (item.Dd != null)
            {
                item.Dd = item.Dd.Substring(11, 5);
            }
        };
        FastReport.Utils.Config.WebMode = true;
        WebReport webReport = new WebReport();
        webReport.Toolbar.Sticky = true;
        webReport.Toolbar.IconColor = IconColors.Black;
        webReport.LocalizationFile = FastReport.Utils.Config.ApplicationFolder + @"FastReports/Localization/Chinese (Simplified).frl";
        webReport.Report.Load(FastReport.Utils.Config.ApplicationFolder + @"Reports/sjbd.frx");
        webReport.Report.RegisterData(new BdsjYt1[] { result }, "Bdsjs", FastReport.Data.BOConverterFlags.AllowFields, 3);
        (webReport.Report.FindObject("DataBdsj") as DataBand).DataSource = webReport.Report.GetDataSource("Bdsjs");
        (webReport.Report.FindObject("DataBzsj") as DataBand).DataSource = webReport.Report.GetDataSource("Bdsjs.Bzsjs");
        (webReport.Report.FindObject("DataZbzsj") as DataBand).DataSource = webReport.Report.GetDataSource("Bdsjs.Bzsjs.Zbzsjs");
        (webReport.Report.FindObject("BarcodeBdbh") as BarcodeObject).Text = string.IsNullOrEmpty(result.Qrcode) ? "" : result.Qrcode;

        if (!string.IsNullOrEmpty(result.SHR))
        {
            var user = _db.Queryable<SysUserInfo>().First(t => t.UserName == result.SHR);
            (webReport.Report.FindObject("TextSHR") as TextObject).Text = result.SHSJ + " " + user.DisplayName + " 审核";
        }
        webReport.Inline = true;
        HtmlString reportHtml = await webReport.Render();
        return new ContentResult { Content = reportHtml.ToString(), ContentType = "text/html" };
    }
    #endregion

    #region 报单信息统计
    [NonUnify]
    [HttpGet("api/Bdxxtj/FindAll")]
    [Authorize(Roles = "bdxxtj_base")]
    public dynamic FindBdxxtj([FromQuery] string param)
    {
        #region 获取报单统计汇总信息
        if (string.IsNullOrEmpty(param)) param = "{}";
        JObject json = JObject.Parse(param);
        string sDdm = json.Value<string>("Zpddm");
        string sCjdm = json.Value<string>("Cjdm");
        string sCwzydddm = json.Value<string>("Cwzydddm");
        string startDate = json.Value<string>("StartTime");
        string endDate = json.Value<string>("EndTime");
        string sSort = json["Sort"] == null ? "" : json["Sort"].ToString();
        int iPageSize = json.Value<int>("limit") == 0 ? 20 : json.Value<int>("limit");
        int iPageNum = json.Value<int>("page") == 0 ? 1 : json.Value<int>("page");
        DateTime dtRq = DateTime.Now;
        if (string.IsNullOrWhiteSpace(startDate) && string.IsNullOrWhiteSpace(endDate))
        {
            return new { success = false, message = "开始日期和结束日期不能同时为空！" };
        }
        if (!DateTime.TryParse(startDate, out dtRq))
        {
            return new { success = false, message = "开始日期格式错误！" };
        }
        if (!DateTime.TryParse(endDate, out dtRq))
        {
            return new { success = false, message = "结束日期格式错误！" };
        }

        try
        {
            int iCount = 0;
            string sWhere = " where bdrq between '" + startDate + "' and '" + endDate + "'";
            if (!string.IsNullOrEmpty(sDdm))
            {
                sWhere += string.Format(" and zpddm='{0}'", sDdm);
            }

            if (!string.IsNullOrEmpty(sCjdm))
            {
                sWhere += string.Format(" and cjdm='{0}'", sCjdm);
            }
            string sWhereCwzydd = "";
            if (!string.IsNullOrEmpty(sCwzydddm))
            {
                sWhereCwzydd += string.Format(" and bdtjdddm='{0}'", sCwzydddm);
            }
            string sql = string.Format("select count(distinct bdrq) from yt1_dat_bdsj {0}", sWhere);

            string sCount = _db.Ado.GetString(sql);
            iCount = int.Parse(string.IsNullOrEmpty(sCount) ? "0" : sCount);
            int iPage = (iPageNum - 1) * iPageSize + 1;
            int iNum = iPageNum * iPageSize;
            sql = $@"select * from (select rownum rn,t.* from(
                        select dt.bdrq,bdzs,wsc,ysc,zcbd,wyx,wrl,wsj,wtj,whs,ytongji,wtongji,yhs,
                        TRUNC(yhs/(zcbd+wyx+wrl+wsj),3)*100 as recovery   from 
                        (
                          select to_char(to_date('{startDate}', 'yyyy-mm-dd hh24:mi:ss') + rownum - 1, 'yyyy-mm-dd') bdrq
                                 from dual connect by rownum <= trunc(to_date('{endDate}','yyyy-mm-dd hh24:mi:ss') - to_date('{startDate}','yyyy-mm-dd hh24:mi:ss')) + 1
                        ) dt 
                        left join 
                        (
                             select count(1) bdzs,count(case when proceeding = 0 then 1 else null end) wsc ,bdrq 
                                    from yt1_dat_bdsj 
                             {sWhere} group by bdrq
                        ) yt1 on dt.bdrq=yt1.bdrq
                        left join 
                        (
                             select count(1) ysc,bdrq,
                                    count(case when qxbdbs = 0 and (proceeding = 3 or proceeding = 4) {sWhereCwzydd} then 1 else null end) zcbd,
                                    count(case when qxbdbs = 1 and (proceeding = 3 or proceeding = 4) {sWhereCwzydd} then 1 else null end) wyx, 
                                    count(case when qxbdbs = 2 and (proceeding = 3 or proceeding = 4) {sWhereCwzydd} then 1 else null end) wrl, 
                                    count(case when qxbdbs = 3 and (proceeding = 3 or proceeding = 4) {sWhereCwzydd} then 1 else null end) wsj, 
                                    count(case when proceeding = 1 then 1 else null end) wtj, 
                                    count(case when proceeding = 3 {sWhereCwzydd} then 1 else null end) whs,
                                    count(case when proceeding = 4 {sWhereCwzydd} then 1 else null end) yhs, 
                                    count(case when tjzt = 2 then 1 else null end) ytongji, 
                                    count(case when tjzt != 2 or tjzh is null then 1 else null end) wtongji 
                                    from dat_bdsj 
                             {sWhere} group by bdrq) bd on dt.bdrq=bd.bdrq order by bdrq desc
                        ) t) where rownum between {iPage} and {iNum}";
            IList<Bdxxtj> list = _db.Ado.SqlQuery<Bdxxtj>(sql);

            return new { success = true, data = new { total = iCount, items = list } };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }

        #endregion
    }
    [NonAction]
    public IList<Bdsj> FindBdxxList(string sBdrq, string sDdm, string sCjdm, string sCwzydddm, string sType, string sSort, int iPageNum, int iPageSize, ref int iCount)
    {
        string sWhere = " from dat_bdsj ";
        if (sType == "bdzs" || sType == "wsc")
        {
            sWhere = " from yt1_dat_bdsj ";
        }
        sWhere += " where bdrq='" + sBdrq + "'";
        if (!string.IsNullOrEmpty(sDdm))
        {
            sWhere += string.Format(" and zpddm='{0}'", sDdm);
        }
        if (!string.IsNullOrEmpty(sCjdm))
        {
            sWhere += string.Format(" and cjdm='{0}'", sCjdm);
        }
        string sWhereCwzydd = "";
        if (!string.IsNullOrEmpty(sCwzydddm))
        {
            sWhereCwzydd += string.Format(" and bdtjdddm='{0}'", sCwzydddm);
        }
        switch (sType)
        {
            case "wsc":
                sWhere += " and proceeding=0";
                break;

            case "zcbd":
                sWhere += " and qxbdbs = 0 and (proceeding = 3 or proceeding = 4) " + sWhereCwzydd;
                break;

            case "wyx":
                sWhere += " and qxbdbs = 1 and (proceeding = 3 or proceeding = 4) " + sWhereCwzydd;
                break;

            case "wrl":
                sWhere += " and qxbdbs = 2 and (proceeding = 3 or proceeding = 4) " + sWhereCwzydd;
                break;

            case "wsj":
                sWhere += " and qxbdbs = 3 and (proceeding = 3 or proceeding = 4) " + sWhereCwzydd;
                break;
            case "bdysh":
                sWhere += " and proceeding=4 " + sWhereCwzydd;
                break;
            case "wtj":
                sWhere += " and proceeding=1";
                break;

            case "whs":
                sWhere += " and proceeding = 3 " + sWhereCwzydd;
                break;

            case "ytongji":
                sWhere += " and tjzt = 2";
                break;

            case "wtongji":
                sWhere += " and (tjzt != 2 or tjzh is null)";
                break;
        }

        string sql = string.Format("select count(1) {0}", sWhere);
        iCount = _db.Ado.GetInt(sql);
        int iPage = (iPageNum - 1) * iPageSize + 1;
        int iNum = iPageNum * iPageSize;

        sql = $@"select * from(select rownum rn,t.* from (select Id,Jx,Jch,Cq,Tq,Sjdm1,Sjxm1,Fsjdm1,Fsjxm1,Sjdm2,sjxm2,fsjdm2,fsjxm2,sfczmc,sfcc,zdczmc,zdcc {sWhere})t) where rn between {iPage} and {iNum} order by cq desc";

        IList<Bdsj> list = _db.Ado.SqlQuery<Bdsj>(sql);

        return list;
    }
    [HttpGet("api/Bdxxtj/FindBdxxList")]
    [Authorize(Roles = "bdxxtjList_base")]
    public dynamic FindBdxxList([FromQuery] string param)
    {
        #region 根据报单日期获取详细信息列表
        if (string.IsNullOrEmpty(param)) param = "{}";
        JObject json = JObject.Parse(param);
        string sDdm = json.Value<string>("Zpddm");
        string sBdrq = json.Value<string>("Bdrq");
        string sCjdm = json.Value<string>("Cjdm");
        string sCwzydddm = json.Value<string>("Cwzydddm");
        string sType = json.Value<string>("sType");
        string sSort = json["Sort"] == null ? "" : json["Sort"].ToString();
        int iPageSize = json.Value<int>("limit") == 0 ? 20 : json.Value<int>("limit");
        int iPageNum = json.Value<int>("page") == 0 ? 1 : json.Value<int>("page");
        DateTime dtRq = DateTime.Now;
        if (string.IsNullOrWhiteSpace(sBdrq))
        {
            return new { success = false, message = "报单日期不能为空！" };
        }
        if (!DateTime.TryParse(sBdrq, out dtRq))
        {
            return new { success = false, message = "报单日期格式错误！" };
        }
        int iCount = 0;
        IList<Bdsj> list = FindBdxxList(sBdrq, sDdm, sCjdm, sCwzydddm, sType, sSort, iPageNum, iPageSize, ref iCount);

        return new { total = iCount, items = list };
        #endregion
    }
    #endregion

    #region 能耗数据对比
    [HttpGet("api/Bdcx/GetNhsjdb")]
    [Authorize(Roles = "nhsjdb_base")]
    public dynamic GetNhsjdb(string param)
    {
        #region 获取报单头信息
        if (string.IsNullOrEmpty(param)) param = "{}";
        JObject json = JObject.Parse(param);
        string sDdm = json.Value<string>("Zpddm");
        string sQueryNhDif = json.Value<string>("QueryNhDif");
        string sCwydm = json.Value<string>("Cwydm");
        string sJx = json.Value<string>("Jx");
        string sJch = json.Value<string>("Jch");
        string sCc = json.Value<string>("Cc");
        string sStartDate = json.Value<string>("StartTime");
        string sEndDate = json.Value<string>("EndTime");
        int iPageSize = json.Value<int>("limit") == 0 ? 20 : json.Value<int>("limit");
        int iPageNum = json.Value<int>("page") == 0 ? 1 : json.Value<int>("page");
        DateTime dtRq = DateTime.Now;
        int iCount = 0;
        string sWhere = "  bdrq between '" + sStartDate + "' and '" + sEndDate + "' and Jsl1_BF is not null ";
        if (!string.IsNullOrEmpty(sDdm))
        {
            sWhere += string.Format(" and zpddm='{0}'", sDdm);
        }
        if (!string.IsNullOrEmpty(sCwydm))
        {
            string sWhereCwy = "";
            string[] cwyArr = sCwydm.Split(',');
            foreach (var cwy in cwyArr)
            {
                if (!string.IsNullOrEmpty(cwy))
                {
                    if (sWhereCwy == "")
                    {
                        sWhereCwy = "sjdm1='" + cwy + "' or sjxm1='" + cwy + "' or fsjdm1='" + cwy + "' or fsjxm1='" + cwy + "' or sjdm2='" + cwy + "' or sjxm2='" + cwy + "'";
                    }
                    else
                    {
                        sWhereCwy += "or sjdm1='" + cwy + "' or sjxm1='" + cwy + "' or fsjdm1='" + cwy + "' or fsjxm1='" + cwy + "' or sjdm2='" + cwy + "' or sjxm2='" + cwy + "'";
                    }
                }
            }
            if (!string.IsNullOrEmpty(sWhereCwy))
            {
                sWhere += " and (" + sWhereCwy + ")";
            }
        }
        if (!string.IsNullOrEmpty(sJx))
        {
            sWhere += string.Format(" and jx='{0}'", sJx);
        }
        if (!string.IsNullOrEmpty(sJch))
        {
            sWhere += string.Format(" and jch='{0}'", sJch);
        }
        if (!string.IsNullOrEmpty(sCc))
        {
            sWhere += string.Format(" and cc='{0}'", sCc);
        }

        string sNhWhere = "";
        if (!string.IsNullOrEmpty(sQueryNhDif))
        {
            switch (sQueryNhDif)
            {
                case "1":  //小于200度
                    if (!string.IsNullOrEmpty(sNhWhere))
                    {
                        sNhWhere += " and (Hdlc1<200 or Hdlc2<200)";
                    }
                    else
                    {
                        sNhWhere += " (Hdlc1<200 or Hdlc2<200)";
                    }
                    break;
                case "2": //200-1000度
                    if (!string.IsNullOrEmpty(sNhWhere))
                    {
                        sNhWhere += " and ((Hdlc1>=200 and Hdlc1<=1000) or (Hdlc2>=200 and Hdlc2<=1000))";
                    }
                    else
                    {
                        sNhWhere += " ((Hdlc1>=200 and Hdlc1<=1000) or (Hdlc2>=200 and Hdlc2<=1000))";
                    }
                    break;
                case "3":  //大于1000度
                    if (!string.IsNullOrEmpty(sNhWhere))
                    {
                        sNhWhere += " and (Hdlc1>1000 or Hdlc1>1000)";
                    }
                    else
                    {
                        sNhWhere += " (Hdlc1>1000 or Hdlc1>1000)";
                    }
                    break;
                case "4":  //小于50度
                    if (!string.IsNullOrEmpty(sNhWhere))
                    {
                        sNhWhere += " and (Zsdhdlc1<50 or Zsdhdlc2<50)";
                    }
                    else
                    {
                        sNhWhere += " (Zsdhdlc1<50 or Zsdhdlc2<50)";
                    }
                    break;
                case "5": //50-100度
                    if (!string.IsNullOrEmpty(sNhWhere))
                    {
                        sNhWhere += " and ((Zsdhdlc1>=50 and Zsdhdlc1<=100) or (Zsdhdlc2>=50 and Zsdhdlc2<=100))";
                    }
                    else
                    {
                        sNhWhere += " ((Zsdhdlc1>=50 and Zsdhdlc1<=100) or (Zsdhdlc2>=50 and Zsdhdlc2<=100))";
                    }
                    break;
                case "6":  //大于100度
                    if (!string.IsNullOrEmpty(sNhWhere))
                    {
                        sNhWhere += " and (Zsdhdlc1>100 or Zsdhdlc2>100)";
                    }
                    else
                    {
                        sNhWhere += " (Zsdhdlc1>100 or Zsdhdlc2>100)";
                    }
                    break;
            }
        }
        if (!string.IsNullOrEmpty(sNhWhere))
        {
            sNhWhere = " where " + sNhWhere;
        }
        string sqlbase = @"select {1} from 
            (
                select t.*,round(abs(hdl1-hdl1_BF),0) Hdlc1,round(abs(hdl2-hdl2_BF),0) Hdlc2,round(abs(zsdhdl1-zsdhdl1_BF),0) Zsdhdlc1,round(abs(zsdhdl2-zsdhdl2_BF),0) Zsdhdlc2 from 
                (
                    SELECT bd.id,bd.zpddm,bd.Bdtjly,bd.sjdm1, bd.sjxm1, bd.fsjdm1,bd.fsjxm1, bd.sjdm2,bd.sjxm2, bd.zcfsmc, bd.jczl, bd.jx, bd.jch, bd.bdrq, bd.cq, bd.tq, bd.jiec, bd.jiaoc,
                    Jsl1,Jsl2,Zsdjsl1,Zsdjsl2,Jcl1,Jcl2,Zsdjcl1,Zsdjcl2,
                    Jsl1_BF,Jsl2_BF,Zsdjsl1_BF,Zsdjsl2_BF,Jcl1_BF,Jcl2_BF,Zsdjcl1_BF,Zsdjcl2_BF,
                    (Jcl1 - Jsl1) Hdl1,(Jcl1_BF - Jsl1_BF) Hdl1_BF,
                    (Jcl2 - Jsl2) Hdl2,(Jcl2_BF - Jsl2_BF) Hdl2_BF,
                    (Zsdjcl1 - Zsdjsl1) Zsdhdl1,(Zsdjcl1_BF - Zsdjsl1_BF) Zsdhdl1_BF,
                    (Zsdjcl2 - Zsdjsl2) Zsdhdl2,(Zsdjcl2_BF - Zsdjsl2_BF) Zsdhdl2_BF
                    FROM DAT_BDSJ bd WHERE {0} and bd.proceeding in (3,4)
                )  t
            ) t {2} order by jiec desc";

        string sCount = _db.Ado.SqlQuery<string>(string.Format(sqlbase, sWhere, "count(1)", sNhWhere)).FirstOrDefault();
        iCount = int.Parse(string.IsNullOrEmpty(sCount) ? "0" : sCount);
        int iPage = (iPageNum - 1) * iPageSize + 1;
        int iNum = iPageNum * iPageSize;

        string sql = $@"select * from (select rownum rn,t.* from 
                            (" + string.Format(sqlbase, sWhere, "t.*,(case when Hdlc1<200 then '小于200度' when Hdlc1>=200 and Hdlc1<=1000 then '200-1000度' when Hdlc1>1000 then '大于1000度' end) bdffr,(case when Zsdhdlc1<50 then '小于50度' when Zsdhdlc1>=50 and Zsdhdlc1<=100 then '50-100度' when Zsdhdlc1>100 then '大于100度' end) bdhsr ", sNhWhere) +
                        ") t) where rn between " + iPage + " and " + iNum;

        IList<Bdsj> list = _db.Ado.SqlQuery<Bdsj>(sql);

        return new { total = iCount, items = list };
        #endregion
    }
    #endregion

    #region 能耗数据对比
    [HttpGet("api/Bdcx/GetCdrds")]
    [Authorize(Roles = "cdrd_base")]
    public dynamic GetCdrds(string param)
    {
        #region 获取报单头信息
        if (string.IsNullOrEmpty(param)) param = "{}";
        JObject json = JObject.Parse(param);
        string sDdm = json.Value<string>("Zpddm");
        string sCwydm = json.Value<string>("Cwydm");
        string sJx = json.Value<string>("Jx");
        string sJch = json.Value<string>("Jch");
        string sCc = json.Value<string>("Cc");
        string sStartDate = json.Value<string>("StartTime");
        string sEndDate = json.Value<string>("EndTime");

        string sQueryFlag = json.Value<string>("QueryFlag");
        string sQueryTimeDif = json.Value<string>("QueryTimeDif");
        int iPageSize = json.Value<int>("limit") == 0 ? 20 : json.Value<int>("limit");
        int iPageNum = json.Value<int>("page") == 0 ? 1 : json.Value<int>("page");
        DateTime dtRq = DateTime.Now;

            int iCount = 0;
        string sWhere = "  bdrq between '" + sStartDate + "' and '" + sEndDate + "'";
        string sCdrdWhere = "(cbd is not null or rbd is not null or AEI_CD is not null or AEI_RD is not null)";
        if (!string.IsNullOrEmpty(sDdm))
        {
            sWhere += string.Format(" and zpddm='{0}'", sDdm);
        }
        if (!string.IsNullOrEmpty(sCwydm))
        {
            string sWhereCwy = "";
            string[] cwyArr = sCwydm.Split(',');
            foreach (var cwy in cwyArr)
            {
                if (!string.IsNullOrEmpty(cwy))
                {
                    if (sWhereCwy == "")
                    {
                        sWhereCwy = "sjdm1='" + cwy + "' or sjxm1='" + cwy + "' or fsjdm1='" + cwy + "' or fsjxm1='" + cwy + "' or sjdm2='" + cwy + "' or sjxm2='" + cwy + "'";
                    }
                    else
                    {
                        sWhereCwy += "or sjdm1='" + cwy + "' or sjxm1='" + cwy + "' or fsjdm1='" + cwy + "' or fsjxm1='" + cwy + "' or sjdm2='" + cwy + "' or sjxm2='" + cwy + "'";
                    }
                }
            }
            if (!string.IsNullOrEmpty(sWhereCwy))
            {
                sWhere += " and (" + sWhereCwy + ")";
            }
        }
        if (!string.IsNullOrEmpty(sJx))
        {
            sWhere += string.Format(" and jx='{0}'", sJx);
        }
        if (!string.IsNullOrEmpty(sJch))
        {
            sWhere += string.Format(" and jch='{0}'", sJch);
        }
        if (!string.IsNullOrEmpty(sCc))
        {
            sWhere += string.Format(" and cc='{0}'", sCc);
        }

        if (!string.IsNullOrEmpty(sQueryFlag))
        {
            switch (sQueryFlag)
            {
                case "1":  //出段都有或入段都有
                    sCdrdWhere = "((cbd is not null and AEI_CD is not null) or (rbd is not null and AEI_RD is not null))";
                    break;
                case "2":  //出段监控有aei无
                    sCdrdWhere = "(cbd is not null and AEI_CD is null)";
                    break;
                case "3":  //出段aei有监控无
                    sCdrdWhere = "(cbd is null and AEI_CD is not null)";
                    break;
                case "4":  //出段都有
                    sCdrdWhere = "(cbd is not null and AEI_CD is not null)";
                    break;
                case "5":  //入段监控有aei无
                    sCdrdWhere = "(rbd is not null and AEI_RD is null)";
                    break;
                case "6":  //入段aei有监控无
                    sCdrdWhere = "(rbd is null and AEI_RD is not null)";
                    break;
                case "7":  //入段都有
                    sCdrdWhere = "(rbd is not null and AEI_RD is not null)";
                    break;
            }
        }

        if (!string.IsNullOrEmpty(sQueryTimeDif))
        {
            switch (sQueryTimeDif)
            {
                case "1":  //小于5分钟
                    if (!string.IsNullOrEmpty(sCdrdWhere))
                    {
                        sCdrdWhere += " and (cwd<5 or rwd<5)";
                    }
                    else
                    {
                        sCdrdWhere += "(cwd<5 or rwd<5)";
                    }
                    break;
                case "2": //5-20分钟
                    if (!string.IsNullOrEmpty(sCdrdWhere))
                    {
                        sCdrdWhere += " and ((cwd>=5 and cwd<=20) or (rwd>=5 and rwd<=20))";
                    }
                    else
                    {
                        sCdrdWhere += " ((cwd>=5 and cwd<=20) or (rwd>=5 and rwd<=20))";
                    }
                    break;
                case "3":  //大于20分钟
                    if (!string.IsNullOrEmpty(sCdrdWhere))
                    {
                        sCdrdWhere += " and ((cwd>20) or (rwd>20))";
                    }
                    else
                    {
                        sCdrdWhere += " ((cwd>20) or (rwd>20))";
                    }
                    break;
            }
        }
        string sqlbase = @"select {1} from 
            (
                SELECT bd.id,bd.zpddm,bd.Bdtjly,bd.sjdm1, bd.sjxm1, bd.fsjdm1,bd.fsjxm1, bd.sjdm2,bd.sjxm2, bd.zcfsmc, bd.jczl, bd.jx, bd.jch, bd.bdrq, bd.cq, bd.tq, bd.jiec, bd.jiaoc,
                (case when bd.cbd is null then bd.cwd else bd.cbd end) cbd, (case when bd.rbd is null then bd.rwd else bd.rbd end)rbd,AEI_CD,AEI_RD,
                 round(case when cbd is not null and aei_cd is not null then abs(to_date(cbd, 'yyyy-mm-dd hh24:mi') - to_date(aei_cd, 'yyyy-mm-dd hh24:mi')) * 1440 else null end,0) cwd, 
                 round(case when rbd is not null and aei_rd is not null then abs(to_date(rbd, 'yyyy-mm-dd hh24:mi') - to_date(aei_rd, 'yyyy-mm-dd hh24:mi')) * 1440 else null end,0) rwd
                FROM DAT_BDSJ bd WHERE {0} and bd.proceeding in (3,4)
            ) t where {2} order by jiec desc";

        string sCount = _db.Ado.SqlQuery<string>(string.Format(sqlbase, sWhere, "count(1)", sCdrdWhere)).FirstOrDefault();
        iCount = int.Parse(string.IsNullOrEmpty(sCount) ? "0" : sCount);
        int iPage = (iPageNum - 1) * iPageSize + 1;
        int iNum = iPageNum * iPageSize;

        string sql = $@"select * from (select rownum rn,t.* from 
                            (" + string.Format(sqlbase, sWhere, "t.*,(case when cwd<5 then '小于5分钟' when cwd>=5 and cwd<=20 then '5-20分钟' when cwd>20 then '大于20分钟' end) bdffr,(case when rwd<5 then '小于5分钟' when rwd>=5 and rwd<=20 then '5-20分钟' when rwd>20 then '大于20分钟' end) bdhsr ", sCdrdWhere) +
                        ") t) where rn between " + iPage + " and " + iNum;
        IList<Bdsj> list = _db.Ado.SqlQuery<Bdsj>(sql);
        return new { total = iCount, items = list };
        #endregion
    }
    #endregion

    #region 监控数据查询
    [HttpGet("api/Bdcx/GetLkjBdts")]
    [Authorize(Roles = "lkjbdt_base")]
    public dynamic GetLkjBdts(string param)
    {
        #region 获取报单头信息
        if (string.IsNullOrEmpty(param)) param = "{}";
        JObject json = JObject.Parse(param);
        string sDdm = json.Value<string>("Zpddm");
        string sCwydm = json.Value<string>("Cwydm");
        string sJx = json.Value<string>("Jx");
        string sJch = json.Value<string>("Jch");
        string sCc = json.Value<string>("Cc");
        string startDate = json.Value<string>("StartTime");
        string endDate = json.Value<string>("EndTime");
        int iPageSize = json.Value<int>("limit") == 0 ? 20 : json.Value<int>("limit");
        int iPageNum = json.Value<int>("page") == 0 ? 1 : json.Value<int>("page");
        DateTime dtRq = DateTime.Now;
        int iCount = 0;
        string sWhere = " where substr(wjjlsj,0,10) between '" + startDate + "' and '" + endDate + "'";
        if (!string.IsNullOrEmpty(sDdm))
        {
            sWhere += string.Format(" and zpddm='{0}'", sDdm);
        }
        if (!string.IsNullOrEmpty(sCwydm))
        {
            string sWhereCwy = "";
            string[] cwyArr = sCwydm.Split(',');
            foreach (var cwy in cwyArr)
            {
                if (!string.IsNullOrEmpty(cwy))
                {
                    if (sWhereCwy == "")
                    {
                        sWhereCwy = "sjh='" + cwy + "' or sjxm='" + cwy + "' or fsjh='" + cwy + "' or fsjxm='" + cwy + "'";
                    }
                    else
                    {
                        sWhereCwy += "or sjh='" + cwy + "' or sjxm='" + cwy + "' or fsjh='" + cwy + "' or fsjxm='" + cwy + "'";
                    }
                }
            }
            if (!string.IsNullOrEmpty(sWhereCwy))
            {
                sWhere += " and (" + sWhereCwy + ")";
            }
        }
        if (!string.IsNullOrEmpty(sJx))
        {
            sWhere += string.Format(" and jxmc='{0}'", sJx);
        }
        if (!string.IsNullOrEmpty(sJch))
        {
            sWhere += string.Format(" and jch='{0}'", sJch);
        }
        if (!string.IsNullOrEmpty(sCc))
        {
            sWhere += string.Format(" and cc='{0}'", sCc);
        }
        string sql = string.Format("select count(1) from lkj_bdsj_bdt {0}", sWhere);
        string sCount = _db.Ado.GetString(sql);
        iCount = int.Parse(string.IsNullOrEmpty(sCount) ? "0" : sCount);
        int iPage = (iPageNum - 1) * iPageSize + 1;
        int iNum = iPageNum * iPageSize;

        sql = $@"select * from (select rownum rn,t.* from (select * from lkj_bdsj_bdt {sWhere} order by wjjlsj desc) t) where rn between {iPage} and {iNum} order by wjjlsj desc";
        IList<LkjBdsjBdt> list = _db.Ado.SqlQuery<LkjBdsjBdt>(sql);

        return new { total = iCount, items = list };
        #endregion
    }
    #endregion

    #region signalr
    [AllowAnonymous]
    [HttpGet("signalr/negotiate")]
    public async Task Negotiate([FromQuery] string clientProtocol, string connectionData, string _)
    {
        JArray json = JArray.Parse(connectionData);
        var hubs = json.FirstOrDefault().Value<string>("name");
        var connection = new HubConnectionBuilder()
        .WithAutomaticReconnect()
        .WithUrl($"http://{App.HttpContext.Request.Host}/{hubs}", opts =>
        {
            opts.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets;
            opts.UseDefaultCredentials = true;
            opts.SkipNegotiation = true;
        })
        .Build();
        await connection.StartAsync();
    }
    #endregion

    #region 其他
    [HttpPost("api/Zdfjdjl/GetJcaeiDetail")]
    public async Task<bool> GetJcaeiDetail(RequestForGetJcaei r)
    {
        try
        {
            var dbOptions = App.GetConfig<DbConnectionOptions>("DbConnection", true);
            List<ConnectionConfig> connectionConfigs = dbOptions.ConnectionConfigs.Where(t => t.ConfigId== "1300000000002").Select(t => new ConnectionConfig {
                ConfigId = t.ConfigId,
                ConnectionString = t.ConnectionString,
                IsAutoCloseConnection = true
            }).ToList();

            var db = new SqlSugarClient(connectionConfigs);
            string sql = $@"select * from JC_AEI_DETAIL where PARENT_ID=@Id";
            IList<JcAeiDetail> jcAeiDetailList = await db.Ado.SqlQueryAsync<JcAeiDetail>(sql, new { Id = r.jcaei.ID });
            _db.Ado.BeginTran();
            await _db.Insertable<JcAeiModel>(r.jcaei).ExecuteCommandAsync();
            await _db.Insertable<JcAeiDetail>(jcAeiDetailList).ExecuteCommandAsync();
            await _db.Ado.ExecuteCommandAsync($" update dic_xtcs set cs='{r.jcaei.IN_DB_TIME.ToString("yyyy-MM-dd HH:mm:ss")}' where csdm='aeidate' ");
            _db.Ado.CommitTran();
            return true;
        }
        catch
        {
            _db.Ado.RollbackTran();
            return false;
        }
    }
    [HttpPost("api/Zdfjdjl/GetJcaei")]
    [NonUnify]
    public async Task<dynamic> GetJcaei()
    {
        try
        {
            var dbOptions = App.GetConfig<DbConnectionOptions>("DbConnection", true);
            List<ConnectionConfig> connectionConfigs = dbOptions.ConnectionConfigs.Where(t => t.ConfigId== "1300000000002").Select(t => new ConnectionConfig
            {
                ConfigId = t.ConfigId,
                ConnectionString = t.ConnectionString,
                IsAutoCloseConnection = true
            }).ToList();

            var db = new SqlSugarClient(connectionConfigs);
            string startDate, endDate;
            int jgsj = 24;   //默认执获取24小时的数据

            startDate = _db.Queryable<Xtcs>().First(t => t.Csdm == "aeidate")?.Cs;  //aei开始时间
            endDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (DateTimeUtil.GetTimeSpan(startDate, endDate, "yyyy-MM-dd HH:mm:ss", "Hour") > jgsj)  //如果时间差大于24个小时，则取24个小时数据
            {
                endDate = DateTime.Parse(startDate).AddHours(jgsj).ToString("yyyy-MM-dd HH:mm:ss");
            }
            string sqlStr = $@"select * from JC_AEI where IN_DB_TIME between '{startDate}' and '{endDate}' order by IN_DB_TIME";
            IList<JcAeiModel> jcAeiList = await db.Ado.SqlQueryAsync<JcAeiModel>(sqlStr);

            if (null != jcAeiList && jcAeiList.Count > 0)
            {
                jcAeiList = jcAeiList.OrderBy(p => p.IN_DB_TIME).ToList<JcAeiModel>();
                return new { success = true, data = jcAeiList, startdate = startDate, enddate = endDate };
            }
            else
            {
                return new { success = false, message = "当前时间段没有数据返回,等待下一次处理", startdate = startDate, enddate = endDate };
            }
        }
        catch (Exception ex)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(ex) + ",等待下一次处理" };
        }
    }
    [HttpGet("api/Zcfs/FindAll")]
    public async Task<dynamic> FindZcfsAll(string param)
    {
        JObject jo = null;
        IList<ISort> sorts = new List<ISort>();
        List<OrderByModel> orders = new List<OrderByModel>();
        if (!string.IsNullOrEmpty(param))
        {
            jo = JObject.Parse(param);
            //sorts.Add(new DapperExtensions.Predicate.Sort { PropertyName = jo["sort"].ToString(), Ascending = true });
            orders.Add(new OrderByModel { FieldName = jo["sort"].ToString(), OrderByType = OrderByType.Asc });
        }
        if (sorts.Count == 0)
        {
            //sorts.Add(new DapperExtensions.Predicate.Sort { PropertyName = "Zcfsdm", Ascending = true });
            orders.Add(new OrderByModel { FieldName = "Zcfsdm", OrderByType = OrderByType.Asc });
        }
        //给定一个默认的排序属性字段
        var users = _db.Queryable<Zcfs>().OrderBy(orders).ToList();
        return new { total = users.Count(), items = users };
    }
    [HttpPost("api/Yt1/Yt1LkjToBdsjDdm_head")]
    public async Task<IEnumerable<BdsjYt1>> Yt1LkjToBdsjDdm_head(RequestForGetYt1BdsjList r)
    {
        #region 获取7天内，没有结合LKJ生成电子报单的数据
        Expressionable<BdsjYt1> expressionable = Expressionable.Create<BdsjYt1>();
        //PredicateGroup pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
        //pg.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.Bdrq, Operator.Ge, DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd")));
        expressionable.And(p => SqlFunc.ToDate(p.Bdrq) <= SqlFunc.ToDate(DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd")));
        //pg.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.Proceeding, Operator.Eq, 0));
        expressionable.And(p => p.Proceeding == 0);
        //pg.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.Jch, Operator.Eq, null, true));  //机车号不为空
        expressionable.And(p => !SqlFunc.IsNullOrEmpty(p.Jch));
        //pg.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.Tq, Operator.Eq, null, true));   //退勤时间不为空                                                                                  //pg.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.Tq, Operator.Le, DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm")));
        expressionable.And(p => !SqlFunc.IsNullOrEmpty(p.Tq));
        //pg.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.Zpddm, Operator.Eq, r.Ddm));
        expressionable.And(p => p.Zpddm == r.Ddm);
        //pg.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.Yt1bdsjid, Operator.Eq, "F7F4E2FA3339645EE053353E030A1B52"));
        var list = _db.Queryable<BdsjYt1>().Where(expressionable.ToExpression()).ToList();
        #endregion
        return list;
    }
    [NonAction]
    public string FindCwyxmByDm2(string cwydm)
    {
        string result = "";
        //IPredicate jwdpredicate = Predicates.Field<Cwyxx>(p => p.Cwydm, Operator.Eq, cwydm);
        Cwyxx cwyxx = _db.Queryable<Cwyxx>().First(p => p.Cwydm == cwydm);
        if (null != cwyxx)
        {
            result = cwyxx.Cwyxm;
        }
        return result;
    }
    //通过机车型号,判断是否是动车
    public bool isDc(string jcxh)
    {
        return ((!string.IsNullOrEmpty(jcxh)) && (jcxh.Length > 3 || (jcxh.Length >= 3 && jcxh.CompareTo("300") >= 0)));
    }
    [NonAction]
    /// <summary>
    /// 对两组报单头数据，整合成一组报单头数据
    /// </summary>
    /// <param name="bdt"></param>
    /// <param name="bdt2"></param>
    /// <param name="jch"></param>
    public void TransBdtData(LkjBdsjBdt bdt, LkjBdsjBdt bdt2, out string jch)
    {
        int bzCount = bdt.bzs.Count;  //第一个报单头的编组个数,用于判断，是否添加“双机固定重联机车”信息
        jch = "";
        /*if (bdt.Jcxh != bdt2.Jcxh)
        {
            return;  //机车型号不同，则返回
        }
        else if (bdt.Jch != bdt2.Jch) //机车号不同
        {*/
        bool isDc1 = isDc(bdt.Jcxh); //是否动车
        bool isDc2 = isDc(bdt2.Jcxh); //是否动车
        if (isDc1 && isDc2) //两个都是动车
        {
            string d1 = bdt.Jch.Substring(bdt.Jch.Length - 1, 1);
            string d2 = bdt2.Jch.Substring(bdt2.Jch.Length - 1, 1);
            string jch1 = bdt.Jch.Substring(0, bdt.Jch.Length - 1);
            string jch2 = bdt2.Jch.Substring(0, bdt2.Jch.Length - 1);
            //如果两个报单头的机车号相同，而且类型是A端或者B端,则给报单赋机车型号
            if ((d1 == "1" || d1 == "0") && (d2 == "1" || d2 == "0") && jch1 == jch2)
            {
                jch = jch1;
            }
            /*
            else
            {
                return;
            }*/
        }/*
                else
                {
                    return; //如果有一个报单头不是动车，则返回
                }
            }*/

        DateTime wjjlsj, wjjssj;
        if (DateTime.TryParseExact(bdt2.Wjjlsj, "yyyy-MM-dd HH:mm:ss", null, DateTimeStyles.None, out wjjlsj) &&
        DateTime.TryParseExact(bdt.Wjjssj, "yyyy-MM-dd HH:mm:ss", null, DateTimeStyles.None, out wjjssj))
        {
            //判断文件建立时间与上一报单文件结束时间，相差大于4小时结束
            /*
            if ((wjjlsj - wjjssj).TotalHours > 4)
            {
                return;
            }*/

            bdt.Wjjssj = bdt2.Wjjssj;   //文件结束时间为最后一个文件的结束时间，也是“交车时间”JIAOC

            List<LkjBdsjBz> bzs = _db.Ado.SqlQuery<LkjBdsjBz>(@"select * from lkj_bdsj_bz where bdtid=@bdtid order by bzcs", new { bdtid = bdt2.Id });
            bdt2.bzs = bzs;
            //GetBdtData(bdt2); //得到编组信息
            if (null == bdt.bzs || bdt.bzs.Count == 0)
            {
                bdt.bzs = bdt2.bzs; //如果上一个文件没有编组信息，则直接把编组信息进行赋值
                return;
            }

            if (null != bdt2.bzs && bdt2.bzs.Count > 0) //如果两个报单头都有编组信息
            {
                //if ((wjjlsj - wjjssj).TotalMilliseconds >= 0)//两个文件有间隔，则判断首尾是否合并
                //{
                #region 尾首合并
                //1.如果第一个文件的最后一条记录和第二个文件的第一条记录，交路号和车站号相同，则合并信息
                //2.或者第一个文件的最后一条记录“交路号”和“车站号”为空，也合并信息
                if (bdt.bzs.Count > 0 &&
                        (
                            (bdt.bzs[bdt.bzs.Count - 1].Sjjlh == bdt2.bzs[0].Sjjlh && bdt.bzs[bdt.bzs.Count - 1].Czh == bdt2.bzs[0].Czh) ||
                            string.IsNullOrEmpty(bdt.bzs[bdt.bzs.Count - 1].Sjjlh) || string.IsNullOrEmpty(bdt.bzs[bdt.bzs.Count - 1].Czh) ||
                            string.IsNullOrEmpty(bdt2.bzs[0].Sjjlh) || string.IsNullOrEmpty(bdt2.bzs[0].Czh) ||
                            //bdt.bzs[bdt.bzs.Count - 1].Sjjlh == "0" || bdt.bzs[bdt.bzs.Count - 1].Czh == "0" ||
                            //bdt2.bzs[0].Sjjlh == "0" || bdt2.bzs[0].Czh == "0" ||
                            (string.IsNullOrEmpty(bdt.bzs[bdt.bzs.Count - 1].Cf) && string.IsNullOrEmpty(bdt.bzs[bdt.bzs.Count - 1].Dd)) ||
                            (string.IsNullOrEmpty(bdt2.bzs[0].Cf) && string.IsNullOrEmpty(bdt2.bzs[0].Dd))
                        )
                   )
                {
                    //如果下一条记录的所有数据都不为空时才合并
                    if (!(string.IsNullOrEmpty(bdt2.bzs[0].Sjjlh) &&
                       string.IsNullOrEmpty(bdt2.bzs[0].Czh) &&
                       string.IsNullOrEmpty(bdt2.bzs[0].Rd) &&
                       string.IsNullOrEmpty(bdt2.bzs[0].Cd)))
                    {
                        if (string.IsNullOrEmpty(bdt2.bzs[0].Sjjlh) &&
                            string.IsNullOrEmpty(bdt2.bzs[0].Czh))  //交路号和车站号都为空时
                        {
                            if (!string.IsNullOrEmpty(bdt2.bzs[0].Cd))  //取最后一个出段时间
                            {
                                bdt.bzs[bdt.bzs.Count - 1].Cd = bdt2.bzs[0].Cd;
                            }
                            //如果前一个入段为空，后一个入段不为空，则赋值
                            if (string.IsNullOrEmpty(bdt.bzs[bdt.bzs.Count - 1].Rd) && !string.IsNullOrEmpty(bdt2.bzs[0].Rd))
                            {
                                bdt.bzs[bdt.bzs.Count - 1].Rd = bdt2.bzs[0].Rd; //得到入段时间
                            }
                        }
                        else
                        {
                            bool isBdtMain = false; //如果前一条记录为主，则为true,否则为false
                            LkjBdsjBz bzMain;      //合并的主记录
                            LkjBdsjBz bzAssist;    //合并的辅记录,出发和到达为空(可能是调车，或者接车后的出站记录)
                                                   //1.上一条和下一条记录出发到达时间都为空，则以下一条为主记录
                            if (string.IsNullOrEmpty(bdt.bzs[bdt.bzs.Count - 1].Cf) && string.IsNullOrEmpty(bdt.bzs[bdt.bzs.Count - 1].Dd) &&
                                string.IsNullOrEmpty(bdt2.bzs[0].Cf) && string.IsNullOrEmpty(bdt2.bzs[0].Dd))
                            {
                                //出站记录和调车，或者调车和调车记录
                                bzMain = bdt2.bzs[0];
                                bzAssist = bdt.bzs[bdt.bzs.Count - 1];
                            }
                            //2.上一条记录出发到达时间都为空，则以下一条为主记录
                            else if (string.IsNullOrEmpty(bdt.bzs[bdt.bzs.Count - 1].Cf) && string.IsNullOrEmpty(bdt.bzs[bdt.bzs.Count - 1].Dd))
                            {
                                bzMain = bdt2.bzs[0];
                                bzAssist = bdt.bzs[bdt.bzs.Count - 1]; //出站记录或者调车记录
                            }
                            //3.下一条记录出发到达时间都为空，则以上一条为主记录
                            else if (string.IsNullOrEmpty(bdt2.bzs[0].Cf) && string.IsNullOrEmpty(bdt2.bzs[0].Dd))
                            {
                                bzMain = bdt.bzs[bdt.bzs.Count - 1];
                                bzAssist = bdt2.bzs[0];  //调车记录
                                isBdtMain = true;
                            }
                            else
                            {
                                //上一条和下一条记录都是正常的记录，则以下一条做为主记录进行合并
                                bzMain = bdt2.bzs[0];
                                bzAssist = bdt.bzs[bdt.bzs.Count - 1];
                            }
                            #region 主记录赋值
                            if (string.IsNullOrEmpty(bzMain.Cf)) bzMain.Cf = bzAssist.Cf;
                            if (string.IsNullOrEmpty(bzMain.Dd)) bzMain.Dd = bzAssist.Dd;
                            bzMain.Jwtcsj += bzAssist.Jwtcsj;//机外停车时间
                            bzMain.Qjtcsj += bzAssist.Qjtcsj;//区间停车时间
                            bzMain.Dcsj += bzAssist.Dcsj;//调车时间
                                                         //陈燕双 2023-03-29 和丰台全科沟通后修改
                                                         //取最后一个出段时间

                            //陈燕双 2024-01-30 全科发现出段时间有出入，原因是2个文件都有出段时间，第二个文件还有交路和车站号，合并时以第二个文件的编组为主文件导致，完善程序。
                            if (isBdtMain)  //前一条记录为主
                            {
                                if (!string.IsNullOrEmpty(bzAssist.Cd))  //取最后一个出段时间
                                {
                                    bzMain.Cd = bzAssist.Cd;
                                }
                                //如果前一个入段为空，后一个入段不为空，则赋值
                                if (string.IsNullOrEmpty(bzMain.Rd) && !string.IsNullOrEmpty(bzAssist.Rd))
                                {
                                    bzMain.Rd = bzAssist.Rd; //得到入段时间
                                }
                            }
                            else //后一条记录为主
                            {
                                if (string.IsNullOrEmpty(bzMain.Cd) && !string.IsNullOrEmpty(bzAssist.Cd))  //主记录为空，次记录不为空时才赋值
                                {
                                    bzMain.Cd = bzAssist.Cd;
                                }
                                //如果前一个入段为空，后一个入段不为空，则赋值
                                if (!string.IsNullOrEmpty(bzAssist.Rd))  //次记录不为空时，直接赋值
                                {
                                    bzMain.Rd = bzAssist.Rd; //得到入段时间
                                }
                            }
                            #endregion

                            bdt.bzs[bdt.bzs.Count - 1] = bzMain;

                            //将第二个文件的第一个编组信息之外的其它编组添加到第一个文件的编组信息中
                            for (int i = 1; i < bdt2.bzs.Count; i++)
                            {
                                bdt.bzs.Add(bdt2.bzs[i]);
                            }
                        }
                    }
                }
                #endregion
                //两个文件有间隔,但交路和车站不同，则直接添加
                #region 去重
                else
                {
                    //bdt.bzs.Add(bdt2.bzs[0]);

                    //将第二个文件的编组信息添加到第一个文件的编组信息中
                    foreach (LkjBdsjBz lbb in bdt2.bzs)
                    {
                        //如果列表中已经存在编组信息，则不添加，否则添加
                        List<LkjBdsjBz> tempBz = bdt.bzs.Where(tb => tb.Sjjlh == lbb.Sjjlh && tb.Czh == lbb.Czh &&
                                                                     tb.Dd == lbb.Dd && tb.Cf == lbb.Cf).ToList();
                        if (tempBz.Count <= 0)
                        {
                            bdt.bzs.Add(lbb);
                        }
                    }
                }
                #endregion
                //}
                //else //两个文件相交
                //{
                //}
            }
        }
        //如果编组有变化，而且不同的机车，则判断“双机固定重联机车”是否已经添加，没有则添加
        //if(bzCount != bdt.bzs.Count)
        //{
        if (bdt.Jcxh != bdt2.Jcxh || bdt.Jch != bdt2.Jch)
        {
            if (null == bdt.Clbdsjs) bdt.Clbdsjs = new List<Clbdsj>();
            Clbdsj clbdsjTmp = bdt.Clbdsjs.Where(m => m.Jx == bdt2.Jxmc && m.Jch == bdt2.Jch).FirstOrDefault();
            if (null == clbdsjTmp)
            {
                Clbdsj clb = new Clbdsj();
                clb.Jx = bdt2.Jxmc;
                clb.Jch = bdt2.Jch;
                bdt.Clbdsjs.Add(clb);
            }
        }

        /*if (!string.IsNullOrEmpty(bdt2.Sjh) && bdt.Sjh != bdt2.Sjh && !bdt.Sjh.Contains(bdt2.Sjh) && !bdt.Fsjh.Contains(bdt2.Sjh))  //正司机
        {
            bdt.Sjh += "," + bdt2.Sjh;
            bdt.Sjxm += "," + bdt2.Sjxm;
        }
        if (!string.IsNullOrEmpty(bdt2.Fsjh) && bdt.Fsjh != bdt2.Fsjh && !bdt.Sjh.Contains(bdt2.Fsjh) && !bdt.Fsjh.Contains(bdt2.Fsjh))  //副司机
        {
            bdt.Fsjh += "," + bdt2.Fsjh;
            bdt.Fsjxm += "," + bdt2.Fsjxm;
        }*/
        //}
    }
    public string FindByJkjx(string jcxh, string jch)
    {
        //IPredicate jxpredicate = Predicates.Field<Jkjx>(p => p.Jkjxdm, Operator.Eq, jcxh);
        IList<Jkjx> jkjxs = _db.Queryable<Jkjx>().Where(p => p.Jkjxdm == jcxh).ToList();
        if (jcxh != "239" && jkjxs != null && jkjxs.Count() == 1)  //1. 239：HXD3CA和HXD3C排除 2.dic_jkjx表多个机型对应同一个jkjxdm
        {
            return jkjxs[0].Jxdm;
        }
        else
        {
            string jkjx = null;
            //IPredicate jchpredicate = Predicates.Field<Jchxx>(p => p.Jch, Operator.Eq, jch);
            IList<Jchxx> jchs = _db.Queryable<Jchxx>().Where(p => p.Jch == jch).ToList();
            //机车号和监控机型进行匹配
            foreach (Jchxx j in jchs)
            {
                foreach (Jkjx jx in jkjxs)
                {
                    if (j.Jx == jx.Jxdm)
                    {

                        jkjx = jx.Jxdm;
                        goto exitJx;
                    }
                }
            }
        exitJx:
            //如果没有找到对应的机型，先通过机车号进行匹配，如果还没有，通过机型进行匹配
            if (string.IsNullOrEmpty(jkjx))
            {
                if (jcxh == "239")  //如果数据库里没有对应的机车号，而且是239的，则直接返回HXD3CA,不然可能和其它机车号相同，导致机型赋值错误。
                    return "HXD3C";
                if (null != jchs && jchs.Count > 0)  //通过机车号匹配机型
                {
                    jkjx = jchs[0].Jx;
                }
                else if (null != jkjxs && jkjxs.Count > 0)  //通过监控机型匹配
                {
                    jkjx = jkjxs[0].Jxdm;
                }
            }
            return jkjx;
        }
    }
    // 通过配属段号找到对应的铁路局代码
    public string FindTljdmByPsdh(string ddm, string param)
    {
        List<Jkjldw> dws = _db.Queryable<Jkjldw>().Where(p => p.Ddm == ddm).OrderBy(p => p.Ord).ToList();
        if (dws == null || dws.Count == 0)
        {
            return "";
        }
        else if (dws.Count == 1)  //如果只有1个监控交路单位，则所有配属局的车都只取这1个监控交路单位对应的车站信息.
        {
            return dws[0].Jkjldwdm;
        }
        else
        {

            //IPredicate ipd = Predicates.Field<Jwdxx>(p => p.JK_PSDH, Operator.Eq, param);

            Jwdxx jwd = _db.Queryable<Jwdxx>().First(p => p.JK_PSDH == param);
            if (jwd != null)
            {
                Jkjldw dw = dws.Where(p => p.Jkjldwdm == jwd.Tljdm).FirstOrDefault();
                if (dw != null)
                {
                    return jwd.Tljdm;
                }
                else
                {
                    return dws[0].Jkjldwdm;
                }
            }
            else
            {
                return dws[0].Jkjldwdm;
            }
        }
    }
    [NonAction]
    public string FindCzmc(string ddm, string sJkjldw, string sJkjlh, string sCzh)
    {
        string result = "";
        Expressionable<Jkcz> expressionable = Expressionable.Create<Jkcz>();
        //IList<IPredicate> predList = new List<IPredicate>();
        // predList.Add(Predicates.Field<Jkcz>(p => p.Jkjldwdm, Operator.Eq, sJkjldw));
        expressionable.And(p => p.Jkjldwdm == sJkjldw);
        //predList.Add(Predicates.Field<Jkcz>(p => p.Jkjlh, Operator.Eq, sJkjlh));
        expressionable.And(p => p.Jkjlh == sJkjlh);
        //predList.Add(Predicates.Field<Jkcz>(p => p.Jkczh, Operator.Eq, sCzh));
        expressionable.And(p => p.Jkczh == sCzh);
        //predList.Add(Predicates.Field<Jkcz>(p => p.Ddm, Operator.Eq, ddm));
        expressionable.And(p => p.Ddm == ddm);
        //IPredicateGroup predGroup = Predicates.Group(GroupOperator.And, predList.ToArray());
        Jkcz jkcz = _db.Queryable<Jkcz>().Where(expressionable.ToExpression()).GroupBy(t => new { t.Jkjldwdm, t.Jkjlh, t.Jkczh, t.Ddm }).First();
        if (jkcz != null)
        {
            result = jkcz.Jkczmc;
        }
        return result;
    }
    [NonAction]
    public Jkcz FindByCzh(string ddm, string sJkjldw, string sJkjlh, string sCzh)
    {
        Jkcz result = new Jkcz();
        IList<Jkcz> jkczs = new List<Jkcz>();
        // 解析字符串
        string sql = $@"select * from dic_jkcz jk left outer join tj_dic_czxx cz on jk.ddm=cz.ddm and jk.czdm=cz.tjczdm
                                where jk.ddm = '" + ddm + "' and jk.Jkjldwdm = '" + sJkjldw + "' and  jk.Jkjlh='" + sJkjlh + "' and jk.Jkczh= '" + sCzh + "'";
        var list = _dapperRepository.Context.Query<Jkcz, TjCzxx, Jkcz>(
                   sql, (jkcz, czxx) =>
                   {
                       //看数据有没有在现有的列表中，没有则加入
                       Jkcz jkczTmp = jkczs.Where(m => m.Id == jkcz.Id).FirstOrDefault();
                       if (null == jkczTmp)
                       {
                           if (czxx != null)
                           {
                               jkcz.Tjczmc = czxx.Czmc;    //得到计统车站名称
                               jkcz.Bdbs = czxx.Bdbs;
                               jkcz.Wdbs = czxx.Wdbs;
                               jkcz.Bdyx = czxx.Bdyx;
                               jkcz.Wdyx = czxx.Wdyx;
                               if (!string.IsNullOrWhiteSpace(czxx.Dblm))
                               {
                                   jkcz.Dblm = czxx.Dblm;
                               }
                           }
                           jkczs.Add(jkcz);
                           jkczTmp = jkcz;
                       }
                       return null;
                   });
        result = jkczs.FirstOrDefault();

        if (result != null)
        {
            return result;
        }
        else
            return new Jkcz();
    }
    //重组编组列表
    [NonAction]
    public List<LkjBdsjBz> RecombinationBz(string ddm, LkjBdsjBdt lkjBdt)
    {
        List<LkjBdsjBz> bzs = lkjBdt.bzs;
        if (null != bzs && bzs.Count > 0)
        {
            List<LkjBdsjBz> lkjbzs = new List<LkjBdsjBz>();

            LkjBdsjBz bzBak = null;  //上一条编组记录的备份
            LkjBdsjBz bzNew = null;  //重组后的新编组数据
            int bzcs = 1;  //编组次数
            for (int i = 0; i < bzs.Count; i++)
            {
                bzNew = new LkjBdsjBz();
                //车次不相同时才填写车次
                if (null == bzBak || bzBak.Cc != bzs[i].Cc)
                {
                    bzNew.Cc = bzs[i].Cc;
                }
                bzNew.Khbb = bzs[i].Khbb;
                bzNew.Khlx = bzs[i].Khlx;
                //编组不相同时才填写编组
                if (null == bzBak || !(bzBak.Zz == bzs[i].Zz && bzBak.Zaiz == bzs[i].Zaiz &&
                    bzBak.Kc == bzs[i].Kc && bzBak.Kongc == bzs[i].Kongc && bzBak.Fyyc == bzs[i].Fyyc &&
                    bzBak.Dck == bzs[i].Dck && bzBak.Ls == bzs[i].Ls && bzBak.Hc == bzs[i].Hc && bzBak.Cc == bzs[i].Cc))
                {
                    bzNew.Zz = bzs[i].Zz;
                    if (bzs[i].Zaiz != 0) bzNew.Zaiz = bzs[i].Zaiz;
                    if (bzs[i].Kc != 0) bzNew.Kc = bzs[i].Kc;
                    if (bzs[i].Zc != 0) bzNew.Zc = bzs[i].Zc;
                    if (bzs[i].Kongc != 0) bzNew.Kongc = bzs[i].Kongc;
                    if (bzs[i].Fyyc != 0) bzNew.Fyyc = bzs[i].Fyyc;
                    if (bzs[i].Dck != 0) bzNew.Dck = bzs[i].Dck;
                    if (bzs[i].Ls != 0) bzNew.Ls = bzs[i].Ls;
                    if (bzs[i].Hc != 0) bzNew.Hc = bzs[i].Hc;
                }
                bzNew.Dd = bzs[i].Dd;
                bzNew.Cf = bzs[i].Cf;
                bzNew.Qjtcsj = bzs[i].Qjtcsj;
                bzNew.Jwtcsj = bzs[i].Jwtcsj;
                bzNew.Sjjlh = bzs[i].Sjjlh;
                bzNew.Srjlh = bzs[i].Srjlh;
                bzNew.Czh = bzs[i].Czh;
                bzNew.Czmc = bzs[i].Czmc;
                bzNew.Dcsj = bzs[i].Dcsj;
                bzNew.Rd = bzs[i].Rd;
                bzNew.Cd = bzs[i].Cd;
                bzNew.Bzcs = bzcs;
                //如果没有"车站名称"，则取监控车站表对应的"车站名称"
                //if (string.IsNullOrWhiteSpace(bzNew.Czmc) && !string.IsNullOrEmpty(bzNew.Czh)) 
                if (!string.IsNullOrEmpty(bzNew.Czh))
                {
                    Jkcz jkcz = FindByCzh(ddm, lkjBdt.Jkjldwdm, bzNew.Sjjlh, bzNew.Czh);
                    if (jkcz != null)
                    {
                        bzNew.Czmc = jkcz.Jkczmc;
                        bzNew.Tjczdm = jkcz.Czdm; //获取统计车站代码
                        bzNew.Tjczmc = jkcz.Tjczmc; //获取统计车站代码
                        bzNew.Dblm = jkcz.Dblm; //获取电报略码
                        bzNew.Bdbs = jkcz.Bdbs;
                        bzNew.Bdyx = jkcz.Bdyx;
                        bzNew.Wdbs = jkcz.Wdbs;
                        bzNew.Wdyx = jkcz.Wdyx;
                    }
                    bzNew.Czmc = string.IsNullOrWhiteSpace(bzNew.Czmc) ? $"{bzNew.Sjjlh}-{bzNew.Czh}" : bzNew.Czmc;

                }
                if (i == 0) //始发站信息
                {
                    lkjBdt.Sfsrjlh = bzNew.Srjlh;
                    lkjBdt.Sfsjjlh = bzNew.Sjjlh;
                    lkjBdt.Sfczh = bzNew.Czh;
                    lkjBdt.Sfczmc = bzNew.Czmc;
                    lkjBdt.Sfcc = bzs[i].Cc;
                }
                if (i == bzs.Count - 1) //终到站信息
                {
                    lkjBdt.Zdsrjlh = bzNew.Srjlh;
                    lkjBdt.Zdsjjlh = bzNew.Sjjlh;
                    lkjBdt.Zdczh = bzNew.Czh;
                    lkjBdt.Zdczmc = bzNew.Czmc;
                    lkjBdt.Zdcc = bzs[i].Cc;
                }

                lkjbzs.Add(bzNew);
                bzBak = bzs[i];
                bzcs++;
            }
            return lkjbzs;
        }
        return bzs;
    }
    [NonAction]
    //查询lkj数据
    public LkjBdsjBdt QueryBdt(string ddm, JObject jo, int minute, QueryBdt_Extend extend = null)
    {
        #region 取得报单头列表信息
        string sjh = jo["Sjdm1"].ToString();

        //sjh_bdt: //如果登录人为副司机，则跳到这里重新取正司机对应的lkj报单头信息
        //司机可能退勤后，再拷贝LKJ数据，所以这里只用文件建立时间作为条件，而没有把文件结束时间做为条件判断
        string sqlText = @"select distinct bdt.* from
                               (
                                   select * from lkj_bdsj_bdt
                                   where wjjlsj between to_char(to_date('{0}:00','yyyy-mm-dd hh24:mi:ss')-20/1440,'yyyy-mm-dd hh24:mi:ss') and '{1}:59'  and 
                                   ((length(sjh)=7 and length(fsjh)=7) or (length(sjh)=4 and length(fsjh)=4))
                                   -- and not(cc like '77777%' or cc like '11111%' or cc like '88888%')
                               ) bdt
                               join
                               (
                                   select * from lkj_bdsj_bdt where (sjh = {2} or fsjh = {2}) and
                                   wjjlsj between to_char(to_date('{0}:00','yyyy-mm-dd hh24:mi:ss')-20/1440,'yyyy-mm-dd hh24:mi:ss') and '{1}:59'
                               ) bdt2 on bdt.sjh = bdt2.sjh or bdt.sjh = bdt2.fsjh or bdt.fsjh = bdt2.sjh 
                                order by bdt.wjjlsj,bdt.wjjssj";

        sqlText = string.Format(sqlText, jo["Cq"].ToString(), jo["Tq"].ToString(), sjh);

        #endregion

        List<LkjBdsjBdt> bdts = _db.Ado.SqlQuery<LkjBdsjBdt>(sqlText).ToList();
        if (bdts.Count() == 0 || bdts == null) return null;

        LkjBdsjBdt lkjBdt = new LkjBdsjBdt();

    #region 排除时间段重复
    repeat:
        //如果只有一个报单头，则读取编组信息，直接返回
        if (bdts.Count() > 1)
        {
            if (minute > 0 && DateTime.Now <= DateTimeUtil.GetDateTime(bdts.Max(p => p.Bdrksj), "yyyy-MM-dd HH:mm:ss").AddMinutes(minute))
            {
                return null;
            }
            //排除时间段包含在其它报单头数据的记录
            foreach (LkjBdsjBdt lbb1 in bdts)
            {
                //如果列表中时间段在其它报单头内，如果当前出段或者入段为空排除，或者包含文件的出段或入段都不为空也排除当前文件
                List<LkjBdsjBdt> tempBdts = bdts.Where(tb => (
                                                                (
                                                                    DateTime.ParseExact(tb.Wjjlsj, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture) <= DateTime.ParseExact(lbb1.Wjjlsj, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture)
                                                                    && DateTime.ParseExact(tb.Wjjssj, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture) >= DateTime.ParseExact(lbb1.Wjjssj, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture)
                                                                    &&
                                                                    (
                                                                        (string.IsNullOrWhiteSpace(lbb1.Cd) && string.IsNullOrWhiteSpace(lbb1.Rd)) ||
                                                                        (!string.IsNullOrWhiteSpace(tb.Cd)) ||
                                                                        (!string.IsNullOrWhiteSpace(tb.Rd))
                                                                    ) && !(tb.Sfczmc == null && null != lbb1.Sfczmc)
                                                                ) ||
                                                                (
                                                                    DateTime.ParseExact(tb.Wjjlsj, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture) == DateTime.ParseExact(lbb1.Wjjlsj, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture)
                                                                    && DateTime.ParseExact(tb.Wjjssj, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture) == DateTime.ParseExact(lbb1.Wjjssj, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture)
                                                                 )
                                                             )
                                                                && tb.Id != lbb1.Id && tb.Jch == lbb1.Jch).ToList();
                if (tempBdts.Count > 0)
                {
                    bdts.Remove(lbb1);
                    goto repeat;
                }
            }
            /*
            if (bdts.Count > 1)
            {
                // 如果第一个报单头文件结束和建立时间小于等于1个小时，而且第一个和第二个报单头文件的机型机车号不同，则把第一个文件删除
                if (DateTimeHelper.GetHoursSpan(bdts[0].Wjjlsj, bdts[0].Wjjssj, "yyyy-MM-dd HH:mm:ss") <= 1 
                        && (bdts[0].Jch != bdts[1].Jch || bdts[0].Jxmc != bdts[1].Jxmc)
                        && string.IsNullOrWhiteSpace(bdts[0].Cd)
                        && string.IsNullOrWhiteSpace(bdts[0].Rd))
                {
                    bdts.Remove(bdts[0]);
                }
            }*/
        }
        #endregion

        // 对于有多个机车号的
        if (extend != null && bdts.GroupBy(x => x.Jch).Count() > 1)
        {
            //对于湖东段V53，做特殊处理。湖东段有站内调车和出入段，可能会出现有两个机车号的情况。
            if (extend.Ddm == "V53")
            {
                string tmpSql = "select * from lkj_bdsj_bz where bdtid in (" + string.Join(",", bdts.Select(x => x.Id).ToList()) + ")";
                var tmpBzList = _db.Ado.SqlQuery<LkjBdsjBz>(tmpSql).ToList();  //所有编组
                var tmpBdts = new List<LkjBdsjBdt>();   //用于临时存放站内调车的报单头文件
                foreach (var item in bdts)
                {
                    //该头文件的编组文件
                    var tmpBzs = tmpBzList.Where(x => x.Bdtid == item.Id).ToList();
                    //如果只有一个编组文件，并且实际交路号和车站号都为空，认为是站内调车
                    if (tmpBzs.Count == 1 && tmpBzs[0].Sjjlh == null && tmpBzs[0].Czh == null)
                    {
                        tmpBdts.Add(item);
                    }
                }

                //排除站内调车的
                var excepts = bdts.Except(tmpBdts);
                //如果排除之后还有2个机车号，说明是真的有两个机车号
                if (excepts.GroupBy(x => x.Jch).Count() > 1)
                {
                    //do nothing
                }
                else
                {
                    //更改站内调车的机车号
                    tmpBdts.ForEach(x => x.Jch = excepts.FirstOrDefault().Jch);
                }

                //标识机车号异常
                if (bdts.GroupBy(x => x.Jch).Count() > 1)
                {
                    extend.Jchyc = "1";
                }
            }
        }

        lkjBdt = bdts[0];

        List<LkjBdsjBz> bzs = _db.Ado.SqlQuery<LkjBdsjBz>(@"select * from lkj_bdsj_bz where bdtid=@bdtid order by bzcs", new { bdtid = lkjBdt.Id });
        //GetBdtData(lkjBdt); //返回第一个报单头对应的编组信息
        lkjBdt.bzs = bzs;
        //翻译乘务员姓名(如果"姓名"为空，则通过"司机号"匹配对应的"姓名")
        if (string.IsNullOrEmpty(lkjBdt.Sjxm) && !string.IsNullOrEmpty(lkjBdt.Sjh))
        {
            lkjBdt.Sjxm = FindCwyxmByDm2(lkjBdt.Sjh);
        }
        if (string.IsNullOrEmpty(lkjBdt.Fsjxm) && !string.IsNullOrEmpty(lkjBdt.Fsjh))
        {
            lkjBdt.Fsjxm = FindCwyxmByDm2(lkjBdt.Fsjh);
        }
        string jch = "";
        for (int i = 1; i < bdts.Count(); i++)
        {
            //翻译乘务员姓名(如果司机号不同，"姓名"为空，则通过"司机号"匹配对应的"姓名")
            if (lkjBdt.Sjh != bdts[i].Sjh && string.IsNullOrEmpty(bdts[i].Sjxm) && !string.IsNullOrEmpty(bdts[i].Sjh))
            {
                bdts[i].Sjxm = FindCwyxmByDm2(bdts[i].Sjh);
            }
            if (!string.IsNullOrEmpty(bdts[i].Fsjh) && lkjBdt.Fsjh != bdts[i].Fsjh && string.IsNullOrEmpty(bdts[i].Fsjxm))
            {
                bdts[i].Fsjxm = FindCwyxmByDm2(bdts[i].Fsjh);
            }

            TransBdtData(lkjBdt, bdts[i], out jch);
        }
        if (jch != "")
            lkjBdt.Jch = jch; //赋动车的机车号

        #region 基础信息匹配名称数据
        //如果"机型名称"为空，则通过"机车型号"匹配对应的"机型名称"
        if (string.IsNullOrEmpty(lkjBdt.Jxmc) && !string.IsNullOrEmpty(lkjBdt.Jcxh))
        {
            lkjBdt.Jxmc = FindByJkjx(lkjBdt.Jcxh, lkjBdt.Jch);
        }

        Jkjx jx = _db.Queryable<Jkjx>().Where(p => p.Jxdm == lkjBdt.Jxmc).First();
        if (null != jx)
        {
            lkjBdt.Db = jx.Db;  //电表数
        }
        if (string.IsNullOrEmpty(lkjBdt.Jkjldwdm)) //如果监控交路单位为空,则重新获取
        {
            string tljdm = FindTljdmByPsdh(ddm, lkjBdt.Psdh); //通过配属段号找到对应的铁路局代码
            if (!string.IsNullOrWhiteSpace(tljdm))
            {
                lkjBdt.Jkjldwdm = tljdm;  //如果铁路局代码不为空，则赋值
            }
        }

        //如果"始发车站名称"为空，则通过"始发车站号"匹配对应的"始发车站名称"
        if (string.IsNullOrEmpty(lkjBdt.Sfczmc) && !string.IsNullOrEmpty(lkjBdt.Sfczh))
        {
            lkjBdt.Sfczmc = FindCzmc(ddm, lkjBdt.Jkjldwdm, lkjBdt.Sfsjjlh, lkjBdt.Sfczh);
        }
        //如果"终到车站名称"为空，则通过"终到车站号"匹配对应的"终到车站名称"
        if (string.IsNullOrEmpty(lkjBdt.Zdczmc) && !string.IsNullOrEmpty(lkjBdt.Zdczh))
        {
            lkjBdt.Zdczmc = FindCzmc(ddm, lkjBdt.Jkjldwdm, lkjBdt.Zdsjjlh, lkjBdt.Zdczh);
        }

        #endregion

        lkjBdt.bzs = RecombinationBz(ddm, lkjBdt);  //重组报单数据，对于数据和上一条相同的，则显示为空

        //lkjBdt.bzs = AddZjgls(lkjBdt.Zpddm,lkjBdt.bzs);
        //处理不足4位机车号,动车的机车号                                   
        while (lkjBdt.Jch.Length < 4)
        {
            lkjBdt.Jch = "0" + lkjBdt.Jch;
        }

        Cwyxx cwy = _db.Queryable<Cwyxx>().First(t => t.Cwydm == lkjBdt.Sjh);
        if (cwy != null)
        {
            lkjBdt.Cjdm = cwy.Cjdm;  //取得乘务员对应的车间代码
            lkjBdt.Cddm = cwy.CheDuiCode;  //取得乘务员对应的车队代码
        }
        return lkjBdt;
    }
    [HttpPost("api/Yt1/Yt1LkjToBdsjDdm_detail")]
    public async Task Yt1LkjToBdsjDdm_detail(RequestForGetYt1BdsjDetail r)
    {
        string ddm = r.Ddm;
        var bdsjYt1 = r.Bdsj;
        int minute = 3;
        #region 重新获取LKJ数据
        JObject jo = new JObject();
        jo.Add("Sjdm1", bdsjYt1.Sjdm1);
        jo.Add("Cq", bdsjYt1.Cq);
        jo.Add("Tq", bdsjYt1.Tq);
        //给定一个默认的排序属性字段
        QueryBdt_Extend extend = new QueryBdt_Extend();
        extend.Ddm = bdsjYt1.Zpddm;
        LkjBdsjBdt Lkjsj = QueryBdt(ddm, jo, minute, extend);

        #endregion

        if (null != Lkjsj && null != Lkjsj.bzs && Lkjsj.bzs.Count > 0)
        {
            #region 得到当前报单明细信息
            BdsjYt1 bdsjYt1Detail = FindYt1("{Id:" + bdsjYt1.Id + "}");
            TypeAdapterConfig<BdsjYt1, Bdsj>.NewConfig().Ignore(t=>t.Id);
            Bdsj bdsj = new Bdsj();
            //Mapper.CreateMap<BdsjYt1, Bdsj>().ForAllMembers(opt => opt.Condition(srs => srs.MemberName != "Id"));
            //Mapper.DynamicMap<BdsjYt1, Bdsj>(bdsjYt1Detail, bdsj);

            bdsj = bdsjYt1Detail.Adapt<Bdsj>();
            #endregion

            #region 报单数据赋值

            Jwdxx jwdxx = _db.Queryable<Jwdxx>().First(p => p.Jwddm == bdsj.Zpddm);
            if (null != jwdxx && jwdxx.Zdbs == "1")
            {
                bdsj.Proceeding = 1;  //发放状态，需要走终端程序
            }
            else
            {
                bdsj.Bdtjsj = bdsjYt1Detail.Eventtime;  //将运统1更新时间赋给乘务员提交报单时间上
                bdsj.Proceeding = 3;
            }

            //出入段重新赋值，丰台全科要求接交车时间和出入段等时间全部以监控为主。
            bdsj.Rbd = null;
            bdsj.Rwd = null;
            bdsj.Cbd = null;
            bdsj.Cwd = null;
            bdsj.Jied = null;
            bdsj.Jiec = null;
            bdsj.Jiaoc = null;

            //bd.Islkj = 1;       
            bdsj.Jczl = Lkjsj.Jclxbs;
            //if (bdsj.Jch == Lkjsj.Jch) //如果机车号相同，则使用监控的机型
            //bdsj.Jch = Lkjsj.Jch;
            //if(!bdsj.Jx.Contains("HXD3C"))
            //bdsj.Jx = Lkjsj.Jxmc;

            bdsj.Bdtjly = "1";
            bdsj.Jkjldwdm = Lkjsj.Jkjldwdm;  //监控交路单位代码
                                             //始发车站
            bdsj.Cjdm = Lkjsj.Cjdm;
            bdsj.Cddm = Lkjsj.Cddm;
            bdsj.Sfsjjlh = Lkjsj.Sfsjjlh;
            bdsj.Sfsrjlh = Lkjsj.Sfsrjlh;
            bdsj.Sfczh = Lkjsj.Sfczh;
            bdsj.Sfczmc = Lkjsj.Sfczmc;
            bdsj.Sfcc = Lkjsj.Sfcc;
            //终到车站
            bdsj.Zdsjjlh = Lkjsj.Zdsjjlh;
            bdsj.Zdsrjlh = Lkjsj.Zdsrjlh;
            bdsj.Zdczh = Lkjsj.Zdczh;
            bdsj.Zdczmc = Lkjsj.Zdczmc;
            bdsj.Zdcc = Lkjsj.Zdcc;
            bdsj.Jcxh = Lkjsj.Jcxh;
            DateTime Jiaoc, Jiec;
            //if (string.IsNullOrWhiteSpace(bdsj.Jiec))
            //{
            if (DateTime.TryParseExact(Lkjsj.Wjjlsj, "yyyy-MM-dd HH:mm:ss", null, DateTimeStyles.None, out Jiec))
            {
                bdsj.Jiec = Jiec.ToString("yyyy-MM-dd HH:mm");
            }
            //}

            //if (string.IsNullOrWhiteSpace(bdsj.Jiaoc))
            //{
            if (DateTime.TryParseExact(Lkjsj.Wjjssj, "yyyy-MM-dd HH:mm:ss", null, DateTimeStyles.None, out Jiaoc))
            {
                bdsj.Jiaoc = Jiaoc.ToString("yyyy-MM-dd HH:mm");
            }
            //}

            if (null == bdsj.Qrcode || bdsj.Qrcode.Contains("N")) //有可能读了lkj数据，又读ic卡数据，N开头的数据正司机可能会有变化
            {
                bdsj.Bdrq = bdsj.Cq.Substring(0, 10);
                bdsj.Qrcode = bdsj.Cq.Replace("-", "").Replace(":", "").Replace(" ", "") + bdsj.Sjdm1;
            }
            #endregion

            #region 编组数据赋值
            if (null != Lkjsj.bzs && Lkjsj.bzs.Count > 0)
            {
                List<Bzsj> bzsjs = new List<Bzsj>();
                //string lkjcc = "";
                //string yt1cc = "";
                bool iscc = false;
                foreach (LkjBdsjBz lkjBzsj in Lkjsj.bzs)
                {
                    //zbzs = new ObservableCollection<Zbzsj>();
                    Bzsj Bzsj = new Bzsj();
                    Bzsj.Cc = lkjBzsj.Cc;
                    Bzsj.Khbb = lkjBzsj.Khbb;
                    Bzsj.Khlx = lkjBzsj.Khlx;
                    //if (bz.Zz != 0)
                    if (lkjBzsj.Zz != null && lkjBzsj.Zz != 0)   //总重不为0时，才赋值
                    {
                        Bzsj.Zz = lkjBzsj.Zz;
                        //if (bz.Zaiz != 0)
                        Bzsj.Zaiz = lkjBzsj.Zaiz;
                        //if (bz.Kc != 0) 
                        Bzsj.Kc = lkjBzsj.Kc;
                        //if (bz.Zc != 0) 
                        Bzsj.Zc = lkjBzsj.Zc;
                        //if (bz.Kongc != 0) 
                        Bzsj.Kongc = lkjBzsj.Kongc;
                        //if (bz.Fyyc != 0) 
                        Bzsj.Fyyc = lkjBzsj.Fyyc;
                        //if (bz.Dck != 0) 
                        Bzsj.Dkc = lkjBzsj.Dck;
                        //if (bz.Ls != 0) 
                        Bzsj.Ls = lkjBzsj.Ls;
                        //if (bz.Hc != 0) 
                        Bzsj.Hc = lkjBzsj.Hc;
                    }

                    Bzsj.Dd = string.IsNullOrEmpty(lkjBzsj.Dd) ? "" : lkjBzsj.Dd.Substring(0, 16);
                    Bzsj.Cf = string.IsNullOrEmpty(lkjBzsj.Cf) ? "" : lkjBzsj.Cf.Substring(0, 16);
                    Bzsj.Qjtcsj = DateTimeUtil.MinutesToTime((int)Math.Round(lkjBzsj.Qjtcsj.GetValueOrDefault(0) / 60.0, MidpointRounding.AwayFromZero));
                    Bzsj.Jwtcsj = DateTimeUtil.MinutesToTime((int)Math.Round(lkjBzsj.Jwtcsj.GetValueOrDefault(0) / 60.0, MidpointRounding.AwayFromZero));
                    Bzsj.Sjjlh = lkjBzsj.Sjjlh;
                    Bzsj.Srjlh = lkjBzsj.Srjlh;
                    Bzsj.Czh = lkjBzsj.Czh;
                    Bzsj.Czmc = lkjBzsj.Czmc;
                    Bzsj.Tjczdm = lkjBzsj.Tjczdm;
                    Bzsj.Tjczmc = lkjBzsj.Tjczmc;
                    Bzsj.Qjgl = lkjBzsj.Qjgl;
                    Bzsj.Dcsj = DateTimeUtil.MinutesToTime((int)Math.Round(lkjBzsj.Dcsj.GetValueOrDefault(0) / 60.0, MidpointRounding.AwayFromZero));
                    Bzsj.Dcfs = "";
                    Bzsj.Ddqy = "";
                    Bzsj.Rd = lkjBzsj.Rd;
                    Bzsj.Cd = lkjBzsj.Cd;
                    Bzsj.Jkdw = "";
                    Bzsj.Bzcs = lkjBzsj.Bzcs;
                    Bzsj.Qsyt1id = "";
                    Bzsj.Kbsjid = "";

                    //取得当前车站对应的运统一信息,如果运统一车站和监控车站相同、或者统计车站相同、或者车次相同，则认为是同一车站
                    List<Bzsj> yt1Bzsjs = bdsj.Bzsjs.Where(p => (p.Cc != null && Bzsj.Cc != null && (p.Cc.Contains(Bzsj.Cc) || Bzsj.Cc.Contains(p.Cc))) || p.Czh == lkjBzsj.Dblm || p.Czmc == Bzsj.Czmc || p.Czmc == Bzsj.Tjczmc).ToList();
                    if (yt1Bzsjs != null && yt1Bzsjs.Count > 0)
                    {
                        foreach (Bzsj yt1Bzsj in yt1Bzsjs)
                        {
                            iscc = false;
                            if (Bzsj.Cc != null && yt1Bzsj.Cc != null)
                            {
                                //lkjcc = System.Text.RegularExpressions.Regex.Replace(Bzsj.Cc, @"[^0-9]+", "");
                                //yt1cc = System.Text.RegularExpressions.Regex.Replace(yt1Bzsj.Cc, @"[^0-9]+", "");
                                if (yt1Bzsj.Cc.Contains(Bzsj.Cc) || Bzsj.Cc.Contains(yt1Bzsj.Cc))
                                    iscc = true;
                            }
                            //如果车次相同，或者lkj和运统一都有总重，而且出发时间或者到达时间在30分钟之内，都更替换为运统一编组
                            if (iscc)
                            {
                                //运统一和LKJ都有总重时，才替换运统一乙数据
                                if (yt1Bzsj.Zz != null && yt1Bzsj.Zz != 0) // && Bzsj.Zz != null && Bzsj.Zz != 0)
                                {
                                    Bzsj.Cljc = yt1Bzsj.Cljc;
                                    Bzsj.Zbzsjs = yt1Bzsj.Zbzsjs == null ? new List<Zbzsj>() : yt1Bzsj.Zbzsjs;
                                    Bzsj.Zz = yt1Bzsj.Zz;
                                    Bzsj.Zaiz = yt1Bzsj.Zaiz;
                                    Bzsj.Kc = yt1Bzsj.Kc;
                                    Bzsj.Ddqy = yt1Bzsj.Ddqy;
                                    Bzsj.Qzjg = yt1Bzsj.Qzjg;
                                    Bzsj.Zc = yt1Bzsj.Zc;
                                    Bzsj.Kongc = yt1Bzsj.Kongc;
                                    Bzsj.Fyyc = yt1Bzsj.Fyyc;
                                    Bzsj.Dkc = yt1Bzsj.Dkc;
                                    Bzsj.Qt = yt1Bzsj.Qt;
                                    Bzsj.Ls = yt1Bzsj.Ls;
                                    Bzsj.Hc = yt1Bzsj.Hc;
                                }
                                else if ((yt1Bzsj.Zz == null || yt1Bzsj.Zz == 0) && yt1Bzsj.Zbzsjs != null)
                                {
                                    //如果相同车次，运统1总重为0，而且运统1乙有数据，我们取监控的编组和运统1乙的信息进行合成
                                    Bzsj.Zbzsjs = yt1Bzsj.Zbzsjs == null ? new List<Zbzsj>() : yt1Bzsj.Zbzsjs;
                                }
                                break;
                            }
                            else if (
                                            yt1Bzsj.Zz != null && yt1Bzsj.Zz != 0 && Bzsj.Zz != null && Bzsj.Zz != 0 &&
                                            (
                                                (!string.IsNullOrWhiteSpace(yt1Bzsj.Cf) && !string.IsNullOrWhiteSpace(lkjBzsj.Cf) &&
                                                DateTimeUtil.GetMinutesSpan(yt1Bzsj.Cf, lkjBzsj.Cf, "yyyy-MM-dd HH:mm:ss") <= 30 &&
                                                DateTimeUtil.GetMinutesSpan(yt1Bzsj.Cf, lkjBzsj.Cf, "yyyy-MM-dd HH:mm:ss") >= -30)
                                                ||
                                                (!string.IsNullOrWhiteSpace(yt1Bzsj.Dd) && !string.IsNullOrWhiteSpace(lkjBzsj.Dd) &&
                                                DateTimeUtil.GetMinutesSpan(yt1Bzsj.Dd, lkjBzsj.Dd, "yyyy-MM-dd HH:mm:ss") <= 30 &&
                                                DateTimeUtil.GetMinutesSpan(yt1Bzsj.Dd, lkjBzsj.Dd, "yyyy-MM-dd HH:mm:ss") >= -30)
                                            )
                                        )
                            {
                                //运统一和LKJ都有总重时，才替换运统一乙数据
                                //监控可能存在多个相同站，时间也隔的很近，所以我们要通过总重判断，否则继续循环
                                if (yt1Bzsj.Zz != null && yt1Bzsj.Zz != 0) // && Bzsj.Zz != null && Bzsj.Zz != 0)
                                {
                                    Bzsj.Cljc = yt1Bzsj.Cljc;
                                    Bzsj.Zbzsjs = yt1Bzsj.Zbzsjs == null ? new List<Zbzsj>() : yt1Bzsj.Zbzsjs;
                                    Bzsj.Zz = yt1Bzsj.Zz;
                                    Bzsj.Zaiz = yt1Bzsj.Zaiz;
                                    Bzsj.Kc = yt1Bzsj.Kc;
                                    Bzsj.Ddqy = yt1Bzsj.Ddqy;
                                    Bzsj.Qzjg = yt1Bzsj.Qzjg;
                                    Bzsj.Zc = yt1Bzsj.Zc;
                                    Bzsj.Kongc = yt1Bzsj.Kongc;
                                    Bzsj.Fyyc = yt1Bzsj.Fyyc;
                                    Bzsj.Dkc = yt1Bzsj.Dkc;
                                    Bzsj.Qt = yt1Bzsj.Qt;
                                    Bzsj.Ls = yt1Bzsj.Ls;
                                    Bzsj.Hc = yt1Bzsj.Hc;
                                }
                                break;
                            }
                        }
                    }
                    bzsjs.Add(Bzsj);

                }
                //编组第一条记录，出段时间：在接车和出发时间之间
                if (!string.IsNullOrEmpty(Lkjsj.bzs[0].Cf) && !string.IsNullOrEmpty(bdsj.Jiec) &&
                    (!string.IsNullOrEmpty(Lkjsj.bzs[0].Cd) || !string.IsNullOrEmpty(Lkjsj.bzs[0].Rd)))
                {
                    string cd = "";
                    if (string.IsNullOrEmpty(Lkjsj.bzs[0].Cd))
                        cd = Lkjsj.bzs[0].Rd;
                    else if (string.IsNullOrEmpty(Lkjsj.bzs[0].Rd))
                        cd = Lkjsj.bzs[0].Cd;
                    else
                        cd = string.Compare(Lkjsj.bzs[0].Cd, Lkjsj.bzs[0].Rd) > 0 ? Lkjsj.bzs[0].Cd : Lkjsj.bzs[0].Rd; //取最大值

                    if (string.Compare(Lkjsj.bzs[0].Cf, cd) > 0 && string.Compare(cd, bdsj.Jiec) > 0)
                    {
                        //1.本段，而且有对应车间，则为本段
                        if (Lkjsj.bzs[0].Bdbs == true)
                        {
                            if (string.IsNullOrEmpty(Lkjsj.bzs[0].Bdyx)) //本段所有车间
                            {
                                if (Lkjsj.bzs[0].Wdbs == true)
                                {
                                    //如果外段对应当前车间，则为出外段
                                    if (!string.IsNullOrEmpty(Lkjsj.bzs[0].Wdyx) && !string.IsNullOrEmpty(Lkjsj.Cjdm) && (Lkjsj.bzs[0].Wdyx + ",").Contains(Lkjsj.Cjdm + ","))
                                    {
                                        bdsj.Cwd = cd.Substring(0, 16);   //出外段
                                    }
                                    else
                                    {
                                        bdsj.Cbd = cd.Substring(0, 16);   //出本段
                                    }
                                }
                                else
                                {
                                    //不属于外段，则所有车间都是本段。
                                    bdsj.Cbd = cd.Substring(0, 16);   //出本段
                                }
                            }
                            //本段对应车间包含本站，则为本段
                            else if (!string.IsNullOrEmpty(Lkjsj.Cjdm) && (Lkjsj.bzs[0].Bdyx + ",").Contains(Lkjsj.Cjdm + ","))
                            {
                                bdsj.Cbd = cd.Substring(0, 16);   //出本段
                            }
                            else
                            {
                                if (Lkjsj.bzs[0].Wdbs == true)
                                {
                                    //如果外段对应当前车间，则为出外段
                                    if (!string.IsNullOrEmpty(Lkjsj.bzs[0].Wdyx) && !string.IsNullOrEmpty(Lkjsj.Cjdm) && (Lkjsj.bzs[0].Wdyx + ",").Contains(Lkjsj.Cjdm + ","))
                                    {
                                        bdsj.Cwd = cd.Substring(0, 16);   //出外段
                                    }
                                    else
                                    {
                                        bdsj.Cbd = cd.Substring(0, 16);   //出本段
                                    }
                                }
                                else
                                {
                                    //不属于外段，则所有车间都是本段。
                                    bdsj.Cbd = cd.Substring(0, 16);   //出本段
                                }
                            }
                        }
                        else if (Lkjsj.bzs[0].Wdbs == true)
                        {
                            bdsj.Cwd = cd.Substring(0, 16);   //出外段
                        }
                        else
                        {
                            bdsj.Cbd = cd.Substring(0, 16);   //出本段
                        }
                    }
                }

                int intLastNum = Lkjsj.bzs.Count - 1;
                //编组最后一条记录，入段时间：在到达和交车时间之间
                if (!string.IsNullOrEmpty(Lkjsj.bzs[intLastNum].Dd) && !string.IsNullOrEmpty(bdsj.Jiaoc) &&
                    (!string.IsNullOrEmpty(Lkjsj.bzs[intLastNum].Cd) || !string.IsNullOrEmpty(Lkjsj.bzs[intLastNum].Rd)))
                {
                    string rd = "";
                    if (string.IsNullOrEmpty(Lkjsj.bzs[intLastNum].Cd))
                        rd = Lkjsj.bzs[intLastNum].Rd;
                    else if (string.IsNullOrEmpty(Lkjsj.bzs[intLastNum].Rd))
                        rd = Lkjsj.bzs[intLastNum].Cd;
                    else
                        rd = string.Compare(Lkjsj.bzs[intLastNum].Cd, Lkjsj.bzs[intLastNum].Rd) > 0 ? Lkjsj.bzs[intLastNum].Rd : Lkjsj.bzs[intLastNum].Cd; //取最小值

                    if (string.Compare(rd, Lkjsj.bzs[intLastNum].Dd) > 0 && string.Compare(bdsj.Jiaoc, rd) > 0)
                    {
                        //1.本段，而且有对应车间，则为本段
                        if (Lkjsj.bzs[intLastNum].Bdbs == true)
                        {
                            if (string.IsNullOrEmpty(Lkjsj.bzs[intLastNum].Bdyx)) //本段所有车间
                            {
                                if (Lkjsj.bzs[intLastNum].Wdbs == true)
                                {
                                    //如果外段对应当前车间，则为出外段
                                    if (!string.IsNullOrEmpty(Lkjsj.bzs[intLastNum].Wdyx) && !string.IsNullOrEmpty(Lkjsj.Cjdm) && (Lkjsj.bzs[intLastNum].Wdyx + ",").Contains(Lkjsj.Cjdm + ","))
                                    {
                                        bdsj.Rwd = rd.Substring(0, 16);   //入段时间
                                    }
                                    else
                                    {
                                        bdsj.Rbd = rd.Substring(0, 16);   //入段时间
                                    }
                                }
                                else
                                {
                                    //不属于外段，则所有车间都是本段。
                                    bdsj.Rbd = rd.Substring(0, 16);   //入段时间
                                }
                            }
                            //本段对应车间包含本站，则为本段
                            else if (!string.IsNullOrEmpty(Lkjsj.Cjdm) && (Lkjsj.bzs[intLastNum].Bdyx + ",").Contains(Lkjsj.Cjdm + ","))
                            {
                                bdsj.Rbd = rd.Substring(0, 16);   //入段时间
                            }
                            else
                            {
                                if (Lkjsj.bzs[intLastNum].Wdbs == true)
                                {
                                    //如果外段对应当前车间，则为出外段
                                    if (string.IsNullOrEmpty(Lkjsj.bzs[intLastNum].Wdyx) || (!string.IsNullOrEmpty(Lkjsj.bzs[intLastNum].Wdyx) && !string.IsNullOrEmpty(Lkjsj.Cjdm) && (Lkjsj.bzs[intLastNum].Wdyx + ",").Contains(Lkjsj.Cjdm + ",")))
                                    {
                                        bdsj.Rwd = rd.Substring(0, 16);   //入段时间
                                    }
                                    else
                                    {
                                        bdsj.Rbd = rd.Substring(0, 16);   //入段时间
                                    }
                                }
                                else
                                {
                                    //不属于外段，则所有车间都是本段。
                                    bdsj.Rbd = rd.Substring(0, 16);   //入段时间
                                }
                            }
                        }
                        else if (Lkjsj.bzs[intLastNum].Wdbs == true)
                        {
                            bdsj.Rwd = rd.Substring(0, 16);   //入段时间
                        }
                        else
                        {
                            bdsj.Rbd = rd.Substring(0, 16);   //入段时间
                        }

                    }
                    //交车：没有入库，拔卡时间和到达时间在十分钟之内，将到达时间赋给交车时间上。 有入库，交车就是拔卡时间。
                    /*if (string.IsNullOrEmpty(rd) && !string.IsNullOrWhiteSpace(Lkjsj.bzs[intLastNum].Dd) &&
                                                DateTimeHelper.GetMinutesSpan(Lkjsj.bzs[intLastNum].Dd, rd, "yyyy-MM-dd HH:mm:ss") >=10)
                    {
                        bdsj.Jiaoc = Lkjsj.bzs[intLastNum].Dd.Substring(0, 16);
                    }*/

                }
                bdsj.Bzsjs = bzsjs;
            }
            #endregion

            //如果没有出段信息，则将接车赋给接点。
            if (string.IsNullOrEmpty(bdsj.Cbd) && string.IsNullOrEmpty(bdsj.Cwd))
            {
                bdsj.Jied = bdsj.Jiec;
            }
            IList<Jchxx> jchs = await _db.Ado.SqlQueryAsync<Jchxx>("select * from dic_jch where jch = @Jch and jx = @Jx", new { Jch = bdsj.Jch, Jx = bdsj.Jx });
            /*if(jchs==null || jchs.Count<=0)
            {
                bdsj.IsWjch == "1"
            }*/
            if (jchs == null || jchs.Count <= 0) //数据库无此机车号
            {
                if (bdsj.Jch != Lkjsj.Jch || bdsj.Jx != Lkjsj.Jxmc)
                {
                    if (Lkjsj.Clbdsjs != null && Lkjsj.Clbdsjs.Count > 0 && Lkjsj.Clbdsjs[0].Jch == bdsj.Jch && Lkjsj.Clbdsjs[0].Jx == bdsj.Jx)
                    {
                        Lkjsj.Clbdsjs[0].Jch = Lkjsj.Jch;
                        Lkjsj.Clbdsjs[0].Jx = Lkjsj.Jxmc;
                    }
                    else
                    {
                        bdsj.Jx = Lkjsj.Jxmc;
                        bdsj.Jch = Lkjsj.Jch;
                    }
                }

                bdsj.Clbdsjs = new List<Clbdsj>();
                if (Lkjsj.Clbdsjs != null && Lkjsj.Clbdsjs.Count > 0)
                {
                    foreach (Clbdsj cl in Lkjsj.Clbdsjs)
                    {
                        //无人重联机车机型和本务机车机型相同时才添加
                        if (bdsj.Jx == cl.Jx && bdsj.Jch != cl.Jch)
                        {
                            bdsj.Clbdsjs.Add(cl);
                        }
                    }
                }
            }
            //当前报单重联机车为空，而且监控不为空时进行赋值。
            else if (Lkjsj.Clbdsjs != null && Lkjsj.Clbdsjs.Count > 0)
            {
                //如果本务机车和监控中的无人重联相同，则将监控中的机型和机车号作为无人重联机车
                if (Lkjsj.Clbdsjs[0].Jch == bdsj.Jch && Lkjsj.Clbdsjs[0].Jx == bdsj.Jx)
                {
                    Lkjsj.Clbdsjs[0].Jch = Lkjsj.Jch;
                    Lkjsj.Clbdsjs[0].Jx = Lkjsj.Jxmc;
                }
                bdsj.Clbdsjs = new List<Clbdsj>();
                foreach (Clbdsj cl in Lkjsj.Clbdsjs)
                {
                    //无人重联机车机型和本务机车机型相同时才添加
                    if (bdsj.Jx == cl.Jx && bdsj.Jch != cl.Jch)
                    {
                        bdsj.Clbdsjs.Add(cl);
                    }
                }
            }
            if (bdsj.Clbdsjs != null && bdsj.Clbdsjs.Count > 0)
            {
                //添加无人重联机型和机车号，机车统计程序输入机型机车号和报单日期时，对于无人重联的报单，输入其中任何一辆机车，都可以把报单调出来。
                bdsj.Jx_Wrcl = bdsj.Clbdsjs[0].Jx;
                bdsj.Jch_Wrcl = bdsj.Clbdsjs[0].Jch;
            }

            bdsj.Jchsfyc = extend.Jchyc;
            //bdsj.Sfytq = "0";
            InsertBdsj(bdsj, bdsjYt1);
        }
    }
    [NonAction]
    /// <summary>
    ///运统1报单结合LKJ数据生成电子报单,添加入库
    /// </summary>
    /// <param name="bdsj"></param>
    public dynamic InsertBdsj(Bdsj bdsj, BdsjYt1 bdsjYt1)
    {
        try
        {

            Bdsj bdsjOld = _db.Queryable<Bdsj>().First(p => p.Yt1bdsjid == bdsj.Yt1bdsjid);
            //数据库中如果没有此记录，才自动生成
            if (null == bdsjOld)
            {
                //删除报单头部信息和报单编组信息
                //var bdsj1 = BdsjRepository.ExecuteSqlCommand($" DELETE FROM DAT_BDSJ WHERE YT1BDSJID='{bdsj.Yt1bdsjid}'");

                /*Cwyxx cwy = GetCwyxx(bdsj.Sjdm1);
                if (cwy != null)
                {
                    bdsj.Cjdm = cwy.Cjdm;  //取得乘务员对应的车间代码
                }*/

                if (((bdsj.Jsl1 != null && bdsj.Jsl1 > 0) || (bdsj.Jsl2 != null && bdsj.Jsl2 > 0)) && bdsj.Bzsjs != null && bdsj.Bzsjs.Count > 0)
                {
                    bdsj.Qxbdbs = "0"; //正常报单
                }
                else if (((bdsj.Jsl1 != null && bdsj.Jsl1 > 0) || (bdsj.Jsl2 != null && bdsj.Jsl2 > 0)) && (bdsj.Bzsjs == null || bdsj.Bzsjs.Count == 0))
                {
                    bdsj.Qxbdbs = "1"; //无运行信息
                }
                else if (((bdsj.Jsl1 == null || bdsj.Jsl1 == 0) && (bdsj.Jsl2 == null || bdsj.Jsl2 == 0)) && bdsj.Bzsjs != null && bdsj.Bzsjs.Count > 0)
                {
                    bdsj.Qxbdbs = "2"; //无燃料信息
                }
                else if (((bdsj.Jsl1 == null || bdsj.Jsl1 == 0) && (bdsj.Jsl2 == null || bdsj.Jsl2 == 0)) && (bdsj.Bzsjs == null || bdsj.Bzsjs.Count == 0))
                {
                    bdsj.Qxbdbs = "3"; //无数据
                }
                _db.Ado.BeginTran();
                //新增报单数据
                var bid = _db.Insertable<Bdsj>(bdsj).ExecuteCommand();

                #region 新增联控机车数据
                if (bdsj.Clbdsjs != null)
                {
                    foreach (var item in bdsj.Clbdsjs)
                    {
                        item.Bdsjid = bid;
                    }
                    _db.Insertable<Clbdsj>(bdsj.Clbdsjs).ExecuteCommand();
                }
                #endregion

                #region 新增编组数据
                if (bdsj.Bzsjs != null)
                {
                    foreach (var item in bdsj.Bzsjs)
                    {
                        item.Bdsjid = bid;
                        var bzsjid = _db.Insertable<Bzsj>(item).ExecuteReturnEntity().Id;
                        if (item.Zbzsjs != null)
                        {
                            //新增编组子数据
                            foreach (var item1 in item.Zbzsjs)
                            {
                                item1.Bzsjid = bzsjid;
                            }
                            item.Zbzsjs.OrderBy(p => p.Bzlx).ToList();
                            _db.Insertable<Zbzsj>(item.Zbzsjs);
                        }
                    }
                }
                #endregion

                #region 更新运统一数据状态
                bdsjYt1.Proceeding = 4;
                _db.Updateable<BdsjYt1>(bdsjYt1).ExecuteCommand();
                #endregion

                _db.Ado.CommitTran();
            }
            return new { success = true, message = "新增成功！" };
        }
        catch (Exception e)
        {
            _db.Ado.RollbackTran();
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    [NonAction]
    /// <summary>
    /// 运统一出勤报单数据转换成报单数据
    /// </summary>
    /// <param name="yt1Bdsj"></param>
    /// <returns></returns>
    public BdsjYt1 CovertYt1Cqxx(Yt1Bdsj yt1Bdsj)
    {
        BdsjYt1 bdsj = null;
        try
        {
            if (!string.IsNullOrEmpty(yt1Bdsj.Sjdm1) && !string.IsNullOrEmpty(yt1Bdsj.Bdrq))
            {
                #region 能耗处理
                yt1Bdsj.Jsl1 = null;
                yt1Bdsj.Jrl1 = null;
                yt1Bdsj.Jcl1 = null;
                yt1Bdsj.Zsdjsl1 = null;
                yt1Bdsj.Zsdjcl1 = null;
                yt1Bdsj.Lcgdjsl1 = null;
                yt1Bdsj.Lcgdjcl1 = null;
                yt1Bdsj.Jsl2 = null;
                yt1Bdsj.Jrl2 = null;
                yt1Bdsj.Jcl2 = null;
                yt1Bdsj.Zsdjsl2 = null;
                yt1Bdsj.Zsdjcl2 = null;
                yt1Bdsj.Lcgdjsl2 = null;
                yt1Bdsj.Lcgdjcl2 = null;
                yt1Bdsj.Dwsj = null;
                yt1Bdsj.Dwyl = null;
                #endregion
                TypeAdapterConfig<Yt1Bdsj, BdsjYt1>.NewConfig().Ignore(t=>t.Id);
                bdsj = yt1Bdsj.Adapt<BdsjYt1>();
                bdsj.Yt1bdsjid = yt1Bdsj.Id;
                bdsj.Proceeding = 0;
            }
        }
        catch (Exception ex)
        { }
        return bdsj;
    }
    [NonAction]
    private string GetZcfsdm(string zcfsmc)
    {
        string zcfsdm = "";
        switch (zcfsmc)
        {
            case "标准班":
                zcfsdm = "1";
                break;
            case "单班单司机":
                zcfsdm = "2";
                break;
            case "双班单司机":
                zcfsdm = "3";
                break;
        }
        return zcfsdm;
    }
    /// <summary>
    /// 添加运统一出勤信息
    /// </summary>
    /// <param name="yt1Bdt"></param>
    public void InsertYt1Cqxx(Yt1Bdsj yt1Bdt)
    {
        if (yt1Bdt != null)
        {
            BdsjYt1 bdsj = CovertYt1Cqxx(yt1Bdt);
            bdsj.Zcfsdm = GetZcfsdm(bdsj.Zcfsmc);  //得到执乘方式代码
            Cwyxx cwy = _db.Queryable<Cwyxx>().First(t => t.Cwydm == bdsj.Sjdm1);
            if (cwy != null)
            {
                bdsj.Cjdm = cwy.Cjdm;  //取得乘务员对应的车间代码
            }
            _db.Insertable<BdsjYt1>(bdsj).ExecuteCommand();
        }
    }
    /// <summary>
    /// 获取运统一报单明细信息
    /// </summary>
    /// <param name="yt1Bdt">运统一报单头信息</param>
    /// <returns></returns>
    public Yt1Bdsj GetYt1BdsjById(Yt1Bdsj yt1Bdt, out string data_code, out string login_code)
    {
        //data_code = "";
        //login_code = "";
        //return getYt1data();
        Yt1Bdsj result = null;
        data_code = "";   //获取数据时，返回的code值，如果code等于403，则不再访问运统一
        login_code = "";  //获取token时，返回的code值，如果code等于403，则不再访问运统一
        //JsonSerializerSettings settings = new JsonSerializerSettings()
        //{
        //    ContractResolver = new CamelCasePropertyNamesContractResolver(),
        //    Converters = new List<JsonConverter> { new CamelCaseOnlyConverter() }
        //};

        if (null == xtcss || xtcss.Count() <= 0)
        {
            xtcss = _db.Queryable<Xtcs>().ToList();
        }

        string url = xtcss.FirstOrDefault(p => p.Csdm.Equals("yt1url"))?.Cs;
        string username = xtcss.FirstOrDefault(p => p.Csdm.Equals("cwqsusername"))?.Cs;
        string password = xtcss.FirstOrDefault(p => p.Csdm.Equals("cwqspassword"))?.Cs;
        Xtcs xtcs = xtcss.Where(p => p.Csdm.Equals("cwqstoken")).FirstOrDefault();
        string token = xtcs.Cs;
        object postBody = "{\n    \"id\": \"" + yt1Bdt.Id + "\",\n    \"pageNum\": 1\n}";
        string msg = "";
        IRestConnection restConnection = new RestConnection(url);
    bdsjid:

        DateTime d1 = DateTime.Now;
        JObject jo = restConnection.Post("/cwqsws/bdsjid/get", postBody, token);
        DateTime d2 = DateTime.Now;
        double sub1 = d2.Subtract(d1).TotalMilliseconds;
        yt1datagettime += sub1;
        yt1datagetcounts += 1;

        data_code = jo.Value<string>("code");
        if (data_code == "401")
        {
            "---报单明细---获取token开始".LogInformation();
            bool loginStatus = restConnection.Login(@"/cwqsws/auth/login", "username", username, "password", password, out msg, out login_code, out token);
            ("---报单明细---获取token结束,logincode:" + login_code + ",消息:" + msg + ",token" + token).LogInformation();
            if (loginStatus == false)
            {
                "---报单明细结束---登录异常".LogInformation();
                return null;
            }
            try
            {
                xtcs.Cs = token;
                _db.Updateable<Xtcs>(xtcs).ExecuteCommand();
                goto bdsjid;  //登录成功后，跳转到标签处，重新获取数据
            }
            catch (Exception ex)
            {
                ("---报单明细结束---保存token异常:" + ex.Message).LogInformation();
                return null;
            }
        }
        else if (data_code == "200")  //访问成功
        {
            try
            {
                string data = StringSecurity.UnZipByteToJson(jo.Value<string>("data"));
                JObject jData = JObject.Parse(data);
                string bdsj = jData["content"].ToString();
                ("明细json开始" + bdsj + "明细json开始结束").LogInformation();
                result = JsonConvert.DeserializeObject<List<Yt1Bdsj>>(bdsj,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Serialize
                    }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ("---报单明细结束---列表数据转换异常" + ex.Message + ",bdid: " + yt1Bdt.Id + "json:" + jo.Value<string>("data")).LogInformation();
                return null;
            }
        }
        else
        {
            ("---报单明细结束---异常,code:" + data_code + ",消息:" + jo.Value<string>("msg")).LogInformation();
            return null;
        }

        if (result != null && result.Bzsjs != null)
        {
            List<Yt1Bzsj> bzs = result.Bzsjs.OrderBy(p => p.Bzcs).ToList();
            result.Bzsjs = bzs;
        }
        return result;
    }
    /// <summary>
    /// 运统一报单数据转换成报单数据
    /// </summary>
    /// <param name="yt1Bdsj"></param>
    /// <returns></returns>
    public BdsjYt1 CovertBdsj(Yt1Bdsj yt1Bdsj)
    {
        BdsjYt1 bdsj = null;
        try
        {
            if (!string.IsNullOrEmpty(yt1Bdsj.Sjdm1) && !string.IsNullOrEmpty(yt1Bdsj.Bdrq))
            {
                #region 能耗处理
                yt1Bdsj.Jsl1 = null;
                yt1Bdsj.Jrl1 = null;
                yt1Bdsj.Jcl1 = null;
                yt1Bdsj.Zsdjsl1 = null;
                yt1Bdsj.Zsdjcl1 = null;
                yt1Bdsj.Lcgdjsl1 = null;
                yt1Bdsj.Lcgdjcl1 = null;
                yt1Bdsj.Jsl2 = null;
                yt1Bdsj.Jrl2 = null;
                yt1Bdsj.Jcl2 = null;
                yt1Bdsj.Zsdjsl2 = null;
                yt1Bdsj.Zsdjcl2 = null;
                yt1Bdsj.Lcgdjsl2 = null;
                yt1Bdsj.Lcgdjcl2 = null;
                yt1Bdsj.Dwsj = null;
                yt1Bdsj.Dwyl = null;
                #endregion

                //Mapper.CreateMap<Yt1Bdsj, BdsjYt1>().ForAllMembers(opt => opt.Condition(srs => !(new string[] { "Id", "Bzsjs", "Clbdsjs", "Nhsjs" }).Contains(srs.MemberName)));
                TypeAdapterConfig<Yt1Bdsj, BdsjYt1>.NewConfig().Ignore(t => new {t.Id,t.Bzsjs,t.Clbdsjs,t.Nhsjs});
                bdsj = yt1Bdsj.Adapt<BdsjYt1>();
                bdsj.Yt1bdsjid = yt1Bdsj.Id;
                bdsj.Proceeding = 0;
                bdsj.Bzsjs = new List<BzsjYt1>();
                string qrcode = bdsj.Qrcode;
                string cljc_pre = null;

                string yt1id_pre = null;
                if (yt1Bdsj.Bzsjs != null && yt1Bdsj.Bzsjs.Count > 0)
                {
                    #region 运统一重联机车赋值
                    if (yt1Bdsj.Yt1Bcs != null && yt1Bdsj.Yt1Bcs.Count > 0)
                    {
                        foreach (Yt1Bc yt1bc in yt1Bdsj.Yt1Bcs)
                        {
                            bool clks = false;   //重联开始
                            bool cljs = false;   //重联结束
                            foreach (Yt1Bzsj bzsj in yt1Bdsj.Bzsjs)
                            {
                                if (yt1bc.Type == bzsj.Yt1id && yt1bc.Gzfs != "本务" && yt1bc.Gzfs != "无人重联")   //如果运统一id相同
                                {
                                    if (yt1bc.Zid == yt1Bdsj.Id &&
                                    !string.IsNullOrWhiteSpace(yt1bc.Jx) &&
                                    !string.IsNullOrWhiteSpace(yt1bc.Jh))   //如果是本务机车，则取重联机车信息
                                    {
                                        //如果是本务机务，本务机车报单“重补”去除“有动力附挂”的显示，2022年7月12日和丰台全科讨论后修改。
                                        if (yt1bc.Gzfs != "有动力附挂")
                                        {
                                            if (yt1bc.Arr_stncode == bzsj.Czh)    //如果当前站和重联请求开始站相同，则添加重联机车
                                            {
                                                if (bzsj.Cljc == null)
                                                    bzsj.Cljc = yt1bc.Jx + "-" + yt1bc.Jh;
                                                else if (!bzsj.Cljc.Contains(yt1bc.Jx + "-" + yt1bc.Jh))
                                                    bzsj.Cljc += "," + yt1bc.Jx + "-" + yt1bc.Jh;
                                                clks = true;
                                            }
                                            else if (yt1bc.Dep_stncode == bzsj.Czh)  //如果当前站和重联请求结束站相同，则不再添加重联机车
                                            {
                                                cljs = true;
                                            }
                                            else if (clks == true && cljs == false)  //中间站添加重联
                                            {
                                                if (bzsj.Cljc == null)
                                                    bzsj.Cljc = yt1bc.Jx + "-" + yt1bc.Jh;
                                                else if (!bzsj.Cljc.Contains(yt1bc.Jx + "-" + yt1bc.Jh))
                                                    bzsj.Cljc += "," + yt1bc.Jx + "-" + yt1bc.Jh;
                                            }
                                        }
                                    }
                                    else if (yt1bc.Glsjbdid == yt1Bdsj.Id && !string.IsNullOrWhiteSpace(yt1bc.Bwjc))   //如果是重联机车，则取本务机车信息
                                    {
                                        bdsj.Gzfs = yt1bc.Gzfs;

                                        if (yt1bc.Arr_stncode == bzsj.Czh && yt1bc.Bwjc != "-")    //如果当前站和重联请求开始站相同，则添加重联机车
                                        {
                                            if (bzsj.Cljc == null)
                                                bzsj.Cljc = yt1bc.Bwjc;
                                            else if (!bzsj.Cljc.Contains(yt1bc.Bwjc))
                                                bzsj.Cljc += "," + yt1bc.Bwjc;
                                            clks = true;
                                        }
                                        else if (yt1bc.Dep_stncode == bzsj.Czh)  //如果当前站和重联请求结束站相同，则不再添加重联机车
                                        {
                                            cljs = true;
                                        }
                                        else if (clks == true && cljs == false)  //中间站添加重联
                                        {
                                            if (bzsj.Cljc == null)
                                                bzsj.Cljc = yt1bc.Bwjc;
                                            else if (!bzsj.Cljc.Contains(yt1bc.Bwjc))
                                                bzsj.Cljc += "," + yt1bc.Bwjc;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region 运统一重联机车去重和摘补赋值
                    for (int i = 0; i < yt1Bdsj.Bzsjs.Count; i++)
                    {
                        if (string.IsNullOrWhiteSpace(cljc_pre)) //如果前一站没有重联，或者已经摘补
                        {
                            cljc_pre = yt1Bdsj.Bzsjs[i].Cljc;
                        }
                        else
                        {
                            //如果前一站有重联，而且当前重联和上一站相同，则当前站（中间站）重联设置为空
                            if (cljc_pre == yt1Bdsj.Bzsjs[i].Cljc)
                            {
                                yt1Bdsj.Bzsjs[i].Cljc = null;
                            }
                            //如果前一站有重联，当前站没有联重，而且当前站不是最后一站, 则加摘补标识
                            else if (string.IsNullOrWhiteSpace(yt1Bdsj.Bzsjs[i].Cljc) && i != yt1Bdsj.Bzsjs.Count - 1)
                            {
                                yt1Bdsj.Bzsjs[i].Cljc = "-";  //摘补
                                cljc_pre = null;  //摘补后 
                            }
                            else
                            {
                                cljc_pre = yt1Bdsj.Bzsjs[i].Cljc;
                            }
                        }

                        if (null == yt1id_pre) //如果前一站没有运统一乙
                        {
                            yt1id_pre = yt1Bdsj.Bzsjs[i].Yt1id;
                        }
                        else
                        {
                            if (yt1id_pre == yt1Bdsj.Bzsjs[i].Yt1id) //如果前一站有运统一乙，而且当前运统一乙和上一站相同，则当前站（中间站）运统一乙设置为空
                            {
                                yt1Bdsj.Bzsjs[i].Zbzsjs = null;
                            }
                            else if (string.IsNullOrWhiteSpace(yt1Bdsj.Bzsjs[i].Yt1id))  //如果yt1id为空
                            {
                                yt1id_pre = null;  //则设置为空 
                            }
                            else
                            {
                                yt1id_pre = yt1Bdsj.Bzsjs[i].Yt1id;
                            }
                        }
                    }
                    #endregion

                    #region 编组、子编组赋值
                    foreach (Yt1Bzsj bzsj in yt1Bdsj.Bzsjs)
                    {
                        TypeAdapterConfig<Yt1Bzsj, BzsjYt1>.NewConfig().Ignore(dest => dest.Id, dest => dest.Bdsjid, dest => dest.Zbzsjs);
                        //Mapper.CreateMap<Yt1Bzsj, BzsjYt1>().ForAllMembers(opt => opt.Condition(srs => !(new string[] { "Id", "Bdsjid", "Zbzsjs", "Yt1Bc" }).Contains(srs.MemberName)));
                        BzsjYt1 bz = bzsj.Adapt<BzsjYt1>();

                        #region 总重、载重转换为整数，重新赋值,其它指标如果为0，则赋null
                        //将运统一总重转换成整数，赋值给总重
                        if (!string.IsNullOrWhiteSpace(bz.Yt1Zz))
                        {
                            try
                            {
                                bz.Zz = Convert.ToInt32(Convert.ToDouble(bz.Yt1Zz));
                            }
                            catch (Exception ex)
                            { }
                        }
                        //将运统一换长转换成整数，赋值给换长
                        if (!string.IsNullOrWhiteSpace(bz.Yt1Hc))
                        {
                            try
                            {
                                bz.Hc = Convert.ToInt32(Convert.ToDouble(bz.Yt1Hc));
                            }
                            catch (Exception ex)
                            { }
                        }

                        if (bz.Qzjg == "0") bz.Qzjg = null;
                        if (bz.Ddqy == "0") bz.Ddqy = null;
                        #endregion

                        bz.Yt1bdsjid = bdsj.Yt1bdsjid;
                        if (!string.IsNullOrEmpty(bz.Dd) && bz.Dd.Equals(bz.Cf))
                        {
                            bz.Dd = null;
                        }

                        if (bzsj.Zbzsjs != null && bzsj.Zbzsjs.Count > 0)
                        {
                            bz.Zbzsjs = new List<ZbzsjYt1>();
                            foreach (Yt1Zbzsj zbzsj in bzsj.Zbzsjs)
                            {
                                TypeAdapterConfig<Yt1Zbzsj, ZbzsjYt1>.NewConfig().Ignore(t => t.Id, t => t.Bzsjid);
                                //Mapper.CreateMap<Yt1Zbzsj, ZbzsjYt1>().ForAllMembers(opt => opt.Condition(srs => !(new string[] { "Id", "Bzsjid" }).Contains(srs.MemberName)));
                                ZbzsjYt1 zbz = zbzsj.Adapt<ZbzsjYt1>();

                                #region 总重、载重转换为整数，重新赋值,其它指标如果为0，则赋null
                                //将运统一总重转换成整数，赋值给总重
                                if (!string.IsNullOrWhiteSpace(zbz.Yt1Zz))
                                {
                                    try
                                    {
                                        zbz.Zz = Convert.ToInt32(Convert.ToDouble(zbz.Yt1Zz));
                                    }
                                    catch (Exception ex)
                                    { }
                                }
                                //将运统一换长转换成整数，赋值给换长
                                if (!string.IsNullOrWhiteSpace(zbz.Yt1Hc))
                                {
                                    try
                                    {
                                        zbz.Hc = Convert.ToInt32(Convert.ToDouble(zbz.Yt1Hc));
                                    }
                                    catch (Exception ex)
                                    { }
                                }
                                if (zbz.Qzjg == "0") zbz.Qzjg = null;
                                if (zbz.Ddqy == "0") zbz.Ddqy = null;
                                #endregion

                                //2022年3月25日，丰台全科反应，车站没有编组信息，经查3月23号之后，所有运统一报单都没有返回编组
                                //dat_bzsj表中编组信息用dat_zbzsj表中的“合计”替换。
                                if (zbz.Yt1Bzlxmc == "合计")
                                {
                                    bz.Zz = zbz.Zz;
                                    bz.Zaiz = zbz.Zaiz;
                                    bz.Kc = zbz.Kc;
                                    bz.Ddqy = zbz.Ddqy;
                                    bz.Qzjg = zbz.Qzjg;
                                    bz.Zc = zbz.Zc;
                                    bz.Kongc = zbz.Kongc;
                                    bz.Fyyc = zbz.Fyyc;
                                    bz.Dkc = zbz.Dkc;
                                    bz.Qt = zbz.Qt;
                                    bz.Ls = zbz.Ls;
                                    bz.Hc = zbz.Hc;
                                }
                                bz.Zbzsjs.Add(zbz);
                            }
                        }
                        bdsj.Bzsjs.Add(bz);
                    }
                    #endregion

                    #region 始发终到车站车次赋值
                    if (bdsj.Bzsjs.Count > 0)
                    {
                        bdsj.Sfczh = bdsj.Bzsjs[0].Czh;
                        bdsj.Sfczmc = bdsj.Bzsjs[0].Czmc;
                        bdsj.Sfcc = bdsj.Bzsjs[0].Cc;

                        bdsj.Zdczh = bdsj.Bzsjs[bdsj.Bzsjs.Count - 1].Czh;
                        bdsj.Zdczmc = bdsj.Bzsjs[bdsj.Bzsjs.Count - 1].Czmc;
                        bdsj.Zdcc = bdsj.Bzsjs[bdsj.Bzsjs.Count - 1].Cc;
                    }
                    #endregion

                    #region 运统一去重(总重、载重、车次等）
                    for (int i = bdsj.Bzsjs.Count - 1; i > 0; i--)
                    {
                        //如果
                        if (string.IsNullOrEmpty(bdsj.Bzsjs[i].Yt1id) || bdsj.Bzsjs[i].Yt1id.Equals(bdsj.Bzsjs[i - 1].Yt1id))
                        {
                            bdsj.Bzsjs[i].Zz = null;
                            bdsj.Bzsjs[i].Yt1Zz = null;
                            bdsj.Bzsjs[i].Zaiz = null;
                            bdsj.Bzsjs[i].Kc = null;
                            bdsj.Bzsjs[i].Ddqy = null;
                            bdsj.Bzsjs[i].Qzjg = null;
                            bdsj.Bzsjs[i].Zc = null;
                            bdsj.Bzsjs[i].Kongc = null;
                            bdsj.Bzsjs[i].Fyyc = null;
                            bdsj.Bzsjs[i].Dkc = null;
                            bdsj.Bzsjs[i].Qt = null;
                            bdsj.Bzsjs[i].Ls = null;
                            bdsj.Bzsjs[i].Hc = null;
                            bdsj.Bzsjs[i].Yt1Hc = null;
                            bdsj.Bzsjs[i].Yt1id = null;
                        }
                        if (!string.IsNullOrEmpty(bdsj.Bzsjs[i].Cc) && bdsj.Bzsjs[i].Cc.Equals(bdsj.Bzsjs[i - 1].Cc))
                        {
                            bdsj.Bzsjs[i].Cc = null;
                        }
                    }
                    #endregion

                }
                //运统一机车能耗信息
                if (yt1Bdsj.Nhsjs != null && yt1Bdsj.Nhsjs.Count > 0)
                {
                    bdsj.Nhsjs = new List<NhsjYt1>();
                    foreach (Yt1Nhsj yt1Nhsj in yt1Bdsj.Nhsjs)
                    {
                        #region 数据处理
                        if (null != yt1Nhsj.Jsl1)
                        {
                            if (yt1Nhsj.Jsl1 > 2147483647)
                                yt1Nhsj.Jsl1 = -1;
                            else if (yt1Nhsj.Jsl1 == 0)
                                yt1Nhsj.Jsl1 = null;
                        }

                        if (null != yt1Nhsj.Jrl1)
                        {
                            if (yt1Nhsj.Jrl1 > 2147483647)
                                yt1Nhsj.Jrl1 = -1;
                            else if (yt1Nhsj.Jrl1 == 0)
                                yt1Nhsj.Jrl1 = null;
                        }
                        if (null != yt1Nhsj.Jcl1)
                        {
                            if (yt1Nhsj.Jcl1 > 2147483647)
                                yt1Nhsj.Jcl1 = -1;
                            else if (yt1Nhsj.Jcl1 == 0)
                                yt1Nhsj.Jcl1 = null;
                        }
                        if (null != yt1Nhsj.Zsdjsl1)
                        {
                            if (yt1Nhsj.Zsdjsl1 > 2147483647)
                                yt1Nhsj.Zsdjsl1 = -1;
                            else if (yt1Nhsj.Zsdjsl1 == 0)
                                yt1Nhsj.Zsdjsl1 = null;
                        }
                        if (null != yt1Nhsj.Zsdjcl1)
                        {
                            if (yt1Nhsj.Zsdjcl1 > 2147483647)
                                yt1Nhsj.Zsdjcl1 = -1;
                            else if (yt1Nhsj.Zsdjcl1 == 0)
                                yt1Nhsj.Zsdjcl1 = null;
                        }
                        if (null != yt1Nhsj.Lcgdjsl1)
                        {
                            if (yt1Nhsj.Lcgdjsl1 > 2147483647)
                                yt1Nhsj.Lcgdjsl1 = -1;
                            else if (yt1Nhsj.Lcgdjsl1 == 0)
                                yt1Nhsj.Lcgdjsl1 = null;
                        }
                        if (null != yt1Nhsj.Lcgdjcl1)
                        {
                            if (yt1Nhsj.Lcgdjcl1 > 2147483647)
                                yt1Nhsj.Lcgdjcl1 = -1;
                            else if (yt1Nhsj.Lcgdjcl1 == 0)
                                yt1Nhsj.Lcgdjcl1 = null;
                        }
                        if (null != yt1Nhsj.Jsl2)
                        {
                            if (yt1Nhsj.Jsl2 > 2147483647)
                                yt1Nhsj.Jsl2 = -1;
                            else if (yt1Nhsj.Jsl2 == 0)
                                yt1Nhsj.Jsl2 = null;
                        }
                        if (null != yt1Nhsj.Jrl2)
                        {
                            if (yt1Nhsj.Jrl2 > 2147483647)
                                yt1Nhsj.Jrl2 = -1;
                            else if (yt1Nhsj.Jrl2 == 0)
                                yt1Nhsj.Jrl2 = null;
                        }
                        if (null != yt1Nhsj.Jcl2)
                        {
                            if (yt1Nhsj.Jcl2 > 2147483647)
                                yt1Nhsj.Jcl2 = -1;
                            else if (yt1Nhsj.Jcl2 == 0)
                                yt1Nhsj.Jcl2 = null;
                        }
                        if (null != yt1Nhsj.Zsdjsl2)
                        {
                            if (yt1Nhsj.Zsdjsl2 > 2147483647)
                                yt1Nhsj.Zsdjsl2 = -1;
                            else if (yt1Nhsj.Zsdjsl2 == 0)
                                yt1Nhsj.Zsdjsl2 = null;
                        }
                        if (null != yt1Nhsj.Zsdjcl2)
                        {
                            if (yt1Nhsj.Zsdjcl2 > 2147483647)
                                yt1Nhsj.Zsdjcl2 = -1;
                            else if (yt1Nhsj.Zsdjcl2 == 0)
                                yt1Nhsj.Zsdjcl2 = null;
                        }
                        if (null != yt1Nhsj.Lcgdjsl2)
                        {
                            if (yt1Nhsj.Lcgdjsl2 > 2147483647)
                                yt1Nhsj.Lcgdjsl2 = -1;
                            else if (yt1Nhsj.Lcgdjsl2 == 0)
                                yt1Nhsj.Lcgdjsl2 = null;
                        }
                        if (null != yt1Nhsj.Lcgdjcl2)
                        {
                            if (yt1Nhsj.Lcgdjcl2 > 2147483647)
                                yt1Nhsj.Lcgdjcl2 = -1;
                            else if (yt1Nhsj.Lcgdjcl2 == 0)
                                yt1Nhsj.Lcgdjcl2 = null;
                        }
                        if (null != yt1Nhsj.Dwsj && yt1Nhsj.Dwsj == "0")
                            yt1Nhsj.Dwsj = null;
                        if (null != yt1Nhsj.Dwyl && (yt1Nhsj.Dwyl == 0 || yt1Nhsj.Dwyl > 2147483647))
                        {
                            yt1Nhsj.Dwsj = null;
                            yt1Nhsj.Dwyl = null;
                        }
                        #endregion
                        TypeAdapterConfig<Yt1Nhsj, NhsjYt1>.NewConfig().Ignore(t => t.Id);
                        //_mapper.CreateMap<Yt1Nhsj, NhsjYt1>().ForAllMembers(opt => opt.Condition(srs => !(new string[] { "Id" }).Contains(srs.MemberName)));
                        NhsjYt1 nhsj = yt1Nhsj.Adapt<NhsjYt1>();
                        bdsj.Nhsjs.Add(nhsj);
                    }

                    NhsjYt1 nhsjYt1 = bdsj.Nhsjs.FirstOrDefault(p => p.Jx.Equals(bdsj.Jx) && p.Jch.Equals(bdsj.Jch));
                    if (null != nhsjYt1)
                    {
                        #region 能耗处理
                        bdsj.Jsl1 = nhsjYt1.Jsl1;
                        bdsj.Jrl1 = nhsjYt1.Jrl1;
                        bdsj.Jcl1 = nhsjYt1.Jcl1;
                        bdsj.Zsdjsl1 = nhsjYt1.Zsdjsl1;
                        bdsj.Zsdjcl1 = nhsjYt1.Zsdjcl1;
                        bdsj.Lcgdjsl1 = nhsjYt1.Lcgdjsl1;
                        bdsj.Lcgdjcl1 = nhsjYt1.Lcgdjcl1;
                        bdsj.Jsl2 = nhsjYt1.Jsl2;
                        bdsj.Jrl2 = nhsjYt1.Jrl2;
                        bdsj.Jcl2 = nhsjYt1.Jcl2;
                        bdsj.Zsdjsl2 = nhsjYt1.Zsdjsl2;
                        bdsj.Zsdjcl2 = nhsjYt1.Zsdjcl2;
                        bdsj.Lcgdjsl2 = nhsjYt1.Lcgdjsl2;
                        bdsj.Lcgdjcl2 = nhsjYt1.Lcgdjcl2;
                        bdsj.Dwsj = nhsjYt1.Dwsj;
                        bdsj.Dwyl = nhsjYt1.Dwyl;
                        #endregion
                    }
                }
            }
        }
        catch (Exception ex)
        { }
        return bdsj;
    }
    public void GetBdsjById(Yt1Bdsj yt1Bdt, string dzbdddm, int bid, out string data_code, out string login_code)
    {
        //获取数据
        Yt1Bdsj yt1Bdsj = null;
        try
        {
            #region 测试用例
            /*data_code = "";  login_code = "";
            yt1Bdsj =  getYt1data(); */
            //yt1Bdt.Id = "DA79161D127FE1A0E053353E030AC5D0";
            #endregion
            yt1Bdsj = GetYt1BdsjById(yt1Bdt, out data_code, out login_code);
            #region 如果没取到，再取三次
            int a = 0;
            while (yt1Bdsj == null && a < 3)
            {
                System.Threading.Thread.Sleep(5 * 1000);
                yt1Bdsj = GetYt1BdsjById(yt1Bdt, out data_code, out login_code);
                a++;
            }
            #endregion
            if (yt1Bdsj != null)
            {
                #region 超过19位时间(毫秒)的处理
                if (!string.IsNullOrEmpty(yt1Bdsj.Eventtime) && yt1Bdsj.Eventtime.Length > 19)
                {
                    yt1Bdsj.Eventtime = yt1Bdsj.Eventtime.Substring(0, 19);

                    if (yt1Bdsj.Yt1Bcs != null)
                    {
                        foreach (var item in yt1Bdsj.Yt1Bcs)
                        {
                            if (!string.IsNullOrEmpty(item.Eventtime) && item.Eventtime.Length > 19)
                            {
                                item.Eventtime = item.Eventtime.Substring(0, 19);
                            }

                            if (!string.IsNullOrEmpty(item.Ctime) && item.Ctime.Length > 19)
                            {
                                item.Ctime = item.Ctime.Substring(0, 19);
                            }
                        }
                    }
                }
                #endregion
                BdsjYt1 bdsj = CovertBdsj(yt1Bdsj);
                bdsj.Zpddm = dzbdddm;
                if (bid == 0) //添加
                {
                    bdsj.Zcfsdm = GetZcfsdm(bdsj.Zcfsmc);  //得到执乘方式代码
                    Cwyxx cwy = _db.Queryable<Cwyxx>().First(t => t.Cwydm == bdsj.Sjdm1);
                    if (cwy != null)
                    {
                        bdsj.Cjdm = cwy.Cjdm;  //取得乘务员对应的车间代码
                    }
                }
                if (!string.IsNullOrEmpty(bdsj.Jch) && bdsj.Jch.Length > 4)
                {
                    //有个别乘务员将机车号输入4位以上，程序截取最后4位
                    bdsj.Jch = bdsj.Jch.Substring(bdsj.Jch.Length - 4, 4);
                }
                if (string.IsNullOrEmpty(bdsj.Tq))
                {
                    bdsj.Tq = yt1Bdt.Tq;
                    bdsj.Zdczh = yt1Bdt.Zdczh;
                    bdsj.Zdczmc = yt1Bdt.Zdczmc;
                    bdsj.Zdcc = yt1Bdt.Zdcc;
                    bdsj.Eventtime = yt1Bdt.Eventtime;
                }
                InsertBdsjYt1(bdsj, bid);
            }
        }
        catch (Exception e)
        {
            throw (new Exception(e.Message));
        }
    }
    /// <summary>
    /// 根据运统1报单批量插入
    /// </summary>
    /// <param name="bdsj"></param>
    public dynamic InsertBdsjYt1(BdsjYt1 bdsj, int bid)
    {

        try
        {
            _db.Ado.BeginTran();
            if (bid == 0) //添加
            {
                //删除报单头部信息和报单编组信息
                var bdsj1 = _db.Ado.ExecuteCommand($" DELETE FROM YT1_DAT_BDSJ WHERE YT1BDSJID='{bdsj.Yt1bdsjid}'");

                //新增报单数据
                bid = _db.Insertable<BdsjYt1>(bdsj).ExecuteCommand();
            }
            else
            {
                string sql = string.Format(@"update yt1_dat_bdsj set Jch={1},Jx={2},Sjdm1={3},Sjxm1={4},Fsjdm1={5},Fsjxm1={6},Xydm1={7},Xyxm1={8},Sjdm2={9},Sjxm2={10},
                                                     Fsjdm2={11},Fsjxm2={12},Xydm2={13},Xyxm2={14},Jczl={15},Bdrq={16},Cq={17},Tq={18},Eventtime={19},Zcfsdm={20},Cwtqdddm={21},
                                                     Jsl1={22}, Jrl1={23}, Jcl1={24}, Zsdjsl1={25}, Zsdjcl1={26}, Lcgdjsl1={27}, Lcgdjcl1={28}, 
                                                     Jsl2={29}, Jrl2={30}, Jcl2={31}, Zsdjsl2={32}, Zsdjcl2={33}, Lcgdjsl2={34}, Lcgdjcl2={35}, 
                                                     Sfczh ={36}, Sfczmc={37},Sfcc={38}, Zdczh={39}, Zdczmc={40}, Zdcc={41},Zcfsmc={42} where yt1bdsjid='{0}'", bdsj.Yt1bdsjid,
                                    bdsj.Jch == null ? "null" : "'" + bdsj.Jch + "'", bdsj.Jx == null ? "null" : "'" + bdsj.Jx + "'",
                                    bdsj.Sjdm1 == null ? "null" : "'" + bdsj.Sjdm1 + "'", bdsj.Sjxm1 == null ? "null" : "'" + bdsj.Sjxm1 + "'",
                                    bdsj.Fsjdm1 == null ? "null" : "'" + bdsj.Fsjdm1 + "'", bdsj.Fsjxm1 == null ? "null" : "'" + bdsj.Fsjxm1 + "'",
                                    bdsj.Xydm1 == null ? "null" : "'" + bdsj.Xydm1 + "'", bdsj.Xyxm1 == null ? "null" : "'" + bdsj.Xyxm1 + "'",
                                    bdsj.Sjdm2 == null ? "null" : "'" + bdsj.Sjdm2 + "'", bdsj.Sjxm2 == null ? "null" : "'" + bdsj.Sjxm2 + "'",
                                    bdsj.Fsjdm2 == null ? "null" : "'" + bdsj.Fsjdm2 + "'", bdsj.Fsjxm2 == null ? "null" : "'" + bdsj.Fsjxm2 + "'",
                                    bdsj.Xydm2 == null ? "null" : "'" + bdsj.Xydm2 + "'", bdsj.Xyxm2 == null ? "null" : "'" + bdsj.Xyxm2 + "'",
                                    bdsj.Jczl == null ? "null" : "'" + bdsj.Jczl + "'", bdsj.Bdrq == null ? "null" : "'" + bdsj.Bdrq + "'",
                                    bdsj.Cq == null ? "null" : "'" + bdsj.Cq + "'", bdsj.Tq == null ? "null" : "'" + bdsj.Tq + "'",
                                    bdsj.Eventtime == null ? "null" : "'" + bdsj.Eventtime + "'", "'" + GetZcfsdm(bdsj.Zcfsmc) + "'", bdsj.Cwtqdddm == null ? "null" : "'" + bdsj.Cwtqdddm + "'",
                                    bdsj.Jsl1 == null ? "null" : bdsj.Jsl1.ToString(), bdsj.Jrl1 == null ? "null" : bdsj.Jrl1.ToString(), bdsj.Jcl1 == null ? "null" : bdsj.Jcl1.ToString(),
                                    bdsj.Zsdjsl1 == null ? "null" : bdsj.Zsdjsl1.ToString(), bdsj.Zsdjcl1 == null ? "null" : bdsj.Zsdjcl1.ToString(),
                                    bdsj.Lcgdjsl1 == null ? "null" : bdsj.Lcgdjsl1.ToString(), bdsj.Lcgdjcl1 == null ? "null" : bdsj.Lcgdjcl1.ToString(),
                                    bdsj.Jsl2 == null ? "null" : bdsj.Jsl2.ToString(), bdsj.Jrl2 == null ? "null" : bdsj.Jrl2.ToString(), bdsj.Jcl2 == null ? "null" : bdsj.Jcl2.ToString(),
                                    bdsj.Zsdjsl2 == null ? "null" : bdsj.Zsdjsl2.ToString(), bdsj.Zsdjcl2 == null ? "null" : bdsj.Zsdjcl2.ToString(),
                                    bdsj.Lcgdjsl2 == null ? "null" : bdsj.Lcgdjsl2.ToString(), bdsj.Lcgdjcl2 == null ? "null" : bdsj.Lcgdjcl2.ToString(),
                                    bdsj.Sfczh == null ? "null" : "'" + bdsj.Sfczh + "'", bdsj.Sfczmc == null ? "null" : "'" + bdsj.Sfczmc + "'", bdsj.Sfcc == null ? "null" : "'" + bdsj.Sfcc + "'",
                                    bdsj.Zdczh == null ? "null" : "'" + bdsj.Zdczh + "'", bdsj.Zdczmc == null ? "null" : "'" + bdsj.Zdczmc + "'", bdsj.Zdcc == null ? "null" : "'" + bdsj.Zdcc + "'", bdsj.Zcfsmc == null ? "null" : "'" + bdsj.Zcfsmc + "'");
                var bdsj1 = _db.Ado.ExecuteCommand(sql);
            }

            //新增联控机车数据
            if (bdsj.Clbdsjs != null)
            {
                foreach (var item in bdsj.Clbdsjs)
                {
                    item.Bdsjid = bid;
                }
                _db.Insertable<ClbdsjYt1>(bdsj.Clbdsjs).ExecuteCommand();
            }

            //新增联控机车数据
            if (bdsj.Yt1Bcs != null && bdsj.Yt1Bcs.Count > 0)
            {
                foreach (var item in bdsj.Yt1Bcs)
                {
                    item.Bdsjid = bid;
                }
                _db.Insertable<Yt1Bc>(bdsj.Yt1Bcs).ExecuteCommand();
            }


            //新增机车能耗数据
            if (bdsj.Nhsjs != null && bdsj.Nhsjs.Count > 0)
            {
                foreach (var item in bdsj.Nhsjs)
                {
                    item.Bdsjid = bid;
                }
                _db.Insertable<NhsjYt1>(bdsj.Nhsjs).ExecuteCommand();
            }

            //新增编组数据
            if (bdsj.Bzsjs != null)
            {
                foreach (var item in bdsj.Bzsjs)
                {
                    item.Bdsjid = bid;
                    var bzsjid = _db.Insertable<BzsjYt1>(item).ExecuteReturnIdentity();
                    if (item.Zbzsjs != null)
                    {
                        List<ZbzsjYt1> zbzsjs = new List<ZbzsjYt1>();
                        //新增编组子数据
                        foreach (var item1 in item.Zbzsjs)
                        {
                            //如果是集特行，而且总重大于零则添加
                            if ((item1.Yt1Bzlx == 1 || item1.Yt1Bzlx == 2 || item1.Yt1Bzlx == 3) && item1.Zz > 0)
                            {
                                item1.Bzsjid = bzsjid;
                                item1.Bzlx = item1.Yt1Bzlx;
                                zbzsjs.Add(item1);
                            }
                        }
                        if (null != zbzsjs && zbzsjs.Count > 0)
                        {
                            zbzsjs.OrderBy(p => p.Bzlx).ToList();
                            _db.Insertable<ZbzsjYt1>(zbzsjs).ExecuteCommand();
                        }
                    }
                }
            }
            _db.Ado.CommitTran();
            ("---插入yt1成功---bdid:" + bdsj.Yt1bdsjid).LogInformation();
            return new { success = true, message = "新增成功！" };
        }
        catch (Exception e)
        {
            _db.Ado.RollbackTran();
            ("---插入yt1失败---bdid:" + bdsj.Yt1bdsjid + "error:" + ExceptionUtil.GetExceptionMsg(e)).LogInformation();
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    public dynamic GetYt1Bdsj(Yt1Bdsj yt1Bdsj, string ddm)
    {
        try
        {
            string data_code = "";   //获取数据时，返回的code值，如果code等于403，则不再访问运统一
            string login_code = "";  //获取token时，返回的code值，如果code等于403，则不再访问运统一
            var list = ddm.Split('-');
            string yt1ddm = list[0];   //运统一存储的段代码
            string dzbdddm = yt1ddm;   //电子报单段代码
            if (list.Count() > 1)
                dzbdddm = list[1];
            //1.对于司机登录又直接退了，没有作业的报单，直接过滤，不再处理。
            if (string.IsNullOrWhiteSpace(yt1Bdsj.Sjdm1) || string.IsNullOrWhiteSpace(yt1Bdsj.Cq) || string.IsNullOrWhiteSpace(yt1Bdsj.Bdrq))
            {
                _db.Ado.ExecuteCommand($" update DIC_BDTQPZ set BDRQ='{yt1Bdsj.Eventtime}' where ddm='" + yt1ddm + "' ");
                return new { success = true, data_code = "", login_code = "" };
            }

            BdsjYt1 bdsjYt1 = _db.Queryable<BdsjYt1>().First(p => p.Yt1bdsjid == yt1Bdsj.Id);

            if (bdsjYt1 == null)
            {
                if (string.IsNullOrWhiteSpace(yt1Bdsj.Tq))
                {
                    //2.1 如果数据库没有记录，而且运统1没有退勤时间，则直接入库
                    if (!string.IsNullOrEmpty(yt1Bdsj.Jch) && yt1Bdsj.Jch.Length > 4)
                    {
                        //有个别乘务员将机车号输入4位以上，程序截取最后4位
                        yt1Bdsj.Jch = yt1Bdsj.Jch.Substring(yt1Bdsj.Jch.Length - 4, 4);
                    }
                    yt1Bdsj.Zpddm = dzbdddm;
                    _db.Ado.BeginTran();
                    InsertYt1Cqxx(yt1Bdsj);
                    _db.Ado.ExecuteCommand($" update DIC_BDTQPZ set BDRQ='{yt1Bdsj.Eventtime}' where ddm='" + yt1ddm + "' ");
                    _db.Ado.CommitTran();
                    return new { success = true, data_code = "", login_code = "" };
                }
                else
                {
                    //2.2 如果数据库没有记录，而且运统1有退勤时间，则获取详细报单数据，再入库。
                    GetBdsjById(yt1Bdsj, dzbdddm, 0, out data_code, out login_code);
                    _db.Ado.ExecuteCommand($" update DIC_BDTQPZ set BDRQ='{yt1Bdsj.Eventtime}' where ddm='" + yt1ddm + "' ");
                    _db.Ado.CommitTran();
                    return new { success = true, data_code = data_code, login_code = login_code };
                }
            }
            //3.数据库有记录
            else
            {
                //3.1 如果更新时间相同，则不做处理
                if (bdsjYt1.Eventtime == yt1Bdsj.Eventtime)
                {
                    return new { success = true, data_code = "", login_code = "" };
                }
                else
                {

                    //3.2 数据库退勤时间为空，运统1新数据退勤时间不为空，则更新详细记录。
                    if (string.IsNullOrEmpty(bdsjYt1.Tq) && !string.IsNullOrWhiteSpace(yt1Bdsj.Tq))
                    {
                        GetBdsjById(yt1Bdsj, dzbdddm, bdsjYt1.Id, out data_code, out login_code);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(yt1Bdsj.Jch) && yt1Bdsj.Jch.Length > 4)
                        {
                            //有个别乘务员将机车号输入4位以上，程序截取最后4位
                            yt1Bdsj.Jch = yt1Bdsj.Jch.Substring(yt1Bdsj.Jch.Length - 4, 4);
                        }
                        string sql = string.Format(@"update yt1_dat_bdsj set Jch={1},Jx={2},Sjdm1={3},Sjxm1={4},Fsjdm1={5},Fsjxm1={6},Xydm1={7},Xyxm1={8},Sjdm2={9},Sjxm2={10},
                                                     Fsjdm2={11},Fsjxm2={12},Xydm2={13},Xyxm2={14},Jczl={15},Bdrq={16},Cq={17},Tq={18},Eventtime={19},Zcfsdm={20},Cwtqdddm={21},
                                                     Sfczh ={22}, Sfczmc={23},Sfcc={24}, Zdczh={25}, Zdczmc={26}, Zdcc={27}, Zcfsmc={28} where yt1bdsjid='{0}'", bdsjYt1.Yt1bdsjid,
                                            yt1Bdsj.Jch == null ? "null" : "'" + yt1Bdsj.Jch + "'", yt1Bdsj.Jx == null ? "null" : "'" + yt1Bdsj.Jx + "'",
                                            yt1Bdsj.Sjdm1 == null ? "null" : "'" + yt1Bdsj.Sjdm1 + "'", yt1Bdsj.Sjxm1 == null ? "null" : "'" + yt1Bdsj.Sjxm1 + "'",
                                            yt1Bdsj.Fsjdm1 == null ? "null" : "'" + yt1Bdsj.Fsjdm1 + "'", yt1Bdsj.Fsjxm1 == null ? "null" : "'" + yt1Bdsj.Fsjxm1 + "'",
                                            yt1Bdsj.Xydm1 == null ? "null" : "'" + yt1Bdsj.Xydm1 + "'", yt1Bdsj.Xyxm1 == null ? "null" : "'" + yt1Bdsj.Xyxm1 + "'",
                                            yt1Bdsj.Sjdm2 == null ? "null" : "'" + yt1Bdsj.Sjdm2 + "'", yt1Bdsj.Sjxm2 == null ? "null" : "'" + yt1Bdsj.Sjxm2 + "'",
                                            yt1Bdsj.Fsjdm2 == null ? "null" : "'" + yt1Bdsj.Fsjdm2 + "'", yt1Bdsj.Fsjxm2 == null ? "null" : "'" + yt1Bdsj.Fsjxm2 + "'",
                                            yt1Bdsj.Xydm2 == null ? "null" : "'" + yt1Bdsj.Xydm2 + "'", yt1Bdsj.Xyxm2 == null ? "null" : "'" + yt1Bdsj.Xyxm2 + "'",
                                            yt1Bdsj.Jczl == null ? "null" : "'" + yt1Bdsj.Jczl + "'", yt1Bdsj.Bdrq == null ? "null" : "'" + yt1Bdsj.Bdrq + "'",
                                            yt1Bdsj.Cq == null ? "null" : "'" + yt1Bdsj.Cq + "'", yt1Bdsj.Tq == null ? "null" : "'" + yt1Bdsj.Tq + "'",
                                            yt1Bdsj.Eventtime == null ? "null" : "'" + yt1Bdsj.Eventtime + "'", "'" + GetZcfsdm(yt1Bdsj.Zcfsmc) + "'", yt1Bdsj.Cwtqdddm == null ? "null" : "'" + yt1Bdsj.Cwtqdddm + "'",
                                            yt1Bdsj.Sfczh == null ? "null" : "'" + yt1Bdsj.Sfczh + "'", yt1Bdsj.Sfczmc == null ? "null" : "'" + yt1Bdsj.Sfczmc + "'", yt1Bdsj.Sfcc == null ? "null" : "'" + yt1Bdsj.Sfcc + "'",
                                            yt1Bdsj.Zdczh == null ? "null" : "'" + yt1Bdsj.Zdczh + "'", yt1Bdsj.Zdczmc == null ? "null" : "'" + yt1Bdsj.Zdczmc + "'", yt1Bdsj.Zdcc == null ? "null" : "'" + yt1Bdsj.Zdcc + "'", yt1Bdsj.Zcfsmc == null ? "null" : "'" + yt1Bdsj.Zcfsmc + "'");
                        var bdsj1 = _db.Ado.ExecuteCommand(sql);
                    }

                    _db.Ado.ExecuteCommand($" update DIC_BDTQPZ set BDRQ='{yt1Bdsj.Eventtime}' where ddm='" + yt1ddm + "' ");
                    _db.Ado.CommitTran();
                }
                return new { success = true, data_code = "", login_code = "" };
            }
        }
        catch (Exception ex)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(ex) };
        }
    }
    /// <summary>
    /// 得到运统一详细数据
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpPost("api/Yt1/GetYt1BdsjNew")]
    [NonUnify]
    public dynamic GetYt1BdsjNew(RequestForGetYt1Bdsj r)
    {
        try
        {
            var result = GetYt1Bdsj(r.Bdsj, r.Ddm);
            /*
            if (r.IsEnd)
            {
                // 通知所有客户端前台刷新页面
                BdshHub bdshHub = new BdshHub();
                bdshHub.Send();
            }*/

            return result;
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    #region 得到运统一报单头列表
    /// <summary>
    /// 取得运统一接口数据，通过支配段和入库时间段得到运统一报单数据
    /// </summary>
    /// <param name="start">开始时间</param>
    /// <param name="end">结束时间</param>
    /// <param name="ddm">段代码</param>
    /// <returns></returns>
    public List<Yt1Bdsj> GetYt1BdsjByZpdRksj(string start, string end, string ddm, out string data_code, out string login_code)
    {
        List<Yt1Bdsj> result = null;
        data_code = "";   //获取数据时，返回的code值，如果code等于403，则不再访问运统一
        login_code = "";  //获取token时，返回的code值，如果code等于403，则不再访问运统一
        try
        {
            ("---列表开始---段代码" + ddm + ",开始时间" + start + ",结束时间" + end).LogInformation();
            if (null == xtcss || xtcss.Count() <= 0)
            {
                xtcss = _db.Queryable<Xtcs>().ToList();
            }
            string url = xtcss.Where(p => p.Csdm.Equals("yt1url")).FirstOrDefault().Cs;
            string username = xtcss.Where(p => p.Csdm.Equals("cwqsusername")).FirstOrDefault().Cs;
            string password = xtcss.Where(p => p.Csdm.Equals("cwqspassword")).FirstOrDefault().Cs;
            Xtcs xtcs = xtcss.Where(p => p.Csdm.Equals("cwqstoken")).FirstOrDefault();
            string token = xtcs.Cs;

            object postBody = "{\n    \"start\": \"" + start + "\",\n    \"end\": \"" + end + "\",\n    \"zpd\": \"" + ddm + "\",\n    \"pageNum\": 1\n}";
            string msg = "";
            IRestConnection restConnection = new RestConnection(url);
        bdsjzpdrksj:
            JObject jo = restConnection.Post("/cwqsws/bdsjzpdrksj/get", postBody, token);
            data_code = jo.Value<string>("code");
            if (data_code == "401")
            {
                ("---列表---获取token开始").LogInformation();
                bool loginStatus = restConnection.Login(@"/cwqsws/auth/login", "username", username, "password", password, out msg, out login_code, out token);
                ("---列表---获取token结束,logincode:" + login_code + ",消息:" + msg + ",token:" + token).LogInformation();
                if (loginStatus == false)
                {
                    ("---列表结束---登录异常").LogInformation();
                    return null;
                }
                try
                {
                    xtcs.Cs = token;
                    _db.Updateable(xtcs).ExecuteCommand();
                    goto bdsjzpdrksj;  //登录成功后，跳转到标签处，重新获取数据
                }
                catch (Exception ex)
                {
                    ("---列表结束---保存token异常:" + ex.Message).LogInformation();
                    return null;
                }
            }
            else if (data_code == "200")  //访问成功
            {
                try
                {
                    string data = StringSecurity.UnZipByteToJson(jo.Value<string>("data"));
                    JObject jData = JObject.Parse(data);

                    string bdsj = jData["content"].ToString();
                    ("列表json开始" + bdsj + "列表json开始结束").LogInformation();

                    result = JsonConvert.DeserializeObject<List<Yt1Bdsj>>(bdsj);

                    foreach (var item in result)
                    {
                        if (!string.IsNullOrEmpty(item.Eventtime) && item.Eventtime.Length > 19)
                        {
                            item.Eventtime = item.Eventtime.Substring(0, 19);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ("---列表结束---列表数据转换异常" + ex.Message + "json:" + jo.Value<string>("data")).LogInformation();
                    return null;
                }
            }
            else
            {
                ("---列表结束---异常,code:" + data_code + ",消息:" + jo.Value<string>("msg")).LogInformation();
                return null;
            }
        }
        catch (Exception e)
        {
            ("---列表结束---其它异常:" + e.Message).LogInformation();
            return null;
        }
        finally
        {
            ("----列表结束----").LogInformation();
        }
        return result;
    }
    public dynamic GetYt1BdsjData(string ddm)
    {
        try
        {
            //List<Yt1Bdsj> yt1bds = getYt1dataList();
            //return new { success = true, data = yt1bds, isjs = true, startdate = "2021-11-26 08:50:00", enddate = "2021-11-27 08:50:00", data_code = "", login_code = "" };

            ddm = ddm.Split('-')[0];
            string startDate, endDate;
            int jgsj = 24;   //默认执获取24小时的数据
            Bdtqpz bdtqpz = _db.Queryable<Bdtqpz>().First(p => p.Ddm == ddm);
            if (null == bdtqpz)
            {
                //如果没有配置，则默认从24个小时前的数据开始取数据
                startDate = DateTime.Now.AddHours(-24).ToString("yyyy-MM-dd HH:mm:ss");
                Bdtqpz result = new Bdtqpz();
                result.Bdrq = startDate;
                result.Zxjg = "*/ 5 * * * * ?";
                result.Ddm = ddm;
                _db.Insertable(result).ExecuteCommand();
            }
            else
            {
                startDate = bdtqpz.Bdrq;
            }
            endDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (DateTimeUtil.GetHoursSpan(startDate, endDate, "yyyy-MM-dd HH:mm:ss") > jgsj)  //如果时间差大于24个小时，则取24个小时数据
            {
                endDate = DateTime.Parse(startDate).AddHours(jgsj).ToString("yyyy-MM-dd HH:mm:ss");
            }
            string data_code = "";   //获取数据时，返回的code值，如果code等于403，则不再访问运统一
            string login_code = "";  //获取token时，返回的code值，如果code等于403，则不再访问运统一
            List<Yt1Bdsj> yt1Bdsjs = GetYt1BdsjByZpdRksj(startDate, endDate, ddm, out data_code, out login_code);

            if (null != yt1Bdsjs && yt1Bdsjs.Count > 0)
            {
                yt1Bdsjs = yt1Bdsjs.OrderBy(p => p.Eventtime).ToList<Yt1Bdsj>();
                return new { success = true, data = yt1Bdsjs, isjs = true, startdate = startDate, enddate = endDate, data_code = data_code, login_code = login_code };
            }
            else
            {
                return new { success = false, message = "当前时间段没有数据返回,等待下一次处理", isjs = true, startdate = startDate, enddate = endDate, data_code = data_code, login_code = login_code };
            }
        }
        catch (Exception ex)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(ex) + ",等待下一次处理", isjs = true };
        }
    }
    /// <summary>
    /// 得到运统一报单头列表
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpPost("api/Yt1/GetYt1BdsjList")]
    [NonUnify]
    public dynamic GetYt1BdsjList(RequestForGetYt1BdsjList r)
    {
        try
        {
            return GetYt1BdsjData(r.Ddm);
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    #endregion
    [NonUnify]
    [NonAction]
    public dynamic InsertLkj(LkjBdsj param)
    {
        try
        {
            string ddm = param.Ddm;
            LkjBdsjBdt bdt = new LkjBdsjBdt();
            TypeAdapterConfig<LkjBdsj, LkjBdsjBdt>.NewConfig().Ignore(t => t.Id);
            //Mapper.CreateMap<LkjBdsj, LkjBdsjBdt>().ForAllMembers(opt => opt.Condition(srs => srs.MemberName != "Id"));
            //Mapper.Map<LkjBdsj, LkjBdsjBdt>(param, bdt);
            bdt = param.Adapt<LkjBdsjBdt>();
            bdt.Zpddm = ddm;
            bdt.Bdrksj = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //翻译乘务员姓名
            if (!string.IsNullOrEmpty(bdt.Sjh))
            {
                bdt.Sjxm = FindCwyxmByDm2(bdt.Sjh);
            }
            if (!string.IsNullOrEmpty(bdt.Fsjh))
            {
                bdt.Fsjxm = FindCwyxmByDm2(bdt.Fsjh);
            }
            string tljdm = FindTljdmByPsdh(ddm, bdt.Psdh); //通过配属段号找到对应的铁路局代码
            if (!string.IsNullOrWhiteSpace(tljdm))
            {
                bdt.Jkjldwdm = tljdm;  //如果铁路局代码不为空，则赋值
            }

            //翻译乘务员车站名
            if (!string.IsNullOrEmpty(bdt.Sfczh))
            {
                bdt.Sfczmc = FindCzmc(ddm, bdt.Jkjldwdm, bdt.Sfsjjlh, bdt.Sfczh);
            }
            if (!string.IsNullOrEmpty(bdt.Zdczh))
            {
                bdt.Zdczmc = FindCzmc(ddm, bdt.Jkjldwdm, bdt.Zdsjjlh, bdt.Zdczh);
            }
            //处理不足4位机车号
            while (bdt.Jch.Length < 4)
            {
                bdt.Jch = "0" + bdt.Jch;
            }
            if (!string.IsNullOrEmpty(bdt.Jcxh))
            {
                bdt.Jxmc = FindByJkjx(bdt.Jcxh, bdt.Jch);
            }
            //翻译机车类别
            if (!string.IsNullOrEmpty(bdt.Jclxbs))
            {
                bdt.Jclxbs = bdt.Jclxbs == "1" ? "D" : "N";
            }

            //获取czmc
            foreach (var item in bdt.bzs)
            {
                var czmc = _db.Ado.GetString(string.Format("select jkczmc from DIC_JKCZ where ddm = '{0}' and jkjldwdm = '{1}' and jkjlh = '{2}' and jkczh = '{3}' ", ddm, bdt.Jkjldwdm, item.Sjjlh, item.Czh));
                if (!string.IsNullOrEmpty(czmc))
                {
                    item.Czmc = czmc;
                }
            }

            _db.Ado.ExecuteCommand("delete LKJ_BDSJ_BDT where filename = :FileName", new { FileName = bdt.Filename });
            int bdtid = _db.Insertable(bdt).ExecuteCommand();
            if (bdtid > 0)
            {
                _db.Ado.BeginTran();
                foreach (var bz in bdt.bzs)
                {
                    bz.Bdtid = bdtid;
                    bz.Dcsj = null;
                    //清空单机数据
                    //----之前和全前沟通的条件---
                    //if (bz.Ls != null && bz.Zz != null && bz.Ls <= 1 && bz.Zz <= 200)
                    //----陈燕双：2024-04-30和滑科以及全科沟通后定下来的条件----
                    //----重车为1、总辆数为1、载重为空时我们就认为是单机----
                    if (bz.Ls == 1 && bz.Zc == 1 && bz.Zaiz == null)
                    {
                        bz.Zz = null;
                        bz.Zaiz = null;
                        bz.Kc = null;
                        bz.Zc = null;
                        bz.Kongc = null;
                        bz.Fyyc = null;
                        bz.Dck = null;
                        bz.Ls = null;
                        bz.Hc = null;

                    }
                    _db.Insertable(bz).ExecuteCommand();
                }
                //取监控车站名称
                //LkjbzService.LkjbzRepository.ExecuteSqlCommand(@"update LKJ_BDSJ_BZ set 
                //czmc = (select jkczmc from dic_jkcz t where ddm = :ddm and jkjldwdm = :jkjldwdm and sjjlh = jkjlh and czh = jkczh and rownum = 1) where bdtid = :BdtId", new { ddm = ddm, jkjldwdm = bdt.Jkjldwdm, BdtId = bdtid });
            }
            else
            {

                return false;
            }
            _db.Ado.CommitTran();
        }
        catch (Exception e)
        {
            _db.Ado.RollbackTran();
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
        return new { success = true };
    }
    [HttpPost("api/Lkjbdt/Insert")]
    [NonUnify]
    public dynamic Insert(LkjBdsj param)
    {
        try
        {
            dynamic dy = InsertLkj(param);
            if (dy.success)
            {
                return new { success = true, data = "" };
            }
            else
            {
                return new { success = false, message = dy.message };
            }
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    [HttpPost("api/Cwyctqxx/ObtainTqxxDetailData")]
    public async Task<bool> ObtainTqxxDetailData(RequestForGetTqxx r)
    {
        _db.Ado.BeginTran();
        _db.Insertable(r.tqxx).ExecuteCommandAsync();
        _db.Ado.ExecuteCommandAsync($" update dic_xtcs set cs='{r.tqxx.Offdutytime.Substring(0, 16)}' where csdm='tqdate' ");
        _db.Ado.CommitTran();
        return true;
    }
    [HttpPost("api/Cwyctqxx/ObtainCqxxDetailData")]
    public async Task<bool> ObtainCqxxDetailData(RequestForGetCqxx r)
    {
        _db.Ado.BeginTran();
        await _db.Insertable(r.cqxx).ExecuteCommandAsync();
        await _db.Ado.ExecuteCommandAsync($" update dic_xtcs set cs='{r.cqxx.Ondutytime.Substring(0, 16)}' where csdm='cqdate' ");
        _db.Ado.CommitTran();
        return true;
    }
    [HttpPost("api/Cwyctqxx/GetCqxxTqxx")]
    [NonUnify]
    public dynamic GetCqxxTqxx(RequestForGetCtqxx r)
    {
        try
        {
            #region 获取时间
            string startDate, endDate, DataType;
            int jgsj = 24;   //默认执获取24小时的数据
            JObject dateTime = new JObject();
            endDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            if (r.CtqName == "1") //出勤
            {
                startDate = _db.Queryable<Xtcs>().Where(t => t.Csdm == "cqdate").First()?.Cs;
                dateTime.Add("begindt", startDate);
                DataType = "Searchdate_Onduytinfo";
            }
            else
            {
                startDate = _db.Queryable<Xtcs>().Where(t => t.Csdm == "tqdate").First()?.Cs;
                dateTime.Add("begindt", startDate);
                DataType = "Searchdate_Offduytinfo";
            }
            if (DateTimeUtil.GetHoursSpan(startDate, endDate, "yyyy-MM-dd HH:mm") > jgsj)  //如果时间差大于24个小时，则取24个小时数据
            {
                dateTime.Add("enddt", DateTime.Parse(startDate).AddHours(jgsj).ToString("yyyy-MM-dd HH:mm"));
            }
            else
            {
                dateTime.Add("enddt", endDate);
            }
            #endregion

            IRestConnection restConnection = new RestConnection("http://10.64.34.200:9060/");
            Dictionary<string, string> paramters = new Dictionary<string, string>();
            paramters.Add("Cid", "379F243CFBF5C00C58FB98A0F534458A253C1A18C9F6AA792129C78AC55192F1DC591471C38BA4A27ED748EC112C11E830BAF20B4881902F");
            paramters.Add("DataType", DataType);
            paramters.Add("Data", dateTime.ToString());

            if (DataType == "Searchdate_Onduytinfo")
            {
                JObject reslutCq = restConnection.Get("interfaceserver/service/Process.do", paramters);
                if (reslutCq.Value<int>("flag") == 1)
                {
                    JArray ctqList = reslutCq.Value<JArray>("datas");
                    return new { success = true, data = ctqList, startdate = startDate, enddate = endDate };

                }
                else
                {
                    return new { success = false, message = "出勤入库失败！失败原因：" + reslutCq };
                }
            }
            else if (DataType == "Searchdate_Offduytinfo")
            {
                JObject reslutTq = restConnection.Get("interfaceserver/service/Process.do", paramters);
                //log.Info(reslutCq.ToString());
                if (reslutTq.Value<int>("flag") == 1)
                {
                    JArray ctqList = reslutTq.Value<JArray>("datas");

                    return new { success = true, data = ctqList, startdate = startDate, enddate = endDate };
                }
                else
                {
                    return new { success = false, message = "退勤入库失败！失败原因：" + reslutTq };
                }
            }
            else
            {
                return new { success = false, message = "入库失败！失败原因：数据类型不匹配！" };
            }
        }
        catch (Exception ex)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(ex) + ",等待下一次处理" };
        }
    }
    [HttpPost("api/Bdsj/UpdateBdsjNhDetail")]
    public bool UpdateBdsjNhDetail(RequestForBdsj r)
    {
        SM4Util sM4Util = new SM4Util();
        if (r.bdsj.Jcxh != null && r.bdsj.Jczl == "D")
        {
            IRestConnection restConnection = new RestConnection("http://10.64.7.204:8101/");
            JObject paramters = new JObject();
            long timestamp = (long)(DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            paramters.Add("cx", r.bdsj.Jcxh);
            paramters.Add("ch", r.bdsj.Jch);
            paramters.Add("param", r.bdsj.Jiec + ":00;" + r.bdsj.Jiaoc + ":00");
            paramters.Add("sso", sM4Util.Encrypt_ECB("whsh;BeiJing@2023.com;" + timestamp));

            var tempResult = restConnection.PostJson("LMD/innerApi/getAmmeterData.do", paramters);
            JObject reslutDb = JObject.Parse(sM4Util.Decrypt_ECB(tempResult));
            if ((reslutDb.Property("result")).Value.ToString().Equals("success"))
            {
                JArray jcDbList = JArray.Parse((reslutDb.Property("datas")).Value.ToString());
                if (jcDbList.Count == 2)
                {
                    if (!string.IsNullOrEmpty(jcDbList[0].Value<string>("time")))
                    {
                        //电能（接）
                        r.bdsj.Jsl1_BF = jcDbList[0].Value<int>("dl_a");
                        r.bdsj.Jsl2_BF = jcDbList[0].Value<int>("dl_b");
                        //再生电（接）
                        r.bdsj.Zsdjsl1_BF = jcDbList[0].Value<int>("fk_a");
                        r.bdsj.Zsdjsl2_BF = jcDbList[0].Value<int>("fk_b");
                    }
                    if (!string.IsNullOrEmpty(jcDbList[1].Value<string>("time")))
                    {
                        //电能（交）
                        r.bdsj.Jcl1_BF = jcDbList[1].Value<int>("dl_a");
                        r.bdsj.Jcl2_BF = jcDbList[1].Value<int>("dl_b");
                        //再生电（交）
                        r.bdsj.Zsdjcl1_BF = jcDbList[1].Value<int>("fk_a");
                        r.bdsj.Zsdjcl2_BF = jcDbList[1].Value<int>("fk_b");
                    }
                }
                var bdsj = r.bdsj;
                string fields = "";
                if (null != bdsj.Jsl1_BF)
                {
                    fields += ",Jsl1_BF=" + bdsj.Jsl1_BF + "";
                }
                if (null != bdsj.Jsl2_BF)
                {
                    fields += ",Jsl2_BF=" + bdsj.Jsl2_BF + "";
                }
                if (null != bdsj.Zsdjsl1_BF)
                {
                    fields += ",Zsdjsl1_BF=" + bdsj.Zsdjsl1_BF + "";
                }
                if (null != bdsj.Zsdjsl2_BF)
                {
                    fields += ",Zsdjsl2_BF=" + bdsj.Zsdjsl2_BF + "";
                }

                if (null != bdsj.Jcl1_BF)
                {
                    fields += ",Jcl1_BF=" + bdsj.Jcl1_BF + "";
                }
                if (null != bdsj.Jcl2_BF)
                {
                    fields += ",Jcl2_BF=" + bdsj.Jcl2_BF + "";
                }
                if (null != bdsj.Zsdjcl1_BF)
                {
                    fields += ",Zsdjcl1_BF=" + bdsj.Zsdjcl1_BF + "";
                }
                if (null != bdsj.Zsdjcl2_BF)
                {
                    fields += ",Zsdjcl2_BF=" + bdsj.Zsdjcl2_BF + "";
                }
                string sql = "update DAT_BDSJ set nhproceeding= '1' " + fields + " where id=" + bdsj.Id;
                _db.Ado.ExecuteCommand(sql);
            }
        }
        return true;
    }
    [HttpPost("api/Bdsj/UpdateBdsjNh")]
    [NonUnify]
    public dynamic UpdateBdsjNh()
    {
        try
        {
            Expressionable<Bdsj> exp = Expressionable.Create<Bdsj>();
            PredicateGroup pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            //pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Bdrq, Operator.Ge, DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd")));
            exp.And(t => t.Bdrq == DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd"));
            exp.And(t => SqlFunc.IsNullOrEmpty(t.NhProceeding));
            //pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.NhProceeding, Operator.Eq, null, false));   //没有更新能耗数据的
            //pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Jcxh, Operator.Eq, null, true));   //没有机车型号的不更新
            exp.And(t => SqlFunc.IsNullOrEmpty(t.Jcxh));
            //pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Jczl, Operator.Eq, "D"));
            exp.And(t => t.Jczl == "D");
            //var pgor = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
            //pgor.Predicates.Add(Predicates.Field<Bdsj>(p => p.Proceeding, Operator.Eq, 3));
            //pgor.Predicates.Add(Predicates.Field<Bdsj>(p => p.Proceeding, Operator.Eq, 4));
            exp.And(t => t.Proceeding == 3 || t.Proceeding == 4);
            //pg.Predicates.Add(pgor);
            var list = _db.Queryable<Bdsj>().Where(exp.ToExpression());
            return new { success = true, data = list, startdate = DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd"), enddate = DateTime.Now.ToString("yyyy-MM-dd") };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    [HttpPost("api/Bdsj/UpdateBdsjAeiDetail")]
    public dynamic UpdateBdsjAeiDetail(RequestForBdsj r)
    {
        string sql = @"select max(case when CDRD=0 then to_char(to_date(substr(tgrq || tgsj,0,12),'yyyy-mm-dd hh24:mi'),'yyyy-mm-dd hh24:mi') else null end) AEI_CD,
            max(case when CDRD=1 then to_char(to_date(substr(tgrq || tgsj,0,12),'yyyy-mm-dd hh24:mi'),'yyyy-mm-dd hh24:mi') else null end) AEI_RD
             from dat_jcaeidetail a  JOIN dat_jcaei b ON a.PARENT_ID = b.ID where txsbjg_cx='{2}' and txsbjg_ch='{3}' and 
          '{0}'<to_char(to_date(substr(tgrq || tgsj,0,12),'yyyy-mm-dd hh24:mi'),'yyyy-mm-dd hh24:mi') and 
          '{1}'>to_char(to_date(substr(tgrq || tgsj,0,12),'yyyy-mm-dd hh24:mi'),'yyyy-mm-dd hh24:mi')";
        Bdsj bdsjAei = _db.Ado.SqlQuerySingle<Bdsj>(string.Format(sql, r.bdsj.Jiec, r.bdsj.Jiaoc, r.bdsj.Jx, r.bdsj.Jch));
        if (bdsjAei != null)
        {
            r.bdsj.Aei_CD = bdsjAei.Aei_CD;
            r.bdsj.Aei_RD = bdsjAei.Aei_RD;
        }
        string fields = "";
        if (!string.IsNullOrEmpty(r.bdsj.Aei_CD))
        {
            fields = ",Aei_CD='" + r.bdsj.Aei_CD + "'";
        }
        if (!string.IsNullOrEmpty(r.bdsj.Aei_RD))
        {
            fields = ",Aei_RD='" + r.bdsj.Aei_RD + "'";
        }
        sql = "update DAT_BDSJ set aeiproceeding= '1' " + fields + " where id=" + r.bdsj.Id;
        _db.Ado.ExecuteCommand(sql);
        return true;
    }
    [HttpPost("api/Bdsj/UpdateBdsjAei")]
    public dynamic UpdateBdsjAei()
    {
        try
        {
            PredicateGroup pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Bdrq, Operator.Ge, DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd")));
            pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.AeiProceeding, Operator.Eq, null, false));   //没有更新AEI数据的
            pg.Predicates.Add(Predicates.Field<Bdsj>(p => p.Jczl, Operator.Eq, "D"));

            var pgor = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
            pgor.Predicates.Add(Predicates.Field<Bdsj>(p => p.Proceeding, Operator.Eq, 3));
            pgor.Predicates.Add(Predicates.Field<Bdsj>(p => p.Proceeding, Operator.Eq, 4));
            pg.Predicates.Add(pgor);

            var list = _dapperRepository.Context.GetList<Bdsj>(pg).ToList();
            return new { success = true, data = list, startdate = DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd"), enddate = DateTime.Now.ToString("yyyy-MM-dd") };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    #endregion

    #region 机车统计系统接口
    /// <summary>
    /// 查询待收集报单
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public IEnumerable<Bdsj> GetDsjBdsj(JObject param)
    {
        var pgMain = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };

        pgMain.Predicates.Add(Predicates.Field<Bdsj>(p => p.Qxbdbs, Operator.Eq, "4", true));  //把便乘、看道等排除

        if (param.ContainsKey("Zpddm") && !string.IsNullOrWhiteSpace(param.Value<string>("Zpddm")))
        {
            pgMain.Predicates.Add(Predicates.Field<Bdsj>(p => p.Zpddm, Operator.Eq, param.Value<string>("Zpddm")));
        }
        //1.机型
        if (param.ContainsKey("Jx") && !string.IsNullOrWhiteSpace(param.Value<string>("Jx")))
        {
            pgMain.Predicates.Add(Predicates.Field<Bdsj>(p => p.Jx, Operator.Eq, param.Value<string>("Jx")));
        }
        //2.机车号
        if (param.ContainsKey("Jch") && !string.IsNullOrWhiteSpace(param.Value<string>("Jch")))
        {
            string[] jchs = null;
            string strStart = "";
            string strEnd = "";
            string strJch = param.Value<string>("Jch");
            if (strJch.Contains("-") || strJch.Contains("~"))
            {
                if (strJch.Contains("-"))
                {
                    strStart = strJch.Substring(0, strJch.IndexOf('-'));
                    strEnd = strJch.Substring(strJch.IndexOf('-') + 1);
                }
                else if (strJch.Contains("~"))
                {
                    strStart = strJch.Substring(0, strJch.IndexOf('~'));
                    strEnd = strJch.Substring(strJch.IndexOf('~') + 1);
                }
                var pgaJch = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
                pgaJch.Predicates.Add(Predicates.Field<Bdsj>(p => p.Jch, Operator.Ge, strStart.PadLeft(4,'0')));
                pgaJch.Predicates.Add(Predicates.Field<Bdsj>(p => p.Jch, Operator.Le, strEnd.PadLeft(4, '0')));

                pgMain.Predicates.Add(pgaJch);
            }
            else if (strJch.Contains(","))
            {
                jchs = param.Value<string>("Jch").Split(',');
                var pgaJch = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
                for (int i = 0; i < jchs.Length; i++)
                {
                    jchs[i] = jchs[i].PadLeft(4, '0');
                    pgaJch.Predicates.Add(Predicates.Field<Bdsj>(p => p.Jch, Operator.Eq, jchs[i]));
                }
                pgMain.Predicates.Add(pgaJch);
            }
            else
            {
                strJch = strJch.PadLeft(4, '0');
                pgMain.Predicates.Add(Predicates.Field<Bdsj>(p => p.Jch, Operator.Eq, strJch));
            }
        }
        //车间
        if (param.ContainsKey("Cjdm") && !string.IsNullOrEmpty(param.Value<string>("Cjdm")))
        {
            pgMain.Predicates.Add(Predicates.Field<Bdsj>(p => p.Cjdm, Operator.Eq, param.Value<string>("Cjdm")));
        }
        //车队
        if (param.ContainsKey("Cddm") && !string.IsNullOrEmpty(param.Value<string>("Cddm")))
        {
            pgMain.Predicates.Add(Predicates.Field<Bdsj>(p => p.Cddm, Operator.Eq, param.Value<string>("Cddm")));
        }
        if (param.ContainsKey("Cwzydddm") && !string.IsNullOrEmpty(param.Value<string>("Cwzydddm")))
        {
            pgMain.Predicates.Add(Predicates.Field<Bdsj>(p => p.Bdtjdddm, Operator.Eq, param.Value<string>("Cwzydddm")));
        }
        //3.司机号
        if (param.ContainsKey("Sjh") && !string.IsNullOrWhiteSpace(param.Value<string>("Sjh")))
        {
            var pgaSjh = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
            pgaSjh.Predicates.Add(Predicates.Field<Bdsj>(p => p.Sjdm1, Operator.Eq, param.Value<string>("Sjh")));
            pgaSjh.Predicates.Add(Predicates.Field<Bdsj>(p => p.Fsjdm1, Operator.Eq, param.Value<string>("Sjh")));
            pgaSjh.Predicates.Add(Predicates.Field<Bdsj>(p => p.Xydm1, Operator.Eq, param.Value<string>("Sjh")));
            pgaSjh.Predicates.Add(Predicates.Field<Bdsj>(p => p.Sjdm2, Operator.Eq, param.Value<string>("Sjh")));
            pgaSjh.Predicates.Add(Predicates.Field<Bdsj>(p => p.Fsjdm2, Operator.Eq, param.Value<string>("Sjh")));
            pgaSjh.Predicates.Add(Predicates.Field<Bdsj>(p => p.Xydm2, Operator.Eq, param.Value<string>("Sjh")));
            pgMain.Predicates.Add(pgaSjh);
        }

        if (param.ContainsKey("Proceeding") && !string.IsNullOrWhiteSpace(param.Value<string>("Proceeding")))
        {
            //4.报单状态：3提交 4回收
            var pgProceeding = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
            foreach (string s in param.Value<string>("Proceeding").Split(','))
            {
                var pgb = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
                pgb.Predicates.Add(Predicates.Field<Bdsj>(p => p.Proceeding, Operator.Eq, s));
                if (s == "3")
                {
                    //4.1.报单提交时间段
                    if (param.ContainsKey("StartDate") && !string.IsNullOrWhiteSpace(param.Value<string>("StartDate")))
                    {
                        pgb.Predicates.Add(Predicates.Field<Bdsj>(p => p.Bdtjsj, Operator.Ge, param.Value<string>("StartDate")));
                    }
                    if (param.ContainsKey("EndDate") && !string.IsNullOrWhiteSpace(param.Value<string>("EndDate")))
                    {
                        pgb.Predicates.Add(Predicates.Field<Bdsj>(p => p.Bdtjsj, Operator.Le, param.Value<string>("EndDate")));
                    }
                }
                else if (s == "4")
                {
                    //4.2.报单回收时间段
                    if (param.ContainsKey("StartDate") && !string.IsNullOrWhiteSpace(param.Value<string>("StartDate")))
                    {
                        pgb.Predicates.Add(Predicates.Field<Bdsj>(p => p.Bdhssj, Operator.Ge, param.Value<string>("StartDate")));
                    }
                    if (param.ContainsKey("EndDate") && !string.IsNullOrWhiteSpace(param.Value<string>("EndDate")))
                    {
                        pgb.Predicates.Add(Predicates.Field<Bdsj>(p => p.Bdhssj, Operator.Le, param.Value<string>("EndDate")));
                    }
                }
                else
                {
                    continue;
                }
                pgProceeding.Predicates.Add(pgb);
            }

            //5.统计状态为空的数据
            pgMain.Predicates.Add(Predicates.Field<Bdsj>(p => p.Tjzt, Operator.Eq, null));

            pgMain.Predicates.Add(pgProceeding);
        }
        if (param.ContainsKey("Tjzh") && !string.IsNullOrWhiteSpace(param.Value<string>("Tjzh")))
        {
            //统计账号
            pgMain.Predicates.Add(Predicates.Field<Bdsj>(p => p.Tjzh, Operator.Eq, param.Value<string>("Tjzh")));
        }
        //按统计状态查询
        if (param.ContainsKey("Tjzt") && !string.IsNullOrWhiteSpace(param.Value<string>("Tjzt")))
        {
            //统计状态:1.收集 2.统计 3. 暂缓统计 4.手工处理 null:未处理
            var pgTjzt = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
            foreach (string s in param.Value<string>("Tjzt").Split(','))
            {
                pgTjzt.Predicates.Add(Predicates.Field<Bdsj>(p => p.Tjzt, Operator.Eq, s));
            }

            pgMain.Predicates.Add(pgTjzt);
            //按统计时间查询
            /*var pga = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };

            var pgb = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            pgb.Predicates.Add(Predicates.Field<Bdsj>(p => p.Sjsj, Operator.Ge, param.Value<string>("StartDate")));
            pgb.Predicates.Add(Predicates.Field<Bdsj>(p => p.Sjsj, Operator.Le, param.Value<string>("EndDate")));
            pga.Predicates.Add(pgb);*/

            var pgc = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            pgc.Predicates.Add(Predicates.Field<Bdsj>(p => p.Tjsj, Operator.Ge, param.Value<string>("StartDate")));
            pgc.Predicates.Add(Predicates.Field<Bdsj>(p => p.Tjsj, Operator.Le, param.Value<string>("EndDate")));
            //pga.Predicates.Add(pgc);
            pgMain.Predicates.Add(pgc);
        }

        IList<ISort> sor = null;
        if (param.ContainsKey("Sort"))
        {
            sor = param.Value<JArray>("Sort").ToObject<List<DapperExtensions.Predicate.Sort>>().ToList<ISort>();
        }
        if (null == sor)
            sor = new List<ISort>();
        if (sor.Count() == 0)
        {
            sor.Add(new DapperExtensions.Predicate.Sort { PropertyName = "Jx", Ascending = true });
            sor.Add(new DapperExtensions.Predicate.Sort { PropertyName = "Jch", Ascending = true });
            sor.Add(new DapperExtensions.Predicate.Sort { PropertyName = "Jiec", Ascending = false });
        }
        var bdsjs = _dapperRepository.Context.GetList<Bdsj>(pgMain, sor);
        return bdsjs;
    }
    /// <summary>
    /// 查询待收集报单
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpGet("api/Bdsj/GetDsjBdsj")]
    [AllowAnonymous]
    [NonUnify]
    public IEnumerable<Bdsj> GetDsjBdsj(string param)
    {
            JObject jo = JObject.Parse(param);
            IEnumerable<Bdsj> bdsjs = GetDsjBdsj(jo);
            return bdsjs;
    }
    /// <summary>
    /// 得到对应机务段的车间信息
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpGet("api/Bdsj/GetCjxx")]
    [AllowAnonymous]
    [NonUnify]
    public dynamic GetCjxx(string param)
    {
        try
        {
            JObject jo = JObject.Parse(param);
            PredicateGroup pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            pg.Predicates.Add(Predicates.Field<Cjxx>(p => p.Ddm, Operator.Eq, jo.Value<string>("Zpddm")));
            IList<ISort> sor = new List<ISort>();
            sor.Add(new DapperExtensions.Predicate.Sort { PropertyName = "Ord", Ascending = true });
            IEnumerable<Cjxx> list = _dapperRepository.Context.GetList<Cjxx>(pg, sor).AsList();
            return new { success = true, data = list };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 得到对应机务段的乘务作业地点信息
    /// </summary>
    /// <param name="jwd"></param>
    /// <returns></returns>
    [HttpGet("api/Bdsj/GetCwzydd")]
    [NonUnify]
    [AllowAnonymous]
    public dynamic GetCwzydd(string param)
    {
        try
        {
            JObject jo = JObject.Parse(param);
            PredicateGroup pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            pg.Predicates.Add(Predicates.Field<Cwzydd>(p => p.Jwddm, Operator.Eq, jo.Value<string>("Zpddm")));
            IList<ISort> sor = new List<ISort>();
            sor.Add(new DapperExtensions.Predicate.Sort { PropertyName = "Ord", Ascending = true });
            IEnumerable<Cwzydd> list = _dapperRepository.Context.GetList<Cwzydd>(pg, sor).AsList();
            return new { success = true, data = list };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    /// <summary>
    /// 得到对应机务段的车队信息
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpGet("api/Bdsj/GetCdxx")]
    [NonUnify]
    [AllowAnonymous]
    public dynamic GetCdxx(string param)
    {
        try
        {
            JObject jo = JObject.Parse(param);
            PredicateGroup pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            pg.Predicates.Add(Predicates.Field<CheDuixx>(p => p.Ddm, Operator.Eq, jo.Value<string>("Zpddm")));
            pg.Predicates.Add(Predicates.Field<CheDuixx>(p => p.CheJianCode, Operator.Eq, jo.Value<string>("Cjdm")));
            IList<ISort> sor = new List<ISort>();
            sor.Add(new DapperExtensions.Predicate.Sort { PropertyName = "CheDuiCode", Ascending = true });
            IEnumerable<CheDuixx> list = _dapperRepository.Context.GetList<CheDuixx>(pg, sor).AsList();
            return new { success = true, data = list };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    /// <summary>
    /// 查询对应统计状态的电子报单数据
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public IEnumerable<Bdsj> GetBdsjByTjzt(JObject param, string tjzt)
    {
        var pgMain = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };

        if (param.ContainsKey("Tjzh") && !string.IsNullOrWhiteSpace(param.Value<string>("Tjzh")))
        {
            //统计账号
            pgMain.Predicates.Add(Predicates.Field<Bdsj>(p => p.Tjzh, Operator.Eq, param.Value<string>("Tjzh")));
            //统计状态:1收集,2统计,3暂缓统计
            pgMain.Predicates.Add(Predicates.Field<Bdsj>(p => p.Tjzt, Operator.Eq, tjzt));
        }

        if (param.ContainsKey("Zpddm") && !string.IsNullOrWhiteSpace(param.Value<string>("Zpddm")))
        {
            //统计账号
            pgMain.Predicates.Add(Predicates.Field<Bdsj>(p => p.Zpddm, Operator.Eq, param.Value<string>("Zpddm")));
        }
        IList<ISort> sor = null;
        if (param.ContainsKey("Sort"))
        {
            sor = param.Value<JArray>("Sort").ToObject<List<DapperExtensions.Predicate.Sort>>().ToList<ISort>();
        }
        if (null == sor)
            sor = new List<ISort>();
        if (sor.Count() == 0)
        {
            sor.Add(new DapperExtensions.Predicate.Sort { PropertyName = "Jx", Ascending = true });
            sor.Add(new DapperExtensions.Predicate.Sort { PropertyName = "Jch", Ascending = true });
            sor.Add(new DapperExtensions.Predicate.Sort { PropertyName = "Jiec", Ascending = true });
        }

        var bdsjs = _dapperRepository.Context.GetList<Bdsj>(pgMain, sor);
        return bdsjs;
    }
    /// <summary>
    /// 查询待统计报单
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpGet("api/Bdsj/GetDtjBdsj")]
    [AllowAnonymous]
    [NonUnify]
    public dynamic GetDtjBdsj(string param)
    {
        try
        {
            JObject jo = JObject.Parse(param);
            IEnumerable<Bdsj> bdsjs = GetBdsjByTjzt(jo, "1");
            return new { success = true, data = bdsjs };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    /// <summary>
    /// 查询暂缓统计报单
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpGet("api/Bdsj/GetZhtjBdsj")]
    [AllowAnonymous]
    [NonUnify]
    public dynamic GetZhtjBdsj(string param)
    {
        try
        {
            JObject jo = JObject.Parse(param);
            IEnumerable<Bdsj> bdsjs =GetBdsjByTjzt(jo, "3");
            return new { success = true, data = bdsjs };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    /// <summary>
    /// 更新统计状态为收集:tjzt=1
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public dynamic UpdateTjztSJ1(JObject param)
    {
        if (param.ContainsKey("Ids") && !string.IsNullOrWhiteSpace(param.Value<string>("Ids")) &&
            param.ContainsKey("Tjzh") && !string.IsNullOrWhiteSpace(param.Value<string>("Tjzh")))
        {
            string sql = string.Format(@"update dat_bdsj set tjzh='{1}',tjzt='1',sjsj=to_char(sysdate,'yyyy-mm-dd hh24:mi:ss') 
                                                        where id in ({0}) and tjzt is null", param.Value<string>("Ids"), param.Value<string>("Tjzh"));
            _db.Ado.ExecuteCommand(sql);

            //返回当前报单，已经被其它账号收集的报单
            sql = string.Format(@"select wm_concat(id) ids from dat_bdsj where id in ({0}) and tjzh<>'{1}'", param.Value<string>("Ids"), param.Value<string>("Tjzh"));
            string ids = _db.Ado.GetString(sql);

            JObject jo = new JObject();
            jo.Add("Ids", ids);
            return new { success = true, data = jo };
        }
        else
        {
            return new { success = false, message = "您传的参数有误，请检查" };
        }
    }
    /// <summary>
    /// 更新统计状态为收集
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpPost("api/Bdsj/UpdateTjztSJ")]
    [NonUnify]
    [AllowAnonymous]
    public dynamic UpdateTjztSJ(JObject param)
    {
        try
        {
            return UpdateTjztSJ1(param);
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    /// <summary>
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public dynamic UpdateTjztTJ1(JObject param)
    {

        if (param.ContainsKey("Ids") && !string.IsNullOrWhiteSpace(param.Value<string>("Ids")) &&
            param.ContainsKey("Tjzh") && !string.IsNullOrWhiteSpace(param.Value<string>("Tjzh")))
        {
            Bdsj bd = _dapperRepository.Context.GetList<Bdsj>(Predicates.Field<Bdsj>(p => p.Id, Operator.Eq, param.Value<string>("Ids"))).FirstOrDefault();
            try
            {
                if (param.ContainsKey("Bzxh"))
                {
                    bd.Bzxh = int.Parse(param.Value<string>("Bzxh"));  //标准消耗
                }
                if (param.ContainsKey("Nhkk"))
                {
                    bd.Nhkk = Math.Round(double.Parse(param.Value<string>("Nhkk")), 2);  //能耗卡控值
                }
                if (param.ContainsKey("Rlbl"))
                {
                    bd.Rlbl = Math.Round(double.Parse(param.Value<string>("Rlbl")), 3);  //燃料比例
                }
                if (param.ContainsKey("Syfs"))
                {
                    bd.Syfs = param.Value<bool>("Syfs");  //上油方式
                }

                if (bd.Bzxh != null && bd.Nhkk != null)
                {
                    double? Sjxh = 0; //实际消耗
                    if (bd.Jczl == "D")
                    {
                        //添加A端消耗
                        if (bd.Jcl1 != null && bd.Jsl1 != null)
                        {
                            Sjxh = bd.Jcl1 - bd.Jsl1;
                        }
                        //添加B端消耗
                        if (bd.Jcl2 != null && bd.Jsl2 != null)
                        {
                            Sjxh += bd.Jcl2 - bd.Jsl2;
                        }
                    }
                    else if (bd.Jcl1 != null && bd.Jsl1 != null)
                    {
                        Sjxh = (bd.Jsl1 - bd.Jcl1) * (bd.Rlbl == null ? 1 : bd.Rlbl) + (bd.Jrl1 == null ? 0 : bd.Jrl1 * (bd.Syfs ? (bd.Rlbl == null ? 1 : bd.Rlbl) : 1));
                    }

                    //没填的也算电量异常
                    if (bd.Jczl == "D")
                    {
                        if ((bd.Jcl1 == null || bd.Jsl1 == null) && (bd.Jcl2 == null || bd.Jsl2 == null))
                        {
                            bd.Nhsfyc = "1";
                        }
                    }
                    else if (bd.Jcl1 == null || bd.Jsl1 == null)
                    {
                        bd.Nhsfyc = "1";
                    }

                    if (Sjxh > 0 && bd.Bzxh != 0 && (Sjxh * (bd.Rlbl == null ? 1 : bd.Rlbl) / bd.Bzxh > bd.Nhkk))
                    {
                        bd.Nhsfyc = "1";
                    }
                    else if (Sjxh <= 0)
                    {
                        bd.Nhsfyc = "1";
                    }
                }

                //再生电异常的，也标记为电量异常
                if (string.IsNullOrEmpty(bd.Nhsfyc) || bd.Nhsfyc == "0")
                {
                    var zsdKcMax = 0;  //空车再生电卡控最大值
                    var zsdZcMax = 0;  //重车再生电卡控最大值
                    if (param.ContainsKey("ZsdKcMax"))
                    {
                        zsdKcMax = int.Parse(param.Value<string>("ZsdKcMax"));
                    }
                    if (param.ContainsKey("ZsdZcMax"))
                    {
                        zsdZcMax = int.Parse(param.Value<string>("ZsdZcMax"));
                    }

                    //编组数据
                    //bd.Bzsjs = BdsjRepository.QuerySql<Bzsj>("select * from DAT_BZSJ t where bdsjid = " + bd.Id).ToList();

                    var bdsjView = new BdsjView(bd);
                    if (zsdKcMax > 0 && zsdZcMax > 0)
                    {
                        int zsd1 = 0;
                        int zsd2 = 0;
                        if (bd.Zsdjcl1 != null && bd.Zsdjsl1 != null)
                        {
                            zsd1 = bd.Zsdjcl1.Value - bd.Zsdjsl1.Value;
                        }
                        if (bd.Zsdjcl2 != null && bd.Zsdjsl2 != null)
                        {
                            zsd2 = bd.Zsdjcl2.Value - bd.Zsdjsl2.Value;
                        }

                        int zsdMax = bdsjView.ZsdKcZc == "1" ? zsdKcMax : zsdZcMax;
                        if ((zsd1 + zsd2) * (bd.Rlbl == null ? 1 : bd.Rlbl) > zsdMax)
                        {
                            bd.Nhsfyc = "1";
                        }
                    }
                }

            }
            catch (Exception ex)
            { }
            bd.Tjzh = param.Value<string>("Tjzh");
            bd.Tjzt = "2";
            bd.Tjsj = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            bd.Sfylr = "1";
            _db.Updateable(bd).ExecuteCommand();
            JObject jo = new JObject();
            return new { success = true, data = jo };
        }
        else
        {
            return new { success = false, message = "请选择要更新的统计数据" };
        }
    }
    /// <summary>
    /// 更新统计状态为统计
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpPost("api/Bdsj/UpdateTjztTJ")]
    [NonUnify]
    [AllowAnonymous]
    public dynamic UpdateTjztTJ(JObject param)
    {
        try
        {
            return UpdateTjztTJ1(param);
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    /// <summary>
    /// 更新统计状态为暂缓统计
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpPost("api/Bdsj/UpdateTjztZhtj")]
    [NonUnify]
    [AllowAnonymous]
    public dynamic UpdateTjztZhtj(JObject param)
    {

        if (param.ContainsKey("Ids") && !string.IsNullOrWhiteSpace(param.Value<string>("Ids")))
        {
            string sql = string.Format(@"update dat_bdsj set tjzt='3' where id in ({0})", param.Value<string>("Ids"));
            _db.Ado.ExecuteCommand(sql);
            JObject jo = new JObject();
            return new { success = true, data = jo };
        }
        else
        {
            return new { success = false, message = "请选择要更新的统计数据" };
        }
    }
    /// <summary>
    /// 更新统计状态为手动处理
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpPost("api/Bdsj/UpdateTjztSdcl")]
    [NonUnify]
    [AllowAnonymous]
    public dynamic UpdateTjztSdcl(JObject param)
    {
        try
        {
            if (param.ContainsKey("Ids") && !string.IsNullOrWhiteSpace(param.Value<string>("Ids")))
            {
                string sql = string.Format(@"update dat_bdsj set tjzt='4' where id in ({0})", param.Value<string>("Ids"));
                _db.Ado.ExecuteCommand(sql);
                JObject jo = new JObject();
                return new { success = true, data = jo };
            }
            else
            {
                return new { success = false, message = "请选择要更新的统计数据" };
            }
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    /// <summary>
    /// 更新统计状态为空(剔除状态)
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("api/Bdsj/UpdateTjztNull")]
    [NonUnify]
    public dynamic UpdateTjztNull(JObject param)
    {
        try
        {
            if (param.ContainsKey("Ids") && !string.IsNullOrWhiteSpace(param.Value<string>("Ids")))
            {
                string sql = string.Format(@"update dat_bdsj set tjzt=null,sjsj=null,tjsj=null,tjzh=null where id in ({0})", param.Value<string>("Ids"));
                _db.Ado.ExecuteCommand(sql);

                JObject jo = new JObject();
                return new { success = true, data = jo };
            }
            else
            {
                return new { success = false, message = "请选择要更新的统计数据" };
            }
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    #endregion

}
