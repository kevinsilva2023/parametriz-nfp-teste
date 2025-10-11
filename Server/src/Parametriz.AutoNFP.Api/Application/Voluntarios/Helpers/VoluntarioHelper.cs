using System.Security.Cryptography;
using System.Text;

namespace Parametriz.AutoNFP.Api.Application.Voluntarios.Helpers
{
    public class VoluntarioHelper
    {

    }
        
    public static class AesEncryptionHelper
    {
        private static readonly int KeySize = 256;
        private static readonly int IvSize = 16; // 128 bits para o IV

        // Este método é para demonstração. Em produção, recupere a chave de uma fonte segura.
        private static byte[] GetKey()
        {
            // Use uma chave segura e gerenciada. Por exemplo, do Azure Key Vault.
            // Nunca armazene a chave em texto plano no código ou arquivos de configuração.
            return Encoding.UTF8.GetBytes("esta-e-uma-chave-segura-e-secreta!"); // Exemplo
        }

        /// <summary>
        /// Criptografa uma string usando AES e retorna uma string em Base64.
        /// Inclui o IV no início da string criptografada para facilitar a descriptografia.
        /// </summary>
        public static string Encrypt(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = KeySize;
                aes.Mode = CipherMode.CBC; // CBC é um modo seguro.

                aes.Key = GetKey();
                aes.GenerateIV(); // Cria um IV único para cada operação.

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    // Escreve o IV na stream para recuperá-lo na descriptografia.
                    ms.Write(aes.IV, 0, IvSize);

                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// Descriptografa uma string em Base64 usando AES.
        /// </summary>
        public static string Decrypt(string cipherTextBase64)
        {
            byte[] fullCipher = Convert.FromBase64String(cipherTextBase64);

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = KeySize;
                aes.Mode = CipherMode.CBC;

                aes.Key = GetKey();

                byte[] iv = new byte[IvSize];
                Array.Copy(fullCipher, 0, iv, 0, IvSize);
                aes.IV = iv;

                using (MemoryStream ms = new MemoryStream())
                {
                    // Copia o texto cifrado (sem o IV) para a stream.
                    ms.Write(fullCipher, IvSize, fullCipher.Length - IvSize);
                    ms.Seek(0, SeekOrigin.Begin); // Retorna ao início da stream.

                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }
    }

}
