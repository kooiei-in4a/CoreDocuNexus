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

        try
        {
            // ���N�G�X�g�{�f�B����JSON��ǂݎ��AAddPasswordToPdfRequest�^�Ƀf�V���A���C�Y
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var request = JsonSerializer.Deserialize<AddPasswordToPdfRequest>(requestBody);

            if (request == null)
            {
                _logger.LogError("�����ȃ��N�G�X�g�ł��B���N�G�X�g�{�f�B���f�V���A���C�Y�ł��܂���ł����B");
                return new BadRequestObjectResult(new AddPasswordToPdfResponse
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "�����ȃ��N�G�X�g�ł��B���N�G�X�g�{�f�B���f�V���A���C�Y�ł��܂���ł����B"
                });
            }

            // �ǂ��炩�̃p�X���[�h���w�肳��Ă��Ȃ��ꍇ�̓G���[
            if (string.IsNullOrWhiteSpace(request.UserPassword) && string.IsNullOrWhiteSpace(request.OwnerPassword))
            {
                _logger.LogError("�{���p�X���[�h�ƃI�[�i�[�p�X���[�h�̗�������ł��B");
                return new BadRequestObjectResult(new AddPasswordToPdfResponse
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "�{���p�X���[�h�ƃI�[�i�[�p�X���[�h�̗�������ł��B"
                });
            }


            // ���̓f�[�^�̊�{�o���f�[�V����
            if (request.PdfFile == null || request.PdfFile.Length == 0)
            {
                _logger.LogError("PDF�t�@�C���͕K�{�ł��B");
                return new BadRequestObjectResult(new AddPasswordToPdfResponse
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "PDF�t�@�C���͕K�{�ł��B"
                });
            }

            // PDF�p�X���[�h�ǉ�����
            var result = await _setPasswordHandler.HandleAsync(request);


            // ���݂̓��N�G�X�g��PDF�t�@�C�������̂܂ܕԂ�
            var response = new AddPasswordToPdfResponse
            {
                PdfFile = request.PdfFile, // �_�~�[: ���ۂɂ̓p�X���[�h�ی삳�ꂽPDF��Ԃ�
                Success = true,
                StatusCode = 0,
                Message = "PDF�Ƀp�X���[�h������ɒǉ�����܂����B"
            };

            _logger.LogInformation("AddPasswordToPdf function completed successfully.");
            return new OkObjectResult(response);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON deserialization error occurred.");
            return new BadRequestObjectResult(new AddPasswordToPdfResponse
            {
                Success = false,
                StatusCode = 400,
                Message = "���N�G�X�g�{�f�B��JSON�`���������ł��B"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred during PDF password addition.");
            return new StatusCodeResult(500);
        }
    }
}