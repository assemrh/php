function Offer_type_switcher() {
    var zyx = $("#Offer_type option:selected").val();
    var owner_ID = $("#Owner").val();
    //console.log(zyx +" "+ owner_ID);
    switch (zyx) {
        case 'Garage':
            //console.log('Garage');
            get_offers_details_section('/Offers/Garage', owner_ID);
            break;
        case 'RentOffice':
            //console.log('RentOffice');
            get_offers_details_section('/Offers/RentOffice', owner_ID);
            break;
        case 'Vehichle':
            //console.log('Vehichle');
            get_offers_details_section('/Offers/Vehicle', owner_ID);
            break;
        case 'Product':
            //console.log('Product');
            get_offers_details_section('/Offers/Part', owner_ID);
            break;
        case 'Winches':
            //console.log('Winches');
            get_offers_details_section('/Offers/Winche', owner_ID);
            break;
        case 'select':
            //console.log('select');
            $("#offer-details").html("");
            break;
        default:
            $("#offer-details").html("");
            break;
    }
}

function Offer_type_switcher_n() {
    var zyx = $("#Offer_type option:selected").val();
    //console.log(zyx);
    switch (zyx) {
        case 'Garage':
            //console.log('Garage');
            //
            break;
        case 'RentOffice':
            //console.log('RentOffice');
            //
            break;
        case 'Vehichle':
            //console.log('Vehichle');
            //
            break;
        case 'Product':
            //console.log('Product');
            //
            break;
        case 'Winches':
            //console.log('Winches');
            //
            break;
        case 'select':
            //console.log('select');
            //
            break;
        default:
            //
            break;
    }
}

function get_offers_details_section(URL, owner_ID) {
    if (owner_ID != 'select') {
        $.ajax({
            type: "Post",
            url: URL,
            data: { Owner: owner_ID},
            success(result) {
                //console.log("Done");
                $("#offer-details").html(result);
            },
            error(error) {
                //console.log(error);
            }
        });
    }

}

function Add_new_offer(URL) {
    var formdata = $("#newofferform").serialize();
    $.ajax({
        type: "Post",
        url: URL,
        data: formdata,
        dataType: "json",
        success(result) {
            if (result.code == 200) {
                //console.log("Done ");
                alert(result.msg);
            }
            else {

                alert(result.msg);
                //console.log(result.msg);
                $("#Information").show(500);
                document.getElementById("msg").innerHTML = result.msg;
            }
        },
        error(error) {
            //console.log(error);
            alert(error);
        }
    });
}


function get_garage_offer_options() {
    var ID = $("#select_garages option:selected").val();
    $.ajax({
        type: "Post",
        dataType: "json",
        url: "/Offers/getGarageOfferOptions",
        data: { ID: ID },
        success(result) {
            if (result.code == 200) {
                document.getElementById("brands-Section").style.display = "block";
                document.getElementById("select-brands").innerHTML = result.brands;
                document.getElementById("categories-Section").style.display = "block";
                document.getElementById("select-categories").innerHTML = result.cate;
                document.getElementById("vehicle-types-Section").style.display = "block";
                document.getElementById("select-vehicle-types").innerHTML = result.VehicleTypes;
                //console.log(result.msg);
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


function get_rent_offer_options() {
    var ID = $("#select_rents option:selected").val();
    $.ajax({
        type: "Post",
        dataType: "json",
        url: "/Offers/getOfficeOfferOptions",
        data: { ID: ID },
        success(result) {
            if (result.code == 200) {
                document.getElementById("models-Section").style.display = "block";
                document.getElementById("select-models").innerHTML = result.models;
                document.getElementById("vehicle-types-Section").style.display = "block";
                document.getElementById("select-vehicle-types").innerHTML = result.VehicleTypes;
                //console.log(result.msg);
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