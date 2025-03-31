using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using NskAppModelLibrary.Context;

namespace NSK_B105090.Models
{
    public class Arguments
    {
        /// <summary>バッチID</summary>
        public string BatchId { get; set; } = string.Empty;
        /// <summary>バッチID（数値化）</summary>
        private int _nBatchId;
        public int BatchIdNum
        {
            get
            {
                return _nBatchId;
            }
        }
        /// <summary>都道府県コード</summary>
        public string TodofukenCd { get; set; } = string.Empty;
        /// <summary>組合等コード</summary>
        public string KumiaitoCd { get; set; } = string.Empty;
        /// <summary>支所コード</summary>
        public string ShishoCd { get; set; } = string.Empty;
        /// <summary>バッチ条件のキー情報</summary>
        public string JokenId { get; set; } = string.Empty;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="args"></param>
        public Arguments(string[] args)
        {
            // args から 各変数へ展開する
            // バッチID
            if (args.Length > 0)
            {
                BatchId = args[0];
            }
            // 都道府県コード
            if (args.Length > 1)
            {
                TodofukenCd = args[1];
            }
            // 組合等コード
            if (args.Length > 2)
            {
                KumiaitoCd = args[2];
            }
            // 支所コード
            if (args.Length > 3)
            {
                ShishoCd = args[3];
            }
            // バッチ条件のキー情報
            if (args.Length > 4)
            {
                JokenId = args[4];
            }
        }


        /// <summary>
        /// 必須チェック
        /// </summary>
        /// <exception cref="AppException"></exception>
        public void IsRequired(ref string errMsg)
        {
            // ３．１．必須チェック

            // ３．１．１．[変数：バッチID]が未入力の場合
            if (string.IsNullOrEmpty(BatchId))
            {
                // 以下のエラーメッセージを [変数：エラーメッセージ] に設定し、ERRORログに出力して「１０．」へ進む。
                // （"ME01645" 引数{0} ：パラメータの取得）
                errMsg = MessageUtil.Get("ME01645", "パラメータの取得");
                throw new AppException("ME01645", errMsg);
            }

            // ３．１．２．[変数：バッチID] が数値変換不可の場合
            if (!int.TryParse(BatchId, out int nBid))
            {
                // 以下のエラーメッセージを [変数：エラーメッセージ] に設定し、ERRORログに出力して「１０．」へ進む。
                // （"ME90012"　引数{0} ：バッチID)
                errMsg = MessageUtil.Get("ME90012", "バッチID");
                throw new AppException("ME90012", errMsg);
            }
            _nBatchId = nBid;

            // ３．１．３．[変数：都道府県コード]が未入力の場合
            if (string.IsNullOrEmpty(TodofukenCd))
            {
                // 以下のエラーメッセージを [変数：エラーメッセージ] に設定し、ERRORログに出力して「１０．」へ進む。
                // （"ME01054" 引数{0} ：都道府県コード）
                errMsg = MessageUtil.Get("ME01054", "都道府県コード");
                throw new AppException("ME01054", errMsg);
            }

            // ３．１．４．[変数：組合等コード]が未入力の場合
            if (string.IsNullOrEmpty(KumiaitoCd))
            {
                // 以下のエラーメッセージを [変数：エラーメッセージ] に設定し、ERRORログに出力して「１０．」へ進む。
                // （"ME01054" 引数{0} ：組合等コード）
                errMsg = MessageUtil.Get("ME01054", "組合等コード");
                throw new AppException("ME01054", errMsg);
            }

            // ３．１．５．[変数：支所コード]が未入力の場合
            if (string.IsNullOrEmpty(ShishoCd))
            {
                // 以下のエラーメッセージを [変数：エラーメッセージ] に設定し、ERRORログに出力して「１０．」へ進む。
                // （"ME01054" 引数{0} ：支所コード）
                errMsg = MessageUtil.Get("ME01054", "支所コード");
                throw new AppException("ME01054", errMsg);
            }

            // ３．１．６．[変数：バッチ条件のキー情報]が未入力の場合
            if (string.IsNullOrEmpty(JokenId))
            {
                // 以下のエラーメッセージを [変数：エラーメッセージ] に設定し、ERRORログに出力して「１０．」へ進む。
                // （"ME01054" 引数{0} ：バッチ条件のキー情報）
                errMsg = MessageUtil.Get("ME01054", "バッチ条件のキー情報");
                throw new AppException("ME01054", errMsg);
            }
        }

        /// <summary>
        /// 整合性チェック
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        public void IsConsistency(NskAppContext dbContext, ref string errMsg)
        {
            // ６．１．「都道府県コード存在情報」を取得する。
            int todofukenCdCnt = dbContext.VTodofukens.Count(x => x.TodofukenCd == TodofukenCd);

            // ６．２．データが取得できない場合（該当データがマスタデータに登録されていない場合）、以下のエラーメッセージを設定し、ERRORログに出力して「１０．」へ進む。
            if (todofukenCdCnt == 0)
            {
                //（"ME10005"　引数{0} ：都道府県コード)
                errMsg = MessageUtil.Get("ME10005", "都道府県コード");
                throw new AppException("ME10005", errMsg);
            }

            // ６．３．[変数：組合等コード]が入力されている場合、「組合等コード存在情報」を取得する。
            if (!string.IsNullOrEmpty(KumiaitoCd))
            {
                int kumiaitoCdCnt = dbContext.VKumiaitos.Count(x =>
                    (x.KumiaitoCd == KumiaitoCd) &&
                    (x.TodofukenCd == TodofukenCd));

                // ６．４．データが取得できない場合（該当データがマスタデータに登録されていない場合）、以下のエラーメッセージを設定し、ERRORログに出力して「１０．」へ進む。
                if (kumiaitoCdCnt == 0)
                {
                    //（"ME10005"　引数{0} ：組合等コード)
                    errMsg = MessageUtil.Get("ME10005", "組合等コード");
                    throw new AppException("ME10005", errMsg);
                }
            }

            // ６．５．[変数：支所コード]が入力されている場合、「支所コード存在情報」を取得する。
            if (!string.IsNullOrEmpty(ShishoCd))
            {
                int shishoCdCnt = dbContext.VShishoNms.Count(x =>
                    (x.TodofukenCd == TodofukenCd) &&
                    (x.KumiaitoCd == KumiaitoCd) &&
                    (x.ShishoCd == ShishoCd));

                // ６．６．データが取得できない場合（該当データがマスタデータに登録されていない場合）、以下のエラーメッセージを設定し、ERRORログに出力して「１０．」へ進む。
                if (shishoCdCnt == 0)
                {
                    //（"ME10005"　引数{0} ：支所コード)
                    errMsg = MessageUtil.Get("ME10005", "支所コード");
                    throw new AppException("ME10005", errMsg);
                }
            }

        }
    }
}
