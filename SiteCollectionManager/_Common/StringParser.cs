using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SiteCollectionManager._Common
{
    public class StringParser
    {
        public static byte[] ConvertHexStringToBytes(string hexStr)
        {
            if (hexStr.StartsWith("0x") && ((hexStr.Length % 2) == 0))
            {
                byte[] buff = new byte[(hexStr.Length - 2) / 2];
                for (int i = 2; i < hexStr.Length; i += 2)
                {
                    buff[(i / 2) - 1] = Convert.ToByte(hexStr.Substring(i, 2), 0x10);
                }
                return buff;
            }
            return null;
        }
    }
}
