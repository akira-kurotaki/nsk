using System.Linq.Expressions;

namespace CoreLibrary.Core.Linq
{
    /// <summary>
    /// 式ツリーを操作するユーティリティクラス
    /// </summary>
    internal static class ExpressionUtil
    {
        /// <summary>
        /// パラメータ式を構築する(ソート順)
        /// </summary>
        /// <param name="propertyName">タイプ名</param>
        /// <returns>構築された式</returns>
        public static Expression<Func<TSource, object>> GetOrderByExpression<TSource>(string propertyName)
        {
            var param = Expression.Parameter(typeof(TSource), "x");
            MemberExpression property = Expression.Property(param, propertyName);
            Type convertType = property.Type;
            if (convertType.IsGenericType && convertType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // Nullableの場合は元の型に変換
                convertType = Nullable.GetUnderlyingType(convertType);
            }
            Expression conversion = Expression.Convert(Expression.Property
                                                       (param, propertyName), convertType);

            return Expression.Lambda<Func<TSource, object>>(conversion, param);
        }

        /// <summary>
        /// パラメータ式を構築する(ソート順)
        /// </summary>
        /// <param name="propertyName">タイプ名</param>
        /// <returns>構築された式</returns>
        public static Expression<Func<TSource, TKey>> GetOrderByExpression<TSource, TKey>(string propertyName)
        {
            var param = Expression.Parameter(typeof(TSource), "x");
            MemberExpression property = Expression.Property(param, propertyName);
            Type convertType = property.Type;
            if (convertType.IsGenericType && convertType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // Nullableの場合は元の型に変換
                convertType = Nullable.GetUnderlyingType(convertType);
            }
            Expression conversion = Expression.Convert(Expression.Property
                                                       (param, propertyName), convertType);

            return Expression.Lambda<Func<TSource, TKey>>(conversion, param);
        }

        /// <summary>
        /// タイプコード取得メソッド
        /// </summary>
        /// <param name="propertyName">タイプ名</param>
        /// <returns>タイプコード</returns>
        public static TypeCode GetPropertyType<TSource>(string propertyName)
        {
            var param = Expression.Parameter(typeof(TSource), "x");
            MemberExpression property = Expression.Property(param, propertyName);
            Type convertType = property.Type;
            if (convertType.IsGenericType && convertType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // Nullableの場合は元の型に変換
                convertType = Nullable.GetUnderlyingType(convertType);
            }
            return Type.GetTypeCode(convertType);
        }

    }
}