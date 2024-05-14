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
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service.DictionaryManage;
/// <summary>
/// 车间信息管理接口
/// </summary>
[NonUnify]
[ApiDescriptionSettings(Order = 390)]
public class CjjlxxManageService : IDynamicApiController, ITransient
{
    private readonly ISqlSugarClient _db;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public CjjlxxManageService(ISqlSugarClient db)
    {
        _db = db;
    }
    [Route("/api/Cjjlxx/FindAllCjjlxxs")]
    [HttpGet]
    [Authorize(Roles = "cjjlxx_base")]
    public dynamic FindAllCjjlxxs()
    {
        try
        {
            //IList<Cjxx> defaultChecked = CjxxService.FindAllJldms();
            IList<Cjxx> defaultChecked=new List<Cjxx>();
            #region CjxxService.FindAllJldms()
            //IList<Cjxx> cjxxs = new List<Cjxx>();
            //var cjxxes = CjxxRepository.FindAll();
            var cjxxes = _db.Queryable<Cjxx>().ToList();
            foreach (var cjxx in cjxxes)
            {
                if (!string.IsNullOrEmpty(cjxx.Jldms))
                {
                    string[] strs = cjxx.Jldms.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < strs.Length; i++)
                    {
                        //cjxxs.Add(new Cjxx() { Cjdm = cjxx.Cjdm, Jldms = strs[i] });
                        defaultChecked.Add(new Cjxx() { Cjdm = cjxx.Cjdm, Jldms = strs[i] });
                    }
                }
            }
            #endregion
            //var trees = CwJlxxService.FindJlxxTree();
            #region CwJlxxService.FindJlxxTree()
            IList<TreeNode> trees = new List<TreeNode>();
            //IEnumerable<CwJlxx> jlxxs = FindAll().OrderBy(p => p.Ord);
            List<CwJlxx> jlxxs = _db.Queryable<CwJlxx>().OrderBy(x => x.Ord).ToList();
            foreach (CwJlxx jlxx in jlxxs)
            {
                TreeNode rootTree1 = new TreeNode() { Id = jlxx.Id, NodeCode = jlxx.Jldm, NodeName = jlxx.Jlmc };
                //rootTrees.Add(rootTree1);
                trees.Add(rootTree1);
            }
            #endregion
            return new { success = true, data = new { items = trees, defaultChecked = defaultChecked } };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    /// <summary>
    /// 添加车间交路信息
    /// </summary>
    /// <param name="cjdms"></param>
    /// <param name="jldms"></param>
    /// <returns></returns>
    [Route("/api/Cjjlxx/Add")]
    [HttpGet]
    [Authorize(Roles = "cjjlxx_add")]
    public dynamic Add(string cjdms, string jldms)
    {
        try
        {
            _db.Ado.BeginTran();
            //IList<CwJlxx> cwjlxxs = CwJlxxService.FindAll().ToList();
            IList<CwJlxx> cwjlxxs = _db.Queryable<CwJlxx>().ToList();
            //int result = CjxxService.AddJdlsmByCjdm(cjdms, jldms, cwjlxxs);
            int result= AddJdlsmByCjdm(cjdms, jldms, cwjlxxs);
            //CjxxService.InsertZdFile();
            InsertZdFile();
            //CjxxService.Commit();
            _db.Ado.CommitTran();
            return new { success = true, data = result };
        }
        catch (Exception e)
        {
            _db.Ado.RollbackTran();
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    private void InsertZdFile()
    {
        //ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        try
        {
            // 修改未归档时，在dic_zd_file(修改未归档信息)中添加一行记录
            //DeleteZdFile();
            string sql = string.Format("delete from {0} where zdbmc = '{1}'", "dic_zd_file", "dic_cjxx");
            //CjxxHisService.CjxxHisRepository.ExecuteSqlCommand(sql);
            _db.Ado.ExecuteCommand(sql);
            sql = string.Format("insert into {0} values('{1}','{2}')", "dic_zd_file", "dic_cjxx", DateTime.Today.ToString("yyyyMMddHH24mmss"));
            //CjxxHisService.CjxxHisRepository.ExecuteSqlCommand(sql);
            _db.Ado.ExecuteCommand(sql);
        }
        catch (Exception e)
        {
            //log.Error("CjxxService:InsertZdFile " + ExceptionUtil.GetExceptionMsg(e));
            throw e;
        }
    }

    private int AddJdlsmByCjdm(string cjdms, string jldms, IList<CwJlxx> cwjlxxs)
    {
        var cjdmjas = JArray.Parse(cjdms).Values().ToList();
        var jldmjas = JArray.Parse(jldms).Values().ToList();
        int result = 0;
        //IList<CwJlxx> judgeRoles = RoleService.FindAll().ToList();
        if (jldmjas.Count > 0)
        {
            foreach (JToken cjdmja in cjdmjas)
            {
                string cjdm = cjdmja.ToString();
                //string updatesql = string.Format("update {0} set jldms = '' where cjdm = '{1}'", "dic_cjxx", cjdm);
                //先把交路置为空值
                string cjjlxx = "";//需要更新的交路值
                                   //CjxxRepository.ExecuteSqlCommand(updatesql);
                foreach (JToken jldmja in jldmjas)
                {
                    string jldm = jldmja.ToString();
                    //用于判断树中选择的交路是否存在于交路信息表中
                    if (cwjlxxs.Where(p => p.Jldm == jldm).Count() > 0)
                    {
                        cjjlxx = cjjlxx + jldm + ",";
                    }
                }
                if (!string.IsNullOrEmpty(cjjlxx)) //存在逗号
                {
                    //去掉最后面的逗号
                    cjjlxx = cjjlxx.Substring(0, cjjlxx.Length - 1);
                    //string updatesql = string.Format("update {0} set jldms = '{1}' where cjdm = '{2}'", "dic_cjxx", cjjlxx, cjdm);
                    //result = CjxxRepository.Context.ExecuteSqlCommand(updatesql);
                    //CjxxRepository.Context.Commit();
                    _db.Updateable<DIC_CJXX>()
                        .SetColumns(x => new DIC_CJXX()
                        {
                            JLDMS = cjjlxx
                        })
                        .Where(x => x.CJDM == cjdm)
                        .ExecuteCommand();
                }
            }
        }
        else
        {
            foreach (JToken cjdmja in cjdmjas)
            {
                string cjdm = cjdmja.ToString();
                string updatesql = string.Format("update {0} set jldms = '' where cjdm = '{1}'", "dic_cjxx", cjdm);
                //result = CjxxRepository.Context.ExecuteSqlCommand(updatesql);
                //CjxxRepository.Context.Commit();
                _db.Updateable<DIC_CJXX>()
                        .SetColumns(x => new DIC_CJXX()
                        {
                            JLDMS = ""
                        })
                        .Where(x => x.CJDM == cjdm)
                        .ExecuteCommand();
            }
        }
        return result;
    }
}
