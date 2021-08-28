var accController = {
    init: function () {
        accController.registerEvents();
    },
    registerEvents: function () {

        $("#FormChangePass").validate({
            errorClass: "is-invalid error",
            rules: {
                passOld: {
                    required: true
                },
                passNew: {
                    required: true,
                    rangelength: [3, 6]
                },
                passNewConfirm: {
                    required: true,
                    rangelength: [3, 6],
                    equalTo: "#passNew"
                }
            },
            messages: {
                passNew: {
                    rangelength: "The length of Password should be 3-6 characters"
                },
                passNewConfirm: {
                    rangelength: "The length of Password should be 3-6 characters",
                    equalTo: "New password does not match"
                }
            }
        });

        $('#btnChangePassword').off('click').on('click', function (e) {
            e.preventDefault();
            $('#changePassModal').modal('show');
        });

        $('.btnSaveChangePass').off('click').on('click', function (e) {
            e.preventDefault();
            if ($("#FormChangePass").valid()) {
                var userName = $('#userNameAdmin').val();
                var passOld = $('#passOld').val();
                var passNew = $('#passNew').val();
                accController.changePass(userName, passOld, passNew);
                $('#changePassModal').modal('hide');
            }
        });
        $('.btnCancleChangePass').off('click').on('click', function (e) {
            e.preventDefault();
            $('#changePassModal').modal('hide');
        });
    },
    changePass: function (userName, passOld, passNew) {
        $.ajax({
            url: '/Account/ChangePassword',
            type: 'POST',
            data: {
                userName: userName,
                passOld: passOld,
                passNew: passNew
            },
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    toastr.success(response.message, response.title);
                    accController.resetInput();
                } else {
                    toastr.error(response.message, response.title);
                }
            }
        });
    },
    resetInput: function () {
        $('#passOld').val('');
        $('#passNew').val('');
        $('#passNewConfirm').val('');
    }
}

accController.init();