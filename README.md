# Response_Caching_With_MediatR
Make your api's response speed max !!
All you need to write the code to implement response catching via mediatR.
Dont Forget to instal these packages in core folder:
MediatR.Extensions.Microsoft.DependencyInjection
Microsoft.Extensions.Caching.Abstractions
Microsoft.Extensions.Configuration.Abstractions
Microsoft.Extensions.Logging.Abstractions
Microsoft.Extensions.Options
Newtonsoft.Json

Once you request for get customers for instance id = 1 , second request is more faster than first request thanks to mediatR response catching operation. For Instance first request 1000ms second is 5ms !!!

