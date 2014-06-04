using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;
using Intuit.Ipp.LinqExtender;
using Intuit.Ipp.QueryFilter;

using QbApiMvc.Models;


namespace QbApiMvc.Controllers
{
    public class QbCustomerController : ApiController
    {

        [HttpGet]
        public IEnumerable<Customer> GetAllCustomers()
        {

            var serviceContext = QbApiHelper.GetServiceContext();

            var query = new QueryService<Customer>(serviceContext);

            var customers = query.Select(c => c).ToList();

            return customers;

        }

        [HttpGet]
        public Customer GetCustomer(int id)
        {
            var serviceContext = QbApiHelper.GetServiceContext();

            var query = new QueryService<Customer>(serviceContext);

            var customer = query.Where(c => c.Id == id.ToString(CultureInfo.InvariantCulture)).FirstOrDefault();

            return customer;

        }


        [HttpGet]
        public string GetCustomerName(int userId)
        {
            var serviceContext = QbApiHelper.GetServiceContext();

            var query = new QueryService<Customer>(serviceContext);

            var customer = query.Where(c => c.Id == userId.ToString(CultureInfo.InvariantCulture)).FirstOrDefault();

            return customer != null ? customer.DisplayName : string.Empty;

        }


        [HttpPost]
        public Customer CreateCustomer(UserProfile userProfile)
        {

            try
            {
                if(userProfile == null)
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "UserProfile cannot be null"));

                if (userProfile.Email == null)
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Email cannot be null"));


                var serviceContext = QbApiHelper.GetServiceContext();

                var service = new DataService(serviceContext);

                var email = new EmailAddress
                {
                    Address = userProfile.Email,
                    Default = true,
                    DefaultSpecified = true
                };

                var customer = new Customer
                {
                    GivenName = userProfile.FirstName,
                    FamilyName = userProfile.LastName,
                    PrimaryEmailAddr = email,
                    UserId = userProfile.UserId.ToString(CultureInfo.InvariantCulture)
                };

                var resultCustomer = service.Add<Customer>(customer);

                return resultCustomer;
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error when creating a new user: " + e.Message));
            }
            

        }



    }
}
