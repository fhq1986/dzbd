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
using DapperExtensions.Predicate;
using FastReport.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using static Abp.Domain.Uow.AbpDataFilters;

namespace Admin.NET.Core.Service.DictionaryManage;
/// <summary>
/// 乘务交路信息管理接口
/// </summary>
[NonUnify]
[ApiDescriptionSettings(Order = 390)]
public class CwJlxxManageService : IDynamicApiController, ITransient
{
    private readonly ISqlSugarClient _db;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public CwJlxxManageService(ISqlSugarClient db)
    {
        _db = db;
    }


    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="param">参数json串</param>
    /// <returns></returns>
    [Route("/api/CwJlxx/FindAll")]
    [HttpGet]
    [Authorize(Roles = "cwjlxx_base")]
    public dynamic FindAll(string param)
    {
        try
        {
            //var result = CwJlxxService.FindCwyJlxxByCondition(param);
            List<CwJlxx> result;
            //if (result is PagedResult<CwJlxx>)
            //{
            //    var pagedResult = result as PagedResult<CwJlxx>;
            //    return new { success = true, data = new { total = pagedResult.TotalRecords, pages = pagedResult.TotalPages, items = result } };
            //}
            //else
            //{
            //    return new { success = true, data = new { total = result.Count(), items = result } };
            //}
            #region CwJlxxService.FindCwyJlxxByCondition(param);
            //PredicateGroup pg;
            List<IConditionalModel> pg;
            IList<ISort> sorts;
            int page, limit;
            //if (!parseParam(param, out pg, out sorts, out page, out limit)) return null;
            if (!SqlSugarHelper.GeneratePredicateAndSort<CwJlxx>(param, out pg, out sorts,out page,out limit))
                return new { success = true, data = new { total = 0, pages = 0, items = new List<CwJlxx>() } };
            JObject jParam = JObject.Parse(param);
            var query = _db.Queryable<CwJlxx>();
            if (pg != null
                && pg.Count > 0)
            {
                query = query.Where(pg);
            }
            if (sorts != null
                && sorts.Count > 0)
            {
                foreach (var item in sorts)
                {
                    if (item.Ascending)
                        query = query.OrderBy($"{item.PropertyName} asc");
                    else
                        query = query.OrderBy($"{item.PropertyName} desc");
                }
            }
            //分页模式
            if (jParam.ContainsKey("pageIndex") && !string.IsNullOrEmpty(jParam.Value<string>("pageIndex")) &&
                jParam.ContainsKey("pageSize") && !string.IsNullOrEmpty(jParam.Value<string>("pageSize")))
            {
                //return CwJlxxRepository.FindAll(pg, sorts, page, limit);
                int pageResult=0, 
                    total=0;
                result = query.ToPageList(page, limit,ref total,ref page);
                return new { success = true, data = new { total = total, pages = page, items = result } };
            }
            else
            {
                //return CwJlxxRepository.FindAll(pg, sorts);
                result=query.ToList();
                return new { success = true, data = new { total = result.Count(), items = result } };
            }
            #endregion
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }

    }

