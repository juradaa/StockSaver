using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PRO_APBD.Client;
using PRO_APBD.Client.Auth;
using PRO_APBD.Client.Services;
using Syncfusion.Blazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<AuthenticationStateProvider, StockAuthStateProvider>();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<IHttpService, HttpService>();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddSyncfusionBlazor();
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBaFt+QHJqVk1nQ1NEaV1CX2BZf1l3QWlbeE4QCV5EYF5SRHBdQlxgSHpRfkdiXns=;Mgo+DSMBPh8sVXJ1S0R+X1pDaV1CQmFJfFBmRGlYe1Ryc0UmHVdTRHRcQlthSX9Qd0FhUX1ddHA=;ORg4AjUWIQA/Gnt2VFhiQlJPcUBAXXxLflF1VWJTflp6dlJWACFaRnZdQV1lS35SdURnW3dcdXRR;MjQ3ODAxMUAzMjMxMmUzMDJlMzBoT2E1dk9WNjg4SXVIcFFoQnpXbE02OExUUDdPMDBPaDVkck1HT0lFUVlRPQ==;MjQ3ODAxMkAzMjMxMmUzMDJlMzBneU5BN1l4ZlFJSXhaN3o0OXQ4QW84N0FsajZsYVVGd0lZSlE1dFpMaGYwPQ==;NRAiBiAaIQQuGjN/V0d+Xk9HfVhdX2NWfFN0RnNYdV90fldBcDwsT3RfQF5jT3xTd0ZiXX1Wc3ZVQg==;MjQ3ODAxNEAzMjMxMmUzMDJlMzBZQTRnMTR4c3ROUHg0bmwvL1JwaHp0Z3NYK2gxOW5lU1o4OWcvMXMxRzE4PQ==;MjQ3ODAxNUAzMjMxMmUzMDJlMzBlQ0d3Smx1NWZpaDIycityOUhjNHkzYy9XNGZJMkZLdjFkWGlYanlFTm5FPQ==;Mgo+DSMBMAY9C3t2VFhiQlJPcUBAXXxLflF1VWJTflp6dlJWACFaRnZdQV1lS35SdURnW3ddeXRR;MjQ3ODAxN0AzMjMxMmUzMDJlMzBhczkyN2tRVEVHK09hdTlxSFc1dEdENkcxRFRhS054MWNGWTNNdVVMeWxjPQ==;MjQ3ODAxOEAzMjMxMmUzMDJlMzBsaU1HNSt2TldkVWt1RkxZa1VIZHZQSk16d2tzZnBEWHZmVVJ0UjBDM3djPQ==;MjQ3ODAxOUAzMjMxMmUzMDJlMzBZQTRnMTR4c3ROUHg0bmwvL1JwaHp0Z3NYK2gxOW5lU1o4OWcvMXMxRzE4PQ==");

await builder.Build().RunAsync();
