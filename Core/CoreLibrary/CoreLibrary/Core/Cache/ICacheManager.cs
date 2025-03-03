namespace CoreLibrary.Core.Cache
{
    /// <summary>
    /// キャッシュマネージャーインタフェース
    /// </summary>
    /// <remarks>
    /// 作成日：2017/10/30
    /// 作成者：Rou I
    /// </remarks>
    public interface ICacheManager
    {
        /// <summary>
        /// キャッシュ対象の取得メソッド。
        /// </summary>
        /// <param name="key">キャッシュキー</param>
        /// <returns>キャッシュ対象</returns>
        T Get<T>(string key);

        /// <summary>
        /// キャッシュ対象の設定メソッド。
        /// </summary>
        /// <param name="key">キャッシュキー</param>
        /// <param name="data">キャッシュデータ</param>
        /// <param name="cacheTime">キャッシュ保存有効期限</param>
        /// <returns>なし</returns>
        void Set(string key, object data, int cacheTime);

        /// <summary>
        /// キャッシュ対象設定ありの判定メソッド。
        /// </summary>
        /// <param name="key">キャッシュキー</param>
        /// <returns>設定ありの場合、trueを返す</returns>
        bool IsSet(string key);

        /// <summary>
        /// キャッシュ対象の削除メソッド。
        /// </summary>
        /// <param name="key">キャッシュキー</param>
        /// <returns>なし</returns>
        void Remove(string key);


        /// <summary>
        /// キャッシュ対象のリフレッシュメソッド。
        /// </summary>
        /// <param name="key">キャッシュキー</param>
        /// <param name="data">キャッシュデータ</param>
        /// <param name="cacheTime">キャッシュ保存有効期限</param>
        /// <returns>なし</returns>
        void Refresh(string key, object data, int cacheTime);

        /// <summary>
        /// キャッシュ全対象のクリアメソッド。
        /// </summary>
        /// <returns>なし</returns>
        void Clear();
    }
}