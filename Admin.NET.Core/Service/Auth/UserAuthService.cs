// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Core.Entity.ElectReport;
using Furion.SpecificationDocument;
using Lazy.Captcha.Core;
using Microsoft.AspNetCore.Http;
using Nest;
using Newtonsoft.Json.Linq;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CgibinUserInfoBatchGetRequest.Types;

namespace Admin.NET.Core.Service;

/// <summary>
/// 系统登录授权服务
/// </summary>
//[ApiDescriptionSettings(Order = 500,Name = "UserAuth")]
[Route("")]
public class UserAuthService : IDynamicApiController, ITransient
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
    SqlSugar.ISqlSugarClient _db;

    public UserAuthService(UserManager userManager,
        SqlSugarRepository<SysUserInfo> sysUserRep,
        SqlSugarRepository<Roles> roles,
        IHttpContextAccessor httpContextAccessor,
        SysMenuService sysMenuService,
        SysOnlineUserService sysOnlineUserService,
        SysConfigService sysConfigService,
        ICaptcha captcha,
        SqlSugar.ISqlSugarClient db,
        SysCacheService sysCacheService)
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
    }

    /// <summary>
    /// 账号密码登录
    /// </summary>
    /// <param name="input"></param>
    /// <remarks>用户名/密码：superadmin/123456</remarks>
    /// <returns></returns>
    [AllowAnonymous]
    [DisplayName("账号密码登录")]
    //[ApiDescriptionSettings(Name="Token")]
    [Route("Token")]
    [NonUnify]
    public async Task<object> Login([Required][FromForm] LoginInput input)
    {
        //// 可以根据域名获取具体租户
        //var host = _httpContextAccessor.HttpContext.Request.Host;

        // 是否开启验证码
        //if (await _sysConfigService.GetConfigValue<bool>(CommonConst.SysCaptcha))
        //{
        //    // 判断验证码
        //    if (!_captcha.Validate(input.CodeId.ToString(), input.Code))
        //        throw Oops.Oh(ErrorCodeEnum.D0008);
        //}
        string userName = CryptogramUtil.DecryptByAES(input.UserName).ToString();
        string password = CryptogramUtil.DecryptByAES(input.Password).ToString();
        // 账号是否存在
        var user = await _sysUserRep.AsQueryable().FirstAsync(u => u.UserName.Equals(userName));
        _ = user ?? throw Oops.Oh(ErrorCodeEnum.D0009);

        // 账号是否被冻结
        if (user.LockoutEnabled)
            throw Oops.Oh(ErrorCodeEnum.D1017);


        // 密码是否正确
        if (CryptogramUtil.CryptoType == CryptogramEnum.MD5.ToString())
        {
            if (user.PasswordHash != MD5Encryption.Encrypt(input.Password))
            {
                App.HttpContext.Response.StatusCode = 400;
                return new { success = false, message = "密码不正确", error = "invalid_grant", error_description = "密码错误，请再试一次。" };
            }
        }
        else
        {
            if (!CryptogramUtil.VerifyHashedPassword(user.PasswordHash, password))
            {
                App.HttpContext.Response.StatusCode = 400;
                return new { success = false, message = "密码不正确", error = "invalid_grant", error_description = "密码错误，请再试一次。" };
            }
        }
        var loginOutput=await CreateToken(user);
        return new { access_token = loginOutput.AccessToken, token_type = "bearer", userName=userName };
    }


    /// <summary>
    /// 生成Token令牌
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [NonAction]
    public async Task<LoginOutput> CreateToken(SysUserInfo user)
    {
        // 单用户登录
        await _sysOnlineUserService.SingleLogin(user.Id);
        //获取用户角色
        var roleNames=await _db.Ado.SqlQueryAsync<string>($"select r.NAME  from roles r INNER JOIN USERROLES u ON r.ID =u.ROLEID  WHERE u.USERID  ='{user.Id}'");
        // 生成Token令牌
        var tokenExpire = 120;
        var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
        {
            { ClaimConst.UserId, user.Id },
            { ClaimConst.TenantId, "" },
            { ClaimConst.Account, user.UserName },
            { ClaimConst.RealName, user.DisplayName },
            { ClaimConst.AccountType, user.Type },
            { ClaimConst.OrgCode,$"{user.Tljdm}|{user.Jwddm}|{user.Cjdm}" },
            { ClaimConst.OrgId,"" },
            { ClaimConst.OrgName,"" },
            { ClaimConst.OrgType, user.Type},
            { ClaimConst.Roles, string.Join(',',roleNames)}
        }, tokenExpire);

        // 生成刷新Token令牌
        var refreshTokenExpire = 60;
        var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken, refreshTokenExpire);

        // 设置响应报文头
        _httpContextAccessor.HttpContext.SetTokensOfResponseHeaders(accessToken, refreshToken);

        // Swagger Knife4UI-AfterScript登录脚本
        // ke.global.setAllHeader('Authorization', 'Bearer ' + ke.response.headers['access-token']);

        return new LoginOutput
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    /// <summary>
    /// 获取登录账号
    /// </summary>
    /// <returns></returns>
    [DisplayName("获取登录账号")]
    public async Task<LoginUserOutput> GetUserInfo()
    {
        var user = await _sysUserRep.GetFirstAsync(u => u.Id == _userManager.UserId) ?? throw Oops.Oh(ErrorCodeEnum.D1011).StatusCode(401);
        // 获取机构
        var org = await _sysUserRep.ChangeRepository<SqlSugarRepository<SysOrg>>().GetFirstAsync(u => u.Id == 0);
        // 获取职位
        var pos = await _sysUserRep.ChangeRepository<SqlSugarRepository<SysPos>>().GetFirstAsync(u => u.Id == 1);
        // 获取拥有按钮权限集合
        var buttons = await _sysMenuService.GetOwnBtnPermList();

        return new LoginUserOutput
        {
            Id = user.Id,
            Account = user.UserName,
            RealName = user.DisplayName,
            AccountType = AccountTypeEnum.SysAdmin,
            Avatar = "",
            Address = "",
            Signature = "",
            OrgId =0,
            OrgCode=$"{user.Tljdm}|{user.Jwddm}|{user.Cjdm}",
            OrgName = org?.Name,
            OrgType = org?.Type,
            PosName = pos?.Name,
            Buttons = buttons
        };
    }

    /// <summary>
    /// 获取刷新Token
    /// </summary>
    /// <param name="accessToken"></param>
    /// <returns></returns>
    [DisplayName("获取刷新Token")]
    public string GetRefreshToken([FromQuery] string accessToken)
    {
        var refreshTokenExpire = _sysConfigService.GetRefreshTokenExpire().GetAwaiter().GetResult();
        return JWTEncryption.GenerateRefreshToken(accessToken, refreshTokenExpire);
    }

    /// <summary>
    /// 退出系统
    /// </summary>
    [DisplayName("退出系统")]
    [HttpPost("api/Account/Logout")]
    public IActionResult Logout()
    {
        if (string.IsNullOrWhiteSpace(_userManager.Account))
            throw Oops.Oh(ErrorCodeEnum.D1011);

        _httpContextAccessor.HttpContext.SignoutToSwagger();
        return new OkResult();
    }

    /// <summary>
    /// 获取登录配置
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [SuppressMonitor]
    [DisplayName("获取登录配置")]
    public async Task<dynamic> GetLoginConfig()
    {
        var secondVerEnabled = await _sysConfigService.GetConfigValue<bool>(CommonConst.SysSecondVer);
        var captchaEnabled = await _sysConfigService.GetConfigValue<bool>(CommonConst.SysCaptcha);
        return new { SecondVerEnabled = secondVerEnabled, CaptchaEnabled = captchaEnabled };
    }

    /// <summary>
    /// 获取水印配置
    /// </summary>
    /// <returns></returns>
    [SuppressMonitor]
    [DisplayName("获取水印配置")]
    public async Task<dynamic> GetWatermarkConfig()
    {
        var watermarkEnabled = await _sysConfigService.GetConfigValue<bool>(CommonConst.SysWatermark);
        return new { WatermarkEnabled = watermarkEnabled };
    }

    /// <summary>
    /// 获取验证码
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [SuppressMonitor]
    [DisplayName("获取验证码")]
    public dynamic GetCaptcha()
    {
        var codeId = YitIdHelper.NextId().ToString();
        var captcha = _captcha.Generate(codeId);
        return new { Id = codeId, Img = captcha.Base64 };
    }
    /// <summary>
    /// 获取角色
    /// </summary>
    /// <returns></returns>
    [HttpGet("api/values/Get")]
    public dynamic GetRoles()
    {
        var user=App.User.Claims;
        var userName = App.User?.FindFirst(ClaimConst.Account)?.Value;
        var lstRoles = new List<Roles>();
        if (userName.Equals("system", StringComparison.CurrentCultureIgnoreCase) ||
            userName.Equals("admin", StringComparison.CurrentCultureIgnoreCase))
        {
            lstRoles.AddRange(_roles.AsQueryable().ToList());
        }
        else
        {
            var r=_db.SqlQueryable<Roles>(string.Format(@"select rl.* from roles rl inner join
                          userroles ur on rl.id = ur.roleid inner join users us on on ur.userid = us.id where u.username='{0}'", userName)).ToList();
            lstRoles.AddRange(r);
        }
        var userInfo = _db.Ado.SqlQuery<SysUserInfo>($"select * from users where username=@UserName",new { UserName= userName });
        return  new { name = userName, user = userInfo.FirstOrDefault(), roles = lstRoles, avatar = "" };
    }
    /// <summary>
    /// 获取菜单
    /// </summary>
    /// <returns></returns>
    [HttpGet("api/Menu/FindSideBarMenu")]
    public dynamic FindSideBarMenu()
    {
        var user = App.User.Claims;
        var userName = App.User?.FindFirst(ClaimConst.Account)?.Value;
        var lstRoles = new List<Roles>();
        var items = FindMenu(userName);
        return items;
    }
    [NonAction]
    public List<dynamic> FindMenu(string username)
    {
        List<dynamic> rootTrees = new List<dynamic>();//跟节点
        List<Menu> menus = GetMenus(username).OrderBy(p => p.Ord).Distinct().ToList();
        List<Menu> parentmenus = menus.Where(p => p.Parentid == null).OrderBy(p => p.Ord).ToList();
        foreach (Menu menu in parentmenus)
        {
            dynamic rootTree1 = new
            {
                id = menu.Id,
                path = menu.Path,
                component = menu.Component,
                // redirct = "/xtgl/qxgl",
                name = menu.Name,
                hidden = !menu.Enabled,
                meta = new
                {
                    title = menu.Title,
                    icon = menu.Icon,
                    noCache = Convert.ToBoolean(menu.NoCache),
                    keepAlive = true
                }
            };
            dynamic rootTree = new
            {
                id = menu.Id,
                path = menu.Path,
                component = menu.Component,
                // redirct = "/xtgl/qxgl",
                name = menu.Name,
                hidden = !menu.Enabled,
                meta = new
                {
                    title = menu.Title,
                    icon = menu.Icon,
                    noCache = Convert.ToBoolean(menu.NoCache),
                    keepAlive = true
                },
                children = Recursion(rootTree1, menus)
            };
            rootTrees.Add(rootTree);
        }
        return rootTrees;
    }
    [NonAction]
    private List<Menu> GetMenus(string username)
    {
        var user = App.User.Claims;
        var userid = App.User?.FindFirst(ClaimConst.UserId)?.Value;
        // 得到登录用户对应的权限id
        var userroleids=_db.Ado.SqlQuery<int>($"select roleid from userroles where userid='{userid}'");
        // 得到权限id对应的菜单id
        IList<int> menuids = _roles.AsQueryable().Where(p => userroleids.Contains(p.Id)).Select(p => p.MenuId).Distinct().ToList();
        // 内置超级用户system和admin
        var menus = _db.Queryable<Menu>();
        List<int> allPar = menus.Where(p => p.Parentid != null).Select(p => p.Parentid.Value).ToList();
        if (username.Equals("system"))
        {
            menuids = menus.Where(p => !allPar.Contains(p.Id)).Select(p => p.Id).ToList();
        }
        else if (username.Equals("admin"))
        {
            menuids = menus.Where(p => !allPar.Contains(p.Id) && !p.Name.Equals("menu")).Select(p => p.Id).ToList();
        }
        //List<Menu> menus = MenuRepository.FindAll().ToList();
        List<Menu> resultMenus = new List<Menu>();
        // 得到根据权限可以显示的菜单数据
        return QueryRoleMenus(resultMenus, _db.Queryable<Menu>().ToList(), menuids);
    }
    [NonAction]
    private List<Menu> QueryRoleMenus(List<Menu> resultMenus, IList<Menu> menus, IList<int> menuIds)
    {
        IList<Menu> resultMenu = menus.Where(p => menuIds.Contains(p.Id)).ToList();
        resultMenus.AddRange(resultMenu);
        IList<int> parMenuIds = resultMenu.Where(p => p.Parentid != null).Select(p => p.Parentid.Value).ToList();
        if (parMenuIds == null || parMenuIds.Count == 0) return resultMenus;
        QueryRoleMenus(resultMenus, menus, parMenuIds);
        return resultMenus;
    }

    public List<dynamic> Recursion(dynamic parentTree, IList<Menu> menus)
    {
        List<dynamic> childtrees = new List<dynamic>();
        var childmenus = menus.Where(p => p.Parentid == parentTree.id);
        if (childmenus == null || childmenus.Count() == 0) return childtrees;
        foreach (Menu childmenu in childmenus)
        {
            dynamic childTree;
            // 在最底层添加role,
            if (menus.Where(p => p.Parentid == childmenu.Id).Count() == 0)
            {
                childTree = new
                {
                    id = childmenu.Id,
                    path = childmenu.Path,
                    component = childmenu.Component,
                    name = childmenu.Name,
                    hidden = !childmenu.Enabled,
                    meta = new
                    {
                        title = childmenu.Title,
                        noCache = Convert.ToBoolean(childmenu.NoCache),
                        keepAlive = true
                    }
                };
            }
            else
            {
                var childTree1 = new
                {
                    id = childmenu.Id,
                    path = childmenu.Path,
                    component = childmenu.Component,
                    // redirct = "/xtgl/qxgl",
                    name = childmenu.Name,
                    hidden = !childmenu.Enabled,
                    meta = new
                    {
                        title = childmenu.Title,
                        icon = childmenu.Icon,
                        noCache = Convert.ToBoolean(childmenu.NoCache),
                        keepAlive = true
                    },
                };
                childTree = new
                {
                    id = childmenu.Id,
                    path = childmenu.Path,
                    component = childmenu.Component,
                    // redirct = "/xtgl/qxgl",
                    name = childmenu.Name,
                    hidden = !childmenu.Enabled,
                    meta = new
                    {
                        title = childmenu.Title,
                        icon = childmenu.Icon,
                        noCache = Convert.ToBoolean(childmenu.NoCache),
                        keepAlive = true
                    },
                    children = Recursion(childTree1, menus)
                };
            }
            childtrees.Add(childTree);
        }
        return childtrees;
    }
    [QueryParameters]
    [HttpGet("api/TjJ2fb/QueryTj_dat_j2fb")]
    public dynamic QueryTj_dat_j2fb([FromQuery] string param)
    {
        try
        {
            JObject jo = JObject.Parse(param);
            string date = jo.Value<string>("DateTime");
            string dw = jo.Value<string>("Dwdm");
            string startDate = date + "0101";
            string endDate = date + "1231";
            IList<Jt2fb> zzdglList;
            IList<Jt2fb> zzxList;
            IList<Jt2fb> jssdList;
            IList<Jt2fb> pjqyzzList;
            List<Jt2fb> Jt2 = new List<Jt2fb>();
            //if (dw == "P")
            //{
            //    //string sql= $"select sum(zzdgl) Zzdgl,yszb,substr(xtrq,0,7) Month from tj_dat_j2fb where fqbs between to_date('{startDate}','yyyymmdd')  and to_date('{endDate}','yyyymmdd') and ddm like 'P%' and yszb in ('HY','XB','KY') group by yszb,substr(xtrq,0,7) order by yszb";
            //    zzdglList = TjJ2fbService.TjJ2fbRepository.FindByCondition<Jt2fb>($"select sum(zzdgl) Zzdgl,yszb,substr(xtrq,0,7) Month from tj_dat_j2fb where fqbs between to_date('{startDate}','yyyymmdd')  and to_date('{endDate}','yyyymmdd') and ddm like 'P%' and yszb in ('HY','XB','KY') group by yszb,substr(xtrq,0,7) order by yszb");
            //    zzxList   = TjJ2fbService.TjJ2fbRepository.FindByCondition<Jt2fb>($"select sum(zzx)   Zzx,  yszb,substr(xtrq,0,7) Month from tj_dat_j2fb where fqbs between to_date('{startDate}','yyyymmdd')  and to_date('{endDate}','yyyymmdd') and ddm like 'P%' and yszb in ('HY','XB','KY') group by yszb,substr(xtrq,0,7) order by yszb");
            //    jssdList = TjJ2fbService.TjJ2fbRepository.FindByCondition<Jt2fb>($"select  case when sum(bwcyz) > 0 then round(sum(bwgl) / sum(bwcyz), 1) else 0 end jssd,  yszb,substr(xtrq,0,7) Month from tj_dat_j2fb where fqbs between to_date('{startDate}','yyyymmdd')  and to_date('{endDate}','yyyymmdd') and ddm like 'P%' and yszb in ('HY','XB','KY') group by yszb,substr(xtrq,0,7) order by yszb");
            //    pjqyzzList = TjJ2fbService.TjJ2fbRepository.FindByCondition<Jt2fb>($"select case when sum(bwgl) > 0 then round(sum(zzdgl_bfpbbd) * 1000.0 / sum(bwgl), 0) else 0 end lcpjzz,  yszb,substr(xtrq,0,7) Month from tj_dat_j2fb where fqbs between to_date('{startDate}','yyyymmdd')  and to_date('{endDate}','yyyymmdd') and ddm like 'P%' and yszb in ('HY','XB','KY') group by yszb,substr(xtrq,0,7) order by yszb");

            //}
            //else
            //{
            //    zzdglList = TjJ2fbService.TjJ2fbRepository.FindByCondition<Jt2fb>($"select sum(zzdgl) Zzdgl,yszb,substr(xtrq,0,7) Month from tj_dat_j2fb where fqbs between to_date('{startDate}','yyyymmdd')  and to_date('{endDate}','yyyymmdd') and ddm= '{dw}' and yszb in ('HY','XB','KY') group by yszb,substr(xtrq,0,7) order by yszb");
            //    zzxList = TjJ2fbService.TjJ2fbRepository.FindByCondition<Jt2fb>($"select sum(Zzx) Zzx,yszb,substr(xtrq,0,7) Month from tj_dat_j2fb where fqbs between to_date('{startDate}','yyyymmdd')  and to_date('{endDate}','yyyymmdd') and ddm= '{dw}' and yszb in ('HY','XB','KY') group by yszb,substr(xtrq,0,7) order by yszb");
            //    jssdList = TjJ2fbService.TjJ2fbRepository.FindByCondition<Jt2fb>($"select  case when sum(bwcyz) > 0 then round(sum(bwgl) / sum(bwcyz), 1) else 0 end jssd,  yszb,substr(xtrq,0,7) Month from tj_dat_j2fb where fqbs between to_date('{startDate}','yyyymmdd')  and to_date('{endDate}','yyyymmdd') and ddm= '{dw}' and yszb in ('HY','XB','KY') group by yszb,substr(xtrq,0,7) order by yszb");
            //    pjqyzzList = TjJ2fbService.TjJ2fbRepository.FindByCondition<Jt2fb>($"select case when sum(bwgl) > 0 then round(sum(zzdgl_bfpbbd) * 1000.0 / sum(bwgl), 0) else 0 end lcpjzz,  yszb,substr(xtrq,0,7) Month from tj_dat_j2fb where fqbs between to_date('{startDate}','yyyymmdd')  and to_date('{endDate}','yyyymmdd') and ddm= '{dw}' and yszb in ('HY','XB','KY') group by yszb,substr(xtrq,0,7) order by yszb");
            //}
            string ddmWhere = null;
            if (dw == "")
            {
                ddmWhere = " ddm like 'P%' and ddm!='P39'";
            }
            else if (dw == "P")
            {
                ddmWhere = " ddm like 'P%' and ddm!='P39' and ddm!='PE5' and ddm!='PE8' and ddm!='PEB'";
            }
            else
            {
                ddmWhere = $@" ddm='{dw}'";
            }
            ddmWhere += " and jclx in ('D','N')  and znd in ('GT','HZ','FK','DT')";
            string sql = $"select sum(zzdgl) Zzdgl,yszb,substr(xtrq,0,7) Month from tj_dat_j2fb where fqbs between to_date('{startDate}','yyyymmdd')  and to_date('{endDate}','yyyymmdd') and {ddmWhere} and yszb in ('HY','XB','KY') group by yszb,substr(xtrq,0,7) order by yszb";
            zzdglList = _db.Ado.SqlQuery<Jt2fb>(sql);
            zzxList = _db.Ado.SqlQuery<Jt2fb>($"select sum(zzx)   Zzx,  yszb,substr(xtrq,0,7) Month from tj_dat_j2fb where fqbs between to_date('{startDate}','yyyymmdd')  and to_date('{endDate}','yyyymmdd') and {ddmWhere} and yszb in ('HY','XB','KY') group by yszb,substr(xtrq,0,7) order by yszb");
            jssdList = _db.Ado.SqlQuery<Jt2fb>($"select  case when sum(bwcyz) > 0 then round(sum(bwgl) / sum(bwcyz), 1) else 0 end jssd,  yszb,substr(xtrq,0,7) Month from tj_dat_j2fb where fqbs between to_date('{startDate}','yyyymmdd')  and to_date('{endDate}','yyyymmdd') and {ddmWhere} and yszb in ('HY','XB','KY') group by yszb,substr(xtrq,0,7) order by yszb");
            pjqyzzList = _db.Ado.SqlQuery<Jt2fb>($"select case when sum(yxzx_bbbbd) > 0 then round(sum(zzdgl_fpbbbbd) * 1000.0 / sum(yxzx_bbbbd), 0) else 0 end lcpjzz,  yszb,substr(xtrq,0,7) Month from tj_dat_j2fb where fqbs between to_date('{startDate}','yyyymmdd')  and to_date('{endDate}','yyyymmdd') and {ddmWhere} and yszb in ('HY','XB','KY') group by yszb,substr(xtrq,0,7) order by yszb");
            Jt2fb jt2zzdglKy = new Jt2fb();
            jt2zzdglKy.Yszb = "KY";
            jt2zzdglKy.Lb = "总重吨公里";
            jt2zzdglKy.Month1 = zzdglList.Where(o => o.Month.Equals(date + "-01") && o.Yszb.Equals("KY")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglKy.Month2 = zzdglList.Where(o => o.Month.Equals(date + "-02") && o.Yszb.Equals("KY")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglKy.Month3 = zzdglList.Where(o => o.Month.Equals(date + "-03") && o.Yszb.Equals("KY")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglKy.Month4 = zzdglList.Where(o => o.Month.Equals(date + "-04") && o.Yszb.Equals("KY")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglKy.Month5 = zzdglList.Where(o => o.Month.Equals(date + "-05") && o.Yszb.Equals("KY")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglKy.Month6 = zzdglList.Where(o => o.Month.Equals(date + "-06") && o.Yszb.Equals("KY")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglKy.Month7 = zzdglList.Where(o => o.Month.Equals(date + "-07") && o.Yszb.Equals("KY")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglKy.Month8 = zzdglList.Where(o => o.Month.Equals(date + "-08") && o.Yszb.Equals("KY")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglKy.Month9 = zzdglList.Where(o => o.Month.Equals(date + "-09") && o.Yszb.Equals("KY")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglKy.Month10 = zzdglList.Where(o => o.Month.Equals(date + "-10") && o.Yszb.Equals("KY")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglKy.Month11 = zzdglList.Where(o => o.Month.Equals(date + "-11") && o.Yszb.Equals("KY")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglKy.Month12 = zzdglList.Where(o => o.Month.Equals(date + "-12") && o.Yszb.Equals("KY")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglKy.MonthHj = jt2zzdglKy.Month1 + jt2zzdglKy.Month2 + jt2zzdglKy.Month3 + jt2zzdglKy.Month4 + jt2zzdglKy.Month5 + jt2zzdglKy.Month6 + jt2zzdglKy.Month7 + jt2zzdglKy.Month8 + jt2zzdglKy.Month9
                + jt2zzdglKy.Month10 + jt2zzdglKy.Month11 + jt2zzdglKy.Month12;
            Jt2fb jt2zzdglHy = new Jt2fb();
            jt2zzdglHy.Yszb = "HY";
            jt2zzdglHy.Lb = "总重吨公里";
            jt2zzdglHy.Month1 = zzdglList.Where(o => o.Month.Equals(date + "-01") && o.Yszb.Equals("HY")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglHy.Month2 = zzdglList.Where(o => o.Month.Equals(date + "-02") && o.Yszb.Equals("HY")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglHy.Month3 = zzdglList.Where(o => o.Month.Equals(date + "-03") && o.Yszb.Equals("HY")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglHy.Month4 = zzdglList.Where(o => o.Month.Equals(date + "-04") && o.Yszb.Equals("HY")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglHy.Month5 = zzdglList.Where(o => o.Month.Equals(date + "-05") && o.Yszb.Equals("HY")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglHy.Month6 = zzdglList.Where(o => o.Month.Equals(date + "-06") && o.Yszb.Equals("HY")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglHy.Month7 = zzdglList.Where(o => o.Month.Equals(date + "-07") && o.Yszb.Equals("HY")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglHy.Month8 = zzdglList.Where(o => o.Month.Equals(date + "-08") && o.Yszb.Equals("HY")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglHy.Month9 = zzdglList.Where(o => o.Month.Equals(date + "-09") && o.Yszb.Equals("HY")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglHy.Month10 = zzdglList.Where(o => o.Month.Equals(date + "-10") && o.Yszb.Equals("HY")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglHy.Month11 = zzdglList.Where(o => o.Month.Equals(date + "-11") && o.Yszb.Equals("HY")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglHy.Month12 = zzdglList.Where(o => o.Month.Equals(date + "-12") && o.Yszb.Equals("HY")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglHy.MonthHj = jt2zzdglHy.Month1 + jt2zzdglHy.Month2 + jt2zzdglHy.Month3 + jt2zzdglHy.Month4 + jt2zzdglHy.Month5 + jt2zzdglHy.Month6 + jt2zzdglHy.Month7 + jt2zzdglHy.Month8 + jt2zzdglHy.Month9
                + jt2zzdglHy.Month10 + jt2zzdglHy.Month11 + jt2zzdglHy.Month12;
            Jt2fb jt2zzdglXb = new Jt2fb();
            jt2zzdglXb.Yszb = "XB";
            jt2zzdglXb.Lb = "总重吨公里";
            jt2zzdglXb.Month1 = zzdglList.Where(o => o.Month.Equals(date + "-01") && o.Yszb.Equals("XB")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglXb.Month2 = zzdglList.Where(o => o.Month.Equals(date + "-02") && o.Yszb.Equals("XB")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglXb.Month3 = zzdglList.Where(o => o.Month.Equals(date + "-03") && o.Yszb.Equals("XB")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglXb.Month4 = zzdglList.Where(o => o.Month.Equals(date + "-04") && o.Yszb.Equals("XB")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglXb.Month5 = zzdglList.Where(o => o.Month.Equals(date + "-05") && o.Yszb.Equals("XB")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglXb.Month6 = zzdglList.Where(o => o.Month.Equals(date + "-06") && o.Yszb.Equals("XB")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglXb.Month7 = zzdglList.Where(o => o.Month.Equals(date + "-07") && o.Yszb.Equals("XB")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglXb.Month8 = zzdglList.Where(o => o.Month.Equals(date + "-08") && o.Yszb.Equals("XB")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglXb.Month9 = zzdglList.Where(o => o.Month.Equals(date + "-09") && o.Yszb.Equals("XB")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglXb.Month10 = zzdglList.Where(o => o.Month.Equals(date + "-10") && o.Yszb.Equals("XB")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglXb.Month11 = zzdglList.Where(o => o.Month.Equals(date + "-11") && o.Yszb.Equals("XB")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglXb.Month12 = zzdglList.Where(o => o.Month.Equals(date + "-12") && o.Yszb.Equals("XB")).Select(o => o.Zzdgl).FirstOrDefault();
            jt2zzdglXb.MonthHj = jt2zzdglXb.Month1 + jt2zzdglXb.Month2 + jt2zzdglXb.Month3 + jt2zzdglXb.Month4 + jt2zzdglXb.Month5 + jt2zzdglXb.Month6 + jt2zzdglXb.Month7 + jt2zzdglXb.Month8 + jt2zzdglXb.Month9
                + jt2zzdglXb.Month10 + jt2zzdglXb.Month11 + jt2zzdglXb.Month12;
            Jt2.Add(jt2zzdglHy);
            Jt2.Add(jt2zzdglKy);
            Jt2.Add(jt2zzdglXb);
            Jt2fb jt2zzxKy = new Jt2fb();
            jt2zzxKy.Yszb = "KY";
            jt2zzxKy.Lb = "总走行";
            jt2zzxKy.Month1 = zzxList.Where(o => o.Month.Equals(date + "-01") && o.Yszb.Equals("KY")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxKy.Month2 = zzxList.Where(o => o.Month.Equals(date + "-02") && o.Yszb.Equals("KY")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxKy.Month3 = zzxList.Where(o => o.Month.Equals(date + "-03") && o.Yszb.Equals("KY")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxKy.Month4 = zzxList.Where(o => o.Month.Equals(date + "-04") && o.Yszb.Equals("KY")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxKy.Month5 = zzxList.Where(o => o.Month.Equals(date + "-05") && o.Yszb.Equals("KY")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxKy.Month6 = zzxList.Where(o => o.Month.Equals(date + "-06") && o.Yszb.Equals("KY")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxKy.Month7 = zzxList.Where(o => o.Month.Equals(date + "-07") && o.Yszb.Equals("KY")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxKy.Month8 = zzxList.Where(o => o.Month.Equals(date + "-08") && o.Yszb.Equals("KY")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxKy.Month9 = zzxList.Where(o => o.Month.Equals(date + "-09") && o.Yszb.Equals("KY")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxKy.Month10 = zzxList.Where(o => o.Month.Equals(date + "-10") && o.Yszb.Equals("KY")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxKy.Month11 = zzxList.Where(o => o.Month.Equals(date + "-11") && o.Yszb.Equals("KY")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxKy.Month12 = zzxList.Where(o => o.Month.Equals(date + "-12") && o.Yszb.Equals("KY")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxKy.MonthHj = jt2zzxKy.Month1 + jt2zzxKy.Month2 + jt2zzxKy.Month3 + jt2zzxKy.Month4 + jt2zzxKy.Month5 + jt2zzxKy.Month6 + jt2zzxKy.Month7 + jt2zzxKy.Month8 + jt2zzxKy.Month9
                + jt2zzxKy.Month10 + jt2zzxKy.Month11 + jt2zzxKy.Month12;
            Jt2fb jt2zzxHy = new Jt2fb();
            jt2zzxHy.Yszb = "HY";
            jt2zzxHy.Lb = "总走行";
            jt2zzxHy.Month1 = zzxList.Where(o => o.Month.Equals(date + "-01") && o.Yszb.Equals("HY")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxHy.Month2 = zzxList.Where(o => o.Month.Equals(date + "-02") && o.Yszb.Equals("HY")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxHy.Month3 = zzxList.Where(o => o.Month.Equals(date + "-03") && o.Yszb.Equals("HY")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxHy.Month4 = zzxList.Where(o => o.Month.Equals(date + "-04") && o.Yszb.Equals("HY")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxHy.Month5 = zzxList.Where(o => o.Month.Equals(date + "-05") && o.Yszb.Equals("HY")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxHy.Month6 = zzxList.Where(o => o.Month.Equals(date + "-06") && o.Yszb.Equals("HY")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxHy.Month7 = zzxList.Where(o => o.Month.Equals(date + "-07") && o.Yszb.Equals("HY")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxHy.Month8 = zzxList.Where(o => o.Month.Equals(date + "-08") && o.Yszb.Equals("HY")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxHy.Month9 = zzxList.Where(o => o.Month.Equals(date + "-09") && o.Yszb.Equals("HY")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxHy.Month10 = zzxList.Where(o => o.Month.Equals(date + "-10") && o.Yszb.Equals("HY")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxHy.Month11 = zzxList.Where(o => o.Month.Equals(date + "-11") && o.Yszb.Equals("HY")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxHy.Month12 = zzxList.Where(o => o.Month.Equals(date + "-12") && o.Yszb.Equals("HY")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxHy.MonthHj = jt2zzxHy.Month1 + jt2zzxHy.Month2 + jt2zzxHy.Month3 + jt2zzxHy.Month4 + jt2zzxHy.Month5 + jt2zzxHy.Month6 + jt2zzxHy.Month7 + jt2zzxHy.Month8 + jt2zzxHy.Month9
                + jt2zzxHy.Month10 + jt2zzxHy.Month11 + jt2zzxHy.Month12;
            Jt2fb jt2zzxXb = new Jt2fb();
            jt2zzxXb.Yszb = "XB";
            jt2zzxXb.Lb = "总走行";
            jt2zzxXb.Month1 = zzxList.Where(o => o.Month.Equals(date + "-01") && o.Yszb.Equals("XB")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxXb.Month2 = zzxList.Where(o => o.Month.Equals(date + "-02") && o.Yszb.Equals("XB")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxXb.Month3 = zzxList.Where(o => o.Month.Equals(date + "-03") && o.Yszb.Equals("XB")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxXb.Month4 = zzxList.Where(o => o.Month.Equals(date + "-04") && o.Yszb.Equals("XB")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxXb.Month5 = zzxList.Where(o => o.Month.Equals(date + "-05") && o.Yszb.Equals("XB")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxXb.Month6 = zzxList.Where(o => o.Month.Equals(date + "-06") && o.Yszb.Equals("XB")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxXb.Month7 = zzxList.Where(o => o.Month.Equals(date + "-07") && o.Yszb.Equals("XB")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxXb.Month8 = zzxList.Where(o => o.Month.Equals(date + "-08") && o.Yszb.Equals("XB")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxXb.Month9 = zzxList.Where(o => o.Month.Equals(date + "-09") && o.Yszb.Equals("XB")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxXb.Month10 = zzxList.Where(o => o.Month.Equals(date + "-10") && o.Yszb.Equals("XB")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxXb.Month11 = zzxList.Where(o => o.Month.Equals(date + "-11") && o.Yszb.Equals("XB")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxXb.Month12 = zzxList.Where(o => o.Month.Equals(date + "-12") && o.Yszb.Equals("XB")).Select(o => o.Zzx).FirstOrDefault();
            jt2zzxXb.MonthHj = jt2zzxXb.Month1 + jt2zzxXb.Month2 + jt2zzxXb.Month3 + jt2zzxXb.Month4 + jt2zzxXb.Month5 + jt2zzxXb.Month6 + jt2zzxXb.Month7 + jt2zzxXb.Month8 + jt2zzxXb.Month9
                + jt2zzxXb.Month10 + jt2zzxXb.Month11 + jt2zzxXb.Month12;
            Jt2.Add(jt2zzxHy);
            Jt2.Add(jt2zzxKy);
            Jt2.Add(jt2zzxXb);
            Jt2fb jt2jssdKy = new Jt2fb();
            jt2jssdKy.Yszb = "KY";
            jt2jssdKy.Lb = "技术速度";
            jt2jssdKy.Month1 = jssdList.Where(o => o.Month.Equals(date + "-01") && o.Yszb.Equals("KY")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdKy.Month2 = jssdList.Where(o => o.Month.Equals(date + "-02") && o.Yszb.Equals("KY")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdKy.Month3 = jssdList.Where(o => o.Month.Equals(date + "-03") && o.Yszb.Equals("KY")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdKy.Month4 = jssdList.Where(o => o.Month.Equals(date + "-04") && o.Yszb.Equals("KY")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdKy.Month5 = jssdList.Where(o => o.Month.Equals(date + "-05") && o.Yszb.Equals("KY")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdKy.Month6 = jssdList.Where(o => o.Month.Equals(date + "-06") && o.Yszb.Equals("KY")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdKy.Month7 = jssdList.Where(o => o.Month.Equals(date + "-07") && o.Yszb.Equals("KY")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdKy.Month8 = jssdList.Where(o => o.Month.Equals(date + "-08") && o.Yszb.Equals("KY")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdKy.Month9 = jssdList.Where(o => o.Month.Equals(date + "-09") && o.Yszb.Equals("KY")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdKy.Month10 = jssdList.Where(o => o.Month.Equals(date + "-10") && o.Yszb.Equals("KY")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdKy.Month11 = jssdList.Where(o => o.Month.Equals(date + "-11") && o.Yszb.Equals("KY")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdKy.Month12 = jssdList.Where(o => o.Month.Equals(date + "-12") && o.Yszb.Equals("KY")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdKy.MonthHj = jt2jssdKy.Month1 + jt2jssdKy.Month2 + jt2jssdKy.Month3 + jt2jssdKy.Month4 + jt2jssdKy.Month5 + jt2jssdKy.Month6 + jt2jssdKy.Month7 + jt2jssdKy.Month8 + jt2jssdKy.Month9
                + jt2jssdKy.Month10 + jt2jssdKy.Month11 + jt2jssdKy.Month12;
            Jt2fb jt2jssdHy = new Jt2fb();
            jt2jssdHy.Yszb = "HY";
            jt2jssdHy.Lb = "技术速度";
            jt2jssdHy.Month1 = jssdList.Where(o => o.Month.Equals(date + "-01") && o.Yszb.Equals("HY")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdHy.Month2 = jssdList.Where(o => o.Month.Equals(date + "-02") && o.Yszb.Equals("HY")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdHy.Month3 = jssdList.Where(o => o.Month.Equals(date + "-03") && o.Yszb.Equals("HY")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdHy.Month4 = jssdList.Where(o => o.Month.Equals(date + "-04") && o.Yszb.Equals("HY")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdHy.Month5 = jssdList.Where(o => o.Month.Equals(date + "-05") && o.Yszb.Equals("HY")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdHy.Month6 = jssdList.Where(o => o.Month.Equals(date + "-06") && o.Yszb.Equals("HY")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdHy.Month7 = jssdList.Where(o => o.Month.Equals(date + "-07") && o.Yszb.Equals("HY")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdHy.Month8 = jssdList.Where(o => o.Month.Equals(date + "-08") && o.Yszb.Equals("HY")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdHy.Month9 = jssdList.Where(o => o.Month.Equals(date + "-09") && o.Yszb.Equals("HY")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdHy.Month10 = jssdList.Where(o => o.Month.Equals(date + "-10") && o.Yszb.Equals("HY")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdHy.Month11 = jssdList.Where(o => o.Month.Equals(date + "-11") && o.Yszb.Equals("HY")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdHy.Month12 = jssdList.Where(o => o.Month.Equals(date + "-12") && o.Yszb.Equals("HY")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdHy.MonthHj = jt2jssdHy.Month1 + jt2jssdHy.Month2 + jt2jssdHy.Month3 + jt2jssdHy.Month4 + jt2jssdHy.Month5 + jt2jssdHy.Month6 + jt2jssdHy.Month7 + jt2jssdHy.Month8 + jt2jssdHy.Month9
                + jt2jssdHy.Month10 + jt2jssdHy.Month11 + jt2jssdHy.Month12;
            Jt2fb jt2jssdXb = new Jt2fb();
            jt2jssdXb.Yszb = "XB";
            jt2jssdXb.Lb = "技术速度";
            jt2jssdXb.Month1 = jssdList.Where(o => o.Month.Equals(date + "-01") && o.Yszb.Equals("XB")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdXb.Month2 = jssdList.Where(o => o.Month.Equals(date + "-02") && o.Yszb.Equals("XB")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdXb.Month3 = jssdList.Where(o => o.Month.Equals(date + "-03") && o.Yszb.Equals("XB")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdXb.Month4 = jssdList.Where(o => o.Month.Equals(date + "-04") && o.Yszb.Equals("XB")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdXb.Month5 = jssdList.Where(o => o.Month.Equals(date + "-05") && o.Yszb.Equals("XB")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdXb.Month6 = jssdList.Where(o => o.Month.Equals(date + "-06") && o.Yszb.Equals("XB")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdXb.Month7 = jssdList.Where(o => o.Month.Equals(date + "-07") && o.Yszb.Equals("XB")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdXb.Month8 = jssdList.Where(o => o.Month.Equals(date + "-08") && o.Yszb.Equals("XB")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdXb.Month9 = jssdList.Where(o => o.Month.Equals(date + "-09") && o.Yszb.Equals("XB")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdXb.Month10 = jssdList.Where(o => o.Month.Equals(date + "-10") && o.Yszb.Equals("XB")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdXb.Month11 = jssdList.Where(o => o.Month.Equals(date + "-11") && o.Yszb.Equals("XB")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdXb.Month12 = jssdList.Where(o => o.Month.Equals(date + "-12") && o.Yszb.Equals("XB")).Select(o => o.Jssd).FirstOrDefault();
            jt2jssdXb.MonthHj = jt2jssdXb.Month1 + jt2jssdXb.Month2 + jt2jssdXb.Month3 + jt2jssdXb.Month4 + jt2jssdXb.Month5 + jt2jssdXb.Month6 + jt2jssdXb.Month7 + jt2jssdXb.Month8 + jt2jssdXb.Month9
                + jt2jssdXb.Month10 + jt2jssdXb.Month11 + jt2jssdXb.Month12;
            Jt2.Add(jt2jssdHy);
            Jt2.Add(jt2jssdKy);
            Jt2.Add(jt2jssdXb);
            Jt2fb jt2pjqyzzKy = new Jt2fb();
            jt2pjqyzzKy.Yszb = "KY";
            jt2pjqyzzKy.Lb = "平均牵引总重";
            jt2pjqyzzKy.Month1 = pjqyzzList.Where(o => o.Month.Equals(date + "-01") && o.Yszb.Equals("KY")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzKy.Month2 = pjqyzzList.Where(o => o.Month.Equals(date + "-02") && o.Yszb.Equals("KY")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzKy.Month3 = pjqyzzList.Where(o => o.Month.Equals(date + "-03") && o.Yszb.Equals("KY")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzKy.Month4 = pjqyzzList.Where(o => o.Month.Equals(date + "-04") && o.Yszb.Equals("KY")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzKy.Month5 = pjqyzzList.Where(o => o.Month.Equals(date + "-05") && o.Yszb.Equals("KY")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzKy.Month6 = pjqyzzList.Where(o => o.Month.Equals(date + "-06") && o.Yszb.Equals("KY")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzKy.Month7 = pjqyzzList.Where(o => o.Month.Equals(date + "-07") && o.Yszb.Equals("KY")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzKy.Month8 = pjqyzzList.Where(o => o.Month.Equals(date + "-08") && o.Yszb.Equals("KY")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzKy.Month9 = pjqyzzList.Where(o => o.Month.Equals(date + "-09") && o.Yszb.Equals("KY")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzKy.Month10 = pjqyzzList.Where(o => o.Month.Equals(date + "-10") && o.Yszb.Equals("KY")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzKy.Month11 = pjqyzzList.Where(o => o.Month.Equals(date + "-11") && o.Yszb.Equals("KY")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzKy.Month12 = pjqyzzList.Where(o => o.Month.Equals(date + "-12") && o.Yszb.Equals("KY")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzKy.MonthHj = jt2pjqyzzKy.Month1 + jt2pjqyzzKy.Month2 + jt2pjqyzzKy.Month3 + jt2pjqyzzKy.Month4 + jt2pjqyzzKy.Month5 + jt2pjqyzzKy.Month6 + jt2pjqyzzKy.Month7 + jt2pjqyzzKy.Month8 + jt2pjqyzzKy.Month9
                + jt2pjqyzzKy.Month10 + jt2pjqyzzKy.Month11 + jt2pjqyzzKy.Month12;
            Jt2fb jt2pjqyzzHy = new Jt2fb();
            jt2pjqyzzHy.Yszb = "HY";
            jt2pjqyzzHy.Lb = "平均牵引总重";
            jt2pjqyzzHy.Month1 = pjqyzzList.Where(o => o.Month.Equals(date + "-01") && o.Yszb.Equals("HY")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzHy.Month2 = pjqyzzList.Where(o => o.Month.Equals(date + "-02") && o.Yszb.Equals("HY")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzHy.Month3 = pjqyzzList.Where(o => o.Month.Equals(date + "-03") && o.Yszb.Equals("HY")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzHy.Month4 = pjqyzzList.Where(o => o.Month.Equals(date + "-04") && o.Yszb.Equals("HY")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzHy.Month5 = pjqyzzList.Where(o => o.Month.Equals(date + "-05") && o.Yszb.Equals("HY")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzHy.Month6 = pjqyzzList.Where(o => o.Month.Equals(date + "-06") && o.Yszb.Equals("HY")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzHy.Month7 = pjqyzzList.Where(o => o.Month.Equals(date + "-07") && o.Yszb.Equals("HY")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzHy.Month8 = pjqyzzList.Where(o => o.Month.Equals(date + "-08") && o.Yszb.Equals("HY")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzHy.Month9 = pjqyzzList.Where(o => o.Month.Equals(date + "-09") && o.Yszb.Equals("HY")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzHy.Month10 = pjqyzzList.Where(o => o.Month.Equals(date + "-10") && o.Yszb.Equals("HY")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzHy.Month11 = pjqyzzList.Where(o => o.Month.Equals(date + "-11") && o.Yszb.Equals("HY")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzHy.Month12 = pjqyzzList.Where(o => o.Month.Equals(date + "-12") && o.Yszb.Equals("HY")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzHy.MonthHj = jt2pjqyzzHy.Month1 + jt2pjqyzzHy.Month2 + jt2pjqyzzHy.Month3 + jt2pjqyzzHy.Month4 + jt2pjqyzzHy.Month5 + jt2pjqyzzHy.Month6 + jt2pjqyzzHy.Month7 + jt2pjqyzzHy.Month8 + jt2pjqyzzHy.Month9
                + jt2pjqyzzHy.Month10 + jt2pjqyzzHy.Month11 + jt2pjqyzzHy.Month12;
            Jt2fb jt2pjqyzzXb = new Jt2fb();
            jt2pjqyzzXb.Yszb = "XB";
            jt2pjqyzzXb.Lb = "平均牵引总重";
            jt2pjqyzzXb.Month1 = pjqyzzList.Where(o => o.Month.Equals(date + "-01") && o.Yszb.Equals("XB")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzXb.Month2 = pjqyzzList.Where(o => o.Month.Equals(date + "-02") && o.Yszb.Equals("XB")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzXb.Month3 = pjqyzzList.Where(o => o.Month.Equals(date + "-03") && o.Yszb.Equals("XB")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzXb.Month4 = pjqyzzList.Where(o => o.Month.Equals(date + "-04") && o.Yszb.Equals("XB")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzXb.Month5 = pjqyzzList.Where(o => o.Month.Equals(date + "-05") && o.Yszb.Equals("XB")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzXb.Month6 = pjqyzzList.Where(o => o.Month.Equals(date + "-06") && o.Yszb.Equals("XB")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzXb.Month7 = pjqyzzList.Where(o => o.Month.Equals(date + "-07") && o.Yszb.Equals("XB")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzXb.Month8 = pjqyzzList.Where(o => o.Month.Equals(date + "-08") && o.Yszb.Equals("XB")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzXb.Month9 = pjqyzzList.Where(o => o.Month.Equals(date + "-09") && o.Yszb.Equals("XB")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzXb.Month10 = pjqyzzList.Where(o => o.Month.Equals(date + "-10") && o.Yszb.Equals("XB")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzXb.Month11 = pjqyzzList.Where(o => o.Month.Equals(date + "-11") && o.Yszb.Equals("XB")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzXb.Month12 = pjqyzzList.Where(o => o.Month.Equals(date + "-12") && o.Yszb.Equals("XB")).Select(o => o.Lcpjzz).FirstOrDefault();
            jt2pjqyzzXb.MonthHj = jt2pjqyzzXb.Month1 + jt2pjqyzzXb.Month2 + jt2pjqyzzXb.Month3 + jt2pjqyzzXb.Month4 + jt2pjqyzzXb.Month5 + jt2pjqyzzXb.Month6 + jt2pjqyzzXb.Month7 + jt2pjqyzzXb.Month8 + jt2pjqyzzXb.Month9
                + jt2pjqyzzXb.Month10 + jt2pjqyzzXb.Month11 + jt2pjqyzzXb.Month12;
            Jt2.Add(jt2pjqyzzHy);
            Jt2.Add(jt2pjqyzzKy);
            Jt2.Add(jt2pjqyzzXb);
            return new { items = Jt2 };
        }
        catch (Exception e)
        {
            return e;
        }
    }

}