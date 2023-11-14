﻿using System.Security.Cryptography;
using System.Text;

namespace ProyectoFinal.Tools
{
    public static class HelperCryptography
    {
        public static string GenerateSalt()
        {
            Random random = new Random();
            string salt = "";
            for (int i = 1; i <= 50; i++)
            {
                int numero = random.Next(0, 255);
                char letra = Convert.ToChar(numero);
                salt += letra;
            }
            return salt;
        }

        public static bool compareArrays(byte[] a, byte[] b)
        {
            bool iguales = true;
            if (a.Length != b.Length)
            {
                iguales = false;
            }
            else
            {
                //comparamos byte a byte
                for (int i = 0; i < a.Length; i++)
                {
                    if (a[i].Equals(b[i]) == false)
                    {
                        iguales = false;
                        break;
                    }
                }
            }

            return iguales;
        }

        /// <summary>
        /// Encripatar contraseña con salt
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns>password encripted</returns>
        public static byte[] EncriptarPassword(string password, string salt)
        {
            string contenido = password + salt;
            SHA256Managed sha = new SHA256Managed();
            byte[] salida = Encoding.UTF8.GetBytes(contenido);
            for (int i = 1; i <= 107; i++)
            {
                salida = sha.ComputeHash(salida);
            }
            sha.Clear();
            return salida;

        }
        /// <summary>
        /// Codificacion SHA256 de texto plano
        /// </summary>
        /// <param name="plaintext">string value</param>
        /// <returns>HEX string encoded</returns>
        public static string EncodeSHA256Hash(string plaintext)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(plaintext);
                byte[] hashBytes = sha256.ComputeHash(bytes);

                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    stringBuilder.Append(hashBytes[i].ToString("x2")); // Convierte el byte a su representación en hexadecimal
                }

                return stringBuilder.ToString();
            }
        }
        /// <summary>
        /// Encriptacion de texto plano
        /// </summary>
        /// <param name="plaintext">string value</param>
        /// <param name="key">key size 32</param>
        /// <param name="iv">iv size 16</param>
        /// <returns>byte[] data encripted</returns>
        public static byte[] EncryptStringToBytes_Aes(string plaintext, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                byte[] encryptedBytes;
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] plainBytes = Encoding.UTF8.GetBytes(plaintext);
                        csEncrypt.Write(plainBytes, 0, plainBytes.Length);
                    }
                    encryptedBytes = msEncrypt.ToArray();
                }
                return encryptedBytes;
            }
        }
        /// <summary>
        /// Decodificacion de un byte[]
        /// </summary>
        /// <param name="ciphertext">data encripted</param>
        /// <param name="key">key size 32</param>
        /// <param name="iv">iv size 16</param>
        /// <returns>string decrypted</returns>
        public static string DecryptStringToBytes_Aes(byte[] ciphertext, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                byte[] decryptedBytes;
                using (var msDecrypt = new MemoryStream(ciphertext))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var msPlain = new MemoryStream())
                        {
                            csDecrypt.CopyTo(msPlain);
                            decryptedBytes = msPlain.ToArray();
                        }
                    }
                }
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }



    }
}