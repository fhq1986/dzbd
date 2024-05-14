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
using DapperExtensions;
using FastReport.Data;
using Lazy.Captcha.Core;
using MySqlConnector;
using Newtonsoft.Json.Linq;
using NPOI.OpenXmlFormats.Wordprocessing;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CgibinTagsMembersGetBlackListResponse.Types;

namespace Admin.NET.Core.Service;

/// <summary>
/// 字典维护
/// </summary>
//[ApiDescriptionSettings(Order = 500,Name = "UserAuth")]
[Route("")]
public class DictionaryService : IDynamicApiController, ITransient
{
    private readonly UserManager _userManager;
    private readonly SqlSugarRepository<SysUserInfo> _sysUserRep;
    private readonly SqlSugarRepository<Roles> _roles;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly SysMenuService _sysMenuService;
    private readonly SysOnlineUserService _sysOnlineUserService;
    private readonly SysConfigService _sysConfigService;
    private readonly ICaptcha _captcha;
    private readonly SysCacheService _sysCacheService;
    private readonly SqlSugarRepository<SysUserInfo> _sysUserInfoRep;
    SqlSugar.ISqlSugarClient _db;
    private readonly SqlSugarRepository<Cwzydd> _Cwzydd;
    public DictionaryService(UserManager userManager,
        SqlSugarRepository<SysUserInfo> sysUserRep,
        SqlSugarRepository<Roles> roles,
        IHttpContextAccessor httpContextAccessor,
        SysMenuService sysMenuService,
        SysOnlineUserService sysOnlineUserService,
        SysConfigService sysConfigService,
        ICaptcha captcha,
        SqlSugar.ISqlSugarClient db,
        SysCacheService sysCacheService, SqlSugarRepository<Cwzydd> Cwzydd, SqlSugarRepository<SysUserInfo> sysUserInfoRep)
    {
        _userManager = userManager;
        _sysUserRep = sysUserRep;
        _roles = roles;
        _httpContextAccessor = httpContextAccessor;
        _sysMenuService = sysMenuService;
        _sysOnlineUserService = sysOnlineUserService;
        _sysConfigService = sysConfigService;
        _captcha = captcha;
        _sysCacheService = sysCacheService;
        _db = db;
        _Cwzydd = Cwzydd;
        _sysUserInfoRep = sysUserInfoRep;
    }

    #region 统计车站信息
    /// <summary>
    /// 树结构(铁路局->机务段)
    /// </summary>
    /// <returns></returns>
    [HttpGet("api/Tree/FindJwdTree")]
    [Authorize(Roles = "jwd_tree")]
    public async Task<IList<TreeNode>> FindJwdTree()
    {
        var user = App.User.Claims;
        var userName = App.User?.FindFirst(ClaimConst.Account)?.Value;
        //管辖的所有铁路局
        IList<TreeNode> fatrees = new List<TreeNode>();
        IList<TreeNode> trees = new List<TreeNode>();

        IList<Tljxx> tljs =await _db.Ado.SqlQueryAsync<Tljxx>($"select id,Tljdm,Tljmc from dic_tlj order by Ord asc");
        IList<Jwdxx> jwdxxs =await _db.Ado.SqlQueryAsync<Jwdxx>($"select id,Jwddm,Jwdmc,Tljdm from dic_jwd order by Ord asc");
        foreach (Tljxx tlj in tljs)
        {
            TreeNode fathertree = new TreeNode() { Id = tlj.Id, NodeCode = tlj.Tljdm, NodeName = tlj.Tljmc };//父节点
            IList<TreeNode> chidrentree = new List<TreeNode>();//子节点集合
            foreach (Jwdxx jwd in jwdxxs)
            {
                if (tlj.Tljdm.Equals(jwd.Tljdm))
                {
                    chidrentree.Add(new TreeNode() { Id = jwd.Id, NodeCode = jwd.Jwddm, NodeName = jwd.Jwdmc, Type = "JWD", Father = fathertree });
                }
            }
            TreeNode father = new TreeNode() { Id = tlj.Id, NodeCode = tlj.Tljdm, NodeName = tlj.Tljmc, Type = "TLJ", Children = chidrentree };
            trees.Add(father);
        }
        fatrees.Add(new TreeNode() { Id = 101, NodeCode = "ZGS", NodeName = "国铁集团", Type = "ZGS", Children = trees });
        return fatrees;
    }
    /// <summary>
    /// 树结构(铁路局->机务段)
    /// </summary>
    /// <returns></returns>
    [HttpGet("api/TjCzxx/FindAll")]
    [Authorize(Roles = "tjczxx_base")]
    public async Task<IActionResult> FindTjCzxxAll([FromQuery] string param)
    {
        JObject json = JObject.Parse(param);
        string sql = @"select tj_dic_czxx.*,dic_czxx.czmc from tj_dic_czxx left join dic_czxx on tj_dic_czxx.czdm=dic_czxx.czdm where 1=1";
        if (json.ContainsKey("Ddm") && !string.IsNullOrEmpty(json.ContainsKey("Ddm").ToString()))
        {
            sql = sql + "and Ddm = '" + json["Ddm"] + "'";
        }
        if (json.ContainsKey("Tjczdm") && !string.IsNullOrEmpty(json.ContainsKey("Tjczdm").ToString()))
        {
            sql = sql + "and Tjczdm = '" + json["Tjczdm"] + "'";
        }
        if (json.ContainsKey("Czdm") && !string.IsNullOrEmpty(json.ContainsKey("Czdm").ToString()))
        {
            sql = sql + "and tj_dic_czxx.Czdm = '" + json["Czdm"] + "'";
        }
        //排序
        if (json["Sort"].ToArray().Length != 0)
        {
            for (int i = 0; i < json["Sort"].ToArray().Length; i++)
            {
                if (Convert.ToBoolean(json["Sort"].ToArray()[i]["Ascending"].ToString()) == true)
                {
                    if (i == 0)
                    {
                        sql = sql + " order by " + json["Sort"].ToArray()[0]["PropertyName"] + "";
                    }
                    else
                    {
                        sql = sql + "," + json["Sort"].ToArray()[i]["PropertyName"] + "";
                    }
                }
                else
                {
                    if (i == 0)
                    {
                        sql = sql + " order by " + json["Sort"].ToArray()[0]["PropertyName"] + " desc";
                    }
                    else
                    {
                        sql = sql + "," + json["Sort"].ToArray()[i]["PropertyName"] + " desc";
                    }
                }
            }
        }
        else
        {
            sql = sql + " order by tj_dic_czxx.ord ";
        }
        var tjczxxs =await  _db.Ado.SqlQueryAsync<TjCzxx>(sql);
        return new JsonResult( new { total = tjczxxs.Count(), items = tjczxxs });
    }
    [HttpGet("api/Czxx/GetAllCz")]
    [Authorize(Roles = "xlt_base")]
    public async  Task<dynamic> GetAllCz(string Tljdm)
    {
        if (string.IsNullOrEmpty(Tljdm))
        {
            List<Zmbm> list = new List<Zmbm>();
            return list;
        }
        else
        {
            string sql = $@"select * from dic_czxx where tljdm=@Tljdm or ','||fjzljdm like @Fjzljdm";
            var list =await _db.Ado.SqlQueryAsync<Czxx>(sql, new { Tljdm = Tljdm, Fjzljdm = $"%,{Tljdm}%" });
            return list;
        }
    }
    /// <summary>
    /// 添加统计车站信息
    /// </summary>
    /// <param name="tjczxx"></param>
    /// <returns></returns>
    [HttpPost("api/TjCzxx/Add")]
    [Authorize(Roles = "tjczxx_add")]
    public async Task<int> AddTjCzxx(TjCzxx tjczxx)
    {
        int result =await _db.Insertable(tjczxx).ExecuteCommandAsync();
        return result;
    }
    /// <summary>
    /// 修改统计车站信息
    /// </summary>
    /// <param name="tjczxx"></param>
    /// <returns></returns>
    [HttpPost("api/TjCzxx/Update")]
    [Authorize(Roles = "tjczxx_edit")]
    public async Task<int> UpdateTjCzxx(TjCzxx tjczxx)
    {
        int result =await _db.Updateable(tjczxx).ExecuteCommandAsync();
        return result;
    }
    /// <summary>
    /// 删除统计车站信息
    /// </summary>
    /// <param name="tjczxx"></param>
    /// <returns></returns>
    [HttpPost("api/TjCzxx/Remove")]
    [Authorize(Roles = "tjczxx_remove")]
    public async Task<int> RemoveTjCzxx(TjCzxx tjczxx)
    {
        int result = await _db.Deleteable(tjczxx).AS("Tj_Dic_Czxx").ExecuteCommandAsync();
        return result;
    }
    #endregion

