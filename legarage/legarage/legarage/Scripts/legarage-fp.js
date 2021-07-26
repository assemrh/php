//forget pasword
function Check_code() {
    var x = $("#verfication_code").val();
    $.ajax({
        type: "POST",
        dataType: "json",
        data: { verfication_code: x },
        url: '/Users/Check_code',
        success(result) {
            if (result.code == 200) {

            }
            else {
                $("#Information").modal("show");
                $("#msg").html(result.msg);
            }
        },
        error(error) {
        }
    });
}