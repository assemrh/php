function Refresh(controllerName) {
    $.ajax({
        dataType: "html",
        url: `/${controllerName}/content`,
        success(result) {
            $("#Table").html(result);
            console.log("Refreshed");
        },
        error(error) {
            $("#modal").modal("hide");
            $("#Information").modal('show');
            $("#info_msg").html(error.responseText);
        }
    });
}

function Get_Contries() {
    //Get: Contries select options
    $.ajax({
        type: "POST",
        dataType: "json",
        url: "/CP/GetCountries/",
        success(result) {
            if (result.code == 200) {
                $("#country").html(result.data);
            }
            else {
                console.log(result.msg);
            }
        },
        error(error) {
            console.log(error.responseText);
        }
    });
}

function Get_Cites() {
        //Get: Cites select options
    var x = $("#country").val();
    if (x != -1) {
        $.ajax({
            type: "POST",
            dataType: "json",
            data: { Country_ID: x },
            url: "/CP/GetCities/",
            success(result) {
                if (result.code == 200) {
                    $("#city").html(result.data);
                }
                else {
                    console.log(result.data);
                }
            },
            error(error) {
                console.log(error.responseText);
            }
        });
    }
}

function Get_Neighborhoods() {
            //Get: Neighborhoods select options
    var x = document.getElementById("city").value;
    if (x != -1) {
        $.ajax({
            type: "POST",
            dataType: "json",
            data: { City_ID: x },
            url: "/CP/GetNeighborhoods/",
            success(result) {
                if (result.code == 200) {
                    document.getElementById("neighborhood").innerHTML = result.data;
                }
                else {
                    document.getElementById("neighborhood").innerHTML = result.data;
                }
            },
            error(error) {
                console.log(error.responseText);
            }
        });
    }
}


/**
 * 
 *
 * Mainm functionalities Add/Edit/Delete/Details
 */

function Add(controllerName) {
    $.ajax({
        type: "Get",
        url: `/${controllerName}/Add`,
        success(result) {
            $("#ModalBody").html(result);
            Get_Contries();
        },
        error(error) {
            console.log(error.responseText);
        }
    });
}

function Adding(controllerName) {
    var formdata = $("#AddForm").serialize();
    $("#Modal").modal('hide');
    $.ajax({
        type: "Post",
        data: formdata,
        dataType: "json",
        url: `/${controllerName}/Adding`,
        success(result) {
            $("#Information").modal('show');
            if (result.code == 200) {
                document.getElementById("info_msg").innerHTML = "Added";
            }
            else {
                document.getElementById("info_msg").innerHTML = result.msg;
            }
            $("#modal").modal("hide"); 
        },
        error(error) {
            console.log(error.responseText);
        },
        complete: function () {
            Refresh(controllerName);
        }
    });
}
function AddNewUser() {
    $("#Modal").modal('hide');
        $.ajax({
            type: "Post",
            url: "/cp_users/Adding",
            dataType: "json",
            data: new FormData($("form")[0]),
            cache: false,
            contentType: false,
            processData: false,
            success(result) {
                $("#Information").modal('show');
                if (result.code == 200) {
                    document.getElementById("info_msg").innerHTML = "Added";
                }
                else {
                    document.getElementById("info_msg").innerHTML = result.msg;
                }
            },
            error(error) {
                console.log(error.responseText);
            },
            complete: function () {
                Refresh('cp_users');
            }
        });
/*    });*/
}
//    var formdata = $("#adduserform").serialize();
//    $("AdduserForm").submit(
//        function (event) {
//            $.ajax({
//                type: "Post",
//                data: formdata,
//                enctype: "multipart/form-data",
//                dataType:"json",
//                success(result) {
//                    alert("dsds");
//                    $("#Information").modal('show');
//                    if (result.code == 200) {
//                        document.getElementById("info_msg").innerHTML = "Added";
//                    }
//                    else {
//                        document.getElementById("info_msg").innerHTML = result.msg;
//                    }
//                },
//                error(error) {
//                    console.log(error.responseText);
//                },
//                complete: function () {
//                    Refresh('cp_users');
//                }
//            });
//        }
        
//    );


function Edit(Id, controllerName) {
    $.ajax({
        type: "Get",
        data: { ID: Id },
        url: `/${controllerName}/Edit`,
        success(result) {
            $("#ModalBody").html(result);
        },
        error(error) {
            console.log(error.responseText);
        }
    });
}

function Editing(controllerName) {
    var formdata = $("#EditForm").serialize();
    $.ajax({
        type: "Post",
        data: formdata,
        dataType: "json",
        url: `/${controllerName}/Editing`,
        success(result) {
            if (result.code == 200) {
                $("#info_msg").html("Edited");
                $("#Modal").modal('hide');
                $("#Information").modal('show');
                Get_Contries();
            }
            else {
                $("#Information").modal('show');
                $("#info_msg").html(result.msg);
            }
        },
        error(error) {
            console.log(error.responseText);
        },
        complete: function () {
            Refresh(controllerName);
        }
    });
}

