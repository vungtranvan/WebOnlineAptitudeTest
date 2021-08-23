var categoryExamController = {
    init: function () {
        categoryExamController.loadData();
    },
    registerEvents: function () {
        $("#FormUpdateCategoryExam").validate({
            errorClass: "is-invalid error",
            rules: {
                textNameCateExam: {
                    required: true
                }
            }
        });

        $('.btnUpdate').off('click').on('click', function (e) {
            e.preventDefault = false;
            categoryExamController.getDetailCategoryExam($(this).data('id'));
            $('#modal-updateCateExam').modal('show');
        });

        $('.btnCancleCateExam').off('click').on('click', function (e) {
            e.preventDefault = false;
            $('#modal-updateCateExam').modal('hide');
        });

        $('.btnUpdateCateExam').off('click').on('click', function (e) {
            e.preventDefault = false;
            if ($("#FormUpdateCategoryExam").valid()) {
                var id = $('#textIdCateExam').val();
                var name = $('#textNameCateExam').val();
                categoryExamController.updateCategoryExam(id, name);
                $('#modal-updateCateExam').modal('hide');
            }
        });

    },
    updateCategoryExam: function (id, name) {
        $.ajax({
            url: '/CategoryExam/Update',
            type: 'POST',
            data: {
                Id: id,
                Name: name
            },
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    categoryExamController.loadData(true);
                    toastr.success(response.message, response.title);
                } else {
                    toastr.error(response.message, response.title);
                }
            }
        });
    },
    getDetailCategoryExam: function (id) {
        $.ajax({
            url: '/CategoryExam/Details',
            type: 'GET',
            data: { id: id },
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    var data = response.data;
                    $('#textIdCateExam').val(data.Id);
                    $('#textNameCateExam').val(data.Name);
                } else {
                    $('#modal-updateCateExam').modal('hide');
                    toastr.error("Can not find Category Exam", "Notification");
                }
            }
        });
    },
    loadData: function () {
        $.ajax({
            url: '/CategoryExam/LoadData',
            type: 'GET',
            success: function (response) {
                if (response.status == true) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                   
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            STT: i + 1,
                            Id: item.Id,
                            Name: item.Name,
                        });
                    });

                    $('#tblDataCategoryExam').html(html);
                    if (response.data.length == 0) {
                        $('#tableCategoryExam').hide();
                        $('.textEmpty').show();
                    } else {
                        $('.textEmpty').hide();
                        $('#tableCategoryExam').show();
                    }
                    categoryExamController.registerEvents();
                }
            }
        });
    }
}

categoryExamController.init();