using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static partial class GlobalDefine
{
#region Handle Time

    public static bool IsWeekend()
    {
        DateTime nowDt = DateTime.Now;
        return (nowDt.DayOfWeek == DayOfWeek.Saturday || nowDt.DayOfWeek == DayOfWeek.Sunday);
    }

    public static bool IsWeeklyStoreResetDay()
    {
        DateTime nowDt = DateTime.Now;
        return nowDt.DayOfWeek == DayOfWeek.Monday;
    }

    public static bool IsRefreshTime(string date)
    {
        if (DateTime.TryParse(date, out DateTime o))
            return IsRefreshTime(o);
        else
            return true;
    }

    public static bool IsRefreshTime(DateTime date)
    {
        return DateTime.Now.Subtract(date).TotalSeconds >= 0;
    }

    private const string remainTimeFormat = "{0:D2}:{1:D2}:{2:D2}";
    public static string GetTimeString(int seconds)
    {
        return string.Format(remainTimeFormat, seconds / 3600, (seconds / 60) % 60, seconds % 60);
    }

    public static int GetSeconds_UntilTommorow()
    {
        TimeSpan sp = DateTime.Today.AddDays(1) - DateTime.Now;
        return (int)sp.TotalSeconds;
    }

    public static TimeSpan GetTime_UntilTommorow()
    {
        TimeSpan sp = DateTime.Today.AddDays(1) - DateTime.Now;
        return sp;
    }

    public static int GetSeconds_UntilMonday()
    {
        for(int i=1; i < 7; i++)
        {
            if (DateTime.Today.AddDays(i).DayOfWeek == DayOfWeek.Monday)
            {
                TimeSpan sp = DateTime.Today.AddDays(i) - DateTime.Now;
                return (int)sp.TotalSeconds;
            }
        }
        
        return 0;
    }

    public static TimeSpan GetTime_UntilMonday()
    {
        for(int i=1; i < 7; i++)
        {
            if (DateTime.Today.AddDays(i).DayOfWeek == DayOfWeek.Monday)
            {
                TimeSpan sp = DateTime.Today.AddDays(i) - DateTime.Now;
                return sp;
            }
        }
        
        return new TimeSpan(0, 0, 0);
    }

    public static DateTime GetMondayOfThisWeek()
    {
        for(int i=0; i < 7; i++)
        {
            if (DateTime.Today.AddDays(-i).DayOfWeek == DayOfWeek.Monday)
            {
                return DateTime.Today.AddDays(-i);
            }
        }

        return DateTime.Today;
    }

#endregion Handle Time

    public static List<string> SortList(List<string> origin, int startIndex)
    {
        List<string> newList = new List<string>();

        for(int i=0; i < origin.Count; i++)
        {
            newList.Add(origin[(startIndex + i) % origin.Count]);
        }

        return newList;
    }
}