using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Utility;
using GrapeCity.ActiveReports.Export.Pdf.Section;
using Microsoft.VisualBasic;
using ModelLibrary.Context;
using ModelLibrary.Models;
using NLog;

namespace NskReportLibrary.Core.Base
{
    /// <summary>
    /// Controllerベースクラス
    /// 
    /// 作成日：2017/12/11
    /// 作成者：KAN RI
    /// </summary>
    public class ReportController : IDisposable
    {
        #region クラス変数
        /// <summary>
        /// 事業共通DBに接続するDBコンテキストクラス
        /// </summary>
        private JigyoCommonContext jigyoCommonDb;

        /// <summary>
        /// システム共通DBに接続するDBコンテキストクラス
        /// </summary>
        private SystemCommonContext systemCommonDb;

        /// <summary>
        /// 農業者情報管理スキーマに接続するDBコンテキスト
        /// </summary>
        private FimContext fimDb;

        /// <summary>
        /// 都道府県別事業スキーマに接続するDBコンテキスト
        /// </summary>
        private JigyoContext jigyoDb;

        /// <summary>
        /// PDFを作成するクラス
        /// </summary>
        protected PdfExport pdfExport = new PdfExport();

        /// <summary>
        /// ロガー
        /// </summary>
        protected static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 汎用区分マスタリスト
        /// </summary>
        //protected List<VHanyokubun> hanyokubuns = null;

        /// <summary>
        /// 名称マスタリスト
        /// </summary>
        //protected List<MMeisho> meishos = null;

        /// <summary>
        /// 単位マスタリスト
        /// </summary>
        //protected List<MTani> tanis = null;

        /// <summary>
        /// 都道府県別事業スキーマの接続先
        /// </summary>
        private DbConnectionInfo dbInfo = null;

        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        protected ReportController(DbConnectionInfo dbInfo)
        {
            //hanyokubuns = GetHanyokubunList();
            //meishos = GetMeishoList();
            //tanis = GetTaniList();

            this.dbInfo = dbInfo;
        }
        #endregion

        /// <summary>
        /// 事業共通DBに接続するDBコンテキストを取得する
        /// </summary>
        /// <returns>事業共通DBに接続するDBコンテキスト</returns>
        protected JigyoCommonContext getJigyoCommonDb()
        {
            if (jigyoCommonDb == null)
            {
                jigyoCommonDb = new JigyoCommonContext();
            }
            return jigyoCommonDb;
        }

        /// <summary>
        /// システム共通DBに接続するDBコンテキストを取得する
        /// </summary>
        /// <returns>システム共通DBに接続するDBコンテキスト</returns>
        protected SystemCommonContext getSystemCommonDb()
        {
            if (systemCommonDb == null)
            {
                systemCommonDb = new SystemCommonContext();
            }
            return systemCommonDb;
        }

        /// <summary>
        /// 都道府県別事業スキーマに接続するDBコンテキストを取得する
        /// 事業システム向け
        /// </summary>
        /// <typeparam name="T">JigyoContext継承クラス</typeparam>
        /// <returns>都道府県別事業スキーマに接続するDBコンテキスト</returns>
        protected T getJigyoDb<T>() where T : JigyoContext
        {
            if (jigyoDb == null)
            {
                jigyoDb = (JigyoContext)Activator.CreateInstance(typeof(T), 
                                                                    dbInfo.ConnectionString,
                                                                    dbInfo.DefaultSchema,
                                                                    ConfigUtil.GetInt("CommandTimeout"));
            }

            return (T)jigyoDb;
        }

        /// <summary>
        /// システム共通スキーマからログインユーザの所属に応じた都道府県別事業スキーマのDB接続先を取得する
        /// </summary>
        /// <param name="systemKbn">システム区分</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <returns>DB接続先情報</returns>
        protected DbConnectionInfo GetDbConnectionInfo(string todofukenCd, string kumiaitoCd, string shishoCd)
        {
            // システム区分
            string systemKbn = ConfigUtil.Get(CoreConst.APP_ENV_SYSTEM_KBN);

            // システム共通スキーマからユーザの所属に応じた都道府県別事業スキーマのDB接続先を取得する
            return DBUtil.GetDbConnectionInfo(systemKbn, todofukenCd, kumiaitoCd, shishoCd);
        }

        #region ユーザIDをキーにして、職員マスタを取得するメソッド
        /// <summary>
        /// ユーザIDをキーにして、都道府県別事業スキーマから職員マスタを取得する
        /// </summary>
        /// <param name="userId">ユーザID</param>
        /// <returns>ユーザIDに対応する職員マスタ情報</returns>
        protected VSyokuin GetUserInfo(JigyoContext context, string userId)
        {
            // システム日時
            var systemDate = DateUtil.GetSysDateTime();

            // 都道府県別事業スキーマから職員情報を取得する
            logger.Info("都道府県別事業スキーマから職員情報を取得する。（ユーザーID;" + userId + "）");
            var userInfo = context.VSyokuins.Where(
                                s => s.UserId == userId &&
                                s.UserYukoStartYmd <= systemDate.Date &&
                                (s.UserYukoEndYmd == null || s.UserYukoEndYmd > systemDate.Date)).SingleOrDefault();
            return userInfo;
        }
        #endregion

