using ShopifyW.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace ShopifyW.Controllers
{
    public class DailyFishHomeController : ApiController
    {
        DailyFishEntities dailyfish = new DailyFishEntities();
        [HttpGet]
        [Route("api/GetAllProducts")]
        public HttpResponseMessage GetALlProduct()
        {
            DailyFish DailyFishM = new DailyFish();
            List<specialOfferMj> specialOfferMJCombo = new List<specialOfferMj>();
            List<Sitems> specialOfferSingle = new List<Sitems>();
            List<fishMJ> fishlist = new List<fishMJ>();
            List<fishMJ> vegetableslist = new List<fishMJ>();
            List<fishMJ> polutryList = new List<fishMJ>();
            List<fishMJ> eggList = new List<fishMJ>();
            Dictionary<string, string> quantityPriceList;
            var specialOfferdata = dailyfish.SpecialOffers.OrderByDescending(x=>x.OfferID).ToList();
            var fishlistdata = dailyfish.Items.Where(x => x.Category.CategoryName == "fish").ToList();
            var vegetabledata = dailyfish.Items.Where(x => x.Category.CategoryName == "vegetables").ToList();
            var polutrydata = dailyfish.Items.Where(x => x.Category.CategoryName == "poultry").ToList();
            var eggdata = dailyfish.Items.Where(x => x.Category.CategoryName == "egg").ToList();
            specialOfferdata.ForEach(sodata =>
            {
                specialOfferMj specialOfferdetail = new specialOfferMj();
                specialOfferdetail.specOfferId = sodata.OfferID;
                // specialOfferdetail.name = sodata.ItemName;
                // specialOfferdetail.malayalam = sodata.ItemMalName;
                specialOfferdetail.price = sodata.Price.ToString();
                // specialOfferdetail.quantity = sodata.Quantity.ToString();
                // specialOfferdetail.img = sodata.ItemImage;
                var sOfferItems = dailyfish.SpecialOfferItems.Where(x => x.OfferID == sodata.OfferID).ToList();
                List<Sitems> ofritemList = new List<Sitems>();
                if (sOfferItems.Count > 0)
                {
                    if (sOfferItems.Count > 1)
                    {
                        sOfferItems.ForEach(ofrdata =>
                        {
                            Sitems ofritem = new Sitems();
                            ofritem.name = ofrdata.Item.ItemName;
                            ofritem.malayalam = ofrdata.Item.ItemMalName;
                            ofritem.productId = ofrdata.ItemID;
                            ofritem.quantity = ofrdata.Quantity.ToString();
                            ofritem.img = ofrdata.Item.ItemImage;
                            ofritem.price = ofrdata.Item.ItemPrices.Where(x => x.ItemID == ofrdata.ItemID).FirstOrDefault().Price.ToString();
                            ofritem.quantity = ofrdata.Item.ItemPrices.Where(x => x.ItemID == ofrdata.ItemID).FirstOrDefault().Quantity.ToString();
                            ofritemList.Add(ofritem);
                        });
                        specialOfferdetail.OfferItems = ofritemList;
                        specialOfferMJCombo.Add(specialOfferdetail);
                    }
                    else
                    {
                        sOfferItems.ForEach(ofrdata =>
                        {
                            Sitems ofritem = new Sitems();
                            ofritem.name = ofrdata.Item.ItemName;
                            ofritem.malayalam = ofrdata.Item.ItemMalName;
                            ofritem.productId = sodata.OfferID;
                            ofritem.quantity = ofrdata.Quantity.ToString();
                            ofritem.img = ofrdata.Item.ItemImage;
                            ofritem.price = ofrdata.SpecialOffer.Price.ToString();
                            ofritem.quantity = dailyfish.SpecialOfferItems.Where(x => x.OfferID == ofrdata.OfferID && x.ItemID == ofrdata.ItemID).FirstOrDefault().Quantity.ToString();
                            specialOfferSingle.Add(ofritem);
                        });

                    }

                }

            });
            fishlistdata.ForEach(fdata =>
            {
                fishMJ fishdetail = new fishMJ();
                fishdetail.productId = fdata.ItemID;
                fishdetail.name = fdata.ItemName;
                fishdetail.malayalam = fdata.ItemMalName;
                fishdetail.img = fdata.ItemImage;
                fishdetail.unit = 1;
                fishdetail.price = dailyfish.ItemPrices.Where(x => x.ItemID == fdata.ItemID).FirstOrDefault().Price.ToString();
                fishdetail.quantity = dailyfish.ItemPrices.Where(x => x.ItemID == fdata.ItemID).FirstOrDefault().Quantity.ToString();
                var quantitypricedata = dailyfish.ItemPrices.Where(x => x.ItemID == fdata.ItemID).ToList();
                List<priceListModel> quantityPriceLists = new List<priceListModel>();
                quantitypricedata.ForEach(dd =>
                {
                    priceListModel priceLists = new priceListModel();
                    priceLists.quantity = dd.Quantity.ToString();
                    priceLists.price = dd.Price.ToString();
                    quantityPriceLists.Add(priceLists);
                });
                fishdetail.isAvailable = fdata.IsAvailable;
                fishdetail.priceList = quantityPriceLists;
                fishlist.Add(fishdetail);
            });
            vegetabledata.ForEach(fdata =>
            {
                fishMJ vegdetail = new fishMJ();
                vegdetail.productId = fdata.ItemID;
                vegdetail.name = fdata.ItemName;
                vegdetail.malayalam = fdata.ItemMalName;
                vegdetail.img = fdata.ItemImage;
                vegdetail.unit = 1;
                vegdetail.price = dailyfish.ItemPrices.Where(x => x.ItemID == fdata.ItemID).FirstOrDefault().Price.ToString();
                vegdetail.quantity = dailyfish.ItemPrices.Where(x => x.ItemID == fdata.ItemID).FirstOrDefault().Quantity.ToString();
                var quantitypricedata = dailyfish.ItemPrices.Where(x => x.ItemID == fdata.ItemID).ToList();
                List<priceListModel> quantityPriceLists = new List<priceListModel>();
                quantitypricedata.ForEach(dd =>
                {
                    priceListModel priceLists = new priceListModel();
                    priceLists.quantity = dd.Quantity.ToString();
                    priceLists.price = dd.Price.ToString();
                    quantityPriceLists.Add(priceLists);
                });
                vegdetail.isAvailable = fdata.IsAvailable;
                vegdetail.priceList = quantityPriceLists;
                vegetableslist.Add(vegdetail);
            });
            polutrydata.ForEach(fdata =>
            {
                fishMJ poloutrydetail = new fishMJ();
                poloutrydetail.productId = fdata.ItemID;
                poloutrydetail.name = fdata.ItemName;
                poloutrydetail.malayalam = fdata.ItemMalName;
                poloutrydetail.img = fdata.ItemImage;
                poloutrydetail.unit = 1;
                poloutrydetail.price = dailyfish.ItemPrices.Where(x => x.ItemID == fdata.ItemID).FirstOrDefault().Price.ToString();
                poloutrydetail.quantity = dailyfish.ItemPrices.Where(x => x.ItemID == fdata.ItemID).FirstOrDefault().Quantity.ToString();
                var quantitypricedata = dailyfish.ItemPrices.Where(x => x.ItemID == fdata.ItemID).ToList();
                List<priceListModel> quantityPriceLists = new List<priceListModel>();
                quantitypricedata.ForEach(dd =>
                {
                    priceListModel priceLists = new priceListModel();
                    priceLists.quantity = dd.Quantity.ToString();
                    priceLists.price = dd.Price.ToString();
                    quantityPriceLists.Add(priceLists);
                });
                poloutrydetail.isAvailable = fdata.IsAvailable;
                poloutrydetail.priceList = quantityPriceLists;
                polutryList.Add(poloutrydetail);
            });
            eggdata.ForEach(fdata =>
            {
                fishMJ fishdetail = new fishMJ();
                fishdetail.productId = fdata.ItemID;
                fishdetail.name = fdata.ItemName;
                fishdetail.malayalam = fdata.ItemMalName;
                fishdetail.img = fdata.ItemImage;
                fishdetail.unit = 1;
                fishdetail.price = dailyfish.ItemPrices.Where(x => x.ItemID == fdata.ItemID).FirstOrDefault().Price.ToString();
                fishdetail.quantity = dailyfish.ItemPrices.Where(x => x.ItemID == fdata.ItemID).FirstOrDefault().Quantity.ToString();
                var quantitypricedata = dailyfish.ItemPrices.Where(x => x.ItemID == fdata.ItemID).ToList();
                List<priceListModel> quantityPriceLists = new List<priceListModel>();
                quantitypricedata.ForEach(dd =>
                {
                    priceListModel priceLists = new priceListModel();
                    priceLists.quantity = dd.Quantity.ToString();
                    priceLists.price = dd.Price.ToString();
                    quantityPriceLists.Add(priceLists);
                });
                fishdetail.isAvailable = fdata.IsAvailable;
                fishdetail.priceList = quantityPriceLists;
                eggList.Add(fishdetail);
            });
            DailyFishM.specialOffer = specialOfferMJCombo;
            DailyFishM.specialOfferSingle = specialOfferSingle;
            DailyFishM.fish = fishlist;
            DailyFishM.vegetables = vegetableslist;
            DailyFishM.poultry = polutryList;
            DailyFishM.egg = eggList;
            return this.Request.CreateResponse(System.Net.HttpStatusCode.OK, DailyFishM);

        }
        [HttpGet]
        [Route("api/GetAllCategory")]
        public HttpResponseMessage GetALlCategory()
        {
            List<categoryModel> catModelList = new List<categoryModel>();
            var catdata = dailyfish.Categories.ToList();
            if (catdata.Count > 0)
            {
                catdata.ForEach(data =>
                {
                    categoryModel catModel = new categoryModel();
                    catModel.CategoryID = data.CategoryID;
                    catModel.CategoryName = data.CategoryName;
                    catModelList.Add(catModel);
                });
            }
            return this.Request.CreateResponse(System.Net.HttpStatusCode.OK, catModelList);

        }
        [HttpGet]
        [Route("api/GetAllOrder")]
        public HttpResponseMessage GetALlOrder()
        {
            List<orderGetModel> orderGetModelList = new List<orderGetModel>();
            orderGetModel orderGetModels = new orderGetModel();
            List<fishMJ> itemList = new List<fishMJ>();
            List<Sitems> sIemList = new List<Sitems>();
            List<OrderDetail> sOrderList = new List<OrderDetail>();
            var orddata = dailyfish.Orders.OrderByDescending(x=>x.OrderID).ToList();
            if (orddata.Count > 0)
            {
                orddata.ForEach(data =>
                {
                    orderGetModels = new orderGetModel();
                    orderGetModels.orderId = data.OrderID;
                    orderGetModels.status = data.Status;
                    orderGetModels.TotalAmount = data.TotalAmount;
                    orderGetModels.TotalItems = data.TotalItems;
                    sOrderList = new List<OrderDetail>();
                    sOrderList = dailyfish.OrderDetails.Where(x => x.OrderId == data.OrderID).ToList();
                    itemList = new List<fishMJ>();
                    sIemList = new List<Sitems>();
                    sOrderList.ForEach(y =>
                    {

                        if (y.SpecialOfferId != null)
                        {
                            Sitems NSitems = new Sitems();
                            NSitems.ItemId = y.SpecialOffer.OfferID;
                            NSitems.quantity = y.Unit.ToString();
                            if (y.SpecialOfferId != null)
                            {
                                int offerid = dailyfish.SpecialOfferItems.Where(t => t.OfferID == y.SpecialOfferId).FirstOrDefault() != null ? dailyfish.SpecialOfferItems.Where(t => t.OfferID == y.SpecialOfferId).FirstOrDefault().ItemID : 0;
                                if (offerid != 0)
                                {
                                    NSitems.name = dailyfish.Items.Where(x => x.ItemID == offerid).FirstOrDefault().ItemName;
                                    NSitems.malayalam = dailyfish.Items.Where(x => x.ItemID == offerid).FirstOrDefault().ItemMalName;
                                    NSitems.img = dailyfish.Items.Where(x => x.ItemID == offerid).FirstOrDefault().ItemImage;
                                }
                            }
                            else
                            {
                                NSitems.name = dailyfish.Items.Where(x => x.ItemID == y.ItemID).FirstOrDefault().ItemName;
                                NSitems.malayalam = dailyfish.Items.Where(x => x.ItemID == y.ItemID).FirstOrDefault().ItemMalName;
                                NSitems.img = dailyfish.Items.Where(x => x.ItemID == y.ItemID).FirstOrDefault().ItemImage;

                            }
                            NSitems.price = y.Price.ToString();
                            sIemList.Add(NSitems);
                        }
                        else
                        {
                            fishMJ fis = new fishMJ();
                            fis.name = y.Item.ItemName;
                            fis.productId = y.Item.ItemID;
                            fis.quantity = y.Quantity.ToString();
                            fis.unit = y.Unit;
                            fis.malayalam = y.Item.ItemMalName;
                            itemList.Add(fis);
                        }

                    });
                    orderGetModels.orderItemsList = itemList;
                    orderGetModels.specialOffer = sIemList;
                    orderGetModels.status = data.Status;
                    orderAddressDetail orderAddressDetails = new orderAddressDetail();
                    orderAddressDetails.Name = data.Name;
                    orderAddressDetails.Mobile = data.Mobile;
                    orderAddressDetails.HouseName = data.HouseName;
                    orderAddressDetails.Address = data.Address;
                    orderAddressDetails.LandMark = data.LandMark;
                    orderAddressDetails.Town = data.Town;
                    orderAddressDetails.City = data.City;
                    orderAddressDetails.Pincode = data.PinCode;
                    orderGetModels.orderAddress = orderAddressDetails;
                    orderGetModelList.Add(orderGetModels);
                });
               
            }
            return this.Request.CreateResponse(System.Net.HttpStatusCode.OK, orderGetModelList);

        }
    }
}
