using System.Security.Cryptography;
using System.Text;

namespace GameOnline.Core.Security
{
    public static class HashPasswordMd5
    {
        public static string EncodePasswordMd5(this string password)
        {
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(password);
            encodedBytes = md5.ComputeHash(originalBytes);
            return BitConverter.ToString(encodedBytes);
        }
    }
}
