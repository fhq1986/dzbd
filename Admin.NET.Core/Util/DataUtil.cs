// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Util;
public class DataUtil
{
    public static DataTable TsvToDataTable<T>(string tsv)
    {
        DataTable dt = new DataTable();
        var properties = typeof(T).GetProperties();
        var lines = tsv.Split('\n');
        var unMatchColumnNames = new List<string>();
        try
        {
            var columnNames = lines[0].Split('\t');
            foreach (string columnName in columnNames)
            {
                var property = properties.Where(p => p.Name.Equals(columnName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                if (null != property)
                {
                    var t = property.PropertyType;
                    // We need to check whether the property is NULLABLE
                    if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        t = t.GetGenericArguments()[0];
                    }
                    DataColumn dateColumn = new DataColumn(columnName.ToUpper(), t);
                    dateColumn.AutoIncrement = false;
                    dateColumn.AllowDBNull = true;
                    dt.Columns.Add(dateColumn);
                }
                else
                {
                    //记录不匹配的字段
                    unMatchColumnNames.Add(columnName.ToUpper());
                    DataColumn dateColumn = new DataColumn(columnName.ToUpper());
                    dateColumn.AutoIncrement = false;
                    dateColumn.AllowDBNull = true;
                    dt.Columns.Add(dateColumn);
                }
            }
            bool hasFqbs = false;
            //如果为分区表则增加fqbs数据
            if (dt.Columns.IndexOf("XTRQ") >= 0 && null != properties.Where(p => p.Name.Equals("Fqbs", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault())
            {
                DataColumn dateColumn = new DataColumn("FQBS");
                dateColumn.DataType = typeof(DateTime);
                dt.Columns.Add(dateColumn);
                hasFqbs = true;
            }
            for (int i = 1; i < lines.Count(); i++)
            {
                var records = lines[i].Split('\t');
                //dt.Rows.Add(records);
                var dr = dt.NewRow();
                for (int j = 0; j < records.Count(); j++)
                {
                    if (dt.Columns[j].DataType != typeof(string) && records[j] == "")
                    {
                        continue;
                    }
                    try
                    {
                        dr[j] = records[j];
                    }
                    catch (Exception e)
                    {
                        string s = e.Message;
                    }
                }
                dt.Rows.Add(dr);
                if (hasFqbs)
                {
                    dt.Rows[dt.Rows.Count - 1]["FQBS"] = DateTime.ParseExact(dt.Rows[dt.Rows.Count - 1].Field<string>("XTRQ"), "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
                }
            }
            //删除不匹配的列
            foreach (var columnName in unMatchColumnNames)
            {
                dt.Columns.Remove(columnName);

            }
        }
        catch (Exception e)
        {
            throw e;
        }
        return dt;
    }
}
