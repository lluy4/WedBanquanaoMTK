namespace DOAN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("THUONGHIEU")]
    public partial class THUONGHIEU
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public THUONGHIEU()
        {
            SANPHAMs = new HashSet<SANPHAM>();
        }

        [Key]
        public int IdTH { get; set; }

        [StringLength(50)]
        [DisplayName("Tên thương hiệu")]
        public string TenTH { get; set; }

        [StringLength(50)]
        [DisplayName("Logo")]
        public string AnhTH { get; set; }

        [StringLength(15)]
        [DisplayName("SĐT")]
        public string SDT { get; set; }

        [StringLength(50)]
        [DisplayName("Địa chỉ")]
        public string DiaChi { get; set; }

        [Column(TypeName = "ntext")]
        [DisplayName("Chi tiết")]
        public string ChiTiet { get; set; }

        public bool? TinhTrang { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SANPHAM> SANPHAMs { get; set; }
    }
}
