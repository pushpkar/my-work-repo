using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiDemo.Models;
using WebApiDemo.Models.Context;

namespace WebApiDemo.Controllers
{
    public class FreightController : ApiController
    {
        //Creating Instance of DatabaseContext class  
        private FreightDBContext db = new FreightDBContext();

        //Creating a method to return Json data
        //This action method will fetch and filter for specific member id record 

        // Get
        public HttpResponseMessage Get(int? sourceCityId = null, int? destinationCityId = null)
        {
            try
            {
                //Prepare data to be returned using Linq as follows
                IQueryable<FreightListViewModel> result;
                if ((sourceCityId == null || sourceCityId < 1) && (destinationCityId == null || destinationCityId < 1))
                {
                    result = from freightDetails in db.FreightMasters
                             select new FreightListViewModel { Id = freightDetails.Id, Mode = freightDetails.Mode.Name, SourceCity = freightDetails.SourceCity.Name, DestinationCity = freightDetails.DestinationCity.Name };
                }
                else if ((sourceCityId != null || sourceCityId > 0) && (destinationCityId == null || destinationCityId < 1))
                {
                    result = from freightDetails in db.FreightMasters
                             where freightDetails.SourceCityId == sourceCityId
                             select new FreightListViewModel { Id = freightDetails.Id, Mode = freightDetails.Mode.Name, SourceCity = freightDetails.SourceCity.Name, DestinationCity = freightDetails.DestinationCity.Name };
                }
                else if ((sourceCityId == null || sourceCityId < 1) && (destinationCityId != null || destinationCityId > 0))
                {
                    result = from freightDetails in db.FreightMasters
                             where freightDetails.DestinationCityId == destinationCityId
                             select new FreightListViewModel { Id = freightDetails.Id, Mode = freightDetails.Mode.Name, SourceCity = freightDetails.SourceCity.Name, DestinationCity = freightDetails.DestinationCity.Name };
                }
                else
                {
                    result = from freightDetails in db.FreightMasters
                             where freightDetails.SourceCityId == sourceCityId && freightDetails.DestinationCityId == destinationCityId
                             select new FreightListViewModel { Id = freightDetails.Id, Mode = freightDetails.Mode.Name, SourceCity = freightDetails.SourceCity.Name, DestinationCity = freightDetails.DestinationCity.Name };
                }
                if (result.Count() == 0)
                {
                    //Sending response as error status code No Record Found with meaningful message.  
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Record Found");
                }
                //Sending response as status code OK with memberdetail entity.  
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                //If any exception occurs Internal Server Error i.e. Status Code 500 will be returned  
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        //Edit
        public HttpResponseMessage Get(int id)
        {
            try
            {

                FreightViewModel freightViewModel = new FreightViewModel();
                List<FreightDetailsViewModel> lstFreightDetailsViewModel = new List<FreightDetailsViewModel>();

                // Fetch and filter specific record
                var freightMasterData = (from freightMasterObj in db.FreightMasters where freightMasterObj.Id == id select freightMasterObj).FirstOrDefault();

                //Checking masterdetais have data or not
                if (freightMasterData != null)
                {
                    // Assign Freight Master data
                    freightViewModel.DestinationCityId = freightMasterData.DestinationCityId;
                    freightViewModel.Id = freightMasterData.Id;
                    freightViewModel.INR_CubeFeet = freightMasterData.INR_CubeFeet;
                    freightViewModel.INR_KG = freightMasterData.INR_KG;
                    freightViewModel.IsActive = freightMasterData.IsActive;
                    freightViewModel.LoadUnLoadCharges = freightMasterData.LoadUnLoadCharges;
                    freightViewModel.ModeId = freightMasterData.ModeId;
                    freightViewModel.SourceCityId = freightMasterData.SourceCityId;

                    var lstFreightDetails = (from freightDetails in db.FreightDetails where freightDetails.FreightMasterId == freightMasterData.Id select freightDetails).ToList();

                    if (lstFreightDetails != null)
                    {
                        foreach (var freightDetail in lstFreightDetails)
                        {
                            lstFreightDetailsViewModel.Add(new FreightDetailsViewModel
                            {
                                CapacityId = freightDetail.CapacityId,
                                FreightMasterId = freightDetail.FreightMasterId,
                                Id = freightDetail.Id,
                                Rate = freightDetail.Rate,
                                RateCriteria = freightDetail.RateCriteria
                            });

                        }
                        freightViewModel.freightDetailsViewModel = lstFreightDetailsViewModel;
                    }

                    //Sending response as status code OK with memberdetail entity.  
                    return Request.CreateResponse(HttpStatusCode.OK, freightViewModel);
                }
                else
                {
                    //Sending response as error status code No Record Found with meaningful message.  
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Record Found");
                }

            }
            catch (Exception ex)
            {
                //If any exception occurs Internal Server Error i.e. Status Code 500 will be returned  
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // Update the recor
        [Route("SaveFreight")]
        public HttpResponseMessage Post(FreightViewModel freightViewModel)
        {
            try
            {

                FreightDetail freightDetailModel = new FreightDetail();
                FreightMaster freightMaster = new FreightMaster();

                //checking feightdetails are available.
                if (freightViewModel != null)
                {
                    // set received data object property with freightMaster
                    freightMaster.DestinationCityId = freightViewModel.DestinationCityId;
                    freightMaster.INR_CubeFeet = freightViewModel.INR_CubeFeet;
                    freightMaster.INR_KG = freightViewModel.INR_KG;
                    freightMaster.IsActive = freightViewModel.IsActive;
                    freightMaster.LoadUnLoadCharges = freightViewModel.LoadUnLoadCharges;
                    freightMaster.ModeId = freightViewModel.ModeId;
                    freightMaster.SourceCityId = freightViewModel.SourceCityId;

                    // Save master table data
                    db.FreightMasters.Add(freightMaster);
                    db.SaveChanges();

                    foreach (var freightDetail in freightViewModel.freightDetailsViewModel)
                    {
                        freightDetailModel.CapacityId = freightDetail.CapacityId;
                        freightDetailModel.FreightMasterId = freightMaster.Id;
                        freightDetailModel.Id = freightDetail.Id;
                        freightDetailModel.Rate = freightDetail.Rate;
                        freightDetailModel.RateCriteria = freightDetail.RateCriteria;
                    }

                    db.SaveChanges();
                    //return response status as successfully updated with member entity  
                    return Request.CreateResponse(HttpStatusCode.OK, "Record Saved");
                }

                else
                {
                    //return response error as NOT FOUND  with message.  
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Record Found");
                }

            }
            catch (Exception ex)
            {
                //return response as bad request  with exception message.  
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        // Update the record
        [Route("Update")]
        public HttpResponseMessage Put(int id, [FromBody]FreightViewModel freightViewModel)
        {
            try
            {
                // Fetch and filter specific record
                var freightMasterData = (from freightMasterObj in db.FreightMasters where freightMasterObj.Id == id select freightMasterObj).FirstOrDefault();

                //Checking masterdetais have data or not
                if (freightMasterData != null)
                {
                    var lstFreightDetails = (from freightDetails in db.FreightDetails where freightDetails.FreightMasterId == freightMasterData.Id select freightDetails).ToList();

                    FreightDetail freightDetailModel = new FreightDetail();
                    //checking feightdetails are available.
                    if (lstFreightDetails != null)
                    {
                        // set received data object property with freightMaster
                        freightMasterData.DestinationCityId = freightViewModel.DestinationCityId;
                        freightMasterData.Id = freightViewModel.Id;
                        freightMasterData.INR_CubeFeet = freightViewModel.INR_CubeFeet;
                        freightMasterData.INR_KG = freightViewModel.INR_KG;
                        freightMasterData.IsActive = freightViewModel.IsActive;
                        freightMasterData.LoadUnLoadCharges = freightViewModel.LoadUnLoadCharges;
                        freightMasterData.ModeId = freightViewModel.ModeId;
                        freightMasterData.SourceCityId = freightViewModel.SourceCityId;

                        // Save master table data
                        db.FreightMasters.Add(freightMasterData);

                        foreach (var freightDetail in freightViewModel.freightDetailsViewModel)
                        {
                            freightDetailModel.CapacityId = freightDetail.CapacityId;
                            freightDetailModel.FreightMasterId = freightDetail.FreightMasterId;
                            freightDetailModel.FreightMasters = freightMasterData;
                            freightDetailModel.Id = freightDetail.Id;
                            freightDetailModel.Rate = freightDetail.Rate;
                            freightDetailModel.RateCriteria = freightDetail.RateCriteria;
                        }
                        db.SaveChanges();
                        //return response status as successfully updated with member entity  
                        return Request.CreateResponse(HttpStatusCode.OK, "Record updated");
                    }

                    else
                    {
                        //return response error as NOT FOUND  with message.  
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Record Found");
                    }
                }
                else
                {
                    //return response error as NOT FOUND  with message.  
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Record Found");
                }

            }
            catch (Exception ex)
            {
                //return response as bad request  with exception message.  
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // Delete freight
        public HttpResponseMessage Delete(int Id)
        {
            FreightMaster freightMaster = db.FreightMasters.Where(x => x.Id == Id).FirstOrDefault();
            try
            {
                if (freightMaster != null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                db.FreightMasters.Remove(freightMaster);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Record Deleted");

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
