using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class Method
{
    /// <summary>
    /// 取得指定的日期是星期幾
    /// </summary>
    /// <param name="date">指定的日期</param>
    /// <returns></returns>
    public static int Week(string date)
    {
        int result;

        date = System.DateTime.Parse(date).DayOfWeek.ToString();
        switch (date)
        {
            case "Sunday":
                result = 0;
                break;
            case "Monday":
                result = 1;
                break;
            case "Tuesday":
                result = 2;
                break;
            case "Wednesday":
                result = 3;
                break;
            case "Thursday":
                result = 4;
                break;
            case "Friday":
                result = 5;
                break;
            case "Saturday":
                result = 6;
                break;
            default:
                result = 0;
                break;
        }
        return result;
    }
}
