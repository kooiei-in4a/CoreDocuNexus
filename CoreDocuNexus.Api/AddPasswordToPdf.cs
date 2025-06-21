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
            // リクエストボディからJSONを読み取り、AddPasswordToPdfRequest型にデシリアライズ
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

            // どちらかのパスワードが指定されていない場合はエラー
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


            // 入力データの基本バリデーション
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

            // ダミー処理: 実際のPDFパスワード追加処理をここに実装
            // 現在はリクエストのPDFファイルをそのまま返す
            var response = new AddPasswordToPdfResponse
            {
                PdfFile = request.PdfFile, // ダミー: 実際にはパスワード保護されたPDFを返す
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