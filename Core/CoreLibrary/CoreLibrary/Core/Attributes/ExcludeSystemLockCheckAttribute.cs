namespace CoreLibrary.Core.Attributes
{
    /// <summary>
    /// システムロックチェック除外属性
    /// </summary>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class)]
    public class ExcludeSystemLockCheckAttribute : Attribute
    {
    }
}