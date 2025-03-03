// ■数字チェック
$.validator.addMethod('numeric',
    function (value, element, param) {
        if (value === '') {
            return true;
        }
        var className = element.className;
        if (className.toString().indexOf('number-add-comma') !== -1) {
            // カンマを削除する
            var num = value.replace(/,/g, '');
            // 「0-9」を数字とする
            return num.match(/^[0-9]+$/);
        } else {
            // 「0-9」を数字とする
            return value.match(/^[0-9]+$/);
        }

    });

$.validator.unobtrusive.adapters.addBool('numeric');

// ■数値(符号無/小数無)チェック
$.validator.addMethod('numberpositive',
    function (value, element, param) {
        if (value === '') {
            return true;
        }

        // 入力値に対して、カンマを削除する
        var num = value.replace(/,/g, '');

        // 0の場合、OKとする
        if (num === '0') {
            return true;
        }

        // 数字以外の場合、不正なデータとして、エラーとする
        if (num.match(/^\d*$/)) {

            var val = new Number(num);
            if (val > 9007199254740991) {
                return false;
            }

            val = val.toString();

            // 整数の桁数チェック
            if (param < val.length) {
                return false;
            }

            return true;

        } else {
            return false;
        }
    });

$.validator.unobtrusive.adapters.addSingleVal('numberpositive', 'intmaxlength');

// ■数値(符号無/小数有)チェック
$.validator.addMethod('numberdec',
    function (value, element, params) {
        if (value === '') {
            return true;
        }

        // 入力値に対して、カンマを削除する
        var num = value.replace(/,/g, '');

        // 0の場合、OKとする
        if (num === '0') {
            return true;
        }

        // 符号無の小数以外の場合、不正なデータとして、エラーとする
        if (num.match(/^(\d*)?(\.\d+)?$/)) {

            // 整数部最大桁数
            var intmaxlength = params['intmaxlength'];
            // 小数部最大桁数
            var decmaxlength = params['decmaxlength'];

            // 小数がある場合
            if (num.indexOf('.') !== -1) {

                // 整数部、小数部取得
                var intDecValue = num.split('.');

                // 整数部の桁数チェック(前0を除く)
                if (intmaxlength < (new Number(intDecValue[0])).toString().length) {
                    return false;
                }

                // 小数部の桁数チェック(後ろ0を除く)
                if (decmaxlength < intDecValue[1].replace(/0+$/g, '').length) {
                    return false;
                }

            } else {

                // 整数部の桁数チェック
                if (intmaxlength < (new Number(num)).toString().length) {
                    return false;
                }
            }

            return true;

        } else {

            return false;
        }
    });

$.validator.unobtrusive.adapters.add(
    'numberdec',
    ['intmaxlength', 'decmaxlength'],
    function (options) {
        options.rules['numberdec'] = options.params;
        options.messages['numberdec'] = options.message;
    }
);

// ■数値(符号有/小数無)チェック
$.validator.addMethod('numbersign',
    function (value, element, param) {
        if (value === '') {
            return true;
        }

        // 入力値に対して、カンマを削除する
        var num = value.replace(/,/g, '');

        // 0の場合、OKとする
        if (num === '0') {
            return true;
        }

        // 「-」、数字以外の場合、不正なデータとして、エラーとする
        if (num.match(/^[-]?\d*$/)) {

            var val = new Number(num);
            if (!(-9007199254740991 <= val && val <= 9007199254740991)) {
                return false;
            }

            val = val.toString();

            // 符号がある場合、符号を削除する
            if (val.indexOf('-') !== -1) {
                val = val.replace('-', '');
            }

            // 整数の桁数チェック
            if (param < val.length) {
                return false;
            }

            return true;

        } else {
            return false;
        }
    });

$.validator.unobtrusive.adapters.addSingleVal('numbersign', 'intmaxlength');

// ■数値(符号有/小数有)チェック
$.validator.addMethod('numbersigndec',
    function (value, element, params) {
        if (value === '') {
            return true;
        }

        // 入力値に対して、カンマを削除する
        var num = value.replace(/,/g, '');

        // 0の場合、OKとする
        if (num === '0') {
            return true;
        }

        // 符号有の小数以外の場合、不正なデータとして、エラーとする
        if (num.match(/^[-]?(\d*)?(\.\d+)?$/)) {

            // "-"のみ入力される場合、エラーとする
            if (num === '-') {
                return false;
            }

            // 符号がある場合、符号を削除する
            if (num.indexOf('-') !== -1) {
                num = num.replace('-', '');
            }

            // 整数部最大桁数
            var intmaxlength = params['intmaxlength'];
            // 小数部最大桁数
            var decmaxlength = params['decmaxlength'];

            // 小数がある場合
            if (num.indexOf('.') !== -1) {

                // 整数部、小数部取得
                var intDecValue = num.split('.');

                // 整数部の桁数チェック(前0を除く)
                if (intmaxlength < (new Number(intDecValue[0])).toString().length) {
                    return false;
                }

                // 小数部の桁数チェック(後ろ0を除く)
                if (decmaxlength < intDecValue[1].replace(/0+$/g, '').length) {
                    return false;
                }

            } else {

                // 整数部の桁数チェック
                if (intmaxlength < (new Number(num)).toString().length) {
                    return false;
                }
            }

            return true;
        } else {

            return false;
        }
    });

$.validator.unobtrusive.adapters.add(
    'numbersigndec',
    ['intmaxlength', 'decmaxlength'],
    function (options) {
        options.rules['numbersigndec'] = options.params;
        options.messages['numbersigndec'] = options.message;
    }
);

// ■半角英数チェック
$.validator.addMethod('halfwidthalphanum',
    function (value, element, param) {
        if (value === '') {
            return true;
        }
        // 「a-zA-Z0-9」を半角英数とする
        return value.match(/^[a-zA-Z0-9]+$/);
    });

$.validator.unobtrusive.adapters.addBool('halfwidthalphanum');

// ■半角英数記号チェック
$.validator.addMethod('halfwidthalphanumsymbol',
    function (value, element, param) {
        if (value === '') {
            return true;
        }
        // 「" "（半角スペース）」～「~」を半角英数記号とする
        return value.match(/^[ -~]+$/);
    });

$.validator.unobtrusive.adapters.addBool('halfwidthalphanumsymbol');

// ■ひらがなチェック
$.validator.addMethod('hiragana',
    function (value, element, param) {
        if (value === '') {
            return true;
        }
        // 「ぁ」～「ゟ(より)」までと、
        // 「ー」をひらがなとする
        return value.match(/^[\u3041-\u309F\u30FC]+$/);
    });

$.validator.unobtrusive.adapters.addBool('hiragana');

// ■全角カタカナチェック
$.validator.addMethod('fullwidthkatakana',
    function (value, element, param) {
        if (value === '') {
            return true;
        }
        // 「゠」～「ヿ(コト)」までを全角カタカナとする
        return value.match(/^[\u30A0-\u30FF]+$/);
    });

$.validator.unobtrusive.adapters.addBool('fullwidthkatakana');

// ■半角カタカナチェック
$.validator.addMethod('halfwidthkatakana',
    function (value, element, param) {
        if (value === '') {
            return true;
        }
        // 「･」～「ﾟ」までと半角英数記号とする
        return value.match(/^[\uFF65-\uFF9F -~]+$/);
    });

$.validator.unobtrusive.adapters.addBool('halfwidthkatakana');

// ■文字数以内入力チェック
$.validator.addMethod('withinstringlength',
    function (value, element, param) {
        if (value === '') {
            return true;
        }
        var count = value.length + (value.match(/\n/g) || []).length;
        if (param < count) {
            return false;
        } else {
            return true;
        }
    });

$.validator.unobtrusive.adapters.addSingleVal('withinstringlength', 'maxlength');

// ■桁数以内入力チェック
$.validator.addMethod('withindigitlength',
    function (value, element, param) {
        if (value === '') {
            return true;
        }
        if (param < value.length) {
            return false;
        } else {
            return true;
        }
    });

$.validator.unobtrusive.adapters.addSingleVal('withindigitlength', 'maxlength');

// ■文字全桁入力チェック
$.validator.addMethod('fullstringlength',
    function (value, element, param) {
        if (value === '') {
            return true;
        }
        if (value.length !== Number(param)) {
            return false;
        } else {
            return true;
        }
    });

$.validator.unobtrusive.adapters.addSingleVal('fullstringlength', 'length');

