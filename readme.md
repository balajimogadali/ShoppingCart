/*
To work with this project follow below steps:
a) This project consists of ShoppingCartApi and ShoppingCartApiTests

1. For this project i used local database , but before using please make sure we did migrations.
2. I used two Context i.e ApplicationDbContext and AuthDbContext targeting same database 
3. There are two roles are existed i.e Reader and Writer
4. If you want to use as writer we have RegisterAsWriter method
4. Make sure you run migration for both contexts.
5. I designed web api and written unit tests using XUnit.
6. Implemented Repository Pattern with DTO's.

//To work with api's we have to register with email and password. 
https://localhost:7145/api/Auth/register
//To generate JWT token , use this below api method 
https://localhost:7145/api/Auth/login
This method give response as below: 

{
  "userId": "e3dea82b-6a12-4048-ad28-f73570606e0a",
  "email": "a@gmail.com",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJhQGdtYWlsLmNvbSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlJlYWRlciIsImV4cCI6MTcxMTE5NTgxNywiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzE0NSIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjQyMDAifQ.083l5Qt0jyEauXMW3YFOeSu2kbEk2nz7ggmQIsbj8Qc",
  "roles": [
    "Reader"
  ]
}

//For remaining Domain api method need this jwt token , so pass it into Authentication Header
//Post, Delete,Put methods need Writer JWT token. 


*/
