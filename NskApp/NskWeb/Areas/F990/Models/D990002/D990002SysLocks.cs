using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F990.Models.D990002
{
    /// <summary>
    /// システムロック
    /// </summary>
    [Serializable]
    public class D990002SysLocks
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D990002SysLocks() {
            Locks = [];
        }

        /// <summary>
        /// システムロック
        /// </summary>
        [Display(Name = "システムロック")]
        public List<D990002SysLockRecord> Locks { get; set; }
    }
}
