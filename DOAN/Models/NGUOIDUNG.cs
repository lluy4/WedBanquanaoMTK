namespace DOAN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NGUOIDUNG")]
    public partial class NGUOIDUNG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NGUOIDUNG()
        {
            GIOHANGs = new HashSet<GIOHANG>();
            HOADONs = new HashSet<HOADON>();
            HOADONs1 = new HashSet<HOADON>();
        }

        [Key]
        public int IdUser { get; set; }

        public int? IdLoaiUser { get; set; }

        [StringLength(50)]
        public string HoTen { get; set; }

        [StringLength(255)]
        public string Avatar { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgaySinh { get; set; }

        public bool GioiTinh { get; set; }

        [StringLength(1000)]
        public string DiaChi { get; set; }

        [StringLength(12)]
        public string SDT { get; set; }

        [StringLength(50)]
        public string Mail { get; set; }

  
        [StringLength(50)]
        public string Username { get; set; }

        
        [StringLength(50)]
        public string Password { get; set; }

        [StringLength(50)]
        public string Password1 { get; set; }

        public bool? TT_User { get; set; }

        public DateTime? NgayTao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GIOHANG> GIOHANGs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HOADON> HOADONs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HOADON> HOADONs1 { get; set; }

        public virtual LOAIUSER LOAIUSER { get; set; }
    }
}