    #region 车间信息
    [HttpGet("api/Cjxx/FindAll")]
    [Authorize(Roles = "cjxx_base")]
    public async Task<IActionResult> FindCJAll([FromQuery] string param)
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        StringBuilder strSql = new StringBuilder();
        strSql.AppendFormat(@"select  c.*,d.dcjmc from dic_cjxx c left outer join dic_dcjxx d on c.dcjdm=d.dcjdm and c.ddm=d.ddm where 1=1");
        if (param != null)
        {
            JObject models = JObject.Parse(param);
            string sDdm = models.Value<string>("Ddm");
            string sDcjmc = models.Value<string>("Dcjmc");
            string sCjdm = models.Value<string>("Cjdm");
            string sCjmc = models.Value<string>("Cjmc");
            string sCjlx = models.Value<string>("Cjlx");
            if (!string.IsNullOrEmpty(sDdm))
            {
                strSql.AppendFormat(" and c.Ddm=@Ddm ", sDdm);
                dict.Add("Ddm", sDdm);
            }
            else if (sDdm == "")
            {
                strSql.AppendFormat(" and c.Ddm='' ", sDdm);
            }
            if (!string.IsNullOrEmpty(sDcjmc))
            {
                strSql.AppendFormat(" and d.Dcjmc=@Dcjmc", sDcjmc);
                dict.Add("Dcjmc", sDcjmc);
            }
            if (!string.IsNullOrEmpty(sCjdm))
            {
                strSql.AppendFormat(" and c.Cjdm=@Cjdm", sCjdm);
                dict.Add("Cjdm", sCjdm);
            }
            if (!string.IsNullOrEmpty(sCjmc))
            {
                strSql.AppendFormat(" and c.Cjmc=@Cjmc", sCjmc);
                dict.Add("Cjmc", sCjmc);
            }
            if (!string.IsNullOrEmpty(sCjlx))
            {
                strSql.AppendFormat(" and c.Cjlx=@Cjlx", sCjlx);
                dict.Add("Cjlx", sCjlx);
            }
            strSql.AppendFormat(" order by c.Ord ");
        }
        IList<Cjxx> list =await _db.Ado.SqlQueryAsync<Cjxx>(strSql.ToString(), dict);
        return new JsonResult(new { total = list.Count(), items = list });
    }
    [HttpGet("api/Dcjxx/FindAll")]
    [Authorize(Roles = "dcjxx_base")]
    public async Task<IActionResult> FindDcjAll([FromQuery] string param)
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        StringBuilder strSql = new StringBuilder();
        strSql.AppendFormat(@"select * from  dic_dcjxx d where 1=1");
        if (param != null)
        {
            JObject models = JObject.Parse(param);
            string sDdm = models.Value<string>("Ddm");
            if (!string.IsNullOrEmpty(sDdm))
            {
                strSql.AppendFormat(" and d.Ddm=Ddm ", sDdm);
                dict.Add("Ddm", sDdm);
            }
            strSql.AppendFormat(" order by Ord ");
        }
        IList<Dcjxx> list =await _db.Ado.SqlQueryAsync<Dcjxx>(strSql.ToString(), dict);
        return new JsonResult( new { total = list.Count(), items = list });
    }
    [HttpGet("api/Tree/FindCwyxxTree")]
    [Authorize(Roles = "cwy_tree")]
    public async Task<IList<TreeNode>> FindCwyxxTree()
    {
        //管辖的所有铁路局
        IList<TreeNode> fatrees = new List<TreeNode>();
        IList<TreeNode> trees = new List<TreeNode>();
        IList<Tljxx> tljs =await _db.Ado.SqlQueryAsync<Tljxx>($"select id,Tljdm,Tljmc from dic_tlj order by Ord asc");//查询所有铁路局dic_tlj
        IList<Jwdxx> jwdxxs =await _db.Ado.SqlQueryAsync<Jwdxx>($"select id,Jwddm,Jwdmc,Tljdm from dic_jwd order by Ord asc");//查询所有机务段信息
        IList<Cjxx> cjxxs =await _db.Ado.SqlQueryAsync<Cjxx>($"select id,CJdm,Cjmc from dic_cjxx order by Ord asc");//查询所有车间信息
        foreach (Tljxx tlj in tljs)
        {
            IList<TreeNode> chidrentree = new List<TreeNode>();//子节点集合
            foreach (Jwdxx jwd in jwdxxs)
            {
                if (tlj.Tljdm.Equals(jwd.Tljdm))
                {
                    TreeNode cjFathertree = new TreeNode() { Id = jwd.Id, NodeCode = jwd.Jwddm, NodeName = jwd.Jwdmc, FatherNodeCode = tlj.Tljdm };//父节点
                    IList<TreeNode> chidrentrees = new List<TreeNode>();//孙子节点集合
                    foreach (Cjxx cj in cjxxs)
                    {
                        if (jwd.Jwddm.Equals(cj.Ddm))
                        {
                            chidrentrees.Add(new TreeNode() { Id = cj.Id, NodeCode = cj.Cjdm, NodeName = cj.Cjmc, Type = "CJ", Father = cjFathertree });
                        }
                    }
                    chidrentree.Add(new TreeNode() { Id = jwd.Id, NodeCode = jwd.Jwddm, NodeName = jwd.Jwdmc, Type = "JWD", Children = chidrentrees, FatherNodeCode = tlj.Tljdm });
                }
            }
            TreeNode father = new TreeNode() { Id = tlj.Id, NodeCode = tlj.Tljdm, NodeName = tlj.Tljmc, Type = "TLJ", Children = chidrentree };
            trees.Add(father);
        }
        fatrees.Add(new TreeNode() { Id = 101, NodeCode = "ZGS", NodeName = "国铁集团", Type = "ZGS", Children = trees });
        return fatrees;
    }
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="tjczxx"></param>
    /// <returns></returns>
    [HttpPost("api/Cjxx/Add")]
    [Authorize(Roles = "cjxx_add")]
    public async Task<int> AddCjxx(Cjxx tjczxx)
    {
        int result =await _db.Insertable(tjczxx).AS("Dic_cjxx").ExecuteCommandAsync();
        return result;
    }
    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="tjczxx"></param>
    /// <returns></returns>
    [HttpPost("api/Cjxx/Update")]
    [Authorize(Roles = "cjxx_edit")]
    public async Task<int> UpdateCjxx(Cjxx tjczxx)
    {
        int result =await _db.Updateable(tjczxx).AS("Dic_cjxx").ExecuteCommandAsync();
        return result;
    }
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="tjczxx"></param>
    /// <returns></returns>
    [HttpPost("api/Cjxx/Remove")]
    [Authorize(Roles = "cjxx_remove")]
    public async Task<int> RemoveCjxx(Cjxx tjczxx)
    {
        int result =await _db.Deleteable(tjczxx).AS("Dic_cjxx").ExecuteCommandAsync();
        return result;
    }
    #endregion

    #region 乘务员信息
    [HttpGet("api/Cwqd/FindAll")]
    [Authorize(Roles = "cwqd_base")]
    public async Task<IActionResult> FindCwqd([FromQuery] string param)
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        StringBuilder strSql = new StringBuilder();
        strSql.AppendFormat(@"select * from  dic_cwqd d where 1=1");
        if (param != null)
        {
            JObject models = JObject.Parse(param);
            string sDdm = models.Value<string>("Ddm");
            if (!string.IsNullOrEmpty(sDdm))
            {
                strSql.AppendFormat(" and d.Ddm=Ddm ", sDdm);
                dict.Add("Ddm", sDdm);
            }
        }
        IList<Dcjxx> list = _db.Ado.SqlQuery<Dcjxx>(strSql.ToString(), dict);
        return new JsonResult( new { total = list.Count(), items = list });
    }
    [HttpGet("api/Cwjl/FindAll")]
    [Authorize(Roles = "cwjl_base")]
    public async Task<IActionResult> FindCwjl([FromQuery] string param)
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        StringBuilder strSql = new StringBuilder();
        strSql.AppendFormat(@"select * from  Dic_Cwjl d where 1=1");
        if (param != null)
        {
            JObject models = JObject.Parse(param);
            string sDdm = models.Value<string>("Ddm");
            if (!string.IsNullOrEmpty(sDdm))
            {
                strSql.AppendFormat(" and d.Ddm=Ddm ", sDdm);
                dict.Add("Ddm", sDdm);
            }
        }
        IList<Dcjxx> list = _db.Ado.SqlQuery<Dcjxx>(strSql.ToString(), dict);
        return new JsonResult(new { total = list.Count(), items = list });
    }

    /// <summary>
    /// 乘务员登录
    /// </summary>
    /// <param name="cwyxx">乘务员信息</param>
    /// <returns></returns>
    [NonUnify]
    [HttpPost]    [AllowAnonymous]
    [HttpPost("api/Cwyxx/Login")]
    public dynamic Login(Cwyxx cwyxx)
    {
        //return CwyxxService.Login(cwyxx);
        try
        {
            if (string.IsNullOrWhiteSpace(cwyxx.Cwydm)) return new { success = false, message = "用户名为空" };
            if (string.IsNullOrWhiteSpace(cwyxx.Password)) return new { success = false, message = "密码为空" };

            //IPredicate predicate = Predicates.Field<Cwyxx>(p => p.Cwydm, Operator.Eq, cwyxx.Cwydm);
            //Cwyxx cwy = CwyxxRepository.FindAll(predicate).FirstOrDefault();
            Cwyxx cwy = _db.Queryable<Cwyxx>()
                            .Where(x => x.Cwydm == cwyxx.Cwydm)
                            .Single();

            // 当查询不到此用户，或者密码不正确则返回用户名/密码不正确
            if (cwy != null)
            {
                Cwzydd cwzydd = _db.Queryable<Cwzydd>()
                                .Where(x => x.Cwzydddm == cwyxx.Cwzydddm).Single();
                if (null != cwzydd)
                {
                    cwy.PageSize = cwzydd.PageSize;
                }
                //Jwdxx jwdxx = JwdxxService.FindAll(Predicates.Field<Jwdxx>(p => p.Jwddm, Operator.Eq, cwy.Ddm)).FirstOrDefault();
                Jwdxx jwdxx = _db.Queryable<Jwdxx>()
                                .Where(x => x.Jwddm == cwy.Ddm).Single();
                if (null != jwdxx)
                {
                    //是否获取运统1数据，如果是，乘务员终端直接获取电子报单数据，否则获取监控文件数据。
                    cwy.Yt1bs = jwdxx.Yt1bs;
                    cwy.Lcgd = jwdxx.Lcgd;
                    cwy.Zdbs = jwdxx.Zdbs;
                    cwy.Dybs = jwdxx.Dybs;
                    cwy.Ickbs = jwdxx.Ickbs;
                    cwy.Ddm = jwdxx.Jwddm;
                }
                //cwy.Jgdm = XtcsService.FindByCsdm("jgdm");
                Xtcs modelXtcs = _db.Queryable<Xtcs>().Where(x => x.Csdm == "jgdm").Single();
                cwy.Jgdm = modelXtcs == null ? "" : modelXtcs.Cs;
                if (cwyxx.Yt1bs == "true") //一体机直接登录，不需要验证密码
                {
                    return new { success = true, data = cwy, isBd = true };  //如果是本务乘务员，终端则可以修改密码
                }
                else
                {
                    if (((cwy.Password == null && cwyxx.Cwydm.Equals(cwyxx.Password)) || (null != cwy.Password && cwy.Password.Equals(MD5Util.MD5Encoding(cwyxx.Password, cwyxx.Cwydm)))))
                    {
                        return new { success = true, data = cwy, isBd = true };  //如果是本务乘务员，终端则可以修改密码
                    }
                    //else if (XtcsService.FindByCsdm("jtkbdmm") == cwyxx.Password)
                    //{
                    //    return new { success = true, data = cwy, isBd = true };  //如果是本务乘务员，终端则可以修改密码
                    //}
                    Xtcs modelXtcs1 = _db.Queryable<Xtcs>()
                                        .Where(x => x.Csdm == "jtkbdmm").Single();
                    if (modelXtcs1 != null && modelXtcs1.Cs == cwyxx.Password)
                    {
                        return new { success = true, data = cwy, isBd = true };  //如果是本务乘务员，终端则可以修改密码
                    }
                    else
                    {
                        return new { success = false, message = "用户名/密码不正确" };
                    }
                }
            }
            else
            {
                return new { success = false, message = "乘务员不存在" };
            }
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    [NonUnify]
    [AllowAnonymous]
    [HttpPost("api/Cwyxx/UpdatePasswd")]
    public dynamic UpdatePasswd(object param)
    {
        //return CwyxxService.UpdatePasswd(param.ToString());
        try
        {
            JObject jo = JObject.Parse(param+"");
            if (null != jo)
            {
                string cwydm = jo["Cwydm"].ToString();
                string passwd = jo["Password"].ToString();   //数据库MD5数据
                string oldPassword = jo["OldPassword"].ToString();  //前台输入原密码
                string newPassword = jo["NewPassword"].ToString();

                if (!((string.IsNullOrEmpty(passwd) && cwydm == oldPassword) || passwd.Equals(MD5Util.MD5Encoding(oldPassword, cwydm))))
                {
                    return new { success = false, message = "原密码错误,请重新输入！" };
                }
                // MD5加密密码
                string md5Password = MD5Util.MD5Encoding(newPassword, cwydm);
                string sqlText = @"update dic_cwyxx set password = '{0}' where cwydm='{1}'";
                //CwyxxRepository.UpdatePasswd(string.Format(sqlText, md5Password, cwydm));
                _db.Ado.ExecuteCommand(string.Format(sqlText, md5Password, cwydm));
                return new { success = true, message = "保存成功", password = md5Password };
            }
            else
            {
                return new { success = true, message = "保存失败" };
            }
        }
        catch (Exception e)
        {
            return new { success = false, message = "保存失败" };
        }
    }
    [NonUnify]
    [HttpGet("api/Cwyxx/FindAlls")]
    [Authorize(Roles = "cwyxx_base")]
    public dynamic FindAlls(string param)
    {
        try
        {
            JObject jo = JObject.Parse(param);
            //string sql = "select distinct cwydm,cwyxm from dic_cwyxx";
            //if (null != jo && jo.ContainsKey("Ddm") && !string.IsNullOrWhiteSpace(jo.Value<string>("Ddm")))
            //{
            //    sql += " where ddm='" + jo.Value<string>("Ddm") + "'";
            //}
            //var list = CwyxxRepository.FindCwyxx<Cwyxx>(sql).ToList();
            var query = _db.Queryable<Cwyxx>();
            if (null != jo && jo.ContainsKey("Ddm") && !string.IsNullOrWhiteSpace(jo.Value<string>("Ddm")))
            {
                string search = jo.Value<string>("Ddm");
                query = query.Where(x => x.Ddm == search);
            }
            List<Cwyxx> list = query.ToList();
            return new { success = true, data = new { total = list.Count(), items = list } };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }

    }
    [HttpGet("api/Cwyxx/FindAll")]
    [Authorize(Roles = "cwyxx_base")]
    public async Task<IActionResult> FindCwyxx([FromQuery] string param)
    {
        StringBuilder strSql = new StringBuilder();
        StringBuilder strCdSql = new StringBuilder();
        IEnumerable<CheDuixx> Cdlist = null;//Dic_cwyxx
        int page = 1;
        int limit = 20;
        strSql.Append(@"select t.*,dic_zhiweixx.ZhiWeiName as Zbmc,dic_gangweixx.gangweiname as Gwmc ,dic_cheduixx.cheduiname as Cdmc from ( select * from dic_cwyxx where 1=1 ");
        JObject json = JObject.Parse(param);
        if (json.ContainsKey("Cwydm") && !string.IsNullOrEmpty(json["Cwydm"].ToString()))
        {
            strSql.AppendFormat(" and Cwydm='{0}'", json["Cwydm"]); ;
        }
        if (json.ContainsKey("Cwyxm") && !string.IsNullOrEmpty(json["Cwyxm"].ToString()))
        {
            strSql.AppendFormat(" and Cwyxm='{0}'", json["Cwyxm"]);
        }
        if (json.ContainsKey("Jpm") && !string.IsNullOrEmpty(json["Jpm"].ToString()))
        {
            strSql.AppendFormat(" and Jpm='{0}'", json["Jpm"]);
        }
        if (json.ContainsKey("CheDuiCode") && !string.IsNullOrEmpty(json["CheDuiCode"].ToString()))
        {
            strSql.AppendFormat(" and CheDuiCode='{0}'", json["CheDuiCode"]);
        }
        if (json.ContainsKey("GangWeiCode") && !string.IsNullOrEmpty(json["GangWeiCode"].ToString()))
        {
            strSql.AppendFormat(" and GangWeiCode='{0}'", json["GangWeiCode"]);
        }
        if (json.ContainsKey("ZhiWeiCode") && !string.IsNullOrEmpty(json["ZhiWeiCode"].ToString()))
        {
            strSql.AppendFormat(" and ZhiWeiCode='{0}'", json["ZhiWeiCode"]);
        }
        if (json.ContainsKey("Gzzb") && !string.IsNullOrEmpty(json["Gzzb"].ToString()))
        {
            strSql.AppendFormat(" and Gzzb='{0}'", json["Gzzb"]);
        }
        if (json.ContainsKey("Isjj") && !string.IsNullOrEmpty(json["Isjj"].ToString()))
        {
            strSql.AppendFormat(" and Isjj='{0}'", json["Isjj"]);
        }
        if (json.ContainsKey("StartTime") && json.ContainsKey("EndTime") && json["StartTime"].ToString() != "" && json["EndTime"].ToString() != "")
        {
            strSql.AppendFormat(" and Rzrq between '" + json["StartTime"] + "' and '" + json["EndTime"] + "'");
        }
        else if (json.ContainsKey("StartTime") && json["StartTime"].ToString() != "")
        {
            strSql.AppendFormat(" and Rzrq >= '" + json["StartTime"] + "'");
        }
        else if (json.ContainsKey("EndTime") && json["EndTime"].ToString() != "")
        {
            strSql.AppendFormat(" and Rzrq <= '" + json["EndTime"] + "'");
        }
        if (json.ContainsKey("Ddm"))
        {
            strSql.AppendFormat(" and Ddm='{0}'", json["Ddm"]);
            Cdlist = _db.Queryable<CheDuixx>().Where(p => p.Ddm == json["Ddm"].ToString()).ToList();
            strCdSql.AppendFormat(" and Ddm='{0}'", json["Ddm"]);
        }
        if (json.ContainsKey("Cjdm") && !string.IsNullOrEmpty(json["Cjdm"].ToString()))
        {
            strSql.AppendFormat(" and Cjdm='{0}'", json["Cjdm"]);
            Cdlist = _db.Queryable<CheDuixx>().Where(p => p.Ddm == json["Cjdm"].ToString()).ToList();
            strCdSql.AppendFormat(" and chejiancode='{0}'", json["Cjdm"]);
        }
        page = json.Value<int>("page");
        limit = json.Value<int>("limit");
        strSql.Append(@"  )t left join dic_zhiweixx on t.ZhiWeiCode=dic_zhiweixx.ZhiWeiCode
                                     left join dic_gangweixx on t.GangWeiCode=dic_gangweixx.gangweicode
                                     left join dic_cheduixx on t.CheDuiCode=dic_cheduixx.cheduicode");
        //排序
        if (json["Sort"].ToArray().Length != 0)
        {
            for (int i = 0; i < json["Sort"].ToArray().Length; i++)
            {
                if (Convert.ToBoolean(json["Sort"].ToArray()[i]["Ascending"].ToString()) == true)
                {
                    if (i == 0)
                    {
                        strSql.Append(" order by " + json["Sort"].ToArray()[0]["PropertyName"] + "");
                    }
                    else
                    {
                        strSql.Append("," + json["Sort"].ToArray()[i]["PropertyName"] + "");
                    }
                }
                else
                {
                    if (i == 0)
                    {
                        strSql.Append(" order by " + json["Sort"].ToArray()[0]["PropertyName"] + " desc");
                    }
                    else
                    {
                        strSql.Append("," + json["Sort"].ToArray()[i]["PropertyName"] + " desc");
                    }
                }
            }
        }
        var list = await _db.Ado.SqlQueryAsync<Cwyxx>(strSql.ToString());
        var Cds = _db.Ado.SqlQuery<CheDuixx>($@"select * from DIC_CHEDUIXX t where 1=1 {strCdSql}").ToPagedList(1, 1000).Items;
        var Gws = _db.Queryable<Gwxx>().OrderBy(p => p.GangWeiCode);
        var Zbs = _db.Queryable<Zbxx>().OrderBy(p => p.ZhiWeiCode);
        var cwyList = list.Skip((page - 1) * limit).Take(limit).ToList();
        return  new JsonResult(new { total = list.Count(), items = cwyList, Cds = Cds, Gws = Gws.ToList(), Zbs = Zbs.ToList(), Cdlist = Cds, allItems = list });
    }
    [HttpGet("api/Cwzydd/FindAll")]
    [Authorize(Roles = "cwzydd_base")]
    public dynamic FindCwzyddAll([FromQuery] string param)
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        StringBuilder strSql = new StringBuilder();
        strSql.AppendFormat(@"select * from  Dic_cwzydd d where 1=1");
        if (param != null)
        {
            JObject models = JObject.Parse(param);
            string sDdm = models.Value<string>("Jwddm");
            string cwzydddm = models.Value<string>("Cwzydddm");
            string cwzyddmc = models.Value<string>("Cwzyddmc");
            if (!string.IsNullOrWhiteSpace(sDdm))
            {
                strSql.AppendFormat(" and d.Jwddm=@Jwddm");
                dict.Add("Jwddm", sDdm.Trim());
            }
            if (!string.IsNullOrEmpty(cwzydddm))
            {
                strSql.AppendFormat(" and d.Cwzydddm=@Cwzydddm");
                dict.Add("Cwzydddm", cwzydddm.Trim());
            }
            if (!string.IsNullOrEmpty(cwzyddmc))
            {
                strSql.AppendFormat(" and d.cwzyddmc like @Cwzyddmc");
                dict.Add("Cwzyddmc", $"%{cwzyddmc.Trim()}%");
            }
            strSql.AppendFormat(" order by Ord ");
        }
        IList<Cwzydd> list = _db.Ado.SqlQuery<Cwzydd>(strSql.ToString(), dict);
        return new { total = list.Count(), items = list };
    }
    [HttpGet("api/Cwzydd/FindAllFromzgs")]
    [Authorize(Roles = "cwzydd_queryfromzgs")]
    public async Task<IList<Cwzydd>> FindAllFromzgs(string param)
    {
        StringBuilder sbSql = new StringBuilder("select * from Dic_cwzydd where 1=1 ");
        if (string.IsNullOrEmpty(param) || param == null) return null;
        JObject jo = JObject.Parse(param);
        if (!string.IsNullOrWhiteSpace(jo["Jwddm"].ToString())) sbSql.Append($" and Jwddm like @Jwddm");

        if (!string.IsNullOrWhiteSpace(jo["sort"].ToString()))
        {
            sbSql.Append($" order by {jo["sort"].ToString()}");
        }
        var data =await _db.Ado.SqlQueryAsync<Cwzydd>(sbSql.ToString(), new { Jwddm=$"%{jo["Jwddm"].ToString()}%" });
        return data;
    }
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="tjczxx"></param>
    /// <returns></returns>
    [HttpPost("api/Cwzydd/Add")]
    [Authorize(Roles = "cwzydd_add")]
    public async Task<int> AddCwzydd(Cwzydd tjczxx)
    {
        int result =await _db.Insertable(tjczxx).ExecuteCommandAsync();
        return result;
    }
    [HttpPost("api/Cwyxx/Add")]
    [Authorize(Roles = "wwyxx_add")]
    public async Task<int> AddCwzyxx(Cwyxx tjczxx)
    {
        int result =await _db.Insertable(tjczxx).ExecuteCommandAsync();
        return result;
    }
    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="tjczxx"></param>
    /// <returns></returns>
    [HttpPost("api/Cwzydd/Update")]
    [Authorize(Roles = "cwzydd_edit")]
    public async Task<int> UpdateCwzydd(Cwzydd tjczxx)
    {
        int result =await _db.Updateable(tjczxx).ExecuteCommandAsync();
        return result;
    }
    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="tjczxx"></param>
    /// <returns></returns>
    [HttpPost("api/Cwyxx/Update")]
    [Authorize(Roles = "wwyxx_edit")]
    public async Task<int> UpdateCwyxx(Cwyxx tjczxx)
    {
        int result =await _db.Updateable(tjczxx).ExecuteCommandAsync();
        return result;
    }
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="tjczxx"></param>
    /// <returns></returns>
    [HttpPost("api/Cwzydd/Remove")]
    [Authorize(Roles = "cwzydd_remove")]
    public async Task<int> RemoveCwzydd(Cwzydd tjczxx)
    {
        int result =await _db.Deleteable(tjczxx).ExecuteCommandAsync();
        return result;
    }
    [HttpPost("api/Cwyxx/Remove")]
    [Authorize(Roles = "cwyxx_remove")]
    public async Task<int> Removecwyxx(Cwyxx tjczxx)
    {
        int result =await _db.Deleteable(tjczxx).ExecuteCommandAsync();
        return result;
    }
    [HttpPost("api/Cwzyxx/Remove")]
    [Authorize(Roles = "cwzyxx_remove")]
    public async Task<int> RemoveCwzyxx(Cwyxx tjczxx)
    {
        int result =await _db.Deleteable(tjczxx).ExecuteCommandAsync();
        return result;
    }
    [NonUnify]
    [HttpGet("api/Jch/GetJxByJch")]
    public dynamic GetJxByJch(string jch)
    {
        try
        {
            //var xsxxs = JchService.GetJxByJch(jch);
            #region JchService.GetJxByJch(jch)
            //string sql = @"select jx jcxh from dic_jch where jch = '" + jch + "'";
            //IList<Jchxx> jchs = JchRepository.GetGroupByJcxh(sql).ToList();
            //return jchs;
            List<Jchxx> xsxxs = _db.Queryable<Jchxx>()
                                    .Where(x => x.Jch == jch)
                                    .Select(x => new Jchxx()
                                    {
                                        JCXH = x.Jx
                                    }).ToList();
            #endregion
            return new { success = true, data = xsxxs, total = xsxxs.Count() };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    #endregion

    #region 乘务作业地点分配
    [HttpGet("api/Tree/FindUserGlTree")]
    public async Task<IList<TreeNode>> FindUserGlTree()
    {
        //管辖的所有铁路局
        IList<TreeNode> fatrees = new List<TreeNode>();
        IList<TreeNode> trees = new List<TreeNode>();
        IList<string> list = new List<string>() { "1,JL,机辆单位", "2,QT,其它单位" };
        IList<Tljxx> tljs = await _db.Ado.SqlQueryAsync<Tljxx>($"select id,Tljdm,Tljmc from dic_tlj order by Ord asc");
        IList<Jwdxx> jwdxxs = await _db.Ado.SqlQueryAsync<Jwdxx>($"select id,Jwddm,Jwdmc,Tljdm from dic_jwd order by Ord asc");
        IList<Cjxx> cjxxs = await _db.Ado.SqlQueryAsync<Cjxx>($"select id,CJdm,Cjmc,Ddm from dic_cjxx order by Ord asc");
        IList<Gwd> gwds = await _db.Ado.SqlQueryAsync<Gwd>($"select Gwddm,Gwdmc,Tljdm from dic_gwd order by Ord asc");
        IList<Gdd> gdds = await  _db.Ado.SqlQueryAsync<Gdd>($"select Gdddm,Gddmc,Tljdm from dic_gdd order by Ord asc");
        IList<Dwd> dwds = await  _db.Ado.SqlQueryAsync<Dwd>($"select Dwddm,Dwdmc,Tljdm from dic_dwd order by Ord asc");
        IList<Xbdm> xbdmxxs = new List<Xbdm>();
        int i = 1;
        foreach (Gwd gwd in gwds)
        {
            Xbdm xbdm = new Xbdm();
            xbdm.xbdm = gwd.Gwddm;
            xbdm.xbmc = gwd.Gwdmc;
            xbdm.ljdm = gwd.Tljdm;
            xbdm.Id = i;
            xbdmxxs.Add(xbdm);
            i++;
        }
        foreach (Gdd gdd in gdds)
        {
            Xbdm xbdm = new Xbdm();
            xbdm.xbdm = gdd.Gdddm;
            xbdm.xbmc = gdd.Gddmc;
            xbdm.ljdm = gdd.Tljdm;
            xbdm.Id = i;
            xbdmxxs.Add(xbdm);
            i++;
        }
        foreach (Dwd dwd in dwds)
        {
            Xbdm xbdm = new Xbdm();
            xbdm.xbdm = dwd.Dwddm;
            xbdm.xbmc = dwd.Dwdmc;
            xbdm.ljdm = dwd.Tljdm;
            xbdm.Id = i;
            xbdmxxs.Add(xbdm);
            i++;
        }
        foreach (Tljxx tlj in tljs)
        {
            IList<TreeNode> chidrentree = new List<TreeNode>();//子节点集合                
            foreach (string str in list)
            {
                List<string> st = str.Split(',').ToList();
                TreeNode flFathertree = new TreeNode() { Id = int.Parse(st[0]), NodeCode = st[1], NodeName = st[2], FatherNodeCode = tlj.Tljdm };//父节点
                IList<TreeNode> chidrentrees = new List<TreeNode>();//孙子节点集合
                if (st[1].Equals("JL"))
                {
                    List<Jwdxx> jwdxxs1 = jwdxxs.Where(o => o.Tljdm.Equals(tlj.Tljdm)).ToList();
                    foreach (Jwdxx jwd in jwdxxs1)
                    {
                        TreeNode cjFathertree = new TreeNode() { Id = int.Parse(st[0]), NodeCode = st[1], NodeName = st[2], FatherNodeCode = jwd.Jwddm };//父节点
                        IList<TreeNode> chidrentrees1 = new List<TreeNode>();//孙子节点集合
                        IList<Cjxx> cjxxs1 = cjxxs.Where(o => o.Ddm.Equals(jwd.Jwddm)).ToList();
                        foreach (Cjxx cjxx in cjxxs1)
                        {
                            chidrentrees1.Add(new TreeNode() { Id = cjxx.Id, NodeCode = cjxx.Cjdm, NodeName = cjxx.Cjmc, Type = "CJ", Father1 = cjFathertree });
                        }
                        chidrentrees.Add(new TreeNode() { Id = jwd.Id, NodeCode = jwd.Jwddm, NodeName = jwd.Jwdmc, Type = "JWD", Children = chidrentrees1, Father = flFathertree });
                    }
                    chidrentree.Add(new TreeNode() { Id = int.Parse(st[0]), NodeCode = st[1], NodeName = st[2], Type = "FL", Children = chidrentrees, FatherNodeCode = tlj.Tljdm });
                }
                if (st[1].Equals("QT"))
                {
                    List<Xbdm> xbdmxxs1 = xbdmxxs.Where(o => o.ljdm.Equals(tlj.Tljdm)).ToList();
                    foreach (Xbdm xbdm in xbdmxxs1)
                    {
                        chidrentrees.Add(new TreeNode() { Id = xbdm.Id, NodeCode = xbdm.xbdm, NodeName = xbdm.xbmc, Type = "XBDM", Father = flFathertree });
                    }
                    chidrentree.Add(new TreeNode() { Id = int.Parse(st[0]), NodeCode = st[1], NodeName = st[2], Type = "FL", Children = chidrentrees, FatherNodeCode = tlj.Tljdm });
                }
            }
            TreeNode father = new TreeNode() { Id = tlj.Id, NodeCode = tlj.Tljdm, NodeName = tlj.Tljmc, Type = "TLJ", Children = chidrentree };
            trees.Add(father);
        }
        fatrees.Add(new TreeNode() { Id = 101, NodeCode = "ZGS", NodeName = "国铁集团", Type = "ZGS", Children = trees });
        return fatrees;
    }
    [HttpGet("api/User/FindAll")]
    [Authorize(Roles = "user_base")]
    public async Task<IActionResult> FindUserAll([FromQuery] string param)
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        StringBuilder strSql = new StringBuilder();
        strSql.AppendFormat(@"select * from users d where 1=1");
        if (param != null)
        {
            JObject models = JObject.Parse(param);
            string sJwddm = models.Value<string>("Jwddm");
            string sJwddm2 = models.Value<string>("Jwddm2");
            string sCjdm = models.Value<string>("Cjdm");
            string displayName = models.Value<string>("DisplayName");
            string userName = models.Value<string>("UserName");
            string type = models.Value<string>("Type");
            string tjdm = models.Value<string>("Tljdm");
            strSql.AppendFormat(" and d.Tljdm=@tljdm");
            dict.Add("Tljdm", tjdm);
            if (!string.IsNullOrEmpty(sJwddm2))
            {
                strSql.AppendFormat(" and d.Jwddm=@Jwddm2 ");
                dict.Add("Jwddm2", sJwddm2.Trim());
            }
            if (!string.IsNullOrEmpty(sJwddm))
            {
                strSql.AppendFormat(" and d.Jwddm=@Jwddm ");
                dict.Add("Jwddm", sJwddm.Trim());
            }
            if (!string.IsNullOrEmpty(sCjdm))
            {
                strSql.AppendFormat(" and d.Cjdm=@Cjdm");
                dict.Add("Cjdm", sCjdm.Trim());
            }
            if (!string.IsNullOrEmpty(displayName))
            {
                strSql.AppendFormat(" and d.DisplayName=@DisplayName ");
                dict.Add("DisplayName", displayName.Trim());
            }
            if (!string.IsNullOrEmpty(userName))
            {
                strSql.AppendFormat(" and d.UserName=@UserName");
                dict.Add("UserName", userName.Trim());
            }
            strSql.AppendFormat(" and d.Type=@Type");
            dict.Add("Type", type);
        }
        IList<SysUserInfo> list = _db.Ado.SqlQuery<SysUserInfo>(strSql.ToString(), dict);
        return new JsonResult(new { total = list.Count(), items = list });
    }
    [HttpPost("api/User/FindAllCwzydd_jwd")]
    [Authorize(Roles = "usercwzydd_base")]
    public async Task<IActionResult>  FindAllCwzydd_jwd(FindAllCwzydd_jwd_Request r)
    {
        IList<UserCwzydd> defaultChecked = _db.Ado.SqlQuery<UserCwzydd>("select * from usercwzydd");
        var trees = _db.Queryable<Cwzydd>().Where(t => t.Jwddm == r.Jwd).OrderBy(t => t.Ord);
        return  new JsonResult( new { items = trees.ToList(), defaultChecked = defaultChecked });
    }

    /// <summary>
    /// 添加用户
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    //[HttpPost("api/User/Add")]
    //[Authorize(Roles = "user_add")]
    //public dynamic Add(SysUserInfo user)
    //{
    //    // 用户密码加密
    //    user.PasswordHash = CryptogramUtil.HashPassword(user.PasswordHash);
    //    user.SecurityStamp = CryptogramUtil.HashPassword(user.PasswordHash);
    //    bool result = _sysUserInfoRep.Insert(user);
    //    return new { success = true, data = result };

    //}
    [HttpGet("api/User/Add")]
    [Authorize(Roles = "usercwzydd_edit")]
    public async Task<int> Add(string users, string cwzydds)
    {
        var userjas = JArray.Parse(users).Values().ToList();
        var cwzyddjas = JArray.Parse(cwzydds).Values().ToList();
        int result = 1;
        IList<Cwzydd> judgeRoles = _db.Queryable<Cwzydd>().ToList();
        foreach (JToken userja in userjas)
        {
            _db.Ado.BeginTran();
            string delsql = string.Format("delete from usercwzydd where UserName = '{0}'", userja.ToString());
            //先删
            _db.Ado.ExecuteCommand(delsql);
            foreach (JToken cwzyddja in cwzyddjas)
            {
                //后加
                string addsql = string.Format("insert into UserCwzydd values('{0}','{1}')", userja.ToString(), cwzyddja.ToString());
                if (judgeRoles.Where(p => p.Cwzydddm.Equals(cwzyddja.ToString())).Count() > 0) _db.Ado.ExecuteCommand(addsql);
            }
            _db.Ado.CommitTran();
        }
        return result;
    }
    #endregion

    #region 监控机型信息
    [AllowAnonymous]
    [NonUnify]
    [HttpGet("api/Jkjx/FindAll")]
    [Authorize(Roles = "jkjx_base")]
    public async Task<IActionResult> FindJKJXAll([FromQuery] string param=null)
    {
        //{"page":"1","limit":"10","Jkjxdm":"","Jxdm":"","sort":"Jkjxdm"}
        StringBuilder strSql = new StringBuilder();
        strSql.AppendFormat(@"select * from  dic_jkjx d where 1=1");
        if (param != null)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            JObject models = JObject.Parse(param);
            string Jkjxdm = models.Value<string>("Jkjxdm");
            string Jxdm = models.Value<string>("Jxdm");
            string sort = models.Value<string>("sort");
            int page = models.Value<int>("page");
            int pageSize = models.Value<int>("limit");
            if (!string.IsNullOrEmpty(Jkjxdm))
            {
                strSql.AppendFormat(" and d.Jkjxdm like @Jkjxdm ");
                dict.Add("Jkjxdm", $"%{Jkjxdm}%");
            }
            if (!string.IsNullOrEmpty(Jxdm))
            {
                strSql.AppendFormat(" and d.Jxdm like @Jxdm ");
                dict.Add("Jxdm", $"%{Jkjxdm}%");
            }
            strSql.AppendFormat($" order by {sort} ");
            try
            {
                var result = _db.Ado.SqlQuery<Jkjx>(strSql.ToString(), dict).ToPagedList(page, pageSize);
                return new JsonResult( new { success = true, items = result.Items });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false,message=ex.Message });
            }

        }
        else
        {
            try
            {
                var result = _db.Ado.SqlQuery<Jkjx>(strSql.ToString()).OrderBy(x=>x.Jxdm).ToList();
                return new JsonResult(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }

        }
        return null;
    }
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="tjczxx"></param>
    /// <returns></returns>
    [HttpPost("api/Jkjx/Add")]
    [Authorize(Roles = "jkjx_add")]
    public dynamic AddJkjx(Jkjx tjczxx)
    {
        int result = _db.Insertable(tjczxx).AS("dic_jkjx").ExecuteCommand();
        return result;
    }
    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="tjczxx"></param>
    /// <returns></returns>
    [HttpPost("api/Jkjx/Update")]
    [Authorize(Roles = "jkjx_edit")]
    public dynamic UpdateJkjx(Jkjx tjczxx)
    {
        int result = _db.Updateable(tjczxx).AS("dic_jkjx").ExecuteCommand();
        return result;
    }
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="tjczxx"></param>
    /// <returns></returns>
    [HttpPost("api/Jkjx/Delete")]
    [Authorize(Roles = "jkjx_remove")]
    public dynamic RemoveJkjx(Jkjx tjczxx)
    {
        int result = _db.Deleteable(tjczxx).AS("dic_jkjx").ExecuteCommand();
        return result;
    }
    #endregion

    #region 机型信息
    [HttpGet("api/Jxxx/FindAll")]
    [Authorize(Roles = "jxxx_base")]
    public dynamic FindJxxx([FromQuery] string param)
    {
        //{"page":"1","limit":"10","Jkjxdm":"","Jxdm":"","sort":"Jkjxdm"}
        StringBuilder strSql = new StringBuilder();
        strSql.AppendFormat(@"select * from  Dic_Jx d where 1=1");
        if (param != null)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            JObject models = JObject.Parse(param);
            string Jz = models.Value<string>("Jz");
            if (!string.IsNullOrEmpty(Jz))
            {
                strSql.AppendFormat(" and d.Jz= @Jz ", Jz);
                dict.Add("Jz", Jz);
            }
            var result = _db.Ado.SqlQuery<Jxxx>(strSql.ToString(), dict);
            return new { total = result.Count, items = result };
        }
        return new { total = 0, items = "null" };
    }
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="tjczxx"></param>
    /// <returns></returns>
    [HttpPost("api/Jxxx/Add")]
    [Authorize(Roles = "jxxx_add")]
    public dynamic AddJxxx(Jxxx tjczxx)
    {
        int result = _db.Insertable(tjczxx).AS("Dic_Jx").ExecuteCommand();
        return result;
    }
    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="tjczxx"></param>
    /// <returns></returns>
    [HttpPost("api/Jxxx/Update")]
    [Authorize(Roles = "jxxx_edit")]
    public dynamic UpdateJxxx(Jxxx tjczxx)
    {
        int result = _db.Updateable(tjczxx).AS("Dic_Jx").ExecuteCommand();
        return result;
    }
    [HttpPost("api/jxxx/Remove")]
    [Authorize(Roles = "jxxx_remove")]
    public dynamic RemoveJxxx(Jxxx tjczxx)
    {
        int result = _db.Deleteable(tjczxx).AS("Dic_Jx").ExecuteCommand();
        return result;
    }
    #endregion

    #region 机车号信息
    [HttpGet("api/Jch/FindAll")]
    [Authorize(Roles = "jch_base")]
    public dynamic FindJch([FromQuery] string param)
    {
        //{"page":"1","limit":"10","Jkjxdm":"","Jxdm":"","sort":"Jkjxdm"}
        StringBuilder strSql = new StringBuilder();
        strSql.AppendFormat(@"select * from  dic_jch d where 1=1");
        if (param != null)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            JObject models = JObject.Parse(param);
            string Jx = models.Value<string>("Jx");
            if (string.IsNullOrWhiteSpace(Jx))
            {
                return new { total = 0 };
            }
            string Jch = models.Value<string>("Jch");
            strSql.AppendFormat(" and d.Jx=@Jx ");
            dict.Add("Jx", Jx.Trim());
            if (!string.IsNullOrEmpty(Jch))
            {
                strSql.AppendFormat(" and d.Jch=@Jch ");
                dict.Add("Jch", Jch.Trim());
            }
            var result = _db.Ado.SqlQuery<Jchxx>(strSql.ToString(), dict);
            return new { total = result.Count, items = result };
        }
        return new { total = 0 };
    }
    /// <summary>
    /// 树结构(机型)
    /// </summary>
    /// <returns></returns>
    [HttpGet("api/Tree/FindJxTree")]
    [Authorize(Roles = "jx_tree")]
    public dynamic FindJxTree()
    {
        //机种
        IList<TreeNode> fatrees = new List<TreeNode>();
        IList<TreeNode> trees = new List<TreeNode>();
        IList<Dmmc> jzs = new List<Dmmc>(); //所有机种
        jzs.Add(new Dmmc() { Dm = "D", Mc = "电力" });
        jzs.Add(new Dmmc() { Dm = "N", Mc = "内燃" });
        IList<Jxxx> jxxxs = _db.Queryable<Jxxx>().OrderBy(p => p.Jx).ToList(); //查询机车号
        foreach (Dmmc jz in jzs)
        {
            TreeNode fathertree = new TreeNode() { NodeCode = jz.Dm, NodeName = jz.Mc };//父节点
            IList<TreeNode> chidrentree = new List<TreeNode>();//子节点集合
            foreach (Jxxx jx in jxxxs)
            {
                if (jz.Dm.Equals(jx.Jz))
                {
                    chidrentree.Add(new TreeNode() { Id = jx.Id, NodeCode = jx.Jx, NodeName = jx.Jx, Type = "Jx", Father = fathertree });
                }
            }

            TreeNode father = new TreeNode() { NodeCode = jz.Dm, NodeName = jz.Mc, Children = chidrentree };
            trees.Add(father);
        }
        fatrees.Add(new TreeNode() { Id = 101, NodeCode = "Zl", NodeName = "机型信息", Type = "Zl", Children = trees });
        return fatrees;
    }
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="tjczxx"></param>
    /// <returns></returns>
    [HttpPost("api/Jch/Add")]
    [Authorize(Roles = "jch_add")]
    public dynamic AddJch(Jchxx tjczxx)
    {
        int result = _db.Insertable(tjczxx).AS("dic_jch").ExecuteCommand();
        return result;
    }
    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="tjczxx"></param>
    /// <returns></returns>
    [HttpPost("api/Jch/Update")]
    [Authorize(Roles = "jch_edit")]
    public dynamic UpdateJch(Jchxx tjczxx)
    {
        int result = _db.Updateable(tjczxx).AS("dic_jch").ExecuteCommand();
        return result;
    }
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="tjczxx"></param>
    /// <returns></returns>
    [HttpPost("api/Jch/Remove")]
    [Authorize(Roles = "jch_remove")]
    public dynamic RemoveJch(Jchxx tjczxx)
    {
        int result = _db.Deleteable(tjczxx).AS("dic_jch").ExecuteCommand();
        return result;
    }
    [HttpPost("api/Jch/FileUpload")]
    public async Task<IActionResult> FileUpload()
    {
        //Check if the request contains multipart / form - data.
        if (!App.HttpContext.Request.Headers["Content-Type"].Any(t => t.Contains("multipart/form-data")))
        {
            return new JsonResult(new { sunccess = false, message = "文件不正确！" });
        }
        try
        {
            var files = App.HttpContext.Request.Form.Files;
            //Read the form data.
            //await App.HttpContext.Request.ReadAsMultipartAsync(provider);
            string sbbm = App.HttpContext.Request.Form["ddm"];
            //读取上传文件
            foreach (var file in files)
            {
                string bbm = file.FileName.Substring(file.FileName.Length - 4, 4);
                if (!bbm.Equals("JTZD", StringComparison.OrdinalIgnoreCase))
                {
                    return new JsonResult(new { sunccess = false, message = "文件不正确！" });
                }
                using var stream = file.OpenReadStream();
                using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.Default))
                {
                    JObject joZdsj = null;
                    try
                    {
                        string data = reader.ReadToEnd();
                        data = data.Replace(@"大碶\", "大碶");
                        joZdsj = JObject.Parse(data);
                        JArray jaZdsjs = joZdsj.Value<JArray>("data");
                        if (null == jaZdsjs) throw new Exception("传入的字典文件不正确！");
                        JObject joXtcs = jaZdsjs.Children<JObject>().FirstOrDefault(p => p.Value<string>("name").Equals("dic_xtcs", StringComparison.OrdinalIgnoreCase));
                        if (null == joXtcs) throw new Exception("传入的字典文件不正确！");
                        JObject joDdm = joXtcs.Value<JArray>("records").Children<JObject>().FirstOrDefault(p => p.Value<string>("csmc").Equals("ddm", StringComparison.OrdinalIgnoreCase));
                        if (null == joDdm) throw new Exception("传入的字典文件不正确！");
                        if (!joDdm.Value<string>("cs").Substring(0, 1).Equals(sbbm, StringComparison.OrdinalIgnoreCase)) return new JsonResult(new { sunccess = false, message = $"传入的字典文件不属于单位{bbm}！" });
                        JObject joJch = jaZdsjs.Children<JObject>().FirstOrDefault(p => p.Value<string>("name").Equals("dic_jcbm", StringComparison.OrdinalIgnoreCase));
                        if (null == joJch) throw new Exception("传入的字典文件缺少局编码！");
                        JArray jaJch = joJch.Value<JArray>("records");
                        if (null == jaJch) throw new Exception("传入的字典文件缺少局编码！");
                        var jchs = new List<Jchxx>();
                        _db.Ado.BeginTran();
                        foreach (var jch in jaJch)
                        {

                            _db.Ado.ExecuteCommandAsync($"delete from dic_jch where jx=@Jx and jch=@Jch", new { Jx = jch.Value<string>("jcxh"), Jch = jch.Value<string>("jch") });
                            var jch1 = new Jchxx
                            {
                                Jx = jch.Value<string>("jcxh"),
                                Jch = jch.Value<string>("jch"),
                                Ziz = (int)(jch.Value<string>("ziz") == "" ? 0 : Convert.ToDecimal(jch.Value<string>("ziz"))),
                                Gl = jch.Value<int>("gl"),
                                Hc = (int)(jch.Value<string>("hc") == "" ? 0 : Convert.ToDecimal(jch.Value<string>("hc"))),
                                Zaiz = (int)(jch.Value<string>("zaiz") == "" ? 0 : Convert.ToDecimal(jch.Value<string>("zaiz"))),
                                Ls = jch.Value<int>("ls"),
                                Ccrq = jch.Value<string>("ccrq")
                            };

                            jchs.Add(jch1);
                        }
                        _db.Fastest<Jchxx>().BulkCopyAsync(jchs);
                        //_db.Insertable(jchs).ExecuteCommandAsync();
                        _db.Ado.CommitTran();
                    }
                    catch (Exception e)
                    {
                        _db.Ado.RollbackTran();
                        return new JsonResult(new { success = false, message = e.Message });
                    }
                    finally
                    {
                        reader.Close();
                    }
                }
            }
            return new JsonResult(new { success = true, message = "数据导入成功！" });
        }
        catch (System.Exception e)
        {
            return new JsonResult(new { success = false, message = e.Message });
        }
    }
    #endregion

    #region 机务段
    [NonUnify]
    [HttpGet("api/Jwdxx/FindAll")]
    [Authorize(Roles = "jwdxx_base")]
    public dynamic FindJwdxx([FromQuery] string param)
    {
        JObject models = JObject.Parse(param);
        string Tljdm = models.Value<string>("Tljdm");
        var result = _db.Queryable<Jwdxx>().WhereIF(!SqlFunc.IsNullOrEmpty(Tljdm), t => t.Tljdm == Tljdm).ToList();
        //return new { total = result.Count(), items = result };
        return new
        {

            success = true,
            data = new
            {
                total = result.Count(),
                items = result
            }
        };
    }
    #endregion
}