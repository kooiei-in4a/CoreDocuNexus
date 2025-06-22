using jp.in4a.CoreDocuNexus.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jp.in4a.CoreDocuNexus.Contracts.Http.AddPasswordToPdf
{
    /// <summary>
    /// PDFへのパスワード追加処理の結果を格納します。
    /// </summary>
    public class AddPasswordToPdfResponse
    {
        /// <summary>
        /// 処理後のPDFファイル。
        /// パスワードが設定されています。
        /// </summary>
        public byte[] PdfFile { get; set; } = Array.Empty<byte>();

        /// <summary>
        /// 処理が成功したかどうかを示します。
        /// trueの場合、成功です。
        /// </summary>
        public bool Success { get; set; } = false;

        /// <summary>
        /// エラーコードを示します。
        /// 0は成功を意味し、それ以外は特定のエラーを示します。
        /// </summary>
        public int StatusCode { get; set; } = 0; // 0 for success, non-zero for error codes

        /// <summary>
        /// 処理結果に関する追加メッセージ。
        /// エラー発生時には、その詳細が含まれます。
        /// </summary>
        public string Message { get; set; } = string.Empty;

        public AddPasswordToPdfResponse(bool success, byte[] pdfFile, string message, int statusCode)
        {
            Success = success;
            PdfFile = pdfFile ?? Array.Empty<byte>();
            Message = message ?? string.Empty;
            StatusCode = statusCode;
        }

        public static AddPasswordToPdfResponse GetSuccess(byte[] PdfFile) => new AddPasswordToPdfResponse(true, PdfFile, string.Empty, 0);

        public static AddPasswordToPdfResponse GetFailure(string error, int code) => new AddPasswordToPdfResponse(false, Array.Empty<byte>(), error, code);
    }
}
