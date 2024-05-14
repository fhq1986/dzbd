// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using DapperExtensions;
using DapperExtensions.Predicate;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Util;
/// <summary>
/// 老项目中的DapperHelper内的方法移植成SQLSugar方法
/// </summary>
public class SqlSugarHelper
{
    public static bool GeneratePredicateAndSort<M>(string param, out List<IConditionalModel> where, out IList<ISort> sorts)
    {
        bool result = false;

        //var propertyNames = typeof(T).GetProperties().Select(o => o.Name);
        // 解析字符串
        if (string.IsNullOrEmpty(param)) param = "{}";
        try
        {
            List<string> propertyNames = typeof(M).GetProperties().Select(o => o.Name).ToList();
            JObject jo = JObject.Parse(param);
            IEnumerable<JProperty> properties = jo.Properties();
            where = new List<IConditionalModel>();
            //建立查询条件
            foreach (JProperty item in properties.Where(x => propertyNames.Contains(x.Name)))
            {
                where.Add(new ConditionalModel { FieldName = item.Name, ConditionalType = ConditionalType.Equal, FieldValue = item.Value + "" });
            }
            if (jo.ContainsKey("Sort") && jo["Sort"] is JArray && (jo["Sort"] as JArray).Count > 0)
            {
                // sorts = jParam["Sort"].ToObject<List<Sort>>() as IList<ISort>;
                sorts = jo.Value<JArray>("Sort").ToObject<List<Sort>>().ToList<ISort>();
            }
            else
            {
                sorts = null;
            }
            result = true;
        }
        catch (Exception e)
        {
            where = null;
            sorts = null;
            throw e;
        }
        return result;
    }

    public static bool GeneratePredicateAndSort<T>(string param, out List<IConditionalModel> where, out IList<ISort> sorts, out int pageIndex, out int pageSize) where T : class
    {
        bool result = false;

        var propertyNames = typeof(T).GetProperties().Select(o => o.Name);
        // 解析字符串
        if (string.IsNullOrEmpty(param)) param = "{}";
        try
        {
            JObject jParam = JObject.Parse(param);
            where = new List<IConditionalModel>();

            //建立查询条件
            foreach (JProperty item in jParam.Properties().Where(x => propertyNames.Contains(x.Name)))
            {
                where.Add(new ConditionalModel { FieldName = item.Name, ConditionalType = ConditionalType.Equal, FieldValue = item.Value + "" });
            }
            if (jParam.ContainsKey("Sort") && jParam["Sort"] is JArray && (jParam["Sort"] as JArray).Count > 0)
            {
                // sorts = jParam["Sort"].ToObject<List<Sort>>() as IList<ISort>;
                sorts = jParam.Value<JArray>("Sort").ToObject<List<Sort>>().ToList<ISort>();
            }
            else
            {
                sorts = null;
            }

            //分页模式
            pageIndex = 1;
            pageSize = 10;
            if (jParam.ContainsKey("pageIndex") && !string.IsNullOrEmpty(jParam.Value<string>("pageIndex")) &&
                jParam.ContainsKey("pageSize") && !string.IsNullOrEmpty(jParam.Value<string>("pageSize")))
            {
                pageIndex = jParam.Value<int>("pageIndex");
                pageSize = jParam.Value<int>("pageSize");
            }

            result = true;
        }
        catch (Exception e)
        {
            throw e;
        }
        return result;
    }
}
