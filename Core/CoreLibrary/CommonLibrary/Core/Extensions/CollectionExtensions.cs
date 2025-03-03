namespace CoreLibrary.Core.Extensions
{
    /// <summary>
    /// Collection拡張メソッド用クラス
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// コレクションが null または Count = 0 か？
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">調べるコレクション</param>
        /// <returns>true : null または Count = 0</returns>
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            return (collection == null || collection.Count == 0);
        }

        /// <summary>
        /// 配列が null または Length = 0 か？
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">調べる配列</param>
        /// <returns>true : null または Length = 0</returns>
        public static bool IsNullOrEmpty<T>(this T[] array)
        {
            return (array == null || array.Length == 0);
        }
    }
}
