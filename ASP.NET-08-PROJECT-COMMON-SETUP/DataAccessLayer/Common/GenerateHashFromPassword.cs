using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Common
{
    public class GenerateHashFromPassword
    {
            /// <summary>
            /// Get HASH value for your password
            /// </summary>
            /// <param name="password"></param>
            /// <returns></returns>
            public static string GetHash(string password, string saltString)
            {
                var salt = System.Text.Encoding.Unicode.GetBytes(saltString);

                // derive a 512-bit subkey (use HMACSHA1 with 10,000 iterations)
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                                                            password: password,
                                                                            salt: salt,
                                                                            prf: KeyDerivationPrf.HMACSHA1,
                                                                            iterationCount: 10000,
                                                                            numBytesRequested: 512 / 8
                                                                            ));
                return hashed;
            }

        public static string GenerateSalt()
        {
            var rng = RandomNumberGenerator.Create();
            byte[] saltBytes = new byte[16];
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

    }
}
