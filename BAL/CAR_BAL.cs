using Model.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DAL;
using System.Data;

namespace BAL
{
    public class CAR_BAL
    {
        private ServerResponse ServerResponse;
        private DataSet ds;
        public CAR_BAL()
        {
            this.ServerResponse = new ServerResponse();
        }
        public ServerResponse Get_CarType()
        {
            ServerResponse = new CAR_DAL().Get_CarType();
            if(ServerResponse.Status==Response_Status.Success)
            {
                ds = (DataSet)ServerResponse.Data;
                IEnumerable<CAR_Type> carTypes = ds.Tables[0].AsEnumerable().Select(dr => new CAR_Type
                {
                    ID = Convert.ToInt32(dr["ID"].ToString()),
                    Title = dr["Title"].ToString()
                }).ToList();
                ServerResponse.Data = carTypes;

            }
            return ServerResponse;
        }
        public ServerResponse Get_Brand()
        {
            ServerResponse = new CAR_DAL().Get_Brand();
            if (ServerResponse.Status == Response_Status.Success)
            {
                ds = (DataSet)ServerResponse.Data;
                IEnumerable<DropdownResult> carTypes = ds.Tables[0].AsEnumerable().Select(dr =>new DropdownResult
                {
                    Key=dr["ID"].ToString(),
                    Value=dr["BRAND"].ToString()
                }
                ).ToList();
                ServerResponse.Data = carTypes;

            }
            return ServerResponse;
        }
        public ServerResponse BulkUpload(IEnumerable<Car> list)
        {
            try
            {
                ServerResponse = new CAR_DAL().BulkUpload(list);
            }
            catch(Exception ex)
            {
                ServerResponse.Status = Response_Status.Error;
                ServerResponse.Msg = ex.Message;
                ServerResponse.Data = "";

            }
            return ServerResponse;
        }

        public ServerResponse AddCAR_UPDATE(Car model, string ActionType)
        {
            try
            {
                ServerResponse = new CAR_DAL().AddCAR_UPDATE(model,ActionType);
                if(ServerResponse.Status==Response_Status.Success)
                {
                    ServerResponse.Data = "";
                }
            }
            catch(Exception ex)
            {
                ServerResponse.Status = Response_Status.Error;
                ServerResponse.Msg = ex.Message;
                ServerResponse.Data = "";
            }
            return ServerResponse;
        }

        public ServerResponse Get_Car_List(Car model)
        {
            try
            {
                ServerResponse = new CAR_DAL().Get_Car_List(model);
                if(ServerResponse.Status==Response_Status.Success)
                {
                    ds = (DataSet)ServerResponse.Data;
                    List<Car> list = ds.Tables[0].AsEnumerable().Select(dr => new Car {
                        BRAND=dr["Brand"].ToString(),
                        ID=Convert.ToInt32(dr["ID"].ToString()),
                        Photo=dr["Photo"].ToString(),
                        Price=Convert.ToDouble(dr["Price"].ToString()),
                        Type=new CAR_Type
                        {
                            ID=Convert.ToInt32(dr["TypeID"].ToString()),
                            Title=dr["Title"].ToString()
                        }
                    }).ToList();
                    ServerResponse.Data = list;
                }
            }
            catch(Exception ex)
            {
                ServerResponse.Status = Response_Status.Error;
                ServerResponse.Msg = ex.Message;
                ServerResponse.Data = "";
            }
            return ServerResponse;
        }
        public ServerResponse DeleteCAR(int Id)
        {
            try
            {
                ServerResponse = new CAR_DAL().DeleteCAR(Id);
                if(ServerResponse.Status==Response_Status.Success)
                {
                    ServerResponse.Data = "";
                }
                else
                {
                    ServerResponse.Data = "";
                }
            }
            catch(Exception ex)
            {
                ServerResponse.Status = Response_Status.Error;
                ServerResponse.Msg = ex.Message;
                ServerResponse.Data = "";
            }
            return ServerResponse;
        }

        public ServerResponse AddOrder(Order_Entity model)
        {
            try
            {
                ServerResponse = new CAR_DAL().AddOrder(model);
                if (ServerResponse.Status == Response_Status.Success)
                {
                    if(string.IsNullOrWhiteSpace(ServerResponse.Msg))
                    {
                        ServerResponse.Data = "";
                    }
                    else
                    {
                        ServerResponse.Status = Response_Status.Error;
                        ServerResponse.Data = "";
                    }
                    
                }
            }
            catch(Exception ex)
            {
                ServerResponse.Status = Response_Status.Error;
                ServerResponse.Msg = ex.Message;
                ServerResponse.Data = "";
            }
            return ServerResponse;
        }

        public ServerResponse OrderList(OrderSearch_entity model)
        {
            try
            {
                ServerResponse = new CAR_DAL().OrderList(model);
                if(ServerResponse.Status==Response_Status.Success)
                {
                    ds = (DataSet)ServerResponse.Data;
                    List<Order_Entity> list = ds.Tables[0].AsEnumerable().Select(dr => new Order_Entity
                    {
                        Car=new Car
                        {
                            BRAND=dr["BRAND"].ToString()
                        },
                        ID=Convert.ToInt32(dr["OrderID"].ToString()),
                        ContactNo=dr["ContactNo"].ToString(),
                        CONTACT_PERSON= dr["CONTACT_PERSON"].ToString(),
                        DROP_LOCATION= dr["DROP_LOCATION"].ToString(),
                        ENDDATE= dr["ENDDATE"].ToString(),
                        PICK_LOCATION= dr["PICK_LOCATION"].ToString(),
                        STARTDATE= dr["STARTDATE"].ToString()
                    }).ToList();
                    ServerResponse.Data = list;
                }
            }
            catch(Exception ex)
            {
                ServerResponse.Status = Response_Status.Error;
                ServerResponse.Msg = ex.Message;
                ServerResponse.Data = "";
            }
            return ServerResponse;
        }

    }
}
