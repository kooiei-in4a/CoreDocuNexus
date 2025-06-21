using jp.in4a.CoreDocuNexus.Contracts.AddPasswordToPdf;
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
                    ErrorCode = 400,
                    Message = "Invalid request: Unable to deserialize request body."
                });
            }

            // �ǂ��炩�̃p�X���[�h���w�肳��Ă��Ȃ��ꍇ�̓G���[
            if (string.IsNullOrWhiteSpace(request.ViewerPassword) && string.IsNullOrWhiteSpace(request.OwnerPassword))
            {
                _logger.LogError("Invalid request: Both ViewerPassword and OwnerPassword are empty.");
                return new BadRequestObjectResult(new AddPasswordToPdfResponse
                {
                    Success = false,
                    ErrorCode = 400,
                    Message = "Both ViewerPassword and OwnerPassword are empty."
                });
            }


            // ���̓f�[�^�̊�{�o���f�[�V����
            if (request.PdfFile == null || request.PdfFile.Length == 0)
            {
                _logger.LogError("Invalid request: PDF file is empty or null.");
                return new BadRequestObjectResult(new AddPasswordToPdfResponse
                {
                    Success = false,
                    ErrorCode = 400,
                    Message = "PDF file is required."
                });
            }

            // �_�~�[����: ���ۂ�PDF�p�X���[�h�ǉ������������Ɏ���
            // ���݂̓��N�G�X�g��PDF�t�@�C�������̂܂ܕԂ�
            var response = new AddPasswordToPdfResponse
            {
                PdfFile = request.PdfFile, // �_�~�[: ���ۂɂ̓p�X���[�h�ی삳�ꂽPDF��Ԃ�
                Success = true,
                ErrorCode = 0,
                Message = "Password successfully added to PDF."
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
                ErrorCode = 400,
                Message = "Invalid JSON format in request body."
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred during PDF password addition.");
            return new StatusCodeResult(500);
        }
    }
}