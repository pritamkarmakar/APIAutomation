using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using APIAutomation.DataModels;
using APIAutomation.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace APIAutomation
{
    /// <summary>
    /// Test class to validate the GET, POST and DELETE operations of the REST API
    /// </summary>
    [TestClass]
    public class APITests
    {
        private string serviceUrl = ConfigurationManager.AppSettings["Service_URL"];
        CommonMethods commonMethods = new CommonMethods();

        /// <summary>
        /// This test will make sure the http://localhost:55570/api/employee API returning correct data. We will compare the API response 
        /// with the stubbed JSON content (GetEmployee.txt) that we have kept in the project 'TestData' folder. In the API response 'Id' is dynamically 
        /// generated and we will replace that using empty GUID for final comparison
        /// 
        /// Note:- if we run this test after running the delete or post test then this will fail as the predefined content will not be able to match with API response. 
        /// But if you have any API that always return same static data you can use this kind of test. Here we are using the technique of comparing the raw JSON response
        /// from the API and making sure all the attributes are there with correct value
        /// </summary>
        [TestMethod]
        [DeploymentItem("TestData", "TestData")]
        public void TestGetEmployee()
        {
            JsonComparer jsonComparer = new JsonComparer();

            // get the service response from the facebook API
            string finalUrl = serviceUrl + "api/employee";
            var apiResponse = commonMethods.HttpGETResponse(finalUrl);
            // replace dynamic properties of this JSON with empty GUID
            string updatedAPIResponse = jsonComparer.ReplaceValues(apiResponse,
                new string[] { "Id"});

            // read the stubbed input json that we have kept in the txt file inside 'TestData' folder
            StreamReader streamReader =
                    new StreamReader(Environment.CurrentDirectory + @"\TestData\GetEmployee.txt");
            string stubbedJSON = streamReader.ReadToEnd();
            // replace dynamic properties of this JSON with empty GUID
            string updatedStubbedJson = jsonComparer.ReplaceValues(stubbedJSON,
                new string[] { "Id" });

            Assert.AreEqual(0, jsonComparer.Compare(updatedStubbedJson, updatedAPIResponse), "API response doesn't match. \n\nActual: {0} \n\nExpected: {1}", updatedAPIResponse, updatedStubbedJson);
        }

        /// <summary>
        /// This test will make sure the http://localhost:55570/api/employee/id API returning correct data
        /// Here we are using the technique of deserializing the API json response into C# class and then verifying the properties inside that
        /// </summary>
        [TestMethod]
        public void TestGetSpecificEmployee()
        {
            // get the service response from the facebook API
            string finalUrl = serviceUrl + "api/employee";
            var apiResponse = commonMethods.HttpGETResponse(finalUrl);

            // retrieve the Id of the first employee in the list. For that we will serialize the above API response in Employee list object
            Company company = (Company)commonMethods.Deserialize(apiResponse, typeof(Company));
            Employee firstEmployee = company.Content[0];

            if (firstEmployee != null)
            {
                var getEmployeeDetails =
                    commonMethods.HttpGETResponse(serviceUrl + string.Format("api/employee/{0}", firstEmployee.Id));
                // Deserialize the api response
                Employee employee = (Employee) commonMethods.Deserialize(getEmployeeDetails, typeof (Employee));

                // Verify API is returning correct employee details
                Assert.AreEqual(firstEmployee.Name, employee.Name, "Name doesn't match");
                Assert.AreEqual(firstEmployee.Role, employee.Role, "Role doesn't match");
            }
            else
            {
                Assert.Fail("No employee exist in the list");
            }
        }

        /// <summary>
        /// Test case to verify the post operation of the API
        /// </summary>
        [TestMethod]
        public void TestPostEmployee()
        {
            string finalUrl = serviceUrl + "api/employee";
            HttpContent httpContent =
                new StringContent("{\"Id\":\"27f7249f-36de-4a84-8a0a-9da598b1fdae\",\"Name\":\"Rishi Karmakar\",\"Role\":\"Program Manager\"}", Encoding.UTF8, "application/json");
            // make a http POST call to add new employee in the list
            var postApiResponse = commonMethods.HttpPostResponse(finalUrl, httpContent);

            // get current list of employees from the API
            var getApiResponse = commonMethods.HttpGETResponse(serviceUrl + "api/employee");
            // verify 'Rishi' is part of the employee list
            Assert.IsTrue(getApiResponse.Contains("{\"Id\":\"27f7249f-36de-4a84-8a0a-9da598b1fdae\",\"Name\":\"Rishi Karmakar\",\"Role\":\"Program Manager\"}"), "POST request failed to save new employee");
        }

        /// <summary>
        /// Test case to verify the delete operation of the API
        /// </summary>
        [TestMethod]
        public void TestDeleteEmployee()
        {
            // get list of employess using the GET API
            string finalUrl = serviceUrl + "api/employee";
            var apiResponse = commonMethods.HttpGETResponse(finalUrl);

            // We will retrieve the Id of the 1st employee from the list, and will delete its record using DELETE API. 
            // We will deserialize the above API response in Compnay list object and then will find the id
            Company company = (Company)commonMethods.Deserialize(apiResponse, typeof(Company));
            Employee employee = company.Content[0];

            if (employee != null)
            {
                // call the delete API
                commonMethods.HttpDeleteResponse(serviceUrl + string.Format("api/employee/{0}", employee.Id));
                // verify GET API doesn't have the deleted employee details
                var updatedApiResponse = commonMethods.HttpGETResponse(finalUrl);
                Assert.IsTrue(!updatedApiResponse.Contains(employee.Name),
                    "Delete operation failed. Deleted employee still in the GET employee API response");
            }
            else
            {
                Assert.Fail("Eemployee list is empty");
            }
        }
    }
}
