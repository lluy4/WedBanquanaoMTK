using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOAN.Models;

namespace DOAN.Controllers
{
    [Authorize(Roles = "*,duyetdon")]
    public class QLHoaDonController : Controller
    {
        TMDTDbContext db = new TMDTDbContext();
        // GET: QLHoaDon
        public ActionResult Index(int? giatri,int error=0)
        {
            IEnumerable<HOADON> list = new List<HOADON>();
            if(giatri==null)
            {
                list = db.HOADONs.Where(x => x.TinhTrang == 4).OrderByDescending(y => y.NgayDH);
                var listTT = db.TINHTRANGs.Where(x => (x.IdTT >= 4 && x.IdTT <= 9) || (x.IdTT == 11));
                ViewBag.items = new SelectList(listTT, "IdTT", "TenTT", 4);
                ViewBag.GiaTri = 4;
            }    
            else
            {
                list = db.HOADONs.Where(x => x.TinhTrang == giatri).OrderByDescending(y => y.NgayDH);
                var listTT = db.TINHTRANGs.Where(x => (x.IdTT >= 4 && x.IdTT <= 9) || (x.IdTT == 11));
                ViewBag.items = new SelectList(listTT, "IdTT", "TenTT", giatri);
                ViewBag.GiaTri = giatri;
            }    
            ViewBag.DanhSach = list;

            ViewBag.Error = error;

            return View(list);
        }

        [HttpPost]
        public ActionResult Index(FormCollection f)
        {
            var kq = f["ddlTinhTrang"];
            var listTT = db.TINHTRANGs.Where(x => (x.IdTT >= 4 && x.IdTT <= 9) || (x.IdTT == 11));

            if (kq != "")
            {
                int giatri = int.Parse(kq);
                var list = db.HOADONs.Where(x => x.TinhTrang ==giatri).OrderBy(y => y.NgayDH);
                ViewBag.DanhSach = list;
                ViewBag.items = new SelectList(listTT, "IdTT", "TenTT",giatri);
                ViewBag.GiaTri = giatri;
                return View(list);
            }
            else
            {
                var list = db.HOADONs.Where(x => (x.TinhTrang >= 4 && x.TinhTrang <= 9) || (x.TinhTrang == 11)).OrderByDescending(y => y.NgayDH);
                ViewBag.DanhSach = list;
                ViewBag.items = new SelectList(listTT, "IdTT", "TenTT");
                ViewBag.GiaTri = 0;
                return View(list);
            }
        }

        public ActionResult ChiTiet(int id)
        {
            var hoadon = db.HOADONs.Find(id);
            if (hoadon == null)
                return HttpNotFound();
            var listCT = db.CHITIETHDs.Where(x => x.IdHD == id);

            ViewBag.DoiTacGH = db.DTGIAOHANGs.SingleOrDefault(x=>x.IdDTGH==hoadon.IdDTGH);
            ViewBag.HoaDon = hoadon;
            ViewBag.ChiTiet = listCT;
            return View();
        }

        public ActionResult XacNhan(int id)
        {
            var user = Session["TaiKhoan"] as NGUOIDUNG;
            if (user == null)
                return RedirectToAction("DangNhap", "Home");

            var hoadon = db.HOADONs.Find(id);
            if (hoadon == null)
                return HttpNotFound();
            try
            {
                hoadon.IdNV = user.IdUser;
                hoadon.TinhTrang = 5;
                db.Entry(hoadon).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "QLHoaDon", new { error = -1 });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "QLHoaDon", new { error = 1 });
            }
        }

        public ActionResult HuyBo(int id)
        {
            var user = Session["TaiKhoan"] as NGUOIDUNG;
            if (user == null)
                return RedirectToAction("DangNhap", "Home");

            var hoadon = db.HOADONs.Find(id);
            if (hoadon == null)
                return HttpNotFound();
            try
            {
                hoadon.IdNV = user.IdUser;
                hoadon.TinhTrang = 6;
                db.Entry(hoadon).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "QLHoaDon", new { error = -1 });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "QLHoaDon", new { error = 1 });
            }
        }
    }
}