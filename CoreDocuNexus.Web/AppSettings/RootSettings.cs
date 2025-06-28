namespace jp.in4a.CoreDocuNexus.Web.AppSettings
{
    public class RootSettings
    {
        public string ApiBaseUrl { get; set; } = string.Empty;

        public int MaxUploadFileSize { get; set; } = 10485760; // 10 MB

        /// <summary>
        /// アップロード最大ファイルサイズ（MB単位）
        /// </summary>
        public double MaxUploadFileSizeMB => MaxUploadFileSize / 1024.0 / 1024.0;
    }
}
