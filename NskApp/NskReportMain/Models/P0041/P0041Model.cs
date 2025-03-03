namespace NskReportMain.Models.P0041
{
    /// <summary>
    /// P0041の帳票用モデル
    /// </summary>
    /// <remarks>
    /// 作成日：2023/05/06
    /// 作成者：KAN RI
    /// </remarks>
    public class P0041Model
    {
        /// <summary>
        /// 契約ID
        /// </summary>
        public int KeiyakuId { get; set; }

        /// <summary>
        /// 算定済みフラグ(判定用)
        /// </summary>
        public string SanteiFlg { get; set; }

        /// <summary>
        /// 経営形態(判定用)
        /// </summary>
        public string KeieiKeitaiCd { get; set; }

        /// <summary>
        /// 最新履歴番号
        /// </summary>
        public short? LastRirekiNo { get; set; }

        /// <summary>
        /// 年度
        /// </summary>
        public string Nendo { get; set; }

        /// <summary>
        /// 西暦年
        /// </summary>
        public string GregorianYear { get; set; }

        /// <summary>
        /// 申請年
        /// </summary>
        public string KanyuShinseiY { get; set; }

        /// <summary>
        /// 申請月
        /// </summary>
        public string KanyuShinseiM { get; set; }

        /// <summary>
        /// 申請日
        /// </summary>
        public string KanyuShinseiD { get; set; }

        /// <summary>
        /// 氏名又は法人名フリガナ
        /// </summary>
        public string HojinFullKana { get; set; }

        /// <summary>
        /// 氏名又は法人名
        /// </summary>
        public string HojinFullNm { get; set; }

        /// <summary>
        /// 代表者氏名フリガナ
        /// </summary>
        public string DaihyoshaKana { get; set; }

        /// <summary>
        /// 代表者氏名
        /// </summary>
        public string DaihyoshaNm { get; set; }

        /// <summary>
        /// 郵便番号
        /// </summary>
        public string PostalCd { get; set; }
        public string PostalCd1 { get; set; }
        public string PostalCd2 { get; set; }
        public string PostalCd3 { get; set; }
        public string PostalCd4 { get; set; }
        public string PostalCd5 { get; set; }
        public string PostalCd6 { get; set; }
        public string PostalCd7 { get; set; }

        /// <summary>
        /// 住所
        /// </summary>
        public string AddressKanji { get; set; }

        /// <summary>
        /// 加入者管理コード
        /// </summary>
        public string KanyushaCode { get; set; }
        public string KanyushaCode1 { get; set; }
        public string KanyushaCode2 { get; set; }
        public string KanyushaCode3 { get; set; }
        public string KanyushaCode4 { get; set; }
        public string KanyushaCode5 { get; set; }
        public string KanyushaCode6 { get; set; }
        public string KanyushaCode7 { get; set; }
        public string KanyushaCode8 { get; set; }
        public string KanyushaCode9 { get; set; }
        public string KanyushaCode10 { get; set; }
        public string KanyushaCode11 { get; set; }
        public string KanyushaCode12 { get; set; }
        public string KanyushaCode13 { get; set; }

        /// <summary>
        /// 経営形態個人チェック
        /// </summary>
        public bool KeieiKeitaiKojin { get; set; }

        /// <summary>
        /// 経営形態法人チェック
        /// </summary>
        public bool KeieiKeitaiHojin { get; set; }

        /// <summary>
        /// 事業年度開始年
        /// </summary>
        public string JigyonendoStartY { get; set; }

        /// <summary>
        /// 事業年度開始月
        /// </summary>
        public string JigyonendoStartM { get; set; }

        /// <summary>
        /// 事業年度開始日
        /// </summary>
        public string JigyonendoStartD { get; set; }

        /// <summary>
        /// 事業年度終了年
        /// </summary>
        public string JigyonendoEndY { get; set; }

        /// <summary>
        /// 事業年度終了月
        /// </summary>
        public string JigyonendoEndM { get; set; }

        /// <summary>
        /// 事業年度終了日
        /// </summary>
        public string JigyonendoEndD { get; set; }

        /// <summary>
        /// 加入申請時青色申告実績５年チェック
        /// </summary>
        public bool AoiroNensu5 { get; set; }

        /// <summary>
        /// 加入申請時青色申告実績４年チェック
        /// </summary>
        public bool AoiroNensu4 { get; set; }

        /// <summary>
        /// 加入申請時青色申告実績３年チェック
        /// </summary>
        public bool AoiroNensu3 { get; set; }

        /// <summary>
        /// 加入申請時青色申告実績２年チェック
        /// </summary>
        public bool AoiroNensu2 { get; set; }

        /// <summary>
        /// 加入申請時青色申告実績１年チェック
        /// </summary>
        public bool AoiroNensu1 { get; set; }

        /// <summary>
        /// 青色申告の種類正規の簿記チェック
        /// </summary>
        public bool AoiroShinkokuSeiki { get; set; }

        /// <summary>
        /// 青色申告の種類簡易簿記チェック
        /// </summary>
        public bool AoiroShinkokuKani { get; set; }

        /// <summary>
        /// 青色申告の種類現金主義の特例による青色申告ではありませんチェック
        /// </summary>
        public bool AoiroShinkokuGenkin { get; set; }

        /// <summary>
        /// 性別男チェック
        /// </summary>
        public bool GenderMan { get; set; }

        /// <summary>
        /// 性別女チェック
        /// </summary>
        public bool GenderWoman { get; set; }

        /// <summary>
        /// 生年月日明治チェック
        /// </summary>
        public bool DateOfBirthMeiji { get; set; }

        /// <summary>
        /// 生年月日大正チェック
        /// </summary>
        public bool DateOfBirthTaisho { get; set; }

        /// <summary>
        /// 生年月日昭和チェック
        /// </summary>
        public bool DateOfBirthShowa { get; set; }

        /// <summary>
        /// 生年月日平成チェック
        /// </summary>
        public bool DateOfBirthHeisei { get; set; }

        /// <summary>
        /// 生年月日年
        /// </summary>
        public string DateOfBirthY { get; set; }

        /// <summary>
        /// 生年月日月
        /// </summary>
        public string DateOfBirthM { get; set; }

        /// <summary>
        /// 生年月日日
        /// </summary>
        public string DateOfBirthD { get; set; }

        /// <summary>
        /// 電話番号
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 携帯電話番号
        /// </summary>
        public string Cell { get; set; }

        /// <summary>
        /// FAX番号
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// 補塡方式保険方式チェック
        /// </summary>
        public bool HotenHoshikiHoken { get; set; }

        /// <summary>
        /// 補塡方式保険積立方式チェック
        /// </summary>
        public bool HotenHoshikiTsumitate { get; set; }

        /// <summary>
        /// 保険料等算定基礎金額
        /// </summary>
        public string KisoGaku { get; set; }

        /// <summary>
        /// 気象災害特例区分
        /// </summary>
        public bool KishouTokureiKbn { get; set; }

        /// <summary>
        /// 気象災害特例適用チェック（期＝1）
        /// </summary>
        public bool KishouTokureiFlg1 { get; set; }

        /// <summary>
        /// 気象災害特例適用チェック（期＝2）
        /// </summary>
        public bool KishouTokureiFlg2 { get; set; }

        /// <summary>
        /// 気象災害特例適用チェック（期＝3）
        /// </summary>
        public bool KishouTokureiFlg3 { get; set; }

        /// <summary>
        /// 気象災害特例適用チェック（期＝4）
        /// </summary>
        public bool KishouTokureiFlg4 { get; set; }

        /// <summary>
        /// 気象災害特例適用チェック（期＝5）
        /// </summary>
        public bool KishouTokureiFlg5 { get; set; }

        /// <summary>
        /// 誓約事項チェック
        /// </summary>
        public bool SeiyakujikoFlg { get; set; }

        /// <summary>
        /// 過去の農業収入金額の加入申請年分が登録済みかどうかの判定用（0：未登録、1：登録済）
        /// </summary>
        public bool JissekiUmuFlg { get; set; }

        /// <summary>
        /// 保険方式補償限度90％チェック
        /// </summary>
        public bool AoiroNensuHokenGendo90 { get; set; }

        /// <summary>
        /// 保険方式補償限度88％チェック
        /// </summary>
        public bool AoiroNensuHokenGendo88 { get; set; }

        /// <summary>
        /// 保険方式補償限度85％チェック
        /// </summary>
        public bool AoiroNensuHokenGendo85 { get; set; }

        /// <summary>
        /// 保険方式補償限度83％チェック
        /// </summary>
        public bool AoiroNensuHokenGendo83 { get; set; }

        /// <summary>
        /// 保険方式補償限度80％チェック
        /// </summary>
        public bool AoiroNensuHokenGendo80 { get; set; }

        /// <summary>
        /// 保険方式補償限度78％チェック
        /// </summary>
        public bool AoiroNensuHokenGendo78 { get; set; }

        /// <summary>
        /// 保険方式補償限度75％チェック
        /// </summary>
        public bool AoiroNensuHokenGendo75 { get; set; }

        /// <summary>
        /// 保険方式補償限度70％チェック
        /// </summary>
        public bool AoiroNensuHokenGendo70 { get; set; }

        /// <summary>
        /// 保険方式補償限度65％チェック
        /// </summary>
        public bool AoiroNensuHokenGendo65 { get; set; }

        /// <summary>
        /// 保険方式補償限度60％チェック
        /// </summary>
        public bool AoiroNensuHokenGendo60 { get; set; }

        /// <summary>
        /// 保険方式補償限度55％チェック
        /// </summary>
        public bool AoiroNensuHokenGendo55 { get; set; }

        /// <summary>
        /// 保険方式補償限度50％チェック
        /// </summary>
        public bool AoiroNensuHokenGendo50 { get; set; }

        /// <summary>
        /// 保険方式補償下限70％
        /// </summary>
        public bool HokenKagen70 { get; set; }

        /// <summary>
        /// 保険方式補償下限60％
        /// </summary>
        public bool HokenKagen60 { get; set; }

        /// <summary>
        /// 保険方式補償下限50％
        /// </summary>
        public bool HokenKagen50 { get; set; }

        /// <summary>
        /// 保険方式補償下限なし
        /// </summary>
        public bool HokenKagenNashi { get; set; }

        /// <summary>
        /// 保険方式支払率90％チェック
        /// </summary>
        public bool HokenShiharaiRitsu90 { get; set; }

        /// <summary>
        /// 保険方式支払率80％チェック
        /// </summary>
        public bool HokenShiharaiRitsu80 { get; set; }

        /// <summary>
        /// 保険方式支払率70％チェック
        /// </summary>
        public bool HokenShiharaiRitsu70 { get; set; }

        /// <summary>
        /// 保険方式支払率60％チェック
        /// </summary>
        public bool HokenShiharaiRitsu60 { get; set; }

        /// <summary>
        /// 保険方式支払率50％チェック
        /// </summary>
        public bool HokenShiharaiRitsu50 { get; set; }

        /// <summary>
        /// 積立方式積立幅10％チェック
        /// </summary>
        public bool TsumitateHaba10 { get; set; }

        /// <summary>
        /// 積立方式積立幅5％チェック
        /// </summary>
        public bool TsumitateHaba5 { get; set; }

        /// <summary>
        /// 積立方式支払率90％チェック
        /// </summary>
        public bool TsumitateShiharaiRitsu90 { get; set; }

        /// <summary>
        /// 積立方式支払率80％チェック
        /// </summary>
        public bool TsumitateShiharaiRitsu80 { get; set; }

        /// <summary>
        /// 積立方式支払率70％チェック
        /// </summary>
        public bool TsumitateShiharaiRitsu70 { get; set; }

        /// <summary>
        /// 積立方式支払率60％チェック
        /// </summary>
        public bool TsumitateShiharaiRitsu60 { get; set; }

        /// <summary>
        /// 積立方式支払率50％チェック
        /// </summary>
        public bool TsumitateShiharaiRitsu50 { get; set; }

        /// <summary>
        /// 積立方式支払率40％チェック
        /// </summary>
        public bool TsumitateShiharaiRitsu40 { get; set; }

        /// <summary>
        /// 積立方式支払率30％チェック
        /// </summary>
        public bool TsumitateShiharaiRitsu30 { get; set; }

        /// <summary>
        /// 積立方式支払率20％チェック
        /// </summary>
        public bool TsumitateShiharaiRitsu20 { get; set; }

        /// <summary>
        /// 積立方式支払率10％チェック
        /// </summary>
        public bool TsumitateShiharaiRitsu10 { get; set; }

        /// <summary>
        /// 基準収入算定特例面積等規模拡大特例チェック
        /// </summary>
        public bool MensekiTokureiKbn { get; set; }

        /// <summary>
        /// 基準収入算定特例収入上昇傾向特例チェック
        /// </summary>
        public bool ShunyuTokurei { get; set; }

        /// <summary>
        /// 保険料等納付方法一括払チェック
        /// </summary>
        public bool HokenryoShunohohoKbnIkatu { get; set; }

        /// <summary>
        /// 保険料等納付方法分割支払チェック
        /// </summary>
        public bool HokenryoShunohohoKbnBunkatsu { get; set; }

        /// <summary>
        /// 保険料等納付方法分割払２回チェック
        /// </summary>
        public bool HokenryoBunkatsuKaisu2 { get; set; }

        /// <summary>
        /// 保険料等納付方法分割払３回チェック
        /// </summary>
        public bool HokenryoBunkatsuKaisu3 { get; set; }

        /// <summary>
        /// 保険料等納付方法分割払５回チェック
        /// </summary>
        public bool HokenryoBunkatsuKaisu5 { get; set; }

        /// <summary>
        /// 保険料等納付方法分割払９回チェック
        /// </summary>
        public bool HokenryoBunkatsuKaisu9 { get; set; }

        /// <summary>
        /// 個人情報の取扱同意チェック
        /// </summary>
        public bool KojinjohoToriatsukaiFlg { get; set; }

        /// <summary>
        /// 見込農業収入金額の算出時に用いる見込単価チェック
        /// </summary>
        public bool KmtTanaHambaiTankaKbnMikomi { get; set; }

        /// <summary>
        /// 保険期間中の販売金額の平均単価チェック
        /// </summary>
        public bool KmtTanaHambaiTankaKbnHeikin { get; set; }

        /// <summary>
        /// 野菜価格安定制度同時利用特例適用するチェック
        /// </summary>
        public bool YasaiTokureiKbnSuru { get; set; }


        /// <summary>
        /// 野菜価格安定制度同時利用特例適用しないチェック
        /// </summary>
        public bool YasaiTokureiKbnShinai { get; set; }

        /// <summary>
        /// 自動継続チェック
        /// </summary>
        public bool JidoTokuyakuMoushikomiFlg { get; set; }

        /// <summary>
        /// 加入申請書再出力フラグ
        /// </summary>
        public string KanyushinseishoReprintFlg { get; set; }

        /// <summary>
        /// 実績収入再出力フラグ
        /// </summary>
        public string JissekishunyuReprintFlg { get; set; }

        /// <summary>
        /// 補助フォーム（個人）再出力フラグ
        /// </summary>
        public string HojoformKojinReprintFlg { get; set; }

        /// <summary>
        /// 補助フォーム（法人）再出力フラグ
        /// </summary>
        public string HojoformHojinReprintFlg { get; set; }

        /// <summary>
        /// 営農計画（農産物）再出力フラグ
        /// </summary>
        public string EinoNosanReprintFlg { get; set; }

        /// <summary>
        /// 営農計画（畜産物）再出力フラグ
        /// </summary>
        public string EinoChikusanReprintFlg { get; set; }

        /// <summary>
        /// 営農計画（経営面積）再出力フラグ
        /// </summary>
        public string EinoKeieimensekiReprintFlg { get; set; }

        /// <summary>
        /// 営農計画（収入試算）再出力フラグ
        /// </summary>
        public string EinoShunyushisanReprintFlg { get; set; }
    }
}