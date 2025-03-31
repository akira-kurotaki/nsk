using NskAppModelLibrary.Models;

namespace NskCommonLibrary.Models
{
    public class BatchJokenNameValue
    {
        /// <summary>
        /// 条件名称
        /// </summary>
        public string 条件名称 { get; set; } = string.Empty;

        /// <summary>
        /// バッチ条件
        /// </summary>
        //public string 条件値 { get; set; }
        public T01050バッチ条件 バッチ条件 { get; set; } = new T01050バッチ条件();
    }
}
