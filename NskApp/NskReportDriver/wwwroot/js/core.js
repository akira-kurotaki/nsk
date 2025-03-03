// ■リアルタイム帳票出力
$(function () {
    $("#CreateReport").click(function () {
        var forms = $('form');
        if (forms.valid()) {
            var reportControlId = $("#ReportControlId")[0].value;
            var userId = $("#UserId")[0].value;
            var joukenId = $("#JoukenId")[0].value;
            var todofukenCd = $("#TodofukenCd")[0].value;
            var kumiaitoCd = $("#KumiaitoCd")[0].value;
            var shishoCd = $("#ShishoCd")[0].value;

            var shishoList0 = $("#ShishoList_0_")[0].value;
            var shishoList1 = $("#ShishoList_1_")[0].value;
            var shishoList2 = $("#ShishoList_2_")[0].value;
            var shishoList3 = $("#ShishoList_3_")[0].value;
            var shishoList4 = $("#ShishoList_4_")[0].value;
            var shishoList5 = $("#ShishoList_5_")[0].value;
            var shishoList6 = $("#ShishoList_6_")[0].value;
            var shishoList7 = $("#ShishoList_7_")[0].value;
            var shishoList8 = $("#ShishoList_8_")[0].value;
            var shishoList9 = $("#ShishoList_9_")[0].value;

            var url = './Home/CreatRealtimeReport?reportControlId=' + reportControlId + '&userId=' + userId + '&joukenId=' + joukenId + '&todofukenCd=' + todofukenCd + '&kumiaitoCd=' + kumiaitoCd + '&shishoCd=' + shishoCd + '&shishoList[0]=' + shishoList0 + '&shishoList[1]=' + shishoList1 + '&shishoList[2]=' + shishoList2 + '&shishoList[3]=' + shishoList3 + '&shishoList[4]=' + shishoList4 + '&shishoList[5]=' + shishoList5 + '&shishoList[6]=' + shishoList6 + '&shishoList[7]=' + shishoList7 + '&shishoList[8]=' + shishoList8 + '&shishoList[9]=' + shishoList9;
            windowOpen(url);
        }
    });
});

// ■帳票を別ウィンドウ表示
function windowOpen(actionName) {
    x = (window.innerWidth) / 1;
    y = (window.innerHeight) / 1;
    window.open(actionName, "", "left=0,top=0,width=" + x + ",height=" + y + ",scrollbars=1,toolbar=0,menubar=0,staus=0,resizable=1");
}