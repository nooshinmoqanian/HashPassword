using System.Data.SqlTypes;
using System.Security.Cryptography;
using System.Text;

namespace freeTime.Utils
{
    public class HashPassword
    {
        public string Hashing(string password)
        {
            using (SHA256 sha256 = SHA256.Create())

            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }


                return builder.ToString();
            }
        }


        private static readonly byte[] Key = Encoding.ASCII.GetBytes("1234567890123456"); 
        private static readonly byte[] IV = Encoding.ASCII.GetBytes("1234567890123456"); // random

        public string Encrypt(string password)
        {
            using(Aes aes = Aes.Create()) 
            {
                aes.Key = Key;
                aes.IV = IV;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using(MemoryStream ms = new MemoryStream()) 
                {
                   using(CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using(StreamWriter  sw = new StreamWriter(cs))
                        {
                           sw.Write(password);
                        }
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }

        }


        public string Decrypt(string password)
        {
            byte[] cipherBytes = Convert.FromBase64String(password);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        public string createSalt(int size)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            byte[] buffer = new byte[size];

            rng.GetBytes(buffer);   

            return Convert.ToBase64String(buffer);

        }

        public string Salitng(string password, string salt)
        {
            byte[] saltByte = Convert.FromBase64String(salt);

            using (SHA256  sha256 = SHA256.Create()) 
            {
                byte[] passByte = Encoding.UTF8.GetBytes(password);
                byte[] passSalt = new byte[passByte.Length + saltByte.Length];

                Buffer.BlockCopy(passByte, 0, passSalt, 0, passByte.Length);
                Buffer.BlockCopy(saltByte, 0, passSalt, passByte.Length, saltByte.Length);

                byte[] hashed = sha256.ComputeHash(passSalt);

                return Convert.ToBase64String(hashed);
            }

           
        }

        public bool VerifiySalting(string password, string passwordHashed, string salt)
        {
            string hashed = Salitng(password, salt);
            return passwordHashed == hashed;
        }
    }
}
