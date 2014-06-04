using System;
using Newtonsoft.Json;

/*
{
    "Line":[
        {
            "Amount":100.00,
            "DetailType":"SalesItemLineDetail",
            "SalesItemLineDetail":
            {
                "ItemRef":
                {
                    "value":"1",
                    "name":"Services"
                }
            }
        }
    ],
    "CustomerRef":
    {
        "value":"21"
    }
}
*/

namespace QbApiMvc.Models
{
    [JsonObject]
    public class CustomerInvoice
    {
        [JsonProperty(PropertyName = "Line")]
        public InvoiceLine[] Line { get; set; }

        [JsonProperty(PropertyName = "CustomerRef")]
        public TypeReference CustomerRef { get; set; }

        [JsonProperty(PropertyName = "BillEmail")]
        public string BillEmail { get; set; }

    }

    [JsonObject]
    public class InvoiceLine
    {
        [JsonProperty(PropertyName = "Amount")]
        public decimal Amount { get; set; }

        [JsonProperty(PropertyName = "DetailType")]
        public string DetailType { get; set; }

        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "SalesItemLineDetail")]
        public SalesItemLineDetail SalesItemLineDetail { get; set; }
    }

    [JsonObject]
    public class SalesItemLineDetail
    {
        [JsonProperty(PropertyName = "ItemRef")]
        public TypeReference ItemRef { get; set; }
    }

    [JsonObject]
    public class TypeReference
    {
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "Value")]
        public string Value { get; set; }

        [JsonProperty(PropertyName = "Type")]
        public string Type { get; set; }
    }
}