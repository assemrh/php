$(document).bind("DOMSubtreeModified load change ", function () {
    $("img").each(function () {
        $(this).on("error", function () {
            $(this).attr("src", "/Images/no-images.png");
        });
    });
});


function Initialization(iscpanel = false) {
    $(document).ready(function () {
        if (document.getElementById("Factory") != null) Get_Options('Factory', '', '/Home/GetCountries', 'Factory');
        if (document.getElementById("MCountry") != null) Get_Options('MCountry', '', '/Home/GetCountries', 'Market');
        if (document.getElementById("phone_keys") != null) Get_PhoneKeys("phone_keys");
        if (document.getElementById("types") != null)Get_VehicleTypes();
        Get_Market();
        if (document.getElementById("country") != null) Get_Contries("Market", "country", iscpanel);
        Get_VehicleTypes();
        //console.log(document.getElementsByClassName("dropzone"));
        try {
            DropZone123();}
        catch (err) {
            //console.log(err);

        }

    });
}
function Get_Brands() {
    var valOfId = $("#Factory").val();
    var html;
    
    if (valOfId != -1) {
        $.ajax({
            type: "POST",
            dataType: "json",
            data: { ID: valOfId},
            url: '/Home/GetBrands/',
            success(result) {
                if (result.code == 200) {
                    $("#Brand").removeAttr("disabled");
                    html = `<option value='-1'>` + $("#Brand > option").html() + `</option>`;
                    var obj = jQuery.parseJSON(result.data);
                    for (var x in obj) {
                        html += `<option value="` + obj[x].ID + `">` + obj[x].Name + `</option>`;
                    }
                    $("#Brand").html(html);
                }
                else {
                    $("#Information").show(500);
                    //console.log(result.msg);
                }
            },
            error(error) {
            }
        });
    }
    else {
        //console.log("x ==-1");
        var html = `<option value='-1'>` + $("#Brand > option").html() + `</option>`;
        $("#Brand option[value='-1']").prop("selected", true);
        $("#Brand").html(html);
        $("#Brand").prop("disabled", true);
        html = `<option value='-1'>` + $("#Model > option").html() + `</option>`;
        $("#Model option[value='-1']").prop("selected", true);
        $("#Model").html(html);
        $("#Model").prop("disabled", true);
    }
}

function Get_Cities() {

}

function Get_Contries(MorForAll, DivID, IsCPanel = false) {
    //console.log("contry");
    var html = "";
    
    $.ajax({
        type: "POST",
        dataType: "json",
        url: "/Home/GetCountries/",
        data: { Option: MorForAll, iscpanel: IsCPanel},
        success(result) {
            if (result.code == 200) {
                var obj = jQuery.parseJSON(result.data);
                //console.log("obj.length :" + obj.length);
                if (obj.length == 1) {
                    html = `<option class="country" value="` + obj[0].ID + `">` + obj[0].Name +`</option>`;
                    $("#" + DivID).html(html);
                    //$("#" + DivID).
                    if (IsCPanel != true) { document.getElementById(DivID).disabled = true;}
                    
                    Get_Options('city', 'country', '/Home/GetCities/');
                    //console.log("done");
                }
                else if (obj.length < 1) {
                    $("#" + DivID).html(`<option value='-1'>No Country in database</option>`);
                }
                else {
                    $("#" + DivID).removeAttr("disabled");
                    html = `<option value='-1'>` + result.label + `</option>`;
                
                    for (var x in obj) {
                        html += `<option value="` + obj[x].ID + `">` + obj[x].Name + `</option>`;
                    }
                    $("#" + DivID).html(html);
                    //console.log(html);
                    if (IsCPanel != true)
                    document.getElementById("city").disabled = true;
                }

            }
            else {
                //console.log("Warning " + result.msg);
            }
        },
        error(error) {
            //console.log(error);
        }
    });
}

function getPage(Nuber, URL) {

    $.ajax({
        type: "Get",
        data: { page: Nuber },
        url: URL,
        success(result) {
            document.getElementById("data_countainer").innerHTML = result;
            //console.log("Refreshed");
        },
        error(error) {
        }
    });
}
$(document).ready(function () {
    $(".btn-Search").on("click", function () {
        var formdata = $("#filterform").serialize();
        var controller = $(this).data("controller");//data-action
        var action = $(this).data("action");//data-action
        var URL = `/${controller}/${action}`;
        $.ajax({
            type: "Get",
            data: formdata,
            url: URL,
            success(result) {
                $("#data_countainer").html(result);
            },
            error(error) {
                //console.log(error);
            }
        });
    });
});
/*/
const link = document.getElementById('link-1');
link.getAttribute('data-target');
$( "div" ).data( "role" );
*/


function Get_Garages() {
    var formdata = $("#filterform").serialize();
    //console.log(formdata);
    $.ajax({
        type: "Get",
        data: formdata,
        url: "/Garages/GetGarages",
        success(result) {
                $("#fill_out_garage").html(result);
        },
        error(error) {
            //console.log(error);
        }
    });
}

