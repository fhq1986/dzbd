// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Core.Entity.ElectReport;
using Admin.NET.Core.Service.SystemManage.Dto;
using Admin.NET.Core.Util;
using AlibabaCloud.OpenApiClient.Models;
using DapperExtensions;
using DapperExtensions.Predicate;
using Minio.DataModel;
using Newtonsoft.Json.Linq;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using RazorEngine.Compilation.ImpromptuInterface.InvokeExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service.SystemManage;
/// <summary>
/// 角色管理接口
/// </summary>
[NonUnify]
[ApiDescriptionSettings(Order = 390)]
public class RoleManageService : IDynamicApiController, ITransient
{
    private readonly ISqlSugarClient _db;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public RoleManageService(ISqlSugarClient db)
    {
        _db = db;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpGet]
    [DisplayName("")]
    [Route("/api/Role/FindAll")]
    public dynamic FindAll(string param)
    {
        try
        {
            //PredicateGroup predicateGroup;
            IList<ISort> sorts;
            #region DapperHelper.GeneratePredicateAndSort<Dto.Role> RoleService.FindAll
            //DapperHelper.GeneratePredicateAndSort<Dto.Role>(param, out predicateGroup, out sorts);
            //var cjxxs = RoleService.FindAll(predicateGroup, sorts);
            //Dictionary<string, string> dicWhere = new Dictionary<string, string>();
            //Dictionary<string, bool> dicOrderBy = new Dictionary<string, bool>();
            var query = _db.Queryable<Role>();
            if (!string.IsNullOrWhiteSpace(param))
            {
                List<string> propertyNames = typeof(Role).GetProperties().Select(o => o.Name).ToList();
                JObject jo = JObject.Parse(param);
                IEnumerable<JProperty> properties = jo.Properties();
                List<IConditionalModel> lstWhere = new List<IConditionalModel>();
                foreach (JProperty item in properties.Where(x=> propertyNames.Contains(x.Name)))
                {
                    //dicWhere.Add(property.Name, property.Value + "");
                    lstWhere.Add(new ConditionalModel { FieldName=item.Name,ConditionalType=ConditionalType.Equal,FieldValue=item.Value+""});
                }
                if (!string.IsNullOrEmpty(jo["MenuId"] + "") && Convert.ToInt32(jo["MenuId"]) != 0)
                {
                    lstWhere.Clear();
                    lstWhere.Add(new ConditionalModel() { FieldName = "MenuId", ConditionalType = ConditionalType.Equal, FieldValue = jo["MenuId"] + "" });
                }
                if (lstWhere != null && lstWhere.Count > 0)
                {
                    query = query.Where(lstWhere);
                }
                if (jo.ContainsKey("Sort") && jo["Sort"] is JArray && (jo["Sort"] as JArray).Count > 0)
                {
                    // sorts = jParam["Sort"].ToObject<List<Sort>>() as IList<ISort>;
                    sorts = jo.Value<JArray>("Sort").ToObject<List<Sort>>().ToList<ISort>();
                    foreach (var item in sorts)
                    {
                        if (item.Ascending)
                            query = query.OrderBy($"{item.PropertyName} asc");
                        else
                            query = query.OrderBy($"{item.PropertyName} desc");
                    }
                }
            }
            #endregion
            List<Role> lst = query.ToList();
            return new { success = true, data = new { total = lst.Count(), items = lst } };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 获取所有角色数据
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [DisplayName("获取所有角色数据")]
    [Route("/api/Role/FindRoleAll")]
    public dynamic FindRoleAll()
    {
        try
        {
            //var roles = RoleService.FindAll();
            var roles = _db.Queryable<Role>().ToList();
            return new { success = true, data = new { total = roles.Count(), items = roles } };
        }
        catch (Exception e)
        {
            return new { success = false, message = e.Message };
        }
    }

    /// <summary>
    /// 添加权限
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    [HttpPost]
    //[Authorize(Role = "role_add")]
    [DisplayName("添加权限")]
    [Route("/api/Role/Add")]
    public dynamic Add(Role role)
    {
        try
        {
            //int result = RoleService.Add(role);
            //RoleService.Commit();
            int result = _db.Insertable(role).ExecuteCommand();

            return new { success = true, data = result };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 更改权限
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost]
    //[Authorize(Role = "role_edit")]
    [DisplayName("更改权限")]
    [Route("/api/Role/Update")]
    public dynamic Update(Role role)
    {
        try
        {
            //RoleService.Update(role);
            //RoleService.Commit();
            int result = _db.Updateable(role).ExecuteCommand();
            if(result>0)
                return new { success = true };
            return new { success = false,message="修改失败，请重试" };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    [HttpGet]
    //[Authorize(Roles = "menu_queryall")]
    [DisplayName("获取所有菜单")]
    [Route("/api/Role/FindAllMenu")]
    public dynamic FindAllMenu()
    {
        try
        {
            //var menus = RoleService.FindTree();
            var menus = FindTree();
            int i = menus.Count();
            return new { success = true, data = new { total = menus.Count(), items = menus } };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 删除权限
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    [HttpPost]
    //[Authorize(Role = "role_delete")]
    [DisplayName("删除权限")]
    [Route("/api/Role/Remove")]
    public dynamic Remove([FromBody]Role role)
    {
        try
        {
            //RoleService.Remove(role);
            //RoleService.Commit();
            int result = _db.Deleteable(role).ExecuteCommand();
            if (result > 0)
                return new { success = true };
            return new { success = false, message = "删除失败，请重试" };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 由于目前没有数据访问层，
    /// 那么将之前的项目中的RoleService中的FindTree方法移植到控制器中，
    /// 作为私有方法
    /// </summary>
    /// <returns></returns>
    private IList<TreeNode> FindTree()
    {
        IList<MenuTreeGrid> menus = QueryTreeGrid();
        IList<MenuTreeGrid> menuf = menus.Where(o => o.parent_id == null).ToList();
        IList<MenuTreeGrid> mensz = menus.Where(o => o.parent_id != null).ToList();
        IList<TreeNode> fatrees = new List<TreeNode>();
        IList<TreeNode> trees = new List<TreeNode>();
        foreach (MenuTreeGrid menu in menuf)
        {
            TreeNode fathertree = new TreeNode() { Id = menu.id, NodeCode = menu.Name, NodeName = menu.Title };//父节点
            IList<TreeNode> chidrentree = new List<TreeNode>();//子节点集合
            foreach (MenuTreeGrid mens in mensz)
            {
                if (menu.id.Equals(mens.parent_id))
                {

                    chidrentree.Add(new TreeNode() { Id = (int)mens.id, NodeCode = mens.Name, NodeName = mens.Title, Father = fathertree });
                }
            }
            //chidrentree.Add(new TreeNode() { Id = 110, NodeName = "无交路人员", Father = fathertree });
            TreeNode father = new TreeNode() { Id = menu.id, NodeName = menu.Title, NodeCode = menu.Name, Children = chidrentree };
            trees.Add(father);

        }
        return trees;
    }

    private IList<MenuTreeGrid> QueryTreeGrid()
    {
        IList<MenuTreeGrid> trees = new List<MenuTreeGrid>();
        //IList<Menu> menus = MenuRepository.FindAll().OrderBy(p => p.Ord).ToList();
        IList<Menu> menus = _db.Queryable<Menu>().OrderBy(p => p.Ord).ToList();
        for (int i = 0; i < menus.Count; i++)
        {
            Menu menu = menus[i];
            int childnum = menus.Where(p => p.Parentid == menu.Id).Count();
            int depth = menu.Parentid == null ? 1 : 2;
            MenuTreeGrid tree = new MenuTreeGrid()
            {
                id = menu.Id,
                Title = menu.Title,
                Component = menu.Component,
                Enabled = menu.Enabled,
                Name = menu.Name,
                Icon = menu.Icon,
                Path = menu.Path,
                Roles = menu.Roles,
                Ord = menu.Ord,
                parent_id = menu.Parentid,
                child_num = childnum,
                depth = depth
            };
            trees.Add(tree);
        }
        return trees;
    }
}
