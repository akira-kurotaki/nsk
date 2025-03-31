namespace NSK_P111030.Models
{
    /// <summary>
    /// 帳票データ
    /// </summary>
    public class NSK_111030PModel
    {
        public required TyouhyouSakuseiJouken TyouhyouSakuseiJouken { get; set; }

        public required List<TyouhyouShousaiData> TyouhyouShousaiDatas { get; set; }
    }
}
