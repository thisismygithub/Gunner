using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using CMoney.WebBackend.WebFunction.VariavlesClass;
using System.Globalization;

public static class StringMethod
{
    /// <summary>
    /// Left
    /// </summary>
    /// <param name="strValue">來源字串</param>
    /// <param name="length">要取得的字串大小</param>
    /// <returns></returns>
    public static string Left(string strValue, int length)
    {
        if (string.IsNullOrEmpty(strValue)) return "";
        var result = strValue.Length < length ? strValue : strValue.Substring(0, length);
        return result;
    }

    /// <summary>
    /// LeftWithBytes
    /// </summary>
    /// <param name="strValue"></param>
    /// <param name="length">count of bytes</param>
    /// <returns></returns>
    public static string LeftWithBytes(string strValue, int length)
    {
        length *= 2;
        var i = 0;
        var sb = new StringBuilder();
        foreach (var charValue in strValue)
        {
            i += Encoding.Default.GetBytes(charValue.ToString()).Length;
            if (i <= length)
            {
                sb.Append(charValue);
            }
            else
            {
                break;
            }
        }
        return sb.ToString();
    }

    /// <summary>
    /// Right
    /// </summary>
    /// <param name="strValue">來源字串</param>
    /// <param name="length">要取得的字串大小</param>
    /// <returns></returns>
    public static string Right(string strValue, int length)
    {
        if (string.IsNullOrEmpty(strValue)) return "";
        var result = strValue.Length < length ? strValue : strValue.Substring(strValue.Length - length, length);
        return result;
    }

    /// <summary>
    /// 取得字元的大小
    /// </summary>
    /// <param name="str">字元</param>
    /// <returns></returns>
    static public int CharLength(string str)
    {
        byte[] strBytes;
        var count = 0;
        if (str == null)
        {
            return count;
        }
        for (int i = 0; i < str.Length; i++)
        {
            strBytes = System.Text.Encoding.UTF8.GetBytes(str.Substring(i, 1));

            if (strBytes.Length == 3)
                count += 2;
            else
                count += 1;
        }
        return count;
    }

    /// <summary>
    /// 截取以byte為寬度的指定字串內容
    /// </summary>
    /// <param name="str">要判斷的字串</param>
    /// <param name="length">字串大小</param>
    /// <returns></returns>
    static public string SubChtString(string str, int length)
    {
        byte[] strBytes;
        string result = "", subString;
        var count = 0;

        for (int i = 0; i < str.Length; i++)
        {
            subString = str.Substring(i, 1);
            strBytes = System.Text.Encoding.UTF8.GetBytes(subString);

            if (strBytes.Length == 3)
                count += 2;
            else
                count += 1;

            if (count > length)
                break;
            else
                result += subString;
        }
        return result;
    }

    /// <summary>
    /// 取得中文字元字數
    /// </summary>
    /// <param name="str">要判斷的字串</param>
    /// <returns></returns>
    static public int ChtCharLength(string str)
    {
        byte[] strBytes;
        var count = 0;

        for (int i = 0; i < str.Length; i++)
        {
            strBytes = System.Text.Encoding.UTF8.GetBytes(str.Substring(i, 1));

            if (strBytes.Length == 3)
                count += 1;

        }
        return count;
    }

    /// <summary>
    /// 過濾\r\n字串
    /// </summary>
    /// <param name="code">要過濾的字串</param>
    /// <returns></returns>
    public static string FilterEscapeChars(string code)
    {
        code = Regex.Replace(code, @"[\r\n]+", "");
        return code;
    }
}

