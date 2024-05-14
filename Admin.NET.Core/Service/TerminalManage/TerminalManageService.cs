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
using DapperExtensions.Predicate;
using DapperExtensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace Admin.NET.Core.Service.TerminalManage;
/// <summary>
/// 终端管理接口
/// </summary>
[NonUnify]
[ApiDescriptionSettings(Order = 390)]
public class TerminalManageService : IDynamicApiController, ITransient
{
    private readonly ISqlSugarClient _db;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public TerminalManageService(ISqlSugarClient db)
    {
        _db = db;
    }

    #region 终端维护
    [HttpGet]
    [Authorize(Roles = "zdwh_base")]
    [Route("/api/Zdgl/FindAll")]
    public dynamic FindAll(string param)
    {
        try
        {
            string ddm = "";
            // 解析字符串
            if (string.IsNullOrEmpty(param)) param = "{}";
            JObject jParam = JObject.Parse(param);
            if (jParam.ContainsKey("Ddm") && !string.IsNullOrEmpty(jParam.Value<string>("Ddm")))
            {
                ddm = jParam.Value<string>("Ddm");
            }
            //var zdgls = ZdglService.FindAll2(ddm).OrderBy(p => p.Cwzyddmc);
            List<Zdgl> zdgls;
            if(string.IsNullOrWhiteSpace(ddm)) 
            {
                zdgls = _db.Queryable<Zdgl>()
                            .Where(x => SqlFunc.IsNullOrEmpty(x.Ddm))
                            .OrderBy(x => x.Cwzyddmc).ToList();
            }
            else
            {
                zdgls = _db.Queryable<Zdgl>()
                            .Where(x => SqlFunc.IsNullOrEmpty(x.Ddm)
                                        || x.Ddm==ddm)
                            .OrderBy(x => x.Cwzyddmc).ToList();
            }
            return new { success = true, data = zdgls };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }

    }
    [HttpPost]
    [Route("/api/Zdgl/FindCurrentZdgl")]
    public dynamic FindCurrentZdgl([FromBody]object param)
    {
        //return ZdglService.FindCurrentZdgl(param.ToString());
        try
        {
            if (param == null
            || string.IsNullOrWhiteSpace(param.ToString()))
                return null;
            JObject jo = JObject.Parse(param.ToString());
            string zddm = jo["Zddm"].ToString();
            //IPredicate predicate = Predicates.Field<Zdgl>(p => p.Zddm, Operator.Eq, zddm);
            //Zdgl zd = ZdglRepository.FindAll(predicate).FirstOrDefault();
            Zdgl zd = _db.Queryable<Zdgl>()
                        .Where(x => x.Zddm == zddm).Single();
            if (zd == null)
            {
                zd = new Zdgl();
                zd.Zddm = zddm;
                zd.Zdmc = zddm;
                zd.Zdpath = zddm;
                //终端不存在，将CPU加入数据库
                //var aa = ZdglRepository.Add(zd);
                //Commit();
                _db.Insertable<Zdgl>(zd).ExecuteCommand();

                //zd = ZdglRepository.FindAll(predicate).FirstOrDefault();
                zd= _db.Queryable<Zdgl>()
                        .Where(x => x.Zddm == zddm).Single();
            }
            return new { success = true, data = zd };
        }
        catch (Exception e)
        {
            return new { success = false, data = 1 };
        }
    }
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="zdgl"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "zdwh_add")]
    [Route("/api/Zdgl/Add")]
    public dynamic Add(Zdgl zdgl)
    {
        try
        {
            //int result = ZdglService.Add(zdgl);
            //ZdglService.Commit();
            _db.Insertable(zdgl).ExecuteCommand();

            zdgl = _db.Queryable<Zdgl>()
                        .Where(x => x.Zddm == zdgl.Zddm
                                    && x.Zdmc == zdgl.Zdmc
                                    && x.Zdpath == zdgl.Zdpath
                                    && x.Cwzydddm==zdgl.Cwzydddm
                                    && x.Cwzyddmc==zdgl.Cwzyddmc
                                    && x.Hdsj==zdgl.Hdsj
                                    && x.Cwydm==zdgl.Cwydm
                                    && x.Cwyxm==zdgl.Cwyxm
                                    && x.Ddm==zdgl.Ddm).Single();
            int result = zdgl == null ? 0 : zdgl.Id;
            return new { success = true, data = result };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 更改
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "zdwh_edit")]
    [Route("/api/Zdgl/Update")]
    public dynamic Update(Zdgl zdgl)
    {
        try
        {
            //ZdglService.UpdateZdgl(zdgl);
            _db.Updateable(zdgl).ExecuteCommand();
            return new { success = true, data = 1 };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 更改
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "zdwh_edit")]
    [Route("/api/Zdgl/UpdateModel")]
    public dynamic UpdateModel(Zdgl zdgl)
    {
        try
        {
            //ZdglService.Update(zdgl);
            //ZdglService.Commit();
            _db.Updateable(zdgl).ExecuteCommand();
            return new { success = true, data = 1 };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "zdwh_delete")]
    [Route("/api/Zdgl/Delete")]
    public dynamic Delete(Zdgl zdgl)
    {
        try
        {
            //ZdglService.Remove(zdgl);
            //ZdglService.Commit();
            _db.Deleteable(zdgl).ExecuteCommand();
            return new { success = true, data = 1 };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    #endregion
    #region 终端事件
    /// <summary>
    /// 查询终端事件
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "zdsj_base")]
    [Route("/api/Zdgl/FindAllZdsj")]
    public dynamic FindAllZdsj(string param)
    {
        try
        {
            //IEnumerable<Zdsj> result = ZdsjService.FindAllZdsj(param);
            List<Zdsj> result;
            if (string.IsNullOrEmpty(param)) param = "{}";
            JObject jParam = JObject.Parse(param);
            List<IConditionalModel> pg=new List<IConditionalModel>();
            //事件时间
            if (jParam.ContainsKey("StartTime") && !string.IsNullOrEmpty(jParam.Value<string>("StartTime")) &&
                jParam.ContainsKey("EndTime") && !string.IsNullOrEmpty(jParam.Value<string>("EndTime")))
            {
                //pg.Predicates.Add(Predicates.Field<Zdsj>(p => p.Sjsj, Operator.Ge, jParam.Value<string>("StartTime")));
                //pg.Predicates.Add(Predicates.Field<Zdsj>(p => p.Sjsj, Operator.Le, jParam.Value<string>("EndTime")));
                pg.Add(new ConditionalModel { FieldName = "sjsj", ConditionalType = ConditionalType.GreaterThan, FieldValue = jParam.Value<string>("StartTime") });
                pg.Add(new ConditionalModel { FieldName = "sjsj", ConditionalType = ConditionalType.LessThanOrEqual, FieldValue = jParam.Value<string>("EndTime") });
            }
            //终端名称
            if (jParam.ContainsKey("Zdmc") && !string.IsNullOrEmpty(jParam.Value<string>("Zdmc")))
            {
                //pg.Predicates.Add(Predicates.Field<Zdsj>(p => p.Zdmc, Operator.Eq, jParam.Value<string>("Zdmc")));
                pg.Add(new ConditionalModel { FieldName = "Zdmc", ConditionalType = ConditionalType.Equal, FieldValue = jParam.Value<string>("Zdmc") });
            }
            if (jParam.ContainsKey("Ddm") && !string.IsNullOrEmpty(jParam.Value<string>("Ddm")))
            {
                //pg.Predicates.Add(Predicates.Field<Zdsj>(p => p.Ddm, Operator.Eq, jParam.Value<string>("Ddm")));
                pg.Add(new ConditionalModel { FieldName = "Ddm", ConditionalType = ConditionalType.Equal, FieldValue = jParam.Value<string>("Ddm") });
            }
            IList<ISort> sorts = new List<ISort>();
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
                    sorts.Add(new Sort { PropertyName = sortName, Ascending = sortType.Equals("ASC") });
                }
            }
            if (sorts.Count() == 0)
            {
                sorts.Add(new Sort { PropertyName = "Sjsj", Ascending = false });
                sorts.Add(new Sort { PropertyName = "Cwyxm", Ascending = true });
            }

            //分页模式
            int page = 0;
            int limit = 0;
            if (jParam.ContainsKey("page") && !string.IsNullOrEmpty(jParam.Value<string>("page")) &&
                jParam.ContainsKey("limit") && !string.IsNullOrEmpty(jParam.Value<string>("limit")))
            {
                page = jParam.Value<int>("page");
                limit = jParam.Value<int>("limit");
            }
            var query = _db.Queryable<Zdsj>();
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

            //if (result is PagedResult<Zdsj>)
            //{
            //    var pagedResult = result as PagedResult<Zdsj>;
            //    return new { success = true, message = "", data = result, total = pagedResult.TotalRecords, pages = pagedResult.TotalPages };
            //}
            //else
            //{
            //    return new { success = true, message = "", data = result, total = result.Count() };
            //}
            //分页模式
            if (page > 0 && limit > 0)
            {
                //return ZdsjRepository.FindAll(pg, sorts, page, limit);
                int total = 0,
                    totalPages = 0;
                result = query.ToPageList(page, limit, ref total, ref totalPages);
                return new { success = true, message = "", data = result, total = total, pages = totalPages };
            }
            else
            {
                //return ZdsjRepository.FindAll(pg, sorts);
                result = query.ToList();
                return new { success = true, message = "", data = result, total = result.Count() };
            }

        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    /// <summary>
    /// 添加终端事件
    /// </summary>
    /// <param name="zdgl"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("/api/Zdgl/AddZdsj")]
    public dynamic AddZdsj(Zdsj zdsj)
    {
        try
        {
            if (!string.IsNullOrEmpty(zdsj.Zdid))
            {
                //var zdgl = ZdglService.FindById(Convert.ToInt32(zdsj.Zdid));
                var zdgl = _db.Queryable<Zdgl>().Where(x => x.Id == SqlFunc.ToInt32(zdsj.Zdid)).Single();
                if (zdgl != null)
                {
                    zdsj.Ddm = zdgl.Ddm;
                }
            }

            //int result = ZdsjService.Add(zdsj);
            //ZdsjService.Commit();
            int result = _db.Insertable(zdsj).ExecuteCommand();
            return new { success = true, data = result };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    #endregion
    #region 终端实时状态
    /// <summary>
    /// 查询终端实时状态
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "zdsszt_base")]
    [Route("/api/Zdgl/FindZdsszt")]
    public dynamic FindZdsszt(string param)
    {
        try
        {
            string ddm = "";
            // 解析字符串
            if (string.IsNullOrEmpty(param)) param = "{}";
            JObject jParam = JObject.Parse(param);
            if (jParam.ContainsKey("Ddm") && !string.IsNullOrEmpty(jParam.Value<string>("Ddm")))
            {
                ddm = jParam.Value<string>("Ddm");
            }
            //var result = ZdglService.FindAllZdgl(ddm);

            string sqlText = @"select (case when hdsj is null or (sysdate-to_date(hdsj,'yyyy-mm-dd hh24:mi:ss'))*1440>15/60 then '离线' 
                else (case when cwydm is null then '空闲' else '占用' end) end) zt,t.* from dic_zdgl t where cwzydddm is not null ";
            if (!string.IsNullOrEmpty(ddm))
            {
                sqlText += " and ddm = '" + ddm + "'";
            }
            sqlText += " order by cwzyddmc";
            var result = _db.Ado.SqlQuery<dynamic>(sqlText);
            return new { success = true, message = "", data = result };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    #endregion
}
