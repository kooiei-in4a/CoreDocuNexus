using jp.in4a.CoreDocuNexus.Shared.Dto;
using PdfSharpCore.Pdf.IO;
using PdfSharpCore.Pdf.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace jp.in4a.CoreDocuNexus.Infrastructure.Pdf
{
    public class PdfSharpCoreEngine : IPdfEngine
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pdfData"></param>
        /// <param name="userPassword"></param>
        /// <param name="ownerPassword"></param>
        /// <returns></returns>
        public Task<Result<byte[]>> SetPasswordAsync(byte[] pdfData, string userPassword, string ownerPassword)
        {

            // 基本的なチェックを行う
            if (pdfData == null || pdfData.Length == 0)
                return Task.FromResult(Result<byte[]>.Failure("PDF data is empty.",-1));
            if (string.IsNullOrWhiteSpace(userPassword))
                return Task.FromResult(Result<byte[]>.Failure("Password cannot be empty.", -1));

            // パスワードが掛かっている場合エラー
            if (PdfUtils.IsPasswordProtected(pdfData))
            {
                return Task.FromResult(Result<byte[]>.Failure("PDF is already password protected.",-1));
            }

            var doc = PdfSharpCoreOpener.OpenPdfFile(pdfData, PdfDocumentOpenMode.Modify);
            var securitySettings = doc.SecuritySettings;
            securitySettings.DocumentSecurityLevel = PdfDocumentSecurityLevel.Encrypted128Bit; 
            securitySettings.UserPassword = userPassword;
            securitySettings.OwnerPassword = ownerPassword;

            // 4. 新しいメモリストリームに変更後のPDFを保存する
            using (var outputStream = new MemoryStream())
            {
                doc.Save(outputStream);
                doc.Close();

                // 5. バイナリデータを返す
                return Task.FromResult(Result<byte[]>.Success(outputStream.ToArray()));
            }

        }

        public Task<Result<byte[]>> MergePdfsAsync(IEnumerable<byte[]> pdfDataList)
        {
            // Dummy implementation for demonstration
            if (pdfDataList == null)
                return Task.FromResult(Result<byte[]>.Failure("PDF data list is null.",-1));

            Console.WriteLine("[Infrastructure] Merging PDFs.");
            // Real implementation would merge PDFs here.
            // For now, let's just return the first PDF in the list.
            var firstPdf = new List<byte[]>(pdfDataList)[0];
            return Task.FromResult(Result<byte[]>.Success(firstPdf));
        }
    }
}