// ■数字全桁入力チェック
$.validator.addMethod('fulldigitlength',
    function (value, element, param) {
        if (value === '') {
            return true;
        }
        if (value.length !== Number(param)) {
            return false;
        } else {
            return true;
        }
    });

$.validator.unobtrusive.adapters.addSingleVal('fulldigitlength', 'length');

// ■最大文字数チェック
$.validator.addMethod('maxlength',
    function (value, element, param) {
        return true;
    });
$.validator.unobtrusive.adapters.addBool('maxlength');

// ■外字チェック
$.validator.addMethod('exceptgaiji',
    function (value, element, param) {
        return true;
    });

$.validator.unobtrusive.adapters.addBool('exceptgaiji');

// ■禁止文字チェック
$.validator.addMethod('prohibitword',
    function (value, element) {
        if (value === '') {
            return true;
        }
        var reg = new RegExp(/\t|"|<[a-zA-Z/?!]|&#/g);
        return this.optional(element) == true || !reg.test(value);
    }, 'タブ文字、"、&lt;英字、&lt;/、&lt;?、&lt;!、&#は使用できません。'
);

function prohibitWord() {
    $('textarea, :text').each(function () {
        $(this).rules("add", { 'prohibitword': true, 'messages': { 'prohibitword': 'タブ文字、"、&lt;英字、&lt;/、&lt;?、&lt;!、&#は使用できません。' } });
    });
}

$(function () {
    $('form').each(function () {
        $(this).validate();
    });

    prohibitWord();
});

// ■二重送信防止処理
function preventDblClick(event) {
    var frm = $(this).parents('form');
    if (0 < frm.length) {
        if ($(this).hasClass('allow-dbl') && !frm.hasClass('allow-dbl')) {
            frm.addClass('allow-dbl');
        } else if (!$(this).hasClass('allow-dbl') && frm.hasClass('allow-dbl')) {
            frm.removeClass('allow-dbl');
        }

        if ($('form').hasClass('first-clicked')) {
            alert("既に実行されています。");
            event.stopImmediatePropagation();
            return false;
        }
    }
}

function preventDblSubmit() {
    // ■二重送信防止処理
    if (!$(this).hasClass('allow-dbl') && 0 == $(this).data("validator").errorList.length) {
        $(this).addClass('first-clicked');
    }

    // ■数値型カンマ編集処理
    $(".number-add-comma").each(function (i, arrVal) {
        var id = "#" + arrVal.id;
        var value = arrVal.value;
        var replaceValue = value.replace(/,/g, "");
        if (value !== replaceValue) {
            $(id).val(replaceValue);
        }
    });
}

$(function () {
    // ■二重送信防止処理
    $('[type=submit]').click(preventDblClick);
    $('[type=button]').click(preventDblClick);
    var frm = $('form');
    if (0 < frm.length) {
        frm.submit(preventDblSubmit);
    }

    // ■戻るボタン対応
    history.pushState(null, null, null);
    window.addEventListener('popstate', function () {
        history.pushState(null, null, null);
        alert("この画面で戻るは禁止されています。(ME00040)");
    });

    setIME();

    //if (window.opener == undefined) {
        $(window).on("beforeunload", function (e) {
            otherWindowClose();
        });
    //}

    setTooltip();

    // ■ページ内移動
    $('#page_move_top').click(
        function () {
            $('html,body').animate({ scrollTop: 0 }, 'slow');
            return false;
        }
    );
    $('#page_move_bottom').click(
        function () {
            $('html,body').animate({ scrollTop: $('.mdl_inner').outerHeight() }, 'slow');
            return false;
        }
    );

    $(document).click(
        function (e) {
            if (!$(e.target).closest(".help_icon").length && !$(e.target).closest(".help_icon_other").length) {
                $(this).find(".help_tooltips").remove();
            }
        }
    );

    $(".help_icon").click(
        function () {
            $(".help_tooltips").remove();
            var text = $(this).attr('data-tooltip');
            $(this).append('<span class="help_tooltips">' + text + '</span>');
        }
    );

    // 日付項目入力の和暦転化
    $('.date_seireki_to_wareki').on("focusout", function () {
        if (!$(this).val()) {
            return;
        }
        if ($(this).val().substring(0, 1).match(/^[A-Z]+$/)) {
            var temp = formatWarekiDate($(this).val());
            $(this).val(temp);
        }
        else {
            setSeirekiToWareki($(this));
        }
    });

    // 'yyyy/MM/dd HH:mm'の西暦時間カレンダー 
    datetimepickerhhmm();

    // 時間項目をフォーマットする('yyyy/MM/dd HH:mm')
    formatDateTimeYYYYMMDDHHmm();
});

// 日付項目入力の和暦転化のfunction
function dateSeirekiToWareki() {
    // 日付項目入力の和暦転化
    $('.date_seireki_to_wareki').on("focusout", function () {
        if (!$(this).val()) {
            return;
        }
        if ($(this).val().substring(0, 1).match(/^[A-Z]+$/)) {
            var temp = formatWarekiDate($(this).val());
            $(this).val(temp);
        }
        else {
            setSeirekiToWareki($(this));
        }
    });
}

// ■日付チェック
$.validator.addMethod('dateymd',
    function (value, element, param) {
        if (value === '') {
            return true;
        }

        var data = value.split("/");

        if (data.length !== 3 && data.length !== 1) {
            return false;
        }

        var replaceValue = value.replace(/[\/]/g, "");

        if (!replaceValue.match(/^[0-9]+$/)) {
            return false;
        }

        if (data.length !== 3 && data.length !== 1) {
            return false;
        }

        var vYear;
        var vMonth;
        var vDay;
        if (data.length === 3) {
            if (data[0].toString(10).length !== 4) {
                return false;
            }
            if (data[1].toString().length === 0 || data[1].toString().length > 2) {
                return false;
            }
            if (data[2].toString().length === 0 || data[2].toString().length > 2) {
                return false;
            }
            vYear = data[0] - 0;
            vMonth = data[1] - 1;
            vDay = data[2] - 0;
        } else {
            if (value.length !== 8) {
                return false;
            }
            vYear = replaceValue.substr(0, 4) - 0;
            vMonth = replaceValue.substr(4, 2) - 1;
            vDay = replaceValue.substr(6, 2) - 0;
        }
        return isRightDate(vYear, vMonth, vDay);
    });

$.validator.unobtrusive.adapters.addBool('dateymd');

// ■日付チェック
$.validator.addMethod('datetime',
    function (value, element, param) {
        if (this.optional(element) || value === '') {
            return true;
        }

        // フォーマットがある場合、フォーマットに合わせて入力内容をチェックする
        return validateDateime(value);
    });

$.validator.unobtrusive.adapters.addSingleVal('datetime', 'format');

// ■和暦日付チェック
$.validator.addMethod('dategymd',
    function (date, element, param) {
        if (date === '') {
            return true;
        }
        var value = date.split("/");
        if (value.length === 3) {
            var vGengoYear = value[0];
            if (vGengoYear.length !== 2 && vGengoYear.length !== 3) {
                return false;
            }
            var vGengo;
            var vYear;
            if (vGengoYear.length === 2) {
                vGengo = vGengoYear.substring(0, 1);
                vYear = vGengoYear.substring(1, 2);
            } else if (vGengoYear.length === 3) {
                vGengo = vGengoYear.substring(0, 1);
                vYear = vGengoYear.substring(1, 3);
            }

            if (!vYear.match(/^[0-9]+$/) || parseInt(vYear) === 0) {
                return false;
            }
            var vMonth = value[1];
            var vDay = value[2];
            if ((vMonth.toString()).length === 0 || !vMonth.match(/^[0-9]+$/) || parseInt(vMonth) === 0) {
                return false;
            }
            if ((vDay.toString()).length === 0 || !vDay.match(/^[0-9]+$/) || parseInt(vDay) === 0) {
                return false;
            }

            vMonth = ('00' + vMonth.toString(10)).slice(-2);
            vDay = ('00' + vDay.toString(10)).slice(-2);

            return getSeirekiDate(vGengo, parseInt(vYear), parseInt(vMonth), parseInt(vDay));
        } else {
            if (date.length !== 7) {
                return false;
            } else {
                var vGengo1 = date.substring(0, 1);
                var vYear1 = date.substring(1, 3);
                var vMonth1 = date.substring(3, 5);
                var vDay1 = date.substring(5, 7);

                if (!vYear1.match(/^[0-9]+$/) || parseInt(vYear1) === 0) {
                    return false;
                }
                if (!vMonth1.match(/^[0-9]+$/) || parseInt(vMonth1) === 0) {
                    return false;
                }
                if (!vDay1.match(/^[0-9]+$/) || parseInt(vDay1) === 0) {
                    return false;
                }
                return getSeirekiDate(vGengo1, parseInt(vYear1), parseInt(vMonth1), parseInt(vDay1));
            }
        }
    });

$.validator.unobtrusive.adapters.addBool('dategymd');

$(function () {
    // 和暦date-picker
    $('.date-picker').datepicker({
        autoclose: true,        // 自動閉じる
        weekStart: 1,           // 月曜開始
        todayHighlight: true,   // 今日を強調する
        format: "gyy/mm/dd",    // 和暦表示フォーマット
        showOnFocus: false,     // フォーカスイン表示
        forceParse: false,
        language: "ja"
    });

    // 西暦date-picker
    $('.date-picker-seireki').datepicker({
        autoclose: true,        // 自動閉じる
        weekStart: 1,           // 月曜開始
        todayHighlight: true,   // 今日を強調する
        format: "yyyy/mm/dd",   // 和暦表示フォーマット
        showOnFocus: false,     // フォーカスイン表示
        forceParse: false,
        language: "ja"
    });
});

function getSeirekiDate(gengo, year, month, day) {
    var gengos = {
        gengo5: "R"    //令和
        , gengo4: "H"    //平成
        , gengo3: "S"    //昭和
        , gengo2: "T"    //大正
        , gengo1: "M"    //明治
    };

    if (gengo === gengos.gengo5) {
        //令和 令和1年 → 2019年5月1日から
        if (year === 1 && month >= 5) {
            return isRightDate(2019, month - 1, day);
        } else if (year >= 1) {
            return isRightDate(year + 2018, month - 1, day);
        }
        return false;
    } else if (gengo === gengos.gengo4) {
        //平成 平成1年～平成31年 → 1989年1月8日から2019年4月30日まで
        if (year === 1 && month === 1 && day >= 8) {
            return isRightDate(1989, month - 1, day);
        } else if (year === 31 && month === 4 && day <= 30) {
            return isRightDate(2019, month - 1, day);
        } else if (year >= 1 && year <= 31) {
            return isRightDate(year + 1988, month - 1, day);
        }
        return false;
    } else if (gengo === gengos.gengo3) {
        //昭和 昭和1年～昭和64年 → 1926年12月25日から1989年1月7日まで
        if (year === 1 && month === 12 && day >= 25) {
            return isRightDate(1926, month - 1, day);
        } else if (year === 64 && month === 1 && day <= 7) {
            return isRightDate(1989, month - 1, day);
        } else if (year >= 1 && year <= 64) {
            return isRightDate(year + 1925, month - 1, day);
        }
        return false;
    } else if (gengo === gengos.gengo2) {
        //大正 大正1年～大正15年 → 1912年7月30日から1926年12月24日まで
        if (year === 1 && month === 7 && day >= 30) {
            return isRightDate(1912, month - 1, day);
        } else if (year === 15 && month === 12 && day <= 24) {
            return isRightDate(1926, month - 1, day);
        } else if (year >= 1 && year <= 15) {
            return isRightDate(year + 1911, month - 1, day);
        }
        return false;
    } else if (gengo === gengos.gengo1) {
        //明治 明治1年～明治45年 → 1868年1月25日から1912年7月29日まで
        if (year === 1 && month === 1 && day >= 25) {
            return isRightDate(1868, month - 1, day);
        } else if (year === 45 && month === 7 && day <= 29) {
            return isRightDate(1912, month - 1, day);
        } else if (year >= 1 && year <= 45) {
            return isRightDate(year + 1867, month - 1, day);
        }
        return false;
    }

    return false;
}

function isRightDate(vYear, vMonth, vDay) {
    // 月,日の妥当性チェック
    if (vMonth >= 0 && vMonth <= 11 && vDay >= 1 && vDay <= 31) {
        var vDt = new Date(vYear, vMonth, vDay);
        if (isNaN(vDt)) {
            return false;
        } else if (vDt.getFullYear() === vYear && vDt.getMonth() === vMonth && vDt.getDate() === vDay) {
            return true;
        } else {
            return false;
        }
    } else {
        return false;
    }
}

// ■数値型カンマ編集
$(function () {
    loadNumberAddComma();
    rewriteValue();
});

function removeRougeChar(convertString) {

    if (convertString.substring(0, 1) === ",") {
        return convertString.substring(1, convertString.length);
    } else if (convertString.substring(0, 1) === "-") {
        if (convertString.substring(1, 2) === ",") {
            return "-" + convertString.substring(2, convertString.length);
        }
    }

    return convertString;
}

$.validator.addMethod('date',
    function (value, element) {
        return true;
    });

// ■HTMLエスケープ
String.prototype.escapeHTML = function () {
    return String(this).replace(/[&<>"']/g, function (match) {
        return {
            '&': '&amp;',
            '<': '&lt;',
            '>': '&gt;',
            '"': '&quot;',
            "'": '&#39;'
        }[match];
    });
};

// ■画像を縮小し、blobデータを返す
function getShrinkImageBlobData(image) {

    // 縦横比を維持した縮小サイズを取得
    w = 1200;
    var ratio = w / image.width;
    h = image.height * ratio;

    // canvasに描画
    var canvas = document.createElement('canvas');
    var ctx = canvas.getContext('2d');
    canvas.setAttribute('width', w);
    canvas.setAttribute('height', h);
    ctx.drawImage(image, 0, 0, w, h);

    // 画質バイナリ化
    var originalBinary = canvas.toDataURL('image/jpeg');
    // blobデータを取得
    var originalBlob = base64ToBlob(originalBinary);

    return originalBlob;
}

// 引数のBase64の文字列をBlob形式にする
function base64ToBlob(base64) {
    var base64Data = base64.split(',')[1],
        data = window.atob(base64Data),
        buff = new ArrayBuffer(data.length),
        arr = new Uint8Array(buff),
        blob,
        i,
        dataLen;
    // blobの生成
    for (i = 0, dataLen = data.length; i < dataLen; i++) {
        arr[i] = data.charCodeAt(i);
    }
    blob = new Blob([arr], { type: 'image/jpeg' });
    return blob;
}

// ■全角カナ→半角カナ変換
function convToHalfWidthKatakana(value) {
    var c;
    var output = "";

    // 全角カナ・全角ひらがな・全角スペース・アルファベット・数字・記号を対応させる。
    var m = {
        'ア': 'ｱ', 'イ': 'ｲ', 'ウ': 'ｳ', 'エ': 'ｴ', 'オ': 'ｵ',
        'カ': 'ｶ', 'キ': 'ｷ', 'ク': 'ｸ', 'ケ': 'ｹ', 'コ': 'ｺ',
        'サ': 'ｻ', 'シ': 'ｼ', 'ス': 'ｽ', 'セ': 'ｾ', 'ソ': 'ｿ',
        'タ': 'ﾀ', 'チ': 'ﾁ', 'ツ': 'ﾂ', 'テ': 'ﾃ', 'ト': 'ﾄ',
        'ナ': 'ﾅ', 'ニ': 'ﾆ', 'ヌ': 'ﾇ', 'ネ': 'ﾈ', 'ノ': 'ﾉ',
        'ハ': 'ﾊ', 'ヒ': 'ﾋ', 'フ': 'ﾌ', 'ヘ': 'ﾍ', 'ホ': 'ﾎ',
        'マ': 'ﾏ', 'ミ': 'ﾐ', 'ム': 'ﾑ', 'メ': 'ﾒ', 'モ': 'ﾓ',
        'ヤ': 'ﾔ', 'ユ': 'ﾕ', 'ヨ': 'ﾖ',
        'ラ': 'ﾗ', 'リ': 'ﾘ', 'ル': 'ﾙ', 'レ': 'ﾚ', 'ロ': 'ﾛ',
        'ワ': 'ﾜ', 'ヲ': 'ｦ', 'ン': 'ﾝ',
        'ァ': 'ｧ', 'ィ': 'ｨ', 'ゥ': 'ｩ', 'ェ': 'ｪ', 'ォ': 'ｫ',
        'ガ': 'ｶﾞ', 'ギ': 'ｷﾞ', 'グ': 'ｸﾞ', 'ゲ': 'ｹﾞ', 'ゴ': 'ｺﾞ',
        'ザ': 'ｻﾞ', 'ジ': 'ｼﾞ', 'ズ': 'ｽﾞ', 'ゼ': 'ｾﾞ', 'ゾ': 'ｿﾞ',
        'ダ': 'ﾀﾞ', 'ヂ': 'ﾁﾞ', 'ヅ': 'ﾂﾞ', 'デ': 'ﾃﾞ', 'ド': 'ﾄﾞ',
        'バ': 'ﾊﾞ', 'ビ': 'ﾋﾞ', 'ブ': 'ﾌﾞ', 'ベ': 'ﾍﾞ', 'ボ': 'ﾎﾞ',
        'パ': 'ﾊﾟ', 'ピ': 'ﾋﾟ', 'プ': 'ﾌﾟ', 'ペ': 'ﾍﾟ', 'ポ': 'ﾎﾟ',
        'ャ': 'ｬ', 'ュ': 'ｭ', 'ョ': 'ｮ', 'ッ': 'ｯ',
        'ヴ': 'ｳﾞ', 'ヷ': 'ﾜﾞ', 'ヺ': 'ｦﾞ',
        'ー': 'ｰ', '・': '･', '゛': 'ﾞ', '゜': 'ﾟ', '　': ' ',
        'あ': 'ｱ', 'い': 'ｲ', 'う': 'ｳ', 'え': 'ｴ', 'お': 'ｵ',
        'か': 'ｶ', 'き': 'ｷ', 'く': 'ｸ', 'け': 'ｹ', 'こ': 'ｺ',
        'さ': 'ｻ', 'し': 'ｼ', 'す': 'ｽ', 'せ': 'ｾ', 'そ': 'ｿ',
        'た': 'ﾀ', 'ち': 'ﾁ', 'つ': 'ﾂ', 'て': 'ﾃ', 'と': 'ﾄ',
        'な': 'ﾅ', 'に': 'ﾆ', 'ぬ': 'ﾇ', 'ね': 'ﾈ', 'の': 'ﾉ',
        'は': 'ﾊ', 'ひ': 'ﾋ', 'ふ': 'ﾌ', 'へ': 'ﾍ', 'ほ': 'ﾎ',
        'ま': 'ﾏ', 'み': 'ﾐ', 'む': 'ﾑ', 'め': 'ﾒ', 'も': 'ﾓ',
        'や': 'ﾔ', 'ゆ': 'ﾕ', 'よ': 'ﾖ',
        'ら': 'ﾗ', 'り': 'ﾘ', 'る': 'ﾙ', 'れ': 'ﾚ', 'ろ': 'ﾛ',
        'わ': 'ﾜ', 'を': 'ｦ', 'ん': 'ﾝ',
        'ぁ': 'ｧ', 'ぃ': 'ｨ', 'ぅ': 'ｩ', 'ぇ': 'ｪ', 'ぉ': 'ｫ',
        'が': 'ｶﾞ', 'ぎ': 'ｷﾞ', 'ぐ': 'ｸﾞ', 'げ': 'ｹﾞ', 'ご': 'ｺﾞ',
        'ざ': 'ｻﾞ', 'じ': 'ｼﾞ', 'ず': 'ｽﾞ', 'ぜ': 'ｾﾞ', 'ぞ': 'ｿﾞ',
        'だ': 'ﾀﾞ', 'ぢ': 'ﾁﾞ', 'づ': 'ﾂﾞ', 'で': 'ﾃﾞ', 'ど': 'ﾄﾞ',
        'ば': 'ﾊﾞ', 'び': 'ﾋﾞ', 'ぶ': 'ﾌﾞ', 'べ': 'ﾍﾞ', 'ぼ': 'ﾎﾞ',
        'ぱ': 'ﾊﾟ', 'ぴ': 'ﾋﾟ', 'ぷ': 'ﾌﾟ', 'ぺ': 'ﾍﾟ', 'ぽ': 'ﾎﾟ',
        'ゃ': 'ｬ', 'ゅ': 'ｭ', 'ょ': 'ｮ', 'っ': 'ｯ',
        'ゔ': 'ｳﾞ',
        'Ａ': 'A', 'Ｂ': 'B', 'Ｃ': 'C', 'Ｄ': 'D', 'Ｅ': 'E',
        'Ｆ': 'F', 'Ｇ': 'G', 'Ｈ': 'H', 'Ｉ': 'I', 'Ｊ': 'J',
        'Ｋ': 'K', 'Ｌ': 'L', 'Ｍ': 'M', 'Ｎ': 'N', 'Ｏ': 'O',
        'Ｐ': 'P', 'Ｑ': 'Q', 'Ｒ': 'R', 'Ｓ': 'S', 'Ｔ': 'T',
        'Ｕ': 'U', 'Ｖ': 'V', 'Ｗ': 'W', 'Ｘ': 'X', 'Ｙ': 'Y',
        'Ｚ': 'Z',
        'ａ': 'a', 'ｂ': 'b', 'ｃ': 'c', 'ｄ': 'd', 'ｅ': 'e',
        'ｆ': 'f', 'ｇ': 'g', 'ｈ': 'h', 'ｉ': 'i', 'ｊ': 'j',
        'ｋ': 'k', 'ｌ': 'l', 'ｍ': 'm', 'ｎ': 'n', 'ｏ': 'o',
        'ｐ': 'p', 'ｑ': 'q', 'ｒ': 'r', 'ｓ': 's', 'ｔ': 't',
        'ｕ': 'u', 'ｖ': 'v', 'ｗ': 'w', 'ｘ': 'x', 'ｙ': 'y',
        'ｚ': 'z',
        '０': '0', '１': '1', '２': '2', '３': '3', '４': '4',
        '５': '5', '６': '6', '７': '7', '８': '8', '９': '9',
        '！': '!', '＃': '#', '＄': '$', '％': '%', '＆': '&',
        '＇': '\'', '’': '\'', '（': '(', '）': ')', '＊': '*',
        '＋': '+', '，': ',', '－': '-', '．': '.', '／': '/',
        '：': ':', '；': ';', '＜': '<', '＝': '=', '＞': '>',
        '？': '?', '＠': '@', '［': '[', '＼': '\\', '］': ']',
        '＾': '^', '＿': '_', '｀': '`', '｛': '{', '｜': '|',
        '｝': '}', '～': '~', '￥': '\\'
    };

    for (var i = 0; i < value.length; i++) {
        c = value.charAt(i);
        if (!m[c]) {
            output += c;
        } else {
            output += m[c];
        }
    }

    return output.replace(/’/g, "'");
}

function convToZengin(value) {
    var c;
    var output = "";

    // 全角カナ・全角ひらがな・全角スペース・アルファベット・数字・記号を対応させる。
    var m = {
        'ア': 'ｱ', 'イ': 'ｲ', 'ウ': 'ｳ', 'エ': 'ｴ', 'オ': 'ｵ',
        'カ': 'ｶ', 'キ': 'ｷ', 'ク': 'ｸ', 'ケ': 'ｹ', 'コ': 'ｺ',
        'サ': 'ｻ', 'シ': 'ｼ', 'ス': 'ｽ', 'セ': 'ｾ', 'ソ': 'ｿ',
        'タ': 'ﾀ', 'チ': 'ﾁ', 'ツ': 'ﾂ', 'テ': 'ﾃ', 'ト': 'ﾄ',
        'ナ': 'ﾅ', 'ニ': 'ﾆ', 'ヌ': 'ﾇ', 'ネ': 'ﾈ', 'ノ': 'ﾉ',
        'ハ': 'ﾊ', 'ヒ': 'ﾋ', 'フ': 'ﾌ', 'ヘ': 'ﾍ', 'ホ': 'ﾎ',
        'マ': 'ﾏ', 'ミ': 'ﾐ', 'ム': 'ﾑ', 'メ': 'ﾒ', 'モ': 'ﾓ',
        'ヤ': 'ﾔ', 'ユ': 'ﾕ', 'ヨ': 'ﾖ',
        'ラ': 'ﾗ', 'リ': 'ﾘ', 'ル': 'ﾙ', 'レ': 'ﾚ', 'ロ': 'ﾛ',
        'ワ': 'ﾜ', 'ヲ': 'ｦ', 'ン': 'ﾝ',
        'ァ': 'ｱ', 'ィ': 'ｲ', 'ゥ': 'ｳ', 'ェ': 'ｴ', 'ォ': 'ｵ',
        'ガ': 'ｶﾞ', 'ギ': 'ｷﾞ', 'グ': 'ｸﾞ', 'ゲ': 'ｹﾞ', 'ゴ': 'ｺﾞ',
        'ザ': 'ｻﾞ', 'ジ': 'ｼﾞ', 'ズ': 'ｽﾞ', 'ゼ': 'ｾﾞ', 'ゾ': 'ｿﾞ',
        'ダ': 'ﾀﾞ', 'ヂ': 'ﾁﾞ', 'ヅ': 'ﾂﾞ', 'デ': 'ﾃﾞ', 'ド': 'ﾄﾞ',
        'バ': 'ﾊﾞ', 'ビ': 'ﾋﾞ', 'ブ': 'ﾌﾞ', 'ベ': 'ﾍﾞ', 'ボ': 'ﾎﾞ',
        'パ': 'ﾊﾟ', 'ピ': 'ﾋﾟ', 'プ': 'ﾌﾟ', 'ペ': 'ﾍﾟ', 'ポ': 'ﾎﾟ',
        'ャ': 'ﾔ', 'ュ': 'ﾕ', 'ョ': 'ﾖ', 'ッ': 'ﾂ',
        'ヴ': 'ｳﾞ', 'ヷ': 'ﾜﾞ', 'ヺ': 'ｦﾞ',
        'ー': '-', '゛': 'ﾞ', '゜': 'ﾟ', '　': ' ',
        'あ': 'ｱ', 'い': 'ｲ', 'う': 'ｳ', 'え': 'ｴ', 'お': 'ｵ',
        'か': 'ｶ', 'き': 'ｷ', 'く': 'ｸ', 'け': 'ｹ', 'こ': 'ｺ',
        'さ': 'ｻ', 'し': 'ｼ', 'す': 'ｽ', 'せ': 'ｾ', 'そ': 'ｿ',
        'た': 'ﾀ', 'ち': 'ﾁ', 'つ': 'ﾂ', 'て': 'ﾃ', 'と': 'ﾄ',
        'な': 'ﾅ', 'に': 'ﾆ', 'ぬ': 'ﾇ', 'ね': 'ﾈ', 'の': 'ﾉ',
        'は': 'ﾊ', 'ひ': 'ﾋ', 'ふ': 'ﾌ', 'へ': 'ﾍ', 'ほ': 'ﾎ',
        'ま': 'ﾏ', 'み': 'ﾐ', 'む': 'ﾑ', 'め': 'ﾒ', 'も': 'ﾓ',
        'や': 'ﾔ', 'ゆ': 'ﾕ', 'よ': 'ﾖ',
        'ら': 'ﾗ', 'り': 'ﾘ', 'る': 'ﾙ', 'れ': 'ﾚ', 'ろ': 'ﾛ',
        'わ': 'ﾜ', 'を': 'ｦ', 'ん': 'ﾝ',
        'ぁ': 'ｱ', 'ぃ': 'ｲ', 'ぅ': 'ｳ', 'ぇ': 'ｴ', 'ぉ': 'ｵ',
        'が': 'ｶﾞ', 'ぎ': 'ｷﾞ', 'ぐ': 'ｸﾞ', 'げ': 'ｹﾞ', 'ご': 'ｺﾞ',
        'ざ': 'ｻﾞ', 'じ': 'ｼﾞ', 'ず': 'ｽﾞ', 'ぜ': 'ｾﾞ', 'ぞ': 'ｿﾞ',
        'だ': 'ﾀﾞ', 'ぢ': 'ﾁﾞ', 'づ': 'ﾂﾞ', 'で': 'ﾃﾞ', 'ど': 'ﾄﾞ',
        'ば': 'ﾊﾞ', 'び': 'ﾋﾞ', 'ぶ': 'ﾌﾞ', 'べ': 'ﾍﾞ', 'ぼ': 'ﾎﾞ',
        'ぱ': 'ﾊﾟ', 'ぴ': 'ﾋﾟ', 'ぷ': 'ﾌﾟ', 'ぺ': 'ﾍﾟ', 'ぽ': 'ﾎﾟ',
        'ゃ': 'ﾔ', 'ゅ': 'ﾕ', 'ょ': 'ﾖ', 'っ': 'ﾂ',
        'ゔ': 'ｳﾞ',
        'Ａ': 'A', 'Ｂ': 'B', 'Ｃ': 'C', 'Ｄ': 'D', 'Ｅ': 'E',
        'Ｆ': 'F', 'Ｇ': 'G', 'Ｈ': 'H', 'Ｉ': 'I', 'Ｊ': 'J',
        'Ｋ': 'K', 'Ｌ': 'L', 'Ｍ': 'M', 'Ｎ': 'N', 'Ｏ': 'O',
        'Ｐ': 'P', 'Ｑ': 'Q', 'Ｒ': 'R', 'Ｓ': 'S', 'Ｔ': 'T',
        'Ｕ': 'U', 'Ｖ': 'V', 'Ｗ': 'W', 'Ｘ': 'X', 'Ｙ': 'Y',
        'Ｚ': 'Z',
        'ａ': 'A', 'ｂ': 'B', 'ｃ': 'C', 'ｄ': 'D', 'ｅ': 'E',
        'ｆ': 'F', 'ｇ': 'G', 'ｈ': 'H', 'ｉ': 'I', 'ｊ': 'J',
        'ｋ': 'K', 'ｌ': 'L', 'ｍ': 'M', 'ｎ': 'N', 'ｏ': 'O',
        'ｐ': 'P', 'ｑ': 'Q', 'ｒ': 'R', 'ｓ': 'S', 'ｔ': 'T',
        'ｕ': 'U', 'ｖ': 'V', 'ｗ': 'W', 'ｘ': 'X', 'ｙ': 'Y',
        'ｚ': 'Z',
        'a': 'A', 'b': 'B', 'c': 'C', 'd': 'D', 'e': 'E',
        'f': 'F', 'g': 'G', 'h': 'H', 'i': 'I', 'j': 'J',
        'k': 'K', 'l': 'L', 'm': 'M', 'n': 'N', 'o': 'O',
        'p': 'P', 'q': 'Q', 'r': 'R', 's': 'S', 't': 'T',
        'u': 'U', 'v': 'V', 'w': 'W', 'x': 'X', 'y': 'Y',
        'z': 'Z',
        '０': '0', '１': '1', '２': '2', '３': '3', '４': '4',
        '５': '5', '６': '6', '７': '7', '８': '8', '９': '9',
        '（': '(', '）': ')', '－': '-', '．': '.', 'ｰ': '-'
    };

    for (var i = 0; i < value.length; i++) {
        c = value.charAt(i);
        if (!m[c]) {
            output += c;
        } else {
            output += m[c];
        }
    }
    return output;
}

// ■帳票を別ウィンドウ表示
function windowOpen(actionName) {
    x = window.innerWidth / 1;
    y = window.innerHeight / 1;
    var win = window.open(actionName, "report", "left=0,top=0,width=" + x + ",height=" + y + ",scrollbars=1,toolbar=0,menubar=0,staus=0,resizable=1");

    win.blur();         // サブウインドウにフォーカスを設定する
    window.focus();     // 自画面からフォーカスを取得
    window.blur();      // 自画面からフォーカスを放す
    win.focus();        // サブウインドウにフォーカスを設定する

    storeOpenWindow("report", window);
    return win;
}

// ■別ウィンドウで画面開く
function windowSizeOpen(actionName, target, width, height) {
    x = width / 1;
    y = height / 1;
    var win = window.open(actionName, target, "left=0,top=0,width=" + x + ",height=" + y + ",scrollbars=1,toolbar=0,menubar=0,staus=0,resizable=1", "_blank");

    if (win) {
        win.resizeTo(x, y);
    }
    
    storeOpenWindow(target, window);
    return win;
}

function storeOpenWindow(target, win) {
    //if (win.opener == undefined) {
        var openWin = win.sessionStorage.getItem('openWindowName');
        if (openWin == null) {
            win.sessionStorage.setItem('openWindowName', target);
        } else {
            //win.sessionStorage.setItem('openWindowName', openWin + ";" + target);
            win.sessionStorage.setItem('openWindowName', target + ";" + openWin);
        }
    //} else {
    //    storeOpenWindow(target, win.opener);
    //}
}

// ■別ウィンドウを閉じる
function otherWindowClose() {
    var openWin = window.sessionStorage.getItem('openWindowName');
    var helpFlag = false;
    if (openWin != null) {
        var openWins = openWin.split(";").filter(function (x, i, self) {
            return self.indexOf(x) === i;
        });

        for (var i = 0; i < openWins.length; i++) {
            if (openWins[i] == 'report') {
                closeReportWindow();
            } else if (openWins[i] == 'help') {
                helpFlag = true;
            } else {
                var win = window.open('', openWins[i], "left=0,top=0,width=10,height=10,scrollbars=1,toolbar=0,menubar=0,staus=0,resizable=1");
                win.close();
            }
        }
    }
    window.sessionStorage.removeItem('openWindowName');
    if (helpFlag) {
        storeOpenWindow('help', window);
    }
}

// ■入力必須チェック
$.validator.addMethod('inputrequired',
    function (value, element, param) {
        return true;
    });

$.validator.unobtrusive.adapters.addBool('inputrequired');

// ■フォームデータをモデルに変換（Ajax用）
function getFormDataModel($form) {
    var indexed_array = {};

    var formData = $form.serializeArray();
    for (i = 0; i < formData.length; i++) {
        if (!indexed_array[formData[i].name] || formData[i].value !== 'false') {
            if (formData[i].value === 'true') {
                indexed_array[formData[i].name] = true;
            } else if (formData[i].value === 'false') {
                indexed_array[formData[i].name] = false;
            } else {
                if ($('input[name =\"' + formData[i].name + '\"]').hasClass('number-add-comma')) {
                    var replaceValue = formData[i].value.replace(/,/g, "");
                    if (formData[i].value !== replaceValue) {
                        indexed_array[formData[i].name] = replaceValue;
                    } else {
                        indexed_array[formData[i].name] = formData[i].value;
                    }
                } else {
                    indexed_array[formData[i].name] = formData[i].value;
                }
            }
        }
    }

    var data = {
        model: indexed_array
    };
    return data;
}

// ■フォームデータを取得（Ajax用）
function getFormData($form) {
    var formData = new FormData();

    var formArray = $form.serializeArray();
    for (i = 0; i < formArray.length; i++) {
        if ($('input[name =\"' + formArray[i].name + '\"]').hasClass('number-add-comma')) {
            var replaceValue = formArray[i].value.replace(/,/g, "");
            if (formArray[i].value !== replaceValue) {
                formData.append(formArray[i].name, replaceValue);
            } else {
                formData.append(formArray[i].name, formArray[i].value);
            }
        } else {
            formData.append(formArray[i].name, formArray[i].value);
        }
    }

    return formData;
}

// ■エラー項目リセット
function resetErrorItems(replaceStr, errorItems) {
    if (errorItems !== null) {
        $.each(errorItems, function (idx, obj) {
            var errorItemName = obj;
            if (replaceStr !== "") {
                errorItemName = errorItemName.replace(replaceStr, "");
            }
            var name = "[name='" + errorItemName + "']";
            $(name).addClass("input-validation-error");
        });
    }
}

// ■エラーメッセージリセット
function resetErrorMessages(replaceStr, errorItems, errorMessages) {
    if (errorItems !== null) {
        $.each(errorItems, function (idx, obj) {
            var errorItemName = obj;
            if (replaceStr !== "") {
                errorItemName = errorItemName.replace(replaceStr, "");
            }
            var name = "span[data-valmsg-for='" + errorItemName + "']";
            $(name).html(errorMessages[idx]);
        });
    }
}

// ■パンくず用 子画面ID取得
function getChildId(screenId) {
    var num = parseInt($("#ChildWindowIdNum").val()) + 1;
    $("#ChildWindowIdNum").val(num);
    return screenId + num;
}

// ■数値型カンマ編集(Ajax通信後回復用)
function loadNumberAddComma() {
    $('.number-add-comma').on("focus", function (e) {
        var $this = $(this);
        var value = $this.val();
        var replaceValue = value.replace(/,/g, "");
        if (value !== replaceValue) {
            $this.val(replaceValue);
            $this.select();
        }
    }).on("blur", function (e) {
        var $this = $(this);
        var value = $(this).val();
        if (value === "") { return; }
        if (value.substring(0, 1) === "-") {
            var tempVar1 = value.substring(1).replace(".", "");
            if (!tempVar1.match(/^[0-9]+$/)) {
                return;
            }
        } else {
            var tempVar2 = value.replace(".", "");
            if (!tempVar2.match(/^[0-9]+$/)) {
                return;
            }
        }

        var integerPartValue = value > 0 ? Math.floor(value) : Math.ceil(value);
        //0未満で-1より大きい小数のときに、マイナス符号を付与
        integerPartValue = value < 0 && integerPartValue === 0 ? "-" + integerPartValue.toString() : integerPartValue.toString();
        var smallPartValue = "";
        if (typeof (String(value)).split(".")[1] !== 'undefined') {
            smallPartValue = "." + (String(value)).split(".")[1];
        }
        integerPartValue = integerPartValue.toString().replace(/,/gi, "").split("").reverse().join("");
        integerPartValue = removeRougeChar(integerPartValue.replace(/(.{3})/g, "$1,").split("").reverse().join(""));
        var replaceValue = integerPartValue + smallPartValue;
        if (value !== replaceValue) {
            $this.val("");
            $this.val(replaceValue);
        }
    });

    $(".number-add-comma").each(function (i, arrVal) {
        var value = arrVal.value;
        var id = "#" + arrVal.id;
        if (value === "") {
            return;
        }
        if (value.substring(0, 1) === "-") {
            var tempVar1 = value.substring(1).replace(".", "");
            if (!tempVar1.match(/^[0-9]+$/)) {
                return;
            }
        } else {
            var tempVar2 = value.replace(".", "");
            if (!tempVar2.match(/^[0-9]+$/)) {
                return;
            }
        }
        var integerPartValue = value > 0 ? Math.floor(value) : Math.ceil(value);
        var smallPartValue = "";
        if (typeof (String(value)).split(".")[1] !== 'undefined') {
            smallPartValue = "." + (String(value)).split(".")[1];
        }
        integerPartValue = integerPartValue.toString().replace(/,/gi, "").split("").reverse().join("");
        integerPartValue = removeRougeChar(integerPartValue.replace(/(.{3})/g, "$1,").split("").reverse().join(""));
        var replaceValue = integerPartValue + smallPartValue;
        if (value !== replaceValue) {
            $(id).val("");
            $(id).val(replaceValue);
        }
    });
}

// ■IE10 padding-right不具合対応
function rewriteValue() {
    $('.text-right').on("blur", function (e) {
        var value = $(this).val();
        if (value === "") {
            return;
        } else {
            $(this).val(value);
        }
    });
}

// Ajax通信後回復用処理一覧
function restoreAjax() {
    loadNumberAddComma();
    rewriteValue();
    prohibitWord();
    datePickerRestore();
    setTooltip();
    setIME(); 
    datetimepickerhhmm();
    // 時間項目をフォーマットする('yyyy/MM/dd HH:mm')
    formatDateTimeYYYYMMDDHHmm();
}

function datePickerRestore() {
    // 和暦date-picker
    $('.date-picker').datepicker({
        autoclose: true,        // 自動閉じる
        weekStart: 1,           // 月曜開始
        todayHighlight: true,   // 今日を強調する
        format: "gyy/mm/dd",    // 和暦表示フォーマット
        showOnFocus: false,     // フォーカスイン表示
        forceParse: false,
        language: "ja"
    });
}

function setTooltip() {
    $('[data-bs-toggle="tooltip"]').tooltip();
}

function setIME() {
    // ■入力モード対応
    // 英数字入力モード属性
    var alphaNumAttrs = [
        'data-val-numeric',
        'data-val-numberpositive',
        'data-val-numberdec',
        'data-val-numbersign',
        'data-val-numbersigndec',
        'data-val-halfwidthalphanum',
        'data-val-halfwidthalphanumsymbol',
        'data-val-email',
        'data-val-dategymd'];

    // 日本語入力モード属性
    var japaneseAttrs = [
        'data-val-hiragana',
        'data-val-fullwidthkatakana',
        'data-val-halfwidthkatakana',
        'data-val-exceptgaiji'];

    $('input, textarea').each(function () {
        var element = $(this);
        $.each(alphaNumAttrs, function (i, val) {
            var attr = $(element).attr(val);
            if (typeof attr !== 'undefined' && attr !== false) {
                $(element).css('ime-mode', 'inactive');
                return false;
            }
        });
        $.each(japaneseAttrs, function (i, val) {
            var attr = $(element).attr(val);
            if (typeof attr !== 'undefined' && attr !== false) {
                $(element).css('ime-mode', 'active');
                return false;
            }
        });
    });
}

// DBの登録、更新、削除の発生するボタンの表示、非表示、活性、非活性について、
// セッション[項目制御]に保持するためのfunction
function updateBtnSts(buttonIdList, url) {
    var jsonDataList = new Array();
    for (var i = 0; i < buttonIdList.length; i++) {
        var _this = $("#" + buttonIdList[i]);
        var displayNoneFlg = false;
        var disabledFlg = false;
        if (_this.length == 0) {
            displayNoneFlg = true;
        } else {
            var parents = _this.parents();
            parents.each(function () {
                if ($(this).css("display") == "none") {
                    displayNoneFlg = true;
                };
            });
        }

        if (0 < _this.length && _this.prop("disabled")) {
            disabledFlg = true;
        }
        var jsonData = { "ButtonID": buttonIdList[i], "DisabledFlg": disabledFlg, "DisplayNoneFlg": displayNoneFlg };
        jsonDataList.push(jsonData);
    };

    $.ajax({
        type: 'POST',
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonDataList),
        dataType: 'json',
        url: url,
        error: function (data) {
        },
        success: function (data) {
        }
    });
}

