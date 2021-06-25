using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CarRental.Models;
using Model.Common;
using BAL;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.Data;
using ExcelDataReader;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace CarRental.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IWebHostEnvironment _hostingEnvironment;
        private ServerResponse ServerResopnse;
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            this._hostingEnvironment = environment;
            this.ServerResopnse = new ServerResponse();
        }

        public IActionResult Index()
        {
             ServerResopnse = new CAR_BAL().Get_CarType();
            if(ServerResopnse.Status==Response_Status.Success)
            {
                ViewBag.Car_Types = new SelectList((IEnumerable<CAR_Type>)ServerResopnse.Data,"ID", "Title");
            }
            return View();
        }
        public IActionResult CarList()
        {
            ServerResopnse = new CAR_BAL().Get_Brand();
            if (ServerResopnse.Status == Response_Status.Success)
            {
                ViewBag.Brand = new SelectList((IEnumerable<string>)ServerResopnse.Data);
            } 
            return View();
        }

        public IActionResult AddCar(int id=0)
        {
            ServerResopnse = new CAR_BAL().Get_CarType();
            ViewBag.Car_Types = new SelectList((IEnumerable<CAR_Type>)ServerResopnse.Data, "ID", "Title");
            CarViewEtity model = new CarViewEtity();
            if(id!=0)
            {
                //edit part
            }
            else
            {
                //new part

            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddCar(CarViewEtity model)
        {
            string filePath = string.Empty;
            string uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            if (model.Photo!=null && model.Photo.Length > 0)
            {
                filePath = Path.Combine(uploads, (model.Photo.FileName));
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Photo.CopyToAsync(fileStream);
                }
            }
            #region Add in DB
            Car dbmodel = new Car
            {
                ID = model.Id,
                BRAND = model.Brand,
                Photo = string.Concat("uploads/", model.Photo.FileName),
                Price = model.price,
                Type = new CAR_Type
                {
                    ID = model.TypeId
                }
            };
            ServerResopnse = new CAR_BAL().AddCAR_UPDATE(dbmodel,"INSERT");
            #endregion

            ServerResopnse = new CAR_BAL().Get_CarType();
            ViewBag.Car_Types = new SelectList((IEnumerable<CAR_Type>)ServerResopnse.Data, "ID", "Title");
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult UploadCARBulk(IFormFile uploadfile)
        {
            try
            {
                if (uploadfile != null && uploadfile.Length > 0)
                {
                    IExcelDataReader reader = null;
                    Stream stream = uploadfile.OpenReadStream();
                    if (uploadfile.FileName.EndsWith(".xls"))
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream, null);
                    }
                    else if (uploadfile.FileName.EndsWith(".xlsx"))
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    double Price;
                    DataSet ds = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });
                    reader.Close();
                    IEnumerable<Car> list = ds.Tables[0].AsEnumerable().Select(dr => new Car
                    {
                        BRAND = dr["BRAND"].ToString(),
                        Price = Double.TryParse(dr["Price"].ToString(), out Price) ? Price : 0,
                        Type = new CAR_Type
                        {
                            Title = dr["Type"].ToString()
                        }
                    }).ToList();

                    #region Call BAL
                    ServerResopnse = new CAR_BAL().BulkUpload(list);
                    #endregion
                }
            }
            catch(Exception ex)
            {
                ServerResopnse.Status =Response_Status.Error;
                ServerResopnse.Msg = ex.Message;
                ServerResopnse.Data = "";
            }
            
            return Json(new { data = ServerResopnse });
        }

    }
}
