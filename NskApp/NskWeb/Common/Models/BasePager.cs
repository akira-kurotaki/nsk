using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Pager;
using NskAppModelLibrary.Context;
using System.Linq;

namespace NskWeb.Common.Models
{
    /// <summary>
    /// ページャー抽象クラス
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public abstract class BasePager<T> : IPager<T> where T : new()
    {
        /// <summary>ページャー</summary>
        public Pagination Pager { get; set; } = new();

        /// <summary>表示用検索結果</summary>
        public List<T> DispRecords { get; set; } = new();

        /// <summary>表示件数</summary>
        public int DisplayCount { get; set; } = CoreConst.PAGE_SIZE;

        /// <summary>検索結果（一時保存用）</summary>
        private List<T> _allRecords = new();

        /// <summary>検索結果全件数（hidden情報）</summary>
        public int AllRecCount { get; set; } = 0;

        /// <summary>
        /// ページ分データを取得する
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="sessionInfo">セッション情報</param>
        /// <param name="pageId">ページID</param>
        public void GetPageDataList(NskAppContext dbContext, BaseSessionInfo sessionInfo, int pageId)
        {
            // 検索結果をクリアする
            DispRecords = new();

            // 検索結果件数を取得する
            _allRecords = GetResult(dbContext, sessionInfo);
            AllRecCount = _allRecords.Count;

            // 検索結果ページ分の取得
            int offset = DisplayCount * (pageId - 1);
            DispRecords = _allRecords.Skip(offset).Take(DisplayCount).ToList();

            // ページャー更新
            UpdatePager(pageId);

        }

        /// <summary>
        /// ページデータを1行分追加し、ページ分取得する
        /// </summary>
        public void AddPageData()
        {
            // 新規行を追加する
            T newRow = new();
            if (newRow is BasePagerRecord pagerRec)
            {
                pagerRec.IsNewRec = true;
            }

            int chkCnt = 0;
            T? checkedFirstRec = default(T);
            foreach (T record in DispRecords)
            {
                if (record is BasePagerRecord dispRec)
                {
                    // 削除行以外でチェックがついている行をカウント
                    if (!dispRec.IsDelRec && dispRec.CheckSelect)
                    {
                        chkCnt++;

                        if (checkedFirstRec is null)
                        {
                            checkedFirstRec = record;
                        }
                    }
                }
            }
            if (chkCnt == 0)
            {
                // 末尾に追加
                DispRecords.Add(newRow);
            }
            else if (chkCnt == 1)
            {
                // 選択行の後に追加
                int secRecIdx = DispRecords.IndexOf(checkedFirstRec);
                DispRecords.Insert(secRecIdx + 1, newRow);
            }

            // ページャー更新
            UpdatePager(Pager.CurrentPage);
        }

        /// <summary>
        /// チェックされたデータを削除し、ページ分取得する
        /// </summary>
        public void DelPageData()
        {
            foreach (T rec in DispRecords)
            {
                if (rec is BasePagerRecord pagerRec && pagerRec.CheckSelect)
                {
                    pagerRec.IsDelRec = true;
                }
            }

            // ページャー更新
            UpdatePager(Pager.CurrentPage);
        }

        /// <summary>
        /// ページャー更新
        /// </summary>
        /// <param name="pageId">ページ</param>
        private void UpdatePager(int pageId)
        {
            // 検索件数を画面表示用モデルに設定する

            int delCount = 0;
            int addCount = 0;
            foreach (T rec in DispRecords)
            {
                if (rec is BasePagerRecord pagerRec)
                {
                    if (pagerRec.IsNewRec)
                    {
                        // 追加行数カウント
                        addCount++;
                    }
                    if (pagerRec.IsDelRec)
                    {
                        // 削除行数カウント
                        delCount++;
                    }
                }
            }
            // 全件数 ＝ 検索件数 ＋ 追加行数 － 削除行数
            int totalCount = AllRecCount + addCount - delCount;

            // 追加行によりページ表示数を超過した場合の超過件数を取得する
            int overflow = 0;
            if (addCount > 0 && totalCount > (DisplayCount * pageId))
            {
                // 超過件数 ＝ (表示データ件数 － 削除行数) － 表示件数
                overflow = (DispRecords.Count - delCount) - DisplayCount;
            }

            // ページに0が指定された場合、1ページ目とする
            pageId = pageId == 0 ? 1 : pageId;

            // ページャーの初期化
            Pager = totalCount == 0 ? new() : new(pageId, DisplayCount, totalCount);

            // 超過件数がある場合
            if (overflow > 0)
            {
                // 表示件数(To)に超過件数を加算する
                Pager.CurrentPageTo += overflow;

                // 表示件数(To)が全件数の場合
                if (Pager.CurrentPageTo == Pager.TotalCount)
                {
                    // 最大ページを減算する
                    Pager.MaxPage--;
                }

                // 表示件数に超過件数を加算する
                Pager.PageSize += overflow;
            }
        }

        /// <summary>
        /// 検索結果を取得する<br/>
        /// 本クラスを継承するクラス側でメソッドを実装すること
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="sessionInfo">セッション情報</param>
        /// <returns></returns>
        public abstract List<T> GetResult(NskAppContext dbContext, BaseSessionInfo sessionInfo);

        /// <summary>
        /// 削除対象レコード取得
        /// </summary>
        /// <returns></returns>
        public List<T> GetDeleteRecs()
        {
            List<T> delRecs = new();
            foreach (T rec in DispRecords)
            {
                if (rec is BasePagerRecord pagerRec)
                {
                    // 追加行でない削除行をリストに追加する
                    if (pagerRec.IsDelRec && !pagerRec.IsNewRec)
                    {
                        delRecs.Add(rec);
                    }
                }
            }
            return delRecs;
        }

        /// <summary>
        /// 追加対象レコード取得
        /// </summary>
        /// <returns></returns>
        public List<T> GetAddRecs()
        {
            List<T> addRecs = new();
            foreach (T rec in DispRecords)
            {
                if (rec is BasePagerRecord pagerRec)
                {
                    // 追加行で削除されていない行をリストに追加する
                    if (pagerRec.IsNewRec && !pagerRec.IsDelRec)
                    {
                        addRecs.Add(rec);
                    }
                }
            }
            return addRecs;
        }

        /// <summary>
        /// 更新対象レコード取得<br/>
        /// 本クラスを継承するクラス側でメソッドを実装すること
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="sessionInfo">セッション情報</param>
        /// <returns></returns>
        public abstract List<T> GetUpdateRecs(ref NskAppContext dbContext, BaseSessionInfo sessionInfo);
    }
}
