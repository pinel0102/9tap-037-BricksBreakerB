using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Text.RegularExpressions;

public static partial class GlobalDefine
{
    public static bool isLevelEditor { get; private set; }

    public const string formatVersion = "v{0}";
    public const string formatVersionWithAppName = "{1} v{0}";
    public const string formatTime_mm_ss = "{0:00}:{1:00}";

    public static void InitRandomSeed()
    {
        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
    }

    public static void ThisIsLevelEditor(bool _isLevelEditor)
    {
        isLevelEditor = _isLevelEditor;
    }

    public static string SecondsToTimeText(int _seconds)
    {
        _seconds = Mathf.Max(0, _seconds);
        return string.Format(formatTime_mm_ss, _seconds / 60, _seconds % 60);
    }

    public static float GetAngle(Vector2 from, Vector2 to)
    {
        Vector2 offset = to - from;
        return Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
    }

    public static string GetVersionText()
    {
        return string.Format(formatVersion, Application.version);
    }

    public static string GetVersionTextWithAppName()
    {
        return string.Format(formatVersionWithAppName, Application.version, Application.productName);
    }

    public static float Root(float num)
    {
        return Mathf.Pow(num, 0.5f);
    }


#region CSV

    private static bool showCSVLog = false;

    public static List<int> CSVToList(string csv)
    {
        List<int> list = new List<int>();

        if (string.IsNullOrEmpty(csv))
            return list;
        
        var values = Regex.Split(csv, SPLIT_RE);
        
        for(var i=0; i < values.Length; i++ ) 
        {
            string value = values[i];
            value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\n", "\n");
            value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
            object finalvalue = value;
            
            if(int.TryParse(value, out int n)) {
                finalvalue = n;
                list.Add((int)finalvalue);
            }

            if (showCSVLog)
                Debug.Log(CodeManager.GetMethodName() + string.Format("list[{0}] = {1}", i, finalvalue));
        }        

        return list;
    }

    public static string ListToCSV(List<int> list)
    {
        List<string> strList = new List<string>();

        for(int i=0; i < list.Count; i++)
        {
            strList.Add(list[i].ToString());
        }

        return ListToCSV(strList);
    }

    public static string ListToCSV(List<float> list)
    {
        List<string> strList = new List<string>();

        for(int i=0; i < list.Count; i++)
        {
            strList.Add(list[i].ToString());
        }

        return ListToCSV(strList);
    }

    public static string ListToCSV(List<bool> list)
    {
        List<string> strList = new List<string>();

        for(int i=0; i < list.Count; i++)
        {
            strList.Add(list[i] ? true.ToString() : false.ToString());
        }

        return ListToCSV(strList);
    }

    public static string ListToCSV(List<string> list)
    {
        if (list == null)
            throw new ArgumentNullException("columns");
        
        if (OneQuote == null || OneQuote[0] != Quote)
        {
            OneQuote = String.Format("{0}", Quote);
            TwoQuotes = String.Format("{0}{0}", Quote);
            QuotedFormat = String.Format("{0}{{0}}{0}", Quote);
        }

        StringBuilder sb = new StringBuilder();
        
        for (int i = 0; i < list.Count; i++)
        {
            if (i > 0)
                sb.Append(Delimiter);
            
            if (list[i].IndexOfAny(SpecialChars) == -1)
                sb.Append(list[i]);
            else
                sb.Append(string.Format(QuotedFormat, list[i].Replace(OneQuote, TwoQuotes)));
        }

        if (showCSVLog)
            Debug.Log(CodeManager.GetMethodName() + sb.ToString());

        return sb.ToString();
    }

    private const string SPLIT_RE = ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))";
    private static char[] TRIM_CHARS = { '\"' };
    private static char[] SpecialChars = new char[] { ',', '"', '\r', '\n' };
    private const int DelimiterIndex = 0;
    private const int QuoteIndex = 1;
    private static string OneQuote = null;
    private static string TwoQuotes = null;
    private static string QuotedFormat = null;    
    private static char Delimiter
    {
        get { return SpecialChars[DelimiterIndex]; }
        set { SpecialChars[DelimiterIndex] = value; }
    }    
    private static char Quote
    {
        get { return SpecialChars[QuoteIndex]; }
        set { SpecialChars[QuoteIndex] = value; }
    }

#endregion CSV

}
