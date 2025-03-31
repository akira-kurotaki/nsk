using NskAppModelLibrary.Context;
using NskWeb.Areas.F000.Consts;
using NskWeb.Areas.F000.Models.D000000;
using NskWeb.Common.Consts;
using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.HelpMenu;
//using CoreLibrary.Core.Menu;
using NskWeb.Common.Menu;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Text;
using System.Configuration;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Dto;
using NskAppModelLibrary.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using NskWeb.Common.NskMenu;
using System.Collections.Generic;
using System.Collections;
using CoreLibrary.Core.DropDown;
using ModelLibrary.Models;
using Microsoft.Extensions.Primitives;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskWeb.Areas.F000.Controllers
{
    /// <summary>
    /// ポータル
    /// </summary>
    /// <remarks>
    /// 作成日：2018/03/07
    /// 作成者：Gon Etuun
    /// </remarks>
    [SessionOutCheck]
    [ExcludeSystemLockCheck]
    [Area("F000")]
    public class D000000Controller : CoreController
    {
        #region メンバー定数
        /// <summary>
        /// 画面ID(D000000)
        /// </summary>
        private static readonly string SCREEN_ID_D000000 = "D000000";

        /// <summary>
        /// View画面ID(D000000)
        /// </summary>
        //private static readonly string VIEW_ID_D000000 = SCREEN_ID_D000000 + "_kbn1";
        private static readonly string VIEW_ID_D000000 = SCREEN_ID_D000000;

        /// <summary>
        /// 画面機能ID(NSKポータル年産変更権限)
        /// </summary>
        private static readonly string FUNC_ID_NSKPTL_NENSAN = SCREEN_ID_D000000 + "_001";

        /// <summary>
        /// セッションキー(D0000)
        /// </summary>
        private static readonly string SESS_D000000 = "D000000_SCREEN";

        /// <summary>
        /// 引受計算支所実行単位区分
        /// </summary>
        private static readonly string HK_KBN_HONHON = "1";

        /// <summary>
        /// 要求ID 引受
        /// </summary>
        private static readonly string REQ_HIKIUKE = "1";
        /// <summary>
        /// 要求ID 評価
        /// </summary>
        private static readonly string REQ_HYOKA = "2";

        public D000000Controller(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }
        #endregion

        #region 初期表示イベント
        /// <summary>
        /// イベント：初期表示
        /// 画面を初期表示する
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Init()
        {

            // モデル初期化
            D000000Model model = new D000000Model();

            // ログインユーザの参照・更新可否判定
            // 画面IDをキーとして、画面マスタ、画面機能権限マスタを参照し、ログインユーザに本画面の権限がない場合は業務エラー画面を表示する。
            if (!ScreenSosaUtil.CanReference(SCREEN_ID_D000000, HttpContext))
            {
                throw new AppException("ME90003", MessageUtil.Get("ME90003"));
            }
            //// 画面に対するユーザの更新可否を取得する。
            //model.CanUpdate = ScreenSosaUtil.CanUpdate(SCREEN_ID_D000000, HttpContext);

            // NSKポータル年産変更権限を取得する。
            model.CanUpdateNensan = ScreenSosaUtil.CanUpdate(FUNC_ID_NSKPTL_NENSAN, HttpContext);

            // パンくずリストを修正する
            SessionUtil.Set(CoreConst.SESS_BREADCRUMB, new List<string>(), HttpContext);

            // 業務アプリ使用のセッションを削除
            SessionUtil.RemoveAll(HttpContext);

            if (SessionUtil.Get<IEnumerable<HelpMenuItem>>(CoreConst.SESS_HELP_MENU_LIST, HttpContext) == null)
            {
                SessionUtil.Set(CoreConst.SESS_HELP_MENU_LIST, HelpMenuUtil.GetHelpMenuItems(HttpContext), HttpContext);
            }


            // ログインユーザの組合等コード、支所コード
            model.TodofukenCd = Syokuin.TodofukenCd;
            model.KumiaitoCd = Syokuin.KumiaitoCd;
            model.ShishoCd = Syokuin.ShishoCd;
            model.ShishoGroupId = Syokuin.ShishoGroupId;
            // 利用可能な支所一覧
            model.ShishoList = ScreenSosaUtil.GetShishoList(HttpContext);

            // 共済目的リスト取得
            model.KyosaiMokutekiList = GetKyosaiMokutekiList();
            // 年産リストの取得
            model = SetNensanModelList(model);

            // セッションからNSKポータル情報を取得
            NSKPortalInfoModel md = SessionUtil.Get<NSKPortalInfoModel>(AppConst.SESS_NSK_PORTAL, HttpContext);
            if (md != null)
            {
                // NSKポータル情報を取得できた場合
                if (string.IsNullOrEmpty(md.SKyosaiMokutekiCd) || string.IsNullOrEmpty(md.SNensanHikiuke) || string.IsNullOrEmpty(md.SNensanHyoka) 
                    || string.IsNullOrEmpty(md.SHikiukeJikkoTanniKbnHikiuke) || string.IsNullOrEmpty(md.SHikiukeJikkoTanniKbnHyoka))
                {
                    throw new SystemException(MessageUtil.Get("MF00005", "セッションから画面情報を取得できない"));
                }
                model.SKyosaiMokutekiCd = md.SKyosaiMokutekiCd;
                model.SNensanHikiuke = md.SNensanHikiuke;
                model.SNensanHikiuke_dmy = md.SNensanHikiuke;
                model.SNensanHyoka = md.SNensanHyoka;
                model.SNensanHyoka_dmy = md.SNensanHyoka;
                model.SHikiukeJikkoTanniKbnHikiuke = md.SHikiukeJikkoTanniKbnHikiuke;
                model.SHikiukeJikkoTanniKbnHyoka = md.SHikiukeJikkoTanniKbnHyoka;

                model.HikiukeNensanList = getNensanSelectList(model.HikiukeNensanModelList, model.SKyosaiMokutekiCd);
                model.HyokaNensanList = getNensanSelectList(model.HyokaNensanModelList, model.SKyosaiMokutekiCd);

                // 年産リストの取得
                model = SetNensanModelList(model);

                // 共済目的コード、年産が確定している場合は、作業状況、とりまとめ状況、引受状況、評価状況を取得する
                model = GetPortalData(model);

            }
            else
            {
                md = new NSKPortalInfoModel();
            }

            // 初期表示情報をセッションに保存する
            SessionUtil.Set(SESS_D000000, model, HttpContext);

            // D000000 ポータルを表示する。
            return View(VIEW_ID_D000000, model);
        }
        #endregion

        #region 決定イベント
        /// <summary>
        /// イベント名：決定
        /// </summary>
        /// <param name="model">加入者一覧モデル</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Decision(D000000Model model)
        {
            string SKyosaiMokutekiCdOld = "";
            string SNensanHikiukeOld = "";
            string SNensanHyokaOld = "";
            // 選択された値を変更不可時の表示用にセット
            model.SNensanHikiuke_dmy = model.SNensanHikiuke;
            model.SNensanHyoka_dmy = model.SNensanHyoka;

            D000000Model md = SessionUtil.Get<D000000Model>(SESS_D000000, HttpContext);
            model.CanUpdateNensan = md.CanUpdateNensan;

            // ログインユーザの組合等コード、支所コード
            model.TodofukenCd = Syokuin.TodofukenCd;
            model.KumiaitoCd = Syokuin.KumiaitoCd;
            model.ShishoCd = Syokuin.ShishoCd;
            model.ShishoGroupId = Syokuin.ShishoGroupId;
            // 利用可能な支所一覧
            model.ShishoList = ScreenSosaUtil.GetShishoList(HttpContext);

            // 共済目的リスト取得
            model.KyosaiMokutekiList = GetKyosaiMokutekiList();
            // 年産リストの取得
            model = SetNensanModelList(model);

            if (string.IsNullOrEmpty(model.SKyosaiMokutekiCd) || string.IsNullOrEmpty(model.SNensanHikiuke) || string.IsNullOrEmpty(model.SNensanHyoka))
            {
                // ひとつでも未入力のときはエラーメッセージを表示
                if (string.IsNullOrEmpty(model.SKyosaiMokutekiCd))
                {
                    model.MessageArea1 = MessageUtil.Get("ME00005", "共済目的","");
                } 
                else if (string.IsNullOrEmpty(model.SNensanHikiuke))
                {
                    model.MessageArea1 = MessageUtil.Get("ME00005", "引受年産","");
                } 
                else if (string.IsNullOrEmpty(model.SNensanHyoka))
                {
                    model.MessageArea1 = MessageUtil.Get("ME00005", "評価年産","");
                }
                SessionUtil.Remove(AppConst.SESS_NSK_PORTAL, HttpContext);
                SessionUtil.Remove(CoreConst.SESS_MENU_LIST, HttpContext);
                return View(VIEW_ID_D000000, model);
            }

            // セッションからNSKポータル情報を取得
            NSKPortalInfoModel m = SessionUtil.Get<NSKPortalInfoModel>(AppConst.SESS_NSK_PORTAL, HttpContext);
            if (m != null)
            {
                // 以前の状態を保存
                SKyosaiMokutekiCdOld = m.SKyosaiMokutekiCd;
                SNensanHikiukeOld = m.SNensanHikiuke;
                SNensanHyokaOld = m.SNensanHyoka;
                // 画面入力の無い項目をセッションから復元
                model.SHikiukeJikkoTanniKbnHikiuke = m.SHikiukeJikkoTanniKbnHikiuke;
                model.SHikiukeJikkoTanniKbnHyoka = m.SHikiukeJikkoTanniKbnHyoka;

            }

            if (!SKyosaiMokutekiCdOld.Equals(model.SKyosaiMokutekiCd) || !SNensanHikiukeOld.Equals(model.SNensanHikiuke) || !SNensanHyokaOld.Equals(model.SNensanHyoka))
            {
                // セッションからNSKポータル情報が取得できなかった時、または共済目的、引受年産、評価年のいずれかが入力値と異なっていた場合は、引受計算支所実行単位区分を取得する
                // 組合等コードを職員情報から取得
                String KumiaitoCd = Syokuin.KumiaitoCd;

                var SHikiukeJikkoTanniKbnHikiuke = getJigyoDb<NskAppContext>().M10010組合等設定s.Where(t => t.組合等コード == KumiaitoCd && t.年産 == int.Parse(model.SNensanHikiuke) && t.共済目的コード == model.SKyosaiMokutekiCd).SingleOrDefault();
                var SHikiukeJikkoTanniKbnHyoka = getJigyoDb<NskAppContext>().M10010組合等設定s.Where(t => t.組合等コード == KumiaitoCd && t.年産 == int.Parse(model.SNensanHyoka) && t.共済目的コード == model.SKyosaiMokutekiCd).SingleOrDefault();

                if (SHikiukeJikkoTanniKbnHikiuke != null && SHikiukeJikkoTanniKbnHyoka != null)
                {
                    model.SHikiukeJikkoTanniKbnHikiuke = SHikiukeJikkoTanniKbnHikiuke.引受計算支所実行単位区分.ToString();
                    model.SHikiukeJikkoTanniKbnHyoka = SHikiukeJikkoTanniKbnHyoka.引受計算支所実行単位区分.ToString();
                }
                else
                {
                    throw new AppException("ME01646", MessageUtil.Get("ME01646", "引受計算支所実行単位区分"));
                }

                // 共済目的が変更された場合、またはメニュー情報がセッションに存在しない場合
                if (!SKyosaiMokutekiCdOld.Equals(model.SKyosaiMokutekiCd) || SessionUtil.Get<IEnumerable<NskMenuItem>>(CoreConst.SESS_MENU_LIST, HttpContext) == null)
                {
                    DbContext dbCom = getJigyoCommonDb();
                    SessionUtil.Set(CoreConst.SESS_MENU_LIST, NskMenuUtil.GetLeftMenuItems(HttpContext, dbCom, model.SKyosaiMokutekiCd), HttpContext);
                }
                // NSKポータル情報をセッション保存
                SessionUtil.Set(AppConst.SESS_NSK_PORTAL, model, HttpContext);

            }
            // 共済目的コード、年産が確定している場合は、作業状況、とりまとめ状況、引受状況、評価状況を取得する
            model = GetPortalData(model);

            model.HikiukeNensanList = getNensanSelectList(model.HikiukeNensanModelList, model.SKyosaiMokutekiCd);
            model.HyokaNensanList = getNensanSelectList(model.HyokaNensanModelList, model.SKyosaiMokutekiCd);

            return View(VIEW_ID_D000000, model);
            //return View("D7208", model);
        }
        #endregion

        #region 共済目的リスト生成
        /// <summary>
        /// 共済目的リスト生成
        /// </summary>
        /// <returns>List<SelectListItem></returns>
        private List<SelectListItem> GetKyosaiMokutekiList()
        {
            var list = new List<SelectListItem>();
            // 先頭にブランクを追加
            SelectListItem item = new SelectListItem();
            item.Value = "";
            item.Text = "";
            list.Add(item);
            // 共済目的コード、名称を取得
            List<M00010共済目的名称> kmlist = getJigyoDb<NskAppContext>().M00010共済目的名称s.ToList();
            foreach (var km in kmlist)
            {
                item = new SelectListItem();
                item.Value = km.共済目的コード;
                item.Text = km.共済目的コード + " " + km.共済目的名称;
                list.Add(item);
            }
            return list;
        }
        #endregion

        #region ポータル画面に表示するデータの取得
        /// <summary>
        /// ポータル画面に表示するデータの取得
        /// </summary>
        /// <param name="model">D000000Model</param>
        /// <returns>D000000Model</returns>
        private D000000Model GetPortalData(D000000Model model)
        {
            model.SagyoJokyoList = GetSagyoJokyoList(model);
            model.Torimatome = GetTorimatome(model);
            if (model.SHikiukeJikkoTanniKbnHikiuke.Equals(HK_KBN_HONHON))
            {
                model.HikiukeHyokaJokyoList_Hiki = GetHikiukeHyokaJokyoListHonsho(model, REQ_HIKIUKE);
            }
            else
            {
                model.HikiukeHyokaJokyoList_Hiki = GetHikiukeHyokaJokyoListShisho(model, REQ_HIKIUKE);
            }
            if (!model.SNensanHikiuke.Equals(model.SNensanHyoka)) {
                if (model.SHikiukeJikkoTanniKbnHyoka.Equals(HK_KBN_HONHON))
                {
                    model.HikiukeHyokaJokyoList_Hyo = GetHikiukeHyokaJokyoListHonsho(model, REQ_HYOKA);
                }
                else
                {
                    model.HikiukeHyokaJokyoList_Hyo = GetHikiukeHyokaJokyoListShisho(model, REQ_HYOKA);
                }
            }
            else
            {
                model.HikiukeHyokaJokyoList_Hyo = null;
            }
            return model;
        }

        /// <summary>
        /// 作業状況の取得
        /// </summary>
        /// <param name="model">D000000Model</param>
        /// <returns>List<D000000SagyoJokyo></returns>
        private List<D000000SagyoJokyo> GetSagyoJokyoList(D000000Model model)
        {
            string inParam = string.Empty;
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();
            sql.Append("SELECT ");
            sql.Append("    T1.支所コード ");
            sql.Append("    , t5.shisho_nm ");
            sql.Append("    , T1.引受回 ");
            sql.Append("    , TO_CHAR(T1.引受計算実施日,'YYYY/MM/DD') 引受計算実施日 ");
            sql.Append("    , TO_CHAR(T3.引受確定実施日,'YYYY/MM/DD') 引受確定実施日 ");
            sql.Append("    , TO_CHAR(T4.当初評価高計算実施日,'YYYY/MM/DD') 当初評価高計算実施日 ");
            //sql.Append("    , T3.引受確定実施日 ");
            //sql.Append("    , T4.当初評価高計算実施日  ");
            sql.Append("FROM ");
            sql.Append("    t_00010_引受回 T1  ");
            sql.Append("    INNER JOIN (  ");
            sql.Append("        SELECT ");
            sql.Append("            支所コード ");
            sql.Append("            , MAX(引受回) 引受回  ");
            sql.Append("        FROM ");
            sql.Append("            t_00010_引受回  ");
            sql.Append("        WHERE ");
            sql.Append("            組合等コード = @KumiaitoCd  ");
            sql.Append("            AND 年産 = @Nensan  ");
            sql.Append("            AND 共済目的コード = @SKyosaiMokutekiCd  ");
            if (model.SHikiukeJikkoTanniKbnHikiuke.Equals(HK_KBN_HONHON))
            {
                // 引受計算実行単位区分=1のとき、本所コードを指定する
                sql.Append("and 支所コード = @HonshoCd ");
                parameters.Add(new NpgsqlParameter("@HonshoCd", AppConst.HONSHO_CD));
            }
            else if (!string.IsNullOrEmpty(inParam = getInParamSishoList(model.ShishoCd, model.ShishoList)))
            {
                // 支所ユーザ（システム利用区分＝支所）のとき
                // 自支所または利用可能支所リストを指定する
                //sql.Append("and 支所コード in (@ShishoList) ");
                //parameters.Add(new NpgsqlParameter("@ShishoList", getInParamSishoList(model.ShishoCd, model.ShishoList)));
                sql.Append("and 支所コード in (" + inParam + ") ");
            }
            sql.Append("        GROUP BY ");
            sql.Append("            支所コード ");
            sql.Append("    ) T2  ");
            sql.Append("        ON T1.支所コード = T2.支所コード  ");
            sql.Append("        AND T1.引受回 = T2.引受回  ");
            sql.Append("    LEFT JOIN t_00020_引受確定 T3  ");
            sql.Append("        ON T1.組合等コード = T3.組合等コード  ");
            sql.Append("        AND T1.年産 = T3.年産  ");
            sql.Append("        AND T1.共済目的コード = T3.共済目的コード  ");
            sql.Append("        AND T1.支所コード = T3.支所コード  ");
            sql.Append("    LEFT JOIN (  ");
            sql.Append("        SELECT ");
            sql.Append("            支所コード ");
            sql.Append("            , MAX(登録日時) 当初評価高計算実施日  ");
            sql.Append("        FROM ");
            sql.Append("            (  ");
            sql.Append("                SELECT ");
            if (model.SHikiukeJikkoTanniKbnHikiuke.Equals(HK_KBN_HONHON))
            {
                sql.Append("                    @HonshoCd 支所コード ");
            }
            else
            {
                sql.Append("                    支所コード ");
            }
            sql.Append("                    , 登録日時  ");
            sql.Append("                FROM ");
            sql.Append("                    t_24270_支所別当初評価集計  ");
            sql.Append("                WHERE ");
            sql.Append("                    組合等コード = @KumiaitoCd  ");
            sql.Append("                    AND 年産 = @Nensan  ");
            sql.Append("                    AND 共済目的コード = @SKyosaiMokutekiCd  ");
            sql.Append("                UNION ALL  ");
            sql.Append("                SELECT ");
            if (model.SHikiukeJikkoTanniKbnHikiuke.Equals(HK_KBN_HONHON))
            {
                sql.Append("                    @HonshoCd 支所コード ");
            }
            else
            {
                sql.Append("                    支所コード ");
            }
            sql.Append("                    , 登録日時  ");
            sql.Append("                FROM ");
            sql.Append("                    t_24290_支所別当初評価高情報  ");
            sql.Append("                WHERE ");
            sql.Append("                    組合等コード = @KumiaitoCd  ");
            sql.Append("                    AND 年産 = @Nensan  ");
            sql.Append("                    AND 共済目的コード = @SKyosaiMokutekiCd ");
            sql.Append("            ) T4W  ");
            sql.Append("        GROUP BY ");
            sql.Append("            支所コード ");
            sql.Append("    ) T4  ");
            sql.Append("        ON T1.支所コード = T4.支所コード  ");
            sql.Append("   LEFT JOIN v_shisho_nm t5 on t5.todofuken_cd = @TodofukenCd and t5.kumiaito_cd = @KumiaitoCd and t5.shisho_cd = t1.支所コード ");
            sql.Append("WHERE ");
            sql.Append("    T1.組合等コード = @KumiaitoCd  ");
            sql.Append("    AND T1.年産 = @Nensan  ");
            sql.Append("    AND T1.共済目的コード = @SKyosaiMokutekiCd  ");
            sql.Append("    ORDER BY T1.支所コード ");
            parameters.Add(new NpgsqlParameter("@TodofukenCd", model.TodofukenCd));
            parameters.Add(new NpgsqlParameter("@KumiaitoCd", model.KumiaitoCd));
            parameters.Add(new NpgsqlParameter("@Nensan", int.Parse(model.SNensanHikiuke)));
            parameters.Add(new NpgsqlParameter("@SKyosaiMokutekiCd", model.SKyosaiMokutekiCd));
            List<D000000SagyoJokyo> sagyoJokyoList = getJigyoDb<NskAppContext>().Database.SqlQueryRaw<D000000SagyoJokyo>(sql.ToString(), parameters.ToArray()).ToList();


            return sagyoJokyoList;
        }

        /// <summary>
        /// とりまとめ状況の取得
        /// </summary>
        /// <param name="model">D000000Model</param>
        /// <returns>D000000Torimatome</returns>
        private D000000Torimatome GetTorimatome(D000000Model model)
        {
            D000000Torimatome torimatome = new D000000Torimatome();
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();
            parameters.Add(new NpgsqlParameter("@KumiaitoCd", model.KumiaitoCd));
            parameters.Add(new NpgsqlParameter("@Nensan", int.Parse(model.SNensanHikiuke)));
            parameters.Add(new NpgsqlParameter("@SKyosaiMokutekiCd", model.SKyosaiMokutekiCd));
            // 報告回、報告実施日の取得
            sql.Append("SELECT ");
            sql.Append("    MAX(T1.報告回) 報告回 ");
            sql.Append("    , TO_CHAR(MAX(T1.報告実施日), 'YYYY/MM/DD') 報告実施日 ");
            sql.Append("FROM ");
            sql.Append("    t_00040_報告回 T1 ");
            sql.Append("WHERE ");
            sql.Append("    T1.組合等コード = @KumiaitoCd  ");
            sql.Append("    AND T1.年産 = @Nensan  ");
            sql.Append("    AND T1.共済目的コード = @SKyosaiMokutekiCd  ");
            Hokokukai Hokokukai = getJigyoDb<NskAppContext>().Database.SqlQueryRaw<Hokokukai>(sql.ToString(), parameters.ToArray()).SingleOrDefault();
            torimatome.報告回 = Hokokukai.報告回;
            torimatome.報告実施日 = Hokokukai.報告実施日;

            // 交付回、交付計算実施日の取得
            sql.Clear();
            sql.Append("SELECT ");
            sql.Append("    MAX(T2.交付回) 交付回 ");
            sql.Append("    , TO_CHAR(MAX(T2.交付計算実施日), 'YYYY/MM/DD') 交付計算実施日 ");
            sql.Append("FROM ");
            sql.Append("    t_00050_交付回 T2 ");
            sql.Append("WHERE ");
            sql.Append("    T2.組合等コード = @KumiaitoCd  ");
            sql.Append("    AND T2.年産 = @Nensan  ");
            sql.Append("    AND T2.共済目的コード = @SKyosaiMokutekiCd ");
            Kofukai Kofukai = getJigyoDb<NskAppContext>().Database.SqlQueryRaw<Kofukai>(sql.ToString(), parameters.ToArray()).SingleOrDefault();
            torimatome.交付回 = Kofukai.交付回;
            torimatome.交付計算実施日 = Kofukai.交付計算実施日;

            // 当初評価高登録日時の取得
            sql.Clear();
            sql.Append("SELECT ");
            sql.Append("    TO_CHAR(MAX(T3.登録日時), 'YYYY/MM/DD') 当初評価高登録日時  ");
            sql.Append("FROM ");
            sql.Append("     (  ");
            sql.Append("        SELECT ");
            sql.Append("            登録日時  ");
            sql.Append("        FROM ");
            sql.Append("            t_24050_組合等当初評価  ");
            sql.Append("        WHERE ");
            sql.Append("            組合等コード = @KumiaitoCd  ");
            sql.Append("            AND 年産 = @Nensan  ");
            sql.Append("            AND 共済目的コード = @SKyosaiMokutekiCd  ");
            sql.Append("        UNION ALL  ");
            sql.Append("        SELECT ");
            sql.Append("            登録日時  ");
            sql.Append("        FROM ");
            sql.Append("            t_24150_組合等当初評価高情報  ");
            sql.Append("        WHERE ");
            sql.Append("            組合等コード = @KumiaitoCd  ");
            sql.Append("            AND 年産 = @Nensan  ");
            sql.Append("            AND 共済目的コード = @SKyosaiMokutekiCd ");
            sql.Append("    ) T3  ");
            Toshohyoka Toshohyoka = getJigyoDb<NskAppContext>().Database.SqlQueryRaw<Toshohyoka>(sql.ToString(), parameters.ToArray()).SingleOrDefault();
            torimatome.当初評価高とりまとめ計算 = Toshohyoka.当初評価高とりまとめ計算;

            // 保険金計算登録日時の取得
            sql.Clear();
            sql.Append("SELECT ");
            sql.Append("    TO_CHAR(MAX(T4.登録日時), 'YYYY/MM/DD') 保険金計算登録日時  ");
            sql.Append("FROM ");
            sql.Append("    t_26010_保険金 T4 ");
            sql.Append("WHERE ");
            sql.Append("    T4.組合等コード = @KumiaitoCd  ");
            sql.Append("    AND T4.年産 = @Nensan  ");
            sql.Append("    AND T4.共済目的コード = @SKyosaiMokutekiCd ");
            HokenkinKeisan HokenkinKeisan = getJigyoDb<NskAppContext>().Database.SqlQueryRaw<HokenkinKeisan>(sql.ToString(), parameters.ToArray()).SingleOrDefault();
            torimatome.保険金計算 = HokenkinKeisan.保険金計算;

            return torimatome;
        }
        private class Hokokukai
        {
            /// <summary>
            /// 報告回
            /// </summary>
            [Column("報告回")]
            public short? 報告回 { get; set; }

            /// <summary>
            /// 報告実施日
            /// </summary>
            [Column("報告実施日")]
            public string 報告実施日 { get; set; }

        }
        private class Kofukai
        {
            /// <summary>
            /// 交付回
            /// </summary>
            [Column("交付回")]
            public short? 交付回 { get; set; }

            /// <summary>
            /// 交付計算実施日
            /// </summary>
            [Column("交付計算実施日")]
            public string 交付計算実施日 { get; set; }

        }
        private class Toshohyoka
        {
            /// <summary>
            /// 登録日時
            /// </summary>
            [Column("当初評価高登録日時")]
            public string 当初評価高とりまとめ計算 { get; set; }

        }
        private class HokenkinKeisan
        {
            /// <summary>
            /// 登録日時
            /// </summary>
            [Column("保険金計算登録日時")]
            public string 保険金計算 { get; set; }
        }

        /// <summary>
        /// 引受/評価状況の取得（本所用）
        /// </summary>
        /// <param name="model">D000000Model</param>
        /// <param name="req">1=引受、2=評価</param>
        /// <returns>List<D000000HikiukeHyokaJokyo></returns>
        private List<D000000HikiukeHyokaJokyo> GetHikiukeHyokaJokyoListHonsho(D000000Model model, string req)
        {

            string inParam = string.Empty;
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();
            sql.Append("with hikiukekai as (  ");
            sql.Append("    select ");
            sql.Append("        支所コード ");
            sql.Append("        , max(共済目的コード) as 共済目的コード ");
            sql.Append("        , max(年産) as 年産 ");
            sql.Append("        , max(引受回) as 引受回  ");
            sql.Append("    from ");
            sql.Append("        t_00010_引受回  ");
            sql.Append("    where ");
            sql.Append("        組合等コード = @KumiaitoCd ");
            sql.Append("        and 引受計算実施日 is not null  ");
            sql.Append("        and 共済目的コード = @SKyosaiMokutekiCd ");
            sql.Append("        and 年産 = @Nensan ");
            sql.Append("        and 支所コード = @HonshoCd ");
            sql.Append("    GROUP BY 支所コード ");
            sql.Append(")  ");
            sql.Append("select  ");
            sql.Append("	v1.shisho_nm 支所 ");
            sql.Append("	,m1.引受方式短縮名  ");
            sql.Append("	,ttt.*  ");
            sql.Append("from (  ");
            sql.Append("  select ");
            sql.Append("    '1' as data_type ");
            sql.Append("    ,t1.支所コード ");
            sql.Append("    ,t1.引受回 ");
            sql.Append("    ,t2.共済目的コード ");
            sql.Append("    ,t2.年産 ");
            sql.Append("    , null 引受方式 ");
            sql.Append("    , t2.組合等計引受戸数 ");
            sql.Append("    , t2.組合等計引受面積 ");
            sql.Append("    , t2.組合等計共済金額 ");
            sql.Append("    , t2.組合等計組合員等負担共済掛金 ");
            sql.Append("    , t2.組合等計賦課金 ");
            sql.Append("    , t3.支払対象被害戸数 ");
            sql.Append("    , t3.支払対象被害面積 ");
            sql.Append("    , t3.支払対象支払共済金見込額 ");
            sql.Append("  from  ");
            sql.Append("    hikiukekai t1 ");
            sql.Append("  left join ( ");
            sql.Append("    select  ");
            sql.Append("        t2.共済目的コード ");
            sql.Append("        ,t2.年産 ");
            sql.Append("        , sum(t2.組合等計引受戸数) as 組合等計引受戸数 ");
            sql.Append("        , sum(t2.組合等計引受面積) as 組合等計引受面積 ");
            sql.Append("        , sum(t2.組合等計共済金額) as 組合等計共済金額 ");
            sql.Append("        , sum(t2.組合等計組合員等負担共済掛金) as 組合等計組合員等負担共済掛金 ");
            sql.Append("        , sum(t2.組合等計賦課金) as 組合等計賦課金 ");
            sql.Append("    from ");
            sql.Append("        hikiukekai t1 ");
            sql.Append("        ,t_13050_支所別引受集計情報 t2 ");
            sql.Append("    where  ");
            sql.Append("        t1.引受回 = t2.引受回 ");
            sql.Append("        and t1.共済目的コード = t2.共済目的コード  ");
            sql.Append("        and t1.年産 = t2.年産 ");
            sql.Append("    group by t2.共済目的コード,t2.年産 ");
            sql.Append("    ) t2 on  ");
            sql.Append("    t1.共済目的コード = t2.共済目的コード  ");
            sql.Append("    and t1.年産 = t2.年産 ");
            sql.Append("  left join ( ");
            sql.Append("    select   ");
            sql.Append("        t3.共済目的コード ");
            sql.Append("        ,t3.年産 ");
            sql.Append("        , sum(t3.支払対象被害戸数) as 支払対象被害戸数 ");
            sql.Append("        , sum(t3.支払対象被害面積) as 支払対象被害面積 ");
            sql.Append("        , sum(t3.支払対象支払共済金見込額) as 支払対象支払共済金見込額  ");
            sql.Append("    from ");
            sql.Append("        hikiukekai t1 ");
            sql.Append("        ,( ");
            sql.Append("        SELECT ");
            sql.Append("            組合等コード ");
            sql.Append("            , 年産 ");
            sql.Append("            , 共済目的コード ");
            sql.Append("            , 類区分 ");
            sql.Append("            , 支所コード ");
            sql.Append("            , 引受方式 ");
            sql.Append("            , 支払対象被害戸数 ");
            sql.Append("            , 支払対象被害面積 ");
            sql.Append("            , 支払対象支払共済金見込額  ");
            sql.Append("        FROM ");
            sql.Append("            t_24270_支所別当初評価集計  ");
            sql.Append("        union all  ");
            sql.Append("        SELECT ");
            sql.Append("            組合等コード ");
            sql.Append("            , 年産 ");
            sql.Append("            , 共済目的コード ");
            sql.Append("            , 類区分 ");
            sql.Append("            , 支所コード ");
            sql.Append("            , case 共済目的コード  ");
            sql.Append("                when '11' then '5'  ");
            sql.Append("                else '4'  ");
            sql.Append("                end as 引受方式 ");
            sql.Append("            , 共済金支払対象_被害組合員等数_合計 as 支払対象被害戸数 ");
            sql.Append("            , 共済金支払対象_引受面積 as 支払対象被害面積 ");
            sql.Append("            , 支払共済金 as 支払対象支払共済金見込額  ");
            sql.Append("        FROM ");
            sql.Append("            t_24290_支所別当初評価高情報 ");
            sql.Append("        WHERE  営農調整フラグ = '1' ");
            sql.Append("        ) t3 ");
            sql.Append("    where  ");
            sql.Append("        t1.共済目的コード = t3.共済目的コード  ");
            sql.Append("        and t1.年産 = t3.年産 ");
            sql.Append("    group by t3.共済目的コード,t3.年産 ");
            sql.Append("  ) t3 on  ");
            sql.Append("    t1.共済目的コード = t3.共済目的コード  ");
            sql.Append("    and t1.年産 = t3.年産 ");
            sql.Append("  union all ");
            sql.Append("  select ");
            sql.Append("    '2' as data_type ");
            sql.Append("    ,t1.支所コード ");
            sql.Append("    ,t1.引受回 ");
            sql.Append("    , t2.共済目的コード ");
            sql.Append("    , t2.年産 ");
            sql.Append("    , t2.引受方式 ");
            sql.Append("    , t2.組合等計引受戸数 ");
            sql.Append("    , t2.組合等計引受面積 ");
            sql.Append("    , t2.組合等計共済金額 ");
            sql.Append("    , t2.組合等計組合員等負担共済掛金 ");
            sql.Append("    , t2.組合等計賦課金 ");
            sql.Append("    , t3.支払対象被害戸数 ");
            sql.Append("    , t3.支払対象被害面積 ");
            sql.Append("    , t3.支払対象支払共済金見込額 ");
            sql.Append("  from  ");
            sql.Append("    hikiukekai t1 ");
            sql.Append("  left join ( ");
            sql.Append("    select  ");
            sql.Append("        t2.共済目的コード ");
            sql.Append("        , t2.年産 ");
            sql.Append("        , t2.引受方式 ");
            sql.Append("        , sum(t2.組合等計引受戸数) as 組合等計引受戸数 ");
            sql.Append("        , sum(t2.組合等計引受面積) as 組合等計引受面積 ");
            sql.Append("        , sum(t2.組合等計共済金額) as 組合等計共済金額 ");
            sql.Append("        , sum(t2.組合等計組合員等負担共済掛金) as 組合等計組合員等負担共済掛金 ");
            sql.Append("        , sum(t2.組合等計賦課金) as 組合等計賦課金 ");
            sql.Append("    from ");
            sql.Append("        hikiukekai t1 ");
            sql.Append("        ,t_13050_支所別引受集計情報 t2 ");
            sql.Append("    where  ");
            sql.Append("        t1.引受回 = t2.引受回 ");
            sql.Append("        and t1.共済目的コード = t2.共済目的コード  ");
            sql.Append("        and t1.年産 = t2.年産 ");
            sql.Append("    group by t2.共済目的コード,t2.年産,t2.引受方式 ");
            sql.Append("    ) t2 on  ");
            sql.Append("    t1.共済目的コード = t2.共済目的コード  ");
            sql.Append("    and t1.年産 = t2.年産 ");
            sql.Append("  left join ( ");
            sql.Append("    select   ");
            sql.Append("        t3.共済目的コード ");
            sql.Append("        , t3.年産 ");
            sql.Append("        , t3.引受方式 ");
            sql.Append("        , sum(t3.支払対象被害戸数) as 支払対象被害戸数 ");
            sql.Append("        , sum(t3.支払対象被害面積) as 支払対象被害面積 ");
            sql.Append("        , sum(t3.支払対象支払共済金見込額) as 支払対象支払共済金見込額  ");
            sql.Append("    from ");
            sql.Append("        hikiukekai t1 ");
            sql.Append("        ,( ");
            sql.Append("        SELECT ");
            sql.Append("            組合等コード ");
            sql.Append("            , 年産 ");
            sql.Append("            , 共済目的コード ");
            sql.Append("            , 類区分 ");
            sql.Append("            , 支所コード ");
            sql.Append("            , 引受方式 ");
            sql.Append("            , 支払対象被害戸数 ");
            sql.Append("            , 支払対象被害面積 ");
            sql.Append("            , 支払対象支払共済金見込額  ");
            sql.Append("        FROM ");
            sql.Append("            t_24270_支所別当初評価集計  ");
            sql.Append("        union all  ");
            sql.Append("        SELECT ");
            sql.Append("            組合等コード ");
            sql.Append("            , 年産 ");
            sql.Append("            , 共済目的コード ");
            sql.Append("            , 類区分 ");
            sql.Append("            , 支所コード ");
            sql.Append("            , case 共済目的コード  ");
            sql.Append("                when '11' then '5'  ");
            sql.Append("                else '4'  ");
            sql.Append("                end as 引受方式 ");
            sql.Append("            , 共済金支払対象_被害組合員等数_合計 as 支払対象被害戸数 ");
            sql.Append("            , 共済金支払対象_引受面積 as 支払対象被害面積 ");
            sql.Append("            , 支払共済金 as 支払対象支払共済金見込額  ");
            sql.Append("        FROM ");
            sql.Append("            t_24290_支所別当初評価高情報 ");
            sql.Append("        WHERE  営農調整フラグ = '1' ");
            sql.Append("        ) t3 ");
            sql.Append("    where  ");
            sql.Append("        t1.共済目的コード = t3.共済目的コード  ");
            sql.Append("        and t1.年産 = t3.年産 ");
            sql.Append("    group by t3.共済目的コード,t3.年産,t3.引受方式 ");
            sql.Append("  ) t3 on  ");
            sql.Append("    t1.共済目的コード = t3.共済目的コード  ");
            sql.Append("    and t1.年産 = t3.年産 ");
            sql.Append("    and t2.引受方式 = t3.引受方式 ");
            sql.Append(") ttt ");
            sql.Append("left join v_shisho_nm v1  ");
            sql.Append("      on v1.todofuken_cd = @TodofukenCd ");
            sql.Append("      and v1.kumiaito_cd = @KumiaitoCd ");
            sql.Append("      and v1.shisho_cd = ttt.支所コード ");
            sql.Append("left join m_10080_引受方式名称 m1 ");
            sql.Append("      on m1.引受方式 = ttt.引受方式 ");
            sql.Append("where ttt.組合等計引受戸数 is not null ");
            sql.Append("order by 支所コード,data_type,引受方式 ");
            parameters.Add(new NpgsqlParameter("@HonshoCd", AppConst.HONSHO_CD));
            parameters.Add(new NpgsqlParameter("@TodofukenCd", model.TodofukenCd));
            parameters.Add(new NpgsqlParameter("@KumiaitoCd", model.KumiaitoCd));
            if (req.Equals(REQ_HIKIUKE))
            {
                parameters.Add(new NpgsqlParameter("@Nensan", int.Parse(model.SNensanHikiuke)));
            }
            else
            {
                parameters.Add(new NpgsqlParameter("@Nensan", int.Parse(model.SNensanHyoka)));
            }
            parameters.Add(new NpgsqlParameter("@SKyosaiMokutekiCd", model.SKyosaiMokutekiCd));
            List<D000000HikiukeHyokaJokyo> JokyoList = getJigyoDb<NskAppContext>().Database.SqlQueryRaw<D000000HikiukeHyokaJokyo>(sql.ToString(), parameters.ToArray()).ToList();

            return JokyoList;
        }

        /// <summary>
        /// 引受/評価状況の取得（支所単位用）
        /// </summary>
        /// <param name="model">D000000Model</param>
        /// <param name="req">1=引受、2=評価</param>
        /// <returns>List<D000000HikiukeHyokaJokyo></returns>
        private List<D000000HikiukeHyokaJokyo> GetHikiukeHyokaJokyoListShisho(D000000Model model, string req)
        {

            string inParam = string.Empty;
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();
            sql.Append("with hikiukekai as (  ");
            sql.Append("    select ");
            sql.Append("        支所コード ");
            sql.Append("        , max(共済目的コード) as 共済目的コード ");
            sql.Append("        , max(年産) as 年産 ");
            sql.Append("        , max(引受回) as 引受回  ");
            sql.Append("    from ");
            sql.Append("        t_00010_引受回  ");
            sql.Append("    where ");
            sql.Append("        組合等コード = @KumiaitoCd  ");
            sql.Append("        and 引受計算実施日 is not null  ");
            sql.Append("        and 共済目的コード = @SKyosaiMokutekiCd  ");
            sql.Append("        and 年産 = @Nensan  ");
            if (!string.IsNullOrEmpty(inParam = getInParamSishoList(model.ShishoCd, model.ShishoList)))
            {
                // 支所ユーザ（システム利用区分＝支所）のとき
                // 自支所または利用可能支所リストを指定する
                //sql.Append("and 支所コード in (@ShishoList) ");
                //parameters.Add(new NpgsqlParameter("@ShishoList", getInParamSishoList(model.ShishoCd, model.ShishoList)));
                sql.Append("and 支所コード in (" + inParam + ") ");
            }
            sql.Append("    GROUP BY 支所コード ");
            sql.Append(")  ");
            sql.Append("select ");
            sql.Append("v1.shisho_nm 支所 ");
            sql.Append(",m1.引受方式短縮名 ");
            sql.Append(",ttt.* ");
            sql.Append("from ( ");
            sql.Append("select ");
            sql.Append("    '1' as data_type ");
            sql.Append("    ,t1.支所コード ");
            sql.Append("    ,t1.引受回 ");
            sql.Append("    , null 引受方式 ");
            sql.Append("    , t2.組合等計引受戸数 ");
            sql.Append("    , t2.組合等計引受面積 ");
            sql.Append("    , t2.組合等計共済金額 ");
            sql.Append("    , t2.組合等計組合員等負担共済掛金 ");
            sql.Append("    , t2.組合等計賦課金 ");
            sql.Append("    , t3.支払対象被害戸数 ");
            sql.Append("    , t3.支払対象被害面積 ");
            sql.Append("    , t3.支払対象支払共済金見込額 ");
            sql.Append("from  ");
            sql.Append("    hikiukekai t1 ");
            sql.Append("left join ( ");
            sql.Append("    select  ");
            sql.Append("        t2.共済目的コード ");
            sql.Append("        ,t2.年産 ");
            sql.Append("        ,t2.支所コード ");
            sql.Append("        , sum(t2.組合等計引受戸数) as 組合等計引受戸数 ");
            sql.Append("        , sum(t2.組合等計引受面積) as 組合等計引受面積 ");
            sql.Append("        , sum(t2.組合等計共済金額) as 組合等計共済金額 ");
            sql.Append("        , sum(t2.組合等計組合員等負担共済掛金) as 組合等計組合員等負担共済掛金 ");
            sql.Append("        , sum(t2.組合等計賦課金) as 組合等計賦課金 ");
            sql.Append("    from ");
            sql.Append("        hikiukekai t1 ");
            sql.Append("        ,t_13050_支所別引受集計情報 t2 ");
            sql.Append("    where  ");
            sql.Append("        t1.引受回 = t2.引受回 ");
            sql.Append("        and t1.共済目的コード = t2.共済目的コード  ");
            sql.Append("        and t1.年産 = t2.年産 ");
            sql.Append("        and t1.支所コード= t2.支所コード ");
            sql.Append("    group by t2.共済目的コード,t2.年産,t2.支所コード ");
            sql.Append("    ) t2 on  ");
            sql.Append("    t1.共済目的コード = t2.共済目的コード  ");
            sql.Append("    and t1.年産 = t2.年産 ");
            sql.Append("    and t1.支所コード=t2.支所コード ");
            sql.Append("left join ( ");
            sql.Append("    select   ");
            sql.Append("        t3.共済目的コード ");
            sql.Append("        ,t3.年産 ");
            sql.Append("        ,t3.支所コード ");
            sql.Append("        , sum(t3.支払対象被害戸数) as 支払対象被害戸数 ");
            sql.Append("        , sum(t3.支払対象被害面積) as 支払対象被害面積 ");
            sql.Append("        , sum(t3.支払対象支払共済金見込額) as 支払対象支払共済金見込額  ");
            sql.Append("    from ");
            sql.Append("        hikiukekai t1 ");
            sql.Append("        ,( ");
            sql.Append("        SELECT ");
            sql.Append("            組合等コード ");
            sql.Append("            , 年産 ");
            sql.Append("            , 共済目的コード ");
            sql.Append("            , 類区分 ");
            sql.Append("            , 支所コード ");
            sql.Append("            , 引受方式 ");
            sql.Append("            , 支払対象被害戸数 ");
            sql.Append("            , 支払対象被害面積 ");
            sql.Append("            , 支払対象支払共済金見込額  ");
            sql.Append("        FROM ");
            sql.Append("            t_24270_支所別当初評価集計  ");
            sql.Append("        union all  ");
            sql.Append("        SELECT ");
            sql.Append("            組合等コード ");
            sql.Append("            , 年産 ");
            sql.Append("            , 共済目的コード ");
            sql.Append("            , 類区分 ");
            sql.Append("            , 支所コード ");
            sql.Append("            , case 共済目的コード  ");
            sql.Append("                when '11' then '5'  ");
            sql.Append("                else '4'  ");
            sql.Append("                end as 引受方式 ");
            sql.Append("            , 共済金支払対象_被害組合員等数_合計 as 支払対象被害戸数 ");
            sql.Append("            , 共済金支払対象_引受面積 as 支払対象被害面積 ");
            sql.Append("            , 支払共済金 as 支払対象支払共済金見込額  ");
            sql.Append("        FROM ");
            sql.Append("            t_24290_支所別当初評価高情報 ");
            sql.Append("        WHERE  営農調整フラグ = '1' ");
            sql.Append("        ) t3 ");
            sql.Append("    where  ");
            sql.Append("        t1.共済目的コード = t3.共済目的コード  ");
            sql.Append("        and t1.年産 = t3.年産 ");
            sql.Append("        and t1.支所コード= t3.支所コード ");
            sql.Append("    group by t3.共済目的コード,t3.年産,t3.支所コード ");
            sql.Append(") t3 on  ");
            sql.Append("    t1.共済目的コード = t3.共済目的コード  ");
            sql.Append("    and t1.年産 = t3.年産 ");
            sql.Append("    and t1.支所コード = t3.支所コード ");
            sql.Append("union all ");
            sql.Append("select ");
            sql.Append("    '2' as data_type ");
            sql.Append("    ,t1.支所コード ");
            sql.Append("    ,t1.引受回 ");
            sql.Append("    , t2.引受方式 ");
            sql.Append("    , t2.組合等計引受戸数 ");
            sql.Append("    , t2.組合等計引受面積 ");
            sql.Append("    , t2.組合等計共済金額 ");
            sql.Append("    , t2.組合等計組合員等負担共済掛金 ");
            sql.Append("    , t2.組合等計賦課金 ");
            sql.Append("    , t3.支払対象被害戸数 ");
            sql.Append("    , t3.支払対象被害面積 ");
            sql.Append("    , t3.支払対象支払共済金見込額 ");
            sql.Append("from  ");
            sql.Append("    hikiukekai t1 ");
            sql.Append("left join ( ");
            sql.Append("    select  ");
            sql.Append("        t2.共済目的コード ");
            sql.Append("        , t2.年産 ");
            sql.Append("        , t2.支所コード ");
            sql.Append("        , t2.引受方式 ");
            sql.Append("        , sum(t2.組合等計引受戸数) as 組合等計引受戸数 ");
            sql.Append("        , sum(t2.組合等計引受面積) as 組合等計引受面積 ");
            sql.Append("        , sum(t2.組合等計共済金額) as 組合等計共済金額 ");
            sql.Append("        , sum(t2.組合等計組合員等負担共済掛金) as 組合等計組合員等負担共済掛金 ");
            sql.Append("        , sum(t2.組合等計賦課金) as 組合等計賦課金 ");
            sql.Append("    from ");
            sql.Append("        hikiukekai t1 ");
            sql.Append("        ,t_13050_支所別引受集計情報 t2 ");
            sql.Append("    where  ");
            sql.Append("        t1.引受回 = t2.引受回 ");
            sql.Append("        and t1.共済目的コード = t2.共済目的コード  ");
            sql.Append("        and t1.年産 = t2.年産 ");
            sql.Append("        and t1.支所コード = t2.支所コード ");
            sql.Append("    group by t2.共済目的コード,t2.年産,t2.支所コード,t2.引受方式 ");
            sql.Append("    ) t2 on  ");
            sql.Append("    t1.共済目的コード = t2.共済目的コード  ");
            sql.Append("    and t1.年産 = t2.年産 ");
            sql.Append("    and t1.支所コード = t2.支所コード ");
            sql.Append("left join ( ");
            sql.Append("    select   ");
            sql.Append("        t3.共済目的コード ");
            sql.Append("        , t3.年産 ");
            sql.Append("        , t3.支所コード ");
            sql.Append("        , t3.引受方式 ");
            sql.Append("        , sum(t3.支払対象被害戸数) as 支払対象被害戸数 ");
            sql.Append("        , sum(t3.支払対象被害面積) as 支払対象被害面積 ");
            sql.Append("        , sum(t3.支払対象支払共済金見込額) as 支払対象支払共済金見込額  ");
            sql.Append("    from ");
            sql.Append("        hikiukekai t1 ");
            sql.Append("        ,( ");
            sql.Append("        SELECT ");
            sql.Append("            組合等コード ");
            sql.Append("            , 年産 ");
            sql.Append("            , 共済目的コード ");
            sql.Append("            , 類区分 ");
            sql.Append("            , 支所コード ");
            sql.Append("            , 引受方式 ");
            sql.Append("            , 支払対象被害戸数 ");
            sql.Append("            , 支払対象被害面積 ");
            sql.Append("            , 支払対象支払共済金見込額  ");
            sql.Append("        FROM ");
            sql.Append("            t_24270_支所別当初評価集計  ");
            sql.Append("        union all  ");
            sql.Append("        SELECT ");
            sql.Append("            組合等コード ");
            sql.Append("            , 年産 ");
            sql.Append("            , 共済目的コード ");
            sql.Append("            , 類区分 ");
            sql.Append("            , 支所コード ");
            sql.Append("            , case 共済目的コード  ");
            sql.Append("                when '11' then '5'  ");
            sql.Append("                else '4'  ");
            sql.Append("                end as 引受方式 ");
            sql.Append("            , 共済金支払対象_被害組合員等数_合計 as 支払対象被害戸数 ");
            sql.Append("            , 共済金支払対象_引受面積 as 支払対象被害面積 ");
            sql.Append("            , 支払共済金 as 支払対象支払共済金見込額  ");
            sql.Append("        FROM ");
            sql.Append("            t_24290_支所別当初評価高情報 ");
            sql.Append("        WHERE  営農調整フラグ = '1' ");
            sql.Append("        ) t3 ");
            sql.Append("    where  ");
            sql.Append("        t1.共済目的コード = t3.共済目的コード  ");
            sql.Append("        and t1.年産 = t3.年産 ");
            sql.Append("        and t1.支所コード = t3.支所コード ");
            sql.Append("    group by t3.共済目的コード,t3.年産,t3.支所コード,t3.引受方式 ");
            sql.Append(") t3 on  ");
            sql.Append("    t1.共済目的コード = t3.共済目的コード  ");
            sql.Append("    and t1.年産 = t3.年産 ");
            sql.Append("    and t1.支所コード = t3.支所コード ");
            sql.Append("    and t2.引受方式 = t3.引受方式 ");
            // 合計部
            sql.Append("union all ");
            sql.Append("select  ");
            sql.Append("    data_type ");
            sql.Append("    , '999' 支所コード ");
            sql.Append("    , 0 引受回 ");
            sql.Append("    , 引受方式 ");
            sql.Append("    , sum(組合等計引受戸数) as 組合等計引受戸数 ");
            sql.Append("    , sum(組合等計引受面積) as 組合等計引受面積 ");
            sql.Append("    , sum(組合等計共済金額) as 組合等計共済金額 ");
            sql.Append("    , sum(組合等計組合員等負担共済掛金) as 組合等計組合員等負担共済掛金 ");
            sql.Append("    , sum(組合等計賦課金) as 組合等計賦課金 ");
            sql.Append("    , sum(支払対象被害戸数) as 支払対象被害戸数 ");
            sql.Append("    , sum(支払対象被害面積) as 支払対象被害面積 ");
            sql.Append("    , sum(支払対象支払共済金見込額) as 支払対象支払共済金見込額  ");
            sql.Append("from ( ");
            sql.Append("  select ");
            sql.Append("    '1' as data_type ");
            sql.Append("    ,t1.支所コード ");
            sql.Append("    ,t2.共済目的コード ");
            sql.Append("    ,t2.年産 ");
            sql.Append("    , null 引受方式 ");
            sql.Append("    , t2.組合等計引受戸数 ");
            sql.Append("    , t2.組合等計引受面積 ");
            sql.Append("    , t2.組合等計共済金額 ");
            sql.Append("    , t2.組合等計組合員等負担共済掛金 ");
            sql.Append("    , t2.組合等計賦課金 ");
            sql.Append("    , t3.支払対象被害戸数 ");
            sql.Append("    , t3.支払対象被害面積 ");
            sql.Append("    , t3.支払対象支払共済金見込額 ");
            sql.Append("  from  ");
            sql.Append("    hikiukekai t1 ");
            sql.Append("  left join ( ");
            sql.Append("    select  ");
            sql.Append("        t2.共済目的コード ");
            sql.Append("        ,t2.年産 ");
            sql.Append("        ,t2.支所コード ");
            sql.Append("        , sum(t2.組合等計引受戸数) as 組合等計引受戸数 ");
            sql.Append("        , sum(t2.組合等計引受面積) as 組合等計引受面積 ");
            sql.Append("        , sum(t2.組合等計共済金額) as 組合等計共済金額 ");
            sql.Append("        , sum(t2.組合等計組合員等負担共済掛金) as 組合等計組合員等負担共済掛金 ");
            sql.Append("        , sum(t2.組合等計賦課金) as 組合等計賦課金 ");
            sql.Append("    from ");
            sql.Append("        hikiukekai t1 ");
            sql.Append("        ,t_13050_支所別引受集計情報 t2 ");
            sql.Append("    where  ");
            sql.Append("        t1.引受回 = t2.引受回 ");
            sql.Append("        and t1.共済目的コード = t2.共済目的コード  ");
            sql.Append("        and t1.年産 = t2.年産 ");
            sql.Append("        and t1.支所コード= t2.支所コード ");
            sql.Append("    group by t2.共済目的コード,t2.年産,t2.支所コード ");
            sql.Append("    ) t2 on  ");
            sql.Append("    t1.共済目的コード = t2.共済目的コード  ");
            sql.Append("    and t1.年産 = t2.年産 ");
            sql.Append("    and t1.支所コード=t2.支所コード ");
            sql.Append("  left join ( ");
            sql.Append("    select   ");
            sql.Append("        t3.共済目的コード ");
            sql.Append("        ,t3.年産 ");
            sql.Append("        ,t3.支所コード ");
            sql.Append("        , sum(t3.支払対象被害戸数) as 支払対象被害戸数 ");
            sql.Append("        , sum(t3.支払対象被害面積) as 支払対象被害面積 ");
            sql.Append("        , sum(t3.支払対象支払共済金見込額) as 支払対象支払共済金見込額  ");
            sql.Append("    from ");
            sql.Append("        hikiukekai t1 ");
            sql.Append("        ,( ");
            sql.Append("        SELECT ");
            sql.Append("            組合等コード ");
            sql.Append("            , 年産 ");
            sql.Append("            , 共済目的コード ");
            sql.Append("            , 類区分 ");
            sql.Append("            , 支所コード ");
            sql.Append("            , 引受方式 ");
            sql.Append("            , 支払対象被害戸数 ");
            sql.Append("            , 支払対象被害面積 ");
            sql.Append("            , 支払対象支払共済金見込額  ");
            sql.Append("        FROM ");
            sql.Append("            t_24270_支所別当初評価集計  ");
            sql.Append("        union all  ");
            sql.Append("        SELECT ");
            sql.Append("            組合等コード ");
            sql.Append("            , 年産 ");
            sql.Append("            , 共済目的コード ");
            sql.Append("            , 類区分 ");
            sql.Append("            , 支所コード ");
            sql.Append("            , case 共済目的コード  ");
            sql.Append("                when '11' then '5'  ");
            sql.Append("                else '4'  ");
            sql.Append("                end as 引受方式 ");
            sql.Append("            , 共済金支払対象_被害組合員等数_合計 as 支払対象被害戸数 ");
            sql.Append("            , 共済金支払対象_引受面積 as 支払対象被害面積 ");
            sql.Append("            , 支払共済金 as 支払対象支払共済金見込額  ");
            sql.Append("        FROM ");
            sql.Append("            t_24290_支所別当初評価高情報 ");
            sql.Append("        WHERE  営農調整フラグ = '1' ");
            sql.Append("        ) t3 ");
            sql.Append("    where  ");
            sql.Append("        t1.共済目的コード = t3.共済目的コード  ");
            sql.Append("        and t1.年産 = t3.年産 ");
            sql.Append("        and t1.支所コード= t3.支所コード ");
            sql.Append("    group by t3.共済目的コード,t3.年産,t3.支所コード ");
            sql.Append("  ) t3 on  ");
            sql.Append("    t1.共済目的コード = t3.共済目的コード  ");
            sql.Append("    and t1.年産 = t3.年産 ");
            sql.Append("    and t1.支所コード = t3.支所コード ");
            sql.Append("union all ");
            sql.Append("  select ");
            sql.Append("    '2' as data_type ");
            sql.Append("    ,t1.支所コード ");
            sql.Append("    , t2.共済目的コード ");
            sql.Append("    , t2.年産 ");
            sql.Append("    , t2.引受方式 ");
            sql.Append("    , t2.組合等計引受戸数 ");
            sql.Append("    , t2.組合等計引受面積 ");
            sql.Append("    , t2.組合等計共済金額 ");
            sql.Append("    , t2.組合等計組合員等負担共済掛金 ");
            sql.Append("    , t2.組合等計賦課金 ");
            sql.Append("    , t3.支払対象被害戸数 ");
            sql.Append("    , t3.支払対象被害面積 ");
            sql.Append("    , t3.支払対象支払共済金見込額 ");
            sql.Append("  from  ");
            sql.Append("    hikiukekai t1 ");
            sql.Append("  left join ( ");
            sql.Append("    select  ");
            sql.Append("        t2.共済目的コード ");
            sql.Append("        , t2.年産 ");
            sql.Append("        , t2.支所コード ");
            sql.Append("        , t2.引受方式 ");
            sql.Append("        , sum(t2.組合等計引受戸数) as 組合等計引受戸数 ");
            sql.Append("        , sum(t2.組合等計引受面積) as 組合等計引受面積 ");
            sql.Append("        , sum(t2.組合等計共済金額) as 組合等計共済金額 ");
            sql.Append("        , sum(t2.組合等計組合員等負担共済掛金) as 組合等計組合員等負担共済掛金 ");
            sql.Append("        , sum(t2.組合等計賦課金) as 組合等計賦課金 ");
            sql.Append("    from ");
            sql.Append("        hikiukekai t1 ");
            sql.Append("        ,t_13050_支所別引受集計情報 t2 ");
            sql.Append("    where  ");
            sql.Append("        t1.引受回 = t2.引受回 ");
            sql.Append("        and t1.共済目的コード = t2.共済目的コード  ");
            sql.Append("        and t1.年産 = t2.年産 ");
            sql.Append("        and t1.支所コード = t2.支所コード ");
            sql.Append("    group by t2.共済目的コード,t2.年産,t2.支所コード,t2.引受方式 ");
            sql.Append("    ) t2 on  ");
            sql.Append("    t1.共済目的コード = t2.共済目的コード  ");
            sql.Append("    and t1.年産 = t2.年産 ");
            sql.Append("    and t1.支所コード = t2.支所コード ");
            sql.Append("  left join ( ");
            sql.Append("    select   ");
            sql.Append("        t3.共済目的コード ");
            sql.Append("        , t3.年産 ");
            sql.Append("        , t3.支所コード ");
            sql.Append("        , t3.引受方式 ");
            sql.Append("        , sum(t3.支払対象被害戸数) as 支払対象被害戸数 ");
            sql.Append("        , sum(t3.支払対象被害面積) as 支払対象被害面積 ");
            sql.Append("        , sum(t3.支払対象支払共済金見込額) as 支払対象支払共済金見込額  ");
            sql.Append("    from ");
            sql.Append("        hikiukekai t1 ");
            sql.Append("        ,( ");
            sql.Append("        SELECT ");
            sql.Append("            組合等コード ");
            sql.Append("            , 年産 ");
            sql.Append("            , 共済目的コード ");
            sql.Append("            , 類区分 ");
            sql.Append("            , 支所コード ");
            sql.Append("            , 引受方式 ");
            sql.Append("            , 支払対象被害戸数 ");
            sql.Append("            , 支払対象被害面積 ");
            sql.Append("            , 支払対象支払共済金見込額  ");
            sql.Append("        FROM ");
            sql.Append("            t_24270_支所別当初評価集計  ");
            sql.Append("        union all  ");
            sql.Append("        SELECT ");
            sql.Append("            組合等コード ");
            sql.Append("            , 年産 ");
            sql.Append("            , 共済目的コード ");
            sql.Append("            , 類区分 ");
            sql.Append("            , 支所コード ");
            sql.Append("            , case 共済目的コード  ");
            sql.Append("                when '11' then '5'  ");
            sql.Append("                else '4'  ");
            sql.Append("                end as 引受方式 ");
            sql.Append("            , 共済金支払対象_被害組合員等数_合計 as 支払対象被害戸数 ");
            sql.Append("            , 共済金支払対象_引受面積 as 支払対象被害面積 ");
            sql.Append("            , 支払共済金 as 支払対象支払共済金見込額  ");
            sql.Append("        FROM ");
            sql.Append("            t_24290_支所別当初評価高情報 ");
            sql.Append("        WHERE  営農調整フラグ = '1' ");
            sql.Append("        ) t3 ");
            sql.Append("    where  ");
            sql.Append("        t1.共済目的コード = t3.共済目的コード  ");
            sql.Append("        and t1.年産 = t3.年産 ");
            sql.Append("        and t1.支所コード = t3.支所コード ");
            sql.Append("    group by t3.共済目的コード,t3.年産,t3.支所コード,t3.引受方式 ");
            sql.Append("  ) t3 on  ");
            sql.Append("    t1.共済目的コード = t3.共済目的コード  ");
            sql.Append("    and t1.年産 = t3.年産 ");
            sql.Append("    and t1.支所コード = t3.支所コード ");
            sql.Append("    and t2.引受方式 = t3.引受方式 ");
            sql.Append(") tt ");
            sql.Append("group by data_type,引受方式 ");
            sql.Append(") ttt ");
            sql.Append("  left join v_shisho_nm v1 ");
            sql.Append("        on v1.todofuken_cd = @TodofukenCd ");
            sql.Append("        and v1.kumiaito_cd = @KumiaitoCd ");
            sql.Append("        and v1.shisho_cd = ttt.支所コード ");
            sql.Append("  left join m_10080_引受方式名称 m1 ");
            sql.Append("        on m1.引受方式 = ttt.引受方式 ");
            sql.Append("where ttt.組合等計引受戸数 is not null ");
            sql.Append("order by 支所コード,data_type,引受方式 ");
            parameters.Add(new NpgsqlParameter("@TodofukenCd", model.TodofukenCd));
            parameters.Add(new NpgsqlParameter("@KumiaitoCd", model.KumiaitoCd));
            if (req.Equals(REQ_HIKIUKE))
            {
                parameters.Add(new NpgsqlParameter("@Nensan", int.Parse(model.SNensanHikiuke)));
            }
            else
            {
                parameters.Add(new NpgsqlParameter("@Nensan", int.Parse(model.SNensanHyoka)));
            }
            parameters.Add(new NpgsqlParameter("@SKyosaiMokutekiCd", model.SKyosaiMokutekiCd));
            List<D000000HikiukeHyokaJokyo> JokyoList = getJigyoDb<NskAppContext>().Database.SqlQueryRaw<D000000HikiukeHyokaJokyo>(sql.ToString(), parameters.ToArray()).ToList();

            return JokyoList;
        }
        #endregion

        #region 年産リスト生成
        /// <summary>
        /// 年産モデルリストのセット
        /// </summary>
        /// <param name="model">D000000Model</param>
        /// <returns>D000000Model</returns>

        private D000000Model SetNensanModelList(D000000Model model)
        {
            model.HikiukeNensanModelList = GetNensanList(REQ_HIKIUKE, model);
            model.HyokaNensanModelList = GetNensanList(REQ_HYOKA, model);

            return model;

        }
        /// <summary>
        /// 年産リストデータの取得
        /// </summary>
        /// <param name="req">1=引受、2=評価</param>
        /// <param name="KyosaiMokutekiCd">共済目的コード</param>
        /// <param name="KumiaitoCd">組合等コード</param>
        /// <returns>List<SelectListItem></returns>
        private List<D000000Nensan> GetNensanList(string req, D000000Model model)
        {
            var list = new List<SelectListItem>();
            //// 先頭にブランクを追加
            //SelectListItem item = null;
            //item = new SelectListItem();
            //item.Value = "";
            //item.Text = "";
            //list.Add(item);

            // t_00010_引受回から指定された共済目的の年産を取得
            string inParam = string.Empty;
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();
            sql.Append("select ");
            sql.Append("共済目的コード ");
            sql.Append(",cast(年産 as character varying(4)) as 年産 ");
            if (req.Equals(REQ_HIKIUKE))
            {
                sql.Append("from t_00010_引受回  ");
            }
            else
            {
                sql.Append("from t_00020_引受確定  ");
            }
            sql.Append("where 組合等コード = @KumiaitoCd ");
            parameters.Add(new NpgsqlParameter("@KumiaitoCd", model.KumiaitoCd));

            if (!string.IsNullOrEmpty(inParam = getInParamSishoList(model.ShishoCd, model.ShishoList)))
            {
                // 本所支所、支所支所で、支所ユーザのときは、条件に支所を入れる
                sql.Append("and 支所コード in (" + inParam + ") ");
            }
            sql.Append("group by 共済目的コード,年産 ");
            sql.Append("order by 共済目的コード,年産 desc ");

            List<D000000Nensan> dlist = getJigyoDb<NskAppContext>().Database.SqlQueryRaw<D000000Nensan>(sql.ToString(), parameters.ToArray()).ToList();

            return dlist;
        }

        /// <summary>
        /// 年産表示用リストの取得
        /// </summary>
        /// <param name="dlist">List<D000000Nensan></param>
        /// <param name="KyosaiMokutekiCd">共済目的コード</param>
        /// <returns>List<SelectListItem></returns>
        private List<SelectListItem> getNensanSelectList(List<D000000Nensan> dlist,string KyosaiMokutekiCd)
        {
            List<D000000Nensan> wlist = dlist.Where(a => a.SKyosaiMokutekiCd == KyosaiMokutekiCd).OrderByDescending(a => a.SNensan).ToList();
            List< SelectListItem > list = new List<SelectListItem>();

            foreach (var md_nensan in wlist)
            {
                SelectListItem item = new SelectListItem();
                item.Value = md_nensan.SNensan;
                item.Text = md_nensan.SNensan;
                list.Add(item);
            }
            return list;
        }

        /// <summary>
        /// 支所をINで検索するための条件値を生成
        /// </summary>
        /// <param name="ShishoCd">支所コード</param>
        /// <param name="ShishoList">利用可能支所リスト</param>
        /// <returns>string </returns>
        private string getInParamSishoList(string ShishoCd, List<Shisho> ShishoList)
        {
            StringBuilder sb = new StringBuilder();
            if (ShishoList.Count > 0)
            {
                // 利用可能支所が設定されているとき、利用可能支所をすべてセット
                string sep = "";
                foreach (Shisho shisho in ShishoList)
                {
                    sb.Append(sep);
                    sb.Append("'" + shisho.ShishoCd + "'");
                    sep = ",";
                }
            }
            else if (!string.IsNullOrEmpty(ShishoCd)) 
            {
                // 利用可能支所が設定されていないとき、かつ自支所が設定されているとき自支所をセット
                sb.Append("'" + ShishoCd + "'");
            }
            return sb.ToString();

        }
        #endregion

        #region 年産ドロップダウンリストデータ取得
        /// <summary>
        /// 年産ドロップダウンリストデータ取得
        /// </summary>
        /// <param name="req">1=引受、2=評価</param>
        /// <param name="KyosaiMokutekiCd">共済目的コード</param>
        /// <returns>json(List<SelectListItem>)</returns>
        [HttpPost]
        public ActionResult GetNensanDropdownList([Bind("req","SKyosaiMokutekiCd"), FromBody] D000000ChangeNensanModel model)
        { 
            D000000Model m = SessionUtil.Get<D000000Model>(SESS_D000000, HttpContext);
            List<SelectListItem> NensanList = new List<SelectListItem>(); 

            if (model.req.Equals(REQ_HIKIUKE)) {
                NensanList = getNensanSelectList(m.HikiukeNensanModelList, model.SKyosaiMokutekiCd);
            }
            else
            {
                NensanList = getNensanSelectList(m.HyokaNensanModelList, model.SKyosaiMokutekiCd);
            }
            return Json(NensanList);
        }
        #endregion

    }

}