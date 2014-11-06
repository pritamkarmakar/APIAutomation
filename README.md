APIAutomation
=============

This is a test suite to verify the functionalities of a RESTful web service.

###Where we can use this project:
Integrate this type of test suite into your continuous delivery pipeline and trigger all your tests after every new build and make sure there is no regression and your REST APIs are working as before. 

###Tools used: 
* Visual Studio 2013 
* [Fiddler](http://www.telerik.com/fiddler)

###What we are going to test:
We are going to test [this](https://github.com/pritamkarmakar/REST-API) REST API. The REST API has following methods -
* Get all employee list (GET) - ../api/employee
* Get details of a specific employee (GET) - ../api/employee/{id}
* Add a new employee (POST) - ../api/employee
* Delete an existing employee (DELETE) - ../api/Employee/{id}

So far I have just added only one test per method and haven't covered other aspects of the testing like invalid input, corner case, edge case etc. Please make sure to cover those scenarios when you will create your own test suite.

###Mechanism to verify the API response:
There are 2 different approaches of verifying a REST API response -

1. **Using static JSON/XML content** - if we know our API going to return fixed response for a particular given input then we can validate the response using a pre existing JSON/XML. We have to call that API from Fiddler and have to grab the response. Once we have it copy that content into a text file (if the response is in JSON format) or in XML format. During the test we will verify API response matches the static content that we kept earlier.
2. **Deserialize the JSON to class** - in this approach we will convert the API JSON response to C# class object. And from this object will read the required parameters. This is useful when your API response changing constantly and you want to make sure after doing a POST/DELETE operation changes are there.

###Components that we have in this test project:
* APITests.cs - this class contains all the test methods
* TestData - this folder contains the stubbed API response, that we will use in the 'Using static JSON/XML content' mechanism
* Helper > CommonMethods.cs - this class has method to make GET, POST, DELETE operation to any API. We will reuse this methods across different tests
* Helper > JsonComparer.cs - this class can remove the dynamic parts of a JSON string to make a comparison. For example - we want to verify our REST API is always returning correct response for a particular employee. This is a static response but inside that response we have some properties that contains datetime datatype and this is dynamic. So we have to replace the datetime value to some fixed value so that we can make a final comparison with the stubbed data. This class can do that, for more details please check [here](https://github.com/pritamkarmakar/JSONComparer)
* DataModels > DataModels.cs - if we are going to verify the API response using the 2nd approach 'Deserialize the JSON to class' then we need to create the class representation of the JSON response. There are plenty of online websites which can help us to get the class representation of a JSON content. My favorite is http://json2csharp.com/. Paste your JSON in the text field of this website and it will give you the class representation. Make sure to give a good name of the class and put that into DataModels.cs file.


That's it, now you can open this project and see the tests. I tried my best to add as much comments possible in each test, but if anything not clear feel free to drop me email (contact information in my profile). Hope this will be helpful.
