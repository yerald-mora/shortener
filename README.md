# Shortener
URL Shortener

To run the app using .NET CLI after cloning (Local SQL Server needed)

## Start API

```
cd .\shortener\nativoshortener.api\
dotnet build
dotnet ef database update
dotnet run
```
 


## Start webapp
```
cd .\shortener\nativoshortener.web\
dotnet run --urls=https://localhost:44366/
```

