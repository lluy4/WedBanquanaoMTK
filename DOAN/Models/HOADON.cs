namespace DOAN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HOADON")]
    public partial class HOADON
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HOADON()
        {
            CHITIETHDs = new HashSet<CHITIETHD>();
        }

        [Key]
        [DisplayName("Mã hoá đơn")]
        public int IdHD { get; set; }

        [DisplayName("Ngày đặt hàng")]
        public DateTime? NgayDH { get; set; }

        [DisplayName("Tổng tiền")]
        public int? TongTien { get; set; }

        [DisplayName("Khách hàng")]
        public int? IdKH { get; set; }

        [DisplayName("Khuyến mãi")]
        public int? IdKM { get; set; }

        [Required]
        [StringLength(12)]
        [DisplayName("SĐT")]
        public string SDT { get; set; }

        [StringLength(1000)]
        [DisplayName("Địa chỉ")]
        public string DiaChi { get; set; }

        public int? TinhTrang { get; set; }

        public int? IdDTGH { get; set; }

        public DateTime? NgayGH { get; set; }

        public bool? DaThanhToan { get; set; }

        [DisplayName("Phí vận chuyển")]
        public int? TienVanChuyen { get; set; }

        public int? IdNV { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHITIETHD> CHITIETHDs { get; set; }

        public virtual DTGIAOHANG DTGIAOHANG { get; set; }

        public virtual NGUOIDUNG NGUOIDUNG { get; set; }

        public virtual KHUYENMAI KHUYENMAI { get; set; }

        public virtual NGUOIDUNG NGUOIDUNG1 { get; set; }

        public virtual TINHTRANG TINHTRANG1 { get; set; }
    }
}
