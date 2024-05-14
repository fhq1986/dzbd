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
using Newtonsoft.Json.Linq;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.Util;
using System.Reflection;
using NPOI.XSSF.UserModel;

namespace Admin.NET.Core.Util;
public class ExcelHelper
{

    public static int MergedRegion(IWorkbook workbook, ISheet sheet, JObject cellCfg, int iCol, int iRow, IList<string> lstCols)
    {
        int result = 1;
        IRow row = sheet.GetRow(iRow);
        if (null == row)
        {
            row = sheet.CreateRow(iRow);
        }

        ICell cell = row.GetCell(iCol);
        if (null == cell)
        {
            cell = row.CreateCell(iCol);
        }
        cell.SetCellValue(cellCfg.Value<string>("caption"));

        ICellStyle cellstyle = workbook.CreateCellStyle();//设置垂直居中格式
        cellstyle.VerticalAlignment = VerticalAlignment.Center;//垂直居中
        cellstyle.Alignment = HorizontalAlignment.Center;//水平居中
        cellstyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
        cellstyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
        cellstyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
        cellstyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
        NPOI.SS.UserModel.IFont font = workbook.CreateFont();
        //设置字体加粗样式
        font.FontHeightInPoints = 11;
        font.IsBold = true;
        //使用SetFont方法将字体样式添加到单元格样式中 
        cellstyle.SetFont(font);
        cell.CellStyle = cellstyle;
        if (null != cellCfg["cols"] && cellCfg.Value<JArray>("cols").Count > 0)
        {
            int iSubCol = iCol;
            int iSubRow = iRow + cellCfg.Value<int>("rowspan");
            for (int i = 0; i < cellCfg.Value<JArray>("cols").Count; i++)
            {
                int iRowSpan = MergedRegion(workbook, sheet, cellCfg.Value<JArray>("cols")[i] as JObject, iSubCol, iSubRow, lstCols);
                //找出子表头中占用的行数
                if (iRowSpan > result)
                {
                    result = iRowSpan;
                }
                iSubCol = iSubCol + cellCfg["cols"][i].Value<int>("colspan");
            }
            result = result + cellCfg.Value<int>("rowspan");
            //通过子标题累计colspan
            cellCfg["colspan"] = iSubCol - iCol;
        }
        else
        {
            lstCols.Add(cellCfg.Value<string>("filedName"));
            if (null != cellCfg["width"] && cellCfg.Value<int>("width") > 0)
            {
                sheet.SetColumnWidth(iCol, (int)((cellCfg.Value<int>("width") + 0.72) * 256));
            }
            else
            {
                sheet.SetColumnWidth(iCol, (int)((9 + 0.72) * 256));
            }
            result = cellCfg.Value<int>("rowspan");
        }
        //存在合并单元格
        if (cellCfg.Value<int>("rowspan") > 1 || cellCfg.Value<int>("colspan") > 1)
        {
            CellRangeAddress region = new CellRangeAddress(iRow, iRow + cellCfg.Value<int>("rowspan") - 1,
                iCol, iCol + cellCfg.Value<int>("colspan") - 1);
            sheet.AddMergedRegion(region);
            ((HSSFSheet)sheet).SetEnclosedBorderOfRegion(region, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.HSSF.Util.HSSFColor.Black.Index);
        }
        return result;
    }

