using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOAN.Models;
using PagedList;
using DOAN.MTK;
using DOAN.MTK.Decorator1;

namespace DOAN.Controllers
{
    public class TimKiemController : Controller
    {
        TMDTDbContext db = new TMDTDbContext();
        // GET: TimKiem

        public ActionResult KQTimKiem(string sTuKhoa, int? page, int? minPrice, int? maxPrice, int? categoryId)
        {
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            int pageSize = 9;
            int pageNumber = (page ?? 1);

            Timkiem search = new BaseSearch();
            search = new Timkiemtheogia(search, minPrice, maxPrice);
            search = new Timkiemtheoloai(search, categoryId);

            var listSP = search.Search(db.SANPHAMs, sTuKhoa);
            ViewBag.TuKhoa = sTuKhoa;
            return View(listSP.OrderBy(n => n.TenSP).ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult TimKiemTuKhoa(string sTuKhoa, int? minPrice, int? maxPrice, int? categoryId)
        {
            return RedirectToAction("KQTimKiem", new { sTuKhoa = sTuKhoa, minPrice = minPrice, maxPrice = maxPrice, categoryId = categoryId });
        }
    }
}