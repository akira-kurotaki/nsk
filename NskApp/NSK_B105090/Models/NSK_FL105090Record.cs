namespace NSK_B105090.Models
{
    /// <summary>
    /// 加入申込書チェックリストデータ
    /// </summary>
    public class NSK_FL105090Record
    {
        #region "項目"
        /// <summary>組合員等コード</summary>							
        public string? KumiaiintoCd { get; set; } = string.Empty;
        /// <summary>組合員等氏名漢字</summary>							
        public string? KumiaiintoNm { get; set; } = string.Empty;
        /// <summary>エラーメッセージ1</summary>						
        public string? ErrorMsg1 { get; set; } = string.Empty;
        /// <summary>エラーメッセージ2</summary>						
        public string? ErrorMsg2 { get; set; } = string.Empty;
        /// <summary>引受方式チェック</summary>							
        public string? HikiukeHoshikiChk { get; set; } = string.Empty;
        /// <summary>引受方式</summary>									
        public string? HikiukeHoshikiCd { get; set; } = string.Empty;
        /// <summary>特約区分</summary>									
        public string? TokuyakuKbn { get; set; } = string.Empty;
        /// <summary>補償割合コード</summary>							
        public string? HoshowariaiCd { get; set; } = string.Empty;
        /// <summary>耕地番号</summary>									
        public string? KochiBango { get; set; } = string.Empty;
        /// <summary>FIMチェック</summary>								
        public string? FimChk { get; set; } = string.Empty;
        /// <summary>各種条件チェック</summary>							
        public string? KakushuJokenChk { get; set; } = string.Empty;
        /// <summary>関連チェック</summary>								
        public string? KanrenChk { get; set; } = string.Empty;
        /// <summary>地名地番</summary>									
        public string? ChimeiChiban { get; set; } = string.Empty;
        /// <summary>本地面積</summary>									
        public decimal? HonchiMenseki { get; set; }
        /// <summary>本地面積チェック</summary>							
        public string? HonchiMensekiChk { get; set; } = string.Empty;
        /// <summary>分筆番号</summary>									
        public string? BunpitsuBango { get; set; } = string.Empty;
        /// <summary>引受面積</summary>									
        public decimal? HikiukeMenseki { get; set; }
        /// <summary>引受面積チェック</summary>							
        public string? HikiukeMensekiChk { get; set; } = string.Empty;
        /// <summary>転作等面積</summary>								
        public decimal? TensakutoMenseki { get; set; }
        /// <summary>転作等面積チェック</summary>						
        public string? TensakutoMensekiChk { get; set; } = string.Empty;
        /// <summary>種類コード</summary>								
        public string? ShuruiCd { get; set; } = string.Empty;
        /// <summary>種類コードチェック</summary>						
        public string? ShuruiCdChk { get; set; } = string.Empty;
        /// <summary>区分</summary>										
        public string? Kbn { get; set; } = string.Empty;
        /// <summary>区分チェック</summary>								
        public string? KbnChk { get; set; } = string.Empty;
        /// <summary>品種</summary>										
        public string? HinshuCd { get; set; } = string.Empty;
        /// <summary>品種チェック</summary>								
        public string? HinshuCdChk { get; set; } = string.Empty;
        /// <summary>収量等級</summary>									
        public string? Shuryotokyu { get; set; } = string.Empty;
        /// <summary>収量等級チェック</summary>							
        public string? ShuryotokyuChk { get; set; } = string.Empty;
        /// <summary>実量単収</summary>									
        public decimal? JitsuryoTanshu { get; set; }
        /// <summary>実量単収チェック</summary>							
        public string? JitsuryoTanshuChk { get; set; } = string.Empty;
        /// <summary>統計単収</summary>									
        public decimal? TokeiTanshu { get; set; }
        /// <summary>統計単収チェック</summary>							
        public string? TokeiTanshuChk { get; set; } = string.Empty;
        /// <summary>参酌係数</summary>									
        public string? SanjakuKeisu { get; set; } = string.Empty;
        /// <summary>参酌係数チェック</summary>							
        public string? SanjakuKeisuChk { get; set; } = string.Empty;
        /// <summary>田畑区分</summary>									
        public string? TahataKbn { get; set; } = string.Empty;
        /// <summary>田畑区分チェック</summary>							
        public string? TahataKbnChk { get; set; } = string.Empty;
        /// <summary>類区分</summary>									
        public string? RuiKbn { get; set; } = string.Empty;
        /// <summary>受委託者区分</summary>								
        public string? JutakushaKbn { get; set; } = string.Empty;
        /// <summary>受委託者区分チェック</summary>						
        public string? JuitakushaKbnChk { get; set; } = string.Empty;
        /// <summary>受委託者コード</summary>							
        public string? JuitakushaCd { get; set; } = string.Empty;
        /// <summary>受委託者コードチェック</summary>					
        public string? JuitakushaCdChk { get; set; } = string.Empty;
        /// <summary>RS区分（GIS）</summary>							
        public string? RsKbnGis { get; set; } = string.Empty;
        /// <summary>局都道府県コード（GIS）</summary>					
        public string? KyokuTodofukenCdGis { get; set; } = string.Empty;
        /// <summary>市区町村コード（GIS）</summary>					
        public string? ShichosonCdGis { get; set; } = string.Empty;
        /// <summary>大字コード（GIS）</summary>						
        public string? OoazaCdGis { get; set; } = string.Empty;
        /// <summary>小字コード（GIS）</summary>						
        public string? KoazaCdGis { get; set; } = string.Empty;
        /// <summary>地番（GIS）</summary>								
        public string? ChibanGis { get; set; } = string.Empty;
        /// <summary>枝番（GIS）</summary>								
        public string? EdabanGis { get; set; } = string.Empty;
        /// <summary>子番（GIS）</summary>								
        public string? KobanGis { get; set; } = string.Empty;
        /// <summary>孫番（GIS）</summary>								
        public string? MagobanGis { get; set; } = string.Empty;
        /// <summary>引受情報削除チェック</summary>						
        public int? HikiukeJohoSakujoChk { get; set; }
        /// <summary>削除時組合員等コード</summary>						
        public string? SakujojiKumiaiintoCd { get; set; } = string.Empty;
        /// <summary>削除時耕地番号</summary>							
        public string? SakujojiKochiBango { get; set; } = string.Empty;
        /// <summary>削除時分筆</summary>								
        public string? SakujojiBunpitsuBango { get; set; } = string.Empty;
        /// <summary>削除時地名地番</summary>							
        public string? SakujojiChimeiChiban { get; set; } = string.Empty;
        /// <summary>削除日付時分秒</summary>							
        public DateTime? SakujoHiduke { get; set; }
        #endregion

        /// <summary>
        /// データをString配列として返す
        /// </summary>
        /// <returns></returns>
        public string[] GetValueArray()
        {
            string[] values =
            [
                // 組合員等コード
                KumiaiintoCd ?? string.Empty,
                    // 組合員等氏名漢字
                    KumiaiintoNm ?? string.Empty,
                    // エラーメッセージ1
                    ErrorMsg1 ?? string.Empty,
                    // エラーメッセージ2
                    ErrorMsg2 ?? string.Empty,
                    // 引受方式チェック
                    HikiukeHoshikiChk ?? string.Empty,
                    // 引受方式
                    HikiukeHoshikiCd ?? string.Empty,
                    // 特約区分
                    TokuyakuKbn ?? string.Empty,
                    // 補償割合コード
                    HoshowariaiCd ?? string.Empty,
                    // 耕地番号
                    KochiBango ?? string.Empty,
                    // FIMチェック
                    FimChk ?? string.Empty,
                    // 各種条件チェック
                    KakushuJokenChk ?? string.Empty,
                    // 関連チェック
                    KanrenChk ?? string.Empty,
                    // 地名地番
                    ChimeiChiban ?? string.Empty,
                    // 本地面積
                    HonchiMenseki?.ToString("0#.##") ?? string.Empty,
                    // 本地面積チェック
                    HonchiMensekiChk ?? string.Empty,
                    // 分筆番号
                    BunpitsuBango ?? string.Empty,
                    // 引受面積
                    HikiukeMenseki?.ToString("0#.##") ?? string.Empty,
                    // 引受面積チェック
                    HikiukeMensekiChk ?? string.Empty,
                    // 転作等面積
                    TensakutoMenseki?.ToString("0#.##") ?? string.Empty,
                    // 転作等面積チェック
                    TensakutoMensekiChk ?? string.Empty,
                    // 種類コード
                    ShuruiCd ?? string.Empty,
                    // 種類コードチェック
                    ShuruiCdChk ?? string.Empty,
                    // 区分
                    Kbn ?? string.Empty,
                    // 区分チェック
                    KbnChk ?? string.Empty,
                    // 品種
                    HinshuCd ?? string.Empty,
                    // 品種チェック
                    HinshuCdChk ?? string.Empty,
                    // 収量等級
                    Shuryotokyu ?? string.Empty,
                    // 収量等級チェック
                    ShuryotokyuChk ?? string.Empty,
                    // 実量単収
                    JitsuryoTanshu?.ToString("000#") ?? string.Empty,
                    // 実量単収チェック
                    JitsuryoTanshuChk ?? string.Empty,
                    // 統計単収
                    TokeiTanshu?.ToString("000#") ?? string.Empty,
                    // 統計単収チェック
                    TokeiTanshuChk ?? string.Empty,
                    // 参酌係数
                    SanjakuKeisu ?? string.Empty,
                    // 参酌係数チェック
                    SanjakuKeisuChk ?? string.Empty,
                    // 田畑区分
                    TahataKbn ?? string.Empty,
                    // 田畑区分チェック
                    TahataKbnChk ?? string.Empty,
                    // 類区分
                    RuiKbn ?? string.Empty,
                    // 受委託者区分
                    JutakushaKbn ?? string.Empty,
                    // 受委託者区分チェック
                    JuitakushaKbnChk ?? string.Empty,
                    // 受委託者コード
                    JuitakushaCd ?? string.Empty,
                    // 受委託者コードチェック
                    JuitakushaCdChk ?? string.Empty,
                    // RS区分（GIS）
                    RsKbnGis ?? string.Empty,
                    // 局都道府県コード（GIS）
                    KyokuTodofukenCdGis ?? string.Empty,
                    // 市区町村コード（GIS）
                    ShichosonCdGis ?? string.Empty,
                    // 大字コード（GIS）
                    OoazaCdGis ?? string.Empty,
                    // 小字コード（GIS）
                    KoazaCdGis ?? string.Empty,
                    // 地番（GIS）
                    ChibanGis ?? string.Empty,
                    // 枝番（GIS）
                    EdabanGis ?? string.Empty,
                    // 子番（GIS）
                    KobanGis ?? string.Empty,
                    // 孫番（GIS）
                    MagobanGis ?? string.Empty,
                    // 引受情報削除チェック
                    $"{HikiukeJohoSakujoChk}",
                    // 削除時組合員等コード
                    SakujojiKumiaiintoCd ?? string.Empty,
                    // 削除時耕地番号
                    SakujojiKochiBango ?? string.Empty,
                    // 削除時分筆
                    SakujojiBunpitsuBango ?? string.Empty,
                    // 削除時地名地番
                    SakujojiChimeiChiban ?? string.Empty,
                    // 削除日付時分秒
                    SakujoHiduke?.ToString("yyyy/MM/dd HH:mm:ss") ?? string.Empty,
                ];

            return values;
        }
    }
}
