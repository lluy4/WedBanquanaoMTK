using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace DOAN.Common
{
    public static class Encryptor
    {
        public static string MD5Hash(string text)
        {
            // Không mã hóa, trả về mật khẩu nguyên gốc
            return text;
        }
    }
}