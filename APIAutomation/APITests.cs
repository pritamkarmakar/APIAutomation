using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using APIAutomation.DataModels;
using APIAutomation.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace APIAutomation
{
    [TestClass]
    public class APITests
    {
        private string accessToken = ConfigurationManager.AppSettings["Access_Token"];
        private string serviceUrl = ConfigurationManager.AppSettings["Service_URL"];

        CommonMethods commonMethods = new CommonMethods();

        /// <summary>
        /// This test will make sure the https://graph.facebook.com/me?access_token= API has correct first_name
        /// </summary>
        [TestMethod]
        public void TestMe()
        {
            // get the service response from the facebook API
            string finalUrl = serviceUrl + string.Format("/me?access_token={0}", accessToken);
            var apiResponse = commonMethods.HttpGETResponse(finalUrl);

            Assert.IsTrue(apiResponse.Contains("\"first_name\":\"Pritam\""), "API response doesn't have the valid first_name value");
        }

        /// <summary>
        /// This method will compare the entire https://graph.facebook.com/me?access_token= JSON response with the stubbed data 
        /// </summary>
        [TestMethod]
        [DeploymentItem("TestData", "TestData")]
        public void TestMeEntireJsonResponse()
        {
            // read the stubbed input json that we have kept in the txt file
            StreamReader streamReader =
                    new StreamReader(Environment.CurrentDirectory + @"\TestData\MeApiResponse.txt");
            string stubbedJSON = streamReader.ReadToEnd();
            // replace dynamic properties of this JSON with empty GUID
            JsonComparer jsonComparer = new JsonComparer();
            string updatedStubbedJson = jsonComparer.ReplaceValues(stubbedJSON,
                new string[] { "updated_time", "timezone" });

            // get the service response from the facebook API
            string finalUrl = serviceUrl + string.Format("/me?access_token={0}", accessToken);
            var apiResponse = commonMethods.HttpGETResponse(finalUrl);
            // replace dynamic properties of this JSON with empty GUID
            string updatedAPIResponse = jsonComparer.ReplaceValues(apiResponse,
                new string[] { "updated_time", "timezone" });

            Assert.AreEqual(0, jsonComparer.Compare(updatedStubbedJson, updatedAPIResponse), "API response doesn't match");
        }

        /// <summary>
        /// This method will deserilize the json response from https://graph.facebook.com/me?access_token= and then will verify individual properties
        /// </summary>
        [TestMethod]
        public void TestMeWithSerialization()
        {
            // get the service response from the facebook API
            string finalUrl = serviceUrl + string.Format("/me?access_token={0}", accessToken);
            var apiResponse = commonMethods.HttpGETResponse(finalUrl);

            // deserialize this json to a class object
            MeResponse response = (MeResponse)commonMethods.Deserialize(apiResponse, typeof(MeResponse));

            //verify the json deserialized properly
            Assert.IsNotNull(response);

            // verify first name
            Assert.AreEqual("Pritam", response.first_name);

            // verify last name
            Assert.AreEqual("Karmakar", response.last_name);
            //...add all other verification
        }
    }
}
