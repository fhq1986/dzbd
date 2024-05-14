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
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service.DictionaryManage;
/// <summary>
/// 权限管理接口
/// </summary>
[NonUnify]
[ApiDescriptionSettings(Order = 390)]
public class TreeManageService : IDynamicApiController, ITransient
{
    private readonly ISqlSugarClient _db;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public TreeManageService(ISqlSugarClient db)
    {
        _db = db;
    }

    /// <summary>
    /// 树结构(铁路局->自定义线别)
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "zdyxb_tree")]
    [DisplayName("树结构(铁路局->自定义线别)")]
    [Route("/api/Tree/FindZdyxbTree")]
    public dynamic FindZdyxbTree()
    {
        try
        {
            //管辖的所有铁路局
            IList<TreeNode> fatrees = new List<TreeNode>();
            IList<TreeNode> trees = new List<TreeNode>();
            //IList<Tljxx> tljs = TljxxService.FindAll().OrderBy(p => p.Ord).ToList();//查询所有铁路局dic_tlj
            //IList<Zdyxb> xzqys = ZdyxbService.FindAll().OrderBy(p => p.Ord).ToList();//查询所有自定义线别信息
            IList<Tljxx> tljs = _db.Queryable<Tljxx>().OrderBy(p => p.Ord).ToList();//查询所有铁路局dic_tlj
            IList<Zdyxb> xzqys = _db.Queryable<Zdyxb>().OrderBy(p => p.Ord).ToList();//查询所有自定义线别信息

            foreach (Tljxx tlj in tljs)
            {
                IList<TreeNode> chidrentree = new List<TreeNode>();//子节点集合
                foreach (Zdyxb xzqy in xzqys)
                {
                    if (tlj.Tljdm.Equals(xzqy.Tljdm))
                    {
                        chidrentree.Add(new TreeNode() { Id = xzqy.Id, NodeCode = xzqy.Zdyxbdm, NodeName = xzqy.Zdyxbmc, Type = "ZDYXB", FatherNodeCode = tlj.Tljdm });
                    }
                }
                TreeNode father = new TreeNode() { Id = tlj.Id, NodeCode = tlj.Tljdm, NodeName = tlj.Tljmc, Type = "TLJ", Children = chidrentree };
                trees.Add(father);
            }
            fatrees.Add(new TreeNode() { Id = 101, NodeCode = "ZGS", NodeName = "国铁集团", Type = "ZGS", Children = trees });
            return new { success = true, data = fatrees };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }



    /// <summary>
    /// 树结构(铁路局->行政区域)
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "xzqy_tree")]
    [DisplayName("树结构(铁路局->行政区域)")]
    [Route("/api/Tree/FindXzqyTree")]
    public dynamic FindXzqyTree()
    {
        try
        {
            //管辖的所有铁路局
            IList<TreeNode> fatrees = new List<TreeNode>();
            IList<TreeNode> trees = new List<TreeNode>();
            //IList<Tljxx> tljs = TljxxService.FindAll().OrderBy(p => p.Ord).ToList();//查询所有铁路局dic_tlj
            //IList<Xzqy> xzqys = xzqyService.FindAll().OrderBy(p => p.Ord).ToList();//查询所有行政区域信息
            IList<Tljxx> tljs = _db.Queryable<Tljxx>().OrderBy(p => p.Ord).ToList();//查询所有铁路局dic_tlj
            IList<Xzqy> xzqys = _db.Queryable<Xzqy>().OrderBy(p => p.Ord).ToList();//查询所有行政区域信息

            foreach (Tljxx tlj in tljs)
            {
                IList<TreeNode> chidrentree = new List<TreeNode>();//子节点集合
                foreach (Xzqy xzqy in xzqys)
                {
                    if (tlj.Tljdm.Equals(xzqy.Tljdm))
                    {
                        chidrentree.Add(new TreeNode() { Id = xzqy.Id, NodeCode = xzqy.Xzqydm, NodeName = xzqy.Xzqymc, Type = "XZQY", FatherNodeCode = tlj.Tljdm });
                    }
                }
                TreeNode father = new TreeNode() { Id = tlj.Id, NodeCode = tlj.Tljdm, NodeName = tlj.Tljmc, Type = "TLJ", Children = chidrentree };
                trees.Add(father);
            }
            fatrees.Add(new TreeNode() { Id = 101, NodeCode = "ZGS", NodeName = "国铁集团", Type = "ZGS", Children = trees });
            return new { success = true, data = fatrees };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    /// <summary>
    /// 树结构(铁路局->供电段)
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "gdd_tree")]
    [DisplayName("树结构(铁路局->供电段)")]
    [Route("/api/Tree/FindGddTree")]
    public dynamic FindGddTree()
    {
        try
        {
            //管辖的所有铁路局
            IList<TreeNode> fatrees = new List<TreeNode>();
            IList<TreeNode> trees = new List<TreeNode>();
            //IList<Tljxx> tljs = TljxxService.FindAll().OrderBy(p => p.Ord).ToList();//查询所有铁路局dic_tlj
            //IList<Gdd> gdds = gddService.FindAll().OrderBy(p => p.Ord).ToList();//查询所有供电段信息
            IList<Tljxx> tljs = _db.Queryable<Tljxx>().OrderBy(p => p.Ord).ToList();//查询所有铁路局dic_tlj
            IList<Gdd> gdds = _db.Queryable<Gdd>().OrderBy(p => p.Ord).ToList();//查询所有供电段信息

            foreach (Tljxx tlj in tljs)
            {
                IList<TreeNode> chidrentree = new List<TreeNode>();//子节点集合
                foreach (Gdd gdd in gdds)
                {
                    if (tlj.Tljdm.Equals(gdd.Tljdm))
                    {
                        chidrentree.Add(new TreeNode() { Id = gdd.Id, NodeCode = gdd.Gdddm, NodeName = gdd.Gddmc, Type = "GDD", FatherNodeCode = tlj.Tljdm });
                    }
                }
                TreeNode father = new TreeNode() { Id = tlj.Id, NodeCode = tlj.Tljdm, NodeName = tlj.Tljmc, Type = "TLJ", Children = chidrentree };
                trees.Add(father);
            }
            fatrees.Add(new TreeNode() { Id = 101, NodeCode = "ZGS", NodeName = "国铁集团", Type = "ZGS", Children = trees });
            return new { success = true, data = fatrees };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 树结构(铁路局->通讯段)
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "txd_tree")]
    [DisplayName("树结构(铁路局->通讯段)")]
    [Route("/api/Tree/FindTxdTree")]
    public dynamic FindTxdTree()
    {
        try
        {
            //var trees = TreeService.FindTxdTree();
            //管辖的所有铁路局
            IList<TreeNode> fatrees = new List<TreeNode>();
            IList<TreeNode> trees = new List<TreeNode>();
            //IList<Tljxx> tljs = TljxxService.FindAll().OrderBy(p => p.Ord).ToList();//查询所有铁路局Tljxx
            //IList<Txd> txds = txdService.FindAll().OrderBy(p => p.Ord).ToList();//查询所有通讯段信息
            IList<Tljxx> tljs = _db.Queryable<Tljxx>().OrderBy(x => x.Ord).ToList();
            IList<Txd> txds = _db.Queryable<Txd>().OrderBy(x => x.Ord).ToList();

            foreach (Tljxx tlj in tljs)
            {
                IList<TreeNode> chidrentree = new List<TreeNode>();//子节点集合
                foreach (Txd txd in txds)
                {
                    if (tlj.Tljdm.Equals(txd.Tljdm))
                    {
                        chidrentree.Add(new TreeNode() { Id = txd.Id, NodeCode = txd.Txddm, NodeName = txd.Txdmc, Type = "TXD", FatherNodeCode = tlj.Tljdm });
                    }
                }
                TreeNode father = new TreeNode() { Id = tlj.Id, NodeCode = tlj.Tljdm, NodeName = tlj.Tljmc, Type = "TLJ", Children = chidrentree };
                trees.Add(father);
            }
            fatrees.Add(new TreeNode() { Id = 101, NodeCode = "ZGS", NodeName = "国铁集团", Type = "ZGS", Children = trees });
            return new { success = true, data = fatrees };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 树结构(铁路局->工务段)
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "gwd_tree")]
    [DisplayName("树结构(铁路局->工务段)")]
    [Route("/api/Tree/FindGwdTree")]
    public dynamic FindGwdTree()
    {
        try
        {
            //var trees = TreeService.FindGwdTree();
            //管辖的所有铁路局
            IList<TreeNode> fatrees = new List<TreeNode>();
            IList<TreeNode> trees = new List<TreeNode>();
            //IList<Tljxx> tljs = TljxxService.FindAll().OrderBy(p => p.Ord).ToList();//查询所有铁路局Tljxx
            //IList<Gwd> gwds = gwdService.FindAll().OrderBy(p => p.Ord).ToList();//查询所有工务段信息
            IList<Tljxx> tljs = _db.Queryable<Tljxx>().OrderBy(x => x.Ord).ToList();
            IList<Gwd> gwds = _db.Queryable<Gwd>().OrderBy(x => x.Ord).ToList();

            foreach (Tljxx tlj in tljs)
            {
                IList<TreeNode> chidrentree = new List<TreeNode>();//子节点集合
                foreach (Gwd gwd in gwds)
                {
                    if (tlj.Tljdm.Equals(gwd.Tljdm))
                    {
                        chidrentree.Add(new TreeNode() { Id = gwd.Id, NodeCode = gwd.Gwddm, NodeName = gwd.Gwdmc, Type = "GWD", FatherNodeCode = tlj.Tljdm });
                    }
                }
                TreeNode father = new TreeNode() { Id = tlj.Id, NodeCode = tlj.Tljdm, NodeName = tlj.Tljmc, Type = "TLJ", Children = chidrentree };
                trees.Add(father);
            }
            fatrees.Add(new TreeNode() { Id = 101, NodeCode = "ZGS", NodeName = "国铁集团", Type = "ZGS", Children = trees });
            return new { success = true, data = fatrees };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    /// <summary>
    /// 树结构(铁路局->电务段)
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "dwd_tree")]
    [DisplayName("树结构(铁路局->电务段)")]
    [Route("/api/Tree/FindDwdTree")]
    public dynamic FindDwdTree()
    {
        try
        {
            //var trees = TreeService.FindDwdTree();
            //管辖的所有铁路局
            IList<TreeNode> fatrees = new List<TreeNode>();
            IList<TreeNode> trees = new List<TreeNode>();
            //IList<Tljxx> tljs = TljxxService.FindAll().OrderBy(p => p.Ord).ToList();//查询所有铁路局Tljxx
            //IList<Dwd> dwds = dwdService.FindAll().OrderBy(p => p.Ord).ToList();//查询所有工务段信息
            IList<Tljxx> tljs = _db.Queryable<Tljxx>().OrderBy(x => x.Ord).ToList();
            IList<Dwd> dwds = _db.Queryable<Dwd>().OrderBy(x => x.Ord).ToList();

            foreach (Tljxx tlj in tljs)
            {
                IList<TreeNode> chidrentree = new List<TreeNode>();//子节点集合
                foreach (Dwd dwd in dwds)
                {
                    if (tlj.Tljdm.Equals(dwd.Tljdm))
                    {
                        chidrentree.Add(new TreeNode() { Id = dwd.Id, NodeCode = dwd.Dwddm, NodeName = dwd.Dwdmc, Type = "DWD", FatherNodeCode = tlj.Tljdm });
                    }
                }
                TreeNode father = new TreeNode() { Id = tlj.Id, NodeCode = tlj.Tljdm, NodeName = tlj.Tljmc, Type = "TLJ", Children = chidrentree };
                trees.Add(father);
            }
            fatrees.Add(new TreeNode() { Id = 101, NodeCode = "ZGS", NodeName = "国铁集团", Type = "ZGS", Children = trees });
            return new { success = true, data = fatrees };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    /// <summary>
    /// 树结构(铁路局->车务站段)
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "cwzd_tree")]
    [DisplayName("树结构(铁路局->车务站段)")]
    [Route("/api/Tree/FindCwzdTree")]
    public dynamic FindCwzdTree()
    {
        try
        {
            //var trees = TreeService.FindCwzdTree();
            //管辖的所有铁路局
            IList<TreeNode> fatrees = new List<TreeNode>();
            IList<TreeNode> trees = new List<TreeNode>();
            //IList<Tljxx> tljs = TljxxService.FindAll().OrderBy(p => p.Ord).ToList();//查询所有铁路局Tljxx
            //IList<Cwzd> cwzds = cwzdService.FindAll().OrderBy(p => p.Ord).ToList();//查询所有车务站段信息
            IList<Tljxx> tljs = _db.Queryable<Tljxx>().OrderBy(x => x.Ord).ToList();
            IList<Cwzd> cwzds = _db.Queryable<Cwzd>().OrderBy(x => x.Ord).ToList();

            foreach (Tljxx tlj in tljs)
            {
                IList<TreeNode> chidrentree = new List<TreeNode>();//子节点集合
                foreach (Cwzd cwzd in cwzds)
                {
                    if (tlj.Tljdm.Equals(cwzd.Tljdm))
                    {
                        chidrentree.Add(new TreeNode() { Id = cwzd.Id, NodeCode = cwzd.Cwzddm, NodeName = cwzd.Cwzdmc, Type = "CWZD", FatherNodeCode = tlj.Tljdm });
                    }
                }
                TreeNode father = new TreeNode() { Id = tlj.Id, NodeCode = tlj.Tljdm, NodeName = tlj.Tljmc, Type = "TLJ", Children = chidrentree };
                trees.Add(father);
            }
            fatrees.Add(new TreeNode() { Id = 101, NodeCode = "ZGS", NodeName = "国铁集团", Type = "ZGS", Children = trees });
            return new { success = true, data = fatrees };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    /// <summary>
    /// 树结构(铁路局->统计区段)
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "qdbm_tree")]
    [DisplayName("树结构(铁路局->统计区段)")]
    [Route("/api/Tree/FindQdbmTree1")]
    public dynamic FindQdbmTree1(string param)
    {
        try
        {
            //管辖的所有铁路局
            IList<TreeNode> fatrees = new List<TreeNode>();
            IList<TreeNode> trees = new List<TreeNode>();
            IList<Tljxx> tljs = new List<Tljxx>();
            IList<Qdbm> qddms = new List<Qdbm>();
            if (string.IsNullOrWhiteSpace(param))
            {
                //tljs = TljxxService.FindAll().OrderBy(p => p.Ord).ToList();//查询所有铁路局dic_tlj
                //qddms = qdbmService.FindAll().OrderBy(p => p.Qddm).ToList();//查询所有统计区段信息
                tljs = _db.Queryable<Tljxx>().OrderBy(p => p.Ord).ToList();//查询所有铁路局dic_tlj
                qddms = _db.Queryable<Qdbm>().OrderBy(p => p.Qddm).ToList();//查询所有统计区段信息
            }
            else
            {
                //qddms = qdbmService.FindAll().Where(b => b.Qddm.Contains(param.ToUpper())).OrderBy(p => p.Ord).ToList();
                qddms = _db.Queryable<Qdbm>().Where(b => b.Qddm.Contains(param.ToUpper())).OrderBy(p => p.Qddm).ToList();
                var tljdms = qddms.Select(b => b.Tljdm).Distinct();
                //tljs = TljxxService.FindAll().Where(b => tljdms.Contains(b.Tljdm)).OrderBy(p => p.Ord).ToList();
                tljs = _db.Queryable<Tljxx>().Where(b => tljdms.Contains(b.Tljdm)).OrderBy(p => p.Ord).ToList();
            }
            foreach (Tljxx tlj in tljs)
            {
                IList<TreeNode> chidrentree = new List<TreeNode>();//子节点集合
                foreach (Qdbm qddm in qddms)
                {
                    if (tlj.Tljdm.Equals(qddm.Tljdm))
                    {
                        var Qdmc = "";
                        if (qddm.Qdmch == qddm.Qdmck)
                        {
                            Qdmc = qddm.Qddm + "（" + qddm.Qdmch + ")";
                        }
                        else if (qddm.Qdmch != qddm.Qdmck)
                        {
                            IList<string> listh1 = qddm.Qdmch.Split('-');
                            IList<string> listh2 = qddm.Qdmch.Split(' ');
                            IList<string> listk1 = qddm.Qdmck.Split('-');
                            IList<string> listk2 = qddm.Qdmck.Split(' ');
                            string cfzh = "";
                            string ddzh = "";
                            string cfzk = "";
                            string ddzk = "";
                            if (listh1.Count == 2)
                            {
                                cfzh = listh1[0];
                                ddzh = listh1[1];
                            }
                            else if (listh2.Count == 2)
                            {
                                cfzh = listh2[0];
                                ddzh = listh2[1];
                            }
                            if (listk1.Count == 2)
                            {
                                cfzk = listk1[0];
                                ddzk = listk1[1];
                            }
                            else if (listk2.Count == 2)
                            {
                                cfzk = listk2[0];
                                ddzk = listk2[1];
                            }
                            if (cfzh == cfzk && ddzk == ddzh)
                            {
                                Qdmc = qddm.Qddm + "（" + qddm.Qdmch + ")";
                            }
                            else
                            {
                                Qdmc = qddm.Qddm + "(" + qddm.Qdmck + "(客)/" + qddm.Qdmch + "(货))";
                            }

                        }
                        chidrentree.Add(new TreeNode() { Id = qddm.Id, NodeCode = qddm.Qddm, NodeName = Qdmc, Type = "QDDM", FatherNodeCode = tlj.Tljdm });
                    }
                }
                TreeNode father = new TreeNode() { Id = tlj.Id, NodeCode = tlj.Tljdm, NodeName = tlj.Tljmc, Type = "TLJ", Children = chidrentree };
                trees.Add(father);
            }
            fatrees.Add(new TreeNode() { Id = 101, NodeCode = "ZGS", NodeName = "国铁集团", Type = "ZGS", Children = trees });
            return new { success = true, data = fatrees };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 树结构(铁路局)
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "tlj_tree")]
    [DisplayName("树结构(铁路局)")]
    [Route("/api/Tree/FindTljTree")]
    public dynamic FindTljTree()
    {
        try
        {
            //var trees = TreeService.FindTljTree();
            //IList<Tljxx> tljxxs = TljxxService.FindAll().OrderBy(p => p.Ord).ToList();
            IList<Tljxx> tljxxs = _db.Queryable<Tljxx>().OrderBy(x => x.Ord).ToList();
            IList<TreeNode> fatrees = new List<TreeNode>();
            IList<TreeNode> chidrentree = new List<TreeNode>();//子节点集合
            foreach (Tljxx tljxx in tljxxs)
            {
                chidrentree.Add(new TreeNode() { Id = tljxx.Id, NodeCode = tljxx.Tljdm, NodeName = tljxx.Tljmc, Type = "TLJ" });
            }
            fatrees.Add(new TreeNode() { Id = 1, NodeCode = "ZGS", NodeName = "国铁集团", Type = "ZGS", Children = chidrentree });
            return new { success = true, data = fatrees };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 树结构(铁路局->机务段)
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "jwd_tree")]
    [DisplayName("树结构(铁路局->机务段)")]
    [Route("/api/Tree/FindJwdTree1")]
    public dynamic FindJwdTree()
    {
        try
        {
            //var trees = TreeService.FindJwdTree();
            //管辖的所有铁路局
            IList<TreeNode> fatrees = new List<TreeNode>();
            IList<TreeNode> trees = new List<TreeNode>();
            //IList<Tljxx> tljs = TljxxService.FindAll().OrderBy(p => p.Ord).ToList();
            //IList<Jwdxx> jwdxxs = JwdxxService.FindAll().OrderBy(p => p.Ord).ToList();
            IList<Tljxx> tljs = _db.Queryable<Tljxx>().OrderBy(p => p.Ord).ToList();
            IList<Jwdxx> jwdxxs = _db.Queryable<Jwdxx>().OrderBy(p => p.Ord).ToList();
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
            return new { success = true, data = fatrees };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 树结构(铁路局->合资公司/地方铁路)
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "qdbm_tree")]
    [DisplayName("树结构(铁路局->合资公司/地方铁路)")]
    [Route("/api/Tree/FindQdbmTree")]
    public dynamic FindQdbmTree()
    {
        try
        {
            //管辖的所有铁路局
            IList<TreeNode> fatrees = new List<TreeNode>();
            IList<TreeNode> trees = new List<TreeNode>();
            //IList<Tljxx> tljs = TljxxService.FindAll().OrderBy(p => p.Ord).ToList();//查询所有铁路局dic_tlj
            IList<Tljxx> tljs = _db.Queryable<Tljxx>().OrderBy(p => p.Ord).ToList();//查询所有铁路局dic_tlj
            //IList<Hzgs> hzgss = HzgsService.FindAll().OrderBy(p => p.Ord).ToList(); //查询所有合资公司信息
            //IList<Dftl> dftls = DftlService.FindAll().OrderBy(p => p.Ord).ToList(); //查询所有地方铁路信息
            IList<Hzgs> hzgss = _db.Queryable<Hzgs>().OrderBy(p => p.Ord).ToList(); //查询所有合资公司信息
            IList<Dftl> dftls = _db.Queryable<Dftl>().OrderBy(p => p.Ord).ToList(); //查询所有地方铁路信息
            foreach (Tljxx tlj in tljs)
            {
                TreeNode fathertree = new TreeNode() { Id = tlj.Id, NodeCode = tlj.Tljdm, NodeName = tlj.Tljmc };//父节点
                IList<TreeNode> chidrentree = new List<TreeNode>();//子节点集合
                foreach (Hzgs hzgs in hzgss)
                {
                    if (tlj.Tljdm.Equals(hzgs.Tljdm))
                    {
                        chidrentree.Add(new TreeNode() { Id = hzgs.Id, NodeCode = hzgs.Dm, NodeName = hzgs.Mc, Type = "YSQY", Father = fathertree });
                    }
                }
                foreach (Dftl dftl in dftls)
                {
                    if (tlj.Tljdm.Equals(dftl.Tljdm))
                    {
                        chidrentree.Add(new TreeNode() { Id = dftl.Id, NodeCode = dftl.Dm, NodeName = dftl.Mc, Type = "YSQY", Father = fathertree });
                    }
                }
                TreeNode father = new TreeNode() { Id = tlj.Id, NodeCode = tlj.Tljdm, NodeName = tlj.Tljmc, Type = "TLJ", Children = chidrentree };
                trees.Add(father);
            }
            fatrees.Add(new TreeNode() { Id = 101, NodeCode = "ZGS", NodeName = "国铁集团", Type = "ZGS", Children = trees });
            return new { success = true, data = fatrees };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 树结构(用户权限)
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "user_tree")]
    [DisplayName("树结构(用户权限)")]
    [Route("/api/Tree/FindUserTree")]
    public dynamic FindUserTree(string tlj, string jwd, string cj, string type, string username)
    {
        try
        {
            //var trees = TreeService.FindUserTree(tlj, jwd, cj, type, username);
            var trees = FindUserTreePrivate(tlj, jwd, cj, type, username);
            return new { success = true, data = trees };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    private object FindUserTreePrivate(string stlj, string sjwd, string scj, string type, string username)
    {
        //管辖的所有铁路局
        IList<TreeNode> fatrees = new List<TreeNode>();
        IList<TreeNode> trees = new List<TreeNode>();
        IList<Tljxx> tljs = new List<Tljxx>();
        IList<Jwdxx> jwdxxs = new List<Jwdxx>();
        IList<Cjxx> cjxxs = new List<Cjxx>();
        if (type == "0" || username == "admin" || username == "system")
        {
            //tljs = TljxxService.FindAll().OrderBy(p => p.Ord).ToList();//查询所有铁路局dic_tlj
            //jwdxxs = JwdxxService.FindAll().OrderBy(p => p.Ord).ToList();//查询所有机务段信息
            //cjxxs = CjxxService.FindAll().OrderBy(p => p.Ord).ToList();//查询所有车间信息
            tljs=_db.Queryable<Tljxx>().OrderBy(p => p.Ord).ToList();//查询所有铁路局dic_tlj
            jwdxxs = _db.Queryable<Jwdxx>().OrderBy(p => p.Ord).ToList();//查询所有机务段信息
            cjxxs = _db.Queryable<Cjxx>().OrderBy(p => p.Ord).ToList();//查询所有车间信息
        }
        else if (type == "1")
        {
            //tljs = TljxxService.FindAll().Where(b => b.Tljdm == stlj).OrderBy(p => p.Ord).ToList();//查询铁路局dic_tlj
            //jwdxxs = JwdxxService.FindAll().Where(b => b.Tljdm == stlj).OrderBy(p => p.Ord).ToList();//查询局机务段信息
            //cjxxs = CjxxService.FindAll().Where(b => jwdxxs.Select(d => d.Jwddm).Contains(b.Ddm)).OrderBy(p => p.Ord).ToList();//查询段车间信息
            tljs = _db.Queryable<Tljxx>().Where(b => b.Tljdm == stlj).OrderBy(p => p.Ord).ToList();//查询铁路局dic_tlj
            jwdxxs = _db.Queryable<Jwdxx>().Where(b => b.Tljdm == stlj).OrderBy(p => p.Ord).ToList();//查询局机务段信息
            cjxxs = _db.Queryable<Cjxx>().Where(b => jwdxxs.Select(d => d.Jwddm).Contains(b.Ddm)).OrderBy(p => p.Ord).ToList();//查询段车间信息
        }
        else if (type == "2")
        {
            jwdxxs = _db.Queryable<Jwdxx>().Where(b => b.Jwddm == sjwd).OrderBy(p => p.Ord).ToList();//查询机务段信息
            tljs = _db.Queryable<Tljxx>().Where(b => jwdxxs.FirstOrDefault().Tljdm == b.Tljdm).OrderBy(p => p.Ord).ToList();//查询铁路局dic_tlj
            cjxxs = _db.Queryable<Cjxx>().Where(b => b.Ddm == sjwd).OrderBy(p => p.Ord).ToList();//查询段车间信息
        }
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
    /// 树结构(监控交路单位)
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "jkjldw_tree")]
    [DisplayName("树结构(监控交路单位)")]
    [Route("/api/Tree/FindJkjldwTree")]
    public dynamic FindJkjldwTree()
    {
        try
        {
            //管辖的所有铁路局
            IList<TreeNode> fatrees = new List<TreeNode>();
            IList<TreeNode> trees = new List<TreeNode>();
            //IList<Tljxx> tljs = TljxxService.FindAll().OrderBy(p => p.Ord).ToList();
            //IList<Jwdxx> jwdxxs = JwdxxService.FindAll().OrderBy(p => p.Ord).ToList();
            //IList<Jkjldw> jkjldws = JkjldwService.FindAll().OrderBy(p => p.Ord).ToList();
            IList<Tljxx> tljs = _db.Queryable<Tljxx>().OrderBy(p => p.Ord).ToList();
            IList<Jwdxx> jwdxxs = _db.Queryable<Jwdxx>().OrderBy(p => p.Ord).ToList();
            IList<Jkjldw> jkjldws = _db.Queryable<Jkjldw>().OrderBy(p => p.Ord).ToList();
            foreach (Tljxx tlj in tljs)
            {
                TreeNode fathertree = new TreeNode() { Id = tlj.Id, NodeCode = tlj.Tljdm, NodeName = tlj.Tljmc };//父节点
                IList<TreeNode> chidrentree = new List<TreeNode>();//子节点集合
                foreach (Jwdxx jwd in jwdxxs)
                {
                    if (tlj.Tljdm.Equals(jwd.Tljdm))
                    {
                        var jwdNode = new TreeNode() { Id = jwd.Id, NodeCode = jwd.Jwddm, NodeName = jwd.Jwdmc, Type = "JWD", Father = fathertree };
                        chidrentree.Add(jwdNode);
                        //监控交路单位
                        IList<TreeNode> jkjldwNodes = new List<TreeNode>();
                        foreach (Jkjldw jkjldw in jkjldws)
                        {
                            if (jkjldw.Ddm.Equals(jwd.Jwddm))
                            {
                                var jkjldwNode = new TreeNode()
                                {
                                    Id = jkjldw.Id,
                                    NodeCode = jkjldw.Jkjldwdm,
                                    NodeName = jkjldw.Jkjldwmc,
                                    Type = "JKJLDW",
                                    Father = new TreeNode()
                                    {
                                        Id = jwd.Id,
                                        NodeCode = jwd.Jwddm,
                                        NodeName = jwd.Jwdmc
                                    },
                                    Father1 = fathertree
                                };
                                jkjldwNodes.Add(jkjldwNode);
                            }
                        }

                        jwdNode.Children = jkjldwNodes;

                    }
                }
                TreeNode father = new TreeNode() { Id = tlj.Id, NodeCode = tlj.Tljdm, NodeName = tlj.Tljmc, Type = "TLJ", Children = chidrentree };
                trees.Add(father);
            }
            fatrees.Add(new TreeNode() { Id = 101, NodeCode = "ZGS", NodeName = "国铁集团", Type = "ZGS", Children = trees });
            return new { success = true, data = fatrees };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 树结构(出入段地点)
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "crddd_tree")]
    [DisplayName("树结构(出入段地点)")]
    [Route("/api/Tree/FindCrdddTree")]
    public dynamic FindCrdddTree(string param)
    {
        try
        {
            //var trees = TreeService.FindCrdddTree(param);
            JObject json = JObject.Parse(param);
            string Ddm = json.Value<string>("Ddm");
            //IList<Crddd> crddds = CrdddxxService.FindAll().Where(p => p.Ddm == Ddm).OrderBy(p => p.Ord).ToList();
            IList<Crddd> crddds = _db.Queryable<Crddd>().Where(p => p.Ddm == Ddm).OrderBy(p => p.Ord).ToList();
            IList<TreeNode> fatrees = new List<TreeNode>();
            foreach (Crddd crddd in crddds)
            {
                fatrees.Add(new TreeNode() { Id = crddd.Id, NodeCode = crddd.Dm, NodeName = crddd.Mc, Type = "Crddd" });
            }
            //var defaultChecked = UserCrdddRepository.FindAllByCondition("select * from dic_user_crddd");
            var defaultChecked = _db.Queryable<UserCrddd>().ToList();
            return new { success = true, data = fatrees, defaultChecked = defaultChecked };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 树结构(轮乘换班地点)
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "lchbdd_tree")]
    [DisplayName("树结构(轮乘换班地点)")]
    [Route("/api/Tree/FindLchbddTree")]
    public dynamic FindLchbddTree(string param)
    {
        try
        {
            //var trees = TreeService.FindLchbddTree(param);
            JObject json = JObject.Parse(param);
            string Ddm = json.Value<string>("Ddm");
            //IList<Cjlhbddxx> Cjlhbdds = CjlhbddxxService.FindAll().Where(p => p.Ddm == Ddm).OrderBy(p => p.Ord).ToList();
            IList<Cjlhbddxx> Cjlhbdds = _db.Queryable<Cjlhbddxx>().Where(p => p.Ddm == Ddm).OrderBy(p => p.Ord).ToList();
            IList<TreeNode> fatrees = new List<TreeNode>();
            foreach (Cjlhbddxx Cjlhbdd in Cjlhbdds)
            {
                fatrees.Add(new TreeNode() { Id = Cjlhbdd.Id, NodeCode = Cjlhbdd.Dm, NodeName = Cjlhbdd.Mc, Type = "Cjlhbdd" });
            }
            //var defaultChecked = UserCrdddRepository.FindAllByCondition("select * from dic_user_cjlhbddxx");
            var defaultChecked = _db.Queryable<UserCjlhbddxx>().ToList();
            return new { success = true, data = fatrees, defaultChecked = defaultChecked };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    /// <summary>
    /// 树结构(轮乘换班地点)
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "xbqd_tree")]
    [DisplayName("树结构(轮乘换班地点)")]
    [Route("/api/Tree/FindXbdmTree")]
    public dynamic FindXbqdTree()
    {
        try
        {
            //方法在老项目中没有实现
            //var trees = TreeService.FindXbqdTree();
            //return new { success = true, data = trees };
            return new { success = true, data = new List<TreeNode>() };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }

    /// <summary>
    /// 树结构(菜单目录)
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "menu_tree")]
    [Route("/api/Tree/FindAllMenuTree")]
    public dynamic FindAllMenuTree()
    {
        try
        {
            //var trees = TreeService.FindAllMenuTree();
            #region MyRegion
            //IList<MenuTreeGrid> menus = menuService.QueryTreeGrid();
            #region MyRegion
            IList<MenuTreeGrid> menus = new List<MenuTreeGrid>();
            //IList<Menu> menus1 = MenuRepository.FindAll().OrderBy(p => p.Ord).ToList();
            IList<Menu> menus1 = _db.Queryable<Menu>().OrderBy(x => x.Ord).ToList();
            for (int i = 0; i < menus1.Count; i++)
            {
                Menu menu = menus1[i];
                int childnum = menus1.Where(p => p.Parentid == menu.Id).Count();
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
                menus.Add(tree);
            }
            #endregion
            IList<MenuTreeGrid> menuf = menus.Where(o => o.parent_id == null).ToList();
            IList<MenuTreeGrid> mensz = menus.Where(o => o.parent_id != null).ToList();
            IList<TreeNode> fatrees = new List<TreeNode>();
            IList<TreeNode> trees = new List<TreeNode>();
            foreach (MenuTreeGrid menu in menuf)
            {
                TreeNode fathertree = new TreeNode() { Id = menu.id, NodeCode = menu.Name, NodeName = menu.Title };//xxxxxxxxxx
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
            #endregion

            return new { success = true, data = trees };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
    [HttpGet]
    public dynamic FindXlTree()
    {
        try
        {
            //IList<ISort> sorts = new List<ISort>(); ;
            //sorts.Add(new Sort { PropertyName = "Ord", Ascending = true });
            //IEnumerable<Xl> xls = XlService.FindAll(sorts);
            List<Xl> xls = _db.Queryable<Xl>().OrderBy(x => x.Ord).ToList();
            IList<TreeNode> nodes = new List<TreeNode>();
            foreach (var xl in xls)
            {
                TreeNode node = new TreeNode() { Id = xl.Id, NodeCode = xl.Dm, NodeName = xl.Mc, Type = "XL" };
                nodes.Add(node);
            }
            return new { success = true, data = nodes };
        }
        catch (Exception e)
        {
            return new { success = false, message = ExceptionUtil.GetExceptionMsg(e) };
        }
    }
}
