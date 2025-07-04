To launch your project create this appsettings.json file:
```{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "RecordDatabase": "<your connection string>"
  }
}
```

### Test Results:
The test lasted for 90mins.
For this test I got 7.3/10
Those were the mistakes:
1. Filter doesn't work (didn't have time to implement)
2. We don't return 404 for getting all resources (oopsie-daisy)
3. Sorting by newest date doesn't work (because I'm stupid, 2 second fix)
4. One of the required request bodies does not work (because I'm forcing insertion with the given id, although i set the database to generate id's)
5. Cathing and throwing a completely new exception, thus deleting the stack trace (I got spanked hard for this, will never do this again)
6. Not everything is validated in the request
7. Wrong service lifetime (overlooked that part)

Overall - a lot of stupid mistakes that altogether can be fixed in 5mins...
