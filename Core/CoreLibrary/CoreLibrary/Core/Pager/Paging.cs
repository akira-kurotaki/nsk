using CoreLibrary.Core.Consts;
using System.Text;

namespace CoreLibrary.Core.Pager
{
    /// <summary>
    /// ページャーUI構築クラス
    /// </summary>
    /// <remarks>
    /// 作成日：2017/11/21
    /// 作成者：Rou I
    /// </remarks>
    [Serializable]
    public class Paging
    {
        /// <summary>
        /// ページャーUIのインスタンス
        /// </summary>
        private readonly IPagination pagination;

        /// <summary>
        /// 最終ページリンクの表示文字列
        /// </summary>
        private readonly string firstString = "&laquo;";

        /// <summary>
        /// 最終ページリンクの表示文字列
        /// </summary>
        private readonly string lastString = "&raquo;";

        /// <summary>
        /// 前ページリンクの表示文字列
        /// </summary>
        private readonly string prevString = "&lt;";

        /// <summary>
        /// 次ページリンクの表示文字列
        /// </summary>
        private readonly string nextString = "&gt;";

        /// <summary>
        /// ページャーのスタイル
        /// </summary>
        private readonly string pagerCss = "paging";

        /// <summary>
        /// 現在ページのスタイル
        /// </summary>
        private readonly string currentCss = "active";

        /// <summary>
        /// 現在ページ表示文字のスタイル
        /// </summary>
        private readonly string currentSpanCss = "sr-only";

        /// <summary>
        /// disableのスタイル
        /// </summary>
        private readonly string disableCss = "disabled";

        /// <summary>
        /// ページャーUIのスタイル
        /// </summary>
        private readonly string navUlCss = "pagination";

        /// <summary>
        /// パラメータ名
        /// </summary>
        private string parameterName = "Pager";

        /// <summary>
        /// アクションパス
        /// </summary>
        private string actionPath = "";

        /// <summary>
        /// 最大表示ページ数
        /// </summary>
        private int showLinks = 5;

        /// <summary>
        /// コンストラクタ(データソース、ページ番号、ページごとの表示件数)
        /// </summary>
        /// <param name="path">アクションパス</param>
        /// <param name="pagination">ページャーUI</param>
        public Paging(string path, IPagination pagination)
        {
            this.actionPath = path;
            this.pagination = pagination;
        }

        /// <summary>
        /// パラメータの設定メソッド。
        /// </summary>
        /// <param name="parameterName">パラメータ</param>
        /// <returns>現在のインスタンス</returns>
        public Paging ParameterName(string parameterName)
        {
            this.parameterName = parameterName;
            return this;
        }

        /// <summary>
        /// リンク表示の設定メソッド。
        /// </summary>
        /// <param name="showLinks">表示フラグ</param>
        /// <returns>現在のインスタンス</returns>
        public Paging ShowLinks(int showLinks)
        {
            this.showLinks = showLinks;
            return this;
        }

        /// <summary>
        /// タグ構築メソッド。
        /// </summary>
        /// <returns>構築したタグの文字列</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            if (this.pagination == null)
            {
                return builder.ToString();
            }

            // 描画範囲
            long start;
            long end;
            if (this.pagination.MaxPage > this.showLinks)
            {
                start = this.pagination.CurrentPage - (this.showLinks / 2);
                end = this.pagination.CurrentPage + (this.showLinks / 2);
                if (start < 1)
                {
                    start = 1;
                }
                else if (end > this.pagination.MaxPage)
                {
                    start = this.pagination.MaxPage - (long)this.showLinks + 1;
                }

                end = start + (long)this.showLinks - 1;
            }
            else
            {
                start = 1;
                end = this.pagination.MaxPage;
            }

            // 開始
            builder.Append(String.Format("<div class=\"{0}\">\n", this.pagerCss));

            // ページ数
            builder.Append(String.Format("<span>{0}～{1}件/{2}件中</span>\n", this.pagination.CurrentPageFrom, this.pagination.CurrentPageTo, this.pagination.TotalCount));

            // ページング開始
            builder.Append(String.Format("<nav>\n<ul class=\"pagination\">\n", this.navUlCss));

