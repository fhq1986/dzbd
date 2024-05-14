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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Core.Hub.Bdgl;
using Admin.NET.Core.Util;
using Newtonsoft.Json.Linq;
using NPOI.OpenXmlFormats.Dml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using StackExchange.Redis;
using Admin.NET.Core.DapperEx;
using Dapper;
using static NPOI.HSSF.Util.HSSFColor;
using System.Runtime.Intrinsics.Arm;
using Microsoft.AspNetCore.SignalR;
using static Abp.Domain.Uow.AbpDataFilters;

namespace Admin.NET.Core.Service.BdglManage;

/// <summary>
/// 报单数据信息管理接口
/// </summary>
[NonUnify]
[ApiDescriptionSettings(Order = 390)]
public class BdsjManageService : IDynamicApiController, ITransient
{
    ISqlSugarClient _db;
    IDapperRepository _dap;
    IHubContext<BdhsHub, IBdffHub> _bdffHubContext;
    public BdsjManageService(SqlSugar.ISqlSugarClient db, IDapperRepository dap, IHubContext<BdhsHub, IBdffHub> bdffHubContext)
    {
        _db = db;
        _dap = dap;
        _bdffHubContext=bdffHubContext;
    }

    /// <summary>
    /// 通过机型和机车号，查询上一个报单信息
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet]
    [Route("/api/Bdsj/GetPreBdsjByJxJch")]
    public dynamic GetPreBdsjByJxJch(string param)
    {
        JObject jo = JObject.Parse(param);
        string Jx = jo["Jx"].ToString();
        string Jch = jo["Jch"].ToString();
        //Bdsj bdsj = BdsjService.GetPreBdsjByJxJch(Jx, Jch);
        //DateTime dt = DateTime.Now.AddDays(-30).Date;
        //var query = _db.Queryable<DAT_BDSJ>()
        //                .Where(x => x.JX == Jx
        //                            && x.JCH == Jch
        //                            && !SqlFunc.IsNullOrEmpty(x.BDRQ))
        //                .Where(x => SqlFunc.ToDate(x.BDRQ) > SqlFunc.ToDate(dt))
        //                .OrderByDescending(x => SqlFunc.ToDate(x.BDRQ));
        //DAT_BDSJ model = query.First();
        //由于BDRQ是字符串类型，增加BDRQ的查询条件（先将BDRQ转成日期类型）则会报错，所以使用SQL
        string sqlText = @"select * from dat_bdsj  where jx='{0}' and jch='{1}' and 
                                to_date(bdrq,'yyyy-mm-dd')>sysdate-30  order by bdrq desc";
        DAT_BDSJ model = _db.SqlQueryable<DAT_BDSJ>(string.Format(sqlText, Jx, Jch)).First();
        if(model==null)
            return new { data = model };
        Bdsj bdsj = new Bdsj()
        {
            Id = (int)model.ID,
            Sjdm1 = model.SJDM1,
            Sjxm1 = model.SJXM1,
            Fsjdm1 = model.FSJDM1,
            Fsjxm1 = model.FSJXM1,
            Xydm1 = model.XYDM1,
            Xyxm1 = model.XYXM1,
            Sjdm2 = model.SJDM2,
            Sjxm2 = model.SJXM2,
            Fsjdm2 = model.FSJDM2,
            Fsjxm2 = model.FSJXM2,
            Xydm2 = model.XYDM2,
            Xyxm2 = model.XYXM2,
            Zcfsdm = model.ZCFSDM,
            Zcfsmc = model.ZCFSMC,
            Jczl = model.JCZL,
            Jx = model.JX,
            Jch = model.JCH,
            Bdrq = model.BDRQ,
            Jied = model.JIED,
            Cq = model.CQ,
            Jiec = model.JIEC,
            Jiaoc = model.JIAOC,
            Tq = model.TQ,
            Cbd = model.CBD,
            Rwd = model.RWD,
            Cwd = model.CWD,
            Rbd = model.RBD,
            Jsl1 = model.JSL1,
            Jrl1 = model.JRL1,
            Jcl1 = model.JCL1,
            Zsdjsl1 = model.ZSDJSL1,
            Zsdjcl1 = model.ZSDJCL1,
            Lcgdjsl1 = model.LCGDJSL1,
            Lcgdjcl1 = model.LCGDJCL1,
            Jsl2 = model.JSL2,
            Jrl2 = model.JRL2,
            Jcl2 = model.JCL2,
            Zsdjsl2 = model.ZSDJSL2,
            Zsdjcl2 = model.ZSDJCL2,
            Lcgdjsl2 = model.LCGDJSL2,
            Lcgdjcl2 = model.LCGDJCL2,
            Dwsj = model.DWSJ,
            Dwyl = model.DWYL,
            Yzmc1 = model.YZMC1,
            Yzyl1 = model.YZYL1,
            Yzmc2 = model.YZMC2,
            Yzyl2 = model.YZYL2,
            Yzmc3 = model.YZMC3,
            Yzyl3 = model.YZYL3,
            Yzmc4 = model.YZMC4,
            Yzyl4 = model.YZYL4,
            Yzmc5 = model.YZMC5,
            Yzyl5 = model.YZYL5,
            Yzmc6 = model.YZMC6,
            Yzyl6 = model.YZYL6,
            Yzmc7 = model.YZMC7,
            Yzyl7 = model.YZYL7,
            Qrcode = model.QRCODE,
            Proceeding = model.PROCEEDING,
            Cwtqdddm = model.CWTQDDDM,
            Cwcqdddm = model.CWCQDDDM,
            Cwjldm = model.CWJLDM,
            Cwjlmc = model.CWJLMC,
            Bdtjdddm = model.BDTJDDDM,
            Bdhsqk = model.BDHSQK,
            Sjgzdm1 = model.SJGZDM1,
            Fsjgzdm1 = model.FSJGZDM1,
            Xygzdm1 = model.XYGZDM1,
            Sjgzdm2 = model.SJGZDM2,
            Fsjgzdm2 = model.FSJGZDM2,
            Xygzdm2 = model.XYGZDM2,
            Sjsrdm1 = model.SJSRDM1,
            Fsjsrdm1 = model.FSJSRDM1,
            Xysrdm1 = model.XYSRDM1,
            Sjsrdm2 = model.SJSRDM2,
            Fsjsrdm2 = model.FSJSRDM2,
            Xysrdm2 = model.XYSRDM2,
            Cwgsdddm = model.CWGSDDDM,
            Bdffr = model.BDFFR,
            Bdhsr = model.BDHSR,
            Fwbj = model.FWBJ,
            Fwzgs = model.FWZGS,
            Fwwd = model.FWWD,
            Fwwj = model.FWWJ,
            Zpjdm = model.ZPJDM,
            Zpjmc = model.ZPJMC,
            Zpddm = model.ZPDDM,
            Zpdmc = model.ZPDMC,
            Lrjdm = model.LRJDM,
            Lrjmc = model.LRJMC,
            Lrddm = model.LRDDM,
            Lrdmc = model.LRDMC,
            Cwyzb1 = model.CWYZB1,
            Cwyzb2 = model.CWYZB2,
            Cwyzb3 = model.CWYZB3,
            Cwyzb4 = model.CWYZB4,
            Cwyzb5 = model.CWYZB5,
            Cwyzb6 = model.CWYZB6,
            Bdxgr = model.BDXGR,
            Bdffsj = model.BDFFSJ,
            Bdtjsj = model.BDTJSJ,
            Bdhssj = model.BDHSSJ,
            Bdxgsj = model.BDXGSJ,
            Jkjldwdm = model.JKJLDWDM,
            Sfsjjlh = model.SFSJJLH,
            Sfsrjlh = model.SFSRJLH,
            Sfczh = model.SFCZH,
            Sfczmc = model.SFCZMC,
            Sfcc = model.SFCC,
            Zdsjjlh = model.ZDSJJLH,
            Zdsrjlh = model.ZDSRJLH,
            Zdczh = model.ZDCZH,
            Zdczmc = model.ZDCZMC,
            Zdcc = model.ZDCZMC,
            Tjzt = model.TJZT,
            Sjsj = model.SJSJ,
            Tjsj = model.TJSJ,
            Tjzh = model.TJZH,
            Yt1bdsjid = model.YT1BDSJID,
            Bdzfsj = model.BDZFSJ,
            Bdzfr = model.BDZFR,
            Lcgdjsl1_2 = string.IsNullOrWhiteSpace(model.LCGDJSL1_2) ? null : int.Parse(model.LCGDJSL1_2.ToString()),
            Lcgdjcl1_2 = string.IsNullOrWhiteSpace(model.LCGDJCL1_2) ? null : int.Parse(model.LCGDJCL1_2.ToString()),
            Lcgdjsl2_2 = string.IsNullOrWhiteSpace(model.LCGDJSL1_2) ? null : int.Parse(model.LCGDJSL2_2.ToString()),
            Lcgdjcl2_2 = string.IsNullOrWhiteSpace(model.LCGDJCL1_2) ? null : int.Parse(model.LCGDJCL2_2.ToString()),
            Eventtime = model.EVENTTIME,
            Bzxh = (int?)model.BZXH,
            Nhkk = model.NHKK,
            Rlbl = model.RLBL,
            Syfs = model.SYFS,
            Nhsfyc = model.NHSFYC,
            Jchsfyc = model.JCHSFYC,
            Jx_Wrcl = model.JX_WRCL,
            Jch_Wrcl = model.JCH_WRCL,
            Sfylr = model.SFYLR,
            Cjdm = model.CJDM,
            Gzfs = model.GZFS,
            Bdhtsj = model.BDHTSJ,
            Bdhtr = model.BDHTR,
            Qxbdbs = model.QXBDBS,
            Bdlxdm = model.BDLXDM,
            Bdlxmc = model.BDLXMC,
            Zzbdbh = model.ZZBDBH,
            //CreateDate = model.CREATEDATE,
            Cddm = model.CDDM,
            Bdtjly = model.BDTJLY,
            Sdzk = model.SDZK,
            Dbsfsh1 = model.DBSFSH1,
            Dbsfsh2 = model.DBSFSH2,
            Jcxh = model.JCXH,
            Zsdjcl2_BF = (int?)model.ZSDJCL2_BF,
            Zsdjsl2_BF = (int?)model.ZSDJSL2_BF,
            Jcl2_BF = (int?)model.JCL2_BF,
            Jsl2_BF = (int?)model.JSL2_BF,
            Zsdjcl1_BF = (int?)model.ZSDJCL1_BF,
            Zsdjsl1_BF = (int?)model.ZSDJSL1_BF,
            Jcl1_BF = (int?)model.JCL1_BF,
            Jsl1_BF = (int?)model.JSL1_BF,
            Aei_RD = model.AEI_RD,
            Aei_CD = model.AEI_CD,
            //AeiProceeding="",
            //Bdlymc="",
            //Bzsjs="",
            //Clbdsjs=""
            //Db=""
            //Hssfyz=""
            //IsWjch=""
            //NhProceeding=""
        };
        return new { data = bdsj };
    }
    /// 通过乘务员代码和出勤信息，得到退勤信息
    [HttpGet]
    [Route("/api/Bdsj/GetTqsjByCq")]
    public dynamic GetTqsjByCq(string param)
    {
        Dictionary<string, string> paramters = new Dictionary<string, string>();
        paramters.Add("param", param);

        //return ZgsService.GetToZgs(paramters, "api/Bdsj/GetTqsjByCq");
        return GetToZgs(paramters, "api/Bdsj/GetTqsjByCq");
    }

    private dynamic GetToZgs(Dictionary<string, string> parameters, string method)
    {
        string msg;
        //string url = XtcsService.FindAll().Where(p => p.Csdm.Equals("zgsurl")).FirstOrDefault().Cs;
        string url= _db.Queryable<Xtcs>().Where(x => x.Csdm == "zgsurl").Single().Cs;
        IRestConnection restConnection = new RestConnection(url);
        if (restConnection.Login(@"/Token", "username", "cf", "password", "123", out msg))
        {
            JObject reslut = restConnection.Get(method, parameters);
            // 断开连接
            restConnection.Logout(url);
            if (reslut.Value<bool>("success"))
            {
                return new { success = true, data = reslut["data"] };
            }
            else
            {
                return new { success = false, data = reslut["message"] };
            }
        }
        else
        {
            return new { success = false, message = "总公司连接失败！" };
        }
    }

    /// <summary>
    /// 无出勤信息的报单数据
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("/api/Bdsj/WcqKbd")]
    public dynamic WcqKbd(string param)
    {
        Bdsj bdsj = new Bdsj();
        //BdsjService.WcqKbd(bdsj, param);
        WcqKbd(bdsj, param);
        return new { data = bdsj };
    }

    /// <summary>
    /// 查询出退勤时间段内是否已经填写报单
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("/api/Bdsj/GetBdsjByCtq")]
    public dynamic GetBdsjByCtq(string param)
    {
        //Bdsj bdsj = BdsjService.GetBdsjByCtq(param);
        #region BdsjService.GetBdsjByCtq(param);
        Bdsj bdsj = GetBdsjByCtqService(param);
        #endregion
        return new { data = bdsj };
    }

    /// <summary>
    /// 查询出退勤时间段内是否已经填写报单
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("/api/Bdsj/GetBdsjByCtq2")]
    public dynamic GetBdsjByCtq2(string param)
    {
        //Bdsj bdsj = BdsjService.GetBdsjByCtq(param);
        Bdsj bdsj = GetBdsjByCtqService(param);
        if (null != bdsj)
        {
            string content = "您已有";
            if (!string.IsNullOrWhiteSpace(bdsj.Cq))
            {
                content += bdsj.Cq + "出勤 ";
            }
            if (!string.IsNullOrWhiteSpace(bdsj.Tq))
            {
                content += bdsj.Tq + "退勤";
            }
            content += "的报单,请勿重复填写";

            return new { success = false, message = content };
        }
        return new { success = true };
    }

    [HttpGet]
    [Route("/api/Bdsj/FindAllBdsj")]
    public dynamic FindAllBdsj(string param)
    {
        //1.获取当前用户运统1退勤为空的报单，重新获取运统1
        //2.生成LKJ和运统一未结合的报单
        //3.将未生成电子报单的记录，保存到电子报单中，只保存报单头信息
        //4.获取所有报单列表
        JObject jo = JObject.Parse(param);

        //兼容有些一体机的退勤时间带秒
        if (jo.ContainsKey("tqsj") && !string.IsNullOrEmpty(jo["tqsj"].ToString()))
        {
            if (jo["tqsj"].ToString().Trim().Length > 16)
            {
                jo["tqsj"] = jo["tqsj"].ToString().Trim().Substring(0, 16);
            }
        }

        IList<Bdsj> bdsjs = EmptyBdsj(jo);
        return new { success = true, data = new { total = bdsjs.Count, items = bdsjs } };
    }

    [HttpPost]
    [Route("/api/Bdsj/Update")]
    public dynamic Update([FromBody]Bdsj bdsj)
    {
        try
        {
            bdsj.Bdtjsj = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");  //报单提交时间
            bdsj.Proceeding = 3;
            try
            {
                //Jwdxx jwdxx = JwdxxService.FindAll(Predicates.Field<Jwdxx>(p => p.Jwddm, Operator.Eq, bdsj.Zpddm)).FirstOrDefault();
                Jwdxx jwdxx = _db.Queryable<DIC_JWD>()
                                .Where(x => x.JWDDM == bdsj.Zpddm)
                                .Select(x=>new Jwdxx()
                                { 
                                    Id=x.ID,
                                    Jwddm=x.JWDDM,
                                    Jwdmc=x.JWDMC,
                                    Jwbm=x.JWBM,
                                    Tljdm=x.TLJDM,
                                    Ord=SqlFunc.IIF(SqlFunc.IsNullOrEmpty(x.ORD),"",x.ORD.ToString()),
                                    Qydm=x.QYDM,
                                    JK_PSDH= SqlFunc.IIF(SqlFunc.IsNullOrEmpty(x.JK_PSDH), "", x.JK_PSDH.ToString()),
                                    Zdbs=x.ZDBS,
                                    Yt1bs=x.YT1BS,
                                    Lcgd=x.LCGD,
                                    Dybs=x.DYBS,
                                    Ickbs=x.ICKBS,
                                    Bfnhbs=x.BFNHBS,
                                    Aeibs=x.AEIBS
                                })
                                .Single();
                if (null != jwdxx)
                {
                    #region 获取aei出入段数据
                    if (jwdxx.Aeibs == "1")
                    {
                        try
                        {
                            //Bdsj bdsjAei = BdsjService.GetAeiCdRd(bdsj.Cq, bdsj.Tq, bdsj.Jx, bdsj.Jch);
                            #region Bdsj bdsjAei = BdsjService.GetAeiCdRd(bdsj.Cq, bdsj.Tq, bdsj.Jx, bdsj.Jch);
                            Bdsj bdsjAei= GetAeiCdRd(bdsj.Cq, bdsj.Tq, bdsj.Jx, bdsj.Jch);
                            #endregion
                            if (bdsjAei != null)
                            {
                                bdsj.Aei_CD = bdsjAei.Aei_CD;
                                bdsj.Aei_RD = bdsjAei.Aei_RD;
                            }
                            /*
                            #region 直拉从aei数据库获取
                            string sql = @"select max(case when CDRD=0 then to_char(to_date(substr(tgrq || tgsj,0,12),'yyyy-mm-dd hh24:mi'),'yyyy-mm-dd hh24:mi') else null end) TGRQ,
                                        max(case when CDRD=1 then to_char(to_date(substr(tgrq || tgsj,0,12),'yyyy-mm-dd hh24:mi'),'yyyy-mm-dd hh24:mi') else null end) DDRQ
                                         from dat_jcaeidetail a  JOIN dat_jcaei b ON a.PARENT_ID = b.ID where txsbjg_cx='{2}' and txsbjg_ch='{3}' and 
                                      '{0}'<to_char(to_date(substr(tgrq || tgsj,0,12),'yyyy-mm-dd hh24:mi'),'yyyy-mm-dd hh24:mi') and 
                                      '{1}'>to_char(to_date(substr(tgrq || tgsj,0,12),'yyyy-mm-dd hh24:mi'),'yyyy-mm-dd hh24:mi')";
                            JcAeiModel jcAeiModel = MySqlHelper.GetDataSetToList<JcAeiModel>(string.Format(sql, bdsj.Jiec, bdsj.Jiaoc, bdsj.Jx, bdsj.Jch)).FirstOrDefault();
                            if (null != jcAeiModel)
                            {
                                bdsj.Aei_CD = jcAeiModel.TGRQ;
                                bdsj.Aei_RD = jcAeiModel.DDRQ;
                            }
                        #endregion*/
                        }
                        catch (Exception ex) { }
                    }
                    #endregion

                    #region 获取薄飞能耗数据
                    if (jwdxx.Bfnhbs == "1" && bdsj.Jcxh != null && bdsj.Jczl == "D")
                    {
                        try
                        {
                            IRestConnection restConnection = new RestConnection("http://10.64.7.204:8101/");
                            JObject paramters = new JObject();
                            long timestamp = (long)(DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                            paramters.Add("cx", bdsj.Jcxh);
                            paramters.Add("ch", bdsj.Jch);
                            paramters.Add("param", bdsj.Jiec + ":00;" + bdsj.Jiaoc + ":00");
                            //paramters.Add("sso", DzbdJwd.Providers.StringSecurity.Sm4Encrypt("whsh;BeiJing@2023.com;" + timestamp));
                            paramters.Add("sso", StringSecurity.Sm4Encrypt("whsh;BeiJing@2023.com;" + timestamp));

                            var tempResult = restConnection.PostJson("LMD/innerApi/getAmmeterData.do", paramters);
                            // log.Info("电表返回数据：" + reslutDb.ToString());
                            //var tempResult = "YOsOGHmKLu4Xk2zc3i4i6BbT2v//HTedO6t/Zv/uP0kvsZL3IibWWkTPGIzZAmCM";
                            JObject reslutDb = JObject.Parse(StringSecurity.Sm4Decrypt(tempResult));
                            if ((reslutDb.Property("result")).Value.ToString().Equals("success"))
                            {
                                JArray jcDbList = JArray.Parse((reslutDb.Property("datas")).Value.ToString());
                                // JArray jcDbList = JArray.Parse("[{\"cc\": \"23698\",\"dn_b\": 0,\"dn_a\": 123000,\"cx\": \"235\",\"ch\": \"1\",\"sj\": \"123456\",\"fk_b\": 0,\"fk_a\": 12300,\"time\": \"2023-10-18 00:01:00\",\"fsj\": \"123457\",\"glb\": \"123.698\"}]");
                                if (jcDbList.Count == 2)
                                {
                                    if (!string.IsNullOrEmpty(jcDbList[0].Value<string>("time")))
                                    {
                                        //电能（接）
                                        bdsj.Jsl1_BF = jcDbList[0].Value<int>("dl_a");
                                        bdsj.Jsl2_BF = jcDbList[0].Value<int>("dl_b");
                                        //再生电（接）
                                        bdsj.Zsdjsl1_BF = jcDbList[0].Value<int>("fk_a");
                                        bdsj.Zsdjsl2_BF = jcDbList[0].Value<int>("fk_b");
                                    }
                                    if (!string.IsNullOrEmpty(jcDbList[1].Value<string>("time")))
                                    {
                                        //电能（交）
                                        bdsj.Jcl1_BF = jcDbList[1].Value<int>("dl_a");
                                        bdsj.Jcl2_BF = jcDbList[1].Value<int>("dl_b");
                                        //再生电（交）
                                        bdsj.Zsdjcl1_BF = jcDbList[1].Value<int>("fk_a");
                                        bdsj.Zsdjcl2_BF = jcDbList[1].Value<int>("fk_b");
                                    }
                                }
                                /*
                                foreach (var jcdb in jcDbList)
                                {
                                    if (jcdb.Value<string>("time") == bdsj.Jiec)
                                    {
                                        //电能（接）
                                        bdsj.Jsl1_BF = jcdb.Value<int>("dl_a");
                                        bdsj.Jsl2_BF = jcdb.Value<int>("dl_b");
                                        //再生电（接）
                                        bdsj.Zsdjsl1_BF = jcdb.Value<int>("fk_a");
                                        bdsj.Zsdjsl2_BF = jcdb.Value<int>("fk_b");
                                    }
                                    else if (jcdb.Value<string>("time") == bdsj.Jiaoc)
                                    {
                                        //电能（交）
                                        bdsj.Jcl1_BF = jcdb.Value<int>("dl_a");
                                        bdsj.Jcl2_BF = jcdb.Value<int>("dl_b");
                                        //再生电（交）
                                        bdsj.Zsdjcl1_BF = jcdb.Value<int>("fk_a");
                                        bdsj.Zsdjcl2_BF = jcdb.Value<int>("fk_b");
                                    }
                                }*/
                            }
                        }
                        catch (Exception ex) { }
                    }
                    #endregion
                }
            }
            catch (Exception e)
            {

            }
            //提交报单数据
            //if (BdsjService.UpdateBdsj(bdsj).success)
            if (UpdateBdsj(bdsj).success)
            {
                //BdffHub bdffhub = new BdffHub();
                //bdffhub.Send();
                //BdhsHub bdhsHub = new BdhsHub();
                var bdhsHub = new BdhsHub(_bdffHubContext);
                bdhsHub.Send();
                return new { success = true, data = "保存成功" };
            }
            else
            {
                return new { success = false, data = "保存失败" };
            }
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    private dynamic UpdateBdsj(Bdsj bdsj)
    {
        int bdid;
        try
        {
            _db.Ado.BeginTran();
            //var bdsj1 = BdsjRepository.ExecuteSqlCommand($" DELETE FROM DAT_BDSJ WHERE YT1BDSJID='{bdsj.Yt1bdsjid}'");
            var bdsj1 = _db.Deleteable<DAT_BDSJ>().Where(x => x.YT1BDSJID == bdsj.Yt1bdsjid).ExecuteCommand();

            if (string.IsNullOrEmpty(bdsj.Bdlxdm))
            {
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
            }
            else
            {
                bdsj.Qxbdbs = "4"; //便乘、看道等
            }
            if (null != bdsj.Clbdsjs && bdsj.Clbdsjs.Count > 0)
            {
                bdsj.Jch_Wrcl = bdsj.Clbdsjs[0].Jch;
                bdsj.Jx_Wrcl = bdsj.Clbdsjs[0].Jx;
            }
            //bdid = BdsjRepository.Add(bdsj);
            DAT_BDSJ modelBdsj = new DAT_BDSJ()
            {
                ID = bdsj.Id,
                SJDM1 = bdsj.Sjdm1,
                SJXM1 = bdsj.Sjxm1,
                FSJDM1 = bdsj.Fsjdm1,
                FSJXM1 = bdsj.Fsjxm1,
                XYDM1 = bdsj.Xydm1,
                XYXM1 = bdsj.Xyxm1,
                SJDM2 = bdsj.Sjdm2,
                SJXM2 = bdsj.Sjxm2,
                FSJDM2 = bdsj.Fsjdm2,
                FSJXM2 = bdsj.Fsjxm2,
                XYDM2 = bdsj.Xydm2,
                XYXM2 = bdsj.Xyxm2,
                ZCFSDM = bdsj.Zcfsdm,
                ZCFSMC = bdsj.Zcfsmc,
                JCZL = bdsj.Jczl,
                JX = bdsj.Jx,
                JCH = bdsj.Jch,
                BDRQ = bdsj.Bdrq,
                JIED = bdsj.Jied,
                CQ = bdsj.Cq,
                JIEC = bdsj.Jiec,
                JIAOC = bdsj.Jiaoc,
                TQ = bdsj.Tq,
                CBD = bdsj.Cbd,
                RWD = bdsj.Rwd,
                CWD = bdsj.Cwd,
                RBD = bdsj.Rbd,
                JSL1 = bdsj.Jsl1,
                JRL1 = bdsj.Jrl1,
                JCL1 = bdsj.Jcl1,
                ZSDJSL1 = bdsj.Zsdjsl1,
                ZSDJCL1 = bdsj.Zsdjcl1,
                LCGDJSL1 = bdsj.Lcgdjsl1,
                LCGDJCL1 = bdsj.Lcgdjcl1,
                JSL2 = bdsj.Jsl2,
                JRL2 = bdsj.Jrl2,
                JCL2 = bdsj.Jcl2,
                ZSDJSL2 = bdsj.Zsdjsl2,
                ZSDJCL2 = bdsj.Zsdjcl2,
                LCGDJSL2 = bdsj.Lcgdjsl2,
                LCGDJCL2 = bdsj.Lcgdjcl2,
                DWSJ = bdsj.Dwsj,
                DWYL = bdsj.Dwyl,
                YZMC1 = bdsj.Yzmc1,
                YZYL1 = bdsj.Yzyl1,
                YZMC2 = bdsj.Yzmc2,
                YZYL2 = bdsj.Yzyl2,
                YZMC3 = bdsj.Yzmc3,
                YZYL3 = bdsj.Yzyl3,
                YZMC4 = bdsj.Yzmc4,
                YZYL4 = bdsj.Yzyl4,
                YZMC5 = bdsj.Yzmc5,
                YZYL5 = bdsj.Yzyl5,
                YZMC6 = bdsj.Yzmc6,
                YZYL6 = bdsj.Yzyl6,
                YZMC7 = bdsj.Yzmc7,
                YZYL7 = bdsj.Yzyl7,
                QRCODE = bdsj.Qrcode,
                PROCEEDING = bdsj.Proceeding,
                CWTQDDDM = bdsj.Cwtqdddm,
                CWCQDDDM = bdsj.Cwcqdddm,
                CWJLDM = bdsj.Cwjldm,
                CWJLMC = bdsj.Cwjlmc,
                BDTJDDDM = bdsj.Bdtjdddm,
                BDHSQK = bdsj.Bdhsqk,
                SJGZDM1 = bdsj.Sjgzdm1,
                FSJGZDM1 = bdsj.Fsjgzdm1,
                XYGZDM1 = bdsj.Xygzdm1,
                SJGZDM2 = bdsj.Sjgzdm2,
                FSJGZDM2 = bdsj.Fsjgzdm2,
                XYGZDM2 = bdsj.Xygzdm2,
                SJSRDM1 = bdsj.Sjsrdm1,
                XYSRDM1 = bdsj.Xysrdm1,
                FSJSRDM1 = bdsj.Fsjsrdm1,
                SJSRDM2 = bdsj.Sjsrdm2,
                XYSRDM2 = bdsj.Xysrdm2,
                FSJSRDM2 = bdsj.Fsjsrdm2,
                CWGSDDDM = bdsj.Cwgsdddm,
                BDFFR = bdsj.Bdffr,
                BDHSR = bdsj.Bdhsr,
                FWBJ = bdsj.Fwbj,
                FWZGS = bdsj.Fwzgs,
                FWWD = bdsj.Fwwd,
                FWWJ = bdsj.Fwwj,
                ZPJDM = bdsj.Zpjdm,
                ZPJMC = bdsj.Zpjmc,
                ZPDDM = bdsj.Zpddm,
                ZPDMC = bdsj.Zpdmc,
                LRJDM = bdsj.Lrjdm,
                LRJMC = bdsj.Lrjmc,
                LRDDM = bdsj.Lrddm,
                LRDMC = bdsj.Lrdmc,
                CWYZB1 = bdsj.Cwyzb1,
                CWYZB2 = bdsj.Cwyzb2,
                CWYZB3 = bdsj.Cwyzb3,
                CWYZB4 = bdsj.Cwyzb4,
                CWYZB5 = bdsj.Cwyzb5,
                CWYZB6 = bdsj.Cwyzb6,
                BDXGR = bdsj.Bdxgr,
                BDFFSJ = bdsj.Bdffsj,
                BDTJSJ = bdsj.Bdtjsj,
                BDHSSJ = bdsj.Bdhssj,
                BDXGSJ = bdsj.Bdxgsj,
                JKJLDWDM = bdsj.Jkjldwdm,
                SFSJJLH = bdsj.Sfsjjlh,
                SFSRJLH = bdsj.Sfsrjlh,
                SFCZH = bdsj.Sfczh,
                SFCZMC = bdsj.Sfczmc,
                SFCC = bdsj.Sfcc,
                ZDSJJLH = bdsj.Zdsjjlh,
                ZDSRJLH = bdsj.Zdsrjlh,
                ZDCZH = bdsj.Zdczh,
                ZDCZMC = bdsj.Zdczmc,
                ZDCC = bdsj.Zdcc,
                TJZT = bdsj.Tjzt,
                SJSJ = bdsj.Sjsj,
                TJSJ = bdsj.Tjsj,
                TJZH = bdsj.Tjzh,
                YT1BDSJID = bdsj.Yt1bdsjid,
                BDZFSJ = bdsj.Bdzfsj,
                BDZFR = bdsj.Bdzfr,
                LCGDJSL1_2 = (bdsj.Lcgdjsl1_2 == null ? null : bdsj.Lcgdjsl1_2.ToString()),
                LCGDJCL1_2 = (bdsj.Lcgdjcl1_2 == null ? null : bdsj.Lcgdjcl1_2.ToString()),
                LCGDJSL2_2 = (bdsj.Lcgdjsl2_2 == null ? null : bdsj.Lcgdjsl2_2.ToString()),
                LCGDJCL2_2 = (bdsj.Lcgdjcl2_2 == null ? null : bdsj.Lcgdjcl2_2.ToString()),
                EVENTTIME = bdsj.Eventtime,
                BZXH = bdsj.Bzxh,
                NHKK = bdsj.Nhkk,
                RLBL = bdsj.Rlbl,
                SYFS = bdsj.Syfs,
                NHSFYC = bdsj.Nhsfyc,
                JCHSFYC = bdsj.Jchsfyc,
                JX_WRCL = bdsj.Jx_Wrcl,
                JCH_WRCL = bdsj.Jch_Wrcl,
                SFYLR = bdsj.Sfylr,
                CJDM = bdsj.Cjdm,
                GZFS = bdsj.Gzfs,
                BDHTSJ = bdsj.Bdhtsj,
                BDHTR = bdsj.Bdhtr,
                QXBDBS = bdsj.Qxbdbs,
                BDLXDM = bdsj.Bdlxdm,
                BDLXMC = bdsj.Bdlxmc,
                ZZBDBH = bdsj.Zzbdbh,
                CDDM = bdsj.Cddm,
                BDTJLY = bdsj.Bdtjly,
                SDZK = bdsj.Sdzk,
                DBSFSH1 = bdsj.Dbsfsh1,
                DBSFSH2 = bdsj.Dbsfsh2,
                JCXH = bdsj.Jcxh,
                ZSDJCL2_BF = bdsj.Zsdjcl2_BF,
                ZSDJSL2_BF = bdsj.Zsdjsl2_BF,
                JCL2_BF = bdsj.Jcl2_BF,
                JSL2_BF = bdsj.Jsl2_BF,
                ZSDJCL1_BF = bdsj.Zsdjcl1_BF,
                ZSDJSL1_BF = bdsj.Zsdjsl1_BF,
                JCL1_BF = bdsj.Jcl1_BF,
                JSL1_BF = bdsj.Jsl1_BF,
                AEI_RD = bdsj.Aei_RD,
                AEI_CD = bdsj.Aei_CD
            };
            if (_db.Insertable<DAT_BDSJ>(modelBdsj).ExecuteCommand() > 0)
            {
                bdid = _db.Queryable<DAT_BDSJ>().Max(x => x.ID);
            }
            else
                bdid = 0;
            if (bdsj.Bzsjs != null)
            {
                foreach (Bzsj bzsj in bdsj.Bzsjs)
                {
                    bzsj.Bdsjid = bdid;
                    bzsj.Id = 0;
                    //int bzid = BzsjService.Add(bzsj);
                    _db.Insertable<DAT_BZSJ>(bzsj).ExecuteCommand();
                    int bzid=_db.Queryable<DAT_BZSJ>().Max(x => x.ID);
                    if (bzsj.Zbzsjs != null)
                    {
                        foreach (Zbzsj zbzsj in bzsj.Zbzsjs)
                        {
                            zbzsj.Bzsjid = bzid;
                            zbzsj.Id = 0;
                            //ZbzsjService.Add(zbzsj);
                            _db.Insertable(zbzsj).ExecuteCommand();
                        }
                    }
                }
            }
            if (null != bdsj.Clbdsjs && bdsj.Clbdsjs.Count > 0)
            {
                List<DAT_CLBDSJ> lstAdd = new List<DAT_CLBDSJ>();
                foreach (Clbdsj clbdsj in bdsj.Clbdsjs)
                {
                    //clbdsj.Bdsjid = bdid;
                    //ClbdsjService.Add(clbdsj);
                    lstAdd.Add(new DAT_CLBDSJ()
                    {
                        ID = clbdsj.Id,
                        BDSJID = clbdsj.Bdsjid == null ? 0 : (decimal)clbdsj.Bdsjid,
                        JX = clbdsj.Jx,
                        JCH = clbdsj.Jch,
                        JSL1 = clbdsj.Jsl1,
                        JRL1 = clbdsj.Jrl1,
                        JCL1 = clbdsj.Jcl1,
                        ZSDJSL1 = clbdsj.Zsdjsl1,
                        ZSDJCL1 = clbdsj.Zsdjcl1,
                        LCGDJSL1 = clbdsj.Lcgdjsl1,
                        LCGDJCL1 = clbdsj.Lcgdjcl1,
                        JSL2 = clbdsj.Jsl2,
                        JRL2 = clbdsj.Jrl2,
                        JCL2 = clbdsj.Jcl2,
                        ZSDJSL2 = clbdsj.Zsdjsl2,
                        ZSDJCL2 = clbdsj.Zsdjcl2,
                        LCGDJSL2 = clbdsj.Lcgdjsl2,
                        LCGDJCL2 = clbdsj.Lcgdjcl2,
                        DWSJ = clbdsj.Dwsj,
                        DWYL = clbdsj.Dwyl,
                        LCGDJSL1_2 = clbdsj.Lcgdjsl1_2 == null ? null : clbdsj.Lcgdjsl1_2.ToString(),
                        LCGDJCL1_2 = clbdsj.Lcgdjcl1_2 == null ? null : clbdsj.Lcgdjcl1_2.ToString(),
                        LCGDJSL2_2 = clbdsj.Lcgdjsl1_2 == null ? null : clbdsj.Lcgdjsl2_2.ToString(),
                        LCGDJCL2_2 = clbdsj.Lcgdjcl1_2 == null ? null : clbdsj.Lcgdjcl2_2.ToString(),
                        DBSFSH1 = clbdsj.Dbsfsh1,
                        DBSFSH2 = clbdsj.Dbsfsh2
                    });
                    _db.Insertable(lstAdd).ExecuteCommand();
                }
            }
            //Commit();
            _db.Ado.CommitTran();
            return new { success = true, data = 1 };
        }
        catch (Exception e)
        {
            _db.Ado.RollbackTran();
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    /// <summary>
    /// 获取报单aei出段和入库数据
    /// </summary>
    /// <param name="cq"></param>
    /// <param name="tq"></param>
    /// <param name="jx"></param>
    /// <param name="jch"></param>
    /// <returns></returns>
    private Bdsj GetAeiCdRd(string cq, string tq, string jx, string jch)
    {
        string sql = @"select max(case when CDRD=0 then to_char(to_date(substr(tgrq || tgsj,0,12),'yyyy-mm-dd hh24:mi'),'yyyy-mm-dd hh24:mi') else null end) AEI_CD,
            max(case when CDRD=1 then to_char(to_date(substr(tgrq || tgsj,0,12),'yyyy-mm-dd hh24:mi'),'yyyy-mm-dd hh24:mi') else null end) AEI_RD
             from dat_jcaeidetail a  JOIN dat_jcaei b ON a.PARENT_ID = b.ID where txsbjg_cx='{2}' and txsbjg_ch='{3}' and 
          '{0}'<to_char(to_date(substr(tgrq || tgsj,0,12),'yyyy-mm-dd hh24:mi'),'yyyy-mm-dd hh24:mi') and 
          '{1}'>to_char(to_date(substr(tgrq || tgsj,0,12),'yyyy-mm-dd hh24:mi'),'yyyy-mm-dd hh24:mi')";
        //return BdsjRepository.FindBdsjModes(string.Format(sql, cq, tq, jx, jch));
        return _db.Ado.SqlQuery<Bdsj>(string.Format(sql, cq, tq, jx, jch)).SingleOrDefault(); ;
    }

    /// <summary>
    /// 根据乘务员工号返回空报单数据
    /// </summary>
    /// <param name="jo"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private IList<Bdsj> EmptyBdsj(JObject jo)
    {
        DateTime dt;
        #region 1.获取未生成电子报单的数据
        try
        {
            if (jo.ContainsKey("username") && !string.IsNullOrEmpty(jo["username"].ToString()))
            {
                //List<BdsjYt1> list = new List<BdsjYt1>();
                //PredicateGroup pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
                //pg.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.Bdrq, Operator.Ge, DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd")));

                //var pgSjOr = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
                //pgSjOr.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.Sjdm1, Operator.Eq, jo["username"].ToString()));
                //pgSjOr.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.Fsjdm1, Operator.Eq, jo["username"].ToString()));
                //pgSjOr.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.Xydm1, Operator.Eq, jo["username"].ToString()));
                //pgSjOr.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.Sjdm2, Operator.Eq, jo["username"].ToString()));
                //pgSjOr.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.Fsjdm2, Operator.Eq, jo["username"].ToString()));
                //pgSjOr.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.Xydm2, Operator.Eq, jo["username"].ToString()));
                //pg.Predicates.Add(pgSjOr);

                //pg.Predicates.Add(Predicates.Field<BdsjYt1>(p => p.Proceeding, Operator.Eq, 0));

                //IList<ISort> sor = new List<ISort>();
                //sor.Add(new Sort { PropertyName = "Cq", Ascending = false });

                //list = BdsjYt1Service.FindAll(pg, sor).ToList();
                //list = _dap.Context.GetList<BdsjYt1>(pg,sor).ToList();
                List<YT1_DAT_BDSJ> list = new List<YT1_DAT_BDSJ>();
                dt = DateTime.Now.AddDays(-10).Date;
                string userName = jo["username"].ToString();
                //BdsjYt1
                var query = _db.Queryable<YT1_DAT_BDSJ>()
                                .Where(x => !SqlFunc.IsNullOrEmpty(x.BDRQ)
                                            && SqlFunc.ToDate(x.BDRQ) >= dt
                                            && (
                                                x.SJDM1 == userName
                                                || x.FSJDM1 == userName
                                                || x.XYDM1 == userName
                                                || x.SJDM2 == userName
                                                || x.FSJDM2 == userName
                                                || x.XYDM2 == userName
                                            ))
                                .Select(x => new YT1_DAT_BDSJ()
                                {
                                }, true);
                list = query.ToList();
                BdsjYt1 model = new BdsjYt1();
                for (int i = 0; i < list.Count; i++)
                {
                    //2024-01-05运统1将不再赋退勤时间
                    //当前退勤的报单才赋退勤时间
                    //InsertBdsj(list[i]);
                    model.Id = (int)list[i].ID;
                    model.Bdffr = list[i].BDFFR;
                    model.Bdffsj = list[i].BDFFSJ;
                    model.Bdhsqk = (int?)list[i].BDHSQK;
                    model.Bdhsr = list[i].BDHSR;
                    model.Bdhssj = list[i].BDHSSJ;
                    model.Bdhtr = list[i].BDHTR;
                    model.Bdhtsj = list[i].BDHTSJ;
                    model.Bdrq = list[i].BDRQ;
                    model.Bdtjdddm = list[i].BDTJDDDM;
                    model.Bdtjsj = list[i].BDTJSJ;
                    model.Bdxgr = list[i].BDXGR;
                    model.Bdxgsj = list[i].BDXGSJ;
                    model.Bdzfr = list[i].BDZFR;
                    model.Bdzfsj = list[i].BDZFSJ;
                    model.Cbd = list[i].CBD;
                    model.Cjdm = list[i].CJDM;
                    model.Cq = list[i].CQ;
                    model.Cwcqdddm = list[i].CWCQDDDM;
                    model.Cwd = list[i].CWD;
                    model.Cwgsdddm = list[i].CWGSDDDM;
                    model.Cwjldm = list[i].CWJLDM;
                    model.Cwjlmc = list[i].CWJLMC;
                    model.Cwtqdddm = list[i].CWTQDDDM;
                    model.Cwyzb1 = list[i].CWYZB1;
                    model.Cwyzb2 = list[i].CWYZB2;
                    model.Cwyzb3 = list[i].CWYZB3;
                    model.Cwyzb4 = list[i].CWYZB4;
                    model.Cwyzb5 = list[i].CWYZB5;
                    model.Cwyzb6 = list[i].CWYZB6;
                    model.Dwsj = list[i].DWSJ;
                    model.Dwyl = (int?)list[i].DWYL;
                    model.Eventtime = list[i].EVENTTIME;
                    model.Fsjdm1 = list[i].FSJDM1;
                    model.Fsjdm2 = list[i].FSJDM2;
                    model.Fsjgzdm1 = list[i].FSJDM1;
                    model.Fsjgzdm2 = list[i].FSJGZDM2;
                    model.Fsjsrdm1 = list[i].FSJSRDM1;
                    model.Fsjsrdm2 = list[i].FSJSRDM2;
                    model.Fsjxm1 = list[i].FSJXM1;
                    model.Fsjxm2 = list[i].FSJXM2;
                    model.Proceeding = (int)list[i].PROCEEDING;
                    model.Psd = list[i].PSD;
                    model.Psj = list[i].PSJ;
                    model.Fwbj = (int?)list[i].FWBJ;
                    model.Fwwd = (int?)list[i].FWWD;
                    model.Fwwj = (int?)list[i].FWWJ;
                    model.Fwzgs = (int?)list[i].FWZGS;
                    model.Gzfs = list[i].GZFS;
                    model.Jch = list[i].JCH;
                    model.Jcl1 = (int?)list[i].JCL1;
                    model.Jcl2 = (int?)list[i].JCL2;
                    model.Jczl = list[i].JCZL;
                    model.Jiaoc = list[i].JIAOC;
                    model.Jiec = list[i].JIEC;
                    model.Jied = list[i].JIED;
                    model.Jkjldwdm = list[i].JKJLDWDM;
                    model.Jrl1 = (int?)list[i].JRL1;
                    model.Jrl2 = (int?)list[i].JRL2;
                    model.Jsl1 = (int?)list[i].JSL1;
                    model.Jsl2 = (int?)list[i].JSL2;
                    model.Jx = list[i].JX;
                    model.SHSJ = list[i].SHSJ;
                    model.Sfcc = list[i].SFCC;
                    model.Sfczh = list[i].SFCZH;
                    model.Sfczmc = list[i].SFCZMC;
                    model.Sfsjjlh = list[i].SFSJJLH;
                    model.Sfsrjlh = list[i].SFSRJLH;
                    model.SHR = list[i].SHR;
                    model.SHZT = list[i].SHZT;
                    model.Sjdm1 = list[i].SJDM1;
                    model.Sjdm2 = list[i].SJDM2;
                    model.Sjgzdm1 = list[i].SJGZDM1;
                    model.Sjgzdm2 = list[i].SJGZDM2;
                    model.Sjsj = list[i].SJSJ;
                    model.Sjsrdm1 = list[i].SJSRDM1;
                    model.Sjsrdm2 = list[i].SJSRDM2;
                    model.Sjxm1 = list[i].SJXM1;
                    model.Sjxm2 = list[i].SJXM2;
                    model.Lcgdjcl1 = (int?)list[i].LCGDJCL1;
                    model.Lcgdjcl1_2 = list[i].LCGDJCL1_2 == null ? null : int.Parse(list[i].LCGDJCL1_2);
                    model.Lcgdjcl2 = (int?)list[i].LCGDJCL2;
                    model.Lcgdjcl2_2 = list[i].LCGDJCL2_2 == null ? null : int.Parse(list[i].LCGDJCL2_2);
                    model.Lcgdjsl1 = (int?)list[i].LCGDJSL1;
                    model.Lcgdjsl2 = (int?)list[i].LCGDJSL2;
                    model.Lcgdjsl1_2 = list[i].LCGDJSL1_2 == null ? null : int.Parse(list[i].LCGDJSL1_2);
                    model.Lcgdjsl2_2 = list[i].LCGDJSL2_2 == null ? null : int.Parse(list[i].LCGDJSL2_2);
                    model.Lrddm = list[i].LRDDM;
                    model.Lrdmc = list[i].LRDMC;
                    model.Lrjdm = list[i].LRJDM;
                    model.Lrjmc = list[i].LRJMC;
                    model.Qrcode = list[i].QRCODE;
                    model.Rbd = list[i].RBD;
                    model.Rwd = list[i].RWD;
                    model.Tjsj = list[i].TJSJ;
                    model.Tjzh = list[i].TJZH;
                    model.Tjzt = list[i].TJZT;
                    model.Tq = list[i].TQ;
                    model.Xydm1 = list[i].XYDM1;
                    model.Xydm2 = list[i].XYDM2;
                    model.Xygzdm1 = list[i].XYGZDM1;
                    model.Xygzdm2 = list[i].XYGZDM2;
                    model.Xysrdm1 = list[i].XYSRDM1;
                    model.Xysrdm2 = list[i].XYSRDM2;
                    model.Xyxm1 = list[i].XYXM1;
                    model.Xyxm2 = list[i].XYXM2;
                    model.Yt1bdsjid = list[i].YT1BDSJID;
                    model.Yzmc1 = list[i].YZMC1;
                    model.Yzmc2 = list[i].YZMC2;
                    model.Yzmc3 = list[i].YZMC3;
                    model.Yzmc4 = list[i].YZMC4;
                    model.Yzmc5 = list[i].YZMC5;
                    model.Yzmc6 = list[i].YZMC6;
                    model.Yzmc7 = list[i].YZMC7;
                    model.Yzyl1 = list[i].YZYL1;
                    model.Yzyl2 = list[i].YZYL2;
                    model.Yzyl3 = list[i].YZYL3;
                    model.Yzyl4 = list[i].YZYL4;
                    model.Yzyl5 = list[i].YZYL5;
                    model.Yzyl6 = list[i].YZYL6;
                    model.Yzyl7 = list[i].YZYL7;
                    model.Zcfsdm = list[i].ZCFSDM;
                    model.Zcfsmc = list[i].ZCFSMC;
                    model.Zdcc = list[i].ZDCC;
                    model.Zdczh = list[i].ZDCZH;
                    model.Zdczmc = list[i].ZDCZMC;
                    model.Zdsjjlh = list[i].ZDSJJLH;
                    model.Zdsrjlh = list[i].ZDSRJLH;
                    model.Zpddm = list[i].ZPDDM;
                    model.Zpdmc = list[i].ZPDMC;
                    model.Zpjdm = list[i].ZPJDM;
                    model.Zpjmc = list[i].ZPJMC;
                    model.Zsdjcl1 = (int?)list[i].ZSDJCL1;
                    model.Zsdjcl2 = (int?)list[i].ZSDJCL2;
                    model.Zsdjsl1 = (int?)list[i].ZSDJSL1;
                    model.Zsdjsl2 = (int?)list[i].ZSDJSL2;
                    InsertBdsj(model);
                }
            }
        }
        catch (Exception ex)
        {
        }
        #endregion
        #region 3.获取所有报单列表
        //string sqlText = string.Format(@"select (case when j.jch is null then 1 else 0 end) IsWjch,b.*  from dat_bdsj b left join dic_jch j on b.jch=j.jch and b.jx=j.jx  where bdrq>=to_char(sysdate-10,'yyyy-mm-dd')");
        //var dyparam = new DynamicParameters();
        dt = DateTime.Now.AddDays(-10).Date;
        var query1 = _db.Queryable<Bdsj>()
                        .LeftJoin<Jchxx>((b, j) => b.Jch == j.Jch && b.Jx == j.Jx)
                        .Where((b, j) => SqlFunc.ToDate(b.Bdrq) > dt);
        string par = "";
        if (!string.IsNullOrEmpty(jo["username"].ToString()))
        {
            //sqlText += " and (sjdm1=:Sjdm1"; //司机1
            //sqlText += " or fsjdm1=:Fsjdm1"; //副司机1
            //sqlText += " or xydm1=:Xydm1"; //学员1
            //sqlText += " or sjdm2=:Sjdm2"; //司机2
            //sqlText += " or fsjdm2=:Fsjdm2"; //副司机2
            //sqlText += " or xydm2=:Xydm2)"; //学员2
            //dyparam.Add("Sjdm1", value: jo["username"].ToString());
            //dyparam.Add("Fsjdm1", value: jo["username"].ToString());
            //dyparam.Add("Xydm1", value: jo["username"].ToString());
            //dyparam.Add("Sjdm2", value: jo["username"].ToString());
            //dyparam.Add("Fsjdm2", value: jo["username"].ToString());
            //dyparam.Add("Xydm2", value: jo["username"].ToString());
            par = jo["username"].ToString();
            query1 = query1.Where((b, j) => (b.Sjdm1 == par
                                            || b.Fsjdm1 == par
                                            || b.Xydm1 == par
                                            || b.Sjdm2 == par
                                            || b.Fsjdm2 == par
                                            || b.Xydm2 == par));
        }
        else if (!string.IsNullOrEmpty(jo["qrcode"].ToString()))
        {
            //sqlText += "and qrcode=:Qrcode";
            //dyparam.Add("Qrcode", value: jo["qrcode"].ToString());
            par = jo["qrcode"].ToString();
            query1 = query1.Where((b, j) => b.Qrcode == par);
        }
        //sqlText += "order by cq desc";
        #endregion
        //return BdsjRepository.FindByCondition(sqlText, dyparam);
        return query1.OrderByDescending((b, j) => b.Cq)
                        .Select((b, j) => new Bdsj()
                        {
                            IsWjch = SqlFunc.IIF(SqlFunc.IsNullOrEmpty(j.Jch), "1", "0"),
                        }, true).ToList();

    }

    /// <summary>
    /// 运统1报单结合LKJ数据生成电子报单,添加入库
    /// </summary>
    /// <param name="bdsjYt1"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void InsertBdsj(BdsjYt1 bdsjYt1)
    {
        try
        {
            //将未生成电子报单的记录，保存到电子报单中，只保存报单头信息
            #region MyRegion
            //Bdsj bdsj = new Bdsj();
            //Mapper.CreateMap<BdsjYt1, Bdsj>().ForAllMembers(opt => opt.Condition(srs => srs.MemberName != "Id"));
            //Mapper.DynamicMap<BdsjYt1, Bdsj>(bdsjYt1, bdsj);
            _db.Ado.BeginTran();
            Bdsj bdsj = new Bdsj()
            {
                Bdffr = bdsjYt1.Bdffr,
                Bdffsj = bdsjYt1.Bdffsj,
                Bdhsqk = bdsjYt1.Bdhsqk,
                Bdhsr = bdsjYt1.Bdhsr,
                Bdhssj = bdsjYt1.Bdhssj,
                Bdhtr = bdsjYt1.Bdhtr,
                Bdhtsj = bdsjYt1.Bdhtsj,
                Bdrq = bdsjYt1.Bdrq,
                Bdtjdddm = bdsjYt1.Bdtjdddm,
                Bdtjsj = bdsjYt1.Bdtjsj,
                Bdxgr = bdsjYt1.Bdxgr,
                Bdxgsj = bdsjYt1.Bdxgsj,
                Bdzfr = bdsjYt1.Bdzfr,
                Bdzfsj = bdsjYt1.Bdzfsj,
                Bzsjs = new List<Bzsj>(),
                Cbd = bdsjYt1.Cbd,
                Cjdm = bdsjYt1.Cjdm,
                Clbdsjs = new List<Clbdsj>(),
                Cq = bdsjYt1.Cq,
                Cwcqdddm = bdsjYt1.Cwcqdddm,
                Cwd = bdsjYt1.Cwd,
                Cwgsdddm = bdsjYt1.Cwgsdddm,
                Cwjldm = bdsjYt1.Cwjldm,
                Cwjlmc = bdsjYt1.Cwjlmc,
                Cwtqdddm = bdsjYt1.Cwtqdddm,
                Cwyzb1 = bdsjYt1.Cwyzb1,
                Cwyzb2 = bdsjYt1.Cwyzb2,
                Cwyzb3 = bdsjYt1.Cwyzb3,
                Cwyzb4 = bdsjYt1.Cwyzb4,
                Cwyzb5 = bdsjYt1.Cwyzb5,
                Cwyzb6 = bdsjYt1.Cwyzb6,
                Dwsj = bdsjYt1.Dwsj,
                Dwyl = bdsjYt1.Dwyl,
                Eventtime = bdsjYt1.Eventtime,
                Fsjdm1 = bdsjYt1.Fsjdm1,
                Fsjdm2 = bdsjYt1.Fsjdm2,
                Fsjgzdm1 = bdsjYt1.Fsjgzdm1,
                Fsjgzdm2 = bdsjYt1.Fsjgzdm2,
                Fsjsrdm1 = bdsjYt1.Fsjsrdm1,
                Fsjsrdm2 = bdsjYt1.Fsjsrdm2,
                Fsjxm1 = bdsjYt1.Fsjxm1,
                Fsjxm2 = bdsjYt1.Fsjxm2,
                Fwbj = bdsjYt1.Fwbj,
                Fwwd = bdsjYt1.Fwwd,
                Fwwj = bdsjYt1.Fwwj,
                Fwzgs = bdsjYt1.Fwzgs,
                Gzfs = bdsjYt1.Gzfs,
                Id = bdsjYt1.Id,
                Jch = bdsjYt1.Jch,
                Jcl1 = bdsjYt1.Jcl1,
                Jcl2 = bdsjYt1.Jcl2,
                Jczl = bdsjYt1.Jczl,
                Jiaoc = bdsjYt1.Jiaoc,
                Jiec = bdsjYt1.Jiec,
                Jied = bdsjYt1.Jied,
                Jkjldwdm = bdsjYt1.Jkjldwdm,
                Jrl1 = bdsjYt1.Jrl1,
                Jrl2 = bdsjYt1.Jrl2,
                Jsl1 = bdsjYt1.Jsl1,
                Jsl2 = bdsjYt1.Jsl2,
                Jx = bdsjYt1.Jx,
                Lcgdjcl1 = bdsjYt1.Lcgdjcl1,
                Lcgdjcl1_2 = bdsjYt1.Lcgdjcl1_2,
                Lcgdjcl2 = bdsjYt1.Lcgdjcl2,
                Lcgdjcl2_2 = bdsjYt1.Lcgdjcl2_2,
                Lcgdjsl1 = bdsjYt1.Lcgdjsl1,
                Lcgdjsl1_2 = bdsjYt1.Lcgdjsl1_2,
                Lcgdjsl2 = bdsjYt1.Lcgdjsl2,
                Lcgdjsl2_2 = bdsjYt1.Lcgdjsl2_2,
                Lrddm = bdsjYt1.Lrddm,
                Lrdmc = bdsjYt1.Lrdmc,
                Lrjdm = bdsjYt1.Lrjdm,
                Lrjmc = bdsjYt1.Lrjmc,
                Proceeding = bdsjYt1.Proceeding,
                Qrcode = bdsjYt1.Qrcode,
                Rbd = bdsjYt1.Rbd,
                Rwd = bdsjYt1.Rwd,
                Sfcc = bdsjYt1.Sfcc,
                Sfczh = bdsjYt1.Sfczh,
                Sfczmc = bdsjYt1.Sfczmc,
                Sfsjjlh = bdsjYt1.Sfsjjlh,
                Sfsrjlh = bdsjYt1.Sfsrjlh,
                Sjdm1 = bdsjYt1.Sjdm1,
                Sjdm2 = bdsjYt1.Sjdm2,
                Sjgzdm1 = bdsjYt1.Sjgzdm1,
                Sjgzdm2 = bdsjYt1.Sjgzdm2,
                Sjsj = bdsjYt1.Sjsj,
                Sjsrdm1 = bdsjYt1.Sjsrdm1,
                Sjsrdm2 = bdsjYt1.Sjsrdm2,
                Sjxm1 = bdsjYt1.Sjxm1,
                Sjxm2 = bdsjYt1.Sjxm2,
                Tjsj = bdsjYt1.Tjsj,
                Tjzh = bdsjYt1.Tjzh,
                Tjzt = bdsjYt1.Tjzt,
                Tq = bdsjYt1.Tq,
                Xydm1 = bdsjYt1.Xydm1,
                Xydm2 = bdsjYt1.Xydm2,
                Xygzdm1 = bdsjYt1.Xygzdm1,
                Xygzdm2 = bdsjYt1.Xygzdm2,
                Xysrdm1 = bdsjYt1.Xysrdm1,
                Xysrdm2 = bdsjYt1.Xysrdm2,
                Xyxm1 = bdsjYt1.Xyxm1,
                Xyxm2 = bdsjYt1.Xyxm2,
                Yt1bdsjid = bdsjYt1.Yt1bdsjid,
                Yzmc1 = bdsjYt1.Yzmc1,
                Yzmc2 = bdsjYt1.Yzmc2,
                Yzmc3 = bdsjYt1.Yzmc3,
                Yzmc4 = bdsjYt1.Yzmc4,
                Yzmc5 = bdsjYt1.Yzmc5,
                Yzmc6 = bdsjYt1.Yzmc6,
                Yzmc7 = bdsjYt1.Yzmc7,
                Yzyl1 = bdsjYt1.Yzyl1,
                Yzyl2 = bdsjYt1.Yzyl2,
                Yzyl3 = bdsjYt1.Yzyl3,
                Yzyl4 = bdsjYt1.Yzyl4,
                Yzyl5 = bdsjYt1.Yzyl5,
                Yzyl6 = bdsjYt1.Yzyl6,
                Yzyl7 = bdsjYt1.Yzyl7,
                Zcfsdm = bdsjYt1.Zcfsdm,
                Zcfsmc = bdsjYt1.Zcfsmc,
                Zdcc = bdsjYt1.Zdcc,
                Zdczh = bdsjYt1.Zdczh,
                Zdczmc = bdsjYt1.Zdczmc,
                Zdsjjlh = bdsjYt1.Zdsjjlh,
                Zdsrjlh = bdsjYt1.Zdsrjlh,
                Zpddm = bdsjYt1.Zpddm,
                Zpdmc = bdsjYt1.Zpdmc,
                Zpjdm = bdsjYt1.Zpjdm,
                Zpjmc = bdsjYt1.Zpjmc,
                Zsdjcl1 = bdsjYt1.Zsdjcl1,
                Zsdjcl2 = bdsjYt1.Zsdjcl2,
                Zsdjsl1 = bdsjYt1.Zsdjsl1,
                Zsdjsl2 = bdsjYt1.Zsdjsl2,
            };
            #endregion

            //删除报单头部信息和报单编组信息
            //BdsjRepository.ExecuteSqlCommand($" DELETE FROM DAT_BDSJ WHERE YT1BDSJID='{bdsj.Yt1bdsjid}'");
            _db.Deleteable<DAT_BDSJ>().Where(x => x.YT1BDSJID == bdsj.Yt1bdsjid).ExecuteCommand();

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

            bdsj.Proceeding = 1;  //发放状态，需要走终端程序
            bdsj.Bdtjsj = bdsjYt1.Eventtime;  //将运统1更新时间赋给乘务员提交报单时间上
            //电表是否损坏
            bdsj.Dbsfsh1 = "否";
            bdsj.Dbsfsh2 = "否";
            //新增报单数据
            //var bid = BdsjRepository.Add(bdsj);
            _db.Insertable<DAT_BDSJ>(bdsj);

            #region 更新运统一数据状态
            //bdsjYt1.Proceeding = 4;
            //BdsjYt1Repository.Update(bdsjYt1);
            _db.Updateable<YT1_DAT_BDSJ>()
                .SetColumns(x => new YT1_DAT_BDSJ() { PROCEEDING = 4 })
                .Where(x => x.ID == bdsjYt1.Id)
                .ExecuteCommand();
            #endregion

            //Commit();
            _db.Ado.CommitTran();
        }
        catch (Exception e)
        {
            //Rollback();
            _db.Ado.RollbackTran();
        }
    }

    /// <summary>
    /// 无出勤信息的报单数据
    /// </summary>
    /// <param name="bdsj"></param>
    /// <param name="cwydm"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void WcqKbd(Bdsj bdsj, string cwydm)
    {
        GetZpdAndGsd(bdsj, cwydm); //得到支配段、归属地等数据
    }
    /// <summary>
    /// 得到支配段、归属地等数据
    /// </summary>
    /// <param name="bdsj"></param>
    /// <param name="cwydm"></param>
    private void GetZpdAndGsd(Bdsj bdsj, string cwydm)
    {
        Dictionary<string, string> paramters = new Dictionary<string, string>();
        paramters.Add("param", cwydm);
        string cwgsdddm1 = "";
        //Jwdxx jwdxx = CwyxxService.FindJwdByCwydm(cwydm, ref cwgsdddm1);
        Jwdxx jwdxx= FindJwdByCwydm(cwydm, ref cwgsdddm1);
        if (jwdxx != null)
        {
            bdsj.Zpjdm = jwdxx.Tljdm;
            bdsj.Zpddm = jwdxx.Jwddm;
            bdsj.Zpdmc = jwdxx.Jwdmc;
            //IPredicate predicate = Predicates.Field<Tljxx>(p => p.Tljdm, Operator.Eq, bdsj.Zpjdm);
            //bdsj.Zpjmc = TljxxService.Find(predicate).Tljmc;
            Tljxx modelTljxx = _db.Queryable<Tljxx>().Where(x => x.Tljdm == bdsj.Zpjdm).Single();
            bdsj.Zpjmc=modelTljxx==null?null:modelTljxx.Tljmc;
            bdsj.Cwgsdddm = cwgsdddm1;//乘务员归属地点代码
        }
    }
    /// <summary>
    /// 根据乘务员代码查询机务段信息
    /// </summary>
    /// <param name="param"></param>
    /// <param name="cwgsdddm"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private Jwdxx FindJwdByCwydm(string param, ref string cwgsdddm)
    {
        //string jwbm = XtcsService.FindByCsdm("jwbm");  //本机务表码
        string jwbm = "";

        Jwdxx jwdxx = new Jwdxx();
        if (param.Substring(0, 2) != jwbm)
        {
            jwbm = param.Substring(0, 2);//如果乘务员不属于本机务段，则乘务员代码前面两位来确定机务表码
        }
        else
        {
            //如果是本机务段，则获取乘务员归属地信息,否则为空
            //var predList = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
            //IPredicate predicate = Predicates.Field<Cwyxx>(p => p.Cwydm, Operator.Eq, param);
            //IPredicate predicate1 = Predicates.Field<Cwyxx>(p => p.Gzdm, Operator.Eq, param);
            //predList.Predicates.Add(predicate);
            //predList.Predicates.Add(predicate1);
            //Cwyxx cwyxx = Find(predList);
            #region Cwyxx cwyxx = Find(predList);
            var queryCwyxx = _db.Queryable<Cwyxx>()
                                .Where(x => x.Cwydm == param
                                        && x.Gzdm == param);
            Cwyxx cwyxx = queryCwyxx.Single();
            #endregion
            if (cwyxx != null)
            {
                cwgsdddm = cwyxx.Cwzydddm;
            }
        }
        //获取机务段信息
        //IPredicate jwdpredicate = Predicates.Field<Jwdxx>(p => p.Jwbm, Operator.Eq, jwbm);
        //jwdxx = JwdxxService.Find(jwdpredicate);
        jwdxx = _db.Queryable<Jwdxx>().Where(x => x.Jwbm == jwbm).Single();
        return jwdxx;
    }


    private Bdsj GetBdsjByCtqService(string param)
    {
        JObject jo = JObject.Parse(param);
        string cq = jo.Value<string>("Cq");
        string tq = jo.Value<string>("Tq");
        string Cwydm = jo.Value<string>("Cwydm");
        string QrCode = jo.Value<string>("QrCode");
        if (!string.IsNullOrEmpty(QrCode))
        {
            QrCode = " and qrcode<> '" + QrCode + "'";
        }

        string sqlText = @"select t.* from dat_bdsj t where t.proceeding<>9 and (sjdm1={2} or fsjdm1={2} or xydm1={2} or sjdm2={2} or fsjdm2={2} or xydm2={2}) {3}  and 
                            ((t.cq is not null and t.cq>='{0}' and  t.cq <'{1}') OR 
                            (t.tq is not null and t.tq>'{0}' and  t.tq <='{1}') OR
                            (t.cq>'{0}' and t.tq<'{1}')) order by t.cq desc";
        //return BdsjRepository.FindBdsjModes(string.Format(sqlText, cq, tq, Cwydm, QrCode));
        /**
         * 刚刚用LINQ做了测试，生成的SQL执行效率不如原生SQL
         * 由于bdsj实体中有很多自定义属性，目前查询有很多找不到，同时有些数据类型和数据库中的不同，但是很多地方又有使用，所以暂时先增加一个DAT_BDSJ数据表
         * */
        DAT_BDSJ model = _db.Ado.SqlQuery<DAT_BDSJ>(string.Format(sqlText, cq, tq, Cwydm, QrCode)).SingleOrDefault();
        Bdsj bdsj = null;
        if (model != null)
        {
            bdsj = new Bdsj()
            {
                Id = (int)model.ID,
                Sjdm1 = model.SJDM1,
                Sjxm1 = model.SJXM1,
                Fsjdm1 = model.FSJDM1,
                Fsjxm1 = model.FSJXM1,
                Xydm1 = model.XYDM1,
                Xyxm1 = model.XYXM1,
                Sjdm2 = model.SJDM2,
                Sjxm2 = model.SJXM2,
                Fsjdm2 = model.FSJDM2,
                Fsjxm2 = model.FSJXM2,
                Xydm2 = model.XYDM2,
                Xyxm2 = model.XYXM2,
                Zcfsdm = model.ZCFSDM,
                Zcfsmc = model.ZCFSMC,
                Jczl = model.JCZL,
                Jx = model.JX,
                Jch = model.JCH,
                Bdrq = model.BDRQ,
                Jied = model.JIED,
                Cq = model.CQ,
                Jiec = model.JIEC,
                Jiaoc = model.JIAOC,
                Tq = model.TQ,
                Cbd = model.CBD,
                Rwd = model.RWD,
                Cwd = model.CWD,
                Rbd = model.RBD,
                Jsl1 = model.JSL1,
                Jrl1 = model.JRL1,
                Jcl1 = model.JCL1,
                Zsdjsl1 = model.ZSDJSL1,
                Zsdjcl1 = model.ZSDJCL1,
                Lcgdjsl1 = model.LCGDJSL1,
                Lcgdjcl1 = model.LCGDJCL1,
                Jsl2 = model.JSL2,
                Jrl2 = model.JRL2,
                Jcl2 = model.JCL2,
                Zsdjsl2 = model.ZSDJSL2,
                Zsdjcl2 = model.ZSDJCL2,
                Lcgdjsl2 = model.LCGDJSL2,
                Lcgdjcl2 = model.LCGDJCL2,
                Dwsj = model.DWSJ,
                Dwyl = model.DWYL,
                Yzmc1 = model.YZMC1,
                Yzyl1 = model.YZYL1,
                Yzmc2 = model.YZMC2,
                Yzyl2 = model.YZYL2,
                Yzmc3 = model.YZMC3,
                Yzyl3 = model.YZYL3,
                Yzmc4 = model.YZMC4,
                Yzyl4 = model.YZYL4,
                Yzmc5 = model.YZMC5,
                Yzyl5 = model.YZYL5,
                Yzmc6 = model.YZMC6,
                Yzyl6 = model.YZYL6,
                Yzmc7 = model.YZMC7,
                Yzyl7 = model.YZYL7,
                Qrcode = model.QRCODE,
                Proceeding = model.PROCEEDING,
                Cwtqdddm = model.CWTQDDDM,
                Cwcqdddm = model.CWCQDDDM,
                Cwjldm = model.CWJLDM,
                Cwjlmc = model.CWJLMC,
                Bdtjdddm = model.BDTJDDDM,
                Bdhsqk = model.BDHSQK,
                Sjgzdm1 = model.SJGZDM1,
                Fsjgzdm1 = model.FSJGZDM1,
                Xygzdm1 = model.XYGZDM1,
                Sjgzdm2 = model.SJGZDM2,
                Fsjgzdm2 = model.FSJGZDM2,
                Xygzdm2 = model.XYGZDM2,
                Sjsrdm1 = model.SJSRDM1,
                Fsjsrdm1 = model.FSJSRDM1,
                Xysrdm1 = model.XYSRDM1,
                Sjsrdm2 = model.SJSRDM2,
                Fsjsrdm2 = model.FSJSRDM2,
                Xysrdm2 = model.XYSRDM2,
                Cwgsdddm = model.CWGSDDDM,
                Bdffr = model.BDFFR,
                Bdhsr = model.BDHSR,
                Fwbj = model.FWBJ,
                Fwzgs = model.FWZGS,
                Fwwd = model.FWWD,
                Fwwj = model.FWWJ,
                Zpjdm = model.ZPJDM,
                Zpjmc = model.ZPJMC,
                Zpddm = model.ZPDDM,
                Zpdmc = model.ZPDMC,
                Lrjdm = model.LRJDM,
                Lrjmc = model.LRJMC,
                Lrddm = model.LRDDM,
                Lrdmc = model.LRDMC,
                Cwyzb1 = model.CWYZB1,
                Cwyzb2 = model.CWYZB2,
                Cwyzb3 = model.CWYZB3,
                Cwyzb4 = model.CWYZB4,
                Cwyzb5 = model.CWYZB5,
                Cwyzb6 = model.CWYZB6,
                Bdxgr = model.BDXGR,
                Bdffsj = model.BDFFSJ,
                Bdtjsj = model.BDTJSJ,
                Bdhssj = model.BDHSSJ,
                Bdxgsj = model.BDXGSJ,
                Jkjldwdm = model.JKJLDWDM,
                Sfsjjlh = model.SFSJJLH,
                Sfsrjlh = model.SFSRJLH,
                Sfczh = model.SFCZH,
                Sfczmc = model.SFCZMC,
                Sfcc = model.SFCC,
                Zdsjjlh = model.ZDSJJLH,
                Zdsrjlh = model.ZDSRJLH,
                Zdczh = model.ZDCZH,
                Zdczmc = model.ZDCZMC,
                Zdcc = model.ZDCZMC,
                Tjzt = model.TJZT,
                Sjsj = model.SJSJ,
                Tjsj = model.TJSJ,
                Tjzh = model.TJZH,
                Yt1bdsjid = model.YT1BDSJID,
                Bdzfsj = model.BDZFSJ,
                Bdzfr = model.BDZFR,
                Lcgdjsl1_2 = string.IsNullOrWhiteSpace(model.LCGDJSL1_2) ? null : int.Parse(model.LCGDJSL1_2.ToString()),
                Lcgdjcl1_2 = string.IsNullOrWhiteSpace(model.LCGDJCL1_2) ? null : int.Parse(model.LCGDJCL1_2.ToString()),
                Lcgdjsl2_2 = string.IsNullOrWhiteSpace(model.LCGDJSL1_2) ? null : int.Parse(model.LCGDJSL2_2.ToString()),
                Lcgdjcl2_2 = string.IsNullOrWhiteSpace(model.LCGDJCL1_2) ? null : int.Parse(model.LCGDJCL2_2.ToString()),
                Eventtime = model.EVENTTIME,
                Bzxh = (int?)model.BZXH,
                Nhkk = model.NHKK,
                Rlbl = model.RLBL,
                Syfs = model.SYFS,
                Nhsfyc = model.NHSFYC,
                Jchsfyc = model.JCHSFYC,
                Jx_Wrcl = model.JX_WRCL,
                Jch_Wrcl = model.JCH_WRCL,
                Sfylr = model.SFYLR,
                Cjdm = model.CJDM,
                Gzfs = model.GZFS,
                Bdhtsj = model.BDHTSJ,
                Bdhtr = model.BDHTR,
                Qxbdbs = model.QXBDBS,
                Bdlxdm = model.BDLXDM,
                Bdlxmc = model.BDLXMC,
                Zzbdbh = model.ZZBDBH,
                //CreateDate = model.CREATEDATE,
                Cddm = model.CDDM,
                Bdtjly = model.BDTJLY,
                Sdzk = model.SDZK,
                Dbsfsh1 = model.DBSFSH1,
                Dbsfsh2 = model.DBSFSH2,
                Jcxh = model.JCXH,
                Zsdjcl2_BF = (int?)model.ZSDJCL2_BF,
                Zsdjsl2_BF = (int?)model.ZSDJSL2_BF,
                Jcl2_BF = (int?)model.JCL2_BF,
                Jsl2_BF = (int?)model.JSL2_BF,
                Zsdjcl1_BF = (int?)model.ZSDJCL1_BF,
                Zsdjsl1_BF = (int?)model.ZSDJSL1_BF,
                Jcl1_BF = (int?)model.JCL1_BF,
                Jsl1_BF = (int?)model.JSL1_BF,
                Aei_RD = model.AEI_RD,
                Aei_CD = model.AEI_CD,
                //AeiProceeding="",
                //Bdlymc="",
                //Bzsjs="",
                //Clbdsjs=""
                //Db=""
                //Hssfyz=""
                //IsWjch=""
                //NhProceeding=""
            };
        }

        return bdsj;
    }
}
