using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F990.Models.D990002
{
    /// <summary>
    /// データロック
    /// </summary>
    [Serializable]
    public class D990002DataLocks
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D990002DataLocks() {
            Locks = [];
        }

        /// <summary>
        /// データロック
        /// </summary>
        [Display(Name = "データロック")]
        public List<D990002DataLockRecord> Locks { get; set; }
    }
}
