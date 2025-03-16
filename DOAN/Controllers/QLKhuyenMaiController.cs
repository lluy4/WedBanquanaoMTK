using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOAN.Models;
using DOAN.Common;
using System.Net;
using System.Data.Entity;

namespace DOAN.Controllers
{
    [Authorize(Roles = "*")]
    public class QLKhuyenMaiController : Controller
    {
        // GET: QLKhuyenMai
        TMDTDbContext db = new TMDTDbContext();
        public ActionResult Index(int error=0)
        {
            var list = db.KHUYENMAIs.Where(x => x.TinhTrang == true).OrderByDescending(y=>y.NgayBD);
            List<LoaiKM> listLoai = new List<LoaiKM>();
            LoaiKM LOAI1 = new LoaiKM();
            LOAI1.IdLoai = 1;
            LOAI1.TenLoai = "Giảm phần trăm";
            listLoai.Add(LOAI1);
            LoaiKM LOAI2 = new LoaiKM();
            LOAI2.IdLoai = 2;
            LOAI2.TenLoai = "Giảm trực tiếp";
            listLoai.Add(LOAI2);
            ViewBag.items = new SelectList(listLoai, "IdLoai", "TenLoai");
            ViewBag.GiaTri = 0;
            ViewBag.DanhSach = list;

            ViewBag.Error = error;

            
            return View(list);
        }

        [HttpPost]
        public ActionResult Index(FormCollection f)
        {
            var kq = f["ddlLoai"];
            List<LoaiKM> listLoai = new List<LoaiKM>();
            LoaiKM LOAI1 = new LoaiKM();
            LOAI1.IdLoai = 1;
            LOAI1.TenLoai = "Giảm phần trăm";
            listLoai.Add(LOAI1);
            LoaiKM LOAI2 = new LoaiKM();
            LOAI2.IdLoai = 2;
            LOAI2.TenLoai = "Giảm trực tiếp";
            listLoai.Add(LOAI2);

            if (kq != "")
            {
                int giatri = int.Parse(kq);
                var list = db.KHUYENMAIs.Where(x => x.TinhTrang == true && x.LoaiKM==giatri).OrderByDescending(y => y.NgayBD);
                ViewBag.DanhSach = list;
                ViewBag.items = new SelectList(listLoai, "IdLoai", "TenLoai",giatri);
                ViewBag.GiaTri = giatri;
                return View(list);
            }
            else
            {
                var list = db.KHUYENMAIs.Where(x => x.TinhTrang == true).OrderByDescending(y => y.NgayBD);
                ViewBag.DanhSach = list;
                ViewBag.items = new SelectList(listLoai, "IdLoai", "TenLoai");
                ViewBag.GiaTri = 0;
                return View(list);
            }
        }

        public ActionResult Create()
        {
            List<LoaiKM> listLoai = new List<LoaiKM>();
            LoaiKM LOAI1 = new LoaiKM();
            LOAI1.IdLoai = 1;
            LOAI1.TenLoai = "Giảm phần trăm";
            listLoai.Add(LOAI1);
            LoaiKM LOAI2 = new LoaiKM();
            LOAI2.IdLoai = 2;
            LOAI2.TenLoai = "Giảm trực tiếp";
            listLoai.Add(LOAI2);

            ViewBag.LoaiKM = new SelectList(listLoai, "IdLoai", "TenLoai");
            return View();
        }


        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(KHUYENMAI khuyenmai)
        {
            khuyenmai.TinhTrang = true;
            if (ModelState.IsValid)
            {
                try
                {
                    db.KHUYENMAIs.Add(khuyenmai);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Quá trình thực hiện thất bại.");
                }
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng kiểm tra lại thông tin đã nhập.");
            }
            List<LoaiKM> listLoai = new List<LoaiKM>();
            LoaiKM LOAI1 = new LoaiKM();
            LOAI1.IdLoai = 1;
            LOAI1.TenLoai = "Giảm phần trăm";
            listLoai.Add(LOAI1);
            LoaiKM LOAI2 = new LoaiKM();
            LOAI2.IdLoai = 2;
            LOAI2.TenLoai = "Giảm trực tiếp";
            listLoai.Add(LOAI2);

            ViewBag.LoaiKM = new SelectList(listLoai, "IdLoai", "TenLoai",khuyenmai.LoaiKM);
            return View(khuyenmai);
        }

        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHUYENMAI khuyenmai = db.KHUYENMAIs.Find(id);
            if (khuyenmai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            if(DateTime.Compare(DateTime.Now, khuyenmai.NgayBD ?? DateTime.Now) < 0)
            {
                try
                {
                    khuyenmai.TinhTrang = false;
                    db.Entry(khuyenmai).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    string message = ex.Message;
                    return Content("<script> alert(\"Quá trình thực hiện thất bại.\")</script>");
                }
            }    
            else
            {
                return RedirectToAction("Index","QLKhuyenMai",new {error=2});
            }    
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHUYENMAI khuyenmai = db.KHUYENMAIs.SingleOrDefault(x => x.IdMa == id);
            if (khuyenmai == null)
            {
                return HttpNotFound();
            }
            if (DateTime.Compare(DateTime.Now, khuyenmai.NgayBD ?? DateTime.Now) < 0)
            {
                List<LoaiKM> listLoai = new List<LoaiKM>();
                LoaiKM LOAI1 = new LoaiKM();
                LOAI1.IdLoai = 1;
                LOAI1.TenLoai = "Giảm phần trăm";
                listLoai.Add(LOAI1);
                LoaiKM LOAI2 = new LoaiKM();
                LOAI2.IdLoai = 2;
                LOAI2.TenLoai = "Giảm trực tiếp";
                listLoai.Add(LOAI2);

                ViewBag.LoaiKM = new SelectList(listLoai, "IdLoai", "TenLoai", khuyenmai.LoaiKM);
                return View(khuyenmai);
            }    
            else
            {
                return RedirectToAction("Index","QLKhuyenMai", new {error=2});
            }    
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(KHUYENMAI khuyenmai)
        {
            khuyenmai.TinhTrang = true;
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(khuyenmai).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Quá trình thực hiện thất bại.");
                }
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng kiểm tra lại thông tin đã nhập.");
            }

            List<LoaiKM> listLoai = new List<LoaiKM>();
            LoaiKM LOAI1 = new LoaiKM();
            LOAI1.IdLoai = 1;
            LOAI1.TenLoai = "Giảm phần trăm";
            listLoai.Add(LOAI1);
            LoaiKM LOAI2 = new LoaiKM();
            LOAI2.IdLoai = 2;
            LOAI2.TenLoai = "Giảm trực tiếp";
            listLoai.Add(LOAI2);

            ViewBag.LoaiKM = new SelectList(listLoai, "IdLoai", "TenLoai",khuyenmai.LoaiKM);
            return View(khuyenmai);
        }

    }
}