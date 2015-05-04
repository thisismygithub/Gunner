using System;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for Validator
/// </summary>
public class Validator
{
    public delegate bool ValidationDelegate(string value);

    // Public regular expressions.
    public static Regex s_regexCellphoneNumber = new Regex(@"^09[0-9]{8}$");
    public static Regex s_regexPhoneNumber = new Regex(@"^[0-9#()\-+]{6,30}$");
    public static Regex s_regexKeyGenCode = new Regex(@"^[a-zA-Z0-9]{3}-[a-zA-Z0-9]{3}-[a-zA-Z0-9]{3}$");
    public static Regex s_regexChinese = new Regex(@"^[\u4E00-\u9fa5]+$");
    //public static Regex s_regexEmailAddress = new Regex(@"^([a-zA-Z0-9]+([\.+_-][a-zA-Z0-9]+)*)@(([a-zA-Z0-9]+((\.|[-]{1,2})[a-zA-Z0-9]+)*)\.[a-zA-Z]{2,6})$");
    public static Regex s_regexEmailAddress = new Regex(@"\A(?:[a-zA-Z0-9_.-]+(?:\.[a-z0-9_.-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?)\Z");
    public static Regex s_regexUrl = new Regex(@"(http|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?");
    public static Regex s_regexGenericUrl = new Regex(@"((http|https):\/\/)?[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?");
    public static Regex s_regexBoolean_String = new Regex(@"^(true|false)$", RegexOptions.IgnoreCase);
    public static Regex s_regexBoolean_Int_String = new Regex(@"^(1|0)$");
    public static Regex s_regexAlphanumeric = new Regex(@"^[a-zA-Z0-9]+$");
    public static Regex s_regexGender_Int_String = new Regex(@"^(1|0)$");
    private static Regex s_regexDangerousXssValues = new Regex(@"(<                 |   
                                                                %3c                 |   # A different encoding of <
                                                                &lt                 |   # A different encoding of <
                                                                \\x3c               |   # A different encoding of <                                                                
                                                                \\u003c             |   # A different encoding of <       
                                                                javascript:         |
                                                                java\\0script:      |
                                                                \\ja\\vasc\\ript:   |
                                                                &\#)",
                                                                RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

    // Private regular expressions.
    private static Regex s_regexDangerousPathValues = new Regex(@"^.*\.\.(\\|/).*$", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

    public Validator()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary>
    /// Validate value by length.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="minLength"></param>
    /// <param name="maxLength"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static string GetValidatedValue(string value, int minLength, int maxLength, string defaultValue)
    {
        string returnValue = defaultValue;

        if (value != null)
        {
            returnValue = (value.Length >= minLength && value.Length <= maxLength && !s_regexDangerousXssValues.IsMatch(value)) ? value : defaultValue;
        }

        return returnValue;
    }

    /// <summary>
    /// Validate value by regular expression.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="regex"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static string GetValidatedValue(string value, Regex regex, string defaultValue)
    {
        string returnValue = defaultValue;

        if (value != null)
        {
            returnValue = (regex.IsMatch(value) && !value.EndsWith("\n")) ? value : defaultValue; // RegEx ignores the ending new line.
        }

        return returnValue;
    }

    /// <summary>
    /// Validate value by whilte list.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="allowedValues"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static string GetValidatedValue(string value, string[] allowedValues, string defaultValue)
    {
        string returnValue = defaultValue;

        if (value != null)
        {
            returnValue = (Array.IndexOf(allowedValues, value) > -1) ? value : defaultValue;
        }

        return returnValue;
    }

    /// <summary>
    /// Validate value by delegate.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="validationDelegate"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static string GetValidatedValue(string value, ValidationDelegate validationDelegate, string defaultValue)
    {
        string returnValue = defaultValue;

        if (value != null)
        {
            returnValue = (validationDelegate(value)) ? value : defaultValue;
        }

        return returnValue;
    }

    /// <summary>
    /// Check if a URL is a CMoney URL.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsCmoneyUrl(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return false;
        }

        if (str.StartsWith("/") || str.StartsWith("./") || str.StartsWith("../"))
        {
            return true;
        }
        else
        {
            try
            {
                Uri url = new Uri(str);
                string host = url.Host;

                if (!host.ToLower().Equals("cmoney.com.tw") &&
                    !host.ToLower().Equals("cmoney.tw") &&
                    !host.ToLower().EndsWith(".cmoney.com.tw") &&
                    !host.ToLower().EndsWith(".cmoney.tw") &&
                    !host.ToLower().StartsWith("192.168.") &&
                    !host.Equals("127.0.0.1") &&
                    !host.ToLower().Equals("localhost") &&
                    !host.Equals("::1"))
                {
                    return false;
                }
                else
                {
                    if (s_regexDangerousXssValues.IsMatch(str))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Check if this is an email address.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsEmailAddress(string str)
    {
        if (str == null)
        {
            return false;
        }

        if (!s_regexEmailAddress.IsMatch(str))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Check if the string is a 32-bit integer.
    /// </summary>
    /// <param name="str">String</param>
    /// <returns></returns>
    public static bool IsInt32(string str)
    {
        int result;

        System.Globalization.NumberStyles NumberStyle = System.Globalization.NumberStyles.Integer;
        return Int32.TryParse(str, NumberStyle, System.Globalization.CultureInfo.CurrentCulture, out result);
    }

    /// <summary>
    /// Check if the string is a 64-bit integer.
    /// </summary>
    /// <param name="str">String</param>
    /// <returns></returns>
    public static bool IsInt64(string str)
    {
        long result;

        System.Globalization.NumberStyles NumberStyle = System.Globalization.NumberStyles.Integer;
        return Int64.TryParse(str, NumberStyle, System.Globalization.CultureInfo.CurrentCulture, out result);
    }

    /// <summary>
    /// Check if the string is numeric.
    /// </summary>
    /// <param name="str">String</param>
    /// <returns></returns>
    public static bool IsNumeric(string str)
    {
        bool returnValue = false;
        double result;

        System.Globalization.NumberStyles NumberStyle = System.Globalization.NumberStyles.Number;
        returnValue = Double.TryParse(str, NumberStyle, System.Globalization.CultureInfo.CurrentCulture, out result);

        return returnValue;
    }

    /// <summary>
    /// Check if the string represents a DateTime object.
    /// </summary>
    /// <param name="str">String</param>
    /// <returns></returns>
    public static bool IsDateTime(string str)
    {
        DateTime result;
        return DateTime.TryParse(str, out result);
    }

    /// <summary>
    /// Check if the string is a long.
    /// </summary>
    /// <param name="str">String</param>
    /// <returns></returns>
    public static bool IsLong(string str)
    {
        long result;

        System.Globalization.NumberStyles NumberStyle = System.Globalization.NumberStyles.Integer;
        return long.TryParse(str, NumberStyle, System.Globalization.CultureInfo.CurrentCulture, out result);
    }

    /// <summary>
    /// Check if the string matches a pattern.
    /// </summary>
    /// <param name="str">String</param>
    public static bool IsMatch(string str, string pattern)
    {
        Regex regex = new Regex(pattern);
        return regex.IsMatch(str);
    }

    /// <summary>
    /// Check if the string matches a regex.
    /// </summary>
    /// <param name="str">String</param>
    public static bool IsMatch(string str, Regex regex)
    {
        return regex.IsMatch(str);
    }

    /// <summary>
    /// Check if the string is a GUID.
    /// </summary>
    /// <param name="str">string</param>
    public static bool IsGuid(string str)
    {
        try
        {
            Guid guid = new Guid(str);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsUniformNumber(string str)
    {
        int uniformNumber;
        if (str == null || str.Trim().Length != 8)
        {
            return false;
        }

        if (!int.TryParse(str, out uniformNumber))
        {
            return false;
        }

        int[] logicIntArray = new int[] { 1, 2, 1, 2, 1, 2, 4, 1 };
        int addition, sum = 0, j = 0;
        for (int i = 0; i < logicIntArray.Length; i++)
        {
            int no = Convert.ToInt32(str.Substring(i, 1));
            j = no * logicIntArray[i];
            addition = ((j / 10) + (j % 10));
            sum += addition;
        }

        if (sum % 10 == 0)
        {
            return true;
        }

        if (str.Substring(6, 1) == "7")
        {
            if (sum % 10 == 9)
            {
                return true;
            }
        }

        return false;
    }
}
