using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DOAN.Models
{
    [MetadataTypeAttribute(typeof(LoaiSanPhamMetadata))]
    public partial class LOAISANPHAM
    {
        internal sealed class LoaiSanPhamMetadata
        {
            //Lưu ý bỏ dấu hỏi chấm ở chỗ Danh mục này đi
            public int DanhMuc { get; set; }
        }
    }
}