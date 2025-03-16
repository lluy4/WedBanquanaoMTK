using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DOAN.Models
{
    [MetadataTypeAttribute(typeof(SanPhamMetadata))]
    public partial class SANPHAM
    {
        internal sealed class SanPhamMetadata
        {
            [Required(ErrorMessage = "{0} không thể để trống")]
            public float? LoiNhuan { get; set; }

            [Required(ErrorMessage = "{0} không thể để trống")]
            public int GiaGoc { get; set; }
        }
    }

}