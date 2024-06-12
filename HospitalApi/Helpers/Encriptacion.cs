using System.Security.Cryptography;
using System.Text;

namespace HospitalApi.Helpers
{
    public class Encriptacion
    {
        public static string StringToSHA512(string s)
        {
            var arreglo = Encoding.UTF8.GetBytes(s);
            var hash = SHA512.HashData(arreglo);
            return Convert.ToHexString(hash).ToLower();
        }
    }
}
