using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    public class QbInvoiceController : ApiController
    {

        [HttpGet]
        public IEnumerable<Invoice> GetAllInvoices()
        {
            var serviceContext = QbApiHelper.GetServiceContext();

            var query = new QueryService<Invoice>(serviceContext);

            var invoices = query.Select(c => c).ToList();

            return invoices;

        }


        [HttpGet]
        public IEnumerable<Invoice> GetInvoicesByDocNumber(int invoiceDocNumber)
        {
            var serviceContext = QbApiHelper.GetServiceContext();

            var query = new QueryService<Invoice>(serviceContext);

            var invoices =
                query.Where( c => c.DocNumber == invoiceDocNumber.ToString(CultureInfo.InvariantCulture) );

            return invoices;

        }


        [HttpGet]
        public IEnumerable<Invoice> GetCustomerInvoices(int customerId)
        {
            var serviceContext = QbApiHelper.GetServiceContext();

            var query = new QueryService<Invoice>(serviceContext);

            var id = customerId.ToString(CultureInfo.InvariantCulture);

            var invoices =
                query.ExecuteIdsQuery(string.Format(@"Select * from Invoice where CustomerRef = '{0}'", id)).ToList();

            return invoices;

        }


        [HttpPost]
        public Invoice CreateNewInvoice(CustomerInvoice cinvoice)
        {
            try
            {
                if (cinvoice == null)
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invoice cannot be null"));


                var serviceContext = QbApiHelper.GetServiceContext();

                var service = new DataService(serviceContext);

                var invoice = MapCustomerInvoiceToInvoice(cinvoice);

                var returnedInvoice = service.Add<Invoice>(invoice);

                return returnedInvoice;

            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "An error ocurred while creating a new Invoice: " + e.Message));
            }

        }

        /// <summary>
        /// Mapping between the Customer Invoice Entity and the Intuit Invoice Entity
        /// </summary>
        /// <returns></returns>
        private static Invoice MapCustomerInvoiceToInvoice(CustomerInvoice customerInvoice)
        {
            if (customerInvoice == null) throw new ArgumentNullException("customerInvoice");

            var invoice = new Invoice();

            var lines = customerInvoice.Line.Select(line => new Line
            {
                Amount = line.Amount, 
                AmountSpecified = true,
                DetailTypeSpecified = true,
                DetailType = LineDetailTypeEnum.SalesItemLineDetail, 
                Description = line.Description,
                AnyIntuitObject = new Intuit.Ipp.Data.SalesItemLineDetail
                {
                    ItemElementName = ItemChoiceType.UnitPrice,
                    AnyIntuitObject = line.Amount,

                    ItemRef = new ReferenceType
                    {
                        Value = line.SalesItemLineDetail.ItemRef.Value,
                        name = line.SalesItemLineDetail.ItemRef.Name,
                        type = line.SalesItemLineDetail.ItemRef.Type
                    }
                }
            }).ToList();

            invoice.Line = lines.ToArray();

            
            invoice.BillEmail = new EmailAddress
            {
                Address = customerInvoice.BillEmail,
                Default = true,
                DefaultSpecified = true
            };

            invoice.TxnDate = DateTime.UtcNow;
            invoice.TxnDateSpecified = true;
            invoice.DueDate = DateTime.UtcNow.AddDays(30);
            invoice.DueDateSpecified = true;

            invoice.EmailStatusSpecified = true;
            invoice.EmailStatus = EmailStatusEnum.NeedToSend;
            

            invoice.CustomerRef = new ReferenceType
            {
                Value = customerInvoice.CustomerRef.Value,
                name = customerInvoice.CustomerRef.Name,
                type = customerInvoice.CustomerRef.Type
            };


            return invoice;
        }
    }
}
