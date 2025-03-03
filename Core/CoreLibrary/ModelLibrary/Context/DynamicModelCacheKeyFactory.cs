using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ModelLibrary.Context
{
    public class DynamicModelCacheKeyFactory : IModelCacheKeyFactory
    {
        /// <summary>
        /// 特定のコンテキストのモデルを一意に識別するキーを作成する
        /// 接続文字列、デフォルトスキーマをキャッシュの条件とするキャッシュ キーを生成する
        /// </summary>
        /// <param name="context">モデル キャッシュ キーを取得するコンテキスト</param>
        /// <param name="designTime">モデルにデザイン時の構成を含める必要があるかどうか</param>
        /// <returns>作成されたキー</returns>
        public object Create(DbContext context, bool designTime)
            => context is ContextBase nohoContext
                ? (context.GetType(), nohoContext.connectionString, nohoContext.defaultSchema, designTime)
                : (object)context.GetType();
    }
}
