var categoryExamConfig = {
    pageSize: $('.pageSizeItem').val(),
    pageIndex: 1
}

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

        $('.txtSearch').off('input').on('input', function () {
            categoryExamController.loadData(true);
        });

        $('.pageSizeItem').off('change').on('change', function () {
            categoryExamConfig.pageSize = $(this).val();
            categoryExamController.loadData(true);
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
    loadData: function (changePageSize) {
        var keyword = $('.txtSearch').val();
        $.ajax({
            url: '/CategoryExam/LoadData',
            type: 'GET',
            data: {
                keyword: keyword,
                page: categoryExamConfig.pageIndex,
                pageSize: categoryExamConfig.pageSize
            },
            dataType: 'json',
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
                    categoryExamController.pagination(response.totalRow, function () {
                        categoryExamController.loadData();
                    }, changePageSize);
                    categoryExamController.registerEvents();
                }
            }
        });
    },
    pagination: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / categoryExamConfig.pageSize);

        if ($('#pagination').length === 0 || changePageSize === true) {
            $('#pagination').empty();
            $('#pagination').removeData('twbs-pagination');
            $('#pagination').unbind("page");
        }

        $('#pagination').twbsPagination({
            totalPages: totalPage,
            first: '<i class="fas fa-angle-double-left"></i>',
            prev: '<i class=" fas fa-angle-left"></i>',
            next: '<i class="fas fa-angle-right"></i>',
            last: '<i class="fas fa-angle-double-right"></i>',
            visiblePages: 8,
            onPageClick: function (event, page) {
                categoryExamConfig.pageIndex = page;
                setTimeout(callback, 0);
            }
        });
    }
}

categoryExamController.init();