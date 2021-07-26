function getPage(Nuber, URL) {

    $.ajax({
        type: "Post",
        dataType: "json",
        data: { page: Nuber },
        url: URL,
        success(result) {
            if (result.code == 200) {
                document.getElementById("Table").innerHTML = result.data;
                //console.log("Refreshed");
            }
            else {
                $("#Information").show(500);
                document.getElementById("msg").innerHTML = result.msg;
            }
        },
        error(error) {
        }
    });
}

//$(".page-link").on("click", function () {
//    var number = "";
//    var controller = "";
//    var url = `/${controller}/get`;
//    getPage(number,url);
//});

function RefreshPage(URL) {
    $.ajax({
        type: "Post",
        dataType: "json",
        url: URL,
        success(result) {
            if (result.code == 200) {
                document.getElementById("Table").innerHTML = result.data;
                //console.log("Refreshed");
            }
            else {
                $("#Information").show(500);
                document.getElementById("msg").innerHTML = result.msg;
            }
        },
        error(error) {
        }
    });
}

function PrevPage(URL) {
    $.ajax({
        type: "Post",
        dataType: "json",
        url: URL,
        success(result) {
            if (result.code == 200) {
                document.getElementById("Table").innerHTML = result.data;
                //console.log("Refreshed");
            }
            else {
                $("#Information").show(500);
                document.getElementById("msg").innerHTML = result.msg;
            }
        },
        error(error) {
        }
    });
}

function NextPage(URL) {
    $.ajax({
        type: "Post",
        dataType: "json",
        url: URL,
        success(result) {
            if (result.code == 200) {
                document.getElementById("Table").innerHTML = result.data;
                //console.log("Refreshed");
            }
            else {
                $("#Information").show(500);
                document.getElementById("msg").innerHTML = result.msg;
            }
        },
        error(error) {
        }
    });
}








var $pagination = $('#pagination'),
    totalRecords = 0,
    records = [],
    displayRecords = [],
    recPerPage = 10,
    page = 1,
    totalPages = 0;
$.ajax({
    url: "http://dummy.restapiexample.com/api/v1/employees",
    async: true,
    dataType: 'json',
    success: function (data) {
        records = data;
        //console.log(records);
        totalRecords = records.length;
        totalPages = Math.ceil(totalRecords / recPerPage);
        //apply_pagination();
    }
});

$(document).ready(function () {
    //Initially load pagenumber=1
    GetPageData(1);
});
function GetPageData(pageNum, pageSize) {
    //After every trigger remove previous data and paging
    $("#tblData").empty();
    $("#paged").empty();
    $.getJSON("/Home/GetPaggedData", { pageNumber: pageNum, pageSize: pageSize }, function (response) {
        var rowData = "";
        for (var i = 0; i < response.Data.length; i++) {
            rowData = rowData + '<tr><td>' + response.Data[i].Id + '</td><td>' + response.Data[i].Name + '</td></tr>';
        }
        $("#tblData").append(rowData);
        PaggingTemplate(response.TotalPages, response.CurrentPage);
    });
}
//This is paging temlpate ,you should just copy paste
function PaggingTemplate(totalPage, currentPage) {
    var template = "";
    var TotalPages = totalPage;
    var CurrentPage = currentPage;
    var PageNumberArray = Array();


    var countIncr = 1;
    for (var i = currentPage; i <= totalPage; i++) {
        PageNumberArray[0] = currentPage;
        if (totalPage != currentPage && PageNumberArray[countIncr - 1] != totalPage) {
            PageNumberArray[countIncr] = i + 1;
        }
        countIncr++;
    };
    PageNumberArray = PageNumberArray.slice(0, 5);
    var FirstPage = 1;
    var LastPage = totalPage;
    if (totalPage != currentPage) {
        var ForwardOne = currentPage + 1;
    }
    var BackwardOne = 1;
    if (currentPage > 1) {
        BackwardOne = currentPage - 1;
    }

    template = "<p>" + CurrentPage + " of " + TotalPages + " pages</p>"
    template = template + '<ul class="pager">' +
        '<li class="previous"><a href="#" onclick="GetPageData(' + FirstPage + ')"><i class="fa fa-fast-backward"></i>&nbsp;First</a></li>' +
        '<li><select ng-model="pageSize" id="selectedId"><option value="20" selected>20</option><option value="50">50</option><option value="100">100</option><option value="150">150</option></select> </li>' +
        '<li><a href="#" onclick="GetPageData(' + BackwardOne + ')"><i class="glyphicon glyphicon-backward"></i></a>';

    var numberingLoop = "";
    for (var i = 0; i < PageNumberArray.length; i++) {
        numberingLoop = numberingLoop + '<a class="page-number active" onclick="GetPageData(' + PageNumberArray[i] + ')" href="#">' + PageNumberArray[i] + ' &nbsp;&nbsp;</a>'
    }
    template = template + numberingLoop + '<a href="#" onclick="GetPageData(' + ForwardOne + ')" ><i class="glyphicon glyphicon-forward"></i></a></li>' +
        '<li class="next"><a href="#" onclick="GetPageData(' + LastPage + ')">Last&nbsp;<i class="fa fa-fast-forward"></i></a></li></ul>';
    $("#paged").append(template);
    $('#selectedId').change(function () {
        GetPageData(1, $(this).val());
    });
}