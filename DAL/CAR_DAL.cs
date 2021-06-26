using Model.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DAL
{
    public class CAR_DAL:DBContext
    {
        private DataSet ds = null;
        private ServerResponse ServerResponse { get; set; }
        public CAR_DAL()
        {
            this.ds = new DataSet();
            this.ServerResponse = new ServerResponse();
        }

        public ServerResponse Get_CarType()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = base.GetConnection();
                    cmd.CommandText = "Proc_CarRental";
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Add Parameters to SQL Command
                    cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar)).Value = "Get_Car_Type";
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(ds);
                    sda.Dispose();
                    ServerResponse.Data = ds;
                    ServerResponse.Status = Response_Status.Success;
                }
            }
            catch (Exception ex)
            {
                ServerResponse.Status = Response_Status.Error;
                ServerResponse.Msg = ex.Message;
                ServerResponse.Data = "";

            }
            finally
            {
                CloseConnection();
            }
            return ServerResponse;
        }

        public ServerResponse Get_Brand()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = base.GetConnection();
                    cmd.CommandText = "Proc_CarRental";
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Add Parameters to SQL Command
                    cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar)).Value = "Get_Brand";
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(ds);
                    sda.Dispose();
                    ServerResponse.Data = ds;
                    ServerResponse.Status = Response_Status.Success;
                }
            }
            catch (Exception ex)
            {
                ServerResponse.Status = Response_Status.Error;
                ServerResponse.Msg = ex.Message;
                ServerResponse.Data = "";

            }
            finally
            {
                CloseConnection();
            }
            return ServerResponse;
        }

        public ServerResponse BulkUpload(IEnumerable<Car> list)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("BRAND");
                dt.Columns.Add("Title");
                dt.Columns.Add("Price");
                foreach(var item in list)
                {
                    dt.Rows.Add(item.BRAND, item.Type.Title, item.Price);
                }
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = base.GetConnection();
                    cmd.CommandText = "Proc_CarRental";
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Add Parameters to SQL Command
                    cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar)).Value = "BulkUploadCar";
                    cmd.Parameters.Add(new SqlParameter("@Bulk_Cars", SqlDbType.Structured)).Value =dt;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(ds);
                    sda.Dispose();
                    if(string.Equals(ds.Tables[0].Rows[0][0].ToString(),"SUCCESS",StringComparison.OrdinalIgnoreCase))
                    {
                        ServerResponse.Status = Response_Status.Success;
                    }
                    else
                    {
                        ServerResponse.Status = Response_Status.Error;
                    }
                }
            }
            catch(Exception ex)
            {
                ServerResponse.Status = Response_Status.Error;
                ServerResponse.Msg = ex.Message;
                ServerResponse.Data = "";
            }
            finally
            {
                CloseConnection();
            }
            return ServerResponse;
        }

        public ServerResponse AddCAR_UPDATE(Car model,string ActionType)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = base.GetConnection();
                    cmd.CommandText = "Proc_CarRental";
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Add Parameters to SQL Command
                    cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.VarChar)).Value = "AddCAR_Update";
                    cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = model.ID;
                    cmd.Parameters.Add(new SqlParameter("@TypeID", SqlDbType.VarChar)).Value =model.Type.ID;
                    cmd.Parameters.Add(new SqlParameter("@Price", SqlDbType.VarChar)).Value = model.Price;
                    cmd.Parameters.Add(new SqlParameter("@Brand", SqlDbType.VarChar)).Value = model.BRAND;
                    cmd.Parameters.Add(new SqlParameter("@Photo", SqlDbType.VarChar)).Value = model.Photo; 
                    cmd.Parameters.Add(new SqlParameter("@ActionType", SqlDbType.VarChar)).Value = ActionType;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(ds);
                    sda.Dispose();
                    if (string.Equals(ds.Tables[0].Rows[0][0].ToString(), "SUCCESS", StringComparison.OrdinalIgnoreCase))
                    {
                        ServerResponse.Status = Response_Status.Success;
                    }
                    else
                    {
                        ServerResponse.Status = Response_Status.Error;
                    }
                }
            }
            catch(Exception ex)
            {
                ServerResponse.Status = Response_Status.Error;
                ServerResponse.Msg = ex.Message;
                ServerResponse.Data = "";
            }
            finally
            {
                CloseConnection();
            }
            return ServerResponse;
        }

        public ServerResponse Get_Car_List(Car model)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = base.GetConnection();
                    cmd.CommandText = "Proc_CarRental";
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Add Parameters to SQL Command
                    cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.VarChar)).Value = "Get_CAR_List";
                    cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = model.ID;
                    cmd.Parameters.Add(new SqlParameter("@Brand_Search", SqlDbType.VarChar)).Value = model.BRAND;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(ds);
                    sda.Dispose();
                    ServerResponse.Data = ds;
                    ServerResponse.Status = Response_Status.Success;
                }
            }
            catch(Exception ex)
            {
                ServerResponse.Status = Response_Status.Error;
                ServerResponse.Msg = ex.Message;
                ServerResponse.Data = "";
            }
            finally
            {
                CloseConnection();
            }
            return ServerResponse;
        }
        public ServerResponse DeleteCAR(int Id)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = base.GetConnection();
                    cmd.CommandText = "Proc_CarRental";
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Add Parameters to SQL Command
                    cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.VarChar)).Value = "DeleteCAR";
                    cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = Id;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(ds);
                    sda.Dispose();
                    ServerResponse.Data = ds;
                    if (string.Equals(ds.Tables[0].Rows[0][0].ToString(), "SUCCESS", StringComparison.OrdinalIgnoreCase))
                    {
                        ServerResponse.Status = Response_Status.Success;
                    }
                    else
                    {
                        ServerResponse.Msg = ds.Tables[0].Rows[0]["MSG"].ToString();
                        ServerResponse.Status = Response_Status.Error;
                    }
                }
            }
            catch(Exception ex)
            {
                ServerResponse.Status = Response_Status.Error;
                ServerResponse.Msg = ex.Message;
                ServerResponse.Data = "";
            }
            finally
            {
                CloseConnection();
            }
            return ServerResponse;
        }

        public ServerResponse AddOrder(Order_Entity model)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = base.GetConnection();
                    cmd.CommandText = "Proc_CarRental";
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Add Parameters to SQL Command
                    cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.VarChar)).Value = "AddCAR";
                    cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value =model.Car.ID;
                    cmd.Parameters.Add(new SqlParameter("@STARTDATE", SqlDbType.VarChar)).Value = model.STARTDATE;
                    cmd.Parameters.Add(new SqlParameter("@ENDDATE", SqlDbType.VarChar)).Value = model.ENDDATE;
                    cmd.Parameters.Add(new SqlParameter("@PICK_LOCATION", SqlDbType.VarChar)).Value = model.PICK_LOCATION;
                    cmd.Parameters.Add(new SqlParameter("@DROP_LOCATION", SqlDbType.VarChar)).Value = model.DROP_LOCATION;
                    cmd.Parameters.Add(new SqlParameter("@ContactNo", SqlDbType.VarChar)).Value = model.ContactNo;
                    cmd.Parameters.Add(new SqlParameter("@CONTACT_PERSON", SqlDbType.VarChar)).Value = model.CONTACT_PERSON;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(ds);
                    sda.Dispose();
                    ServerResponse.Data = ds;
                    if (string.Equals(ds.Tables[0].Rows[0][0].ToString(), "SUCCESS", StringComparison.OrdinalIgnoreCase))
                    {
                        ServerResponse.Status = Response_Status.Success;
                        ServerResponse.Msg = ds.Tables[0].Rows[0]["Msg"].ToString();
                    }
                    else
                    {
                        ServerResponse.Status = Response_Status.Error;
                    }
                }
            }
            catch(Exception ex)
            {
                ServerResponse.Status = Response_Status.Error;
                ServerResponse.Msg = ex.Message;
                ServerResponse.Data = "";
            }
            finally
            {
                CloseConnection();
            }
            return ServerResponse;
        }
        public ServerResponse OrderList(OrderSearch_entity model)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = base.GetConnection();
                    cmd.CommandText = "Proc_CarRental";
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Add Parameters to SQL Command
                    cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.VarChar)).Value = "Get_Order_List";
                    cmd.Parameters.Add(new SqlParameter("@Brand_Search", SqlDbType.VarChar)).Value = model.CarID;
                    cmd.Parameters.Add(new SqlParameter("@STARTDATE", SqlDbType.VarChar)).Value = model.FromDate;
                    cmd.Parameters.Add(new SqlParameter("@ENDDATE", SqlDbType.VarChar)).Value = model.EndDate;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(ds);
                    sda.Dispose();
                    ServerResponse.Data = ds;
                    if (string.Equals(ds.Tables[1].Rows[0][0].ToString(), "SUCCESS", StringComparison.OrdinalIgnoreCase))
                    {
                        ServerResponse.Status = Response_Status.Success;
                    }
                    else
                    {
                        ServerResponse.Status = Response_Status.Error;
                    }
                }
            }
            catch(Exception ex)
            {
                ServerResponse.Status = Response_Status.Error;
                ServerResponse.Msg = ex.Message;
                ServerResponse.Data = "";
            }
            finally
            {
                CloseConnection();
            }
            return ServerResponse;
        }
    }
}
