namespace jp.in4a.CoreDocuNexus.Web.AppSettings
{
    /// <summary>
    /// アプリケーションの基本的な設定を管理します。
    /// </summary>
    public class RootSettings
    {
        /// <summary>
        /// アプリケーション名を取得または設定します。
        /// </summary>
        public string AppName { get; set; } = string.Empty;

        /// <summary>
        /// アプリケーションの概要説明を取得または設定します。
        /// </summary>
        public string AppDescription { get; set; } = string.Empty;

        /// <summary>
        /// アップロード可能なファイルの最大サイズをバイト単位で取得または設定します。
        /// デフォルトは 10,485,760 バイト (10 MB) です。
        /// </summary>
        public int MaxUploadFileSize { get; set; } = 10485760; // 10 MB

        /// <summary>
        /// アップロード可能なファイルの最大サイズをメガバイト(MB)単位で取得します。
        /// </summary>
        /// <remarks>
        /// このプロパティは <see cref="MaxUploadFileSize"/> の値を変換して提供します。
        /// </remarks>
        public double MaxUploadFileSizeMB => MaxUploadFileSize / 1024.0 / 1024.0;

        /// <summary>
        /// 通信するAPIサーバーのベースURLを取得または設定します。
        /// </summary>
        public string ApiBaseUrl { get; set; } = string.Empty;

        /// <summary>
        /// アプリケーションの実行環境名 (例: "Development", "Staging", "Production") を取得または設定します。
        /// </summary>
        public string EnvironmentName { get; set; } = string.Empty;

        /// <summary>
        /// 外部サービスとの連携に使用するAPIキーを取得または設定します。
        /// </summary>
        public string ApiKey { get; set; } = string.Empty;

        /// <summary>
        /// 現在の実行環境が開発環境 ("Development") かどうかを示します。
        /// </summary>
        /// <remarks>
        /// 環境名の比較は大文字・小文字を区別しません。
        /// </remarks>
        public bool IsDevelopment => EnvironmentName.Equals("Development", StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// 現在の実行環境がステージング環境 ("Staging") かどうかを示します。
        /// </summary>
        /// <remarks>
        /// 環境名の比較は大文字・小文字を区別しません。
        /// </remarks>
        public bool IsStaging => EnvironmentName.Equals("Staging", StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// 現在の実行環境が本番環境 ("Production") かどうかを示します。
        /// </summary>
        /// <remarks>
        /// 環境名の比較は大文字・小文字を区別しません。
        /// </remarks>
        public bool IsProduction => EnvironmentName.Equals("Production", StringComparison.OrdinalIgnoreCase);
    }
}
