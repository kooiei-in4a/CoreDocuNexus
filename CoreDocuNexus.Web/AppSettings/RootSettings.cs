namespace jp.in4a.CoreDocuNexus.Web.AppSettings
{
    public class RootSettings
    {
        public string AppName { get; set; } = string.Empty;

        public string AppDescription { get; set; } = string.Empty;

        public int MaxUploadFileSize { get; set; } = 10485760; // 10 MB

        /// <summary>
        /// アップロード最大ファイルサイズ（MB単位）
        /// </summary>
        public double MaxUploadFileSizeMB => MaxUploadFileSize / 1024.0 / 1024.0;

        public string ApiBaseUrl { get; set; } = string.Empty;


        public string EnvironmentName { get; set; } = string.Empty;

        public string ApiKey { get; set; } = string.Empty;

        public bool IsDevelopment => EnvironmentName.Equals("Development", StringComparison.OrdinalIgnoreCase);
        public bool IsStaging => EnvironmentName.Equals("Staging", StringComparison.OrdinalIgnoreCase);
        public bool IsProduction => EnvironmentName.Equals("Production", StringComparison.OrdinalIgnoreCase);

    }
}
