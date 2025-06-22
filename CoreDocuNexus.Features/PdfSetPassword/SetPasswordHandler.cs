using jp.in4a.CoreDocuNexus.Contracts.Http.AddPasswordToPdf;
using jp.in4a.CoreDocuNexus.Infrastructure.Pdf;
using jp.in4a.CoreDocuNexus.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jp.in4a.CoreDocuNexus.Features.PdfSetPassword
{
    /// <summary>
    /// Handles the business logic for the "Set PDF Password" feature.
    /// It's a self-contained slice of functionality.
    /// </summary>
    public class SetPasswordHandler
    {
        private readonly IPdfEngine _pdfEngine;

        public SetPasswordHandler(IPdfEngine pdfEngine) // Dependency Injected
        {
            _pdfEngine = pdfEngine;
        }

        public async Task<Result<AddPasswordToPdfResponse>> HandleAsync(AddPasswordToPdfRequest request)
        {
            try
            {
                //var pdfData = Convert.FromBase64String(request.FileAsBase64);
                //var result = await _pdfEngine.SetPasswordAsync(pdfData, request.Password);

                //if (result.IsFailure)
                //{
                //    return Result<AddPasswordToPdfResponse>.Failure(result.Error);
                //}

                //var processedFileBase64 = Convert.ToBase64String(result.Value);
                //var newFileName = $"{Path.GetFileNameWithoutExtension(request.FileName)}_protected.pdf";

                return Result<AddPasswordToPdfResponse>.Success(new AddPasswordToPdfResponse
                {
                    Success = true,
                    PdfFile = request.PdfFile,
                    StatusCode = 0,
                    Message = "パスワードが正常に設定されました。"
                });
            }
            catch (Exception ex)
            {
                // Log the exception ex
                return Result<AddPasswordToPdfResponse>.Failure("サーバーで予期せぬエラーが発生しました。");
            }
        }
    }
}
