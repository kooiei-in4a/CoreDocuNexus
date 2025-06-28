using jp.in4a.CoreDocuNexus.Contracts.Http.AddPasswordToPdf;
using jp.in4a.CoreDocuNexus.Features.PdfSetPassword;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace jp.in4a.CoreDocuNexus.Api;

public class AddPasswordToPdf
{
    private readonly ILogger<AddPasswordToPdf> _logger;
    private readonly SetPasswordHandler _setPasswordHandler;

    public AddPasswordToPdf(ILogger<AddPasswordToPdf> logger, SetPasswordHandler setPasswordHandler)
    {
        _logger = logger;
        _setPasswordHandler = setPasswordHandler;
    }

    [Function("AddPasswordToPdf")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
    {
        _logger.LogInformation("AddPasswordToPdf function started processing request.");

        // �����J�n����ɒǉ�
        _logger.LogInformation("Waiting 10 seconds to reduce server load...");
        await Task.Delay(10000);
        _logger.LogInformation("Wait period completed, starting processing.");

        try
        {
            // ���N�G�X�g�{�f�B����JSON��ǂݎ��AAddPasswordToPdfRequest�^�Ƀf�V���A���C�Y
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var request = JsonSerializer.Deserialize<AddPasswordToPdfRequest>(requestBody);

            if (request == null)
            {
                _logger.LogError("�����ȃ��N�G�X�g�ł��B���N�G�X�g�{�f�B���f�V���A���C�Y�ł��܂���ł����B");
                return new BadRequestObjectResult(AddPasswordToPdfResponse.GetFailure(
                            "�����ȃ��N�G�X�g�ł��B���N�G�X�g�{�f�B���f�V���A���C�Y�ł��܂���ł����B", 400));
            }

            // �ǂ��炩�̃p�X���[�h���w�肳��Ă��Ȃ��ꍇ�̓G���[
            if (string.IsNullOrWhiteSpace(request.UserPassword) && string.IsNullOrWhiteSpace(request.OwnerPassword))
            {
                _logger.LogError("�{���p�X���[�h�ƃI�[�i�[�p�X���[�h�̗�������ł��B");
                return new BadRequestObjectResult(AddPasswordToPdfResponse.GetFailure(
                            "�{���p�X���[�h�ƃI�[�i�[�p�X���[�h�̗�������ł��B", 400));
            }


            // ���̓f�[�^�̊�{�o���f�[�V����
            if (request.PdfFile == null || request.PdfFile.Length == 0)
            {
                _logger.LogError("PDF�t�@�C���͕K�{�ł��B");
                return new BadRequestObjectResult(AddPasswordToPdfResponse.GetFailure(
                            "PDF�t�@�C���͕K�{�ł��B", 400));
            }

            // PDF�p�X���[�h�ǉ�����
            var result = await _setPasswordHandler.HandleAsync(request);

            if (!result.IsSuccess)
            {
                _logger.LogInformation($"AddPasswordToPdf function fail.{result.Message}");
                return new BadRequestObjectResult(AddPasswordToPdfResponse.GetFailure(result.Message, result.StatusCode));
            }


            // ���݂̓��N�G�X�g��PDF�t�@�C�������̂܂ܕԂ�
            var response = AddPasswordToPdfResponse.GetSuccess(result.Value);

            _logger.LogInformation("AddPasswordToPdf function completed successfully.");
            return new OkObjectResult(response);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON deserialization error occurred.");
            return new BadRequestObjectResult(
                               AddPasswordToPdfResponse.GetFailure("���N�G�X�g�{�f�B��JSON�`���������ł��B", 400)
                           );

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred during PDF password addition.");
            return new StatusCodeResult(500);
        }
    }
}