////function AddnewCate(URL) {
////    $("#add-new").submit(

////    )

////    $.ajax({
////        type: "Post",
////        dataType: "json",
////        url: URL,
////        success(result) {
////            if (result.code == 200) {
////                document.getElementById("Table").innerHTML = result.data
////                //console.log("Refreshed")
////            }
////            else {
////                $("#Information").show(500)
////                document.getElementById("msg").innerHTML = result.msg
////            }
////        },
////        error(error) {
////        }
////    })
////}


////$(".btn-add").click(
////    function () {
////    var _frm = $(".add-form")[0]
////    var _data = new FormData(_frm)
////    var _action = $(_frm).attr('action')
////    var previous = $(_frm).data('previous')
////        $.ajax({
////            type: "Post",
////            data: _data,
////            url: _action,
////            dataType: "multipart/form-data",
////            processData: false,
////            success(result, textStatus, jqXHR) {
////                console.log(textStatus + ": " + jqXHR.status)
////                console.log(result)
////                if (jqXHR.status == 201)
////                    swal("Done", result.msg, "success")
////                setTimeout(function () {
////                    $(".alert-success").hide(600)
////                    if (previous != null || previous != '')
////                        location.href = previous
////                    else
////                        location.href = '/admin'
////                }, 2500)

////            },
////            error(jqXHR, textStatus, errorThrown) {
////                swal("Oops" + textStatus, "Something went wrong!" + textStatus, textStatus)
////                console.log(textStatus + ": " + jqXHR.status + " " + errorThrown)
////            }
////        })
////    }
////){
//    dropzone_input.click()
//}, false)

////dropzone end


async function AJAXSubmit(formData) {
        //var resultElement = oFormElement.elements.namedItem("result")

    //const formData = new FormData(oFormElement)
    const action = '/admin/ControlPanel/UploadFile/'
        try {
            const response = await fetch(action, {
                method: 'POST',
                body: formData
            })

            if (response.ok) {
                    //window.location.href = '/'
                console.log('ok:'+ response)
            }

            //resultElement.value = 'Result: ' + response.status + ' ' +
               // response.statusText
        } catch (error) {
            console.error('Error:', error)
        }
    }
//$('input[type="file"]').change(function () {
//    //var resultElement = oFormElement.elements.namedItem("result")
//    //const formData = new FormData(oFormElement)
//    const formData = new FormData()
//    const file = this.files[0]
//    const name = this.getAttribute("name")
//    console.log(name)
//    formData.append('file', file)
//    formData.append('name', name)
//    AJAXSubmit(formData)
//})
$('select[data-target]').change(function () {
    const id =  this.value
    const targetDiv = this.getAttribute('data-target')
    const URL = this.getAttribute('data-action') + "GetOption?id=" + id

    $.ajax({
        type: "Get",
        url: URL,
        success(result, textStatus, jqXHR) {
            console.log(jqXHR)
            console.log(result)
            document.getElementById(targetDiv).innerHTML = result
        },
        error(error) {
            console.error(error)
        }
    })

})

$("#form").submit(function (event) {

    //var formData = {
    //    name: $("#name").val(),
    //    email: $("#email").val(),
    //    superheroAlias: $("#superheroAlias").val(),
    //}

    event.preventDefault()

    var $form = $(this),
        term = $form.serialize(),
        url = $form.attr("action"),
        _data = new FormData(this),
        previous = $form.data('previous')

    console.log(_data)
    console.log(term)
    $.ajax({
        type: "Post",
        data: term,
        url: url,
        processData: false,
        success(result, textStatus, jqXHR) {
            console.log(textStatus + ": " + jqXHR.status)
            console.log(result)
            if (jqXHR.status == 201) {
                Swal.fire({
                    icon: 'success',
                    title: result.msg,
                    showConfirmButton: false,
                    timer: 2500
                })
                setTimeout(function () {
                    $(".alert-success").hide(600)
                    if (previous != null || previous != '')
                        location.href = previous
                    else
                        location.href = '/admin'
                }, 2500)
            }
            else {
                Swal.fire({
                    icon: 'warning',
                    title: result.msg,
                    showConfirmButton: false,
                    timer: 2500
                })
            }

        },
        error(result, textStatus, errorThrown) {
            console.log(result)
            console.log(textStatus)
            console.log(errorThrown)
            Swal.fire({
                icon: 'warning',
                title: result.responseJSON.msg,
                showConfirmButton: false,
                timer: 2500
            })
            console.log(textStatus + ": " + result.status + " " + errorThrown + result)
        }
    })
})