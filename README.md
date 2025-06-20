CoreDocuNexus
CoreDocuNexus は、堅牢なドキュメント管理機能を提供するWebアプリケーションです。AGPLv3ライセンスの持つ強力なコミュニティ保護の利点と、高性能な商用ライブラリの活用を、クリーンなアーキテクチャで両立させることを目指しています。

✨ 主な機能
Webインターフェースを通じたドキュメント管理
PDFドキュメントの動的な生成機能
ライセンスの互換性を維持するためのマイクロサービスアーキテクチャ

🏗️ アーキテクチャ
本プロジェクトは、ライセンスの互換性問題を解決するため、プロセス分離・ライセンス分離アーキテクチャを採用しています。
メインのAGPLv3アプリケーションと、プロプライエタリライブラリ（Syncfusion）を呼び出すMITライセンスのマイクロサービスは、ネットワークAPIを介して連携します。これにより、AGPLv3のコピーレフト効果がプロプライエタリライブラリに影響を及ぼすことを防ぎます。

graph TD
    subgraph "ユーザー空間"
        User[👤 ユーザー<br>(ブラウザ)]
    end

    subgraph "サーバー空間"
        MainApp["メインアプリケーション<br><b>(GNU AGPLv3)</b>"]
        CoreDocuNexusService["マイクロサービス<br><b>CoreDocuNexus</b><br>(<b>MIT License</b>)"]
        Syncfusion["Syncfusion .NET PDF Library<br>(Proprietary License)"]
    end

    User -- HTTPS --> MainApp
    MainApp -- "PDF生成をリクエスト<br>(REST API / RPC)" --> CoreDocuNexusService
    CoreDocuNexusService -- "ライブラリ呼び出し" --> Syncfusion


🚀 インストールとビルド手順
前提条件
.NET SDK
Git
1. リポジトリのクローン
git clone [https://github.com/kooiei-in4a/CoreDocuNexus.git](https://github.com/kooiei-in4a/CoreDocuNexus.git)
cd CoreDocuNexus


2. ⚙️ Syncfusion ライセンスキーの設定
本プロジェクトを実行するには、Syncfusionのライセンスキーが必須です。
Syncfusionのアカウントから有効なライセンスキーを取得してください。Community License の資格がある場合、無償で利用できます。
【重要】取得したライセンスキーを appsettings.json やその他のソース管理対象ファイルに直接書き込まないでください。
以下のいずれかの安全な方法でライセンスキーを設定します。
環境変数 (推奨)
# Linux/macOS
export SYNCFUSION_LICENSE_KEY="YOUR_KEY_HERE"

# Windows (PowerShell)
$env:SYNCFUSION_LICENSE_KEY="YOUR_KEY_HERE"


.NET User Secrets (ローカル開発用)
cd src/CoreDocuNexus.Api  # CoreDocuNexusのAPIプロジェクトへ移動
dotnet user-secrets init
dotnet user-secrets set "SyncfusionLicenseKey" "YOUR_KEY_HERE"


3. アプリケーションの実行
dotnet run --project src/CoreDocuNexus.Web/CoreDocuNexus.Web.csproj


🛡️ License
This project consists of multiple components with different licenses.
Main Application: Licensed under the GNU Affero General Public License v3.0 (AGPLv3). Please see the LICENSE file for full details.
CoreDocuNexus (PDF Generation Service): The infrastructure and API components responsible for PDF generation are licensed under the MIT License.

⚠️ Important Notice on Dependencies & Your Obligations
This project relies on the Syncfusion .NET PDF library for its core PDF functionalities. This library is a proprietary commercial software and is NOT licensed under AGPLv3 or MIT. Its use is governed by the Syncfusion License Agreement.
As a user of this source code, you have the following obligations:
You Must Obtain a Syncfusion License:
To build, run, or self-host this project, you must independently acquire a valid license for the Syncfusion library. You may be eligible for the free Syncfusion Community License.
You Must Meet the Eligibility for Community License:
To use the Community License, you or your organization must meet all of the following criteria:
Annual gross revenue of less than $1,000,000 USD.
Fewer than six developers in your organization.
(Please refer to Syncfusion's official website for the latest terms.)
You Must Purchase a Commercial License if Ineligible:
If you or your organization do not meet the criteria for the Community License, you are required to purchase a commercial license from Syncfusion to use this project.
Our Commitment to Compliance:
To maintain license compliance, our architecture ensures that calls to the proprietary Syncfusion library are isolated within the components licensed under the permissive MIT License. These components communicate with the main AGPLv3 application at arm's length (via API calls), preventing license conflicts.

🙌 貢献の方法 (How to Contribute)
本プロジェクトへの貢献に興味を持っていただき、ありがとうございます。貢献を希望される方は、以下のガイドラインに従ってください。
このリポジトリをフォークしてください。
新しいブランチを作成します (git checkout -b feature/your-feature-name)。
変更をコミットします (git commit -m 'Add some feature')。
ブランチにプッシュします (git push origin feature/your-feature-name)。
プルリクエストを作成してください。
貢献にあたっては、CODE_OF_CONDUCT.md を尊重してください。また、すべての貢献者には CLA (Contributor License Agreement) への署名をお願いしています。プルリクエスト作成時にbotが自動で案内します。

⚖️ 免責事項
本ドキュメントおよびプロジェクトは、OSSライセンスに関する一般的な知見とベストプラクティスに基づくものであり、法的な保証を行うものではありません。最終的な判断や、具体的なビジネスでの利用に際しては、必要に応じて弁護士などの法律専門家にご相談ください。