    public static int CreatHeader(IWorkbook workbook, ISheet sheet, JObject cfgHeader, IList<string> lstCols)
    {
        int result = 1;
        IRow row = sheet.CreateRow(0);
        ICell cell = row.CreateCell(0);
        cell.SetCellValue(cfgHeader.Value<string>("header"));

        ICellStyle cellstyle = workbook.CreateCellStyle();//设置垂直居中格式
        cellstyle.VerticalAlignment = VerticalAlignment.Center;//垂直居中
        cellstyle.Alignment = HorizontalAlignment.Center;//水平居中
        NPOI.SS.UserModel.IFont font = workbook.CreateFont();
        //设置字体加粗样式
        font.FontHeightInPoints = 18;
        font.IsBold = true;
        //使用SetFont方法将字体样式添加到单元格样式中 
        cellstyle.SetFont(font);
        cell.CellStyle = cellstyle;
        if (null != cfgHeader["cols"] && cfgHeader.Value<JArray>("cols").Count > 0)
        {
            int iCol = 0;
            int iRow = 1;
            for (int i = 0; i < cfgHeader.Value<JArray>("cols").Count; i++)
            {
                int iRowSpan = MergedRegion(workbook, sheet, cfgHeader.Value<JArray>("cols")[i] as JObject, iCol, iRow, lstCols);
                if (iRowSpan > result)
                {
                    result = iRowSpan;
                }
                iCol = iCol + cfgHeader["cols"][i].Value<int>("colspan");
            }
            result = result + 1;
            //合并标题
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, iCol - 1));
        }
        return result;
    }
    /// <summary>
    /// 复杂表头导出Excel
    /// </summary>
    /// <param name="TData">数据源</param>
    /// <param name="filePath">文件存放路径</param>
    /// <param name="jsonfilepath">json文件路径 如："G:\WorkSite\WorkS\Cwyjjgs\Cwyjjgs\Scripts\app\data\datxpjl.json"</param>
    public static void SaveListDataToExcelFileNPOI(IList<dynamic> listdata, string filePath, JObject jsonHeader)
    {
        List<string> lstCols = new List<string>();
        try
        {
            if (null == jsonHeader) return;
            IWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();
            //建立表头
            int iDataRow = CreatHeader(workbook, sheet, jsonHeader, lstCols);
            ICellStyle cellstyle = workbook.CreateCellStyle();
            cellstyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            cellstyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            cellstyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            cellstyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            sheet.CreateFreezePane(0, iDataRow);

            for (int i = 0; i < listdata.Count; i++)
            {
                IRow row = sheet.CreateRow(i + iDataRow);
                for (int j = 0; j < lstCols.Count; j++)
                {
                    ICell cell = row.CreateCell(j);
                    if (!string.IsNullOrEmpty(lstCols[j]))
                    {
                        JObject temp = listdata[i];

                        if (temp[lstCols[j]] != null)
                        {
                            switch (temp[lstCols[j]].Type.ToString())
                            {
                                case "Float":
                                    cell.SetCellValue(temp.Value<float>(lstCols[j]));
                                    break;
                                case "String":
                                    cell.SetCellValue(temp.Value<string>(lstCols[j]));
                                    break;
                                case "Integer":
                                    cell.SetCellValue(temp.Value<int>(lstCols[j]));
                                    break;
                                default:
                                    cell.SetCellValue(temp.Value<dynamic>(lstCols[j]));
                                    break;
                            }
                        }
                    }
                    cell.CellStyle = cellstyle;
                }
            }

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static bool SaveModelDataToExcelNPOI(IList<dynamic> listdata, JObject jsonHeader, Stream stream, string format = "xls")
    {
        List<string> lstCols = new List<string>();
        try
        {
            if (null == jsonHeader) return false;

            IWorkbook workbook;
            if (format.Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                workbook = new XSSFWorkbook();
            }
            else
            {
                workbook = new HSSFWorkbook();
            }
            ISheet sheet = workbook.CreateSheet();
            //建立表头
            int iDataRow = CreatHeader(workbook, sheet, jsonHeader, lstCols);
            ICellStyle cellstyle = workbook.CreateCellStyle();
            cellstyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            cellstyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            cellstyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            cellstyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            sheet.CreateFreezePane(0, iDataRow);

            for (int i = 0; i < listdata.Count; i++)
            {
                IRow row = sheet.CreateRow(i + iDataRow);
                dynamic temp = listdata[i];
                //System.Reflection.PropertyInfo[] pInfo = temp.GetType().GetProperties();
                for (int j = 0; j < lstCols.Count; j++)
                {
                    ICell cell = row.CreateCell(j);

                    if (!string.IsNullOrEmpty(lstCols[j]))
                    {
                        //多级属性用点分隔
                        string[] fields = lstCols[j].Split('.');
                        dynamic subObject = temp;
                        PropertyInfo pi = null;
                        foreach (string field in fields)
                        {
                            if (pi != null)
                            {
                                subObject = pi.GetValue(subObject, null);
                                if (null == subObject)
                                {
                                    pi = null;
                                    break;
                                }
                            }
                            pi = subObject.GetType().GetProperty(field);
                            if (pi == null) break;
                        }

                        if (pi != null)
                        {
                            object value = pi.GetValue(subObject, null);
                            if (value != null)
                            {
                                if (pi.PropertyType.Equals(typeof(double)))
                                {
                                    cell.SetCellValue(double.Parse(value.ToString()));
                                }
                                else if (pi.PropertyType.Equals(typeof(float)))
                                {
                                    cell.SetCellValue(double.Parse(value.ToString()));
                                }
                                else if (pi.PropertyType.Equals(typeof(decimal)))
                                {
                                    cell.SetCellValue(double.Parse(value.ToString()));
                                }
                                else if (pi.PropertyType.Equals(typeof(double?)))
                                {
                                    cell.SetCellValue(double.Parse(value.ToString()));
                                }
                                else if (pi.PropertyType.Equals(typeof(float?)))
                                {
                                    cell.SetCellValue(double.Parse(value.ToString()));
                                }
                                else if (pi.PropertyType.Equals(typeof(decimal?)))
                                {
                                    cell.SetCellValue(double.Parse(value.ToString()));
                                }
                                else if (pi.PropertyType.Equals(typeof(int))
                                    || pi.PropertyType.Equals(typeof(int?)))
                                {
                                    cell.SetCellValue(int.Parse(value.ToString()));
                                }
                                else if (pi.PropertyType.Equals(typeof(string)))
                                {
                                    cell.SetCellValue((string)value);
                                }
                                else
                                {
                                    cell.SetCellValue(value.ToString());
                                }
                                /*
                                switch (pi.PropertyType.Name.ToString())
                                {
                                    case "Double":
                                        cell.SetCellValue((double) value);
                                        break;
                                    case "String":
                                        cell.SetCellValue((string) value);
                                        break;
                                    case "Integer":
                                        cell.SetCellValue((int) value);
                                        break;
                                    default:
                                        cell.SetCellValue(value.ToString());
                                        break;
                                }
                                */
                            }
                        }
                    }
                    cell.CellStyle = cellstyle;
                }
            }
            /*
            for (int i = 0; i < listdata.Count; i++)
            {
                sheet.AutoSizeColumn(i, true);
            }
            */

            workbook.Write(stream);
            /*
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
            }
            */
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static int MergedRegion(IWorkbook workbook, ISheet sheet, ICellStyle cellstyle, NPOI.SS.UserModel.IFont font, JObject cellCfg, int iCol, int iRow, IList<string> lstCols, ref int lockedCol)
    {
        int result = 1;
        IRow row = sheet.GetRow(iRow);
        if (null == row)
        {
            row = sheet.CreateRow(iRow);
        }

        ICell cell = row.GetCell(iCol);
        if (null == cell)
        {
            cell = row.CreateCell(iCol);
        }
        cell.SetCellValue(cellCfg.Value<string>("caption"));

        //ICellStyle cellstyle = workbook.CreateCellStyle();//设置垂直居中格式
        cellstyle.VerticalAlignment = VerticalAlignment.Center;//垂直居中
        cellstyle.Alignment = HorizontalAlignment.Center;//水平居中
        cellstyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
        cellstyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
        cellstyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
        cellstyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
        //设置字体加粗样式
        font.FontHeightInPoints = 11;
        font.Boldweight = short.MaxValue;
        //使用SetFont方法将字体样式添加到单元格样式中 
        cellstyle.SetFont(font);
        cell.CellStyle = cellstyle;
        if (null != cellCfg["cols"] && cellCfg.Value<JArray>("cols").Count > 0)
        {
            int iSubCol = iCol;
            int iSubRow = iRow + cellCfg.Value<int>("rowspan");
            for (int i = 0; i < cellCfg.Value<JArray>("cols").Count; i++)
            {
                int iRowSpan = MergedRegion(workbook, sheet, cellstyle, font, cellCfg.Value<JArray>("cols")[i] as JObject, iSubCol, iSubRow, lstCols, ref lockedCol);
                //找出子表头中占用的行数
                if (iRowSpan > result)
                {
                    result = iRowSpan;
                }
                iSubCol = iSubCol + cellCfg["cols"][i].Value<int>("colspan");
            }
            result = result + cellCfg.Value<int>("rowspan");
            //通过子标题累计colspan
            cellCfg["colspan"] = iSubCol - iCol;
        }
        else
        {
            lstCols.Add(cellCfg.Value<string>("filedName"));
            //返回锁定列的值
            if (cellCfg.Value<bool>("locked"))
            {
                lockedCol = lstCols.Count;
            }
            result = cellCfg.Value<int>("rowspan");
        }
        //存在合并单元格
        if (cellCfg.Value<int>("rowspan") > 1 || cellCfg.Value<int>("colspan") > 1)
        {
            CellRangeAddress region = new CellRangeAddress(iRow, iRow + cellCfg.Value<int>("rowspan") - 1,
                iCol, iCol + cellCfg.Value<int>("colspan") - 1);
            sheet.AddMergedRegion(region);
            ((HSSFSheet)sheet).SetEnclosedBorderOfRegion(region, NPOI.SS.UserModel.BorderStyle.Thin, NPOI.HSSF.Util.HSSFColor.Black.Index);
        }
        return result;
    }

    public static int CreatHeader(IWorkbook workbook, ISheet sheet, ICellStyle titleCellstyle, NPOI.SS.UserModel.IFont titleFont, ICellStyle headerCellstyle, NPOI.SS.UserModel.IFont headerFont, JObject cfgHeader, IList<string> lstCols, ref int lockedCol)
    {
        int result = 1;
        IRow row = sheet.CreateRow(0);
        ICell cell = row.CreateCell(0);
        cell.SetCellValue(cfgHeader.Value<string>("header"));

        titleCellstyle.VerticalAlignment = VerticalAlignment.Center;//垂直居中
        titleCellstyle.Alignment = HorizontalAlignment.Center;//水平居中
                                                              //设置字体加粗样式
        titleFont.FontHeightInPoints = 18;
        titleFont.Boldweight = short.MaxValue;
        //使用SetFont方法将字体样式添加到单元格样式中 
        titleCellstyle.SetFont(titleFont);
        cell.CellStyle = titleCellstyle;
        if (null != cfgHeader["cols"] && cfgHeader.Value<JArray>("cols").Count > 0)
        {
            int iCol = 0;
            int iRow = 1;
            for (int i = 0; i < cfgHeader.Value<JArray>("cols").Count; i++)
            {
                int iRowSpan = MergedRegion(workbook, sheet, headerCellstyle, headerFont, cfgHeader.Value<JArray>("cols")[i] as JObject, iCol, iRow, lstCols, ref lockedCol);
                if (iRowSpan > result)
                {
                    result = iRowSpan;
                }
                iCol = iCol + cfgHeader["cols"][i].Value<int>("colspan");
            }
            result = result + 1;
            //合并标题
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, iCol - 1));
        }
        return result;
    }

    public static bool SaveModelDataToExcelSheetNPOI(IWorkbook workbook, ISheet sheet, ICellStyle cellstyle, ICellStyle titleCellstyle, NPOI.SS.UserModel.IFont titleFont, ICellStyle headerCellstyle, NPOI.SS.UserModel.IFont headerFont, IList<dynamic> listdata, JObject jsonHeader)
    {
        List<string> lstCols = new List<string>();
        try
        {
            if (null == jsonHeader) return false;
            //建立表头
            int iLockedCol = 0;
            int iDataRow = CreatHeader(workbook, sheet, titleCellstyle, titleFont, headerCellstyle, headerFont, jsonHeader, lstCols, ref iLockedCol);
            cellstyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            cellstyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            cellstyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            cellstyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            sheet.CreateFreezePane(iLockedCol, iDataRow);

            for (int i = 0; i < listdata.Count; i++)
            {
                IRow row = sheet.CreateRow(i + iDataRow);
                dynamic temp = listdata[i];
                //System.Reflection.PropertyInfo[] pInfo = temp.GetType().GetProperties();
                for (int j = 0; j < lstCols.Count; j++)
                {
                    ICell cell = row.CreateCell(j);

                    if (!string.IsNullOrEmpty(lstCols[j]))
                    {
                        //PropertyInfo pi = temp.GetType().GetProperty(lstCols[j]);
                        //多级属性用点分隔
                        string[] fields = lstCols[j].Split('.');
                        dynamic subObject = temp;
                        PropertyInfo pi = null;
                        foreach (string field in fields)
                        {
                            if (pi != null)
                            {
                                subObject = pi.GetValue(subObject, null);
                                if (null == subObject)
                                {
                                    pi = null;
                                    break;
                                }
                            }
                            pi = subObject.GetType().GetProperty(field);
                            if (pi == null) break;
                        }
                        if (pi != null)
                        {
                            object value = pi.GetValue(subObject, null);
                            if (value != null)
                            {
                                if (pi.PropertyType.Equals(typeof(double)))
                                {
                                    cell.SetCellValue(double.Parse(value.ToString()));
                                }
                                else if (pi.PropertyType.Equals(typeof(float)))
                                {
                                    cell.SetCellValue(double.Parse(value.ToString()));
                                }
                                else if (pi.PropertyType.Equals(typeof(decimal)))
                                {
                                    cell.SetCellValue(double.Parse(value.ToString()));
                                }
                                else if (pi.PropertyType.Equals(typeof(double?)))
                                {
                                    cell.SetCellValue(double.Parse(value.ToString()));
                                }
                                else if (pi.PropertyType.Equals(typeof(float?)))
                                {
                                    cell.SetCellValue(double.Parse(value.ToString()));
                                }
                                else if (pi.PropertyType.Equals(typeof(decimal?)))
                                {
                                    cell.SetCellValue(double.Parse(value.ToString()));
                                }
                                else if (pi.PropertyType.Equals(typeof(int))
                                    || pi.PropertyType.Equals(typeof(int?)))
                                {
                                    cell.SetCellValue(int.Parse(value.ToString()));
                                }
                                else if (pi.PropertyType.Equals(typeof(string)))
                                {
                                    cell.SetCellValue((string)value);
                                }
                                else
                                {
                                    cell.SetCellValue(value.ToString());
                                }
                                /*
                                switch (pi.PropertyType.Name.ToString())
                                {
                                    case "Float":
                                        cell.SetCellValue((double)value);
                                        break;
                                    case "String":
                                        cell.SetCellValue((string)value);
                                        break;
                                    case "Integer":
                                        cell.SetCellValue((int)value);
                                        break;
                                    case "Byte":
                                        cell.SetCellValue((byte)value);
                                        break;
                                    default:
                                        cell.SetCellValue(value.ToString());
                                        break;
                                }
                                */
                            }
                        }
                    }
                    cell.CellStyle = cellstyle;
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public static JObject GetJsonHeaderFromString(string sHeader)
    {
        return JObject.Parse(sHeader);
    }

    public static int GetHeaderRows(JObject headerCfg)
    {
        int result = 0;
        GetColRows(headerCfg, ref result);
        return result;
    }

    public static void GetColRows(JObject cellCfg, ref int rows)
    {
        rows += cellCfg.Value<int>("rowspan");
        if (null != cellCfg["cols"] && cellCfg.Value<JArray>("cols").Count > 0)
        {
            GetColRows(cellCfg.Value<JArray>("cols")[0] as JObject, ref rows);
        }
    }
    public static int GetHeaderInfo(JObject headerCfg, IList<string> lstCols)
    {
        return GetCellInfo(headerCfg, 0, 0, lstCols);
    }
    public static int GetCellInfo(JObject cellCfg, int iCol, int iRow, IList<string> lstCols)
    {
        int result = 1;
        if (null != cellCfg["cols"] && cellCfg.Value<JArray>("cols").Count > 0)
        {
            int iSubCol = iCol;
            int iSubRow = iRow + cellCfg.Value<int>("rowspan");
            for (int i = 0; i < cellCfg.Value<JArray>("cols").Count; i++)
            {
                int iRowSpan = GetCellInfo(cellCfg.Value<JArray>("cols")[i] as JObject, iSubCol, iSubRow, lstCols);
                //找出子表头中占用的行数
                if (iRowSpan > result)
                {
                    result = iRowSpan;
                }
                iSubCol = iSubCol + cellCfg["cols"][i].Value<int>("colspan");
            }
            result = result + cellCfg.Value<int>("rowspan");
            //通过子标题累计colspan
            cellCfg["colspan"] = iSubCol - iCol;
        }
        else
        {
            lstCols.Add(cellCfg.Value<string>("filedName"));
            result = cellCfg.Value<int>("rowspan");
        }
        return result;
    }
    public static void MergerCopyRegion(ISheet fromSheet, ISheet toSheet)
    {
        int sheetMergerCount = fromSheet.NumMergedRegions;
        for (int i = 0; i < sheetMergerCount; i++)
        {
            toSheet.AddMergedRegion(fromSheet.GetMergedRegion(i));
        }
    }
    public static void CopySheet(ISheet fromSheet, ISheet toSheet)
    {
        MergerCopyRegion(fromSheet, toSheet);
        IRow fromRow, toRow;
        CellType fromCellType;
        int Column_Num = -1;
        for (int i = 0; i <= fromSheet.LastRowNum; i++)
        {
            if (fromSheet.GetRow(i) != null)
            {
                fromRow = fromSheet.GetRow(i);
                toRow = toSheet.CreateRow(i);
                toRow.Height = fromRow.Height;
                for (int j = 0; j <= fromRow.LastCellNum; j++)
                {

                    if (fromRow.GetCell(j) != null)
                    {
                        #region 内容
                        fromCellType = fromRow.GetCell(j).CellType;
                        toRow.CreateCell(j).SetCellType(fromRow.GetCell(j).CellType);
                        if (fromCellType == CellType.Numeric)
                        { toRow.GetCell(j).SetCellValue(fromRow.GetCell(j).NumericCellValue); }
                        switch (fromCellType)
                        {
                            case CellType.Boolean: toRow.GetCell(j).SetCellValue(fromRow.GetCell(j).BooleanCellValue); break;
                            case CellType.Numeric:
                                if (HSSFDateUtil.IsCellDateFormatted(fromRow.GetCell(j)))
                                { toRow.GetCell(j).SetCellValue((DateTime)fromRow.GetCell(j).DateCellValue); }
                                else { toRow.GetCell(j).SetCellValue(fromRow.GetCell(j).NumericCellValue); }
                                break;
                            case CellType.String: toRow.GetCell(j).SetCellValue(fromRow.GetCell(j).RichStringCellValue); break;
                            case CellType.Formula: toRow.GetCell(j).SetCellValue(fromRow.GetCell(j).CellFormula); break;
                            case CellType.Error: toRow.GetCell(j).SetCellValue(fromRow.GetCell(j).ErrorCellValue); break;

                        }
                        #endregion

                        #region 复制单元格格式
                        ICellStyle fromCellStyle = fromRow.GetCell(j).CellStyle;
                        if (fromCellStyle != null)
                        {
                            toRow.GetCell(j).CellStyle = fromCellStyle;
                        }
                        #endregion
                    }
                }
                if (fromRow.LastCellNum > Column_Num) { Column_Num = fromRow.LastCellNum; }
            }
        }
        for (int i = 0; i < Column_Num; i++)
        {
            toSheet.SetColumnWidth(i, fromSheet.GetColumnWidth(i));//列宽
        }
    }
}