// 小数位数取得
function getDecimalPointLength(number) {
    if (number.toString() == "NaN") {
        return 0;
    }
    var numbers = String(number).split('.');
    return (numbers[1]) ? numbers[1].length : 0;
};

// 日付項目入力の和暦転化
function setSeirekiToWareki(_this) {
    var sysDateyyyymmdd = $("#sysDate_for_date_seireki_to_wareki").val();
    var sysDate = new Date(sysDateyyyymmdd);
    var seireki = _this.val();
    if (seireki && -1 < seireki.indexOf("/")) {
        // 数字ではない場合
        var replaceValue = seireki.replace(/[\/]/g, "");
        if (!replaceValue.match(/^[0-9]+$/)) {
            return false;
        }

        var year;
        var month;
        var day;
        var data = seireki.split("/");
        if (data.length !== 2 && data.length !== 3) {
            return false;
        }
        // YYYY/MM/DD、YYYY/M/DD、YYYY/MM/D、YYYY/M/D
        // /MM/DD、/M/DD、/MM/D、/M/D
        if (data.length === 3) {

            if (data[0].toString(10).length !== 4 && data[0].toString(10).length !== 0) {
                return false;
            }

            // YYYYがある場合
            if (data[0].toString(10).length === 4) {
                year = data[0];
            }
            // YYYYがない場合
            if (data[0].toString(10).length === 0) {
                year = sysDate.getFullYear();
            }

            month = data[1];
            day = data[2];
        }

        // MM/DD、M/DD、MM/D、M/D
        if (data.length === 2) {
            // YYYYがない場合
            year = sysDate.getFullYear();
            month = data[0];
            day = data[1];
        }

        if (month.toString().length === 0 || month.toString().length > 2) {
            return false;
        }
        if (day.toString().length === 0 || day.toString().length > 2) {
            return false;
        }
        var vYear = year - 0;
        var vMonth = month - 1;
        var vDay = day - 0;
        if (isRightDate(vYear, vMonth, vDay)) {
            var vDt = new Date(vYear, vMonth, vDay);
            var wareki = getWarekiFormSeireki(vDt);
            if (wareki) {
                _this.val(wareki);
                return true;
            }
        }
        return false;
    }
}

