using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Linq;
using System.Linq.Expressions;

namespace CoreLibrary.Core.Extensions
{
    /// <summary>
    /// System.Linq.IQueryableの拡張メソッド実装クラス
    /// </summary>
    public static class IQueryableExtention
    {        
        /// <summary>
        /// 指定されたソート順をソートした結果を返します。
        /// </summary>
        /// <typeparam name="TSource">source の要素の型</typeparam>
        /// <param name="source">テストおよびカウントする要素が格納されているシーケンス。</param>
        /// <param name="optionList">ソート順オプションリスト</param>
        /// <returns>ソートした結果を返します。</returns>
        public static IQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source,
                                                           List<OrderbyOption> optionList)
        {
            if (optionList == null)
            {
                return source;
            }
            else
            {
                if (optionList.Count() == 0)
                {
                    return source;
                }
                else
                {
                    IOrderedQueryable<TSource> returnSource = null;
                    for (int index = 0; index < optionList.Count(); index++)
                    {
                        OrderbyOption option = optionList[index];
                        if (string.IsNullOrEmpty(option.PropertyName))
                        {
                            return source;
                        }
                        if (index == 0)
                        {
                            returnSource = source.OrderBy(option.PropertyName, option.SortOrder);
                        }
                        else
                        {
                            returnSource = returnSource.ThenBy(option.PropertyName, option.SortOrder);
                        }
                    }
                    return returnSource;
                }
            }
        }
        