/// <summary>
/// 
/// </summary>
public static class Extension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="strValue"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string Left(this string strValue, int length)
    {
        if (string.IsNullOrEmpty(strValue)) return "";
        var result = strValue.Length < length ? strValue : strValue.Substring(0, length);
        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strValue"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string Right(this string strValue, int length)
    {
        if (string.IsNullOrEmpty(strValue)) return "";
        var result = strValue.Length < length ? strValue : strValue.Substring(strValue.Length - length, length);
        return result;
    }

    #region 1 擴充方法

    #region 文字類

    #region 轉成取得JSON格式文字
    /// <summary>
    /// 靜態屬性JavaScriptSerializer
    /// </summary>
    private static JavaScriptSerializer JavaScriptSerializer { get; set; }

    /// <summary>
    /// 轉成取得JSON格式文字
    /// </summary>
    /// <param Name="Object">物件</param>
    /// <param name="Object"> </param>
    /// <returns></returns>
    public static string ToJsonString(this object Object)
    {
        if (JavaScriptSerializer == null)
        {
            JavaScriptSerializer = new JavaScriptSerializer();
        }
        return JavaScriptSerializer.Serialize(Object);
    }
    #endregion

    #region 轉成取得遮罩文字

    /// <summary>
    /// 轉成取得遮罩文字
    /// </summary>
    /// <param Name="Value">輸入值</param>
    /// <param Name="Count">顯示字數</param>
    /// <param Name="MaskText">遮罩符號</param>
    /// <param name="value"> </param>
    /// <param name="count"> </param>
    /// <param name="maskText"> </param>
    /// <returns></returns>
    public static string ToMaskString(this string value, int count = 4, string maskText = "*")
    {
        var result = "";
        if (value.Length > 0)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if (i < count)
                {
                    result += value[i];
                }
                else
                {
                    result += maskText;
                }
            }
        }
        return result;
    }
    #endregion

    #region 轉成取得最大數字

    /// <summary>
    /// 轉成取得最大數字
    /// </summary>
    /// <param Name="Value">原始值</param>
    /// <param Name="Max">最大值</param>
    /// <param name="value"> </param>
    /// <param name="max"> </param>
    /// <param name="maxIntShowType"> </param>
    /// <returns>超過最大值用+號表示</returns>
    public static string ToMaxCount(this int value, int max, MaxIntShowType maxIntShowType = MaxIntShowType.MaxIntWithPlus)
    {
        var output = value.ToString(CultureInfo.InvariantCulture);
        if (value >= max)
        {
            switch (maxIntShowType)
            {
                case MaxIntShowType.MaxIntWithPlus:
                    output = (max).ToString(CultureInfo.InvariantCulture) + "+";
                    break;
                case MaxIntShowType.N:
                    output = "N";
                    break;
                default:
                    output = (max).ToString(CultureInfo.InvariantCulture) + "+";
                    break;
            }
        }
        if (value <= 0)
        {
            output = null;
        }
        return output;
    }
    #endregion

    #region 轉成取得最多文字

    /// <summary>
    /// 轉成取得最多文字
    /// </summary>
    /// <param name="value">原始文字 </param>
    /// <param name="max">最大字數 </param>
    /// <returns></returns>
    public static string ToMaxText(this string value, int max)
    {
        max *= 2;
        var i = 0;
        var sb = new StringBuilder();
        foreach (var n in value)
        {
            i += Encoding.Default.GetBytes(n.ToString(CultureInfo.InvariantCulture)).Length;
            if (i <= max)
            {
                sb.Append(n);
            }
            else
            {
                break;
            }
        }
        return sb.ToString();
    }
    #endregion

    #region 轉成取得格式化數字

    /// <summary>
    /// 轉成取得格式化數字
    /// </summary>
    /// <param name="value">原始值 </param>
    /// <returns>有逗號的格式化數字</returns>
    public static string ToNumFormat(this int value)
    {
        return string.Format("{0:N0}", value);
    }
    #endregion

    #region 轉成取得小數點幾位文字
    /// <summary>
    /// 轉成取得小數點幾位文字
    /// </summary>
    /// <param name="value">輸入值</param>
    /// <param name="n">小數點後幾位數</param>
    /// <returns></returns>
    public static string ToDecimalString(this double value, int n = 2)
    {
        var format = "0.";
        if (n > 0)
        {
            for (var i = 0; i < n; i++)
            {
                format += "0";
            }
        }
        return value.ToString(format);
    }
    #endregion

    #region 轉成取得兩個文字間的文字
    /// <summary>
    /// 轉成取得兩個文字間的文字
    /// </summary>
    /// <param name="value">輸入值</param>
    /// <param name="start">起始文字</param>
    /// <param name="end">結束文字</param>
    public static string ToBetweenString(this string value, string start, string end)
    {
        if (value.LastIndexOf(end) > -1 && value.IndexOf(start) > -1)
        {
            value = value.Remove(value.LastIndexOf(end)).Remove(0, value.IndexOf(start) + 1);
        }
        else
        {
            value = "";
        }
        return value;
    }
    #endregion

    #region 轉成取得文字或null值
    /// <summary>
    /// 轉成取得文字或null值
    /// </summary>
    /// <param name="Value">輸入值</param>
    /// <returns></returns>
    public static string ToStringOrNull(this string Value)
    {
        if (Value == "" || Value == null)
        {
            return null;
        }
        else
        {
            return Value;
        }
    }
    #endregion

    #endregion

    #region 數字類

    #region 轉成int數字

    /// <summary>
    /// 轉成int數字
    /// </summary>

    /// <param Name="Default">預設值</param>
    /// <param name="value"> </param>
    /// <param name="Default"> </param>
    /// <returns></returns>
    public static int ToInt(this string value, int Default = 0)
    {
        int number = !int.TryParse(value, out number) ? Default : number;
        return number;
    }
    #endregion

    #region 轉成uint數字

    /// <summary>
    /// 轉成uint數字
    /// </summary>
    /// <param name="value"> 輸入值</param>
    /// <param name="Default">預設值 </param>
    /// <returns></returns>
    public static uint ToUint(this string value, uint Default = 0)
    {
        uint number = !uint.TryParse(value, out number) ? Default : number;
        return number;
    }
    #endregion

    #region 轉成long數字
    /// <summary>
    /// 轉成long數字
    /// </summary>
    /// <param Name="Value">輸入值</param>
    /// <param Name="Default">預設值</param>
    /// <returns></returns>
    public static long ToLong(this string Value, long Default = 0)
    {
        long Number = !long.TryParse(Value, out Number) ? Default : Number;
        return Number;
    }
    #endregion

    #endregion

    #region 真偽類

    #region 轉成bool布林值
    /// <summary>
    /// 轉成bool布林值
    /// </summary>
    /// <param name="val">輸入值</param>
    /// <param name="defaultValue">預設值</param>
    /// <returns></returns>
    public static bool ToBool(this string val, bool defaultValue = false)
    {
        if (string.IsNullOrEmpty(val))
        {
            return false;
        }
        val = val.ToLower();
        if (val == "false" || val == "0" || val == "no" || val == "n")
        {
            return false;
        }
        if (val == "true" || val == "1" || val == "yes" || val == "y")
        {
            return true;
        }
        return defaultValue;
    }
    #endregion

    #region 轉成bool布林值或null值
    /// <summary>
    /// 轉成bool布林值或null值
    /// </summary>
    /// <param name="value">輸入值</param>
    /// <returns></returns>
    public static bool? ToBoolOrNull(this string value)
    {
        bool? Default = null;
        value = value.ToLower();
        if (value == "true" || value == "1" || value == "yes" || value == "y")
        {
            Default = true;
        }
        if (value == "false" || value == "0" || value == "no" || value == "n")
        {
            Default = false;
        }
        return Default;
    }
    #endregion

    #endregion

    #endregion

}