function Get_Models() {
    var valOfId = $("#Brand").val();
    if (valOfId != -1) {
        $.ajax({
            type: "POST",
            dataType: "json",
            data: { ID: valOfId },
            url: '/Home/GetModels/',
            success(result) {
                if (result.code == 200) {
                    $("#Model").removeAttr("disabled");
                    var html = `<option value='-1'>` + result.labels.SelectModel + `</option>`;
                    var obj = jQuery.parseJSON(result.data);
                    for (var x in obj) {
                        html += `<option value="` + obj[x].ID + `">` + obj[x].Name + `</option>`;
                    }
                    $("#Model").html(html);
                }
                else {
                    $("#Information").show(500);
                    //console.log(result.msg);
                }
            },
            error(error) {
            }
        });
    }
    else {
        //console.log("x ==-1");
        var html = `<option value='-1'>` + $("#Model > option").html() + `</option>`;
        $("#Model option[value='-1']").prop("selected", true);
        $("#Model").html(html);
        $("#Model").prop("disabled", true);
    }

}

function FilterParts() {
    var formdata = $("#filterform").serialize();
    //console.log(formdata);
    $.ajax({
        type: "Post",
        dataType: "json",
        data: formdata,
        url: '/Garages/FilterResult',
        success(result) {
            if (result.code == 200) {
                if (result.data != null) {
                    //console.log("Filter done");
                    //console.log(result.data);
                }
                else {
                    //console.log(error);
                }
            }
            else {
                //console.log(error);
            }
        },
        error(error) {
            //console.log(error);
        }
    });
}

function Get_Offers() {

}

function Get_Options(HtmlDivId, ValDivID, ControllerUrl, Parameter) {

    // HtmlDivId => The ID of Div where the options will be innered by innerHTML
    // ValDivID => The ID of Div Where the value is to be taken from 
    // ControllerUrl => The URl of the controller which will return the data
    // Option => Optional Parameters

    var valOfId;
    if (document.getElementById(ValDivID) != null) {
        valOfId = $("#" + ValDivID).val();
    }

    if (valOfId != -1) {
        $.ajax({
            type: "POST",
            dataType: "json",
            data: { ID: valOfId, Option: Parameter },
            url: ControllerUrl,
            success(result) {
                if (result.code == 200) {
                    $("#" + HtmlDivId).removeAttr("disabled");
                    var html = `<option value='-1'>` + result.label + `</option>`;
                    var obj = jQuery.parseJSON(result.data);
                    for (var x in obj) {
                        html += `<option value="` + obj[x].ID + `">` + obj[x].Name + `</option>`;
                    }
                    $("#" + HtmlDivId).html(html);
                }
                else {
                    $("#Information").show(500);
                    //console.log(result.msg);
                }
            },
            error(error) {
            }
        });
    }
    else {
        //console.log("x ==-1");
        var html = `<option value='-1'>` + $("#" + HtmlDivId + "> option").html() + `</option>`;
        $('#' + HtmlDivId + " option[value='-1']").prop("selected", true);
        $("#" + HtmlDivId).html(html);
        //$("#"+ HtmlDivId).prop("disabled", true);
    }
}

function Get_Parts() {
    var formdata = $("#filterform").serialize();
    //console.log(formdata);
    $.ajax({
        type: "Get",
        data: formdata,
        url: '/Parts/Get_Parts',
        success(result) {
            ////console.log(result.data);
            $("#fill_it_out").html(result);
        },
        error(error) {
            //console.log(error);
        }
    });
}

function Get_offers() {
    var formdata = $("#filterform").serialize();
    //console.log(formdata);
    $.ajax({
        type: "Post",
        dataType: "json",
        data: formdata,
        url: '/Parts/Get_Parts',
        success(result) {
            if (result.code == 200) {
                if (result.data != null) {
                    //console.log(result.data);
                    $("#fill_it_out").html(result.data);
                }
                else {
                    //console.log(error);
                }
            }
            else {
                //console.log(error);
            }
        },
        error(error) {
            //console.log(error);
        }
    });
}

function Get_PhoneKeys(DivID) {
    //console.log(DivID);
    $.ajax({
        type: "POST",
        dataType: "json",
        url: "/Home/GetPhoneKeys/",
        success(result) {
            if (result.code == 200) {
                if (document.getElementById(DivID) != null) {
                    document.getElementById(DivID).innerHTML = result.data;
                }
            }
            else {
                //console.log("Warning", result.msg);
            }
        },
        error(error) {
        }
    });
}

function Get_RentOffices() {
    var formdata = $("#filterform").serialize();
    //console.log(formdata);
    $.ajax({
        type: "Post",
        dataType: "json",
        data: formdata,
        url: '/RentOffices/GetRentOffices',
        success(result) {
            if (result.code == 200) {
                if (result.data != null) {
                    $("#Rent_Index").html(result.data);
                    ////console.log("Get_RentOffices done");
                }
                else {
                    //console.log(error);
                }
            }
            else {
                //console.log(error);
            }
        },
        error(error) {
            //console.log(error);
        }
    });
}

