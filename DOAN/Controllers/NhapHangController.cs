using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DOAN.Models;


namespace DOAN.Controllers
{
    [Authorize(Roles = "*")]
    public class NhapHangController : Controller
    {
        TMDTDbContext db = new TMDTDbContext();
        // GET: NhapHang
        public ActionResult Index()
        {
            var model = db.NHAPHANGs.OrderByDescending(x => x.NgayNhap);
            return View(model);
        }

       

        public ActionResult PhieuNhap(int idTH=0, int error=0,string noidung=null)
        {
            ViewBag.Error = error;
            ViewBag.NoiDung = noidung;
            if(idTH==0)
            {
                ViewBag.SanPham = db.SANPHAMs.Where(x=>x.TinhTrang==1 || x.TinhTrang==2);
                ViewBag.ThuongHieu = null;
            }    
            else
            {
                THUONGHIEU th = db.THUONGHIEUx.Find(idTH);
                if (th == null)
                    return HttpNotFound();
                ViewBag.ThuongHieu = th;
                ViewBag.SanPham = db.SANPHAMs.Where(x => x.IdTH == th.IdTH && (x.TinhTrang == 1 || x.TinhTrang == 2));
            }    
            return View();
        }

        [HttpPost]
        public ActionResult PhieuNhap(IEnumerable<NHAPHANG> Model, FormCollection f)
        {
            string loi = "";
            int error = 0;
            try
            {
                DateTime dt = DateTime.Parse(f["NgayNhap"].ToString());
                foreach (var item in Model)
                {
                    NHAPHANG nh = new NHAPHANG();
                    nh.IdSP = item.IdSP;
                    nh.NgayNhap = dt;
                    nh.SoLuong = item.SoLuong;
                    nh.GiaNhap = item.GiaNhap;
                    if (db.NHAPHANGs.Where(x => x.NgayNhap == nh.NgayNhap && x.IdSP == nh.IdSP).Count() > 0)
                    {
                        error = 1;
                        loi += db.SANPHAMs.Find(nh.IdSP).TenSP + ", ";
                    }
                    else
                    {
                        db.NHAPHANGs.Add(nh);
                        db.SaveChanges();
                    }
                }
                
                if(error==1)
                {
                    string noidung="Sản phẩm "+loi.Substring(0, loi.Length - 2)+" đã được nhập hàng vào ngày hôm nay. Không thể cập nhật lại";
                    return RedirectToAction("PhieuNhap", "NhapHang", new { error = error, noidung = noidung });
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Please check the information you entered.");
                ViewBag.SanPham = db.SANPHAMs;
                return View();
            }
            
        }
    }
}