using CoreLibrary.Core.Consts;
using NLog;
using System.Security.Cryptography;
using System.Text;

namespace CoreLibrary.Core.Utility
{
    /// <summary>
    /// ファイル暗号化/復号化のユーティリティクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2017/11/27
    /// 作成者：Nakamura Koichi
    /// </remarks>
    public static class CryptoUtil
    {
        /// <summary>
        /// セッションオブジェクト許容サイズ(単位：Byte)
        /// </summary>
        private static readonly long cryptoMaxSize = Convert.ToInt64(ConfigUtil.Get("CryptoMaxSize"));

        /// <summary>
        /// ファイルの暗号化
        /// </summary>
        /// <param name="targetData">ファイルデータ</param>
        /// <param name="fileName">ファイル名</param>
        /// <returns>暗号化済みデータ</returns>
        public static byte[] Encrypt(byte[] targetData, string fileName)
        {
            CheckInputData(targetData, fileName);
            using (var inStream = new MemoryStream(targetData))
            {
                //using (AesManaged aes = new AesManaged
                //{
                //    BlockSize = CoreConst.AES_BLOCK_SIZE_BIT,
                //    KeySize = CoreConst.AES_KEY_SIZE_BIT,
                //    Mode = CipherMode.CBC,
                //    Padding = PaddingMode.PKCS7
                //})
                //{
                using (Aes aes = Aes.Create()) {
                    aes.BlockSize = CoreConst.AES_BLOCK_SIZE_BIT;
                    aes.KeySize = CoreConst.AES_KEY_SIZE_BIT;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    // IVはAESが自動生成(16Byte)
                    aes.GenerateIV();

                    // Keyはハッシュ値を設定
                    var salt = GetSalt();
                    var phrase = CoreConfigUtil.Get(CoreConst.CORE_CONFIG_KEY_PHRASE);
                    aes.Key = GetSHA256(fileName, salt, phrase);

                    using (var outStream = new MemoryStream())
                    {
                        // ファイル先頭へIVを追加
                        outStream.Write(aes.IV, 0, CoreConst.AES_BLOCK_SIZE_BIT / 8);
                        // IVの次にSaltを追加
                        outStream.Write(salt, 0, CoreConst.AES_SALT_SIZE_BYTE);

                        using (var cryptoTransform = aes.CreateEncryptor())
                        {
                            using (var cryptoStream = new CryptoStream(outStream, cryptoTransform, CryptoStreamMode.Write))
                            {

                                var buffer = new byte[inStream.Length];
                                inStream.Read(buffer, 0, buffer.Length);
                                cryptoStream.Write(buffer, 0, buffer.Length);
                            }
                        }
                        return outStream.ToArray();
                    }

                }
            }
        }

        /// <summary>
        /// ファイルの復号化
        /// </summary>
        /// <param name="targetData">ファイルデータ</param>
        /// <param name="fileName">ファイル名</param>
        /// <returns>復号化済みファイルデータ</returns>
        public static byte[] Decrypt(byte[] targetData, string fileName)
        {
            CheckInputData(targetData, fileName);
            using (var inStream = new MemoryStream(targetData))
            {
                //using (AesManaged aes = new AesManaged
                //{
                //    BlockSize = CoreConst.AES_BLOCK_SIZE_BIT,
                //    KeySize = CoreConst.AES_KEY_SIZE_BIT,
                //    Mode = CipherMode.CBC,
                //    Padding = PaddingMode.PKCS7
                //})
                //{
                using (Aes aes = Aes.Create())
                {
                    aes.BlockSize = CoreConst.AES_BLOCK_SIZE_BIT;
                    aes.KeySize = CoreConst.AES_KEY_SIZE_BIT;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    // IVはファイル先頭16Byteを取得
                    var iv = new byte[CoreConst.AES_BLOCK_SIZE_BIT / 8];
                    inStream.Read(iv, 0, iv.Length);
                    aes.IV = iv;

                    // SaltはIVの次から32Byteを取得
                    var salt = new byte[CoreConst.AES_SALT_SIZE_BYTE];
                    inStream.Read(salt, 0, salt.Length);

                    // Keyはハッシュ値を設定
                    var phrase = CoreConfigUtil.Get(CoreConst.CORE_CONFIG_KEY_PHRASE);
                    aes.Key = GetSHA256(fileName, salt, phrase);

                    using (var outStream = new MemoryStream())
                    {
                        using (var cryptoTransform = aes.CreateDecryptor())
                        {
                            using (var cryptoStream = new CryptoStream(outStream, cryptoTransform, CryptoStreamMode.Write))
                            {
                                var buffer = new byte[inStream.Length - inStream.Position];
                                inStream.Read(buffer, 0, buffer.Length);
                                cryptoStream.Write(buffer, 0, buffer.Length);
                            }
                        }
                        return outStream.ToArray();
                    }
                }
            }

        }

