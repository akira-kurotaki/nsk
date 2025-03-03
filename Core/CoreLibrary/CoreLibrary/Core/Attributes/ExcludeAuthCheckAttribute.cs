namespace CoreLibrary.Core.Attributes
{
    /// <summary>
    /// 権限チェック除外属性
    /// </summary>
    /// <remarks>
    /// 作成日：2018/02/15
    /// 作成者：MATSUBAYSHI Atsushi
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class)]
    public class ExcludeAuthCheckAttribute : Attribute
    {
    }
}