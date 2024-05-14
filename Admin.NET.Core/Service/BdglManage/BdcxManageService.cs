// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Core.DapperEx;
using Admin.NET.Core.Entity;
using Admin.NET.Core.Entity.ElectReport;
using Admin.NET.Core.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CgibinUserInfoBatchGetRequest.Types;

namespace Admin.NET.Core.Service.BdglManage;

/// <summary>
/// 报单查询信息管理接口
/// </summary>
[NonUnify]
[ApiDescriptionSettings(Order = 390)]
public class BdcxManageService : IDynamicApiController, ITransient
{
    ISqlSugarClient _db;
    IDapperRepository _dap;
    public BdcxManageService(SqlSugar.ISqlSugarClient db, IDapperRepository dap)
    {
        _db = db;
        _dap = dap;
    }
    /// <summary>
    /// 查询编组数据
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("/api/Bdcx/FindAllBzsj")]
    public dynamic FindAllBzsj(string param)
    {
        try
        {
            //var user = BzcxService.FindAll(param);
            var user = FindAll(param);
            return new { success = true, data = new { items = user } };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 根据乘务员查询所有报单
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("/api/Bdcx/FindAllBdsjBycwy")]
    public dynamic FindAllBdsjBycwy(string param)
    {
        try
        {
            //var user = BdsjService.FindAllBdsjByCwy(param);
            //return new { success = true, data = new { items = user } };

            if(string.IsNullOrWhiteSpace(param))
            {
                return new { success = true, data = new { items =new List<Bdsj>() } };
            }
            IList<Bdsj> result = null;
            JObject jo = JObject.Parse(param);
            IList<string> properties = jo.Properties().Select(p => p.Name).ToList();
            var query = _db.Queryable<DAT_BDSJ>();
            if (properties.Contains("qrcode") && !string.IsNullOrEmpty(jo["qrcode"].ToString()))
            {
                //sqlText += " and qrcode=:qrcode ";
                //dyparam.Add("qrcode", value: jo["qrcode"].ToString());
                query = query.Where(x => x.QRCODE == jo["qrcode"].ToString());
            }
            if (properties.Contains("Bdrq") && !string.IsNullOrEmpty(jo["Bdrq"].ToString()))
            {
                //sqlText += " and Bdrq=:Bdrq ";
                //dyparam.Add("Bdrq", value: jo["Bdrq"].ToString());
                query = query.Where(x => x.BDRQ == jo["Bdrq"].ToString());
            }
            if (properties.Contains("Sjdm1") && !string.IsNullOrEmpty(jo["Sjdm1"].ToString()))
            {
                //sqlText += " and (sjdm1=:Sjdm1";
                //sqlText += " or fsjdm1=:Fsjdm1";
                //sqlText += " or xydm1=:Xydm1";
                //sqlText += " or sjdm2=:Sjdm2";
                //sqlText += " or fsjdm2=:Fsjdm2";
                //sqlText += " or xydm2=:Xydm2)";
                //dyparam.Add("Sjdm1", value: jo["Sjdm1"].ToString());
                //dyparam.Add("Fsjdm1", value: jo["Sjdm1"].ToString());
                //dyparam.Add("Xydm1", value: jo["Sjdm1"].ToString());
                //dyparam.Add("Sjdm2", value: jo["Sjdm1"].ToString());
                //dyparam.Add("Fsjdm2", value: jo["Sjdm1"].ToString());
                //dyparam.Add("Xydm2", value: jo["Sjdm1"].ToString());
                query = query.Where(x => x.SJDM1 == jo["Sjdm1"].ToString()
                                        || x.FSJDM1 == jo["Sjdm1"].ToString()
                                        || x.XYDM1 == jo["Sjdm1"].ToString()
                                        || x.SJDM2 == jo["Sjdm1"].ToString()
                                        || x.FSJDM2 == jo["Sjdm1"].ToString()
                                        || x.XYDM2 == jo["Sjdm1"].ToString());
            }
            //IList<Bdsj> allbdsjs = BdsjRepository.FindByCondition(sqlText, dyparam);
            //bdsjs = allbdsjs.OrderByDescending(p => p.Id).ToList();
            List<DAT_BDSJ> lst = query.OrderByDescending(x => x.ID).ToList();
            if(lst!=null && lst.Count>0)
            {
                result = new List<Bdsj>();
                foreach (var item in lst)
                {
                    result.Add(new Bdsj()
                    {
                        Id = (int)item.ID,
                        Sjdm1 = item.SJDM1,
                        Sjxm1 = item.SJXM1,
                        Fsjdm1 = item.FSJDM1,
                        Fsjxm1 = item.FSJXM1,
                        Xydm1 = item.XYDM1,
                        Xyxm1 = item.XYXM1,
                        Sjdm2 = item.SJDM2,
                        Sjxm2 = item.SJXM2,
                        Fsjdm2 = item.FSJDM2,
                        Fsjxm2 = item.FSJXM2,
                        Xydm2 = item.XYDM2,
                        Xyxm2 = item.XYXM2,
                        Zcfsdm = item.ZCFSDM,
                        Zcfsmc = item.ZCFSMC,
                        Jczl = item.JCZL,
                        Jx = item.JX,
                        Jch = item.JCH,
                        Bdrq = item.BDRQ,
                        Jied = item.JIED,
                        Cq = item.CQ,
                        Jiec = item.JIEC,
                        Jiaoc = item.JIAOC,
                        Tq = item.TQ,
                        Cbd = item.CBD,
                        Rwd = item.RWD,
                        Cwd = item.CWD,
                        Rbd = item.RBD,
                        Jsl1 = (int?)item.JSL1,
                        Jrl1 = (int?)item.JRL1,
                        Jcl1 = (int?)item.JCL1,
                        Zsdjsl1 = (int?)item.ZSDJSL1,
                        Zsdjcl1 = (int?)item.ZSDJCL1,
                        Lcgdjsl1 = (int?)item.LCGDJSL1,
                        Lcgdjcl1 = (int?)item.LCGDJCL1,
                        Jsl2 = (int?)item.JSL2,
                        Jrl2 = (int?)item.JRL2,
                        Jcl2 = (int?)item.JCL2,
                        Zsdjsl2 = (int?)item.ZSDJSL2,
                        Zsdjcl2 = (int?)item.ZSDJCL2,
                        Lcgdjsl2 = (int?)item.LCGDJSL2,
                        Lcgdjcl2 = (int?)item.LCGDJCL2,
                        Dwsj = item.DWSJ,
                        Dwyl = (int?)item.DWYL,
                        Yzmc1 = item.YZMC1,
                        Yzyl1 = item.YZYL1,
                        Yzmc2 = item.YZMC2,
                        Yzyl2 = item.YZYL2,
                        Yzmc3 = item.YZMC3,
                        Yzyl3 = item.YZYL3,
                        Yzmc4 = item.YZMC4,
                        Yzyl4 = item.YZYL4,
                        Yzmc5 = item.YZMC5,
                        Yzyl5 = item.YZYL5,
                        Yzmc6 = item.YZMC6,
                        Yzyl6 = item.YZYL6,
                        Yzmc7 = item.YZMC7,
                        Yzyl7 = item.YZYL7,
                        Qrcode = item.QRCODE,
                        Proceeding = (int?)item.PROCEEDING,
                        Cwtqdddm = item.CWTQDDDM,
                        Cwcqdddm = item.CWCQDDDM,
                        Cwjldm = item.CWJLDM,
                        Cwjlmc = item.CWJLMC,
                        Bdtjdddm = item.BDTJDDDM,
                        Bdhsqk = (int?)item.BDHSQK,
                        Sjgzdm1 = item.SJGZDM1,
                        Fsjgzdm1 = item.FSJGZDM1,
                        Xygzdm1 = item.XYGZDM1,
                        Sjgzdm2 = item.SJGZDM2,
                        Fsjgzdm2 = item.FSJGZDM2,
                        Xygzdm2 = item.XYGZDM2,
                        Sjsrdm1 = item.SJSRDM1,
                        Fsjsrdm1 = item.FSJSRDM1,
                        Xysrdm1 = item.XYSRDM1,
                        Sjsrdm2 = item.SJSRDM2,
                        Fsjsrdm2 = item.FSJSRDM2,
                        Xysrdm2 = item.XYSRDM2,
                        Cwgsdddm = item.CWGSDDDM,
                        Bdffr = item.BDFFR,
                        Bdhsr = item.BDHSR,
                        Fwbj = (int?)item.FWBJ,
                        Fwzgs = (int?)item.FWZGS,
                        Fwwd = (int?)item.FWWD,
                        Fwwj = (int?)item.FWWJ,
                        Zpjdm = item.ZPJDM,
                        Zpjmc = item.ZPJMC,
                        Zpddm = item.ZPDDM,
                        Zpdmc = item.ZPDMC,
                        Lrjdm = item.LRJDM,
                        Lrjmc = item.LRJMC,
                        Lrddm = item.LRDDM,
                        Lrdmc = item.LRDMC,
                        Cwyzb1 = item.CWYZB1,
                        Cwyzb2 = item.CWYZB2,
                        Cwyzb3 = item.CWYZB3,
                        Cwyzb4 = item.CWYZB4,
                        Cwyzb5 = item.CWYZB5,
                        Cwyzb6 = item.CWYZB6,
                        Bdxgr = item.BDXGR,
                        Bdffsj = item.BDFFSJ,
                        Bdtjsj = item.BDTJSJ,
                        Bdhssj = item.BDHSSJ,
                        Bdxgsj = item.BDXGSJ,
                        Jkjldwdm = item.JKJLDWDM,
                        Sfsjjlh = item.SFSJJLH,
                        Sfsrjlh = item.SFSRJLH,
                        Sfczh = item.SFCZH,
                        Sfczmc = item.SFCZMC,
                        Sfcc = item.SFCC,
                        Zdsjjlh = item.ZDSJJLH,
                        Zdsrjlh = item.ZDSRJLH,
                        Zdczh = item.ZDCZH,
                        Zdczmc = item.ZDCZMC,
                        Zdcc = item.ZDCZMC,
                        Tjzt = item.TJZT,
                        Sjsj = item.SJSJ,
                        Tjsj = item.TJSJ,
                        Tjzh = item.TJZH,
                        Yt1bdsjid = item.YT1BDSJID,
                        Bdzfsj = item.BDZFSJ,
                        Bdzfr = item.BDZFR,
                        Lcgdjsl1_2 = string.IsNullOrWhiteSpace(item.LCGDJSL1_2) ? null : int.Parse(item.LCGDJSL1_2.ToString()),
                        Lcgdjcl1_2 = string.IsNullOrWhiteSpace(item.LCGDJCL1_2) ? null : int.Parse(item.LCGDJCL1_2.ToString()),
                        Lcgdjsl2_2 = string.IsNullOrWhiteSpace(item.LCGDJSL1_2) ? null : int.Parse(item.LCGDJSL2_2.ToString()),
                        Lcgdjcl2_2 = string.IsNullOrWhiteSpace(item.LCGDJCL1_2) ? null : int.Parse(item.LCGDJCL2_2.ToString()),
                        Eventtime = item.EVENTTIME,
                        Bzxh = (int?)item.BZXH,
                        Nhkk = item.NHKK,
                        Rlbl = item.RLBL,
                        Syfs = item.SYFS,
                        Nhsfyc = item.NHSFYC,
                        Jchsfyc = item.JCHSFYC,
                        Jx_Wrcl = item.JX_WRCL,
                        Jch_Wrcl = item.JCH_WRCL,
                        Sfylr = item.SFYLR,
                        Cjdm = item.CJDM,
                        Gzfs = item.GZFS,
                        Bdhtsj = item.BDHTSJ,
                        Bdhtr = item.BDHTR,
                        Qxbdbs = item.QXBDBS,
                        Bdlxdm = item.BDLXDM,
                        Bdlxmc = item.BDLXMC,
                        Zzbdbh = item.ZZBDBH,
                        //CreateDate = item.CREATEDATE,
                        Cddm = item.CDDM,
                        Bdtjly = item.BDTJLY,
                        Sdzk = item.SDZK,
                        Dbsfsh1 = item.DBSFSH1,
                        Dbsfsh2 = item.DBSFSH2,
                        Jcxh = item.JCXH,
                        Zsdjcl2_BF = (int?)item.ZSDJCL2_BF,
                        Zsdjsl2_BF = (int?)item.ZSDJSL2_BF,
                        Jcl2_BF = (int?)item.JCL2_BF,
                        Jsl2_BF = (int?)item.JSL2_BF,
                        Zsdjcl1_BF = (int?)item.ZSDJCL1_BF,
                        Zsdjsl1_BF = (int?)item.ZSDJSL1_BF,
                        Jcl1_BF = (int?)item.JCL1_BF,
                        Jsl1_BF = (int?)item.JSL1_BF,
                        Aei_RD = item.AEI_RD,
                        Aei_CD = item.AEI_CD
                    });
                }
            }
            return new { success = true, data = new { items = result } };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 报单查询
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    private List<BzsjViewModel> FindAll(string param)
    {
        if (string.IsNullOrEmpty(param) || param == null) return null;
        JObject jo = JObject.Parse(param);
        //IList<IPredicate> predList = new List<IPredicate>();
        var query = _db.Queryable<DAT_BZSJ>();
        IList<string> properties = jo.Properties().Select(p => p.Name).ToList();
        if (!string.IsNullOrWhiteSpace(jo["Id"].ToString()))
            //predList.Add(Predicates.Field<Bzsj>(p => p.Bdsjid, Operator.Eq, jo["Id"].ToString()));
            query = query.Where(x => x.BDSJID == SqlFunc.ToInt32(jo["Id"].ToString()));
        //IPredicateGroup predGroup = Predicates.Group(GroupOperator.And, predList.ToArray());
        //IList<ISort> sor = new List<ISort>();
        if (properties.Contains("sort") && !string.IsNullOrWhiteSpace(jo["sort"].ToString()))
        {
            //sor.Add(new Sort { PropertyName = jo["sort"].ToString(), Ascending = true });
            query = query.OrderBy($"{jo["sort"].ToString()} asc");
        }
        //var users = BzcxRepository.FindAll(predGroup, sor);
        //List<Bzsj> ss = users.ToList();
        //List<BzsjViewModel> bzsj = new List<BzsjViewModel>();
        //BzsjViewModel bz;
        //for (int i = 0; i < ss.Count; i++)
        //{
        //    bz = new BzsjViewModel();
        //    bz.Cc = ss[i].Cc;
        //    bz.Czmc = ss[i].Czmc;
        //    bz.Cljc = ss[i].Cljc;
        //    bz.Dd = ss[i].Dd == null ? "" : ss[i].Dd.Substring(ss[i].Dd.Length - 8, 5);
        //    bz.Cf = ss[i].Cf == null ? "" : ss[i].Cf.Substring(ss[i].Cf.Length - 8, 5);
        //    bz.Dcsj = ss[i].Dcsj;
        //    bz.Jwtcsj = ss[i].Jwtcsj;
        //    bz.Qjgl = ss[i].Qjgl;
        //    bz.Zz = ss[i].Zz;
        //    bz.Zaiz = ss[i].Zaiz;
        //    bz.Kc = ss[i].Kc;
        //    bz.Qzjg = ss[i].Qzjg;
        //    bz.Ddqy = ss[i].Ddqy;
        //    bz.Zc = ss[i].Zc;
        //    bz.Kongc = ss[i].Kongc;
        //    bz.Fyyc = ss[i].Fyyc;
        //    bz.Dkc = ss[i].Dkc;
        //    bz.Qt = ss[i].Qt;
        //    bz.Ls = ss[i].Ls;
        //    bz.Hc = ss[i].Hc;
        //    bz.zbzsjs = ZbzcxService.FindByBzid(ss[i].Id.ToString()).ToList();
        //    bzsj.Add(bz);
        //}
        List<BzsjViewModel> bzsj = query.Select(x=>new BzsjViewModel()
        { 
            Cc=x.CC,
            Czmc=x.CZMC,
            Cljc=x.CLJC,
            Dd=SqlFunc.IIF(SqlFunc.IsNullOrEmpty(x.DD),"",SqlFunc.Substring(x.DD,x.DD.Length-8,5)),
            Cf=SqlFunc.IIF(SqlFunc.IsNullOrEmpty(x.CF),"",SqlFunc.Substring(x.CF,x.CF.Length-8,5)),
            Dcsj=x.DCSJ,
            Jwtcsj=x.JWTCSJ,
            Qjgl=x.QJGL,
            Zz=x.ZZ,
            Zaiz=x.ZAIZ,
            Kc=x.KC,
            Qzjg=x.QZJG,
            Ddqy=x.DDQY,
            Zc=x.ZC,
            Kongc=x.KONGC,
            Fyyc=x.FYYC,
            Dkc=x.DKC,
            Qt=x.QT,
            Ls=x.LS,
            Hc=x.HC
        }).ToList();
        
        return bzsj;
    }
}