// 元号マスタ取得
function getGengoStartEndDate() {
    return {
        // 令和開始
        RStart: new Date(2019, 4, 1),
        // 平成開始
        HStart: new Date(1989, 0, 8),
        // 平成終了
        HEnd: new Date(2019, 3, 30),
        // 昭和開始
        SStart: new Date(1926, 11, 25),
        // 昭和終了
        SEnd: new Date(1989, 0, 7),
        // 大正開始
        TStart: new Date(1912, 6, 30),
        // 大正終了
        TEnd: new Date(1926, 11, 24),
        // 明治開始
        MStart: new Date(1868, 0, 1),
        // 明治終了
        MEnd: new Date(1912, 6, 29),
    };
}

// 和暦取得
function getWarekiFormSeireki(seireki) {

    if (!seireki) {

        return null;
    }

    var date = new Date(seireki);
    if (isNaN(date)) {
        return null;
    }

    var gengos = getGengoStartEndDate();
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();

    var gengo = "";
    var gengoYear = 0;
    // 令和
    if (date >= gengos.RStart) {
        gengo = "R";
        gengoYear = year - gengos.RStart.getFullYear() + 1;
    }
    // 平成
    else if (date >= gengos.HStart && date <= gengos.HEnd) {

        gengo = "H";
        gengoYear = year - gengos.HStart.getFullYear() + 1;
    }
    // 昭和
    else if (date >= gengos.SStart && date <= gengos.SEnd) {

        gengo = "S";
        gengoYear = year - gengos.SStart.getFullYear() + 1;
    }
    // 大正
    else if (date >= gengos.TStart && date <= gengos.TEnd) {

        gengo = "T";
        gengoYear = year - gengos.TStart.getFullYear() + 1;
    }
    // 明治
    else if (date >= gengos.MStart && date <= gengos.MEnd) {

        gengo = "M";
        gengoYear = year - gengos.MStart.getFullYear() + 1;
    }
    else {
        return null;
    }

    gengo += ((gengoYear < 10 ? "0" : "") + gengoYear);
    gengo += ("/" + (month < 10 ? "0" : "") + month);
    gengo += ("/" + (day < 10 ? "0" : "") + day);

    return gengo;
}

