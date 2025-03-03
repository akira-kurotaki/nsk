using Microsoft.AspNetCore.Identity;

namespace CoreLibrary.Core.Utility
{
    /// <summary>
    /// パスワードユーティリティ
    /// </summary>
    public static class PasswordUtil
    {
        /// <summary>
        /// パスワードハッシュを生成する
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password">パスワード（平文）</param>
        /// <returns>パスワードハッシュ</returns>
        public static string HashPassword<TUser>(TUser user, string password) where TUser : class
        {
            var hasher = new PasswordHasher<TUser>();

            return hasher.HashPassword(user, password);
        }

        /// <summary>
        /// パスワードハッシュの比較結果を返す
        /// </summary>
        /// <param name="user"></param>
        /// <param name="hashedPassword">パスワードハッシュ</param>
        /// <param name="providedPassword">パスワード（平文）</param>
        /// <returns>true：一致、false：不一致（ハッシュアルゴリズムが古いも含む）</returns>
        public static bool VerifyHashedPassword<TUser>(TUser user, string hashedPassword, string providedPassword)
            where TUser : class
        {
            try
            {
                var hasher = new PasswordHasher<TUser>();
                return (PasswordVerificationResult.Success == hasher.VerifyHashedPassword(user, hashedPassword, providedPassword));
            }
            catch (Exception)
            {
                // 引数がnullや、パスワードハッシュがBase64エンコード文字列ではない場合に例外が発生する
                return false;
            }
        }
    }
}