function Delete(id, controllerName) {
    $("#DeleteWarning").modal('show');
    $("#button").on('click',function () {
        $.ajax({
            type: "Post",
            data: { ID: id },
            dataType: "json",
            url: `/${controllerName}/Delete`,
            success(result) {
                $("#DeleteWarning").modal('hide');
                if (result.code == 200) {
                    $("#info_msg").html(result.msg);
                    $("#Information").modal('show');
                }
                else if (result.code == 404) {
                    $("#Information").modal('show');
                    $("#info_msg").html(result.msg);
                } else {
                    $("#info_msg").html(result);
                }
                Refresh(controllerName);
            },
            error(error) {
            }
        });
    } );
}

function Details(id, controllerName) {
    $.ajax({
        data: { ID: id },
        url: `/${controllerName}/Details`,
        success(result) {
            $("#ModalBody").html(result);
        },
        error(error) {
            console.log(error.responseText);
        }
    });
}




/**
 * 
 * 
 */



//function AddUser() {
//    Adding("/CP_Users/AddUser/", "AddUserForm");
//}
//function AddCity() {
//    Adding("/CP_Cities/AddCity/", "AddCityForm");
//    Refresh("/CP_Cities/content/");
//}





//function ShowAddModal(ControllerUrl) {
//    $.ajax({
//        type: "Post",
//        url: ControllerUrl,
//        success(result) {
//            var x = document.getElementById("ModalBody");
//            x.innerHTML = result;
//            console.log("done");
//        },
//        error(error) {
//            console.log(error.msg);
//            console.log(error.responseText);

//        }
//    });
//}



//function ShowAddUser() {
//    ShowAddModal("ModalBody", "/CP_Users/Add/");
//    Get_Contries();
//}
//function ShowAddCity() {
//    ShowAddModal("ModalBody", "/CP_Cities/Add/");
//    Get_Contries();
//    console.log("done");

//}

//function ShowEditCity(Id) {
//    Edit(Id, "/CP_Cities/Edit/");
//}
//function ShowEditUser(Id) {
//    Edit(Id, "/CP_Users/Edit/");
//}

//function EditCity(Id) {
//    Editing(Id, "/CP_Cities/EditCity/");
//}












//function fill_DataDetails(id) {
//    $.ajax({
//        type: "Post",
//        data: { UID: id },
//        url: "/CP_Users/Details/",
//        success(result) {
//            var x = document.getElementById("UserModalBody");
//            x.innerHTML = result;
//        },
//        error(error) {
//        }
//    });
//}

//function deleteEntry(id) {
//    var con = confirm("Are you sure to delete it?");
//    if (con == true) {
//        $.ajax({
//            type: "Post",
//            data: { id: id },
//            dataType: "json",
//            url: "/CP_Users/DeleteUser/",
//            success(result) {
//                if (result.code == 200) {
//                    $("#Information").show(500);
//                    document.getElementById("info_msg").innerHTML = "Deleted Successfully";
//                }
//                else {
//                    $("#Information").show(500);
//                    document.getElementById("info_msg").innerHTML = result.msg;
//                }
//            },
//            error(error) {
//                console.log(error.responseText);
//            }
//        });
//    }
//}

//function Update(id) {
//    var formdata = $("#EditUserForm").serialize();
//    $.ajax({
//        type: "Post",
//        data: formdata,
//        dataType: "json",
//        url: "/CP_Users/EditUser/",
//        success(result) {
//            if (result.code == 200) {
//                $("#Information").show(500)
//                document.getElementById("info_msg").innerHTML = "Edited";
//                Get_Contries();
//                //DropZone123();
//            }
//            else {
//                $("#Information").show(500)
//                document.getElementById("info_msg").innerHTML = result.msg;
//            }
//        },
//        error(error) {
//            console.log(error.responseText);
//        }
//    });
//}

//function fill_Data(id) {
//    $.ajax({
//        type: "Post",
//        data: { UID: id },
//        url: "/CP_Users/Edit/",
//        success(result) {
//            var x = document.getElementById("UserModalBody");
//            x.innerHTML = result;
//            //DropZone123();
//            Get_Contries();
//        },
//        error(error) {
//            console.log(error.responseText);
//        }
//    });
//}

//function ShowAdd() {
//    $.ajax({
//        type: "Post",
//        url: "/CP_Users/Add/",
//        success(result) {
//            var x = document.getElementById("UserModalBody");
//            x.innerHTML = result;
//            Get_Contries();
//            //DropZone123();
//        },
//        error(error) {
//        }
//    });
//}