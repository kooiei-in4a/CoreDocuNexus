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

// ���𔻒肷��
var environment = DetermineEnvironment(builder.HostEnvironment.BaseAddress);
Console.WriteLine($"Detected environment: {environment}");

// ��{��appsettings.json��ǂݍ���
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

// ���ʂ�appsettings.{environment}.json��ǂݍ���
builder.Configuration.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: false);

var rootSettings = new RootSettings();
builder.Configuration.Bind(rootSettings);
builder.Services.AddSingleton(rootSettings);

//var rootSettings = new RootSettings();
//builder.Configuration.Bind(rootSettings);
//builder.Services.AddSingleton(rootSettings);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(rootSettings.ApiBaseUrl)});
builder.Services.AddScoped<PdfPasswordService>();


await builder.Build().RunAsync();


// �����胁�\�b�h
static string DetermineEnvironment(string baseAddress)
{
    if (string.IsNullOrEmpty(baseAddress))
        return "Development";

    var uri = new Uri(baseAddress);

    // URL�Ɋ�Â��Ċ��𔻒�
    return uri.Host switch
    {
        "thankful-stone-09975f100.2.azurestaticapps.net" => "Staging",
        "polite-pebble-0cc069800.1.azurestaticapps.net" => "Production",
        _ => "Development"
    };
}

public class EnvironmentInfo
{
    public string Name { get; set; } = "Development";
}