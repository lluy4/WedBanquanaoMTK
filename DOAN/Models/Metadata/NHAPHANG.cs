using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DOAN.Models
{

    [MetadataTypeAttribute(typeof(NhapHangMetadata))]
    public partial class NHAPHANG
    {
        internal sealed class NhapHangMetadata
        {
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
            public DateTime NgayNhap { get; set; }
        }
    }
}