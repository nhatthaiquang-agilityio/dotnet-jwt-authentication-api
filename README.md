# ASP NET Core Jwt Authentication API
ASP NET Core API + MongoDB: JWT Authentication
+ Register User
+ Render Jwt Token
+ Using Jwt Token for Getting All Users

### Requirements
+ Docker Compose
+ ASP NET Core 2.1
+ MongoDB
+ Jwt Authentication Token

### Issues
+ 'HS256' requires the SecurityKey.KeySize to be greater than '128' bits
    - Fixed: Increase the length of secret in appsettings


### Usage
+ Start MongoDB
```
cd devops
docker-compose up
```

+ Start Web API in Visual Studio

### Demo
+ Create user
```
POST http://localhost:5000/api/users/register

Body:
{
	"firstname": "nhat",
	"lastname": "thai",
	"username": "nhatthai",
	"password": "nhatthai"
}

Response:
{
    "id": "5ce66ed038a2a505e2b4dad8",
    "firstName": "nhat",
    "lastName": "thai",
    "username": "nhatthai",
    "password": "$MYHASH$V1$10000$vSfoOqi+JBZr+ccF+y6UeFrTA310JJ2E0kRliwBKA/30Ee+d",
    "token": null
}
```

+ Authenticate User and Generate JWT Token
```
POST http://localhost:5000/api/users/authenticate
Body:
{
    "username": "nhatthai",
	"password": "nhatthai"
}

Response:
{
    "id": "5ce66ed038a2a505e2b4dad8",
    "firstName": "nhat",
    "lastName": "thai",
    "username": "nhatthai",
    "password": null,
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjVjZTY2ZWQwMzhhMmE1MDVlMmI0ZGFkOCIsIm5iZiI6MTU1ODYwNjU0NCwiZXhwIjoxNTU5MjExMzQ0LCJpYXQiOjE1NTg2MDY1NDR9.8BIW5ITdy-BC_8jbcQ-nAL3pTZ65-XMxmcIH4VN7m8s"
}
```

+ Get all users
```
GET http://localhost:5000/api/users
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjVjZTY2ZWQwMzhhMmE1MDVlMmI0ZGFkOCIsIm5iZiI6MTU1ODYwNjU0NCwiZXhwIjoxNTU5MjExMzQ0LCJpYXQiOjE1NTg2MDY1NDR9.8BIW5ITdy-BC_8jbcQ-nAL3pTZ65-XMxmcIH4VN7m8s

Response:
[
    {
        "id": "5ce66ed038a2a505e2b4dad8",
        "firstName": "nhat",
        "lastName": "thai",
        "username": "nhatthai",
        "password": "$MYHASH$V1$10000$vSfoOqi+JBZr+ccF+y6UeFrTA310JJ2E0kRliwBKA/30Ee+d",
        "token": null
    }
]
```

### Reference
+ [AspNetCore Jwt Authentication API](https://jasonwatmore.com/post/2018/08/14/aspnet-core-21-jwt-authentication-tutorial-with-example-api)
+ [Hash Algorithm](https://codinginfinite.com/c-sharp-hashing-algorithm-class-asp-net-core/)