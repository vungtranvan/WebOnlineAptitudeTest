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
                },
                textTimeTest: {
                    required: true,
                    min: 5
                }
            }
        });

        $('.btnUpdate').off('click').on('click', function (e) {
            e.preventDefault();
            categoryExamController.getDetailCategoryExam($(this).data('id'));
            $('#modal-updateCateExam').modal('show');
        });

        $('.btnCancleCateExam').off('click').on('click', function (e) {
            e.preventDefault();
            $('#modal-updateCateExam').modal('hide');
        });

        $('.btnUpdateCateExam').off('click').on('click', function (e) {
            e.preventDefault();
            if ($("#FormUpdateCategoryExam").valid()) {
                var id = $('#textIdCateExam').val();
                var name = $('#textNameCateExam').val();
                var timetest = $('#textTimeTest').val();
                categoryExamController.updateCategoryExam(id, name, timetest);
                $('#modal-updateCateExam').modal('hide');
            }
        });

    },
    updateCategoryExam: function (id, name, timetest) {
        $.ajax({
            url: '/CategoryExam/Update',
            type: 'POST',
            data: {
                Id: id,
                Name: name,
                TimeTest: timetest
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
                    $('#textTimeTest').val(data.TimeTest);
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
                   
                    $.each(data, function (i, item) {
                        html +=
                                `<tr class="intro-x">
                                    <td>${i+1}</td>
                                    <td>${item.Name}</td>
                                    <td class="text-center">${item.TimeTest}</td>
                                    <td class="FlexIconAction">
                                        <a class="flex items-center text-theme-6 btnUpdate" href="#" data-id="${item.Id}"> <i class="fas fa-edit"></i> Edit </a>
                                    </td>
                                 </tr>`;
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