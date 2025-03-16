namespace DOAN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DTGIAOHANG")]
    public partial class DTGIAOHANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DTGIAOHANG()
        {
            HOADONs = new HashSet<HOADON>();
        }

        [Key]
        public int IdDTGH { get; set; }

        [StringLength(50)]
        public string TenDTGH { get; set; }

        public bool? TinhTrang { get; set; }

        public int? TienVanChuyen { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HOADON> HOADONs { get; set; }
    }
}
