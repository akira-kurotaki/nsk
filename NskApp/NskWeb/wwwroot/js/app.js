// 2つのメッセージを改行し、結合
function ConcatMessage(message1, message2) {

    var retValue = message1;
    if ("" != retValue) {
        retValue = retValue + '<br />';
    }
    return retValue + message2;
}

// 数字フォーマット
function parseFormatNum(number, n) {

    if (number.toString() == "NaN") {
        return "";
    }

    if (n != 0) {
        n = (n > 0 && n <= 20) ? n : 2;
    }

    var katasu = n <= 0 ? 0 : n;
    var number_val = (parseFloat(number.toString()) * Math.pow(10, katasu)) / Math.pow(10, katasu);

    var sub_val = number_val.toLocaleString().split(".")[0];
    var sub_xs = number_val.toFixed(n).split(".")[1];

    var number_str = sub_xs ? sub_val + "." + sub_xs : sub_val.toString();
    number_val = parseFloat(number_str);

    if (number_val == 0) {
        return number_val.toFixed(n);
    }
    else {
        return number_str;
    }
}

// 数字に最後の無効のセロを削除する
function cutZero(old) {
    newstr = old;
    // 小数の部分  
    var leng = old.length - old.indexOf(".") - 1
    // 小数か
    if (old.indexOf(".") > -1) {
        for (i = leng; i > 0; i--) {
            if (newstr.lastIndexOf("0") > -1 && newstr.substr(newstr.length - 1, 1) == 0) {
                var k = newstr.lastIndexOf("0");
                // 小数に最後のセロが一つしかない場合、小数の部分を削除する
                if (newstr.charAt(k - 1) == ".") {
                    return newstr.substring(0, k - 1);
                } else {
                    // セロ削除
                    newstr = newstr.substring(0, k);
                }
            } else {
                return newstr;
            }
        }
    }
    return old;
}

// 西暦年の取得 
function getSeirekiYear(gengo, year) {

    if ("" == gengo) {
        return 0;
    }
    var gengos = {

        // 令和
        gengo5: "R",
        // 平成
        gengo4: "H",
        // 昭和
        gengo3: "S",
        // 大正
        gengo2: "T",
        // 明治
        gengo1: "M"
    }
    if (gengo == gengos.gengo5) {
        return parseInt(year) + 2018;
    } else if (gengo == gengos.gengo4) {
        return parseInt(year) + 1988;
    } else if (gengo == gengos.gengo3) {
        return parseInt(year) + 1925;
    } else if (gengo == gengos.gengo2) {
        return parseInt(year) + 1911;
    } else if (gengo == gengos.gengo1) {
        return parseInt(year) + 1867;
    }
}

// 四捨五入（マイナスの場合、その絶対値で扱い対応）
function absRound(value) {

    if (value < 0) {
        return Math.round(Math.abs(value)) * -1;
    }
    else {
        return Math.round(value);
    }
}

// 四捨五入（正数の場合）
function round(number, precision) {
    return Math.round(+number + 'e' + precision) / Math.pow(10, precision);
}
// 四捨五入（負数の場合）
function roundAbs(number, precision) {
    if (number < 0) {
        return round(Math.abs(number), precision) * -1;
    }
    else {
        return round(number, precision);
    }
}

// 元号取得
function getGengos() {

    return {

        // 令和
        R: 2019,
        // 平成
        H: 1989,
        // 昭和
        S: 1926,
        // 大正
        T: 1912,
        // 明治
        M: 1868
    };
}

// 和暦取得
function getRealWareki(seireki) {

    if (!seireki) {

        return null;
    }
    var gengos = getGengos();

    var date = new Date(seireki);
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();
    var gengo = "";
    var gengoYear = 0;
    if (year > gengos.G) {

        gengo = "R";
        gengoYear = year - gengos.R + 1
    }
    else if (year > gengos.H && year <= gengos.R) {

        gengo = "H";
        gengoYear = year - gengos.H + 1
    }
    else if (year > gengos.S && year <= gengos.H) {

        gengo = "S";
        gengoYear = year - gengos.S + 1
    }
    else if (year > gengos.T && year <= gengos.S) {

        gengo = "T";
        gengoYear = year - gengos.T + 1
    }
    else if (year > gengos.M && year <= gengos.T) {

        gengo = "M";
        gengoYear = year - gengos.M + 1
    }

    gengo += ((gengoYear < 10 ? "0" : "") + gengoYear);
    gengo += ("/" + (month < 10 ? "0" : "") + month);
    gengo += ("/" + (day < 10 ? "0" : "") + day);

    return gengo;
}

// 西暦取得
function getRealSeireki(wareki) {

    if (!wareki) {

        return null;
    }
    var gengos = getGengos();

    var gengo = wareki.substring(0, 1);
    var year = parseInt(wareki.substring(1, 3));
    var month = parseInt(wareki.substring(4, 6));
    var day = parseInt(wareki.substring(7, 9));

    return gengos[gengo] ? new Date(gengos[gengo] + year - 1, month - 1, day) : null;
}

// エリア分で属性チェックを行う
function checkInputValid(areaTarget) {
    var areaValidCheckFlg = true;
    if ($(areaTarget).length == 0) {
        return true;
    }
    $(areaTarget).find("input[type!=hidden],select,textarea").each(function (index, element) {
        if ($(this) != undefined && 0 < $(this).length &&
            !$(this).prop("disabled") && !$(this).valid()) {
            areaValidCheckFlg = false;
        }
    });

    return areaValidCheckFlg;
}

// 0を補足
function PrefixInteger(num, length) {

    return (Array(length).join('0') + num).slice(-length);

}

// Boolean型に変換
function toBoolean(data) {
    return data.toLowerCase() === 'true';
}