// 和暦日付の整形
function formatWarekiDate(date) {
    var value = date.split("/");
    if (value.length == 3) {
        var vGengoYear = value[0];
        if (vGengoYear.length != 2 && vGengoYear.length != 3) {
            return date;
        }
        var vGengo;
        var vYear;
        if (vGengoYear.length == 2) {
            vGengo = vGengoYear.substring(0, 1);
            vYear = vGengoYear.substring(1, 2);
        } else if (vGengoYear.length == 3) {
            vGengo = vGengoYear.substring(0, 1);
            vYear = vGengoYear.substring(1, 3);
        }

        if (!vYear.match(/^[0-9]+$/) || parseInt(vYear) == 0) {
            return date;
        }
        var vMonth = value[1];
        var vDay = value[2];
        if ((vMonth.toString()).length == 0 || !vMonth.match(/^[0-9]+$/) || parseInt(vMonth) == 0) {
            return date;
        }
        if ((vDay.toString()).length == 0 || !vDay.match(/^[0-9]+$/) || parseInt(vDay) == 0) {
            return date;
        }

        vYear = ('00' + vYear.toString(10)).slice(-2);
        vMonth = ('00' + vMonth.toString(10)).slice(-2);
        vDay = ('00' + vDay.toString(10)).slice(-2);

        if (!checkWarekiDate(vGengo, parseInt(vYear), parseInt(vMonth), parseInt(vDay))) {
            return date;
        }

        return vGengo + vYear + "/" + vMonth + "/" + vDay;

    } else {
        if (date.length != 7) {
            return date;
        } else {
            var vGengo = date.substring(0, 1);
            var vYear = date.substring(1, 3);
            var vMonth = date.substring(3, 5);
            var vDay = date.substring(5, 7);

            if (!vYear.match(/^[0-9]+$/) || parseInt(vYear) == 0) {
                return date;
            }
            if (!vMonth.match(/^[0-9]+$/) || parseInt(vMonth) == 0) {
                return date;
            }
            if (!vDay.match(/^[0-9]+$/) || parseInt(vDay) == 0) {
                return date;
            }

            if (!checkWarekiDate(vGengo, parseInt(vYear), parseInt(vMonth), parseInt(vDay))) {
                return date;
            }

            return vGengo + vYear + "/" + vMonth + "/" + vDay;
        }
    }
    return date;
}

