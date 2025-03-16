using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOAN.Models;
using PagedList;

namespace DOAN.Controllers
{
    public class TimKiemController : Controller
    {
        TMDTDbContext db = new TMDTDbContext();
        // GET: TimKiem

       
        public ActionResult KQTimKiem(string sTuKhoa, int ?page)
        {
            if(Request.HttpMethod!="GET")
            {
                page = 1;
            }
            int pageSize = 9;
            int pageNumber = (page ?? 1);
            var listSP = db.SANPHAMs.Where(n => n.TenSP.Contains(sTuKhoa) && n.TinhTrang==1);
            ViewBag.TuKhoa = sTuKhoa;
            return View(listSP.OrderBy(n => n.TenSP).ToPagedList(pageNumber, pageSize));
        }


        [HttpPost]
        public ActionResult TimKiemTuKhoa (string sTuKhoa)
        {
            return RedirectToAction("KQTimKiem", new { sTuKhoa=sTuKhoa});
        }
    }
}