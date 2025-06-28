using jp.in4a.CoreDocuNexus.Shared.Dto;
using jp.in4a.CoreDocuNexus.Shared.Dto.Pdf;

namespace jp.in4a.CoreDocuNexus.Infrastructure.Pdf
{
    /// <summary>
    /// Abstract interface for the PDF processing engine.
    /// This separates the core logic from specific library implementations.
    /// </summary>
    public interface IPdfEngine
    {
        Task<Result<byte[]>> SetPasswordAsync(byte[] pdfData, string userPassword,string ownerPassword,DocumentPermissions permissions);
        Task<Result<byte[]>> MergePdfsAsync(IEnumerable<byte[]> pdfDataList);
    }
}
