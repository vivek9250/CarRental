using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Common
{
    public class CAR_Type
    {
        public int ID { get; set; }
        public string Title { get; set; }
    }
    public class Car
    {
        public Car()
        {
            this.Type = new CAR_Type();
        }
        public int ID { get; set; }
        public CAR_Type Type { get; set; }
        public double Price { get; set; }
        public string BRAND { get; set; }
        public string Photo { get; set; }
    }
    public class Order_Entity
    {
        public Order_Entity()
        {
            this.Car = new Car();
        }
        public int ID { get; set; }
        public string STARTDATE{get;set;}
        public string ENDDATE { get; set; }
        public string PICK_LOCATION { get; set; }
        public string DROP_LOCATION { get; set; }
        public string ContactNo { get; set; }
        public string CONTACT_PERSON { get; set; }
        public Car Car { get; set; }
    }
    public class OrderSearch_entity
    {
        public string FromDate { get; set; }
        public string EndDate { get; set; }
        public string CarID { get; set; }
    }
}
