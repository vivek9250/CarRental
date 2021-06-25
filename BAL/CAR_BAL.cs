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
                IEnumerable<string> carTypes = ds.Tables[0].AsEnumerable().Select(dr =>dr["BRAND"].ToString()).ToList();
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
    }
}
