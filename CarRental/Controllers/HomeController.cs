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
             ServerResopnse = new CAR_BAL().Get_Brand();
            if(ServerResopnse.Status==Response_Status.Success)
            {
                ViewBag.Brand = new SelectList((IEnumerable<DropdownResult>)ServerResopnse.Data, "Key", "Value");
            }
            return View();
        }
        public IActionResult CarList()
        {
            ServerResopnse = new CAR_BAL().Get_Brand();
            CarViewModel model = new CarViewModel();
            if (ServerResopnse.Status == Response_Status.Success)
            {
                ViewBag.Brand = new SelectList((IEnumerable<DropdownResult>)ServerResopnse.Data,"Value","Value");
            }
            ServerResopnse = new CAR_BAL().Get_Car_List(new Car());
            if(ServerResopnse.Status==Response_Status.Success)
            {
               List<Car> carlist= (List<Car>)ServerResopnse.Data;
                model.Cars = carlist;
            }
            return View(model);
        }

        public IActionResult AddCar(int id=0)
        {
            ServerResopnse = new CAR_BAL().Get_CarType();
            ViewBag.Car_Types = new SelectList((IEnumerable<CAR_Type>)ServerResopnse.Data, "ID", "Title");
            CarViewEtity model = new CarViewEtity();
            if(id!=0)
            {
                //edit part
                model.Id = id;
                Car dbreq = new Car
                {
                    BRAND = "",
                    ID = id
                };
                ServerResopnse = new CAR_BAL().Get_Car_List(dbreq);
                List<Car> carlist = (List<Car>)ServerResopnse.Data;
                Car car = carlist.FirstOrDefault();
                model.Brand = car.BRAND;
                model.Id = car.ID;
                model.PhotoFile = car.Photo;
                model.price = car.Price;
                model.TypeId = car.Type.ID;
            }
            else
            {
                //new part

            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddCar(CarViewEtity model,string FileChanged)
        {
            Car dbmodel = new Car
            {
                ID = model.Id,
                BRAND = model.Brand,

                Price = model.price,
                Type = new CAR_Type
                {
                    ID = model.TypeId
                }
            };
            FileChanged = FileChanged ?? "false";
            if (model.Id!=0)
            {
                if(!string.Equals(FileChanged,"true",StringComparison.OrdinalIgnoreCase))
                {
                    dbmodel.Photo = model.PhotoFile;
                }
            }
            string filePath = string.Empty;
            string uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            if (model.Photo!=null && model.Photo.Length > 0)
            {
                filePath = Path.Combine(uploads, (model.Photo.FileName));
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Photo.CopyToAsync(fileStream);
                }
                dbmodel.Photo = string.Concat("uploads/", model.Photo.FileName);
            }
            if(model.Id!=0)
            {
                #region Update in DB
                ServerResopnse = new CAR_BAL().AddCAR_UPDATE(dbmodel, "UPDATE");
                #endregion
            }
            else
            {
                #region Add in DB
                ServerResopnse = new CAR_BAL().AddCAR_UPDATE(dbmodel, "INSERT");
                #endregion
            }


            if (ServerResopnse.Status == Response_Status.Success)
            {
                if (model.Id != 0)
                {
                    //edit case
                    TempData["Stats"] = "Success";
                    ViewBag.msg = "Record Updated Successfully";
                }
                else
                {
                    //new add
                    TempData["Stats"] = "Success";
                    ViewBag.msg = "Record Added Successfully";
                }
            }
            else
            {
                TempData["Stats"] = "Error";
                ViewBag.msg = "Something Wend Wrong";
            }
            
            ServerResopnse = new CAR_BAL().Get_CarType();
            ViewBag.Car_Types = new SelectList((IEnumerable<CAR_Type>)ServerResopnse.Data, "ID", "Title");
            return View(model);
        }

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
        public IActionResult Get_Car_List_Search(string Brand_Search, int Id)
        {
            Car dbreq = new Car
            {
                BRAND= Brand_Search,
                ID=Id
            };
            ServerResopnse = new CAR_BAL().Get_Car_List(dbreq);
            List<Car> CarList = (List<Car>)ServerResopnse.Data;
            return PartialView("_CarList", CarList);
        }

        [HttpPost]
        public IActionResult DeleteCAR(int Id)
        {
            ServerResopnse = new CAR_BAL().DeleteCAR(Id);
            return Json(new { data = ServerResopnse });
        }

        [HttpPost]
        public IActionResult AddOrder(Order model)
        {
            Order_Entity db_req = new Order_Entity
            {
                Car = new Car
                {
                    ID = Convert.ToInt32(model.CarID)
                },
                ContactNo = model.Contact_No,
                CONTACT_PERSON = model.Contact_Person,
                DROP_LOCATION = model.Drop_Location,
                ENDDATE = model.End_Date,
                PICK_LOCATION = model.Pick_Location,
                STARTDATE = model.Start_Date
            };
            ServerResopnse = new CAR_BAL().AddOrder(db_req);
            return Json(new {data=ServerResopnse});
        }

        public IActionResult _OrderList(OrderSearch model)
        {
            
            return PartialView("_OrderList",model);
        }
        [HttpPost]
        public IActionResult Get_Order_List_Search(OrderSearch model)
        {
            return ViewComponent("OrderList",model);
        }
    }
}
