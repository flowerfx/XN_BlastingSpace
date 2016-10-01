using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
namespace BlastGamePort
{
    class DeCryptData 
    {
        static string Hash = "P@@Sw0rd";
        public static string EncryptString(string Str)
        {
            string reValue = "";
            char[] t = Str.ToCharArray();
            char[] tHash = Hash.ToCharArray();
            int stepH = 0;
            for (int i = 0; i < t.Count(); i++)
            {
                int Num = Convert.ToInt32(t[i]) - Convert.ToInt32(tHash[stepH]);
                string temp = Convert.ToChar(Num).ToString();
                stepH++;
                if (stepH >= Hash.Length)
                    stepH = 0;
                reValue += temp;
            }
            return reValue;
        }


        public static string DecryptString(string Str)
        {
            string reValue = "";
            char[] t = Str.ToCharArray();
            char[] tHash = Hash.ToCharArray();
            int stepH = 0;
            for (int i = 0; i < t.Count(); i++)
            {
                int Num = Convert.ToInt32(t[i]) + Convert.ToInt32(tHash[stepH]);
                string temp = Convert.ToChar(Num).ToString();
                stepH++;
                if(stepH >=Hash.Length)
                    stepH = 0;
                reValue += temp;
            }
            return reValue;
        }


        public static byte[] StringToAscii(string s)
        {
            byte[] retval = new byte[s.Length];
            for (int ix = 0; ix < s.Length; ++ix)
            {
                char ch = s[ix];
                if (ch <= 0x7f) retval[ix] = (byte)ch;
                else retval[ix] = (byte)'?';
            }
            return retval;
        }

    }   
}
