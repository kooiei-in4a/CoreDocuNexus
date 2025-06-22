using jp.in4a.CoreDocuNexus.Contracts.Http.AddPasswordToPdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace jp.in4a.CoreDocuNexus.Api;

public class AddPasswordToPdf
{
    private readonly ILogger<AddPasswordToPdf> _logger;

    public AddPasswordToPdf(ILogger<AddPasswordToPdf> logger)
    {
        _logger = logger;
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
                _logger.LogError("Invalid request: Unable to deserialize request body.");
                return new BadRequestObjectResult(new AddPasswordToPdfResponse
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "�����ȃ��N�G�X�g�ł��B���N�G�X�g�{�f�B���f�V���A���C�Y�ł��܂���ł����B"
                });
            }

            // �ǂ��炩�̃p�X���[�h���w�肳��Ă��Ȃ��ꍇ�̓G���[
            if (string.IsNullOrWhiteSpace(request.ViewerPassword) && string.IsNullOrWhiteSpace(request.OwnerPassword))
            {
                _logger.LogError("Invalid request: Both ViewerPassword and OwnerPassword are empty.");
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
                _logger.LogError("Invalid request: PDF file is empty or null.");
                return new BadRequestObjectResult(new AddPasswordToPdfResponse
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "PDF�t�@�C���͕K�{�ł��B"
                });
            }

            // �_�~�[����: ���ۂ�PDF�p�X���[�h�ǉ������������Ɏ���
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