function checkWarekiDate(gengo, year, month, day) {

    if (gengo == "R") {
        //令和 令和1年 → 2019年5月1日から
        if (year == 1 && month >= 5) {
            return isRightDate(2019, month - 1, day);
        } else if (year >= 1) {
            return isRightDate(year + 2018, month - 1, day);
        }
        return false;
    } else if (gengo == "H") {
        //平成 平成1年～平成31年 → 1989年1月8日から2019年4月30日まで
        if (year == 1 && month == 1 && day >= 8) {
            return isRightDate(1989, month - 1, day);
        } else if (year == 31 && month == 4 && day <= 30) {
            return isRightDate(2019, month - 1, day);
        } else if (year >= 1 && year <= 31) {
            return isRightDate(year + 1988, month - 1, day);
        }
        return false;
    } else if (gengo == "S") {
        //昭和 昭和1年～昭和64年 → 1926年12月25日から1989年1月7日まで
        if (year == 1 && month == 12 && day >= 25) {
            return isRightDate(1926, month - 1, day);
        } else if (year == 64 && month == 1 && day <= 7) {
            return isRightDate(1989, month - 1, day);
        } else if (year >= 1 && year <= 64) {
            return isRightDate(year + 1925, month - 1, day);
        }
        return false;
    } else if (gengo == "T") {
        //大正 大正1年～大正15年 → 1912年7月30日から1926年12月24日まで
        if (year == 1 && month == 7 && day >= 30) {
            return isRightDate(1912, month - 1, day);
        } else if (year == 15 && month == 12 && day <= 24) {
            return isRightDate(1926, month - 1, day);
        } else if (year >= 1 && year <= 15) {
            return isRightDate(year + 1911, month - 1, day);
        }
        return false;
    } else if (gengo == "M") {
        //明治 明治1年～明治45年 → 1868年1月25日から1912年7月29日まで
        if (year == 1 && month == 1 && day >= 25) {
            return isRightDate(1868, month - 1, day);
        } else if (year == 45 && month == 7 && day <= 29) {
            return isRightDate(1912, month - 1, day);
        } else if (year >= 1 && year <= 45) {
            return isRightDate(year + 1867, month - 1, day);
        }
        return false;
    }

    return false;
}

