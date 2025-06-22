using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using PdfSharpCore.Pdf.IO.enums;


namespace jp.in4a.CoreDocuNexus.Infrastructure.Pdf
{
    internal class PdfSharpCoreOpener
    {
        public static PdfDocument OpenPdfFile(byte[] pdfBinary)
        {
            return OpenPdfFile(pdfBinary, null, null, PdfDocumentOpenMode.ReadOnly, PdfReadAccuracy.Strict);
        }

        public static PdfDocument OpenPdfFile(byte[] pdfBinary, string? password)
        {
            return OpenPdfFile(pdfBinary, password, password, PdfDocumentOpenMode.ReadOnly, PdfReadAccuracy.Strict);
        }
        
        public static PdfDocument OpenPdfFile(byte[] pdfBinary, PdfDocumentOpenMode pdfDocumentOpenMode)
        {
            return OpenPdfFile(pdfBinary, null, null, pdfDocumentOpenMode, PdfReadAccuracy.Strict);
        }

        public static PdfDocument OpenPdfFile(byte[] pdfBinary, string? password, PdfDocumentOpenMode pdfDocumentOpenMode)
        {
            return OpenPdfFile(pdfBinary, password, password, pdfDocumentOpenMode, PdfReadAccuracy.Strict);
        }


        public static PdfDocument OpenPdfFile(byte[] pdfBinary, PdfDocumentOpenMode pdfDocumentOpenMode, PdfReadAccuracy pdfReadAccuracy)
        {
            return OpenPdfFile(pdfBinary, null, null, pdfDocumentOpenMode, pdfReadAccuracy);
        }

        public static PdfDocument OpenPdfFile(byte[] pdfBinary, string? password, PdfDocumentOpenMode pdfDocumentOpenMode, PdfReadAccuracy pdfReadAccuracy)
        {
            return OpenPdfFile(pdfBinary, password, password, pdfDocumentOpenMode, pdfReadAccuracy);
        }

        public static PdfDocument OpenPdfFile(
                byte[] pdfBinary,
                string? userPassword,
                string? ownerPassword,
                PdfDocumentOpenMode pdfDocumentOpenMode,
                PdfReadAccuracy pdfReadAccuracy)
        {
            // PDFをメモリに取り込む
            using (var inputStream = new MemoryStream(pdfBinary))
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(ownerPassword))
                    {
                        return PdfReader.Open(inputStream, ownerPassword, pdfDocumentOpenMode, pdfReadAccuracy);
                    }
                    else if (!string.IsNullOrWhiteSpace(userPassword))
                    {
                        return PdfReader.Open(inputStream, userPassword, pdfDocumentOpenMode, pdfReadAccuracy);
                    }
                    else
                    {
                        return PdfReader.Open(inputStream, pdfDocumentOpenMode, pdfReadAccuracy);
                    }
                }
                catch (PdfReaderException ex)
                {
                    throw new InvalidOperationException("PDFファイルが大きすぎてメモリに読み込めません。", ex);
                }
                catch (OutOfMemoryException ex)
                {
                    throw new InvalidOperationException("PDFファイルが大きすぎてメモリに読み込めません。", ex);
                }
                catch (IOException ex)
                {
                    throw new InvalidOperationException("PDFファイルの処理中にI/Oエラーが発生しました。", ex);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("パスワード解除処理中に予期しないエラーが発生しました。", ex);
                }
            }

        }
    }
}
