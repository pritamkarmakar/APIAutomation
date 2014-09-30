using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace APIAutomation.Helper
{
    class CommonMethods
    {
        #region Http GET, POST, DELETE Opetation

        /// <summary>
        /// Method to retrieve the response of a http GET request
        /// </summary>
        /// <param name="url">API URL</param>
        /// <returns>response from the GET request</returns>
        public string HttpGETResponse(string url)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.ExpectContinue = false;
            HttpResponseMessage apiVersionResp = null;

            try
            {
                apiVersionResp =
                    client.GetAsync(url).Result;
                if (apiVersionResp.StatusCode != HttpStatusCode.OK)
                    return string.Empty;
            }
            catch (AggregateException ex)
            {
                return string.Empty;
            }

            return apiVersionResp.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Method to retrieve the response after http POST request
        /// </summary>
        /// <param name="url">API URL</param>
        /// <param name="content">HttpContent httpContent</param>
        /// <returns>response after the POST request</returns>
        public static string HttpPostResponse(string url, HttpContent content)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage apiVersionResp = null;

            try
            {
                apiVersionResp = client.PostAsync(url, content).Result;
                if (apiVersionResp.StatusCode != HttpStatusCode.OK)
                    return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }

            return apiVersionResp.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Method to retrieve the response after http PUT request
        /// </summary>
        /// <param name="url">API URL</param>
        /// <param name="content">HttpContent httpContent</param>
        /// <returns>response after the PUT request</returns>
        public static string HttpPutResponse(string url, HttpContent content)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage apiVersionResp = null;

            try
            {
                apiVersionResp = client.PutAsync(url, content).Result;
                if (apiVersionResp.StatusCode != HttpStatusCode.OK)
                    return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }

            return apiVersionResp.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Method to retrieve the response after http DELETE operation
        /// </summary>
        /// <param name="url">API URL</param>
        /// <returns>response after the DELETE operation</returns>
        public static string HttpDeleteResponse(string url)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage apiVersionResp = null;

            try
            {
                apiVersionResp = client.DeleteAsync(url).Result;
            }
            catch (Exception)
            {
                return string.Empty;
            }

            return apiVersionResp.Content.ReadAsStringAsync().Result;
        }

        #endregion


        /// <summary>
        /// Generic method to deserialize json string to corresponding class type
        /// </summary>
        /// <param name="json">json string</param>
        /// <param name="type">class type to deserialize</param>
        /// <returns></returns>
        public object Deserialize(string json, Type type)
        {
            try
            {
                return JsonConvert.DeserializeObject(json, type);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
