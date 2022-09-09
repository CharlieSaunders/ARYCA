# ARYCA 
(A)ny (R)equest (Y)ou (C)ould (A)sk

PWA that implements ideas or things that I am learning/developing.

## Tech 
* **Hosting:** Docker
* **Client:** C# WASM (W/Server), Blazor, Bootstrap, Blazored, ApexCharts
* **Server:** ASP.Net Core Web API (.Net 6.0)
* **DB:** SqLite
* **ORM/Helpers:** EF 6, SonarLint, LibMan, 
* **Auth:** JWT
* **Test:** Nunit, Moq

## The Why?
When developing I find it beneficial to implement concepts in different use cases. 
I would read or watch something then look at how I can implement this effectively.
Treating this app like a commercial app allows me to always remain focused on not just the implementation of the concept
but also how this impacts the end user.

## Run Locally
You can run the application via Docker or through the terminal. 

### Docker
To run the application:
Inside the solution folder '~/ARYCA'' run the below
```
docker-compose up --force-recreate --build -d
```

This will create the required containers for the apps.

### VSCode/Terminal
For Services - this will launch the API with your machines IP address:
```
cd ~ARYCA/Services
dotnet run
```

For Client - this will then run the Client on localhost:5001:
```
cd ~ARYCA/Client
./LocalLaunch.ps1
```

You may have to create the db (depends on if the ARYCA.db is present in the Services directory):
```
cd ~ARYCA/Services
dotnet ef migrations add myNewMigration
dotnet ef database update
```
If you have run a migration then a user will need to be created

POST public/users
```
Request:
{
	"Username": "string",
	"FirstName": "string",
	"SecondName": "string",
	"Role": "string"
}
```
^Role: "Admin", "Standard"

Access the container for the client from Docker Desktop (or if you know the URL for the app from Docker) or via http://localhost:5001/Login (on your machine).

## Assets

### Icons
 * https://game-icons.net/
 * https://useiconic.com/open

### Images/Svgs
 * https://www.svgrepo.com
 * https://www.flaticon.com/

### API's
 * https://api2.binance.com/api/v3
 * https://exchange.blockchain.com/api/

### Diagrams
 * https://whimsical.com/

### Docker Network
![Docker Network](DockerNetwork.PNG?raw=true "Docker Network Diagram")