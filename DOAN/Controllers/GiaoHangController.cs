using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOAN.Models;

namespace DOAN.Controllers
{
    [Authorize(Roles = "vanchuyen")]
    public class GiaoHangController : Controller
    {
        TMDTDbContext db = new TMDTDbContext();
        // GET: GiaoHang
        public ActionResult Index(int error=0)
        {
            var list = db.HOADONs.Where(x => (x.TinhTrang > 6 && x.TinhTrang <= 9) || (x.TinhTrang == 11)||(x.TinhTrang==5)).OrderByDescending(y => y.NgayDH);
            var listTT = db.TINHTRANGs.Where(x => (x.IdTT > 6 && x.IdTT <= 9) || (x.IdTT == 11) || (x.IdTT == 5));
            ViewBag.items = new SelectList(listTT, "IdTT", "TenTT");
            ViewBag.GiaTri = 0;
            ViewBag.DanhSach = list;

            ViewBag.Error = error;

            return View(list);
        }

        [HttpPost]
        public ActionResult Index(FormCollection f)
        {
            var kq = f["ddlTinhTrang"];
            var listTT = db.TINHTRANGs.Where(x => (x.IdTT > 6 && x.IdTT <= 9) || (x.IdTT == 11)||(x.IdTT==5));

            if (kq != "")
            {
                int giatri = int.Parse(kq);
                var list = db.HOADONs.Where(x => x.TinhTrang == giatri).OrderBy(y => y.NgayDH);
                ViewBag.DanhSach = list;
                ViewBag.items = new SelectList(listTT, "IdTT", "TenTT", giatri);
                ViewBag.GiaTri = giatri;
                return View(list);
            }
            else
            {
                var list = db.HOADONs.Where(x => (x.TinhTrang > 6 && x.TinhTrang <= 9) || (x.TinhTrang == 11) || (x.TinhTrang == 5)).OrderByDescending(y => y.NgayDH);
                ViewBag.DanhSach = list;
                ViewBag.items = new SelectList(listTT, "IdTT", "TenTT");
                ViewBag.GiaTri = 0;
                return View(list);
            }
        }

        public ActionResult ThayDoiTinhTrang(int id)
        {
            var hoadon = db.HOADONs.Find(id);
            if (hoadon == null)
                return HttpNotFound();
            List<TINHTRANG> list=new List<TINHTRANG>();
            if (hoadon.TinhTrang == 5)
                list = db.TINHTRANGs.Where(x => (x.IdTT > 6 && x.IdTT!=10)||(x.IdTT==5)).ToList();
            else if (hoadon.TinhTrang == 9)
                list = db.TINHTRANGs.Where(x => x.IdTT == 9).ToList();
            else if (hoadon.TinhTrang == 11)
                list = db.TINHTRANGs.Where(x => x.IdTT == 11).ToList();
            else if (hoadon.TinhTrang==7||hoadon.TinhTrang==8)
                list = db.TINHTRANGs.Where(x => x.IdTT >=hoadon.TinhTrang && x.IdTT != 10).ToList();

            ViewBag.TinhTrang = new SelectList(list, "IdTT", "TenTT", hoadon.TinhTrang);
            ViewBag.HoaDon = hoadon;
            return View();
        }

        [HttpPost]
        public ActionResult ThayDoiTinhTrang(FormCollection f, int id)
        {
            var kq = f["ddlTinhTrang"];
            var hoadon = db.HOADONs.Find(id);
            if (hoadon == null)
                return HttpNotFound();

            if (kq!="")
            {
                int giatri = int.Parse(kq);
                hoadon.TinhTrang = giatri;
                if(hoadon.TinhTrang==9)
                {
                    hoadon.DaThanhToan = true;
                    hoadon.NgayGH = DateTime.Now;
                }    
                try
                {
                    db.Entry(hoadon).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "GiaoHang", new { error=-1 });
                }
                catch (Exception e)
                {

                }
            }   
            List<TINHTRANG> list = new List<TINHTRANG>();
            if (hoadon.TinhTrang == 5)
                list = db.TINHTRANGs.Where(x => x.IdTT > 6 || x.IdTT != 10 || x.IdTT == 5).ToList();
            else if (hoadon.TinhTrang == 9)
                list = db.TINHTRANGs.Where(x => x.IdTT == 9).ToList();
            else if (hoadon.TinhTrang == 11)
                list = db.TINHTRANGs.Where(x => x.IdTT == 11).ToList();
            else if (hoadon.TinhTrang == 7 || hoadon.TinhTrang == 8)
                list = db.TINHTRANGs.Where(x => x.IdTT >= hoadon.TinhTrang).ToList();


            ViewBag.TinhTrang = new SelectList(list, "IdTT", "TenTT", hoadon.TinhTrang);
            ViewBag.HoaDon = hoadon;
            return View();
        }
    }
}