namespace DOAN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PHANQUYEN")]
    public partial class PHANQUYEN
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdLoaiUser { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(30)]
        public string TinhNang { get; set; }

        public virtual LOAIUSER LOAIUSER { get; set; }
    }
}
