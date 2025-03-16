using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DOAN.Models
{
    [MetadataTypeAttribute(typeof(GioHangMetadata))]
    public partial class GIOHANG
    {
        internal sealed class GioHangMetadata
        {
            //Lưu ý bỏ dấu hỏi chấm ở chỗ So Luong này đi
            public int SoLuong { get; set; }
        }
    }
}