namespace NSK_P111020.Models
{
    /// <summary>
    /// 帳票データ
    /// </summary>
    public class NSK_111020PModel
    {
        public required TyouhyouSakuseiJouken TyouhyouSakuseiJouken { get; set; }

        public required List<TyouhyouShousaiData> TyouhyouShousaiDatas { get; set; }
    }
}
