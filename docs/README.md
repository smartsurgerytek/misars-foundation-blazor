# Misars Foundation App
## Prerequisite
- VS Code
- Power Shell 7
- .Net v2.1.5 or higher
- [.NET 8.0+ SDK](https://dotnet.microsoft.com/download/dotnet)
- [Node v18 or 20](https://nodejs.org/en)
## Quick Start
### Step 1. Installation
Install ABP Studio on your Windows PC from the [official website][[offical-abp-website-get-started]]
Use ABP CLI install ABP suite on yor windows
### Step 2. Registeration
Register an ABP Account from [official website][offical-abp-website-register].
### Step 3. Install ABP CLI and ABP suite

```pwsh
dotnet tool install -g Volo.ABP.Studio.Cli 
```
```pwsh
abp suite install
abp suite update 
#check the latest version 
```
### Step 4. Login from CLI
1. Login to ABP
```pwsh
abp login <username> -p <password>
```
### Step 5. Config `appsettings.json` ![status:draft]
```pwsh
code src\Acme.BookStore.DbMigrator\appsettings.json
```
Change 
```json
{
  "ConnectionStrings": {
  "Default": "Data Source=C:\\Users\\penoc\\repo\\misars-foundation-blazor.db;"

    ...
  },
  ...
}
```
to this:
```json
{
  "ConnectionStrings": {
    "Default": "Data Source=C:\\Users\\<username>\\repo\\misars-foundation-blazor.db;"
    ...
  },
  ...
}
```
Replace it with the path you downloaded and add `/`
### Step 6. Install Client-Side Libraries
```pwsh
abp install-libs
```
### Step 7. Run DbMigrator
```pwsh
dotnet run --project .\src\Acme.BookStore.DbMigrator -v n
```
If you have error, try cd to `.\src\Acme.BookStore.DbMigrator` and run this command
```pwsh
dotnet run
```
### Step 8. Run HttpApi.Host
```pwsh
dotnet run --project .\src\Acme.BookStore.HttpApi.Host
```
### Step 8. Run Blazor
```pwsh
dotnet run --project .\src\Acme.BookStore.Blazor
```
## Trobleshooting
### Issue - Step 7. Run DbMigrator [status:close]
```pwsh
. .\src\Acme.BookStore.DbMigrator\bin\Debug\net8.0\Acme.BookStore.DbMigrator.exe
```
### Issue - Step 8. Run HttpApi.Host [status:open]
When run
```
dotnet run --project .\src\Acme.BookStore.HttpApi.Host -v n
```
Should Expect:
```
[19:01:37 INF] User profile is available. Using 'C:\Users\penoc\AppData\Local\ASP.NET\DataProtection-Keys' as key repository and Windows DPAPI to encrypt keys at rest.
...
[19:01:39 INF] Now listening on: https://localhost:44388
...
[19:01:39 INF] Application started. Press Ctrl+C to shut down.
```
But Shows at the end:
```
Build succeeded.
    0 Warning(s)
    0 Error(s)
```

[offical-abp-website-get-started]: https://abp.io/get-started
[offical-abp-website-register]: https://account.abp.io/Account/Register
[status:draft]: https://img.shields.io/badge/draft-red
[status:close]: https://img.shields.io/badge/close-green
[status:open]: https://img.shields.io/badge/open-red