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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service.DictionaryManage;

/// <summary>
/// 铁路局信息管理接口
/// </summary>
[NonUnify]
[ApiDescriptionSettings(Order = 390)]
public class TljxxManageService : IDynamicApiController, ITransient
{
    private readonly ISqlSugarClient _db;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public TljxxManageService(ISqlSugarClient db)
    {
        _db = db;
    }

    [HttpPost]
    [DisplayName("导入")]
    [Route("/api/Tljxx/Import")]
    public dynamic Import(IFormFile excelData)
    {
        try
        {
            if (excelData == null)
                return new { success = false, message = "请选择需要导入的文件" };
            //HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];//获取传统context
            //HttpRequestBase request = context.Request;//定义传统request对象
            //var file = request.Files["excelData"];
            //string Path = HttpContext.Current.Server.MapPath("/Excel/");
            string Path = Directory.GetCurrentDirectory() + @"\Excel\";
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);  //目录不存在则创建
            }
            //var NewFileName = Guid.NewGuid().ToString() + file.FileName;
            var NewFileName = Guid.NewGuid().ToString() + excelData.FileName;
            var NewPath = Path + NewFileName;
            //file.SaveAs(NewPath);
            //将流写入文件
            using (Stream stream = excelData.OpenReadStream())
            {
                // 把 Stream 转换成 byte[]
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                // 设置当前流的位置为流的开始
                stream.Seek(0, SeekOrigin.Begin);
                // 把 byte[] 写入文件
                FileStream fs = new FileStream(NewPath, FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(bytes);
                bw.Close();
                fs.Close();
            }
            //DataTable dt = TljxxService.ExcelToData(NewPath);
            DataTable dt = NpoiUtil.ExcelToData(NewPath);
            //return TljxxService.InsertData(dt);
            return InsertData(dt);
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="param">参数json串</param>
    /// <returns></returns>
    [HttpGet]
    //[Authorize(Roles = "tlj_base")]
    [DisplayName("分页查询")]
    [Route("/api/Tljxx/FindAll")]
    public dynamic FindAll(string param = "")
    {
        try
        {
            //PredicateGroup predicateGroup;
            List<IConditionalModel> predicateGroup = new List<IConditionalModel>();
            IList<ISort> sorts;
            //DapperHelper.GeneratePredicateAndSort<Tljxx>(param, out predicateGroup, out sorts);
            SqlSugarHelper.GeneratePredicateAndSort<Tljxx>(param, out predicateGroup, out sorts);
            //var tljxxs = TljxxService.FindAll(predicateGroup, sorts);
            var query = _db.Queryable<Tljxx>();
            if (predicateGroup != null
                && predicateGroup.Count > 0)
            {
                query = query.Where(predicateGroup);
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
            var tljxxs = query.ToList();
            return new { success = true, data = new { total = tljxxs.Count(), items = tljxxs } };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="tljxx"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "tlj_add")]
    [Route("/api/Tljxx/Add")]
    public dynamic Add([FromBody] Tljxx tljxx)
    {
        try
        {
            //int result = TljxxService.Add(tljxx);
            //TljxxService.Commit();
            _db.Insertable(tljxx).ExecuteCommand();
            tljxx = _db.Queryable<Tljxx>()
                        .Where(x => x.Tljdm == tljxx.Tljdm
                                    && x.Tljmc == tljxx.Tljmc
                                    && x.Jwbm == tljxx.Jwbm).Single();
            return new { success = true, data = tljxx != null ? tljxx.Id : 0 };
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
    [Authorize(Roles = "tlj_edit")]
    [Route("/api/Tljxx/Update")]
    public dynamic Update([FromBody] Tljxx tljxx)
    {
        try
        {
            //TljxxService.Update(tljxx);
            //TljxxService.Commit();
            _db.Updateable(tljxx).ExecuteCommand();
            return new { success = true };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="tljxx"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "tlj_delete")]
    [Route("/api/Tljxx/Remove")]
    public dynamic Remove([FromBody] Tljxx tljxx)
    {
        try
        {
            //TljxxService.Remove(tljxx);
            //TljxxService.Commit();
            _db.Deleteable(tljxx).ExecuteCommand();
            return new { success = true };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 将数据插入到数据库
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    private dynamic InsertData(DataTable dt)
    {
        try
        {
            //插入的集合
            List<Tljxx> insert = new List<Tljxx>();
            //更新的集合
            List<Tljxx> update = new List<Tljxx>();
            //数据检查对比集合
            var tljxxs = _db.Queryable<Tljxx>().ToList();
            if (dt.Rows.Count > 0)
            {
                _db.Ado.BeginTran();
                //开始数据判断操作
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var tlj = tljxxs.Where(p => p.Tljdm == dt.Rows[i][0].ToString()).FirstOrDefault();
                    if (tlj != null)
                    {
                        Tljxx t = new Tljxx
                        {
                            Id = tlj.Id
                            ,
                            Tljdm = dt.Rows[i][0].ToString()
                            ,
                            Tljmc = dt.Rows[i][1].ToString()
                            ,
                            Jwbm = dt.Rows[i][2].ToString()
                            ,
                            Ord = Convert.ToInt32(dt.Rows[i][3])
                        };
                        update.Add(t);
                        //_repository.Update(t);
                        _db.Updateable(t).ExecuteCommand();
                    }
                    else
                    {
                        insert.Add(new Tljxx
                        {
                            Tljdm = dt.Rows[i][0].ToString()
                            ,
                            Tljmc = dt.Rows[i][1].ToString()
                            ,
                            Jwbm = dt.Rows[i][2].ToString()
                            ,
                            Ord = Convert.ToInt32(dt.Rows[i][3])
                        });
                    }
                }
                //_repository.BulkAdd(insert);
                _db.Insertable(insert).ExecuteCommand();
                //Commit();
                _db.Ado.CommitTran();
                return new { success = true };
            }
            else
            {
                return new { success = false, message = "解析的excel数据为空！" };
            }
        }
        catch (Exception ex)
        {
            _db.Ado.RollbackTran();
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(ex) };
        }
    }
}
