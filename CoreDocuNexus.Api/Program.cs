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
//                .ConfigureFunctionsWorkerDefaults() // �ŐV�̕������[�J�[�v���Z�X���f���̕W���ݒ�
//                .ConfigureServices(services =>
//                {
//                    // Application Insights �̐ݒ� (�I�v�V����)
//                    services.AddApplicationInsightsTelemetryWorkerService();
//                    services.ConfigureFunctionsApplicationInsights();

//                    // --- �������炪�ˑ�������(DI)�̐ݒ�ł� ---

//                    // Infrastructure�w�̓o�^
//                    // IPdfEngine���v�����ꂽ��APdfSharpEngine�̎��̂�n��
//                    services.AddScoped<IPdfEngine, PdfSharpCoreEngine>();

//                    // Feature Handlers�̓o�^
//                    // ����ɂ��AAPI�N���X�̃R���X�g���N�^�Ńn���h���𒍓��ł���悤�ɂȂ�
//                    services.AddScoped<SetPasswordHandler>();

//                })
//                .Build();

//            host.Run();
//        }
//    }
//}