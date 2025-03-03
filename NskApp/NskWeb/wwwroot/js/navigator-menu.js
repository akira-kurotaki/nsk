$(document).ready(function () {

    $('#navi  h4').click(function () {
        var objects;
        h4_id = this.id;
        //alert(pid);

        // クリックされた項目の子メニューが現在非表示の場合
        if ($("#div_" + h4_id).css('display') == 'none') {
            // クリックされた項目の兄弟の子メニューを非表示にする
            // 自身を含むDIVのIDを取得
            pid = $(this).closest('div').attr('id');
            //alert(pid);
            // 取得したDIVの子DIVのIDを取得
            objects = $("#" + pid + " > div[id != 'div_" + h4_id + "']");
            //objects = $("#" + pid + " > div");
            // 取得した子DIVをすべて非表示にする
            for (var obj of objects) {
                //alert(obj.id);
                $("#" + obj.id).slideUp('slow');
            }

            // 第一メニュー項目の子メニューを非表示にする
            // 第一メニュー項目のIDを取得
            var xxx = h4_id.match(/(^[^-]+)(-|$)/);
            if (xxx != null) {
                pid = xxx[1];
                //alert("[" + h4_id + "],[" + pid + "]");
                // 第一メニューの子メニューのIDを取得
                objects = $("#navi > div > div[id != 'div_" + pid + "']");
                for (var obj of objects) {
                    //alert(obj.id);
                    $("#" + obj.id).slideUp('slow');
                }
            }
            // 孫メニューを非表示にする
            $("#div_" + h4_id + " div").hide();
        }
        $("#div_" + h4_id).slideToggle('slow');
    });


});

$(function () {

    $(".main_area").click(function () {
        closeNavi();
    });

    $("#top_menu").click(function () {
        if ($("#navi > div").length > 0) {
            // メニューが設定されている時のみ実施
            if ($("#navi").is(':hidden') == true) {
                $("#navi > div div").attr("style", "display:none;");    // 子メニュー以降を非表示
                $("#navi").attr("style", "display:block;");
                $("#help_faq").attr("style", "display:none;");
            } else { $("#navi").attr("style", "display:none;"); }
        }
    });

    $("#user_help_menu").click(function () {
        if ($("#help_faq").is(':hidden') == true) {
            var left = $("#user_help_menu").offset().left - $(".main_area").offset().left;
            $("#help_faq").attr("style", "display:block;left:" + left + "px;min-width:" + $("#help_faq").outerWidth() + "px");
            $("#navi").attr("style", "display:none;");
        } else { $("#help_faq").attr("style", "display:none;"); }
    });
});

function closeNavi() {
    $("#navi").attr("style", "display:none;");
    $("#help_faq").attr("style", "display:none;");
}