        /// <summary>
        /// ファイルの復号化
        /// （メモリ使用量減少版）
        /// </summary>
        /// <param name="inStream">ファイルデータ</param>
        /// <param name="fileName">ファイル名</param>
        /// <returns>復号化済みファイルデータ</returns>
        public static byte[] Decrypt(Stream inStream, string fileName)
        {
            CheckInputData(inStream, fileName);
            {
                //using (AesManaged aes = new AesManaged
                //{
                //    BlockSize = CoreConst.AES_BLOCK_SIZE_BIT,
                //    KeySize = CoreConst.AES_KEY_SIZE_BIT,
                //    Mode = CipherMode.CBC,
                //    Padding = PaddingMode.PKCS7
                //})
                using (Aes aes = Aes.Create())
                {
                    aes.BlockSize = CoreConst.AES_BLOCK_SIZE_BIT;
                    aes.KeySize = CoreConst.AES_KEY_SIZE_BIT;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    // IVはファイル先頭16Byteを取得
                    var iv = new byte[CoreConst.AES_BLOCK_SIZE_BIT / 8];
                    inStream.Read(iv, 0, iv.Length);
                    aes.IV = iv;

                    // SaltはIVの次から32Byteを取得
                    var salt = new byte[CoreConst.AES_SALT_SIZE_BYTE];
                    inStream.Read(salt, 0, salt.Length);

                    // Keyはハッシュ値を設定
                    var phrase = CoreConfigUtil.Get(CoreConst.CORE_CONFIG_KEY_PHRASE);
                    aes.Key = GetSHA256(fileName, salt, phrase);

                    using (var outStream = new MemoryStream())
                    {
                        using (var cryptoTransform = aes.CreateDecryptor())
                        {
                            int megabyte = 1024 * 1024;
                            int maxLen = (int)((inStream.Length - inStream.Position) / megabyte);
                            using (var cryptoStream = new CryptoStream(outStream, cryptoTransform, CryptoStreamMode.Write))
                            {
                                for (var i = 0; i < maxLen + 1; ++i)
                                {
                                    var buffer = new byte[megabyte];
                                    var len = inStream.Read(buffer, 0, buffer.Length);
                                    cryptoStream.Write(buffer, 0, len);
                                }
                            }
                        }
                        return outStream.ToArray();
                    }
                }
            }
        }

        /// <summary>
        /// 対象データのSHA256によるハッシュ値を取得
        /// </summary>
        /// <param name="target">対象データ</param>
        /// <returns>ハッシュ値</returns>
        public static byte[] GetSHA256(byte[] target)
        {
            //using (var sha256 = SHA256Managed.Create())
            //using (var sha256 = SHA256.Create())
            //{
            //    return sha256.ComputeHash(target);
            //}

            return SHA256.HashData(target);
        }

        /// <summary>
        /// 対象文字列のSHA256によるハッシュ値を取得(ソルト、フレーズを使用)
        /// </summary>
        /// <param name="target">対象文字列</param>
        /// <param name="salt">ソルト</param>
        /// <param name="phrase">フレーズ</param>
        /// <returns>ハッシュ値(32Byte)</returns>
        private static byte[] GetSHA256(string target, byte[] salt, string phrase)
        {
            Encoding encoding = Encoding.UTF8;
            return GetSHA256(salt.Concat(
                encoding.GetBytes(target).Concat(
                    encoding.GetBytes(phrase))).ToArray());
        }

