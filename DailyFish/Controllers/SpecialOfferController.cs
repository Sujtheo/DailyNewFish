using ShopifyW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ShopifyW.Controllers
{
    public class SpecialOfferController : ApiController
    {
        DailyFishEntities dailyfish = new DailyFishEntities();
        [HttpPost]
        [Route("api/SpecialOffer")]
        public HttpResponseMessage post(specialOfferMj specialOffer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        SpecialOffer spcadd = new SpecialOffer();
                       // spcadd.ItemName = specialOffer.name;
                       //spcadd.ItemMalName = specialOffer.malayalam;
                        spcadd.Price = Convert.ToInt32(specialOffer.price);
                        //spcadd.Quantity = Convert.ToInt32(specialOffer.quantity);
                       // spcadd.ItemImage = specialOffer.img;
                        spcadd.Unit = 1;
                        dailyfish.SpecialOffers.Add(spcadd);
                        dailyfish.SaveChanges();
                        if (specialOffer.OfferItems.Count > 0)
                        {
                            if (specialOffer.OfferItems.Count > 1)
                            {
                                IList<Sitems> OfferItemList = new List<Sitems>();
                                OfferItemList = specialOffer.OfferItems;
                                OfferItemList.ToList().ForEach(data =>
                                {
                                    SpecialOfferItem spoitem = new SpecialOfferItem();
                                    spoitem.ItemID = data.ItemId;
                                    spoitem.OfferID = spcadd.OfferID;
                                    spoitem.Quantity = data.quantity;
                                    dailyfish.SpecialOfferItems.Add(spoitem);
                                    dailyfish.SaveChanges();
                                });
                            }
                            else
                            {
                                SpecialOfferItem spoitem = new SpecialOfferItem();
                                spoitem.ItemID = specialOffer.OfferItems[0].ItemId;
                                spoitem.OfferID = spcadd.OfferID;
                                spoitem.Quantity = specialOffer.OfferItems[0].quantity;
                                dailyfish.SpecialOfferItems.Add(spoitem);
                                dailyfish.SaveChanges();
                            }
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, "Added SuccessFully");
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
    }
}
