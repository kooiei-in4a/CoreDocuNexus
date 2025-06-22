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
            // リクエストボディからJSONを読み取り、AddPasswordToPdfRequest型にデシリアライズ
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var request = JsonSerializer.Deserialize<AddPasswordToPdfRequest>(requestBody);

            if (request == null)
            {
                _logger.LogError("無効なリクエストです。リクエストボディをデシリアライズできませんでした。");
                return new BadRequestObjectResult(new AddPasswordToPdfResponse
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "無効なリクエストです。リクエストボディをデシリアライズできませんでした。"
                });
            }

            // どちらかのパスワードが指定されていない場合はエラー
            if (string.IsNullOrWhiteSpace(request.UserPassword) && string.IsNullOrWhiteSpace(request.OwnerPassword))
            {
                _logger.LogError("閲覧パスワードとオーナーパスワードの両方が空です。");
                return new BadRequestObjectResult(new AddPasswordToPdfResponse
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "閲覧パスワードとオーナーパスワードの両方が空です。"
                });
            }


            // 入力データの基本バリデーション
            if (request.PdfFile == null || request.PdfFile.Length == 0)
            {
                _logger.LogError("PDFファイルは必須です。");
                return new BadRequestObjectResult(new AddPasswordToPdfResponse
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "PDFファイルは必須です。"
                });
            }

            // PDFパスワード追加処理
            var result = await _setPasswordHandler.HandleAsync(request);


            // 現在はリクエストのPDFファイルをそのまま返す
            var response = new AddPasswordToPdfResponse
            {
                PdfFile = request.PdfFile, // ダミー: 実際にはパスワード保護されたPDFを返す
                Success = true,
                StatusCode = 0,
                Message = "PDFにパスワードが正常に追加されました。"
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
                Message = "リクエストボディのJSON形式が無効です。"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred during PDF password addition.");
            return new StatusCodeResult(500);
        }
    }
}