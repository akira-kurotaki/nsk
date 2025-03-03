namespace NskWeb.Areas.F04.Models.D0401
{
    /// <summary>
    /// ファイル情報モデル
    /// </summary>
    [Serializable]
    public class D0401ModelVar
    {
        /// <summary>
        /// 変数：予想外件数
        /// </summary>        
        public int VarYosogaiKensu { get; set; }

        /// <summary>
        /// ファイル名(パスを含む)
        /// </summary>
        public string FileNameWithPath { get; set; }

        /// <summary>
        /// ファイル名(パスと拡張子を含まない)
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 一時フォルダ
        /// </summary>
        public string TempFolder { get; set; }

        /// <summary>
        /// 一時ファイルパス(ファイル名を含む)
        /// </summary>
        public string TempFilePath { get; set; }

        /// <summary>
        /// ファイル識別名
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        /// 改ざん検知ハッシュ値
        /// </summary>
        public string FileHash { get; set; }

        /// <summary>
        /// ファイルの保存先
        /// </summary>
        public string UploadFolder { get; set; }

        /// <summary>
        /// ファイルパス
        /// </summary>
        public string FileSavePath { get; set; }

        /// <summary>
        /// ファイルサイズ
        /// </summary>
        public int FileSize { get; set; }
    }
}
