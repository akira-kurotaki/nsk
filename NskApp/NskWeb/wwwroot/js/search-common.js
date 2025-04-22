const SC_FNC_KUMIAIINTO = 1;
const SC_FNC_HINSHU = 2;
const SC_FNC_SANTIMEIGARA = 3;
const SC_FNC_TOKEITANI = 4;

// fnc
// 1 = NSK_000020D	組合員等コード検索子画面
// 2 = NSK_000021D	品種コード検索子画面
// 3 = NSK_000022D	産地別銘柄コード検索子画面
// 4 = NSK_000023D	統計単位地域コード検索子画面
function searchCommon(fnc, idCode, idName) {
    //alert("[" + fnc + "," + idCode + "," + idName + "]");
    var sid = "";
    var aid = "";
    var url = "";
    var idx = idCode + ";" + idName;
    switch (fnc) {
        case SC_FNC_KUMIAIINTO:
            //url = '@Url.Action("Init", "D000020", new { key = "IDX", area = "F000" })'.replace("IDX", idx);
            sid = "D000020";
            aid = "F000";
            break;
        case SC_FNC_HINSHU:
            //url = '@Url.Action("Init", "D000021", new { key = "IDX", area = "F000" })'.replace("IDX", idx);
            sid = "D000021";
            aid = "F000";
            break;
        case SC_FNC_SANTIMEIGARA:
            //url = '@Url.Action("Init", "D000022", new { key = "IDX", area = "F000" })'.replace("IDX", idx);
            sid = "D000022";
            aid = "F000";
            break;
        case SC_FNC_TOKEITANI:
            //url = '@Url.Action("Init", "D000023", new { key = "IDX", area = "F000" })'.replace("IDX", idx);
            sid = "D000023";
            aid = "F000";
            break;
        default:
            return;
    }
    url = "/" + aid + "/" + sid + "?key=" + idx;
    //alert("[" + url + "," + sid + "," + aid + "," + idx + "]");
    const childwindow = window.open(url, 'childwindow', 'width=1000,height=800');

}
function receiveValueFromChild(key, arr) {
    var ids = key.split(';');
    $('#' + ids[0]).val(arr[0]);
    $('#' + ids[1]).val(arr[1]);
}
function searchCommonSetName(fnc, idCode, idName) {
    var sid = "";
    var aid = "";
    var url = "";
    switch (fnc) {
        case SC_FNC_KUMIAIINTO:
            //url = '@Url.Action("GetNameByCode", "D000020", new { area = "F000" })';
            sid = "D000020";
            aid = "F000";
            break;
        case SC_FNC_HINSHU:
            //url = '@Url.Action("GetNameByCode", "D000021", new { area = "F000" })';
            sid = "D000021";
            aid = "F000";
            break;
        case SC_FNC_SANTIMEIGARA:
            //url = '@Url.Action("GetNameByCode", "D000022", new { area = "F000" })';
            sid = "D000022";
            aid = "F000";
            break;
        case SC_FNC_TOKEITANI:
            //url = '@Url.Action("GetNameByCode", "D000023", new { area = "F000" })';
            sid = "D000023";
            aid = "F000";
            break;
        default:
            return;
    }
    url = "/" + aid + "/" + sid + "/GetNameByCode";
    var targetCd = $("#" + idCode).val();
    var data = { "TargetCd": targetCd }
    $("#" + idName).val("");
    $.ajax({
        type: 'POST',
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(data),
        dataType: 'json',
        url: url,
        success: function (data) {
            $("#" + idName).val(data.ReturnNm).text(data.ReturnNm);
        }
    })

}
