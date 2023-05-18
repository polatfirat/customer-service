# customer-service
Customer service with In-Memory database

Default API Endpoints

Get : https://localhost:7122/api/v1/customer

Get By Id : https://localhost:7122/api/v1/customer/{customerId}

Get With Filter : https://localhost:7122/api/v1/customer/search?FirstName={firstName}&LastName={lastName}&Age={age}&Address={address}

Post : https://localhost:7122/api/v1/customer
      Body Example : 
      {
          "FirstName":"TestName",
          "LastName" : "TestLastName",
          "Age":20,
          "Address":"Test Address"
      }
      
      
Put : https://localhost:7122/api/v1/customer
      Body Example : 
      {
          "Id": {customerId}
          "FirstName":"Update_TestName",
          "LastName" : "Update_TestLastName",
          "Age":30,
          "Address":"Update_Test Address"
      }
      
Delete : https://localhost:7122/api/v1/customer/{customerId}


