using ShopifyW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ShopifyW.Controllers
{
    public class AddItemController : ApiController
    {
        DailyFishEntities dailyfish = new DailyFishEntities();
        [HttpPost]
        [Route("api/AddItem")]
        public HttpResponseMessage post(itemInputModel ItemDetail)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Item items = new Item();
                        items.CategoryId = ItemDetail.CategoryId;
                        items.ItemName = ItemDetail.ItemName;
                        items.ItemMalName = ItemDetail.ItemMalName;
                        items.ItemImage = ItemDetail.ItemImage;
                        items.IsAvailable = ItemDetail.isAvailable;
                        dailyfish.Items.Add(items);
                        ItemPrice itempr = new ItemPrice();
                        itempr.Quantity = ItemDetail.Quantity;
                        itempr.Price = ItemDetail.Price;
                        itempr.Unit = 1;
                        itempr.ItemID = items.ItemID;

                        return Request.CreateResponse(HttpStatusCode.OK, "Added SuccessFully");
                    }
                    catch
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid Item Details");
                    }

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid Model");
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid Model");
            }
        }
        [HttpPost]
        [Route("api/AddOrder")]
        public HttpResponseMessage post(orderModel orderModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        int newOrderId = 0;
                        Order orders = new Order();
                        orders.Name = orderModel.orderAddress.Name;
                        orders.Mobile = orderModel.orderAddress.Mobile;
                        orders.HouseName = orderModel.orderAddress.HouseName;
                        orders.Address = orderModel.orderAddress.Address;
                        orders.LandMark = orderModel.orderAddress.LandMark;
                        orders.Town = orderModel.orderAddress.Town;
                        orders.City = orderModel.orderAddress.City;
                        orders.PinCode = orderModel.orderAddress.Pincode;
                        orders.TotalItems = orderModel.TotalItems;
                        orders.TotalAmount = orderModel.TotalAmount;
                        orders.Status = 1;
                        dailyfish.Orders.Add(orders);
                        dailyfish.SaveChanges();
                        newOrderId = orders.OrderID;
                        var sOffer = orderModel.specialOffer.ToList();
                        if (sOffer.Count > 0)
                        {
                            sOffer.ForEach(data =>
                            {
                                OrderDetail odetail = new OrderDetail();
                                odetail.OrderId = newOrderId;
                                odetail.Unit = data.unit;
                                odetail.SpecialOfferId = data.itemId;
                                odetail.Price = data.price;
                                odetail.Quantity = data.quantity;
                                dailyfish.OrderDetails.Add(odetail);
                                dailyfish.SaveChanges();

                            });
                        }
                        var otems = orderModel.orderItemsList.ToList();
                        if (otems.Count > 0)
                        {
                            otems.ForEach(data =>
                            {
                                OrderDetail odetail = new OrderDetail();
                                odetail.OrderId = orders.OrderID;
                                odetail.Unit = data.unit;
                                odetail.ItemID = data.itemId;
                                odetail.Price = data.price;
                                odetail.Quantity = data.quantity;
                                dailyfish.OrderDetails.Add(odetail);
                                dailyfish.SaveChanges();

                            });
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, orders.OrderID);
                    }
                    catch(Exception ex)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                    }

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid Model");
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid Model");
            }
        }
        [HttpPost]
        [Route("api/AddAvilableItem")]
        public HttpResponseMessage post(List<int> ItemIdList)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var datitems=dailyfish.Items.Where(x=>x.IsAvailable == true).ToList();
                        datitems.ForEach(dat =>
                        {
                            dat.IsAvailable = false;
                        });
                        dailyfish.SaveChanges();
                        ItemIdList.ForEach(udata =>
                        {
                            var udatitems = dailyfish.Items.Where(x => x.ItemID == udata).FirstOrDefault();
                            udatitems.IsAvailable = true;
                            dailyfish.SaveChanges();
                        });
                        return Request.CreateResponse(HttpStatusCode.OK, "OK");
                    }
                    catch
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid Model");
                    }

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid Model");
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid Model");
            }
        }

        [HttpPost]
        [Route("api/UpdateOrderStatus")]
        public HttpResponseMessage UpdateOrderStatus(int orderId,int status)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var udatitems = dailyfish.Orders.Where(x => x.OrderID == orderId).FirstOrDefault();
                        udatitems.Status = status;
                        dailyfish.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "OK");
                    }
                    catch
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid Model");
                    }

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid Model");
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid Model");
            }
        }
        [HttpPost]
        [Route("api/Updateprize")]
        public HttpResponseMessage Updateprize(AddPricemodel AddPriceList)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        int itemid = AddPriceList.itemPrices.FirstOrDefault().ItemID;
                        var obj = dailyfish.ItemPrices.Where(x => x.ItemID == itemid).ToList();
                        if (obj != null &&  obj.Count > 0)
                        {
                            dailyfish.ItemPrices.RemoveRange(obj);
                            dailyfish.SaveChanges();
                        }
                        AddPriceList.itemPrices.ForEach(data =>
                        {
                            ItemPrice itempr = new ItemPrice();
                            itempr.Quantity = data.Quantity;
                            itempr.Price = data.Price;
                            itempr.Unit = 1;
                            itempr.ItemID = data.ItemID;
                            dailyfish.ItemPrices.Add(itempr);
                            dailyfish.SaveChanges();
                        });
                     
                        return Request.CreateResponse(HttpStatusCode.OK, "OK");
                    }
                    catch
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid Model");
                    }

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid Model");
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid Model");
            }
        }

        [HttpPost]
        [Route("api/DeleteItem")]
        public HttpResponseMessage DeleteItem(int ProductId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var obj = dailyfish.ItemPrices.Where(x => x.ItemID == ProductId).ToList();
                        if (obj != null && obj.Count > 0)
                        {
                            dailyfish.ItemPrices.RemoveRange(obj);
                            dailyfish.SaveChanges();
                        }
                        var so = dailyfish.SpecialOfferItems.Where(x => x.ItemID == ProductId).ToList();
                        if (so != null && so.Count > 0)
                        {
                            dailyfish.SpecialOfferItems.RemoveRange(so);
                            dailyfish.SaveChanges();
                        }
                        var prod = dailyfish.Items.Where(x => x.ItemID == ProductId).FirstOrDefault();
                        if (prod != null)
                        {
                            dailyfish.Items.Remove(prod);
                            dailyfish.SaveChanges();
                        }

                        return Request.CreateResponse(HttpStatusCode.OK, "Deleted");
                    }
                    catch
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid Item");
                    }

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid Item");
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid Item");
            }
        }
        [HttpPost]
        [Route("api/DeleteOffer")]
        public HttpResponseMessage DeleteOffer(int OfferId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        
                        var so = dailyfish.SpecialOfferItems.Where(x => x.OfferID == OfferId).ToList();
                        if (so != null && so.Count > 0)
                        {
                            dailyfish.SpecialOfferItems.RemoveRange(so);
                            dailyfish.SaveChanges();
                        }
                        var prod = dailyfish.SpecialOffers.Where(x => x.OfferID == OfferId).FirstOrDefault();
                        if (prod != null)
                        {
                            dailyfish.SpecialOffers.Remove(prod);
                            dailyfish.SaveChanges();
                        }

                        return Request.CreateResponse(HttpStatusCode.OK, "Deleted");
                    }
                    catch
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid Item");
                    }

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid Item");
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid Item");
            }
        }
        [HttpPost]
        [Route("api/DeleteOrder")]
        public HttpResponseMessage DeleteOrder(int OrderId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {

                        var so = dailyfish.OrderDetails.Where(x => x.OrderId == OrderId).ToList();
                        if (so != null && so.Count > 0)
                        {
                            dailyfish.OrderDetails.RemoveRange(so);
                            dailyfish.SaveChanges();
                        }
                        var prod = dailyfish.Orders.Where(x => x.OrderID == OrderId).FirstOrDefault();
                        if (prod != null)
                        {
                            dailyfish.Orders.Remove(prod);
                            dailyfish.SaveChanges();
                        }

                        return Request.CreateResponse(HttpStatusCode.OK, "Deleted");
                    }
                    catch
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid Item");
                    }

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid Item");
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Invalid Item");
            }
        }
    }
}
