using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jp.in4a.CoreDocuNexus.Contracts.Http.AddPasswordToPdf
{
    /// <summary>
    /// PDFにパスワードを追加するリクエストのデータを格納します。
    /// </summary>
    public class AddPasswordToPdfRequest
    {
        /// <summary>
        /// パスワードを追加する対象のPDFファイル。
        /// バイト配列として表現されます。
        /// </summary>
        public byte[] PdfFile { get; set; } = Array.Empty<byte>();

        /// <summary>
        /// PDFを開く際に要求される閲覧用パスワード。
        /// </summary>
        public string ViewerPassword { get; set; } = string.Empty;

        /// <summary>
        /// 権限の変更など、PDFのセキュリティ設定を変更する際に要求される所有者パスワード。
        /// </summary>
        public string OwnerPassword { get; set; } = string.Empty;
    }
}