            // 先頭
            if (this.pagination.CurrentPage > 1)
            {
                builder.Append(String.Format("<li>\n<a href=\"{0}\" aria-label=\"First\">\n", CreatePageLink(1, CoreConst.PAGER_DIV_ID)));
                builder.Append(String.Format("<span aria-hidden=\"true\">{0}</span>\n", this.firstString));
                builder.Append("</a>\n</li>\n");

                builder.Append(String.Format("<li>\n<a href=\"{0}\" aria-label=\"Previous\">\n", CreatePageLink(this.pagination.CurrentPage - 1, CoreConst.PAGER_DIV_ID)));
                builder.Append(String.Format("<span aria-hidden=\"true\">{0}</span>\n", this.prevString));
                builder.Append("</a>\n</li>\n");
            }
            else
            {
                builder.Append(String.Format("<li>\n<span aria-label=\"First\" class=\"{0}\">\n", this.disableCss));
                builder.Append(String.Format("<span aria-hidden=\"true\">{0}</span>\n", this.firstString));
                builder.Append("</span>\n</li>\n");

                builder.Append(String.Format("<li>\n<span aria-label=\"Previous\" class=\"{0}\">\n", this.disableCss));
                builder.Append(String.Format("<span aria-hidden=\"true\">{0}</span>\n", this.prevString));
                builder.Append("</span>\n</li>\n");
            }

            // ページ
            for (long i = start; i <= end; i++)
            {
                if (i == this.pagination.CurrentPage)
                {
                    builder.Append(String.Format("<li class=\"{0}\"><a href=\"{1}\">{2}<span class=\"{3}\">(current)</span></a></li>\n", this.currentCss, CreatePageLink(i, CoreConst.PAGER_DIV_ID), i, this.currentSpanCss));
                }
                else
                {
                    builder.Append(String.Format("<li><a href=\"{0}\">{1}</a></li>\n", CreatePageLink(i, CoreConst.PAGER_DIV_ID), i));
                }
            }

            // 最後
            if (this.pagination.HasNextPage)
            {
                builder.Append(String.Format("<li>\n<a href=\"{0}\" aria-label=\"Next\">\n", CreatePageLink(this.pagination.CurrentPage + 1, CoreConst.PAGER_DIV_ID)));
                builder.Append(String.Format("<span aria-hidden=\"true\">{0}</span>\n", this.nextString));
                builder.Append("</a>\n</li>\n");

                builder.Append(String.Format("<li>\n<a href=\"{0}\" aria-label=\"Last\">\n", CreatePageLink(this.pagination.MaxPage, CoreConst.PAGER_DIV_ID)));
                builder.Append(String.Format("<span aria-hidden=\"true\">{0}</span>\n", this.lastString));
                builder.Append("</a>\n</li>\n");
            }
            else
            {
                builder.Append(String.Format("<li>\n<span aria-label=\"Next\" class=\"{0}\">\n", this.disableCss));
                builder.Append(String.Format("<span aria-hidden=\"true\">{0}</span>\n", this.nextString));
                builder.Append("</span>\n</li>\n");

                builder.Append(String.Format("<li>\n<span aria-label=\"Last\" class=\"{0}\">\n", this.disableCss));
                builder.Append(String.Format("<span aria-hidden=\"true\">{0}</span>\n", this.lastString));
                builder.Append("</span>\n</li>\n");
            }

            // 終了
            builder.Append("</ul>\n</nav>\n</div>\n");

            return builder.ToString();
        }

