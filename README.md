# ASP-Web-API-multi-layer

## Run the app

```bash
$ dotnet WebApi/bin/Debug/net6.0/WebApi.dll
```

## About

### Architecrure: `Classical Multilayer Architecture`

### Stack:

![ASP.NET](https://img.shields.io/badge/ASP.NET%20Core%206%20-blueviolet?style=for-the-badge&logo=dotnet)
![EF Core](https://img.shields.io/badge/EF%20Core%206%20-informational?style=for-the-badge&logo=dotnet)
![AutoMapper](https://img.shields.io/badge/AutoMapper%20-orange?style=for-the-badge)
![LINQ](https://img.shields.io/badge/LINQ%20-yellowgreen?style=for-the-badge&logo=dotnet)
![SQLite](https://img.shields.io/badge/sqlite-%2307405e.svg?style=for-the-badge&logo=sqlite&logoColor=white)
![Swagger](https://img.shields.io/badge/-Swagger-%23Clojure?style=for-the-badge&logo=swagger&logoColor=white)
![Azure](https://img.shields.io/badge/azure-%230072C6.svg?style=for-the-badge&logo=microsoftazure&logoColor=white)

### Domain design
- We have `teams`, where `employes` are doing different `tasks`.
- Every team have a `team lead` and every task is contained in `sprint` _(time span in which employees have tasks to complete)_.
- Team lead have to generate `report` after each sprint where all employee's `task changes` during current sprint will be. 

### Layers
- [x] **Domain** layer
  - Rich Domain Model

- [x] **Data access** layer
  - EF Core 6
  - SQLite
  - Transactions

- [x] **Services** layer
  - Async/Await
  - LINQ

- [x] **WebApi** layer
  - AutoMapper
  - MiddleWares _(i.e. Caching && Simple Authorization)_
  - Swagger

<br>

> #### Other features:
- [x] _`Application`_ is hosted on [_`Azure server`_](https://azure.microsoft.com/en-gb/)
- [x] Beautiful documentation in README.md :)

<br>

> ### Try out my application [here](https://lipa-reports.azurewebsites.net)
