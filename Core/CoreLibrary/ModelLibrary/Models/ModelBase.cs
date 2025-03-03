using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// 収入保険モデル抽象クラス
    /// 
    /// 作成日：2017/12/04
    /// 作成者：MATSUBAYASHI Atsushi
    /// </summary>
    [Serializable]
    public abstract class ModelBase
    {
        [ConcurrencyCheck]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("xmin")]
        public uint Xmin { get; set; }
    }
}