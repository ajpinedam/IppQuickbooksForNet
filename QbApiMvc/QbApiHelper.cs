using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Intuit.Ipp.Data;
using Intuit.Ipp.Core;
using Intuit.Ipp.DataService;
using Intuit.Ipp.Exception;
using Intuit.Ipp.LinqExtender;
using Intuit.Ipp.Security;
using Intuit.Ipp.QueryFilter;

using QbApiMvc.Models;


namespace QbApiMvc
{
    public class QbApiHelper
    {

        public static ServiceContext GetServiceContext()
        {
            var oAuthValiator = new OAuthRequestValidator(ConstValues.AccessToken, ConstValues.AccessTokenSecret, ConstValues.ConsumerKey, ConstValues.ConsumerSecret);

            var serviceContext = new ServiceContext(ConstValues.AppToken, ConstValues.CompanyId, IntuitServicesType.QBO, oAuthValiator);

            return serviceContext;
        }

    }
}