    /// <summary>
    /// 导出Excel
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [Route("/api/CwJlxx/ExportExcel")]
    [HttpGet]
    [Authorize(Roles = "cwjlxx_base")]
    public dynamic ExportExcel(string param)
    {
        try
        {
            //给定一个默认的排序属性字段
            //var users = CwJlxxService.ExcelExport(param);
            var users = ExcelExport(param);
            return new { success = true, data = new { items = users } };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    private dynamic ExcelExport(string param)
    {
        if (string.IsNullOrEmpty(param) || param == null) return null;
        JObject jo = JObject.Parse(param);
        var query = _db.Queryable<CwJlxx>();
        //IList<IPredicate> predList = new List<IPredicate>();
        //if (!string.IsNullOrWhiteSpace(jo["Jldm"].ToString())) predList.Add(Predicates.Field<CwJlxx>(p => p.Jldm, Operator.Eq, jo["Jldm"].ToString()));
        if (!string.IsNullOrWhiteSpace(jo["Jldm"]+""))
            query = query.Where(x => x.Jldm == jo["Jldm"].ToString());
        //if (!string.IsNullOrWhiteSpace(jo["Jlmc"].ToString())) predList.Add(Predicates.Field<CwJlxx>(p => p.Jlmc, Operator.Eq, jo["Jlmc"].ToString()));
        if (!string.IsNullOrWhiteSpace(jo["Jlmc"]+""))
            query = query.Where(x => x.Jlmc == jo["Jlmc"].ToString());
        //IPredicateGroup predGroup = Predicates.Group(GroupOperator.And, predList.ToArray());
        //IList<ISort> sor = new List<ISort>();
        if (!string.IsNullOrWhiteSpace(jo["sort"].ToString()))
        {
            //sor.Add(new Sort { PropertyName = jo["sort"].ToString(), Ascending = true });
            query = query.OrderBy(jo["sort"].ToString()+" asc");
        }
        //var users = CwJlxxRepository.FindAll(predGroup, sor);
        var users = query.ToList();
        return users;
    }

    /// <summary>
    /// 从总公司查询交路信息
    /// </summary>
    /// <param name="param">参数json串</param>
    /// <returns></returns>
    [Route("/api/CwJlxx/FindAllFromZgs")]
    [HttpGet]
    public dynamic FindAllFromZgs(string param)
    {
        try
        {
            //从总公司查询乘务员交路信息
            Dictionary<string, string> paramters = new Dictionary<string, string>();
            paramters.Add("param", param);
            //dynamic dy = ZgsService.GetToZgs(paramters, "api/CwJlxx/FindAllByCondition");
            dynamic dy = GetToZgs(paramters, "api/CwJlxx/FindAllByCondition");
            if (dy.success)
            {
                return dy;
            }
            else  //从机务段查询乘务员交路信息
            {
                //var cwjlxxs = CwJlxxService.FindCwyJlxxByCondition(param);
                //PredicateGroup pg;
                List<IConditionalModel> pg;
                IList<ISort> sorts;
                int page, limit;
                //if (!parseParam(param, out pg, out sorts, out page, out limit)) return null;
                if (!SqlSugarHelper.GeneratePredicateAndSort<CwJlxx>(param, out pg, out sorts, out page, out limit))
                    return new { success = true, data = new { total = 0, pages = 0, items = new List<CwJlxx>() } };
                JObject jParam = JObject.Parse(param);
                var query = _db.Queryable<CwJlxx>();
                if (pg != null
                    && pg.Count > 0)
                {
                    query = query.Where(pg);
                }
                if (sorts != null
                    && sorts.Count > 0)
                {
                    foreach (var item in sorts)
                    {
                        if (item.Ascending)
                            query = query.OrderBy($"{item.PropertyName} asc");
                        else
                            query = query.OrderBy($"{item.PropertyName} desc");
                    }
                }
                List<CwJlxx> result;
                //分页模式
                if (jParam.ContainsKey("pageIndex") && !string.IsNullOrEmpty(jParam.Value<string>("pageIndex")) &&
                    jParam.ContainsKey("pageSize") && !string.IsNullOrEmpty(jParam.Value<string>("pageSize")))
                {
                    //return CwJlxxRepository.FindAll(pg, sorts, page, limit);
                    int pageResult = 0,
                        total = 0;
                    result = query.ToPageList(page, limit, ref total, ref page);
                    return new { success = true, data = new { total = total, pages = page, items = result } };
                }
                else
                {
                    //return CwJlxxRepository.FindAll(pg, sorts);
                    result = query.ToList();
                    return new { success = true, data =result};
                }
            }
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    private dynamic GetToZgs(Dictionary<string, string> parameters, string method)
    {
        string msg;
        //string url = XtcsService.FindAll().Where(p => p.Csdm.Equals("zgsurl")).FirstOrDefault().Cs;
        string url = _db.Queryable<Xtcs>().Where(x => x.Csdm == "zgsurl").First().Cs;
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
    /// 分页查询
    /// </summary>
    /// <param name="param">参数json串</param>
    /// <returns></returns>
    //[HttpGet]
    //public dynamic FindAllByCondition(string param)
    //{
    //    try
    //    {
    //        //从总公司模糊查询乘务员信息
    //        Dictionary<string, string> paramters = new Dictionary<string, string>();
    //        paramters.Add("param", param);
    //        return ZgsService.GetToZgs(paramters, "api/CwJlxx/FindAllByCondition");
    //    }
    //    catch (Exception e)
    //    {
    //        return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
    //    }

    //}

    /// <summary>
    /// 列表树: 车间信息
    /// </summary>
    /// <returns></returns>
    [Route("/api/CwJlxx/FindTree")]
    [HttpGet]
    [Authorize(Roles = "cwjlxx_tree")]
    public dynamic FindTree()
    {
        try
        {
            //var trees = CwJlxxService.FindTree();
            // 本段机务段代码
            //string bdjwdmc = XtcsService.FindByCsdm("jwdmc");
            //string bdjwddm = XtcsService.FindByCsdm("jwddm");
            string bdjwdmc = _db.Queryable<Xtcs>().Where(x => x.Csdm == "jwdmc").Single()?.Cs,
                    bdjwddm = _db.Queryable<Xtcs>().Where(x => x.Csdm == "jwddm").Single()?.Cs;
            //var cjxxs = CjxxService.FindAll();
            var cjxxs = _db.Queryable<Cjxx>().ToList();
            IList<TreeNode> trees = new List<TreeNode>();
            IList<TreeNode> chidrentree = new List<TreeNode>();
            foreach (Cjxx cjxx in cjxxs)
            {
                chidrentree.Add(new TreeNode() { Id = cjxx.Id, NodeCode = cjxx.Cjdm, NodeName = cjxx.Cjmc });
            }
            TreeNode father = new TreeNode() { Id = 1, NodeName = bdjwdmc, NodeCode = bdjwddm, Children = chidrentree };
            trees.Add(father);
            return new { success = true, data = trees };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="cwJlxx"></param>
    /// <returns></returns>
    [Route("/api/CwJlxx/Insert")]
    [HttpPost]
    [Authorize(Roles = "cwjlxx_add")]
    public dynamic Insert([FromBody] CwJlxx cwJlxx)
    {
        try
        {
            //int result = CwJlxxService.Add(cwJlxx);
            _db.Ado.BeginTran();
            _db.Insertable(cwJlxx).ExecuteCommand();
            CwJlxx model = _db.Queryable<CwJlxx>()
                            .Where(x => x.jllx == cwJlxx.jllx
                                        && x.Jldm == cwJlxx.Jldm
                                        && x.Jlmc == cwJlxx.Jlmc
                                        && x.Cjdm == cwJlxx.Cjdm).Single();
            int result=model==null ? 0 : model.Id;
            //CwJlxxService.InsertZdFile();
            #region CwJlxxService.InsertZdFile();
            //DeleteZdFile();
            string sql = string.Format("delete from {0} where zdbmc = '{1}'", "dic_zd_file", "dic_CwJlxx");
            _db.Ado.ExecuteCommand(sql);
            sql = string.Format("insert into {0} values('{1}','{2}')", "dic_zd_file", "dic_CwJlxx", DateTime.Today.ToString("yyyyMMddHH24mmss"));
            //CwJlxxHisService.CwJlxxHisRepository.ExecuteSqlCommand(sql);
            _db.Ado.ExecuteCommand(sql);
            #endregion
            //CwJlxxService.Commit();
            _db.Ado.CommitTran();
            return new { success = true, data = result };
        }
        catch (Exception e)
        {
            _db.Ado.RollbackTran();
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 更改
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [Route("/api/CwJlxx/Update")]
    [HttpPost]
    [Authorize(Roles = "cwjlxx_edit")]
    public dynamic Update([FromBody] CwJlxx cwJlxx)
    {
        try
        {
            string sql = "";
            _db.Ado.BeginTran();
            //CwJlxx jlxx = CwJlxxService.Find(Predicates.Field<CwJlxx>(p => p.Jldm, Operator.Eq, cwJlxx.Jldm));
            CwJlxx jlxx = _db.Queryable<CwJlxx>().Where(x => x.Jldm == cwJlxx.Jldm).Single();
            if (jlxx == null) //交路代码修改了，需要更新车间表中Jldms字段
            {
                //CwJlxx jlxxbefore = CwJlxxService.Find(Predicates.Field<CwJlxx>(p => p.Id, Operator.Eq, cwJlxx.Id)); //更新前对象
                CwJlxx jlxxbefore = _db.Queryable<CwJlxx>().Where(x => x.Id == cwJlxx.Id).Single();
                //更新车间表中Jldms字段
                //CjxxService.updateJldms(jlxxbefore.Jldm, cwJlxx.Jldm);
                sql = string.Format("update dic_cjxx set jldms = replace(jldms,'{0}','{1}') where ',' || jldms || ',' like '%,{2},%'", jlxxbefore.Jldm, cwJlxx.Jldm, jlxxbefore.Jldm);
                _db.Ado.ExecuteCommand(sql);
                InsertZdFile();
            }
            //CwJlxxService.Update(cwJlxx);
            _db.Updateable(cwJlxx).ExecuteCommand();
            //CwJlxxService.InsertZdFile();
            InsertZdFile();
            //CwJlxxService.Commit();
            _db.Ado.CommitTran();
            return new { success = true, data = 1 };
        }
        catch (Exception e)
        {
            _db.Ado.RollbackTran();
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    private void InsertZdFile()
    {
        try
        {
            // 修改未归档时，在dic_zd_file(修改未归档信息)中添加一行记录
            DeleteZdFile();
            string sql = string.Format("insert into {0} values('{1}','{2}')", "dic_zd_file", "dic_cjxx", DateTime.Today.ToString("yyyyMMddHH24mmss"));
            //CjxxHisService.CjxxHisRepository.ExecuteSqlCommand(sql);
            _db.Ado.ExecuteCommand(sql);
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    private void DeleteZdFile()
    {
        string sql = string.Format("delete from {0} where zdbmc = '{1}'", "dic_zd_file", "dic_cjxx");
        _db.Ado.ExecuteCommand(sql);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [Route("/api/CwJlxx/Delete")]
    [HttpPost]
    [Authorize(Roles = "cwjlxx_delete")]
    public dynamic Delete([FromBody]CwJlxx cwJlxx)
    {
        try
        {
            _db.Ado.BeginTran();
            //CwJlxxService.Remove(cwJlxx);
            _db.Deleteable(cwJlxx).ExecuteCommand();
            //CwJlxxService.InsertZdFile();
            InsertZdFile();
            _db.Ado.CommitTran();
            //CwJlxxService.Commit();
            return new { success = true, data = 1 };
        }
        catch (Exception e)
        {
            _db.Ado.RollbackTran();
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 数据库字典表归档
    /// </summary>
    /// <returns></returns>
    [Route("/api/CwJlxx/ToHistory")]
    [HttpGet]
    [Authorize(Roles = "cwjlxx_tohistory")]
    public dynamic ToHistory()
    {
        //return CwJlxxService.ToHistory();
        return CwJlxxToHistory();
    }

    private dynamic CwJlxxToHistory()
    {
        try
        {
            _db.Ado.BeginTran();
            bool bolTlj = true;
            bool bolZgs = true;
            string messageTlj = "";
            string messageZgs = "";
            // 含有车间版本号，归档之前先查询是否需要归档
            if (CjxxService_FindZdFile())
            {
                dynamic cjresult = CjxxService_ToHistory(bolTlj, bolZgs);
                bolTlj = cjresult.successTlj;  //铁路局是否归档成功
                bolZgs = cjresult.successZgs;  //铁路局是否归档成功
                messageTlj = cjresult.messageTlj;
                messageZgs = cjresult.messageZgs;
            }
            // 含有乘务交路版本号，归档之前先查询是否需要归档
            if (FindZdFile())
            {
                dynamic jlresult = ToHistory(bolTlj, bolZgs);
                bolTlj = jlresult.successTlj;  //铁路局是否归档成功
                bolZgs = jlresult.successZgs;  //铁路局是否归档成功
                if (bolTlj == false && messageTlj == "") messageTlj = jlresult.messageTlj;
                if (bolZgs == false && messageZgs == "") messageZgs = jlresult.messageZgs;
            }
            _db.Ado.CommitTran();
            if (bolTlj == true && bolZgs == true)  //如果铁路局或者总公司字典信息
            {
                return new { success = true, successTlj = bolTlj, successZgs = bolZgs };
            }
            else
                return new { success = false, successTlj = bolTlj, successZgs = bolZgs, message = (messageTlj + " " + messageZgs).Trim(), messageTlj = messageTlj, messageZgs = messageZgs };
        }
        catch (Exception e)
        {
            _db.Ado.RollbackTran();
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e), messageTlj = "", messageZgs = "" };
        }
    }

    private dynamic CjxxService_ToHistory(bool bolTlj, bool bolZgs)
    {
        try
        {
            UpdateHistory();
            //var cjxxes = CjxxHisService.FindAll();
            List<CjxxHis> cjxxes = _db.Queryable<CjxxHis>().ToList();
            string latest = cjxxes.Max(p => p.Version);
            List<CjxxHis> cjxxhises = cjxxes.Where(p => p.Version.Equals(latest)).ToList();
            //string jwddm = XtcsService.FindByCsdm("jwddm");
            Xtcs modelJwddm = _db.Queryable<Xtcs>().Where(x => x.Csdm == "jwddm").Single();
            string jwddm = modelJwddm == null ? "" : modelJwddm.Cs;
            dynamic data = new { jwddm = jwddm, cjxxhis = cjxxhises };
            string messageTlj = "";
            string messageZgs = "";
            dynamic result = null, resultZgs = null;
            if (bolTlj) result = TljService_PostToTlj(data, "api/Cjxx/ToHistory");
            if (bolZgs) resultZgs = ZgsService_PostToZgs(data, "api/Cjxx/ToHistory");
            if (null != result && result.success && null != resultZgs && resultZgs.success)
            {
                DeleteZdFile();// 归档成功后删除未归档记录
                //Commit();
                _db.Ado.CommitTran();
                return new { success = true, successTlj = bolTlj, successZgs = bolZgs };
            }
            else
            {
                if (null != result && result.success == false)
                {
                    messageTlj = result.message;
                    bolTlj = result.success;
                }
                if (null != resultZgs && resultZgs.success == false)
                {
                    messageZgs = resultZgs.message;
                    bolZgs = resultZgs.success;
                }
                return new { success = false, successTlj = bolTlj, successZgs = bolZgs, message = (messageTlj + " " + messageZgs).Trim(), messageTlj = messageTlj, messageZgs = messageZgs };
            }
        }
        catch (Exception e)
        {
            return new { success = false, successTlj = false, successZgs = false, message = e.Message, messageTlj = "", messageZgs = "" };
        }
    }

    private dynamic TljService_PostToTlj(object data, string method)
    {
        string msg;
        //string url = XtcsService.FindAll().Where(p => p.Csdm.Equals("tljurl")).FirstOrDefault().Cs;
        Xtcs modelXtcs = _db.Queryable<Xtcs>().Where(x => x.Csdm == "tljurl").Single();
        string url = modelXtcs == null ? "" : modelXtcs.Cs;
        IRestConnection restConnection = new RestConnection(url);
        if (restConnection.Login(@"/Token", "username", "cf", "password", "123", out msg))
        {
            JObject reslut = restConnection.Post(method, data);
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
            return new { success = false, message = "局连接失败！" };
        }
    }

    private bool CjxxService_FindZdFile()
    {
        string sql = string.Format("select zdbmc from {0} where zdbmc = '{1}'", "dic_zd_file", "dic_cjxx");
        //string result = CjxxRepository.FindZdFile(sql);
        string result = _db.Ado.SqlQuery<string>(sql).Single();
        if (string.IsNullOrEmpty(result)) return false;
        else return true;
    }

    private dynamic ToHistory(bool bolTlj, bool bolZgs)
    {
        try
        {
            UpdateHistory();

            //string jwddm = XtcsService.FindByCsdm("jwddm");
            Xtcs modelXtcs = _db.Queryable<Xtcs>().Where(x => x.Csdm == "jwddm").Single();
            string jwddm = modelXtcs == null ? "" : modelXtcs.Cs;
            //var cwJlxxes = CwJlxxHisService.FindAll();
            List<CwJlxxHis> cwJlxxes = _db.Queryable<CwJlxxHis>().ToList();
            string latest = cwJlxxes.Max(p => p.Version);
            List<CwJlxxHis> cwJlxxhises = cwJlxxes.Where(p => p.Version.Equals(latest)).ToList();
            dynamic data = new { jwddm = jwddm, CwJlxxhis = cwJlxxhises };
            string messageTlj = "";
            string messageZgs = "";
            dynamic result = null, resultZgs = null;
            if (bolTlj)
                //result = TljService.PostToTlj(data, "api/CwJlxx/ToHistory");
                result= PostToTlj(data, "api/CwJlxx/ToHistory");
            if (bolZgs)
                //resultZgs = ZgsService.PostToZgs(data, "api/CwJlxx/ToHistory");
                resultZgs= ZgsService_PostToZgs(data, "api/CwJlxx/ToHistory");
            if (null != result && result.success && null != resultZgs && resultZgs.success)
            {
                DeleteZdFile();// 归档成功后删除未归档记录
                _db.Ado.CommitTran();
                return new { success = true, successTlj = bolTlj, successZgs = bolZgs };
            }
            else
            {
                if (null != result && result.success == false)
                {
                    messageTlj = result.message;
                    bolTlj = result.success;
                }
                if (null != resultZgs && resultZgs.success == false)
                {
                    messageZgs = resultZgs.message;
                    bolZgs = resultZgs.success;
                }
                return new { success = false, successTlj = bolTlj, successZgs = bolZgs, message = (messageTlj + " " + messageZgs).Trim(), messageTlj = messageTlj, messageZgs = messageZgs };
            }
        }
        catch (Exception e)
        {
            return new { success = false, successTlj = false, successZgs = false, message = ExceptionUtil.GetExceptionMsg(e), messageTlj = "", messageZgs = "" };
        }
    }

    private dynamic ZgsService_PostToZgs(object data, string method)
    {
        string msg;
        //string url = XtcsService.FindAll().Where(p => p.Csdm.Equals("zgsurl")).FirstOrDefault().Cs;
        Xtcs modelXtcs = _db.Queryable<Xtcs>().Where(x => x.Csdm == "zgsurl").Single();
        if (modelXtcs == null)
        {
            return new { success = false, data = "系统参数不存在" };
        }
        string url = modelXtcs.Cs;
        IRestConnection restConnection = new RestConnection(url);
        if (restConnection.Login(@"/Token", "username", "cf", "password", "123", out msg))
        {
            JObject reslut = restConnection.Post(method, data);
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

    private dynamic PostToTlj(object data, string method)
    {
        string msg;
        //string url = XtcsService.FindAll().Where(p => p.Csdm.Equals("tljurl")).FirstOrDefault().Cs;
        Xtcs modelXtcs = _db.Queryable<Xtcs>().Where(x => x.Csdm == "tljurl").Single();
        if(modelXtcs==null)
        {
            return new { success = false, data = "系统参数不存在" };
        }
        string url = modelXtcs.Cs;
        IRestConnection restConnection = new RestConnection(url);
        if (restConnection.Login(@"/Token", "username", "cf", "password", "123", out msg))
        {
            JObject reslut = restConnection.Post(method, data);
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
            return new { success = false, message = "局连接失败！" };
        }
    }

    private void UpdateHistory()
    {
        // 历史表中添加的是本日期最新修改的数据、即现用表的数据，每日期一版
        //var cwJlxxes = CwJlxxRepository.FindAll();
        List<CwJlxx> cwJlxxes = _db.Queryable<CwJlxx>().ToList();
        string version = DateTime.Today.ToString("yyyyMMdd");
        // 先删后加
        string sql = string.Format("delete from {0} where version = '{1}'", "dic_CwJlxx_his", version);
        //CwJlxxHisService.CwJlxxHisRepository.ExecuteSqlCommand(sql);
        _db.Ado.ExecuteCommand(sql);
        // 车间版本
        //string cjversion = CjxxHisService.FindAll().Max(p => p.Version);
        string cjversion = _db.Queryable<CjxxHis>().Max(x => x.Version);
        foreach (var cwJlxx in cwJlxxes)
        {
            CwJlxxHis cwJlxxHis = new CwJlxxHis() { Cjdm = cwJlxx.Cjdm, Jldm = cwJlxx.Jldm, jllx = cwJlxx.jllx, Jlmc = cwJlxx.Jlmc, Ord = cwJlxx.Ord, CjVersion = cjversion, Version = version };
            //CwJlxxHisService.Add(cwJlxxHis);
            _db.Insertable(cwJlxxHis).ExecuteCommand();
        }
    }

    private bool FindZdFile()
    {
        string sql = string.Format("select zdbmc from {0} where zdbmc = '{1}'", "dic_zd_file", "dic_CwJlxx");
        //string result = CwJlxxRepository.FindZdFile(sql);
        string result = _db.Ado.SqlQuery<string>(sql).SingleOrDefault();
        if (string.IsNullOrEmpty(result)) return false;
        else return true;
    }
}
