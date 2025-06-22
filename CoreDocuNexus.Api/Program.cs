using jp.in4a.CoreDocuNexus.Features.PdfSetPassword;
using jp.in4a.CoreDocuNexus.Infrastructure.Pdf;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

//builder.Services
//    .AddApplicationInsightsTelemetryWorkerService()
//    .ConfigureFunctionsApplicationInsights();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights()
    .AddScoped<IPdfEngine, PdfSharpCoreEngine>()
    .AddScoped<SetPasswordHandler>();

builder.Build().Run();

//namespace CoreDocuNexus.Api
//{

//    using jp.in4a.CoreDocuNexus.Features.PdfSetPassword;
//    using jp.in4a.CoreDocuNexus.Infrastructure.Pdf;
//    using Microsoft.Azure.Functions.Worker;
//    using Microsoft.Extensions.DependencyInjection;
//    using Microsoft.Extensions.Hosting;

//    public class Program
//    {
//        public static void Main()
//        {
//            var host = new HostBuilder()
//                .ConfigureFunctionsWorkerDefaults() // 最新の分離ワーカープロセスモデルの標準設定
//                .ConfigureServices(services =>
//                {
//                    // Application Insights の設定 (オプション)
//                    services.AddApplicationInsightsTelemetryWorkerService();
//                    services.ConfigureFunctionsApplicationInsights();

//                    // --- ここからが依存性注入(DI)の設定です ---

//                    // Infrastructure層の登録
//                    // IPdfEngineが要求されたら、PdfSharpEngineの実体を渡す
//                    services.AddScoped<IPdfEngine, PdfSharpCoreEngine>();

//                    // Feature Handlersの登録
//                    // これにより、APIクラスのコンストラクタでハンドラを注入できるようになる
//                    services.AddScoped<SetPasswordHandler>();

//                })
//                .Build();

//            host.Run();
//        }
//    }
//}