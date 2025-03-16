using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOAN.Models;
using PagedList;



namespace DOAN.Controllers
{
    public class SanPhamController : Controller
    {
        TMDTDbContext db = new TMDTDbContext();
        // GET: SanPham
        //Trang chi tiết sản phẩm
        public ActionResult Index(int ?id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(x => x.IdSP == id && x.TinhTrang==1);
            if(sp==null)
            {
                return HttpNotFound();
            }    
            return View(sp);
        }

      
        public ActionResult SanPhamTheoThuongHieu(int? page, int ?idTH, int idLoai=0)
        {
            ViewBag.IdTH = idTH;
            ViewBag.IdLoai = idLoai;
            if (idTH == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            IEnumerable<SANPHAM> listSP;
            if (idLoai == 0)
            {
                listSP = db.SANPHAMs.Where(x => x.IdTH == idTH && x.TinhTrang==1);
            }
            else
            {
                listSP = db.SANPHAMs.Where(x => x.IdLoaiSP == idLoai && x.IdTH == idTH && x.TinhTrang==1);
            }
            ViewBag.Loai = null;
            if(idLoai!=0)
            {
                ViewBag.Loai= db.LOAISANPHAMs.SingleOrDefault(x => x.IdLoaiSP == idLoai && x.TinhTrang==true);
            }    
            ViewBag.TH = db.THUONGHIEUx.SingleOrDefault(x => x.IdTH == idTH && x.TinhTrang==true);


            //So san pham tren 1 trang
            int PageSize = 9;
            //So trang hien tai
            int PageNumber = (page ?? 1);
            ViewBag.IdTH = idTH;
            ViewBag.IdLoai = idLoai;
            return View(listSP.OrderBy(x=>x.IdSP).ToPagedList(PageNumber,PageSize));
        }

      
        public ActionResult SanPhamTheoLoai(int? page, int? idLoai,int idTH=0)
        {
            ViewBag.IdTH = idTH;
            ViewBag.IdLoai = idLoai;
            if (idLoai == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            IEnumerable<SANPHAM> listSP;
            if (idTH==0)
            {
                listSP = db.SANPHAMs.Where(x => x.IdLoaiSP == idLoai && x.TinhTrang==1);
            }
            else
            {
                listSP = db.SANPHAMs.Where(x => x.IdLoaiSP == idLoai&& x.IdTH==idTH && x.TinhTrang==1);
            }
            ViewBag.TH = null;
            if(idTH!=0)
            {
                ViewBag.TH = db.THUONGHIEUx.SingleOrDefault(x => x.IdTH == idTH && x.TinhTrang==true);
            }    
            
            ViewBag.Loai = db.LOAISANPHAMs.SingleOrDefault(x => x.IdLoaiSP == idLoai && x.TinhTrang==true);
            //So san pham tren 1 trang
            int PageSize = 9;
            //So trang hien tai
            int PageNumber = (page ?? 1);
            ViewBag.IdTH = idTH;
            ViewBag.IdLoai = idLoai;
            return View(listSP.OrderBy(x => x.IdSP).ToPagedList(PageNumber, PageSize));
        }


        public ActionResult SanPhamTheoDanhMuc(int? page, int? idDM, int idTH = 0)
        {
            ViewBag.IdTH = idTH;
            ViewBag.IdDM = idDM;
            if (idDM == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            IEnumerable<SANPHAM> listSP;
            if (idTH == 0)
            {
                listSP = db.SANPHAMs.Where(x => x.LOAISANPHAM.DanhMuc==idDM && x.TinhTrang == 1);
            }
            else
            {
                listSP = db.SANPHAMs.Where(x => x.LOAISANPHAM.DanhMuc == idDM && x.IdTH == idTH && x.TinhTrang == 1);
            }
            ViewBag.TH = null;
            if (idTH != 0)
            {
                ViewBag.TH = db.THUONGHIEUx.SingleOrDefault(x => x.IdTH == idTH && x.TinhTrang == true);
            }

           
            //So san pham tren 1 trang
            int PageSize = 9;
            //So trang hien tai
            int PageNumber = (page ?? 1);
            ViewBag.IdTH = idTH;
            ViewBag.IdDM = idDM;
            return View(listSP.OrderBy(x => x.IdSP).ToPagedList(PageNumber, PageSize));
        }

        [ChildActionOnly]
        public ActionResult BrandsPartial(LOAISANPHAM loai)
        {
            ViewBag.Loai = loai;
            var listTH = db.THUONGHIEUx.Where(x=>x.TinhTrang==true);
            return PartialView(listTH);
        }

        [ChildActionOnly]
        public ActionResult TypePartial(THUONGHIEU thuonghieu)
        {
            ViewBag.TH = thuonghieu;
            var listLoai = db.LOAISANPHAMs.Where(x=>x.TinhTrang==true);
            return PartialView(listLoai);
        }

        [ChildActionOnly]
        public ActionResult DanhMucSP(IEnumerable<SANPHAM> listSP)
        {
            return PartialView(listSP);
        }

        
    }
}