using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jp.in4a.CoreDocuNexus.Shared.Dto.Pdf
{
    /// <summary>
    /// ドキュメントのアクセス許可を管理するクラスです。
    /// </summary>
    public class DocumentPermissions
    {
        /// <summary>
        /// 印刷を許可するかどうかを取得または設定します。
        /// </summary>
        /// <value>trueの場合、印刷が許可されます。</value>
        public bool AllowPrinting { get; set; }

        /// <summary>
        /// 内容のコピーを許可するかどうかを取得または設定します。
        /// </summary>
        /// <value>trueの場合、コピーが許可されます。</value>
        public bool AllowCopying { get; set; }

        /// <summary>
        /// フォームフィールドの入力を許可するかどうかを取得または設定します。
        /// </summary>
        /// <value>trueの場合、フォームフィールドの入力が許可されます。</value>
        public bool AllowFormFieldFilling { get; set; }

        /// <summary>
        /// 文書の変更を許可するかどうかを取得または設定します。
        /// </summary>
        /// <value>trueの場合、文書の変更が許可されます。</value>
        public bool AllowDocumentChanges { get; set; }

        /// <summary>
        /// 注釈の修正を許可するかどうかを取得または設定します。
        /// </summary>
        /// <value>trueの場合、注釈の修正が許可されます。</value>
        public bool AllowAnnotationModification { get; set; }

        /// <summary>
        /// スクリーンリーダーなどのアクセシビリティ目的での内容の抽出を許可するかどうかを取得または設定します。
        /// </summary>
        /// <value>trueの場合、アクセシビリティのための内容抽出が許可されます。</value>
        public bool AllowContentExtractionForAccessibility { get; set; }


        /// <summary>
        /// 文書のアセンブリ（ページの挿入、回転、削除など）を許可するかどうかを取得または設定します。
        /// </summary>
        /// <value>trueの場合、文書のアセンブリが許可されます。</value>
        public bool AllowDocumentAssembly { get; set; }

        /// <summary>
        /// 高解像度（最高品質）での印刷を許可するかどうかを取得または設定します。
        /// この設定は、通常 AllowPrinting が true の場合に意味を持ちます。
        /// </summary>
        /// <value>trueの場合、最高品質での印刷が許可されます。</value>
        public bool AllowPermitFullQualityPrint { get; set; }

        /// <summary>
        /// デフォルトのコンストラクタ。
        /// すべての権限を false (非許可) で初期化します。
        /// </summary>
        public DocumentPermissions()
        {
            AllowPrinting = false;
            AllowCopying = false;
            AllowFormFieldFilling = false;
            AllowDocumentChanges = false;
            AllowAnnotationModification = false;
            AllowContentExtractionForAccessibility = false;
            AllowDocumentAssembly = false;
            AllowPermitFullQualityPrint = false;
        }

        /// <summary>
        /// すべての権限を許可する設定でインスタンスを生成します。
        /// </summary>
        /// <returns>すべての権限が許可されたDocumentPermissionsインスタンス。</returns>
        public static DocumentPermissions AllowAll()
        {
            return new DocumentPermissions
            {
                AllowPrinting = true,
                AllowCopying = true,
                AllowFormFieldFilling = true,
                AllowDocumentChanges = true,
                AllowAnnotationModification = true,
                AllowContentExtractionForAccessibility = true,
                AllowDocumentAssembly = true,
                AllowPermitFullQualityPrint = true
            };
        }

        /// <summary>
        /// すべての権限を許可する設定でインスタンスを生成します。
        /// </summary>
        /// <returns>すべての権限が許可されたDocumentPermissionsインスタンス。</returns>
        public static DocumentPermissions DenyAll()
        {
            return new DocumentPermissions
            {
                AllowPrinting = false,
                AllowCopying = false,
                AllowFormFieldFilling = false,
                AllowDocumentChanges = false,
                AllowAnnotationModification = false,
                AllowContentExtractionForAccessibility = false,
                AllowDocumentAssembly = false,
                AllowPermitFullQualityPrint = false
            };
        }
    }
}