function Get_Services() {

}

function Get_vehicle() {
    var formdata = $("#filterform").serialize();
    //console.log(formdata);
    $.ajax({
        type: "Post",
        dataType: "json",
        data: formdata,
        url: "/Vehicles/GetVehicles",
        success(result) {
            if (result.code == 200) {
                $("#vehiclecarts").html(result.data);
            }
            else {
                $("#Information").show(500);
                //console.log(result.msg);
            }
        },
        error(error) {
            //console.log(error);
        }
    });
}

function Get_VehicleTypes() {
    $.ajax({
        type: "POST",
        dataType: "json",
        url: '/Home/GetVehicleTypes/',
        success(result) {
            if (result.code == 200) {
                var html = `<option value='-1'>` + $("#types > option").html() + `</option>`;
                var obj = jQuery.parseJSON(result.data);
                for (var x in obj) {
                    html += `<option value="` + obj[x].ID + `">` + obj[x].Name + `</option>`;
                }
                $("#types").html(html);
            }
            else {
                $("#Information").show(500);
                //console.log(result.msg);
            }
        },
        error(error) {
        }
    });
}

function Get_Winches() {
    var formdata = $("#filterform").serialize();
    //console.log(formdata);
    $.ajax({
        type: "Get",
        data: formdata,
        url: "/Winches/GetWinches",
        success(result) {
            $("#data_countainer").html(result);
        },
        error(error) {
            //console.log(error);
        }
    });
}

function goToGarageDetails(gid) {
    window.location.href = "/Garages/GarageDetails?id=" + gid;
}

function getcheckboxes(ClassName) {
    var items_list = document.getElementsByClassName(ClassName);
    var checkboxes = [];
    for (var i = 0; i < items_list.length; i++) {
        var item = items_list[i];
        if (item.getAttribute('type') == 'checkbox') {
            checkboxes.push(item);
        }
    }
    return checkboxes;
}
function toggle(source, ClassName) {
    checkboxes = getcheckboxes(ClassName);
    //ClassName = "." + ClassName;
    //$(ClassName);
    for (var i = 0; i < checkboxes.length; i++) {
        checkboxes[i].checked = source.checked;
        $(checkboxes[i]).click();
    }
}

function select_brands() {
    var val = $('#select-brand').val();
    //console.log("doe");
    if (val != "-1") {
        $(".model-row").addClass("hide-bf");
        $("." + val).removeClass("hide-bf");
    }
    else {
        $(".model-row").removeClass("hide-bf");
    }
}


function search_boxes(searchWord, targetrRow, displayMod = "flex") {
    let checkboxeslist = document.getElementsByClassName(targetrRow);
    let reg = new RegExp(searchWord, 'gi');
    for (var i = 0; i < checkboxeslist.length; i++) {
        if (checkboxeslist[i].textContent.toLowerCase().search(reg) > -1) {
            checkboxeslist[i].style.display = displayMod;
        }
        else {
            checkboxeslist[i].style.display = "none";
        }
    }
}

$(document).ready(function () {
    $("input.brands").on("change select click", function () {
        $("input.model-row").css("display","none");
    });
});

function select_models() {
    $('input:radio.brands').on('change select click', function () {
        var val = $(this).data('brand');
        if (val != '-1') {
            $('.model-row').addClass('hide-bf');
            $('.' + val).removeClass('hide-bf');
        }
        else {
            $('.model-row').removeClass('hide-bf');
        }
    });

    $('input:checkbox.brands').on('change select click', function () {
        $('.model-row').addClass('hide-bf');
        $('input:checkbox.brands').each(function (index, element) {
            //console.log(index + " - " + element);
            var val = $(element).data('brand');
            //console.log(val);
            if ($(element).is(':checked')) {
                $('.' + val).removeClass('hide-bf');
            }
            else {
                //$('.' + val).addClass('hide-bf');
            }
        });
    });
 }

//function formatState(state) {
//    if (!state.id) {
//        return state.text;
//    }
//    var baseUrl = "/user/pages/images/flags";
//    var $state = $(
//        '<span><img src="' + baseUrl + '/' + state.element.value.toLowerCase() + '.png" class="img-flag" /> ' + state.text + '</span>'
//    );
//    return $state;
//};

//$(document).ready(function () {
//    $("");
//    function formatState(state) {
//        if (!state.id) {
//            return state.text;
//        }
//        var baseUrl = "/user/pages/images/flags";
//        var $state = $(
//            '<span><img src="' + baseUrl + '/' + state.element.value.toLowerCase() + '.png" class="img-flag" /> ' + state.text + '</span>'
//        );
//        return $state;
//    };

//    $(".js-example-templating").select2({
//        templateResult: formatState
//    });
//});
