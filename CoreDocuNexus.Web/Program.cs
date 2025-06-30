using jp.in4a.CoreDocuNexus.Web;
using jp.in4a.CoreDocuNexus.Web.AppSettings;
using jp.in4a.CoreDocuNexus.Web.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();

// シンプルに設定を読み込む（Azureでは常にProduction.jsonを使用）
var rootSettings = new RootSettings();
builder.Configuration.Bind(rootSettings);
builder.Services.AddSingleton(rootSettings);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(rootSettings.ApiBaseUrl)});
builder.Services.AddScoped<PdfPasswordService>();


await builder.Build().RunAsync();


