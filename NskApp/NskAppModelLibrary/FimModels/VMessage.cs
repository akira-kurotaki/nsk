using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.FimModels
{
    /// <summary>
    /// メッセージマスタ
    /// </summary>
    [Serializable]
    [Table("v_message")]
    public class VMessage : ModelBase
    {
        /// <summary>
        /// メッセージID
        /// </summary>
        [Required]
        [Key]
        [Column("message_id", Order = 1)]
        [StringLength(7)]
        public string MessageId { get; set; }

        /// <summary>
        /// メッセージ
        /// </summary>
        [Column("message")]
        [StringLength(1000)]
        public string Message { get; set; }
    }
}
