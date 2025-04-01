using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOAN.Models;
using DOAN.Common;
using System.Net;
using System.Data.Entity;
using DOAN.MTK.SimpleFactory;
namespace DOAN.Controllers
{
    public class QuanLyLoaiSPController : Controller
    {
        TMDTDbContext db = new TMDTDbContext();

        // GET: QuanLyLoaiSP
        public ActionResult Index()
        {
            var model = db.LOAISANPHAMs.Where(x => x.TinhTrang == true);
            ViewBag.DanhMuc = DanhMucFactory.CreateDanhMucList();
            return View(model);
        }

        public ActionResult Create()
        {
            ViewBag.DanhMuc = new SelectList(DanhMucFactory.CreateDanhMucList(), "IdDM", "TenDM");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public ActionResult Create(LOAISANPHAM loaiSP)
        {
            loaiSP.TinhTrang = true;

            if (ModelState.IsValid)
            {
                try
                {
                    db.LOAISANPHAMs.Add(loaiSP);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Loại sản phẩm không thể tạo");
                }
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng kiểm tra lại thông tin đã nhập");
            }
            ViewBag.DanhMuc = new SelectList(DanhMucFactory.CreateDanhMucList(), "IdDM", "TenDM", loaiSP.DanhMuc);
            return View(loaiSP);
        }

        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOAISANPHAM loaiSP = db.LOAISANPHAMs.Find(id);
            if (loaiSP == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            try
            {
                // Cập nhật TinhTrang của loại sản phẩm
                loaiSP.TinhTrang = false;
                db.Entry(loaiSP).State = EntityState.Modified;

                // Cập nhật TinhTrang của các sản phẩm liên quan
                var sanPhams = db.SANPHAMs.Where(sp => sp.IdLoaiSP == id).ToList();
                foreach (var sp in sanPhams)
                {
                    sp.TinhTrang = 0; // Hoặc giá trị tương ứng để đánh dấu sản phẩm bị xóa
                    db.Entry(sp).State = EntityState.Modified;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return Content("<script> alert(\"Quá trình thực hiện thất bại\")</script>");
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOAISANPHAM loaiSP = db.LOAISANPHAMs.FirstOrDefault(x => x.IdLoaiSP == id);
            if (loaiSP == null)
            {
                return HttpNotFound();
            }
            ViewBag.DanhMuc = new SelectList(DanhMucFactory.CreateDanhMucList(), "IdDM", "TenDM", loaiSP.DanhMuc);
            return View(loaiSP);
        }

        [HttpPost]
        [Route("Edit")]
        [ValidateInput(false)]
        public ActionResult Edit(LOAISANPHAM loaiSP)
        {
            loaiSP.TinhTrang = true;
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(loaiSP).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Loại sản phẩm không thể tạo");
                }
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng kiểm tra lại thông tin đã nhập");
            }
            ViewBag.DanhMuc = new SelectList(DanhMucFactory.CreateDanhMucList(), "IdDM", "TenDM", loaiSP.DanhMuc);
            return View(loaiSP);
        }
    }
}