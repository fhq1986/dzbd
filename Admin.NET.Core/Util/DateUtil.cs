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
public class DateUtil
{
    public static DateTime getDateTime(String datestring)
    {
        DateTime dtStart;
        DateTime.TryParseExact(datestring, "yyyyMMddHHmm", System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out dtStart);
        return dtStart;
    }
    public static DateTime getDateTime(String s, string format)
    {
        DateTime dt;
        DateTime.TryParseExact(s, format, System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out dt);
        return dt;
    }
    public static DateTime getDayTime(String datestring)
    {
        DateTime dtStart;
        DateTime.TryParseExact(datestring, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out dtStart);
        return dtStart;
    }

    public static DateTime getSecTime(string datestring)
    {
        DateTime dtStart;
        DateTime.TryParseExact(datestring, "MM-dd-yy  hh;mmPM", System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out dtStart);
        return dtStart;
    }

    public static DateTime getSecTime1(string datestring)
    {
        DateTime dtStart;
        DateTime.TryParseExact(datestring, "yyyy - MM - dd HH: mm:ss", System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out dtStart);
        return dtStart;
    }

    /// <summary>
    /// 得到当前系统日期
    /// </summary>
    /// <returns></returns>
    public static string GetXtrq()
    {
        return DateTime.Now.ToString("yyyyMMdd");
    }

    /// <summary>
    /// 获得两个字符串日期之间的日期集合
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public static IList<string> getDayList(string start, string end)
    {
        DateTime startTime = Convert.ToDateTime(start);
        DateTime endTime = Convert.ToDateTime(end);
        IList<string> dateList = new List<string>();
        for (DateTime i = startTime; i <= endTime; i = i.AddDays(1))
        {
            dateList.Add(i.ToString("yyyy-MM-dd"));
        }
        return dateList;
    }
    public static string GetReportBblb(string bblb)
    {
        if (bblb == "R") bblb = "日报";
        if (bblb == "X") bblb = "旬报";
        if (bblb == "Y") bblb = "月报";
        if (bblb == "J") bblb = "季报";
        if (bblb == "N") bblb = "年报";
        if (bblb == "D") bblb = "定制";
        return bblb;
    }
    public static bool GetReportDate(string bblb, string date, out string startDate, out string endDate, out string dateLabel)
    {
        bool result = false;
        startDate = "";
        endDate = "";
        dateLabel = "";
        DateTime dtDate;
        if (!DateTime.TryParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out dtDate))
        {
            return false;
        }
        if (bblb == "R")
        {
            startDate = date;
            endDate = date;
            dateLabel = $"{date.Substring(0, 4)}年{date.Substring(5, 2)}月{date.Substring(8, 2)}日";
        }
        else if (bblb == "X")
        {
            if (dtDate.Day >= 1 && dtDate.Day <= 10)
            {
                startDate = $"{date.Substring(0, 8)}01";
                endDate = $"{date.Substring(0, 8)}10";
                dateLabel = $"{date.Substring(0, 4)}年{date.Substring(5, 2)}月(上旬)";
            }
            else if (dtDate.Day >= 11 && dtDate.Day <= 20)
            {
                startDate = $"{date.Substring(0, 8)}11";
                endDate = $"{date.Substring(0, 8)}20";
                dateLabel = $"{date.Substring(0, 4)}年{date.Substring(5, 2)}月(中旬)";
            }
            else
            {
                startDate = $"{date.Substring(0, 8)}21";
                endDate = $"{date.Substring(0, 8)}{DateTime.DaysInMonth(dtDate.Year, dtDate.Month)}";
                dateLabel = $"{date.Substring(0, 4)}年{date.Substring(5, 2)}月(下旬)";
            }
            result = true;
        }
        else if (bblb == "Y")
        {
            startDate = $"{date.Substring(0, 8)}01";
            endDate = $"{date.Substring(0, 8)}{DateTime.DaysInMonth(dtDate.Year, dtDate.Month)}";
            dateLabel = $"{date.Substring(0, 4)}年{date.Substring(5, 2)}月";
            result = true;
        }
        else if (bblb == "J")
        {
            if (dtDate.Month >= 1 && dtDate.Month <= 3)
            {
                startDate = $"{date.Substring(0, 4)}-01-01";
                endDate = $"{date.Substring(0, 4)}-03-31";
                dateLabel = $"{date.Substring(0, 4)}年1季度";
            }
            else if (dtDate.Month >= 4 && dtDate.Month <= 6)
            {
                startDate = $"{date.Substring(0, 4)}-04-01";
                endDate = $"{date.Substring(0, 4)}-06-30";
                dateLabel = $"{date.Substring(0, 4)}年2季度";
            }
            else if (dtDate.Month >= 7 && dtDate.Month <= 9)
            {
                startDate = $"{date.Substring(0, 4)}-07-01";
                endDate = $"{date.Substring(0, 4)}-09-30";
                dateLabel = $"{date.Substring(0, 4)}年3季度";
            }
            else if (dtDate.Month >= 10 && dtDate.Month <= 12)
            {
                startDate = $"{date.Substring(0, 4)}-10-01";
                endDate = $"{date.Substring(0, 4)}-12-31";
                dateLabel = $"{date.Substring(0, 4)}年4季度";
            }
            result = true;
        }
        else if (bblb == "N")
        {
            startDate = $"{date.Substring(0, 4)}-01-01";
            endDate = $"{date.Substring(0, 4)}-12-31";
            dateLabel = $"{date.Substring(0, 4)}年";
            result = true;
        }
        return result;
    }
}
