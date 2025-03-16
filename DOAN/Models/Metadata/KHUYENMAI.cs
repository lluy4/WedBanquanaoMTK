using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace DOAN.Models
{
    [MetadataTypeAttribute(typeof(KhuyenMaiMetadata))]
    public partial class KHUYENMAI
    {
        internal sealed class KhuyenMaiMetadata
        {
            [Required(ErrorMessage = "{0} không được để trống")]
            [StringLength(30, ErrorMessage = "Không quá 30 ký tự")]
            [DisplayName("Mã khuyến mãi")]
            public string MaKM { get; set; }

            [DisplayName("Loại khuyến mãi")]
            [Required(ErrorMessage = "{0} không được để trống")]
            public int? LoaiKM { get; set; }

            [DisplayName("Ngày bắt đầu")]
            [Required(ErrorMessage = "{0} không được để trống")]
            public DateTime? NgayBD { get; set; }

            [DisplayName("Ngày kết thúc")]
            [Required(ErrorMessage = "{0} không được để trống")]
            public DateTime? NgayKT { get; set; }

            [DisplayName("Giá trị")]
            [Required(ErrorMessage = "{0} không được để trống")]
            public int? GiaTri { get; set; }

            [DisplayName("Chi tiết")]
            public string ChiTiet { get; set; }
        }
    }
}