        /// <summary>
        /// タグ構築メソッド。
        /// </summary>
        /// <param name="functionName">クリックファンクション名</param>
        /// <returns>構築したタグの文字列</returns>
        public string ToString(string functionName)
        {
            StringBuilder builder = new StringBuilder();

            if (this.pagination == null)
            {
                return builder.ToString();
            }

            // 描画範囲
            long start;
            long end;
            if (this.pagination.MaxPage > this.showLinks)
            {
                start = this.pagination.CurrentPage - (this.showLinks / 2);
                end = this.pagination.CurrentPage + (this.showLinks / 2);
                if (start < 1)
                {
                    start = 1;
                }
                else if (end > this.pagination.MaxPage)
                {
                    start = this.pagination.MaxPage - (long)this.showLinks + 1;
                }

                end = start + (long)this.showLinks - 1;
            }
            else
            {
                start = 1;
                end = this.pagination.MaxPage;
            }

            // 開始
            builder.Append(String.Format("<div class=\"{0}\">\n", this.pagerCss));

            // ページ数
            builder.Append(String.Format("<span>{0}～{1}件/{2}件中</span>\n", this.pagination.CurrentPageFrom, this.pagination.CurrentPageTo, this.pagination.TotalCount));

            // ページング開始
            builder.Append(String.Format("<nav>\n<ul class=\"pagination\">\n", this.navUlCss));

            // 先頭
            if (this.pagination.CurrentPage > 1)
            {
                builder.Append(String.Format("<li>\n<a href=\"#\" aria-label=\"First\" onclick=\"{0}('{1}');return false;\" title=\"最初のページへ\">\n", functionName, CreatePageLink(1)));
                builder.Append(String.Format("<span aria-hidden=\"true\">{0}</span>\n", this.firstString));
                builder.Append("</a>\n</li>\n");

                builder.Append(String.Format("<li>\n<a href=\"#\" aria-label=\"Previous\" onclick=\"{0}('{1}');return false;\" title=\"前のページへ\">\n", functionName, CreatePageLink(this.pagination.CurrentPage - 1)));
                builder.Append(String.Format("<span aria-hidden=\"true\">{0}</span>\n", this.prevString));
                builder.Append("</a>\n</li>\n");
            }
            else
            {
                builder.Append(String.Format("<li>\n<span aria-label=\"First\" class=\"{0}\">\n", this.disableCss));
                builder.Append(String.Format("<span aria-hidden=\"true\" title=\"最初のページへ\">{0}</span>\n", this.firstString));
                builder.Append("</span>\n</li>\n");

                builder.Append(String.Format("<li>\n<span aria-label=\"Previous\" class=\"{0}\">\n", this.disableCss));
                builder.Append(String.Format("<span aria-hidden=\"true\" title=\"前のページへ\">{0}</span>\n", this.prevString));
                builder.Append("</span>\n</li>\n");
            }

            // ページ
            for (long i = start; i <= end; i++)
            {
                if (i == this.pagination.CurrentPage)
                {
                    builder.Append(String.Format("<li class=\"{0}\"><a href=\"#\" onclick=\"{1}('{2}');return false;\" title=\"{3}ページ目へ\">{3}<span class=\"{4}\">(current)</span></a></li>\n", this.currentCss, functionName, CreatePageLink(i), i, this.currentSpanCss));
                }
                else
                {
                    builder.Append(String.Format("<li><a href=\"#\" onclick=\"{0}('{1}');return false;\" title=\"{2}ページ目へ\">{2}</a></li>\n", functionName, CreatePageLink(i), i));
                }
            }

            // 最後
            if (this.pagination.HasNextPage)
            {
                builder.Append(String.Format("<li>\n<a href=\"#\" aria-label=\"Next\" onclick=\"{0}('{1}');return false;\" title=\"次のページへ\">\n", functionName, CreatePageLink(this.pagination.CurrentPage + 1)));
                builder.Append(String.Format("<span aria-hidden=\"true\">{0}</span>\n", this.nextString));
                builder.Append("</a>\n</li>\n");

                builder.Append(String.Format("<li>\n<a href=\"#\" aria-label=\"Last\" onclick=\"{0}('{1}');return false;\" title=\"最後のページへ\">\n", functionName, CreatePageLink(this.pagination.MaxPage)));
                builder.Append(String.Format("<span aria-hidden=\"true\">{0}</span>\n", this.lastString));
                builder.Append("</a>\n</li>\n");
            }
            else
            {
                builder.Append(String.Format("<li>\n<span aria-label=\"Next\" class=\"{0}\">\n", this.disableCss));
                builder.Append(String.Format("<span aria-hidden=\"true\" title=\"次のページへ\">{0}</span>\n", this.nextString));
                builder.Append("</span>\n</li>\n");

                builder.Append(String.Format("<li>\n<span aria-label=\"Last\" class=\"{0}\">\n", this.disableCss));
                builder.Append(String.Format("<span aria-hidden=\"true\" title=\"最後のページへ\">{0}</span>\n", this.lastString));
                builder.Append("</span>\n</li>\n");
            }

            // 終了
            builder.Append("</ul>\n</nav>\n</div>\n");

            return builder.ToString();
        }


        /// <summary>
        /// リンク作成メソッド。
        /// </summary>
        /// <param name="page">ページ番号</param>
        /// <param name="divId">ページャーブロックID</param>
        /// <returns>作成したリンクの文字列</returns>
        private string CreatePageLink(long page, string divId)
        {
            return string.Format("{0}/{1}#{2}", this.actionPath, page, divId);
        }

        /// <summary>
        /// リンク作成メソッド。
        /// </summary>
        /// <param name="page">ページ番号</param>
        /// <returns>作成したリンクの文字列</returns>
        private string CreatePageLink(long page)
        {
            return string.Format("{0}/{1}", this.actionPath, page);
        }

    }
}