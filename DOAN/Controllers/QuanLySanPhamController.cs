using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DOAN.Models;
using DOAN.MTK.FactoryMethod;
using DOAN.MTK.Repository;
using DOAN.MTK.Singleton;
using DOAN.MTK.FactoryMethod;

namespace DOAN.Controllers
{
    [Authorize(Roles = "*,nhaphang")]
    public class QuanLySanPhamController : Controller
    {
        private readonly TMDTDbContext _context;
        private readonly ISanPhamRepository _sanPhamRepository;
        private readonly IThuongHieuRepository _thuongHieuRepository;
        private readonly IKhuyenMaiRepository _khuyenMaiRepository;
        private readonly ILoaiSanPhamRepository _loaiSanPhamRepository;
        private readonly INhapHangRepository _nhapHangRepository;

        public QuanLySanPhamController()
        {
            _context = new TMDTDbContext();
            _sanPhamRepository = new SanPhamRepository(_context);
            _thuongHieuRepository = new ThuongHieuRepository(_context);
            _khuyenMaiRepository = new KhuyenMaiRepository(_context);
            _loaiSanPhamRepository = new LoaiSanPhamRepository(_context);
            _nhapHangRepository = new NhapHangRepository(_context);
        }

        // GET: QuanLySanPham
        public ActionResult Index(int error = 0)
        {
            ViewBag.ThuongHieu = null;
            ViewBag.Error = error;

            var list = _sanPhamRepository.GetActiveSanPhams();
            var listTH = _thuongHieuRepository.GetActiveThuongHieus();
            ViewBag.items = new SelectList(listTH, "IdTH", "TenTH");
            ViewBag.GiaTri = 0;
            ViewBag.DanhSach = list;

            return View(list);
        }

        [HttpPost]
        public ActionResult Index(FormCollection f)
        {
            var kq = f["ddlThuongHieu"];
            var listTH = _thuongHieuRepository.GetActiveThuongHieus();

            if (kq != "")
            {
                int giatri = int.Parse(kq);
                ViewBag.ThuongHieu = _thuongHieuRepository.GetById(giatri);
                var list = _sanPhamRepository.GetSanPhamsByThuongHieu(giatri);

                ViewBag.DanhSach = list;
                ViewBag.items = new SelectList(listTH, "IdTH", "TenTH", giatri);
                ViewBag.GiaTri = giatri;
                return View(list);
            }
            else
            {
                ViewBag.ThuongHieu = null;
                var list = _sanPhamRepository.GetActiveSanPhams();
                ViewBag.DanhSach = list;
                ViewBag.items = new SelectList(listTH, "IdTH", "TenTH");
                ViewBag.GiaTri = 0;
                return View(list);
            }
        }

