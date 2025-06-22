using PdfSharpCore.Pdf.IO;
using PdfSharpCore.Pdf.IO.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jp.in4a.CoreDocuNexus.Infrastructure.Pdf
{
    internal class PdfUtils
    {
        public static bool IsPasswordProtected(byte[] pdfBinary)
        {
            try
            {
                using (var inputStream = new MemoryStream(pdfBinary))
                {
                    PdfReader.Open(inputStream, PdfDocumentOpenMode.ReadOnly, PdfReadAccuracy.Strict);
                    return false; // PDFが開けた場合、パスワード保護されていない
                }
            }
            catch (PdfReaderException ex) when (ex.Message.Contains("パスワード") || ex.Message.Contains("password"))
            {
                return true;
            }
        }


        public static void PdfBinCheck(byte[] pdfBinary)
        {
            // 入力検証
            if (pdfBinary == null || pdfBinary.Length == 0)
                throw new ArgumentException("PDFバイナリデータが無効です。", nameof(pdfBinary));
            // PDFヘッダーの簡易チェック
            if (pdfBinary.Length < 8 || !System.Text.Encoding.ASCII.GetString(pdfBinary, 0, 4).Equals("%PDF"))
                throw new ArgumentException("有効なPDFファイルではありません。", nameof(pdfBinary));
        }
    }
}
