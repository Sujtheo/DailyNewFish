using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopifyW.Models
{
    public class DailyFish
    {
        public IList<specialOfferMj> specialOffer { get; set; }
        public IList<Sitems> specialOfferSingle { get; set; }
        public IList<fishMJ> fish { get; set; }
        public IList<fishMJ> vegetables { get; set; }
        public IList<fishMJ> poultry { get; set; }
        public IList<fishMJ> egg { get; set; }

    }
    public class specialOfferMj
    {
        public int specOfferId { get; set; }
        //public string name { get; set; }
       // public string malayalam { get; set; }
        public string price { get; set; }
       // public string quantity { get; set; }
       // public string img { get; set; }
        public int unit { get; set; }
        public IList<Sitems> OfferItems { get; set; }
    }
    public class Sitems
    {
        public int ItemId { get; set; }
        public int productId { get; set; }
        public string name { get; set; }
        public string malayalam { get; set; }
        public string quantity { get; set; }
        public string img { get; set; }
        public string price { get; set; }
    }
    public class fishMJ
    {
        public int productId { get; set; }
        public string name { get; set; }
        public string malayalam { get; set; }
        public string price { get; set; }
        public int? unit { get; set; }
        public string quantity { get; set; }
        public string img { get; set; }
        public List<priceListModel> priceList { get; set; }
        public bool? isAvailable { get; set; }
    }
    public class priceListModel
    {
        public string quantity;
        public string price;
    }

    public class categoryModel
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }
    public class itemModel
    {
        public int ItemID { get; set; }
        public int CategoryId { get; set; }
        public string ItemName { get; set; }
        public string ItemMalName { get; set; }
        public string ItemImage { get; set; }
    }
    public class AddPricemodel
    {
        public List<ItemPriceArray> itemPrices = new List<ItemPriceArray>();
        
    }
    public class ItemPriceArray
    {
        public int ItemID { get; set; }
        public string Quantity { get; set; }
        public int? Price { get; set; }
    }
    public class itemPriceModel
    {
        public int PriceID { get; set; }
        public int ItemID { get; set; }
        public string Quantity { get; set; }
        public int? Price { get; set; }
    }
    public class unitItemModel
    {
        public int UnitID { get; set; }
        public int ItemID { get; set; }
        public int unit { get; set; }
        public int? prize { get; set; }
        public int? quantity { get; set; }
    }
    public class itemInputModel
    {
        public int ItemID { get; set; }
        public int CategoryId { get; set; }
        public string ItemName { get; set; }
        public string ItemMalName { get; set; }
        public string ItemImage { get; set; }
        public string Quantity { get; set; }
        public int? Price { get; set; }
        public int? Unit { get; set; }
        public bool? isAvailable { get; set; }
    }
    public class orderModel
    {
        public IList<orderItems> orderItemsList { get; set; }
        public IList<orderItems> specialOffer { get; set; }
        public orderAddressDetail orderAddress { get; set; }
        public int TotalItems { get; set; }
        public string TotalAmount { get; set; }
    }
    public class orderAddressDetail
    {
        public int OrderID { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string HouseName { get; set; }
        public string Address { get; set; }
        public string LandMark { get; set; }
        public string Town { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
    }
    public class orderItems
    {
        public int itemId { get; set; }
        public string quantity { get; set; }
        public double price { get; set; }
        public int unit { get; set; }
    }
    public class orderGetModel
    {
        public int orderId { get; set; }
        public IList<fishMJ> orderItemsList { get; set; }
        public IList<Sitems> specialOffer { get; set; }
        public orderAddressDetail orderAddress { get; set; }
        public int? TotalItems { get; set; }
        public string TotalAmount { get; set; }
        public int? status { get; set; }
    }
}