using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json;

namespace Phone_Light_Sensor.Helpers
{
    public static class CRMGateway
    {
        public class EmailData
        {
            public string to;
            public string subject;
            public string body;
        }

        public class AutoCase
        {
            public string name;
            public string latitude;
            public string longitude;
        }

        public static void SendEmail()
        {
            var actionUri = "http://siena5986.azurewebsites.net/api/Email";

            EmailData emailData = new EmailData { to = "dalild.microsoft.com", subject = "Subject", body = "Body" };
            string postBody = JsonConvert.SerializeObject(emailData);

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = httpClient.PostAsync(actionUri, new StringContent(postBody, Encoding.UTF8, "application/json"));
        }

        public async static void CreateAutoCase(double latitude, double longitude, string street, string zipcode, string city, string country)
        {
            string name = "Phone Light Sensor Exposure to Excessive Light";

            WebRequest request = WebRequest.Create("http://crmgateway.azurewebsites.net/api/AutoCase?name=" + name + "&latitude=" + latitude.ToString() + "&longitude=" + longitude.ToString() + "&street=" +  street + "&zipcode=" + zipcode + "&city=" + city + "&country=" + country);
            HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync());
        }
    }
}