        #region 帳票条件IDをキーにして、帳票条件を取得するメソッド
        /// <summary>
        /// 帳票条件IDをキーにして、帳票条件を取得する
        /// </summary>
        /// <param name="joukenId">帳票条件ID</param>
        /// <returns>帳票条件リスト</returns>
        protected List<TReportJouken> GetReportJouken(JigyoContext context, int joukenId)
        {
            logger.Info("帳票条件IDをキーにして、帳票条件を取得する。（条件ID：" + joukenId + "）");
            var reportJouken = context.TReportJoukens.Where(p => p.JoukenId == joukenId).OrderBy(p => p.SerialNumber).ToList();
            return reportJouken;
        }
        #endregion

        #region 予約IDをキーにして、バッチ予約を取得するメソッド
        /// <summary>
        /// 予約IDをキーにして、バッチ予約を取得する
        /// </summary>
        /// <param name="joukenId">予約ID</param>
        /// <returns>バッチ予約</returns>
        //protected TBatchYoyaku GetBatchYoyaku(long yoyakuId)
        //{
        //    var query =
        //        from a in context.TBatchYoyakus
        //        from b in context.MReportKanris.Where(p => p.ReportControlId.Equals(a.ReportControlId) && p.SerialNumber.Equals(1)).DefaultIfEmpty()
        //        where a.YoyakuId == yoyakuId
        //        select new
        //        {
        //            a.YoyakuDate,
        //            a.YoyakuNm,
        //            b.ReportControlNm
        //        };

        //    return query.ToList().Select(p => new TBatchYoyaku
        //    {
        //        YoyakuDate = p.YoyakuDate,
        //        YoyakuNm = string.IsNullOrEmpty(p.YoyakuNm) ? p.ReportControlNm : p.YoyakuNm
        //    }).Single();
        //}
        #endregion

