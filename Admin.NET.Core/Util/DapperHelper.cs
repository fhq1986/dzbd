// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using DapperExtensions.Predicate;
using DapperExtensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Util;
public  class DapperHelper
{
    private static Expression<Func<T, object>> GetExpression<T>(string propertyName)
    {
        ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
        return Expression.Lambda<Func<T, object>>(Expression.Convert(Expression.Property(parameter, propertyName), typeof(object)), parameter);
    }
    public static bool GeneratePredicateAndSort<T>(string param, out PredicateGroup predicateGroup, out IList<ISort> sorts) where T : class
    {
        bool result = false;

        var propertyNames = typeof(T).GetProperties().Select(o => o.Name);
        // 解析字符串
        if (string.IsNullOrEmpty(param)) param = "{}";
        try
        {
            JObject jParam = JObject.Parse(param);
            //建立查询条件
            predicateGroup = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            foreach (var prop in jParam.Properties().Where(p => propertyNames.Contains(p.Name)))
            {
                if (prop.Value is JArray)
                {
                    var pga = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
                    foreach (var value in prop.Value as JArray)
                    {
                        var predicate = Predicates.Field<T>(GetExpression<T>(prop.Name), Operator.Eq, (value as JValue).Value);
                        pga.Predicates.Add(predicate);
                    }
                    predicateGroup.Predicates.Add(pga);
                }
                else if (prop.Value is JValue)
                {
                    var predicate = Predicates.Field<T>(GetExpression<T>(prop.Name), Operator.Eq, (prop.Value as JValue).Value);
                    predicateGroup.Predicates.Add(predicate);
                }
                else if (prop.Value is JObject)
                {
                    var pga = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
                    foreach (var prop2 in (prop.Value as JObject).Properties().Where(p => new string[] { "Le", "Ge", "Lt", "Gt" }.Contains(p.Name)))
                    {
                        var predicate = Predicates.Field<T>(GetExpression<T>(prop.Name), (Operator)Enum.Parse(typeof(Operator), prop2.Name), (prop2.Value as JValue).Value);
                        pga.Predicates.Add(predicate);
                    }
                    predicateGroup.Predicates.Add(pga);
                }

            }

            //建立排序表达式
            sorts = new List<ISort>();
            if (jParam.ContainsKey("Sort") && jParam["Sort"] is JArray && (jParam["Sort"] as JArray).Count > 0)
            {
                // sorts = jParam["Sort"].ToObject<List<Sort>>() as IList<ISort>;
                sorts = jParam.Value<JArray>("Sort").ToObject<List<Sort>>().ToList<ISort>();
            }
            result = true;
        }
        catch (Exception e)
        {
            throw e;
        }
        return result;

    }

    public static bool GeneratePredicateAndSort<T>(string param, out PredicateGroup predicateGroup, out IList<ISort> sorts, out int pageIndex, out int pageSize) where T : class
    {
        bool result = false;

        var propertyNames = typeof(T).GetProperties().Select(o => o.Name);
        // 解析字符串
        if (string.IsNullOrEmpty(param)) param = "{}";
        try
        {
            JObject jParam = JObject.Parse(param);
            //建立查询条件
            predicateGroup = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            foreach (var prop in jParam.Properties().Where(p => propertyNames.Contains(p.Name)))
            {
                if (prop.Value is JArray)
                {
                    var pga = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
                    foreach (var value in prop.Value as JArray)
                    {
                        var predicate = Predicates.Field<T>(GetExpression<T>(prop.Name), Operator.Eq, (value as JValue).Value);
                        pga.Predicates.Add(predicate);
                    }
                    predicateGroup.Predicates.Add(pga);
                }
                else if (prop.Value is JValue)
                {
                    var predicate = Predicates.Field<T>(GetExpression<T>(prop.Name), Operator.Eq, (prop.Value as JValue).Value);
                    predicateGroup.Predicates.Add(predicate);
                }
                else if (prop.Value is JObject)
                {
                    var pga = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
                    foreach (var prop2 in (prop.Value as JObject).Properties().Where(p => new string[] { "Le", "Ge", "Lt", "Gt" }.Contains(p.Name)))
                    {
                        var predicate = Predicates.Field<T>(GetExpression<T>(prop.Name), (Operator)Enum.Parse(typeof(Operator), prop2.Name), (prop2.Value as JValue).Value);
                        pga.Predicates.Add(predicate);
                    }
                    predicateGroup.Predicates.Add(pga);
                }

            }

            //建立排序表达式
            sorts = new List<ISort>();
            if (jParam.ContainsKey("Sort") && jParam["Sort"] is JArray && (jParam["Sort"] as JArray).Count > 0)
            {
                // sorts = jParam["Sort"].ToObject<List<Sort>>() as IList<ISort>;
                sorts = jParam.Value<JArray>("Sort").ToObject<List<Sort>>().ToList<ISort>();
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