        /// <summary>
        /// 指定されたソート順をソートした結果を返します。
        /// </summary>
        /// <typeparam name="TSource">source の要素の型</typeparam>
        /// <param name="source">テストおよびカウントする要素が格納されているシーケンス。</param>
        /// <param name="propertyName">要素名</param>
        /// <param name="order">ソート順タイプ</param>
        /// <returns>ソートした結果を返します。</returns>
        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source,
                                                                  string propertyName, CoreConst.SortOrder order)
        {
            TypeCode code = ExpressionUtil.GetPropertyType<TSource>(propertyName);

            if (TypeCode.Int16 == code)
            {
                Expression<Func<TSource, short>> expression = ExpressionUtil.GetOrderByExpression<TSource, short>(propertyName);
                return CoreConst.SortOrder.ASC.Equals(order) ? source.OrderBy(expression) : source.OrderByDescending(expression);
            }
            else if (TypeCode.Int32 == code)
            {
                Expression<Func<TSource, int>> expression = ExpressionUtil.GetOrderByExpression<TSource, int>(propertyName);
                return CoreConst.SortOrder.ASC.Equals(order) ? source.OrderBy(expression) : source.OrderByDescending(expression);
            }
            else if (TypeCode.Int64 == code)
            {
                Expression<Func<TSource, long>> expression = ExpressionUtil.GetOrderByExpression<TSource, long>(propertyName);
                return CoreConst.SortOrder.ASC.Equals(order) ? source.OrderBy(expression) : source.OrderByDescending(expression);
            }
            else if (TypeCode.Decimal == code)
            {
                Expression<Func<TSource, decimal>> expression = ExpressionUtil.GetOrderByExpression<TSource, decimal>(propertyName);
                return CoreConst.SortOrder.ASC.Equals(order) ? source.OrderBy(expression) : source.OrderByDescending(expression);
            }
            else if (TypeCode.Double == code)
            {
                Expression<Func<TSource, double>> expression = ExpressionUtil.GetOrderByExpression<TSource, double>(propertyName);
                return CoreConst.SortOrder.ASC.Equals(order) ? source.OrderBy(expression) : source.OrderByDescending(expression);
            }
            else if (TypeCode.DateTime == code)
            {
                Expression<Func<TSource, DateTime>> expression = ExpressionUtil.GetOrderByExpression<TSource, DateTime>(propertyName);
                return CoreConst.SortOrder.ASC.Equals(order) ? source.OrderBy(expression) : source.OrderByDescending(expression);
            }
            else if (TypeCode.String == code)
            {
                Expression<Func<TSource, string>> expression = ExpressionUtil.GetOrderByExpression<TSource, string>(propertyName);
                return CoreConst.SortOrder.ASC.Equals(order) ? source.OrderBy(expression) : source.OrderByDescending(expression);
            }
            else if (TypeCode.Boolean == code)
            {
                Expression<Func<TSource, bool>> expression = ExpressionUtil.GetOrderByExpression<TSource, bool>(propertyName);
                return CoreConst.SortOrder.ASC.Equals(order) ? source.OrderBy(expression) : source.OrderByDescending(expression);
            }
            else
            {
                Expression<Func<TSource, object>> expression = ExpressionUtil.GetOrderByExpression<TSource, object>(propertyName);
                return CoreConst.SortOrder.ASC.Equals(order) ? source.OrderBy(expression) : source.OrderByDescending(expression);
            }
        }

        /// <summary>
        /// 指定されたソート順をソートした結果を返します。
        /// </summary>
        /// <typeparam name="TSource">source の要素の型</typeparam>
        /// <param name="source">テストおよびカウントする要素が格納されているシーケンス。</param>
        /// <param name="propertyName">要素名</param>
        /// <param name="order">ソート順タイプ</param>
        /// <returns>ソートした結果を返します。</returns>
        public static IOrderedQueryable<TSource> ThenBy<TSource>(this IOrderedQueryable<TSource> source,
                                                                 string propertyName, CoreConst.SortOrder order)
        {
            TypeCode code = ExpressionUtil.GetPropertyType<TSource>(propertyName);

            if (TypeCode.Int16 == code)
            {
                Expression<Func<TSource, short>> expression = ExpressionUtil.GetOrderByExpression<TSource, short>(propertyName);
                return CoreConst.SortOrder.ASC.Equals(order) ? source.ThenBy(expression) : source.ThenByDescending(expression);
            }
            else if (TypeCode.Int32 == code)
            {
                Expression<Func<TSource, int>> expression = ExpressionUtil.GetOrderByExpression<TSource, int>(propertyName);
                return CoreConst.SortOrder.ASC.Equals(order) ? source.ThenBy(expression) : source.ThenByDescending(expression);
            }
            else if (TypeCode.Int64 == code)
            {
                Expression<Func<TSource, long>> expression = ExpressionUtil.GetOrderByExpression<TSource, long>(propertyName);
                return CoreConst.SortOrder.ASC.Equals(order) ? source.ThenBy(expression) : source.ThenByDescending(expression);
            }
            else if (TypeCode.Decimal == code)
            {
                Expression<Func<TSource, decimal>> expression = ExpressionUtil.GetOrderByExpression<TSource, decimal>(propertyName);
                return CoreConst.SortOrder.ASC.Equals(order) ? source.ThenBy(expression) : source.ThenByDescending(expression);
            }
            else if (TypeCode.Double == code)
            {
                Expression<Func<TSource, double>> expression = ExpressionUtil.GetOrderByExpression<TSource, double>(propertyName);
                return CoreConst.SortOrder.ASC.Equals(order) ? source.ThenBy(expression) : source.ThenByDescending(expression);
            }
            else if (TypeCode.DateTime == code)
            {
                Expression<Func<TSource, DateTime>> expression = ExpressionUtil.GetOrderByExpression<TSource, DateTime>(propertyName);
                return CoreConst.SortOrder.ASC.Equals(order) ? source.ThenBy(expression) : source.ThenByDescending(expression);
            }
            else if (TypeCode.String == code)
            {
                Expression<Func<TSource, string>> expression = ExpressionUtil.GetOrderByExpression<TSource, string>(propertyName);
                return CoreConst.SortOrder.ASC.Equals(order) ? source.ThenBy(expression) : source.ThenByDescending(expression);
            }
            else if (TypeCode.Boolean == code)
            {
                Expression<Func<TSource, bool>> expression = ExpressionUtil.GetOrderByExpression<TSource, bool>(propertyName);
                return CoreConst.SortOrder.ASC.Equals(order) ? source.ThenBy(expression) : source.ThenByDescending(expression);
            }
            else
            {
                Expression<Func<TSource, object>> expression = ExpressionUtil.GetOrderByExpression<TSource, object>(propertyName);
                return CoreConst.SortOrder.ASC.Equals(order) ? source.ThenBy(expression) : source.ThenByDescending(expression);
            }
        }
    }
}