using AdsReportingPortal.Api.Service.Interface;
using System.Security.Cryptography;

namespace AdsReportingPortal.Api.Service.Implementation
{
    public class EncryptionService : IEncryptionService
    {
        public string Decrypt(string cipherText, string key)
        {
            byte[] fullCipher = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Convert.FromBase64String(key);
                aes.Padding = PaddingMode.PKCS7;

                
                byte[] iv = new byte[aes.BlockSize / 8];
                byte[] actualCipher = new byte[fullCipher.Length - iv.Length];

                Array.Copy(fullCipher, iv, iv.Length);
                Array.Copy(fullCipher, iv.Length, actualCipher, 0, actualCipher.Length);

                aes.IV = iv;

                using (MemoryStream memoryStream = new MemoryStream(actualCipher))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(cryptoStream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
        }

        public string Encrypt(string plainText, string key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Convert.FromBase64String(key);
                aes.GenerateIV();
                aes.Padding = PaddingMode.PKCS7;

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    
                    memoryStream.Write(aes.IV, 0, aes.IV.Length);

                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (StreamWriter writer = new StreamWriter(cryptoStream))
                        {
                            writer.Write(plainText);
                        }
                    }

                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
        }

    }
}
