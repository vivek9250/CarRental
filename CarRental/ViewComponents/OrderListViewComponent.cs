using BAL;
using CarRental.Models;
using Microsoft.AspNetCore.Mvc;
using Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.ViewComponents
{
    public class OrderListViewComponent:ViewComponent
    {
        private ServerResponse ServerResopnse;
        public async Task<IViewComponentResult> InvokeAsync(OrderSearch ? model=null)
        {
            model = model ?? new OrderSearch();
            OrderSearch_entity req = new OrderSearch_entity
            {
                CarID = model.CarID,
                EndDate = model.EndDate,
                FromDate = model.FromDate
            };
            ServerResopnse = new CAR_BAL().OrderList(req);
            List<Order_Entity> dblist = (List<Order_Entity>)ServerResopnse.Data;
            return View(dblist);
        }
    }
}
