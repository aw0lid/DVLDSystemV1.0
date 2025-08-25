using System;
using System.Security.Cryptography;
using System.Text;


namespace DVLDBackend
{
    public static class clsCryptographyLibrary
    {

        public static string ComputeSHA256Hash(string rawData)
        {
            using (var sha256 = SHA256.Create())
            {
                return BitConverter.ToString(
                    sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData))
                ).Replace("-", "").ToLower();
            }
        }
    }
}

