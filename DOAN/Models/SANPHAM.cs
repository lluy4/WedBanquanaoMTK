namespace DOAN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SANPHAM")]
    public partial class SANPHAM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SANPHAM()
        {
            CHITIETHDs = new HashSet<CHITIETHD>();
            GIOHANGs = new HashSet<GIOHANG>();
            NHAPHANGs = new HashSet<NHAPHANG>();
        }

        [Key]
        public int IdSP { get; set; }

        [StringLength(500)]
        [DisplayName("Tên sản phẩm")]
        public string TenSP { get; set; }

        [StringLength(50)]
        [DisplayName("Hình sản phẩm")]
        public string AnhSP { get; set; }

        public DateTime? NgayTao { get; set; }

        [Column(TypeName = "ntext")]
        [DisplayName("Mô tả")]
        public string MoTa { get; set; }

        [DisplayName("Tình trạng")]
        public int? TinhTrang { get; set; }

        [DisplayName("Giá gốc")]
        public int GiaGoc { get; set; }

        [DisplayName("Lợi nhuận")]
        public float? LoiNhuan { get; set; }

        [StringLength(20)]
        [DisplayName("Đơn vị")]
        public string DonVi { get; set; }

        [DisplayName("Số lượng")]
        public int? SoLuong { get; set; }

        [DisplayName("Số lần mua")]
        public int? SoLanMua { get; set; }

   
        public int? MaKM { get; set; }

        [DisplayName("Thương hiệu")]
        public int? IdTH { get; set; }

        [DisplayName("Loại sản phẩm")]
        public int? IdLoaiSP { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHITIETHD> CHITIETHDs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GIOHANG> GIOHANGs { get; set; }

        public virtual KHUYENMAI KHUYENMAI { get; set; }

        public virtual LOAISANPHAM LOAISANPHAM { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NHAPHANG> NHAPHANGs { get; set; }

        public virtual THUONGHIEU THUONGHIEU { get; set; }

        public virtual TINHTRANG TINHTRANG1 { get; set; }
    }
}