        [Authorize(Roles = "*")]
        public ActionResult Create(int? idTH)
        {
            if (idTH == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THUONGHIEU th = _thuongHieuRepository.GetById(idTH.Value);
            if (th == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            ViewBag.MaKM = new SelectList(_khuyenMaiRepository.GetAllKhuyenMais(), "IdMa", "MaKM");
            ViewBag.IdTH = new SelectList(_thuongHieuRepository.GetActiveThuongHieus(), "IdTH", "TenTH", th.IdTH);
            ViewBag.IdLoaiSP = new SelectList(_loaiSanPhamRepository.GetAllLoaiSanPhams(), "IdLoaiSP", "TenLoai");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        [Authorize(Roles = "*")]
        public ActionResult Create(SANPHAM sp, HttpPostedFileBase[] AnhSP)
        {
            for (int i = 0; i < AnhSP.Length; i++)
            {
                if (AnhSP[i] != null && i == 0 && AnhSP[i].ContentLength > 0)
                {
                    // Kiểm tra sp.IdTH và TenTH
                    string tenth = "default"; // Giá trị mặc định nếu IdTH hoặc TenTH là null
                    if (sp.IdTH.HasValue)
                    {
                        var thuongHieu = _thuongHieuRepository.GetById(sp.IdTH.Value);
                        if (thuongHieu != null && !string.IsNullOrEmpty(thuongHieu.TenTH))
                        {
                            tenth = thuongHieu.TenTH.ToLower();
                        }
                    }
                    var fileName = Path.GetFileName(AnhSP[i].FileName);
                    var path = Path.Combine(Server.MapPath("~/assets/client/hinhsp/" + tenth), fileName);
                    sp.AnhSP = fileName;
                    if (!System.IO.File.Exists(path))
                    {
                        AnhSP[i].SaveAs(path);
                    }
                }
            }

            sp.NgayTao = DateTime.Now;
            sp.SoLanMua = 0;
            sp.SoLuong = 0;
            if (sp.SoLuong > 0)
                sp.TinhTrang = 1;
            else
                sp.TinhTrang = 2;

            if (ModelState.IsValid)
            {
                try
                {
                    _sanPhamRepository.AddSanPham(sp);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Quá trình thực hiện thất bại");
                    ViewBag.MaKM = new SelectList(_khuyenMaiRepository.GetAllKhuyenMais(), "IdMa", "MaKM", sp.MaKM);
                    ViewBag.IdTH = new SelectList(_thuongHieuRepository.GetActiveThuongHieus(), "IdTH", "TenTH", sp.IdTH);
                    ViewBag.IdLoaiSP = new SelectList(_loaiSanPhamRepository.GetAllLoaiSanPhams(), "IdLoaiSP", "TenLoai", sp.IdLoaiSP);
                }
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng kiểm tra lại thông tin đã nhập");
                ViewBag.MaKM = new SelectList(_khuyenMaiRepository.GetAllKhuyenMais(), "IdMa", "MaKM", sp.MaKM);
                ViewBag.IdTH = new SelectList(_thuongHieuRepository.GetActiveThuongHieus(), "IdTH", "TenTH", sp.IdTH);
                ViewBag.IdLoaiSP = new SelectList(_loaiSanPhamRepository.GetAllLoaiSanPhams(), "IdLoaiSP", "TenLoai", sp.IdLoaiSP);
            }
            return View(sp);
        }

        [HttpPost]
        [Authorize(Roles = "*")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM sp = _sanPhamRepository.GetById(id.Value);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            try
            {
                _sanPhamRepository.DeleteSanPham(id.Value);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "QuanLySanPham", new { error = 2 });
            }
        }

        [Authorize(Roles = "*")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM sp = _sanPhamRepository.GetById(id.Value);
            if (sp == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKM = new SelectList(_khuyenMaiRepository.GetAllKhuyenMais(), "IdMa", "MaKM", sp.MaKM);
            ViewBag.IdTH = new SelectList(_thuongHieuRepository.GetActiveThuongHieus(), "IdTH", "TenTH", sp.IdTH);
            ViewBag.IdLoaiSP = new SelectList(_loaiSanPhamRepository.GetAllLoaiSanPhams(), "IdLoaiSP", "TenLoai", sp.IdLoaiSP);
            ViewBag.AnhCu = sp.AnhSP;
            return View(sp);
        }

        [HttpPost]
        [Route("Edit")]
        [ValidateInput(false)]
        [Authorize(Roles = "*")]
        public ActionResult Edit(SANPHAM sp, HttpPostedFileBase[] AnhSP, string AnhCu)
        {
            for (int i = 0; i < AnhSP.Length; i++)
            {
                if (AnhSP[i] != null && i == 0 && AnhSP[i].ContentLength > 0)
                {
                    // Kiểm tra sp.IdTH và TenTH
                    string tenth = "default"; // Giá trị mặc định nếu IdTH hoặc TenTH là null
                    if (sp.IdTH.HasValue)
                    {
                        var thuongHieu = _thuongHieuRepository.GetById(sp.IdTH.Value);
                        if (thuongHieu != null && !string.IsNullOrEmpty(thuongHieu.TenTH))
                        {
                            tenth = thuongHieu.TenTH.ToLower();
                        }
                    }
                    var fileName = Path.GetFileName(AnhSP[i].FileName);
                    var path = Path.Combine(Server.MapPath("~/assets/client/hinhsp/" + tenth), fileName);
                    sp.AnhSP = fileName;
                    if (!System.IO.File.Exists(path))
                    {
                        AnhSP[i].SaveAs(path);
                    }
                }
            }
            if (sp.AnhSP == null || sp.AnhSP == "")
            {
                sp.AnhSP = AnhCu;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _sanPhamRepository.UpdateSanPham(sp);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Quá trình thực hiện thất bại.");
                    ViewBag.MaKM = new SelectList(_khuyenMaiRepository.GetAllKhuyenMais(), "IdMa", "MaKM", sp.MaKM);
                    ViewBag.IdTH = new SelectList(_thuongHieuRepository.GetActiveThuongHieus(), "IdTH", "TenTH", sp.IdTH);
                    ViewBag.IdLoaiSP = new SelectList(_loaiSanPhamRepository.GetAllLoaiSanPhams(), "IdLoaiSP", "TenLoai", sp.IdLoaiSP);
                }
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng kiểm tra lại thông tin đã nhập.");
                ViewBag.MaKM = new SelectList(_khuyenMaiRepository.GetAllKhuyenMais(), "IdMa", "MaKM", sp.MaKM);
                ViewBag.IdTH = new SelectList(_thuongHieuRepository.GetActiveThuongHieus(), "IdTH", "TenTH", sp.IdTH);
                ViewBag.IdLoaiSP = new SelectList(_loaiSanPhamRepository.GetAllLoaiSanPhams(), "IdLoaiSP", "TenLoai", sp.IdLoaiSP);
            }
            return View(sp);
        }

        public ActionResult NhapHang(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM sp = _sanPhamRepository.GetById(id.Value);
            if (sp == null)
            {
                return HttpNotFound();
            }
            NHAPHANG nh = new NHAPHANG
            {
                IdSP = sp.IdSP,
                NgayNhap = DateTime.Now,
                GiaNhap = sp.GiaGoc,
                SoLuong = 0
            };
            ViewBag.TenSP = sp.TenSP;
            return View(nh);
        }

        [HttpPost]
        public ActionResult NhapHang(NHAPHANG nh)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (_nhapHangRepository.ExistsNhapHang(nh.IdSP, nh.NgayNhap))
                    {
                        return RedirectToAction("Index", "QuanLySanPham", new { error = 1 });
                    }
                    _nhapHangRepository.AddNhapHang(nh);
                    return RedirectToAction("Index", "QuanLySanPham", new { error = -1 });
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
            return View(nh);
        }
        //SINGLETON VŨ
        public class QLHoaDonController : Controller
        {
            public ActionResult Index()
            {
                var dbContext = DatabaseContext.GetInstance();
                dbContext.ExecuteQuery("SELECT * FROM HOADON");
                return View();
            }
        }
        //FactoryMethod VŨ
        public class QuanlySanPhamController : Controller
        {
            public ActionResult TaoSanPham(string loaiSanPham, string tenSanPham)
            {
                QuanAoFactory factory = null;

                // Chọn factory dựa trên loại sản phẩm
                switch (loaiSanPham)
                {
                    case "AoThun":
                        factory = new AoThunFactory();
                        break;
                    case "QuanJean":
                        factory = new QuanJeanFactory();
                        break;
                    case "Vay":
                        factory = new VayFactory();
                        break;
                    default:
                        throw new ArgumentException("Loại sản phẩm không hợp lệ.");
                }

                // Tạo sản phẩm và hiển thị thông tin
                if (factory != null)
                {
                    var sanPham = factory.TaoQuanAo(tenSanPham);
                    sanPham.HienThiThongTin();
                }

                return View();
            }
        }



    }

}