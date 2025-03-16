using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOAN.Common;
using DOAN.Models;

namespace DOAN.Controllers
{
    [Authorize(Roles = "*,thongke")]
    public class DashboardController : Controller
    {
        TMDTDbContext db = new TMDTDbContext();
        // GET: Dashboard
        public ActionResult Index()
        {
            var user = Session["TaiKhoan"] as NGUOIDUNG;
            if(user==null)
            {
                return RedirectToAction("DangNhap", "Home");
            }    
            ViewBag.CanXacNhan=db.HOADONs.Where(x => x.TinhTrang == 4).Count();
            ViewBag.DangKyMoi = db.NGUOIDUNGs.Where(x => x.NgayTao.Value.Month == DateTime.Now.Month && x.NgayTao.Value.Year == DateTime.Now.Year && x.IdLoaiUser == 1).Count();
            ViewBag.BiHuy = db.HOADONs.Where(x => x.NgayDH.Value.Month == DateTime.Now.Month && x.NgayDH.Value.Year == DateTime.Now.Year && x.TinhTrang == 6).Count();
            ViewBag.ThatBai=db.HOADONs.Where(x => x.NgayDH.Value.Month == DateTime.Now.Month && x.NgayDH.Value.Year == DateTime.Now.Year && x.TinhTrang == 11).Count();

            int giatri = DateTime.Now.Year;
            

            var nam = db.HOADONs.Select(x => x.NgayDH.Value.Year).Distinct();
            List<Nam> listNam = new List<Nam>();
            foreach(var item in nam)
            {
                Nam n = new Nam();
                n.IdNam = item;
                n.TenNam = "Năm " + item;
                listNam.Add(n);
            }
            ViewBag.items = new SelectList(listNam, "IdNam", "TenNam", giatri);

            List<string> labels = new List<string>();
            List<int> values = new List<int>();
            for (int i=1;i<=12; i++)
            {
                
                int solieu = db.HOADONs.Where(x => x.NgayDH.Value.Month == i && x.NgayDH.Value.Year == DateTime.Now.Year && x.TinhTrang == 9).Count();
                values.Add(solieu);
                labels.Add("Tháng " + i);
            }
            ViewBag.Label = labels;
            ViewBag.Value = values;
            ViewBag.GiaTri = giatri;
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection f)
        {
            ViewBag.CanXacNhan = db.HOADONs.Where(x => x.TinhTrang == 4).Count();
            ViewBag.DangKyMoi = db.NGUOIDUNGs.Where(x => x.NgayTao.Value.Month == DateTime.Now.Month && x.NgayTao.Value.Year == DateTime.Now.Year && x.IdLoaiUser == 1).Count();
            ViewBag.BiHuy = db.HOADONs.Where(x => x.NgayDH.Value.Month == DateTime.Now.Month && x.NgayDH.Value.Year == DateTime.Now.Year && x.TinhTrang == 6).Count();
            ViewBag.ThatBai = db.HOADONs.Where(x => x.NgayDH.Value.Month == DateTime.Now.Month && x.NgayDH.Value.Year == DateTime.Now.Year && x.TinhTrang == 11).Count();

            var kq = f["ddlTinhTrang"];

            var nam = db.HOADONs.Select(x => x.NgayDH.Value.Year).Distinct();
            List<Nam> listNam = new List<Nam>();
            foreach (var item in nam)
            {
                Nam n = new Nam();
                n.IdNam = item;
                n.TenNam = "Năm " + item;
                listNam.Add(n);
            }

            if (kq != "")
            {
                int giatri = int.Parse(kq);

                List<string> labels = new List<string>();
                List<int> values = new List<int>();
                for (int i = 1; i <= 12; i++)
                {

                    int solieu = db.HOADONs.Where(x => x.NgayDH.Value.Month == i && x.NgayDH.Value.Year == giatri && x.TinhTrang == 9).Count();
                    values.Add(solieu);
                    labels.Add("Tháng " + i);
                }
                ViewBag.Label = labels;
                ViewBag.Value = values;
                ViewBag.items = new SelectList(listNam, "IdNam", "TenNam", giatri);

                ViewBag.GiaTri = giatri;
                return View();
            }
            else
            {
                int giatri = DateTime.Now.Year;
                List<string> labels = new List<string>();
                List<int> values = new List<int>();
                for (int i = 1; i <= 12; i++)
                {

                    int solieu = db.HOADONs.Where(x => x.NgayDH.Value.Month == i && x.NgayDH.Value.Year == DateTime.Now.Year && x.TinhTrang == 9).Count();
                    values.Add(solieu);
                    labels.Add("Tháng " + i);
                }
                ViewBag.Label = labels;
                ViewBag.Value = values;
                ViewBag.items = new SelectList(listNam, "IdNam", "TenNam", giatri);
                ViewBag.GiaTri = giatri;
                return View();
            }
        }

        public ActionResult LoiPhanQuyen()
        {
            return View();
        }
    }
}