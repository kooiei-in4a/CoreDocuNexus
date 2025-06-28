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

        // 処理開始直後に追加
        _logger.LogInformation("Waiting 10 seconds to reduce server load...");
        await Task.Delay(10000);
        _logger.LogInformation("Wait period completed, starting processing.");

        try
        {
            // リクエストボディからJSONを読み取り、AddPasswordToPdfRequest型にデシリアライズ
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var request = JsonSerializer.Deserialize<AddPasswordToPdfRequest>(requestBody);

            if (request == null)
            {
                _logger.LogError("無効なリクエストです。リクエストボディをデシリアライズできませんでした。");
                return new BadRequestObjectResult(AddPasswordToPdfResponse.GetFailure(
                            "無効なリクエストです。リクエストボディをデシリアライズできませんでした。", 400));
            }

            // どちらかのパスワードが指定されていない場合はエラー
            if (string.IsNullOrWhiteSpace(request.UserPassword) && string.IsNullOrWhiteSpace(request.OwnerPassword))
            {
                _logger.LogError("閲覧パスワードとオーナーパスワードの両方が空です。");
                return new BadRequestObjectResult(AddPasswordToPdfResponse.GetFailure(
                            "閲覧パスワードとオーナーパスワードの両方が空です。", 400));
            }


            // 入力データの基本バリデーション
            if (request.PdfFile == null || request.PdfFile.Length == 0)
            {
                _logger.LogError("PDFファイルは必須です。");
                return new BadRequestObjectResult(AddPasswordToPdfResponse.GetFailure(
                            "PDFファイルは必須です。", 400));
            }

            // PDFパスワード追加処理
            var result = await _setPasswordHandler.HandleAsync(request);

            if (!result.IsSuccess)
            {
                _logger.LogInformation($"AddPasswordToPdf function fail.{result.Message}");
                return new BadRequestObjectResult(AddPasswordToPdfResponse.GetFailure(result.Message, result.StatusCode));
            }


            // 現在はリクエストのPDFファイルをそのまま返す
            var response = AddPasswordToPdfResponse.GetSuccess(result.Value);

            _logger.LogInformation("AddPasswordToPdf function completed successfully.");
            return new OkObjectResult(response);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON deserialization error occurred.");
            return new BadRequestObjectResult(
                               AddPasswordToPdfResponse.GetFailure("リクエストボディのJSON形式が無効です。", 400)
                           );

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred during PDF password addition.");
            return new StatusCodeResult(500);
        }
    }
}