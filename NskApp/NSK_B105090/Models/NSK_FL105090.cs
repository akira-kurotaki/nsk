using CoreLibrary.Core.Utility;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NskAppModelLibrary.Context;
using System.Text;

namespace NSK_B105090.Models
{
    public class NSK_FL105090
    {
        /// <summary>
        /// 引数
        /// </summary>
        private Arguments arg;

        /// <summary>
        /// バッチ条件
        /// </summary>
        private BatchJoken batchJoken;

        /// <summary>
        /// 加入申込書チェックリストデータ
        /// </summary>
        private List<NSK_FL105090Record> records = [];

        /// <summary>
        /// 一時出力フォルダパス
        /// </summary>
        private string tempFolderPath = string.Empty;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="arg">引数</param>
        /// <param name="batchJoken">バッチ条件</param>
        public NSK_FL105090(Arguments arg, BatchJoken batchJoken)
        {
            this.arg = arg;
            this.batchJoken = batchJoken;
        }

        /// <summary>
        /// 対象データの取得
        /// </summary>
        /// <param name="dbContext"></param>
        public void Search(NskAppContext dbContext)
        {
            // ７．１．１．「加入申込書チェックリストデータ」を取得する。
            StringBuilder query = new();
            query.Append(" SELECT ");                                           
            query.Append($"      組合員等コード          As \"{nameof(NSK_FL105090Record.KumiaiintoCd)}\" ");                                        
            query.Append($"    , 組合員等氏名漢字        As \"{nameof(NSK_FL105090Record.KumiaiintoNm)}\" ");                                         
            query.Append($"    , エラーメッセージ1       As \"{nameof(NSK_FL105090Record.ErrorMsg1)}\" ");                                         
            query.Append($"    , エラーメッセージ2       As \"{nameof(NSK_FL105090Record.ErrorMsg2)}\" ");                                         
            query.Append($"    , 引受方式チェック        As \"{nameof(NSK_FL105090Record.HikiukeHoshikiChk)}\" ");                                        
            query.Append($"    , 引受方式                As \"{nameof(NSK_FL105090Record.HikiukeHoshikiCd)}\" ");                                         
            query.Append($"    , 特約区分                As \"{nameof(NSK_FL105090Record.TokuyakuKbn)}\" ");                                         
            query.Append($"    , 補償割合コード          As \"{nameof(NSK_FL105090Record.HoshowariaiCd)}\" ");                                         
            query.Append($"    , 耕地番号                As \"{nameof(NSK_FL105090Record.KochiBango)}\" ");                                         
            query.Append($"    , FIMチェック             As \"{nameof(NSK_FL105090Record.FimChk)}\" ");                                         
            query.Append($"    , 各種条件チェック        As \"{nameof(NSK_FL105090Record.KakushuJokenChk)}\" ");                                         
            query.Append($"    , 関連チェック            As \"{nameof(NSK_FL105090Record.KanrenChk)}\" ");                                         
            query.Append($"    , 地名地番                As \"{nameof(NSK_FL105090Record.ChimeiChiban)}\" ");                                         
            query.Append($"    , 本地面積                As \"{nameof(NSK_FL105090Record.HonchiMenseki)}\" ");                                         
            query.Append($"    , 本地面積チェック        As \"{nameof(NSK_FL105090Record.HonchiMensekiChk)}\" ");                                         
            query.Append($"    , 分筆番号                As \"{nameof(NSK_FL105090Record.BunpitsuBango)}\" ");                                         
            query.Append($"    , 引受面積                As \"{nameof(NSK_FL105090Record.HikiukeMenseki)}\" ");                                         
            query.Append($"    , 引受面積チェック        As \"{nameof(NSK_FL105090Record.HikiukeMensekiChk)}\" ");                                         
            query.Append($"    , 転作等面積              As \"{nameof(NSK_FL105090Record.TensakutoMenseki)}\" ");                                         
            query.Append($"    , 転作等面積チェック      As \"{nameof(NSK_FL105090Record.TensakutoMensekiChk)}\" ");                                         
            query.Append($"    , 種類コード              As \"{nameof(NSK_FL105090Record.ShuruiCd)}\" ");                                         
            query.Append($"    , 種類コードチェック      As \"{nameof(NSK_FL105090Record.ShuruiCdChk)}\" ");                                         
            query.Append($"    , 区分                    As \"{nameof(NSK_FL105090Record.Kbn)}\" ");                                         
            query.Append($"    , 区分チェック            As \"{nameof(NSK_FL105090Record.KbnChk)}\" ");                                         
            query.Append($"    , 品種                    As \"{nameof(NSK_FL105090Record.HinshuCd)}\" ");                                         
            query.Append($"    , 品種チェック            As \"{nameof(NSK_FL105090Record.HinshuCdChk)}\" ");                                         
            query.Append($"    , 収量等級                As \"{nameof(NSK_FL105090Record.Shuryotokyu)}\" ");                                         
            query.Append($"    , 収量等級チェック        As \"{nameof(NSK_FL105090Record.ShuryotokyuChk)}\" ");                                         
            query.Append($"    , 実量単収                As \"{nameof(NSK_FL105090Record.JitsuryoTanshu)}\" ");                                         
            query.Append($"    , 実量単収チェック        As \"{nameof(NSK_FL105090Record.JitsuryoTanshuChk)}\" ");                                         
            query.Append($"    , 統計単収                As \"{nameof(NSK_FL105090Record.TokeiTanshu)}\" ");                                         
            query.Append($"    , 統計単収チェック        As \"{nameof(NSK_FL105090Record.TokeiTanshuChk)}\" ");                                         
            query.Append($"    , 参酌係数                As \"{nameof(NSK_FL105090Record.SanjakuKeisu)}\" ");                                         
            query.Append($"    , 参酌係数チェック        As \"{nameof(NSK_FL105090Record.SanjakuKeisuChk)}\" ");                                         
            query.Append($"    , 田畑区分                As \"{nameof(NSK_FL105090Record.TahataKbn)}\" ");                                         
            query.Append($"    , 田畑区分チェック        As \"{nameof(NSK_FL105090Record.TahataKbnChk)}\" ");                                         
            query.Append($"    , 類区分                  As \"{nameof(NSK_FL105090Record.RuiKbn)}\" ");                                         
            query.Append($"    , 受委託者区分            As \"{nameof(NSK_FL105090Record.JutakushaKbn)}\" ");                                         
            query.Append($"    , 受委託者区分チェック    As \"{nameof(NSK_FL105090Record.JuitakushaKbnChk)}\" ");                                         
            query.Append($"    , 受委託者コード          As \"{nameof(NSK_FL105090Record.JuitakushaCd)}\" ");                                         
            query.Append($"    , 受委託者コードチェック  As \"{nameof(NSK_FL105090Record.JuitakushaCdChk)}\" ");                                         
            query.Append($"    , RS区分（GIS）           As \"{nameof(NSK_FL105090Record.RsKbnGis)}\" ");                                         
            query.Append($"    , 局都道府県コード（GIS） As \"{nameof(NSK_FL105090Record.KyokuTodofukenCdGis)}\" ");                                         
            query.Append($"    , 市区町村コード（GIS）   As \"{nameof(NSK_FL105090Record.ShichosonCdGis)}\" ");                                         
            query.Append($"    , 大字コード（GIS）       As \"{nameof(NSK_FL105090Record.OoazaCdGis)}\" ");                                         
            query.Append($"    , 小字コード（GIS）       As \"{nameof(NSK_FL105090Record.KoazaCdGis)}\" ");                                         
            query.Append($"    , 地番（GIS）             As \"{nameof(NSK_FL105090Record.ChibanGis)}\" ");                                         
            query.Append($"    , 枝番（GIS）             As \"{nameof(NSK_FL105090Record.EdabanGis)}\" ");                                         
            query.Append($"    , 子番（GIS）             As \"{nameof(NSK_FL105090Record.KobanGis)}\" ");                                         
            query.Append($"    , 孫番（GIS）             As \"{nameof(NSK_FL105090Record.MagobanGis)}\" ");                                         
            query.Append($"    , 引受情報削除チェック    As \"{nameof(NSK_FL105090Record.HikiukeJohoSakujoChk)}\" ");                                         
            query.Append($"    , 削除時組合員等コード    As \"{nameof(NSK_FL105090Record.SakujojiKumiaiintoCd)}\" ");                                         
            query.Append($"    , 削除時耕地番号          As \"{nameof(NSK_FL105090Record.SakujojiKochiBango)}\" ");                                         
            query.Append($"    , 削除時分筆              As \"{nameof(NSK_FL105090Record.SakujojiBunpitsuBango)}\" ");                                         
            query.Append($"    , 削除時地名地番          As \"{nameof(NSK_FL105090Record.SakujojiChimeiChiban)}\" ");
            query.Append($"    , 削除日付時分秒          As \"{nameof(NSK_FL105090Record.SakujoHiduke)}\" ");                                         

            query.Append("  FROM ");                                            
            query.Append("  ( ");                                            
            query.Append("   SELECT ");                                           
            query.Append("     T1.組合員等コード ");                                         
            query.Append("    , T3.hojin_full_nm AS 組合員等氏名漢字 "); // 氏名又は法人名                                         
            query.Append("    , CASE ");                                         
            query.Append("      WHEN   T1.FIMチェック      = '1' THEN 'FIMマスターエラー' ");                      
            query.Append("      WHEN   T1.各種条件チェック = '1' THEN '各種条件エラー' ");                      
            query.Append("      WHEN   T1.関連チェック     = '1' THEN '分筆関連エラー' ");                      
            query.Append("      ELSE  '' ");                                      
            query.Append("     END AS エラーメッセージ1 ");                                         
            query.Append("    , CASE ");                                         
            query.Append("      WHEN  (T1.FIMチェック      = '1' ");                           
            query.Append("         OR  T1.各種条件チェック = '1') ");                           
            query.Append("        AND  T1.関連チェック     = '1' THEN '分筆関連エラー' ");                      
            query.Append("      ELSE  '' ");                                      
            query.Append("     END AS エラーメッセージ2 ");                                         
            query.Append("    , T1.引受方式チェック ");                                         
            query.Append("    , T1.引受方式 ");                                         
            query.Append("    , T1.特約区分 ");                                         
            query.Append("    , T1.補償割合コード ");                                         
            query.Append("    , T1.耕地番号 ");                                         
            query.Append("    , T1.FIMチェック ");                                         
            query.Append("    , T1.各種条件チェック ");                                         
            query.Append("    , T1.関連チェック ");                                         
            query.Append("    , T2.地名地番 ");                                         
            query.Append("    , T2.耕地面積 AS 本地面積 ");                                         
            query.Append("    , T1.耕地面積チェック AS 本地面積チェック ");                                         
            query.Append("    , T1.分筆番号 ");                                         
            query.Append("    , T2.引受面積 ");                                         
            query.Append("    , T1.引受面積チェック ");                                         
            query.Append("    , T2.転作等面積 ");                                         
            query.Append("    , T1.転作等面積チェック ");                                         
            query.Append("    , T2.種類コード ");                                         
            query.Append("    , T1.種類チェック AS 種類コードチェック ");                                         
            query.Append("    , T2.区分コード AS 区分 ");                                         
            query.Append("    , T1.区分チェック ");                                         
            query.Append("    , T2.品種コード AS 品種 ");                                         
            query.Append("    , T1.品種チェック ");                                         
            query.Append("    , T2.収量等級コード AS 収量等級 ");                                         
            query.Append("    , T1.収量等級チェック ");                                         
            query.Append("    , T2.実量基準単収 AS 実量単収 ");                                         
            query.Append("    , T1.実量単収チェック ");                                         
            query.Append("    , T2.統計単収 ");                                         
            query.Append("    , T1.統計単収チェック ");                                         
            query.Append("    , T2.参酌コード AS 参酌係数 ");                                         
            query.Append("    , T1.参酌チェック AS 参酌係数チェック ");                                         
            query.Append("    , T2.田畑区分 ");                                         
            query.Append("    , T1.田畑区分チェック ");                                         
            query.Append("    , T2.類区分 ");                                         
            query.Append("    , T2.受委託区分 AS 受委託者区分 ");                                         
            query.Append("    , T1.受委託区分チェック AS 受委託者区分チェック ");                                         
            query.Append("    , T2.受委託者コード ");                                         
            query.Append("    , T1.受委託者チェック AS 受委託者コードチェック ");
            query.Append("    , T4.RS区分 AS RS区分（GIS） ");
            query.Append("    , T4.局都道府県コード AS 局都道府県コード（GIS） ");
            query.Append("    , T4.市区町村コード AS 市区町村コード（GIS） ");
            query.Append("    , T4.大字コード AS 大字コード（GIS） ");
            query.Append("    , T4.小字コード AS 小字コード（GIS） ");
            query.Append("    , T4.地番 AS 地番（GIS） ");
            query.Append("    , T4.枝番 AS 枝番（GIS） ");
            query.Append("    , T4.子番 AS 子番（GIS） ");
            query.Append("    , T4.孫番 AS 孫番（GIS） ");
            query.Append("    , 0 AS 引受情報削除チェック ");　                                         
            query.Append("    , '' AS 削除時組合員等コード ");                                         
            query.Append("    , '' AS 削除時耕地番号 ");                                         
            query.Append("    , '' AS 削除時分筆 ");                                         
            query.Append("    , '' AS 削除時地名地番 ");
            query.Append("    , null AS 削除日付時分秒 ");                                         

            query.Append("   FROM ");                                           
            query.Append("     t_12020_引受チェック            T1 ");                                   
            query.Append("     INNER JOIN t_11090_引受耕地     T2 ");                                
            query.Append("      ON   T2.組合等コード   = T1.組合等コード ");                               
            query.Append("      AND  T2.年産           = T1.年産 ");                               
            query.Append("      AND  T2.共済目的コード = T1.共済目的コード ");                               
            query.Append("      AND  T2.組合員等コード = T1.組合員等コード ");                               
            query.Append("      AND  T2.耕地番号       = T1.耕地番号 ");                               
            query.Append("      AND  T2.分筆番号       = T1.分筆番号 ");                               
            query.Append("     LEFT JOIN v_nogyosha            T3 "); // 農業者情報
            query.Append("      ON   T3.kumiaiinto_cd  = T1.組合員等コード "); // 組合員等コード                              
            query.Append("     LEFT JOIN t_11100_引受gis       T4 ");                                
            query.Append("      ON   T4.組合等コード   = T1.組合等コード ");                               
            query.Append("      AND  T4.年産           = T1.年産 ");                               
            query.Append("      AND  T4.共済目的コード = T1.共済目的コード ");                               
            query.Append("      AND  T4.組合員等コード = T1.組合員等コード ");                               
            query.Append("      AND  T4.耕地番号       = T1.耕地番号 ");
            query.Append("      AND  T4.分筆番号       = T1.分筆番号 ");                               

            query.Append("   WHERE ");                                           
            query.Append("      1 = 1 ");                                        
            query.Append("      AND  T1.組合等コード   = @条件_組合等コード ");                                
            query.Append("      AND  T1.年産           = @条件_年産 ");                                
            query.Append("      AND  T1.共済目的コード = @条件_共済目的コード ");                                
            query.Append("      AND  CASE ");                                        
            query.Append("       WHEN  @条件_大地区コード <> '' THEN  T1.大地区コード = @条件_大地区コード ");                
            query.Append("       ELSE  1 = 1 ");                                     
            query.Append("      END ");                                        
            query.Append("    AND  CASE ");                                        
            query.Append("       WHEN  @条件_大地区コード <> '' AND @条件_小地区コードFrom <> '' THEN  T1.小地区コード >= @条件_小地区コードFrom ");     
            query.Append("       ELSE  1 = 1 ");                                     
            query.Append("      END ");                                        
            query.Append("    AND  CASE ");                                        
            query.Append("       WHEN  @条件_大地区コード <> '' AND @条件_小地区コードTo <> '' THEN  T1.小地区コード <= @条件_小地区コードTo ");     
            query.Append("       ELSE  1 = 1 ");                                     
            query.Append("      END ");                                        
            query.Append("    AND  CASE ");                                        
            query.Append("       WHEN   @条件_組合員等コードFrom <> '' THEN  T1.組合員等コード >= @条件_組合員等コードFrom ");                
            query.Append("       ELSE  1 = 1 ");                                      
            query.Append("      END ");                                        
            query.Append("    AND  CASE ");                                        
            query.Append("       WHEN   @条件_組合員等コードTo <> '' THEN  T1.組合員等コード <= @条件_組合員等コードTo ");                
            query.Append("       ELSE  1 = 1 ");                                      
            query.Append("      END ");                                        
            query.Append("    AND  CASE ");                                        
            query.Append("       WHEN  @条件_類区分 <> '' THEN  T1.類区分 = @条件_類区分 ");                
            query.Append("       ELSE  1 = 1 ");                                     
            query.Append("      END ");                                        
            query.Append("    AND  CASE ");                                        
            query.Append("       WHEN  @条件_引受方式 <> '' THEN  T1.引受方式 = @条件_引受方式 ");                
            query.Append("       ELSE  1 = 1 ");                                     
            query.Append("      END ");                                        
            query.Append("    AND  CASE ");                                        
            query.Append("       WHEN  @条件_補償割合 <> '' THEN  T1.補償割合コード = @条件_補償割合 ");                
            query.Append("       ELSE  1 = 1 ");                                     
            query.Append("      END ");                                        
            query.Append("    AND  CASE ");                                        
            query.Append("       WHEN  @条件_特約区分 <> '' THEN  T1.特約区分 = @条件_特約区分 ");                
            query.Append("       ELSE  1 = 1 ");                                     
            query.Append("      END ");                                        
            query.Append("    AND  CASE ");                                        
            query.Append("       WHEN   @条件_更新日時From <> '' THEN  to_char(T1.登録日時, 'YYYY/MM/DD')  >= to_char(to_date(@条件_更新日時From, 'YYYY/MM/DD'), 'YYYY/MM/DD') ");                
            query.Append("       ELSE  1 = 1 ");                                      
            query.Append("      END ");                                        
            query.Append("    AND  CASE ");                                        
            query.Append("       WHEN   @条件_更新日時To <> '' THEN  to_char(T1.登録日時, 'YYYY/MM/DD')  <= to_char(to_date(@条件_更新日時To, 'YYYY/MM/DD'), 'YYYY/MM/DD') ");                
            query.Append("       ELSE  1 = 1 ");
            query.Append("      END ");

            query.Append("  UNION ALL ");                                            

            query.Append("   SELECT ");                                           
            query.Append("      '' AS 組合員等コード ");                                        
            query.Append("    , '' AS 組合員等氏名漢字 ");                                         
            query.Append("    , '' AS エラーメッセージ1 ");                                         
            query.Append("    , '' AS エラーメッセージ2 ");                                         
            query.Append("    , '' AS 引受方式チェック ");                                        
            query.Append("    , '' AS 引受方式 ");                                         
            query.Append("    , '' AS 特約区分 ");                                         
            query.Append("    , '' AS 補償割合コード ");                                         
            query.Append("    , '' AS 耕地番号 ");                                         
            query.Append("    , '' AS FIMチェック ");                                         
            query.Append("    , '' AS 各種条件チェック ");                                         
            query.Append("    , '' AS 関連チェック ");                                         
            query.Append("    , '' AS 地名地番 ");                                         
            query.Append("    , null AS 本地面積 ");                                         
            query.Append("    , '' AS 本地面積チェック ");                                         
            query.Append("    , '' AS 分筆番号 ");                                         
            query.Append("    , null AS 引受面積 ");                                         
            query.Append("    , '' AS 引受面積チェック ");                                         
            query.Append("    , null AS 転作等面積 ");                                         
            query.Append("    , '' AS 転作等面積チェック ");                                         
            query.Append("    , '' AS 種類コード ");                                         
            query.Append("    , '' AS 種類コードチェック ");                                         
            query.Append("    , '' AS 区分 ");                                         
            query.Append("    , '' AS 区分チェック ");                                         
            query.Append("    , '' AS 品種 ");                                         
            query.Append("    , '' AS 品種チェック ");                                         
            query.Append("    , '' AS 収量等級 ");                                         
            query.Append("    , '' AS 収量等級チェック ");                                         
            query.Append("    , null AS 実量単収 ");                                         
            query.Append("    , '' AS 実量単収チェック ");                                         
            query.Append("    , null AS 統計単収 ");                                         
            query.Append("    , '' AS 統計単収チェック ");                                         
            query.Append("    , '' AS 参酌係数 ");                                         
            query.Append("    , '' AS 参酌係数チェック ");                                         
            query.Append("    , '' AS 田畑区分 ");                                         
            query.Append("    , '' AS 田畑区分チェック ");                                         
            query.Append("    , '' AS 類区分 ");                                         
            query.Append("    , '' AS 受委託者区分 ");                                         
            query.Append("    , '' AS 受委託者区分チェック ");                                         
            query.Append("    , '' AS 受委託者コード ");                                         
            query.Append("    , '' AS 受委託者コードチェック "); 
            query.Append("    , '' AS RS区分（GIS） ");                                         
            query.Append("    , '' AS 局都道府県コード（GIS） ");                                         
            query.Append("    , '' AS 市区町村コード（GIS） ");                                         
            query.Append("    , '' AS 大字コード（GIS） ");                                         
            query.Append("    , '' AS 小字コード（GIS） ");                                         
            query.Append("    , '' AS 地番（GIS） ");                                         
            query.Append("    , '' AS 枝番（GIS） ");                                         
            query.Append("    , '' AS 子番（GIS） ");                                         
            query.Append("    , '' AS 孫番（GIS） ");                                         
            query.Append("    , 1 AS 引受情報削除チェック ");                                         
            query.Append("    , T5.組合員等コード AS 削除時組合員等コード ");                                         
            query.Append("    , T5.耕地番号 AS 削除時耕地番号 ");                                         
            query.Append("    , T5.分筆番号 AS 削除時分筆 ");                                         
            query.Append("    , T5.地名地番 AS 削除時地名地番 ");
            query.Append("    , T5.削除日時 AS 削除日付時分秒 ");                                         

            query.Append("   FROM ");                                           
            query.Append("     t_12140_引受耕地削除データ保持         T5 ");                                

            query.Append("   WHERE ");                                           
            query.Append("      1 = 1 ");                                       
            query.Append("    AND  T5.組合等コード   = @条件_組合等コード ");                                
            query.Append("    AND  T5.年産           = @条件_年産 ");                                
            query.Append("    AND  T5.共済目的コード = @条件_共済目的コード ");                                
            query.Append("    AND  CASE ");                                        
            query.Append("       WHEN  @条件_組合員等コードFrom <> '' THEN  T5.組合員等コード >= @条件_組合員等コードFrom ");                  
            query.Append("       ELSE  1 = 1 ");                                      
            query.Append("      END ");                                        
            query.Append("    AND  CASE ");                                        
            query.Append("       WHEN  @条件_組合員等コードTo <> '' THEN  T5.組合員等コード <= @条件_組合員等コードTo ");                  
            query.Append("       ELSE  1 = 1 ");                                      
            query.Append("      END ");
            query.Append("  ) A ");

            if (!string.IsNullOrEmpty(batchJoken.OrderByKey1) ||
                !string.IsNullOrEmpty(batchJoken.OrderByKey2) ||
                !string.IsNullOrEmpty(batchJoken.OrderByKey3))
            {
                // ORDER BY
                query.Append(" ORDER BY ");

                bool isPutOrder = false;
                //  画面指定ソート順
                if (!string.IsNullOrEmpty(batchJoken.OrderByKey1))
                {
                    isPutOrder = true;
                    switch (int.Parse(batchJoken.OrderBy1))
                    {
                        case (int)CoreLibrary.Core.Consts.CoreConst.SortOrder.DESC:
                            // ※「変数：出力順１」の入力がある場合、かつ「変数：昇順・降順１」が降順の場合
                            query.Append($" @出力順1 {CoreLibrary.Core.Consts.CoreConst.SortOrder.DESC} ");
                            break;
                        case (int)CoreLibrary.Core.Consts.CoreConst.SortOrder.ASC:
                            // ※「変数：出力順１」の入力がある場合、かつ「変数：昇順・降順１」が昇順の場合
                            query.Append($" @出力順1 {CoreLibrary.Core.Consts.CoreConst.SortOrder.ASC} ");
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(batchJoken.OrderByKey2))
                {
                    if (isPutOrder)
                    {
                        // ソート条件1が出力されていた場合、カンマを付与する
                        query.Append(", ");
                    }
                    isPutOrder = true;
                    switch (int.Parse(batchJoken.OrderBy2))
                    {
                        case (int)CoreLibrary.Core.Consts.CoreConst.SortOrder.DESC:
                            // ※「変数：出力順２」の入力がある場合、かつ「変数：昇順・降順２」が降順の場合
                            query.Append($" @出力順2 {CoreLibrary.Core.Consts.CoreConst.SortOrder.DESC} ");
                            break;
                        case (int)CoreLibrary.Core.Consts.CoreConst.SortOrder.ASC:
                            // ※「変数：出力順２」の入力がある場合、かつ「変数：昇順・降順２」が昇順の場合
                            query.Append($" @出力順2 {CoreLibrary.Core.Consts.CoreConst.SortOrder.ASC} ");
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(batchJoken.OrderByKey3))
                {
                    if (isPutOrder)
                    {
                        // ソート条件1が出力されていた場合、カンマを付与する
                        query.Append(", ");
                    }
                    switch (int.Parse(batchJoken.OrderBy3))
                    {
                        case (int)CoreLibrary.Core.Consts.CoreConst.SortOrder.DESC:
                            // ※「変数：出力順３」の入力がある場合、かつ「変数：昇順・降順３」が降順の場合
                            query.Append($" @出力順3 {CoreLibrary.Core.Consts.CoreConst.SortOrder.DESC} ");
                            break;
                        case (int)CoreLibrary.Core.Consts.CoreConst.SortOrder.ASC:
                            // ※「変数：出力順３」の入力がある場合、かつ「変数：昇順・降順３」が昇順の場合
                            query.Append($" @出力順3 {CoreLibrary.Core.Consts.CoreConst.SortOrder.ASC} ");
                            break;
                    }
                }
            }


            // パラメータに値を付与する
            NpgsqlParameter[] parameters =
            [
                new("条件_組合等コード", batchJoken.KumiaitoCd),
                new("条件_年産", short.Parse(batchJoken.Nensan)),
                new("条件_共済目的コード", batchJoken.KyosaiMokutekiCd),
                new("条件_大地区コード", batchJoken.DaichikuCd ?? string.Empty),
                new("条件_小地区コードFrom", batchJoken.ShochikuStart ?? string.Empty),
                new("条件_小地区コードTo", batchJoken.ShochikuEnd ?? string.Empty),
                new("条件_組合員等コードFrom", batchJoken.KumiaiintoCdStart ?? string.Empty),
                new("条件_組合員等コードTo", batchJoken.KumiaiintoCdEnd ?? string.Empty),
                new("条件_類区分", batchJoken.RuiKbn ?? string.Empty),
                new("条件_引受方式", batchJoken.HikiukeHoushikiCd ?? string.Empty),
                new("条件_補償割合", batchJoken.HoshoWariaiCd ?? string.Empty),
                new("条件_特約区分", batchJoken.TokuyakuKbn ?? string.Empty),
                new("条件_更新日時From", batchJoken.UpdateDateStart ?? string.Empty),
                new("条件_更新日時To", batchJoken.UpdateDateEnd ?? string.Empty),
                new("出力順1", batchJoken.OrderBy1 ?? string.Empty),
                new("出力順2", batchJoken.OrderBy2 ?? string.Empty),
                new("出力順3", batchJoken.OrderBy3 ?? string.Empty),
            ];

            // SQLのクエリ結果をListに格納する
            records = new();
            records.AddRange(dbContext.Database.SqlQueryRaw<NSK_FL105090Record>(query.ToString(), parameters));
        }

        /// <summary>
        /// 出力対象が空（0件）か判定する
        /// </summary>
        /// <returns>true:0件、false:0件以外</returns>
        public bool IsEmpty()
        {
            return (records.Count == 0);
        }

        /// <summary>
        /// データ出力処理
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void OutputDataFile(DateTime sysDate, string sysDateTime)
        {
            // ８．１．[変数：条件_文字コード]で指定した文字コードの出力用加入申込書チェックリストデータファイル作成
            // 一時領域にデータ一時出力フォルダとファイルを作成する
            // フォルダ名：[設定ファイル：FILE_TEMP_FOLDER_PATH]/バッチID_yyyyMMddHHmmss/ 
            tempFolderPath = Path.Combine(
                ConfigUtil.Get(
                    NskCommonLibrary.Core.Consts.CoreConst.FILE_TEMP_FOLDER_PATH),
                    $"{arg.BatchId}{CoreLibrary.Core.Consts.CoreConst.SYMBOL_UNDERSCORE}{sysDateTime}");
            Directory.CreateDirectory(tempFolderPath);

            // ファイル名：[変数：条件_ファイル名].txt
            string fileName = $"{batchJoken.FileName}{NskCommonLibrary.Core.Consts.CoreConst.FILE_EXTENSION_TXT}";
            string filePath = Path.Combine(tempFolderPath, fileName);

            // ８．２．加入申込書チェックリストデータ出力
            // ファイル設計書に沿って加入申込書チェックリストデータファイルに加入申込書チェックリストデータを出力する。
            using (FileStream fs = File.Create(filePath))
            {
                // 文字コード
                Encoding encoding = Encoding.Default;
                if (batchJoken.MojiCd == $"{(int)CoreLibrary.Core.Consts.CoreConst.CharacterCode.UTF8}")
                {
                    encoding = Encoding.UTF8;
                }
                else if (batchJoken.MojiCd == $"{(int)CoreLibrary.Core.Consts.CoreConst.CharacterCode.SJIS}")
                {
                    encoding = Encoding.GetEncoding("Shift_JIS");
                }

                using (StreamWriter writer = new(fs, encoding))
                {
                    writer.Write(CsvUtil.GetLine(GetHeaderRow(sysDate)));

                    for (int i = 0; i < records.Count; i++)
                    {
                        writer.Write(CsvUtil.GetLine(records[i].GetValueArray()));
                    }
                }
            }

        }

        /// <summary>
        /// ヘッダ行データ取得
        /// </summary>
        /// <param name="sysDate">システム日付</param>
        /// <returns></returns>
        private string[] GetHeaderRow(DateTime sysDate)
        {
            string jpnSysDate = GetJapaneseDate(sysDate, "ggg y年M月d日");// ggg e\年m\月d\日

            string[] headerRow = [
                // 見出し
                $"{batchJoken.KyosaiMokutekiNm}　共済加入申込書入力ﾁｪｯｸﾘｽﾄ",
                // 年産
                $"{batchJoken.Nensan}",
                // 共済目的コード
                $"{batchJoken.KyosaiMokutekiCd}",
                // 組合等コード
                $"{batchJoken.KumiaitoCd}",
                // 大地区コード
                $"{batchJoken.DaichikuCd}",
                // 大地区名
                $"{batchJoken.DaichikuNm}",
                // 小地区コード(開始)
                $"{batchJoken.ShochikuStart}",
                // 小地区名(開始)
                $"{batchJoken.ShochikuNmStart}",
                // 小地区コード(終了)
                $"{batchJoken.ShochikuEnd}",
                // 小地区名(終了)
                $"{batchJoken.ShochikuNmEnd}",
                // 組合員等コード(開始)
                $"{batchJoken.KumiaiintoCdStart}",
                // 組合員等コード(終了)
                $"{batchJoken.KumiaiintoCdEnd}",
                // 日付
                $"{jpnSysDate}"
            ];

            return headerRow.ToArray();
        }

        /// <summary>
        /// 和暦日付文字取得
        /// </summary>
        /// <param name="date">日付</param>
        /// <param name="format">書式</param>
        /// <returns></returns>
        private string GetJapaneseDate(DateTime date, string format)
        {
            // カルチャの「言語-国/地域」を「日本語-日本」に設定します。
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("ja-JP");
            // 和暦を表すクラスです。
            System.Globalization.JapaneseCalendar jp = new System.Globalization.JapaneseCalendar();
            // 現在のカルチャで使用する暦を、和暦に設定します。
            ci.DateTimeFormat.Calendar = jp;

            string dateString = date.ToString(format, ci);
            return dateString;
        }

        /// <summary>
        /// Zip暗号化
        /// </summary>
        /// <param name="zipFolderPath"></param>
        /// <param name="uid"></param>
        public void EncryptFile(string zipFolderPath, string uid)
        {
            // ８．３．Zip暗号化を行う。
            // ８．３．１．「８．１．」のフォルダ内のテキストをZip化（暗号化）し、
            // Zipファイルを共通部品「FolderUtil.MoveFile」で[変数：ZIPファイル格納先パス]に移動する。
            // ※共通部品「FolderUtil.MoveFile」内で「システム共通スキーマ.バッチダウンロードファイル]へ
            // [変数：ZIPファイル格納先パス] とファイル名でパスを登録します。
            Dictionary<string, string> zipFilePath = ZipUtil.CreateZip(tempFolderPath);
            NskCommonLibrary.Core.Utility.FolderUtil.MoveFile(zipFilePath, zipFolderPath, uid, arg.BatchIdNum);
        }

        /// <summary>
        /// データ一時出力フォルダの削除
        /// </summary>
        public void DeleteTempFolder()
        {
            // ８．３．２．「８．１．」のフォルダを削除する。

            if (Directory.Exists(tempFolderPath))
            {
                Directory.Delete(tempFolderPath, true);
            }
        }

    }
}
