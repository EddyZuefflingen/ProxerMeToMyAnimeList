using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ProxerMeToMyAnimeList.Classes
{
    internal class PKCE
    {
        public string CodeVerifier = "";

        public string CodeChallenge = "";

        public PKCE()
        {
            CodeVerifier = GenerateNonce();
            CodeChallenge = GenerateCodeChallenge(CodeVerifier);
        }

        string GenerateNonce()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var nonce = new char[128];
            for (int i = 0; i < nonce.Length; i++)
                nonce[i] = chars[random.Next(chars.Length)];

            return new string(nonce);
        }

        string GenerateCodeChallenge(string codeVerifier)
        {
            var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
            var b64Hash = Convert.ToBase64String(hash);
            var code = Regex.Replace(b64Hash, "\\+", "-");
            code = Regex.Replace(code, "\\/", "_");
            code = Regex.Replace(code, "=+$", "");
            return code;
        }
    }
}
