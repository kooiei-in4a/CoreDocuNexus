using jp.in4a.CoreDocuNexus.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jp.in4a.CoreDocuNexus.Infrastructure.Pdf
{
    public class PdfSharpCoreEngine : IPdfEngine
    {
        public Task<Result<byte[]>> SetPasswordAsync(byte[] pdfData, string password)
        {
            // Dummy implementation for demonstration
            if (pdfData == null || pdfData.Length == 0)
                return Task.FromResult(Result<byte[]>.Failure("PDF data is empty."));
            if (string.IsNullOrWhiteSpace(password))
                return Task.FromResult(Result<byte[]>.Failure("Password cannot be empty."));

            Console.WriteLine($"[Infrastructure] Setting password for a PDF of {pdfData.Length} bytes.");
            // In a real scenario, you would use a PDF library to encrypt the file.
            // For this example, we just return the original data.
            return Task.FromResult(Result<byte[]>.Success(pdfData));
        }

        public Task<Result<byte[]>> MergePdfsAsync(IEnumerable<byte[]> pdfDataList)
        {
            // Dummy implementation for demonstration
            if (pdfDataList == null)
                return Task.FromResult(Result<byte[]>.Failure("PDF data list is null."));

            Console.WriteLine("[Infrastructure] Merging PDFs.");
            // Real implementation would merge PDFs here.
            // For now, let's just return the first PDF in the list.
            var firstPdf = new List<byte[]>(pdfDataList)[0];
            return Task.FromResult(Result<byte[]>.Success(firstPdf));
        }
    }
}
