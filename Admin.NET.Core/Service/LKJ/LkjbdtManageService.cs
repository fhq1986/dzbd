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
using Admin.NET.Core.WebService.HybzJctjWs;
using Admin.NET.Core.WebService.KctjWeb;
using Dapper;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Newtonsoft.Json.Linq;
using NPOI.OpenXmlFormats.Dml;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service.LKJ;
/// <summary>
/// 
/// </summary>
[AllowAnonymous]
[NonUnify]
[ApiDescriptionSettings(Order = 390)]
public class LkjbdtManageService : IDynamicApiController, ITransient
{
    ISqlSugarClient _db;
    IDapperRepository _dap;
    public LkjbdtManageService(SqlSugar.ISqlSugarClient db, IDapperRepository dap)
    {
        _db = db;
        _dap = dap;
    }
    [HttpPost]
    public dynamic FindAll(object param)
    {
        try
        {
            JObject jo = JObject.Parse(param.ToString());
            //给定一个默认的排序属性字段
            //LkjBdsjBdt lkjBdt = LkjbdtService.QueryBdt(jo["Zpddm"].ToString(), jo, 0);
            LkjBdsjBdt lkjBdt = QueryBdt(jo["Zpddm"].ToString(), jo, 0);
            if (null != lkjBdt)
            {
                lkjBdt.Zpddm = jo["Zpddm"].ToString();
                GetZbzsj(lkjBdt); //运统一数据，不需要再取集特行
            }
            return new { success = true, data = lkjBdt };
        }
        catch (System.Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    [HttpPost]

    public async void GetZbzsj(LkjBdsjBdt lkjBdt)
    {
        try
        {
            //HybzJctjWsPortTypeClient client = new HybzJctjWsPortTypeClient(HybzJctjWsPortTypeClient.EndpointConfiguration.HybzJctjWsHttpSoap11Endpoint);
            //var result = await client.GetQsyt1ByCzCcAsync(lkjBdt.Zpddm, "", "", "", 5);
            foreach (var bz in lkjBdt.bzs)
            {
                if (bz.Zz > 0)
                {
                    #region 货运
                    if (bz.Khbb == "0" || bz.Khbb == "2")
                    {
                        //确报信息
                        //HybzJctj hybzJctj = new HybzJctj("http://10.3.102.96:8080/HybzJctj/services/HybzJctjWs", 5000);
                        JArray QueryQbsj = new JArray();
                        string dbm;
                        //Jkcz jkcz = JkczService.FindDblmAndJcz(lkjBdt.Jkjldwdm, bz.Sjjlh, bz.Czh);
                        Jkcz jkcz = _db.Ado.SqlQuery<Jkcz>($@"select nvl(t1.dblm,t2.dblm) dblm,t2.jczdblm,t2.fdxs from dic_jkcz t1 
                                                                left join tj_dic_czxx t2 on t1.czdm=t2.tjczdm
                                                                where t1.jkjldwdm='{lkjBdt.Jkjldwdm}' and t1.jkjlh='{bz.Sjjlh}' and t1.jkczh='{bz.Czh}' ").SingleOrDefault();
                        if (jkcz != null)
                        {
                            if (!string.IsNullOrWhiteSpace(jkcz.Dblm))
                            {
                                //hybzJctj.GetQsyt1BtByCzCc(lkjBdt.Zpddm, jkcz.dblm, bz.Cc, bz.Cf, 20, ref QueryQbsj);
                                QueryQbsj=await HybzJctj.GetQsyt1BtByCzCc(lkjBdt.Zpddm, jkcz.Dblm, bz.Cc, bz.Cf, 20);
                            }
                            try
                            {
                                if ((null == QueryQbsj || QueryQbsj.Count <= 0) && !string.IsNullOrWhiteSpace(jkcz.Jczdblm) && !string.IsNullOrWhiteSpace(jkcz.Fdxs))
                                {
                                    //hybzJctj.GetQsyt1BtByCzCc(lkjBdt.Zpddm, jkcz.Jczdblm, bz.Cc, bz.Cf, int.Parse(jkcz.Fdxs) * 60, ref QueryQbsj);
                                    QueryQbsj = await HybzJctj.GetQsyt1BtByCzCc(lkjBdt.Zpddm, jkcz.Jczdblm, bz.Cc, bz.Cf, int.Parse(jkcz.Fdxs) * 60);
                                }
                            }
                            catch (System.Exception e)
                            { }
                            if (null != QueryQbsj && QueryQbsj.Count > 0)
                            {
                                dbm = QueryQbsj[0]["bwm"].ToString();

                                JObject jo = null;
                                int resposei = 0;
                                //读取报文存在超时，失败则读取3次
                                while (resposei < 3)
                                {
                                    //if (hybzJctj.GetQsyt1ByBwm(lkjBdt.Zpddm, dbm, ref jo)) break;
                                    jo = await HybzJctj.GetQsyt1ByBwm(lkjBdt.Zpddm, dbm);
                                    if (jo==null || jo.Count<=0)
                                        break;
                                    resposei += 1;
                                }
                                //如果没有读取数据则跳过本次循环
                                if (jo == null) continue;

                                var zbzs = jo.Value<JArray>("bw").ToList();

                                if (zbzs.Count > 0 && zbzs.Where(p => p.Value<string>("clsysx").Equals("01") ||
                                                                      p.Value<string>("clsysx").Equals("02") ||
                                                                      p.Value<string>("clsysx").Equals("03")).Count() > 0)
                                {
                                    Zbzsj zbz1 = new Zbzsj();
                                    Zbzsj zbz2 = new Zbzsj();
                                    Zbzsj zbz3 = new Zbzsj();

                                    //客行李标示
                                    string[] aryKc = { "KC", "XL" };

                                    bz.Zbzsjs = new List<Zbzsj>();

                                    zbz1.Zz = Convert.ToInt32(zbzs.Where(p => p.Value<string>("clsysx").Equals("01")).Sum(p => p.Value<decimal>("zaiz") + p.Value<decimal>("ziz")));
                                    if (null != zbz1.Zz && zbz1.Zz != 0)
                                    {
                                        zbz1.Zaiz = Convert.ToInt32(zbzs.Where(p => p.Value<string>("clsysx").Equals("01")).Sum(p => p.Value<decimal>("zaiz")));
                                        zbz1.Hc = Convert.ToInt32(zbzs.Where(p => p.Value<string>("clsysx").Equals("01")).Sum(p => p.Value<decimal>("hc")) * 10);
                                        zbz1.Zc = zbzs.Where(p => p.Value<string>("clsysx").Equals("01") && (!p.Value<string>("hwmc").Contains("修")) && p.Value<decimal>("zaiz") > 0).Count();
                                        zbz1.Kongc = zbzs.Where(p => p.Value<string>("clsysx").Equals("01") && (!p.Value<string>("hwmc").Contains("修")) && p.Value<decimal>("zaiz") == 0).Count();
                                        zbz1.Fyyc = zbzs.Where(p => p.Value<string>("clsysx").Equals("01") && p.Value<string>("hwmc").Contains("修")).Count();
                                        zbz1.Ls = zbz1.Zc + zbz1.Kongc;  //货车辆数合计=重车+空车
                                        zbz1.Zaiz = zbz1.Zaiz == 0 ? null : zbz1.Zaiz;
                                        zbz1.Hc = zbz1.Hc == 0 ? null : zbz1.Hc;
                                        zbz1.Zc = zbz1.Zc == 0 ? null : zbz1.Zc;
                                        zbz1.Kongc = zbz1.Kongc == 0 ? null : zbz1.Kongc;
                                        zbz1.Fyyc = zbz1.Fyyc == 0 ? null : zbz1.Fyyc;
                                        zbz1.Ls = zbz1.Ls == 0 ? null : zbz1.Ls;
                                        zbz1.Bzlx = 1;
                                        bz.Zbzsjs.Add(zbz1);
                                    }
                                    zbz2.Zz = Convert.ToInt32(zbzs.Where(p => p.Value<string>("clsysx").Equals("02")).Sum(p => p.Value<decimal>("zaiz") + p.Value<decimal>("ziz")));
                                    if (null != zbz2.Zz && zbz2.Zz != 0)
                                    {
                                        zbz2.Zaiz = Convert.ToInt32(zbzs.Where(p => p.Value<string>("clsysx").Equals("02")).Sum(p => p.Value<decimal>("zaiz")));
                                        zbz2.Hc = Convert.ToInt32(zbzs.Where(p => p.Value<string>("clsysx").Equals("02")).Sum(p => p.Value<decimal>("hc")) * 10);
                                        zbz2.Zc = zbzs.Where(p => p.Value<string>("clsysx").Equals("02") && (!p.Value<string>("hwmc").Contains("修")) && p.Value<decimal>("zaiz") > 0).Count();
                                        zbz2.Kongc = zbzs.Where(p => p.Value<string>("clsysx").Equals("02") && (!p.Value<string>("hwmc").Contains("修")) && p.Value<decimal>("zaiz") == 0).Count();
                                        zbz2.Fyyc = zbzs.Where(p => p.Value<string>("clsysx").Equals("02") && p.Value<string>("hwmc").Contains("修")).Count();
                                        zbz2.Ls = zbz2.Zc + zbz2.Kongc;  //货车辆数合计=重车+空车
                                        zbz2.Zaiz = zbz2.Zaiz == 0 ? null : zbz2.Zaiz;
                                        zbz2.Hc = zbz2.Hc == 0 ? null : zbz2.Hc;
                                        zbz2.Zc = zbz2.Zc == 0 ? null : zbz2.Zc;
                                        zbz2.Kongc = zbz2.Kongc == 0 ? null : zbz2.Kongc;
                                        zbz2.Fyyc = zbz2.Fyyc == 0 ? null : zbz2.Fyyc;
                                        zbz2.Ls = zbz2.Ls == 0 ? null : zbz2.Ls;
                                        zbz2.Bzlx = 2;
                                        bz.Zbzsjs.Add(zbz2);
                                    }
                                    zbz3.Zz = Convert.ToInt32(zbzs.Where(p => p.Value<string>("clsysx").Equals("03") && (!aryKc.Contains(p.Value<string>("czcx")))).Sum(p => p.Value<decimal>("zaiz") + p.Value<decimal>("ziz")));
                                    if (null != zbz3.Zz && zbz3.Zz != 0)
                                    {
                                        zbz3.Zaiz = Convert.ToInt32(zbzs.Where(p => p.Value<string>("clsysx").Equals("03") && (!aryKc.Contains(p.Value<string>("czcx")))).Sum(p => p.Value<decimal>("zaiz")));
                                        zbz3.Hc = Convert.ToInt32(zbzs.Where(p => p.Value<string>("clsysx").Equals("03") && (!aryKc.Contains(p.Value<string>("czcx")))).Sum(p => p.Value<decimal>("hc")) * 10);
                                        zbz3.Zc = zbzs.Where(p => p.Value<string>("clsysx").Equals("03") && (!aryKc.Contains(p.Value<string>("czcx"))) && (!p.Value<string>("hwmc").Contains("修")) && p.Value<decimal>("zaiz") > 0).Count();
                                        zbz3.Kongc = zbzs.Where(p => p.Value<string>("clsysx").Equals("03") && (!aryKc.Contains(p.Value<string>("czcx"))) && (!p.Value<string>("hwmc").Contains("修")) && p.Value<decimal>("zaiz") == 0).Count();
                                        zbz3.Fyyc = zbzs.Where(p => p.Value<string>("clsysx").Equals("03") && (!aryKc.Contains(p.Value<string>("czcx"))) && (p.Value<string>("hwmc").Contains("修"))).Count();
                                        zbz3.Ls = zbz3.Zc + zbz3.Kongc;  //货车辆数合计=重车+空车
                                        zbz3.Zaiz = zbz3.Zaiz == 0 ? null : zbz3.Zaiz;
                                        zbz3.Hc = zbz3.Hc == 0 ? null : zbz3.Hc;
                                        zbz3.Zc = zbz3.Zc == 0 ? null : zbz3.Zc;
                                        zbz3.Kongc = zbz3.Kongc == 0 ? null : zbz3.Kongc;
                                        zbz3.Fyyc = zbz3.Fyyc == 0 ? null : zbz3.Fyyc;
                                        zbz3.Ls = zbz3.Ls == 0 ? null : zbz3.Ls;
                                        zbz3.Bzlx = 3;
                                        bz.Zbzsjs.Add(zbz3);
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region 客运
                    else if ((bz.Khbb == "1" || bz.Khbb == "3") && null != bz.Cf)
                    {
                        //KcccModel kccc = KcccService.FindKcccModes(bz.Cc); //当前车次，查找始发车次，始发时长
                        KcccModel kccc = _db.Queryable<KcccModel>()
                                            .Where(x => x.Lrcc == bz.Cc).Single();
                        string cfrq = bz.Cf.Substring(0, 10).Replace("-", "");  //默认为当前出发时间
                        string sfcc = bz.Cc;  //默认为当前车次
                        if (null != kccc)
                        {
                            sfcc = kccc.Sfcc;    //给始发车次赋值
                            try
                            {
                                //如果有始发时长，则计算出发日期
                                if (!string.IsNullOrWhiteSpace(kccc.Sfsc) && kccc.Sfsc != "00:00" && kccc.Sfsc.Contains(":"))
                                {
                                    string[] sfsc = kccc.Sfsc.Split(':');
                                    if (sfsc.Length == 2)
                                    {
                                        //计算始发日期
                                        cfrq = DateTime.Parse(bz.Cf).AddHours(-int.Parse(sfsc[0])).AddMinutes(-int.Parse(sfsc[1])).ToString("yyyyMMdd");
                                    }
                                }
                            }
                            catch (System.Exception ex)
                            { }
                        }
                        //FeedBackService ws = new FeedBackService();
                        //ws.Timeout = 5000;
                        //string sKybz = ws.getKybz(cfrq, sfcc);
                        FeedBackServicePortTypeClient client = new FeedBackServicePortTypeClient(FeedBackServicePortTypeClient.EndpointConfiguration.FeedBackServiceHttpPort);
                        string sKybz = await client.getKybzAsync(cfrq, sfcc);


                        string[] tsKybz = sKybz.Split('@');

                        if (tsKybz.Length >= 2)
                        {
                            double zz = 0, zaiz = 0, hc = 0;
                            int ls = 0;  //总重，载重，换长，量数
                            for (int i = 1; i < tsKybz.Length; i++)
                            {
                                string[] tsKybzjl = tsKybz[i].Split(',');
                                if (tsKybzjl.Length == 17)
                                {
                                    string sCz = tsKybzjl[1].ToUpper();
                                    if (sCz == "XL" || sCz == "UZ")
                                    {
                                        zz += double.Parse(tsKybzjl[5]) + double.Parse(tsKybzjl[6]);  //自重+载重
                                        zaiz += double.Parse(tsKybzjl[6]);  //载重
                                        hc += double.Parse(tsKybzjl[7]) * 10;  //换长;
                                        ls += 1;
                                    }
                                }
                            }
                            if (ls > 0)
                            {

                                bz.Zbzsjs = new List<Zbzsj>();

                                Zbzsj zbz1 = new Zbzsj();
                                zbz1.Zz = (int)zz;
                                zbz1.Zaiz = (int)zaiz;
                                zbz1.Hc = (int)hc;
                                zbz1.Kc = ls;
                                zbz1.Ls = ls;
                                zbz1.Bzlx = 3;

                                bz.Zbzsjs.Add(zbz1);
                            }
                        }
                    }
                    #endregion
                }
            }
        }
        catch (System.Exception e)
        {

        }
    }

    private LkjBdsjBdt QueryBdt(string ddm, JObject jo, int minute, QueryBdt_Extend extend = null)
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

        //List<LkjBdsjBdt> bdts = LkjbdtRepository.QueryBdt(sqlText).ToList();
        List<LkjBdsjBdt> bdts = _db.Ado.SqlQuery<LkjBdsjBdt>(sqlText).ToList();
        if (bdts.Count() == 0 || bdts == null) return null;

        LkjBdsjBdt lkjBdt = new LkjBdsjBdt();

    #region 排除时间段重复
    repeat:
        //如果只有一个报单头，则读取编组信息，直接返回
        if (bdts.Count() > 1)
        {
            if (minute > 0 && DateTime.Now <= Util.DateUtil.getDateTime(bdts.Max(p => p.Bdrksj), "yyyy-MM-dd HH:mm:ss").AddMinutes(minute))
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
                //var tmpBzList = LkjbdtRepository.QuerySql<LkjBdsjBz>(tmpSql).ToList();  //所有编组
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
                    //tmpBdts.Each(x => x.Jch = excepts.FirstOrDefault().Jch);
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

        GetBdtData(lkjBdt); //返回第一个报单头对应的编组信息

        //翻译乘务员姓名(如果"姓名"为空，则通过"司机号"匹配对应的"姓名")
        if (string.IsNullOrEmpty(lkjBdt.Sjxm) && !string.IsNullOrEmpty(lkjBdt.Sjh))
        {
            //lkjBdt.Sjxm = CwyxxService.FindCwyxmByDm2(lkjBdt.Sjh);
            lkjBdt.Sjxm = FindCwyxmByDm2(lkjBdt.Sjh);
        }
        if (string.IsNullOrEmpty(lkjBdt.Fsjxm) && !string.IsNullOrEmpty(lkjBdt.Fsjh))
        {
            //lkjBdt.Fsjxm = CwyxxService.FindCwyxmByDm2(lkjBdt.Fsjh);
            lkjBdt.Fsjxm = FindCwyxmByDm2(lkjBdt.Fsjh);
        }
        string jch = "";
        for (int i = 1; i < bdts.Count(); i++)
        {
            //翻译乘务员姓名(如果司机号不同，"姓名"为空，则通过"司机号"匹配对应的"姓名")
            if (lkjBdt.Sjh != bdts[i].Sjh && string.IsNullOrEmpty(bdts[i].Sjxm) && !string.IsNullOrEmpty(bdts[i].Sjh))
            {
                //bdts[i].Sjxm = CwyxxService.FindCwyxmByDm2(bdts[i].Sjh);
                bdts[i].Sjxm = FindCwyxmByDm2(lkjBdt.Sjh);
            }
            if (!string.IsNullOrEmpty(bdts[i].Fsjh) && lkjBdt.Fsjh != bdts[i].Fsjh && string.IsNullOrEmpty(bdts[i].Fsjxm))
            {
                //bdts[i].Fsjxm = CwyxxService.FindCwyxmByDm2(bdts[i].Fsjh);
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

        //Jkjx jx = JkjxService.FindAll().Where(p => p.Jxdm == lkjBdt.Jxmc).FirstOrDefault();
        Jkjx jx = _db.Queryable<Jkjx>().Where(p => p.Jxdm == lkjBdt.Jxmc).Single();
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
        Jkcz modelJkcz;
        if (string.IsNullOrEmpty(lkjBdt.Sfczmc) && !string.IsNullOrEmpty(lkjBdt.Sfczh))
        {
            //lkjBdt.Sfczmc = JkczService.FindCzmc(ddm, lkjBdt.Jkjldwdm, lkjBdt.Sfsjjlh, lkjBdt.Sfczh);
            modelJkcz = _db.Queryable<Jkcz>()
                            .Where(x => x.Jkjldwdm == lkjBdt.Jkjldwdm
                                        && x.Ddm == ddm
                                        && x.Jkjlh == lkjBdt.Sfsjjlh
                                        && x.Jkczh == lkjBdt.Sfczh)
                            .Single();
            lkjBdt.Sfczmc = modelJkcz == null ? "" : modelJkcz.Jkczmc;
        }
        //如果"终到车站名称"为空，则通过"终到车站号"匹配对应的"终到车站名称"
        if (string.IsNullOrEmpty(lkjBdt.Zdczmc) && !string.IsNullOrEmpty(lkjBdt.Zdczh))
        {
            //lkjBdt.Zdczmc = JkczService.FindCzmc(ddm, lkjBdt.Jkjldwdm, lkjBdt.Zdsjjlh, lkjBdt.Zdczh);
            modelJkcz = _db.Queryable<Jkcz>()
                            .Where(x => x.Jkjldwdm == lkjBdt.Jkjldwdm
                                        && x.Ddm == ddm
                                        && x.Jkjlh == lkjBdt.Zdsjjlh
                                        && x.Jkczh == lkjBdt.Zdczh)
                            .Single();
            lkjBdt.Zdczmc = modelJkcz == null ? "" : modelJkcz.Jkczmc;
        }

        #endregion

        lkjBdt.bzs = RecombinationBz(ddm, lkjBdt);  //重组报单数据，对于数据和上一条相同的，则显示为空

        //lkjBdt.bzs = AddZjgls(lkjBdt.Zpddm,lkjBdt.bzs);
        //处理不足4位机车号,动车的机车号                                   
        while (lkjBdt.Jch.Length < 4)
        {
            lkjBdt.Jch = "0" + lkjBdt.Jch;
        }

        Cwyxx cwy = GetCwyxx(lkjBdt.Sjh);
        if (cwy != null)
        {
            lkjBdt.Cjdm = cwy.Cjdm;  //取得乘务员对应的车间代码
            lkjBdt.Cddm = cwy.CheDuiCode;  //取得乘务员对应的车队代码
        }
        return lkjBdt;
    }

    //根据工号返回乘务员信息
    public Cwyxx GetCwyxx(string cwydm)
    {
        //Cwyxx cWyxx = new Cwyxx();
        //cWyxx = CwyxxService.FindAll().Where(p => p.Cwydm == cwydm).FirstOrDefault();
        //return cWyxx;
        return _db.Queryable<Cwyxx>().Where(p => p.Cwydm == cwydm).Single();
    }
    /// <summary>
    /// 重组编组列表
    /// </summary>
    /// <param name="ddm"></param>
    /// <param name="lkjBdt"></param>
    /// <returns></returns>
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
    private Jkcz FindByCzh(string ddm, string sJkjldw, string sJkjlh, string sCzh)
    {
        Jkcz result = new Jkcz();
        IList<Jkcz> jkczs = new List<Jkcz>();
        // 解析字符串
        string sql = $@"select * from dic_jkcz jk left outer join tj_dic_czxx cz on jk.ddm=cz.ddm and jk.czdm=cz.tjczdm
                                    where jk.ddm = '" + ddm + "' and jk.Jkjldwdm = '" + sJkjldw + "' and  jk.Jkjlh='" + sJkjlh + "' and jk.Jkczh= '" + sCzh + "'";
        //var list = (JkczRepository.Context as IDapperRepositoryContext).Connection.Query<Jkcz, TjCzxx, Jkcz>(
        //           sql, (jkcz, czxx) =>
        //           {
        //               //看数据有没有在现有的列表中，没有则加入
        //               Jkcz jkczTmp = jkczs.Where(m => m.Id == jkcz.Id).FirstOrDefault();
        //               if (null == jkczTmp)
        //               {
        //                   if (czxx != null)
        //                   {
        //                       jkcz.Tjczmc = czxx.Czmc;    //得到计统车站名称
        //                       jkcz.Bdbs = czxx.Bdbs;
        //                       jkcz.Wdbs = czxx.Wdbs;
        //                       jkcz.Bdyx = czxx.Bdyx;
        //                       jkcz.Wdyx = czxx.Wdyx;
        //                       if (!string.IsNullOrWhiteSpace(czxx.Dblm))
        //                       {
        //                           jkcz.Dblm = czxx.Dblm;
        //                       }
        //                   }
        //                   jkczs.Add(jkcz);
        //                   jkczTmp = jkcz;
        //               }
        //               return null;
        //           });
        var list = _dap.Context.Query<Jkcz, TjCzxx, Jkcz>(
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
    /// <summary>
    /// 通过配属段号找到对应的铁路局代码
    /// </summary>
    /// <param name="ddm"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    private string FindTljdmByPsdh(string ddm, string param)
    {
        //List<Jkjldw> dws = JkjldwService.FindAll().Where(p => p.Ddm == ddm).OrderBy(p => p.Ord).ToList();
        List<Jkjldw> dws = _db.Queryable<Jkjldw>().Where(x => x.Ddm == ddm).OrderBy(x => x.Ord).ToList();
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

            //Jwdxx jwd = FindAll(ipd).FirstOrDefault();
            Jwdxx jwd = _db.Queryable<Jwdxx>().Where(x => x.JK_PSDH == param).Single();
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

    private string FindByJkjx(string jcxh, string jch)

    {
        //IPredicate jxpredicate = Predicates.Field<Jkjx>(p => p.Jkjxdm, Operator.Eq, jcxh);
        //IList<Jkjx> jkjxs = FindAll(jxpredicate).ToList();
        List<Jkjx> jkjxs = _db.Queryable<Jkjx>().Where(x => x.Jkjxdm == jcxh).ToList();
        if (jcxh != "239" && jkjxs != null && jkjxs.Count() == 1)  //1. 239：HXD3CA和HXD3C排除 2.dic_jkjx表多个机型对应同一个jkjxdm
        {
            return jkjxs[0].Jxdm;
        }
        else
        {
            string jkjx = null;
            //IPredicate jchpredicate = Predicates.Field<Jchxx>(p => p.Jch, Operator.Eq, jch);
            //IList<Jchxx> jchs = jchService.FindAll(jchpredicate).ToList();
            List<Jchxx> jchs = _db.Queryable<Jchxx>().Where(x => x.Jch == jch).ToList();
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

    /// <summary>
    /// 对两组报单头数据，整合成一组报单头数据
    /// </summary>
    /// <param name="bdt"></param>
    /// <param name="bdt2"></param>
    /// <param name="jch"></param>
    private void TransBdtData(LkjBdsjBdt bdt, LkjBdsjBdt bdt2, out string jch)
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
            GetBdtData(bdt2); //得到编组信息
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

    //通过机车型号,判断是否是动车
    public bool isDc(string jcxh)
    {
        return ((!string.IsNullOrEmpty(jcxh)) && (jcxh.Length > 3 || (jcxh.Length >= 3 && jcxh.CompareTo("300") >= 0)));
    }
    private string FindCwyxmByDm2(string cwydm)
    {
        //ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        string result = "";
        try
        {
            //IPredicate jwdpredicate = Predicates.Field<Cwyxx>(p => p.Cwydm, Operator.Eq, cwydm);
            //Cwyxx cwyxx = FindAll(jwdpredicate).FirstOrDefault();
            Cwyxx cwyxx = _db.Queryable<Cwyxx>()
                            .Where(x => x.Cwydm == cwydm).First();
            if (null != cwyxx)
            {
                result = cwyxx.Cwyxm;
            }
        }
        catch (System.Exception e)
        {
            //log.Error("FindCwyxmByDm2 " + cwydm + ExceptionUtil.GetExceptionMsg(e));
        }
        return result;
    }

    //得到报单头对应的编组信息
    private void GetBdtData(LkjBdsjBdt lkjBdt)
    {
        List<LkjBdsjBz> bzs = new List<LkjBdsjBz>();
        List<LkjBdsjBz> bz = new List<LkjBdsjBz>();

        #region MyRegion
        //bz = LkjbzService.QueryBz(lkjBdt.Id.ToString()).ToList();
        string sqlText = string.Format(@"select * from lkj_bdsj_bz where bdtid='" + lkjBdt.Id.ToString() + "' order by bzcs");
        //List<LkjBdsjBz> bz = LkjbzRepository.QueryBz(sqlText).ToList();
        bz = _db.Ado.SqlQuery<LkjBdsjBz>(sqlText).ToList();
        foreach (LkjBdsjBz b in bz)
        {
            bzs.Add(b);
        }
        lkjBdt.bzs = bzs;
        #endregion
        foreach (LkjBdsjBz b in bz)
        {
            bzs.Add(b);
        }
        lkjBdt.bzs = bzs;
    }
}
