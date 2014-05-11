using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using Zolilo.Data;
using BCrypt.Net;

namespace Zolilo.Security
{
    internal static class SecurityEncryption
    {
        private static string ComputeSalt(DR_Accounts account)
        {
            return BCrypt.Net.BCrypt.GenerateSalt();
            //return ((char)0x33 + (char)0x30 + (((long)account.ID * 604).ToString()) + (char)0x34 + "6o4");
        }


        internal static string ComputeHash(string code)
        {
            return BCrypt.Net.BCrypt.HashPassword(code);
        }

        internal static bool VerifyCode(string codeInput, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(codeInput, storedHash);
        }
    }   
}