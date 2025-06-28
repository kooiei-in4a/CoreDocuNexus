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

    public class SetPasswordHandler
    {
        private readonly IPdfEngine _pdfEngine;

        public SetPasswordHandler(IPdfEngine pdfEngine) // Dependency Injected
        {
            _pdfEngine = pdfEngine;
        }

        public async Task<Result<byte[]>> HandleAsync(AddPasswordToPdfRequest request)
        {
            try
            {
                // パスワードを設定する
                return await _pdfEngine.SetPasswordAsync(request.PdfFile, request.UserPassword, request.OwnerPassword,request.permissions);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(Result<byte[]>.Failure($"サーバーで予期せぬエラーが発生しました。\r\n{ex.Message}", -1));
                
            }
        }
    }
}