        #region 文字列から指定した位置の文字を取得するメソッド
        /// <summary>
        /// 文字列から指定した位置の文字を取得するメソッド
        /// </summary>
        /// <param name="value">文字列</param>
        /// <param name="position">位置</param>
        /// <param name="length">取得文字長さ</param>
        /// <returns>文字</returns>
        protected string GetCharsByPosition(string value, int position, int length = 1)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            if (value.Length > position)
            {
                if (value.Length > position + length)
                {
                    return value.Substring(position, length);
                }
                else
                {
                    return value.Substring(position);
                }
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion

        #region 操作履歴を登録するメソッド
        /// <summary>
        /// 操作履歴登録
        /// </summary>
        /// <param name="keiyakuIds">対象契約IDリスト</param>
        /// <param name="controllerId">コントローラID</param>
        /// <param name="gyomu">業務</param>
        /// <param name="sosaNaiyo">操作内容</param>
        /// <param name="displayColor">表示色区分</param>
        /// <param name="userId">ユーザID</ param >
        /// <param name="systemRiyoKbn">ユーザIDのシステム利用者区分</param>
        /// <returns>True：成功　False：失敗</returns>
        //protected bool RegisterHistory(
        //    List<int> keiyakuIds,
        //    string controllerId,
        //    string gyomu,
        //    string sosaNaiyo,
        //    string displayColor,
        //    string userId,
        //    string systemRiyoKbn
        //    )
        //{
        //    try
        //    {
        //        var tKeiyakus = context.TKeiyakus.Where(a => keiyakuIds.Contains(a.KeiyakuId)).ToList();
        //        int cnt = 0;
        //        logger.Info("作成区分＝バッチ作成中：更新開始");
        //        foreach (var item in tKeiyakus)
        //        {
        //            TSosaRireki sosaRireki = new TSosaRireki()
        //            {
        //                KeiyakuId = item.KeiyakuId,
        //                RirekiNo = Convert.ToInt16(item.LastRirekiNo),
        //                SosaDate = DateUtil.GetSysDateTime(),
        //                UserId = userId,
        //                SystemRiyoKbn = systemRiyoKbn,
        //                ScreenId = controllerId,
        //                Gyomu = gyomu,
        //                SosaNaiyo = sosaNaiyo,
        //                DisplayColor = displayColor
        //            };

        //            context.TSosaRirekis.Add(sosaRireki);

        //            // 500件ずつInfoログを出力する。
        //            if (++cnt % 500 == 0)
        //            {
        //                logger.Info(string.Format("操作履歴：{0}件登録完了（未コミット）", cnt));
        //            }
        //        }

        //        context.SaveChanges();
        //        logger.Info(string.Format("操作履歴：{0}件登録完了", cnt));
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(e);
        //        return false;
        //    }
        //    return true;
        //}
        #endregion

        #region 備考を設定するメソッド
        /// <summary>
        /// 備考を設定するメソッド
        /// </summary>
        /// <param name="biko">備考</param>
        /// <returns>備考</returns>
        //protected string SetBiko(string biko)
        //{
        //    return string.IsNullOrEmpty(biko) ? string.Empty : (biko.Length > 18 ? ReportConst.STRING_BESSHI : biko);
        //}
        #endregion

        #region 「別紙参照」を設定する(指定した文字数を超える場合)メソッド
        /// <summary>
        /// 「別紙参照」を設定する(指定した文字数を超える場合)メソッド
        /// </summary>
        /// <param name="data">データ</param>
        /// <returns>変換後のデータ</returns>
        //protected string ConvertBesshi(string data, int length)
        //{
        //    return string.IsNullOrEmpty(data) ? string.Empty : (data.Length > length ? ReportConst.STRING_BESSHI : data);
        //}
        #endregion

        #region 郵便番号を全角にフォーマットするメソッド
        /// <summary>
        /// 郵便番号を全角にフォーマットするメソッド
        /// </summary>
        /// <param name="postalCd">郵便番号</param>
        /// <returns>郵便番号(９９９ー９９９９)</returns>
        protected string FormatPostalCdToWide(string postalCd)
        {
            if (string.IsNullOrEmpty(postalCd))
            {
                return string.Empty;
            }

            if (postalCd.Length > 3)
            {
                postalCd = Microsoft.VisualBasic.Strings.StrConv(postalCd, VbStrConv.Wide, 0x0411);
                return postalCd.Substring(0, 3) + "ー" + postalCd.Substring(3);
            }
            else
            {
                return Microsoft.VisualBasic.Strings.StrConv(postalCd, VbStrConv.Wide, 0x0411);
            }
        }
        #endregion

        #region 汎用区分マスタの取得メソッド
        /// <summary>
        /// 汎用区分マスタの取得メソッド。
        /// </summary>
        /// <returns>汎用区分リスト</returns>
        //protected List<VHanyokubun> GetHanyokubunList()
        //{
        //    return context.VHanyokubuns.ToList();
        //}
        #endregion

        #region 名称マスタの取得メソッド
        /// <summary>
        /// 名称マスタの取得メソッド。
        /// </summary>
        /// <returns>名称リスト</returns>
        //protected List<MMeisho> GetMeishoList()
        //{
        //    // 会長理事名の取得(会長理事名の名称種別は"001")
        //    IEnumerable<MMeisho> meishos = context.MMeishos.Where(p => p.NmSbt == "001");

        //    return meishos.ToList();
        //}
        #endregion

        #region 単位マスタの取得メソッド
        /// <summary>
        /// 単位マスタの取得メソッド。
        /// </summary>
        /// <returns>単位リスト</returns>
        //protected List<MTani> GetTaniList()
        //{
        //    // 単位マスタリスト
        //    IEnumerable<MTani> tanis = syndb.MTanis.Where(p => p.DeleteFlg == "0");

        //    return tanis.ToList();
        //}
        #endregion

        #region 区分名称の取得メソッド
        /// <summary>
        /// 区分名称の取得メソッド。
        /// </summary>
        /// <param name="kbnSbt">区分種別</param>
        /// <param name="kbnCd">区分コード</param>
        /// <returns>区分名称</returns>
        //protected string GetKbnNm(string kbnSbt, string kbnCd)
        //{
        //    if (string.IsNullOrEmpty(kbnSbt))
        //    {
        //        return null;
        //    }
        //    if (string.IsNullOrEmpty(kbnCd))
        //    {
        //        return string.Empty;
        //    }

        //    var hanyokubun = hanyokubuns.Where(p => p.KbnSbt == kbnSbt && p.KbnCd == kbnCd).FirstOrDefault();

        //    return hanyokubun == null ? string.Empty : hanyokubun.KbnNm;
        //}
        #endregion

        #region 会長理事名の取得メソッド
        /// <summary>
        /// 会長理事名の取得メソッド。
        /// </summary>
        /// <param name="tekiyoYmd">適用年月日</param>
        /// <returns>会長理事名</returns>
        //protected string GetKaichoRijiNm(DateTime? tekiyoYmd)
        //{
        //    if (meishos == null)
        //    {
        //        return string.Empty;
        //    }

        //    var meisho = meishos.Where(p => p.TekiyoStartYmd <= tekiyoYmd).OrderByDescending(p => p.TekiyoStartYmd).FirstOrDefault();
        //    if (meisho != null)
        //    {
        //        return meisho.Nm;
        //    }
        //    else
        //    {
        //        return string.Empty;
        //    }
        //}
        #endregion

        #region 単位名称の取得メソッド
        /// <summary>
        /// 単位名称の取得メソッド。
        /// </summary>
        /// <param name="taniCd">単位コード</param>
        /// <returns>単位名称</returns>
        //protected string GetTaniNm(string taniCd)
        //{
        //    if (tanis == null || string.IsNullOrEmpty(taniCd))
        //    {
        //        return string.Empty;
        //    }

        //    var tani = tanis.Where(p => p.TaniCd == taniCd).FirstOrDefault();
        //    if (tani != null)
        //    {
        //        return tani.TaniNm;
        //    }
        //    else
        //    {
        //        return string.Empty;
        //    }
        //}
        #endregion

        #region 単位レコードの取得メソッド
        /// <summary>
        /// 単位レコードの取得メソッド。
        /// </summary>
        /// <param name="taniCd">単位コード</param>
        /// <returns>単位レコード</returns>
        //protected MTani GetTani(string taniCd)
        //{
        //    if (tanis == null || string.IsNullOrEmpty(taniCd))
        //    {
        //        return null;
        //    }

        //    var tani = tanis.Where(p => p.TaniCd == taniCd).FirstOrDefault();
        //    if (tani != null)
        //    {
        //        return tani;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        #endregion

        #region 数値型加算(両方nullの場合nullを返す)メソッド
        /// <summary>
        /// 数値型加算(両方nullの場合nullを返す)メソッド
        /// </summary>
        /// <param name="d1">数値型データ1</param>
        /// <param name="d2">数値型データ2</param>
        /// <returns>加算した結果</returns>
        protected decimal? AddConvertNull(decimal? d1, decimal? d2)
        {
            if (d1 == null && d2 == null)
            {
                return null;
            }
            else
            {
                return Decimal.Add(d1 ?? Decimal.Zero, d2 ?? Decimal.Zero);
            }
        }
        #endregion

        #region 数値型減算(両方nullの場合nullを返す)メソッド
        /// <summary>
        /// 数値型減算(両方nullの場合nullを返す)メソッド
        /// </summary>
        /// <param name="d1">数値型データ1</param>
        /// <param name="d2">数値型データ2</param>
        /// <returns>減算した結果</returns>
        protected decimal? SubtractConvertNull(decimal? d1, decimal? d2)
        {
            if (d1 == null && d2 == null)
            {
                return null;
            }
            else
            {
                return Decimal.Subtract(d1 ?? Decimal.Zero, d2 ?? Decimal.Zero);
            }
        }
        #endregion

        #region 帳票の処理対象分割処理メソッド
        /// <summary>
        /// 帳票の処理対象分割処理メソッド
        /// </summary>
        /// <param name="keiyakuIds">処理対象リスト</param>
        /// <param name="splitCount">分割件数</param>
        /// <returns>分割したリスト</returns>
        protected List<List<T>> SplitList<T>(List<T> list, int splitCount)
        {
            var idLists = new List<List<T>>();

            for (int i = 0; i < list.Count; i += splitCount)
            {
                var currentSplitList = list.GetRange(i, Math.Min(splitCount, list.Count - i));
                idLists.Add(currentSplitList);
            }

            return idLists;
        }
        #endregion

        #region SQLのパラメータARRAY類型作成
        /// <summary>
        /// 「(1,2022),(2,2022)」のような文字列をTupleに変更する。
        /// </summary>
        /// <param name="input">「(1,2022),(2,2022)」のような文字列</param>
        /// <returns>Tupleに変更した結果</returns>
        protected List<Tuple<int, short>> TupleTableKey(string input)
        {
            string[] pairs = input.Split(new[] { "), (" }, StringSplitOptions.RemoveEmptyEntries);
            var tupleList = new List<Tuple<int, short>>();
            foreach (var pair in pairs)
            {
                string cleanPair = pair.Trim('(', ')');
                string[] values = cleanPair.Split(',');
                int nogyoshaId = int.Parse(values[0].Trim());
                short nendo = short.Parse(values[1].Trim());
                tupleList.Add(Tuple.Create(nogyoshaId, nendo));
            }

            return tupleList;
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false;

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (jigyoCommonDb != null)
                    {
                        jigyoCommonDb.Dispose();
                    }

                    if (systemCommonDb != null)
                    {
                        systemCommonDb.Dispose();
                    }

                    if (jigyoDb != null)
                    {
                        jigyoDb.Dispose();
                    }

                    if (fimDb != null)
                    {
                        fimDb.Dispose();
                    }
                }
                disposedValue = true;
            }
        }

        ~ReportController()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
    }