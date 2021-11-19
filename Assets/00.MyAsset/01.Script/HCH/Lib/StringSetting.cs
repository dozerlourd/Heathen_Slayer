using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HCH
{
    public class StringSetting
    {
        public static string SetString(string str, int lineLength = 18)
        {
            string returnStr = "";

            for (int i = 0; i < str.Length / lineLength; i++)
            {
                returnStr += str.Substring(i + (i * (lineLength - 1)), Mathf.Min(lineLength, str.Length)) + "\r\n";
            }

            return returnStr;
        }
    }
}