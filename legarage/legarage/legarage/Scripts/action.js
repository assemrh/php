
function Refresh(URL) {
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

function getContent(url) {
    $.ajax({
        dataType: "html",
        url: url,
        success(result) {
            $("#Table").html(result);
        },
        error(error) {
            $("#modal").modal("hide");
            $("#Information").modal('show');
            $("#info_msg").html("something wrong");
            ////alert("Console'a bak ");
            //console.log(error);
        }
    });
}

$("#rate").on("click", function () {
    $.ajax({
        dataType: "html",
        url: "/Rating/Rate/",
        success(result) {
            $("#ModalBody").html(result);
        },
        error(error) {
            $("#ModalBody").html(error.responseText);
            //$("#modal").modal("hide");
            //$("#Information").modal('show');
            //$("#info_msg").html("something wrong");
            ////alert("Console'a bak ");
            //console.log(error);
        }
    });
});
//sendRate
function sendRate() {
    var _value = $('.rating-inputs:checked').val();
    var _src_type = $("input[name='src_type']").val();
    var _src_id = $('input[name="src_id"]').val();

    var values = {
        value: _value,
        src_type: _src_type,
        src_id: _src_id
    };

    $.ajax({
        type: "Post",
        data: values,
        dataType: "Json",
        url: "/Rating/AddRating",
        success(result) {
            $("#ModalBody").html(result.msg);

            if (result.code == 401)
                $("#ModalBody").html("You have to log in");
            if (result.code == 200)
                $("#ModalBody").html(result.msg);
            //console.log(result);

        },
        error(error) {

            //console.log(error);
            $("#ModalBody").html(JSON.parse(error.responseText).msg);
        }
    });
}
function Update(URL,refresh) {
    var formdata = $("#EditForm").serialize();
    $.ajax({
        type: "Post",
        data: formdata,
        dataType: "json",
        url: URL,
        success(result) {
            if (result.code == 200) {
                $("#Information").show(500);
                document.getElementById("msg").innerHTML = result.msg;
                $("#Modal").modal('hide');
                $(".modal-backdrop").modal('hide');
                Refresh(refresh);
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

function Edit(id,URL) {
    $.ajax({
        type: "Post",
        data: { ID: id },
        url: URL,
        success(result) {
            var x = document.getElementById("ModalBody");
            x.innerHTML = result;
            //Get_Contries('Market', 'country');

            DropZone123();
        },
        error(error) {
            //console.log(error.responseText);
        }
    });
}

function Details(id, URL) {
    $.ajax({
        type: "Post",
        data: { ID: id },
        url: URL,
        success(result) {
            var x = document.getElementById("ModalBody");
            x.innerHTML = result;
        },
        error(error) {
            //console.log(error.responseText);
            document.getElementById("ModalBody").innerHTML(error.responseText);
        }
    });
}

function Add(URL, TorF) {
    $.ajax({
        type: "Post",
        url: URL,
        success(result) {
            var x = document.getElementById("ModalBody");
            x.innerHTML = result;
            ////console.log("Result is : \n"+result);
            //Get_Contries(TorF);
            Get_PhoneKeys("phone_keys");
            Get_Contries(TorF, 'country', true);
            select_models();
            DropZone123();
        },
        error(error) {
            //console.log(error.responseText);
        }
    });
}

function Adding(URL,refresh) {
    var formdata = $("#AddForm").serialize();
    $.ajax({
        type: "Post",
        data: formdata,
        dataType: "json",
        url: URL,
        success(result) {
            if (result.code == 200) {
                $("#Information").show(500);
                document.getElementById("msg").innerHTML = result.msg;
                $("#Modal").hide();
                $(".modal-backdrop").hide();
                if (URL != null) {
                    Refresh(refresh);
                }
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

function Images(id, URL) {
    $.ajax({
        type: "Post",
        data: { ID: id },
        url: URL,
        success(result) {
            var x = document.getElementById("ModalBody");
            x.innerHTML = result;
            DropZone123();
        },
        error(error) {
        }
    });
}

function Delete(id, URL) {
    $("#DeleteWarning").show(500);
    document.getElementById('button').onclick = function() {
        $.ajax({
            type: "Post",
            data: { ID: id },
            dataType: "json",
            url: URL,
            success(result) {
                if (result.code == 200) {
                    $("#Information").show(500);
                    document.getElementById("msg").innerHTML = result.msg;
                    $("#DeleteWarning").hide();
                    $(".modal-backdrop").hide();
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
}

function DeleteImage(EID, IID, URL, RefreshURL) {
    $("#DeleteWarning").show(500);
    document.getElementById('button').onclick = function () {
        $.ajax({
            type: "Post",
            dataType: "json",
            data: { ID: IID },
            url: URL,
            success(result) {
                if (result.code == 200) {
                    document.querySelector("#id").value;
                    $("#DeleteWarning").hide();
                    RefreshImages(EID, RefreshURL);
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
}

function AddImage(id,URL,RefreshURL) {
    $.ajax({
        type: "Post",
        data: { ID: id },
        dataType: "json",
        url: URL,
        success(result) {
            if (result.code == 200) {
                $("#Information").show(500);
                document.getElementById("msg").innerHTML = result.msg;
                RefreshImages(id, RefreshURL);
                var link = document.querySelector(".dz-remove");
                while (link != null) {
                    link.click();
                    link = document.querySelector(".dz-remove");
                }
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

function RefreshImages(id,URL) {
    $.ajax({
        type: "Post",
        dataType: "json",
        data: { ID : id },
        url: URL,
        success(result) {
            if (result.code == 200) {
                document.getElementById("ImagesForm").innerHTML = result.data;
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

function DropZone123() {
    Dropzone.autoDiscover = false;
    //console.log("Drop Zone");
    $(".dropzone").dropzone({
        url: "/Home/DropAttachmentsSingle/",
        addRemoveLinks: true,
        maxFiles: 1,
        removedfile: function (file) {
            var name = file.name;
            $.ajax({
                type: 'POST',
                dataType: "json",
                url: '/Home/CelarImagesSession/',
            });
            var _ref;
            return (_ref = file.previewElement) != null ? _ref.parentNode.removeChild(file.previewElement) : void 0;
        }
    });
}
 

function seekoRent() {
    $("#maintabdisp").hide();
    $("#maintabdispparts").hide();
    $("#maintabdispgarage").hide();
    $("#maintabdispbuyandsell").hide();
    $("#maintabdisprent").show();
}

function seekoRecovery() {
    $("#maintabdispparts").hide();
    $("#maintabdispgarage").hide();
    $("#maintabdispbuyandsell").hide();
    $("#maintabdisprent").hide();
    $("#maintabdisp").show();
}

function seekoParts() {

    $("#maintabdisp").hide();
    $("#maintabdispgarage").hide();
    $("#maintabdispbuyandsell").hide();
    $("#maintabdisprent").hide();
    $("#maintabdispparts").show();
}

function seekoSale() {
    $("#maintabdisp").hide();
    $("#maintabdispgarage").hide();
    $("#maintabdisprent").hide();
    $("#maintabdispparts").hide();
    $("#maintabdispbuyandsell").show();
}

function seekoGarages() {
    $("#maintabdispparts").hide();
    $("#maintabdisp").hide();
    $("#maintabdispbuyandsell").hide();
    $("#maintabdisprent").hide();
    $("#maintabdispgarage").show();

}

function goToGarages() {
    window.location.href = "/Garages/Index";
}

function goToRecovery() {
    window.location.href = "/Winches/Index";
}

function goToBuyandSell() {
    window.location.href = "/Vehicles/Index";
}
function goToAdd() {
    window.location.href = "/Home/Add";
}

function goToAbout() {
    window.location.href = "/Home/About";
}
function goToAbout() {
    window.location.href = "/Home/About";
}

function goToProductDetails() {
    window.location.href = "/Part/PartDetails";
    //console.log("@Resources.Home.buycontactsellerindetails");
}
function highlightSelected() {
    $("#garageHead").addClass("beekoHigh");
}

function goTo(somewhere) {
    window.location.href = somewhere;
}
function goToHome() {
    window.location.href = "/Home";
}
function goToContact() {
    window.location.href = "/Home/Contact";
}
function goToOffer() {
    window.location.href = "/Offer/Index";
}
function goToGarage() {
    window.location.href = "/Garages/Index";
}
function goToBuyAndSell() {
    window.location.href = "/Vehicles/Index";
}
function goToRent() {
    window.location.href = "/RentOffices/Index";
}
function goToRent() {
    window.location.href = "/RentOffices/Index";
}

function goToUsedPart() {
    window.location.href = "/Parts/Index";
}
function goToRecovery() {
    window.location.href = "/Winches/Index";
}
function goToLogin() {
    window.location.href = "/Users/Index";
}
function goToRegister() {
    window.location.href = "/Users/Register";
}

function phone_key(iso, code) {
    var x = '<img src="https://www.countryflags.io/' + iso + '/shiny/24.png">'
    $("#phone_keyMenuButton").html(x);
    $("#phone_key").val(code);
}
function selectCounty() {
    $("#country").val($("#countries").val());
    //Get_Options('city', 'country', '/CP/GetCities/');
}

function Select_Market(ID) {
    $.ajax({
        type: "Post",
        dataType: "json",
        data: { MID: ID },
        url: "/Home/SelectMarket",
        success(result) {
            if (result.code == 200) {
               goToHome(); //console.log("Market Session has been selected");
            }
            else {  }
        },
        error(error) {
        }
    });
}
function Get_Market() {
    $.ajax({
        type: "Post",
        dataType: "json",
        url: "/Home/GetMarket",
        success(result) {
            if (result.code == 200) {
                if (result.name != null) {
                    $("#Select_Market").html(result.name);
                   
                    //console.log("Market selected");
                }
                else {
                    $("#Select_Market").html("All Market");
                    //console.log(error);
                }
                if (result.iso != null) {
                    $("#Market_flag").html('<img src="https://www.countryflags.io/' + result.iso + '/shiny/24.png">');
                }
                else {
                    $("#Market_flag").html('<i class="fa fa-globe" aria-hidden="true"></i>');

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


function getFilters(htmlID, formID, ControllerUrl) {
    var formdata = $("#" + formID).serialize();
    //console.log(formdata);
    $.ajax({
        type: "Post",
        dataType: "json",
        data: formdata,
        url: ControllerUrl,
        success(result) {
            if (result.code == 200) {
                if (result.data != null) {
                    //console.log("Filter done");
                    $("#" + htmlID).html(result.data);
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

function restFilters() {
    goTo("/Garages/Index");
}


$("#phone_keyMenuButton").click(function () {
    //console.log("log");
    $("#phone_keys").toggle(
        function () {
            $(this).addClass("show");
        }, function () {
            $(this).removeClass("show");
        }
    );
});


function SiteElementDDLChanged(element,URL) {
    //console.log('tesat');
    if (element == 'out') {
        //hide the second ddl and its lable
        $(".secound-ddl").hide();
        //show the title , link , image
        $(".site-elements-section").show();
    } else {
        //show the second ddl and its lable
        $(".secound-ddl").show();
        //hide the title , link , image
        $(".site-elements-section").hide();
        // call ajax function to get all elemnts instand of element parameters and fill the second ddl and its lable
        $.ajax({
            type: "Post",
            data: { table_name : element},
            dataType: "json",
            url: URL,
            success(result) {
                if (result.code == 200) {
                    document.getElementById("secound_ddl").innerHTML = result.data;
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
    //chech element and if not out get data and fill ddl and disable the divs
    //if out enable the dives 
}



function forget_password() {
    let email = $("#valied_email").val();
    $.ajax({
        type: "Post",
        data: { valied_email: email },
        dataType: "json",
        url: "/users/Send_me_Code",
        success(result) {
            if (result.code == 200) {
                goTo("/users/Forgotpassword")
                document.getElementById("secound_ddl").innerHTML = result.data;
            }
            else {
                $('#Information').modal('show');
                $("#msg").html(result.msg);
            }
        },
        error(error) {
        }
    });

}

function resetpassword() {
    $.ajax({
        type: "Post",
        data: { valied_email: email },
        dataType: "json",
        url: "/users/Send_me_Code",
        success(result) {
            if (result.code == 200) {
                goTo("/users/Forgotpassword")
                document.getElementById("secound_ddl").innerHTML = result.data;
            }
            else {
                $('#Information').modal('show');
                $("#msg").html(result.msg);
            }
        },
        error(error) {
        }
    });
}


$("#cng_psswrd_frm").on("submit", function (event) {
    event.preventDefault();
    var formdata = $(this).serialize();
    //console.log(formdata);
    var URL = $(this).attr('action');
    $.ajax({
        type: "Post",
        dataType: "json",
        data: formdata,
        url: URL,
        success(result) {
            //console.log(result);
            if (result.code == 200) {
                if (result.msg != null) {
                    setTimeout(function () {
                        ////alert("seccses");
                        window.location.href = "/users/Index";
                    }, 3000);
                    $("#Information").show(500);
                    document.getElementById("msg").innerHTML = result.msg;
                }
                else {
                    //console.log(result);
                }
            }
            else {
                //console.log(result);
            }
        },
        error(error) {
            //console.log(error);
        }
    });
});