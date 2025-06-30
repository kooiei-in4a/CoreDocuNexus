using jp.in4a.CoreDocuNexus.Contracts.Http.AddPasswordToPdf;
using jp.in4a.CoreDocuNexus.Shared.Dto.Pdf;
using jp.in4a.CoreDocuNexus.Web.AppSettings;
using Microsoft.VisualBasic;
using System.Net.Http.Json;
using System.Runtime;
using System.Text;
using System.Text.Json;

namespace jp.in4a.CoreDocuNexus.Web.Services
{
    public class PdfPasswordService
    {
        public class ReturnValue
        {
            public byte[] PdfFile { get; set; } = Array.Empty<byte>();
            public string Message { get; set; } = string.Empty;
            public bool Success { get; set; }
        }

        private readonly RootSettings _settings;
        private readonly HttpClient _httpClient;
        private const string AddPasswordEndpoint = "api/AddPasswordToPdf"; 

        public PdfPasswordService(HttpClient httpClient,RootSettings settings)
        {
            _httpClient = httpClient;
            _settings = settings;
        }

        public async Task<ReturnValue> AddPasswordToPdfAsync(byte[] pdfFile, string userPassword, string ownerPassword,DocumentPermissions permissions)
        {
            // リクエスト作成ロジックをここに移動
            var request = new AddPasswordToPdfRequest
            {
                PdfFile = pdfFile,
                UserPassword = userPassword,
                OwnerPassword = ownerPassword,
                permissions = permissions
            };

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var endPointUrl = AddPasswordEndpoint;
            if (_settings.IsProduction || _settings.IsStaging)
            {
                // あくまでもAzure Functionを直接実行されないようにしているだけ
                endPointUrl += $"?code=fpfruICygcFnLUTOo-" + "CGGHMTb1tuhf87ePD_vTjpo7KFAzFu89jQEg==";
            }
            // API呼び出しとレスポンス検証ロジックをここに移動
            var response = await _httpClient.PostAsync(endPointUrl, content);

            // 成功しなかった場合は例外を投げる
            if (!response.IsSuccessStatusCode)
            {
                // ここで詳細なエラー処理も可能
                throw new HttpRequestException($"API request failed with status code {response.StatusCode}");
            }

            var responseJson = await response.Content.ReadFromJsonAsync<AddPasswordToPdfResponse>();

            var returnValue = CheckResponse(response, responseJson);

            if (returnValue.Success)
            {
                returnValue.PdfFile = responseJson!.PdfFile;
            }

            return returnValue;

        }

        private ReturnValue CheckResponse(HttpResponseMessage response, AddPasswordToPdfResponse? responseJson)
        {
            var returnValue = new ReturnValue();

            // レスポンスがnullまたは成功フラグがfalseの場合はエラー
            if (response.IsSuccessStatusCode)
            {
                if (responseJson == null)
                {
                    returnValue.Message = "APIからのレスポンスが無効です。";
                    returnValue.Success = false;
                    return returnValue;
                }
                if (!responseJson.Success)
                {
                    returnValue.Message = $"APIエラー: {responseJson.Message}";
                    returnValue.Success = false;
                    return returnValue;
                }

                returnValue.Success = true;
                return returnValue;
            }
            else
            {
                // 400エラーの場合、レスポンスボディにエラー情報が含まれる
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    returnValue.Message = "400エラー: APIエラー: 無効なリクエストです。";
                    returnValue.Success = false;
                    return returnValue;
                }
                // 500エラーの場合、レスポンスボディは空
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    returnValue.Message = "500エラー: サーバー内部エラー";
                    returnValue.Success = false;
                    return returnValue;
                }
                else
                {
                    returnValue.Message = $"{(int)response.StatusCode}エラー: {response.ReasonPhrase}";
                    returnValue.Success = false;
                    return returnValue;
                }
            }
        }
    }
}