// 和暦日付のチェック
// dateは「GYY/MM/DD」の和暦
function checkFormatedWarekiDate(date) {
    var value = date.split("/");
    if (value.length == 3) {
        var vGengoYear = value[0];
        if (vGengoYear.length != 2 && vGengoYear.length != 3) {
            return false;
        }
        var vGengo;
        var vYear;
        if (vGengoYear.length == 2) {
            vGengo = vGengoYear.substring(0, 1);
            vYear = vGengoYear.substring(1, 2);
        } else if (vGengoYear.length == 3) {
            vGengo = vGengoYear.substring(0, 1);
            vYear = vGengoYear.substring(1, 3);
        }

        if (!vYear.match(/^[0-9]+$/) || parseInt(vYear) == 0) {
            return false;
        }
        var vMonth = value[1];
        var vDay = value[2];
        if ((vMonth.toString()).length == 0 || !vMonth.match(/^[0-9]+$/) || parseInt(vMonth) == 0) {
            return false;
        }
        if ((vDay.toString()).length == 0 || !vDay.match(/^[0-9]+$/) || parseInt(vDay) == 0) {
            return false;
        }

        vYear = ('00' + vYear.toString(10)).slice(-2);
        vMonth = ('00' + vMonth.toString(10)).slice(-2);
        vDay = ('00' + vDay.toString(10)).slice(-2);

        if (!checkWarekiDate(vGengo, parseInt(vYear), parseInt(vMonth), parseInt(vDay))) {
            return false;
        }

        return true
    }
    return false;
}