        /// <summary>
        /// 対象データのSHA256による16進数表記のハッシュ値を取得
        /// </summary>
        /// <param name="target">対象データ</param>
        /// <returns>16進数のハッシュ値文字列</returns>
        public static string GetSHA256Hex(byte[] target)
        {
            var hashList = GetSHA256(target);
            StringBuilder retValue = new StringBuilder();
            foreach (var hash in hashList)
            {
                retValue.Append(hash.ToString("x2"));
            }
            return retValue.ToString();
        }

        /// <summary>
        /// 対象データのMD5による16進数表記のハッシュ値を取得
        /// </summary>
        /// <param name="target">対象データ</param>
        /// <returns>16進数のハッシュ値文字列</returns>
        public static string GetMD5Hex(byte[] target)
        {
            //MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            MD5 md5 = MD5.Create();
            byte[] bs = md5.ComputeHash(target);
            md5.Clear();

            StringBuilder retValue = new StringBuilder();
            foreach (byte b in bs)
            {
                retValue.Append(b.ToString("x2"));
            }

            return retValue.ToString();
        }

        /// <summary>
        /// ファイル毎に異なるソルトを取得
        /// </summary>
        /// <returns>ソルト</returns>
        private static byte[] GetSalt()
        {
            //using (RNGCryptoServiceProvider random = new RNGCryptoServiceProvider())
            using (RandomNumberGenerator random = RandomNumberGenerator.Create())
            {
                var retSalt = new byte[CoreConst.AES_SALT_SIZE_BYTE];
                random.GetBytes(retSalt);
                return retSalt;
            }
        }

        /// <summary>
        /// 入力データチェック
        /// </summary>
        /// <param name="targetData">ファイルデータ</param>
        /// <param name="fileName">ファイル名</param>
        private static void CheckInputData(byte[] targetData, string fileName)
        {
            var messageId = "MF00002";
            var targetData_Name = "ファイルデータ";
            var fileName_Name = "ファイル名";

            if (null == targetData)
            {
                throw new ArgumentNullException(SystemMessageUtil.Get(messageId, targetData_Name));
            }

            if (null == fileName)
            {
                throw new ArgumentNullException(SystemMessageUtil.Get(messageId, fileName_Name));
            }
            else if (string.Empty.Equals(fileName))
            {
                throw new ArgumentException(SystemMessageUtil.Get(messageId, fileName_Name));
            }

            if (targetData.LongLength > cryptoMaxSize)
            {
                // 最大値を超えるサイズ（{0}(byte)）のデータを暗号化 / 復号化しようとしています。
                // （ファイル名: {1} , サイズ: {2}(byte)）。システム管理者に連絡してください。
                LogManager.GetCurrentClassLogger().Log(NLog.LogLevel.Warn, SystemMessageUtil.Get(
                    "MW00007", cryptoMaxSize.ToString(), fileName, targetData.LongLength.ToString()));
            }
        }

        /// <summary>
        /// 入力データチェック
        /// </summary>
        /// <param name="inStream">ファイルデータ（ストリーム）</param>
        /// <param name="fileName">ファイル名</param>
        private static void CheckInputData(Stream inStream, string fileName)
        {
            var messageId = "MF00002";
            var targetData_Name = "ファイルデータ";
            var fileName_Name = "ファイル名";

            if (null == inStream)
            {
                throw new ArgumentNullException(SystemMessageUtil.Get(messageId, targetData_Name));
            }

            if (null == fileName)
            {
                throw new ArgumentNullException(SystemMessageUtil.Get(messageId, fileName_Name));
            }
            else if (string.Empty.Equals(fileName))
            {
                throw new ArgumentException(SystemMessageUtil.Get(messageId, fileName_Name));
            }

            if (inStream.Length > cryptoMaxSize)
            {
                // 最大値を超えるサイズ（{0}(byte)）のデータを暗号化 / 復号化しようとしています。
                // （ファイル名: {1} , サイズ: {2}(byte)）。システム管理者に連絡してください。
                LogManager.GetCurrentClassLogger().Log(NLog.LogLevel.Warn, SystemMessageUtil.Get(
                    "MW00007", cryptoMaxSize.ToString(), fileName, inStream.Length.ToString()));
            }
        }
    }
}