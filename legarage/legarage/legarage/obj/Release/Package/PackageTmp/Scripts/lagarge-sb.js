//legarage seachbox
$(".search_Box").on({
    keyup: function () {
        searchAjax();
    }, focus: function () {
        searchAjax();
    }
});

function searchAjax() {
    var val = $(".search_Box").val();
    //document.getElementById("resultBox").style.display = "block";
    if (val.trim() == "") {
        //document.getElementById("resultBox").style.display = "none";
        $("#resultBox").empty().hide();
        return;
    }
    $.ajax({
        dataType: "json",
        url: "/home/GetSearchResult?keyWord=" + val,
        success(result) {

            success(result);
        },
        error(error) {
            //console.log(error);
        }
    });
}

function success(result) {
    if (result != null && result.count > 0) {
        //console.log(result.count);
        $("#resultBox").empty().show();
        $(result.data).each(function (index, element) {
            $('#resultBox').append(`
                    <a href="/${this.Type}/Details?Id=${this.ID}" class="list-group-item card rounded-0 list-group-item-action" aria-current="true">
                        <div class="row g-0">
                            <div class="col-md-3 rounded">
                              <img src="/Images/${this.Type}/${this.Img}" alt="this is iamge for ${this.Title}" width="50" height="50">
                            </div>
                            <div class="d-flex col-md-9 justify-content-between">
                                <h5 class="mb-1 h5 ">${this.Title}</h5>
                                <p><small class="muted mt-5" style="font-size:.75rem;">${this.Type}</small></p>
                            </div>
                        </div>
                        <!--<p class="mb-1">Some placeholder content in a paragraph.</p>
                        <small>And some small print.</small>-->
                    </a>`);
                        
        });

    }
    else {
        $('#resultBox').empty().append(`
                    <a href="#" class="list-group-item list-group-item-action disabled" aria-current="true">
                        <div class="d-flex w-100 justify-content-between">
                            <h5 class="mb-1 muted"> No Data Found !</h5>
                            <small></small>
                        </div>
                    </a>
                        `);
        //console.log(result.count);
    }


}

//function searchResult(data) {
//    switch (data.Type) {
//        case 'Garages':
//            //console.log('Garages');
//            document.getElementById('Garages').innerHTML += `
//                    <div  onclick="goToGarageDetails('${data.ID}');" class="list-group-item list-group-item-action  mb-2" aria-current="true">
//                        ${data.Title}
//                    </div>`;
//            break;
//        case 'RentOffices':
//            //console.log('RentOffices');
//            document.getElementById('RentOffices').innerHTML += `
//                    <div  onclick="goToGarageDetails('${data.ID}');" class="list-group-item list-group-item-action  mb-2" aria-current="true">
//                        ${data.Title}
//                    </div>`;
//            break;
//        case 'Vehichles':
//            document.getElementById('RentOffices').innerHTML += `
//                    <div  onclick="goToGarageDetails('${data.ID}');" class="list-group-item list-group-item-action  mb-2" aria-current="true">
//                        ${data.Title}
//                    </div>`;
//            //console.log('Vehichles');
//            break;
//        case 'Products':
//            //console.log('Products');
//            document.getElementById('Products').innerHTML += `
//                    <div  onclick="goToGarageDetails('${data.ID}');" class="list-group-item list-group-item-action  mb-2" aria-current="true">
//                        ${data.Title}
//                    </div>`;
//            break;
//        case 'Winches':
//            //console.log('Winches');
//            document.getElementById('Winches').innerHTML += `
//                    <div  onclick="goToGarageDetails('${data.ID}');" class="list-group-item list-group-item-action  mb-2" aria-current="true">
//                        ${data.Title}
//                    </div>`;
//            break;
//        case 'Offers':
//            //console.log('offer');
//            document.getElementById('offer').innerHTML += `
//                    <div  onclick="goToGarageDetails('${data.ID}');" class="list-group-item list-group-item-action  mb-2" aria-current="true">
//                        ${data.Title}
//                    </div>`;
//            break;
//        default:
//            //console.log(data.Type);
//            break;
//    }
//}