// 'yyyy/MM/dd HH:mm'の西暦時間カレンダー 
function datetimepickerhhmm() {
    // 'yyyy/MM/dd HH:mm'の西暦時間カレンダー 
    $('.datetimepicker-hhmm').each(function () {
        const picker = new tempusDominus.TempusDominus($(this)[0], {
            localization: {
                format: 'yyyy/MM/dd HH:mm',
                dayViewHeaderFormat: { year: 'numeric', month: 'long' },
                today: '本日',
                close: '閉じる',
                selectMonth: '月を選択',
                previousMonth: '前月',
                nextMonth: '次月',
                selectYear: '年を選択',
                previousYear: '前年',
                nextYear: '次年',
                selectDecade: '期間を選択',
                previousDecade: '前期間',
                nextDecade: '次期間',
                previousCentury: '前世紀',
                nextCentury: '次世紀',
                pickHour: '時間を取得',
                incrementHour: '時間を増加',
                decrementHour: '時間を減少',
                pickMinute: '分を取得',
                incrementMinute: '分を増加',
                decrementMinute: '分を減少',
                pickSecond: '秒を取得',
                incrementSecond: '秒を増加',
                decrementSecond: '秒を減少',
                toggleMeridiem: '午前/午後切替',
                selectTime: '時間を選択',
                selectDate: '年月日を選択',
            },
            display: {
                components: {
                    clock: true
                },
                buttons: {
                    close: true,
                },
                theme: 'light',
            },
            useCurrent: false,
        });
    });
}

// 時間項目をフォーマットする('yyyy/MM/dd HH:mm')
function formatDateTimeYYYYMMDDHHmm() {
    $(".datetimepicker-hhmm .datetimepicker-input").on("focusout", function () {
        const timestr = formatDateTime($(this).val(), 'yyyy/MM/dd HH:mm');
        $(this).val(timestr);
    });
}

// 日時項目をフォーマットする（format指定）
function formatDateTime(value, foramt) {
    const date = parseDateTime(value);

    if (date && date instanceof Date && !isNaN(date.getTime())) {
        return formatDateToStr(date, foramt);
    }

    return value;
}

// 日時をチェックする
function validateDateime(value) {
    const date = parseDateTime(value);

    if (date && date instanceof Date && !isNaN(date.getTime())) {
        return true;
    }
    return false;
}

// strを日時類型へ変換する
// 変換失敗の場合、元の値を返却する
// 変換成功の場合、変換後の日時を返却する
function parseDateTime(value) {
    if (!value) {
        return '';
    }

    // 以下のフォーマットの日時は全てチェックOK
    /*const formats = [
        "yyyy/MM/dd HH:mm:ss",
        "yyyy-MM-dd HH:mm:ss",
        "yyyy/MM/dd HH:mm",
        "yyyy-MM-dd HH:mm",
        "yyyy/MM/dd HH",
        "yyyy-MM-dd HH",
        "yyyy/MM/dd ",
        "yyyy-MM-dd ",
        "yyyy/MM/dd",
        "yyyy-MM-dd",
    ];*/
    const regex = /^(?:(\d{4})[-/](0?[1-9]|1[0-2])[-/](0?[1-9]|[12]\d|3[01])(?:\s+(?:(?:([01]?\d|2[0-3])(?::([0-5]?\d)(?::([0-5]?\d))?)?)?)?)?)$/;

    const match = value.match(regex);

    if (!match) {
        return value;
    }

    const year = parseInt(match[1], 10);
    const month = parseInt(match[2], 10) - 1;
    const day = parseInt(match[3], 10);
    const hour = match[4] !== undefined ? parseInt(match[4], 10) : 0;
    const minute = match[5] !== undefined ? parseInt(match[5], 10) : 0;
    const second = match[6] !== undefined ? parseInt(match[6], 10) : 0;

    if (isRightDateTime(year, month, day, hour, minute, second)) {
        return new Date(year, month, day, hour, minute, second);
    }
    else {
        return value;
    }
}

// 日時をフォーマットする、フォーマット後の文字列を返却する
function formatDateToStr(date, format) {
    if (!(date instanceof Date) || isNaN(date.getTime())) {
        return date;
    }
    const padZero = num => (num < 10 ? '0' : '') + num;

    return format.replace(/yyyy|MM|dd|HH|mm|ss/gi, (match, offset, string) => {
        switch (match.toLowerCase()) {
            case 'yyyy': return date.getFullYear();
            case 'mm':
                const prevChar = offset > 0 ? string[offset - 1] : '';
                const nextChar = offset + match.length < string.length ? string[offset + match.length] : '';

                if (/[\/-]/.test(prevChar) || /[:T ]/.test(nextChar)) {
                    return padZero(date.getMonth() + 1);
                } else {
                    return padZero(date.getMinutes());
                }
            case 'dd': return padZero(date.getDate());
            case 'hh': return padZero(date.getHours());
            case 'ss': return padZero(date.getSeconds());
            default: return match;
        }
    });
}

// 月,日, 時, 分の妥当性チェック
function isRightDateTime(vYear, vMonth, vDay, hour, minute, second) {
    // 月,日, 時, 分の妥当性チェック
    if (vMonth >= 0 && vMonth <= 11 && vDay >= 1 && vDay <= 31 &&
        0 <= hour && hour <= 23 && 0 <= minute && minute <= 59 && 0 <= second && second <= 59) {
        var vDt = new Date(vYear, vMonth, vDay, hour, minute, second);
        if (isNaN(vDt)) {
            return false;
        } else if (vDt.getFullYear() === vYear && vDt.getMonth() === vMonth && vDt.getDate() === vDay &&
            vDt.getHours() == hour && vDt.getMinutes() == minute, vDt.getSeconds() == second) {
            return true;
        } else {
            return false;
        }
    } else {
        return false;
    }
}