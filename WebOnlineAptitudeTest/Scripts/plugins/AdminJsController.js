var adminConfig = {
    pageSize: $('.pageSizeItem').val(),
    pageIndex: 1
}

var adminController = {
    init: function () {
        adminController.loadData();
    },
    registerEvents: function () {
        $('.txtSearchAdmin').off('input').on('input', function () {
            adminController.loadData(true);
        });

        $('.pageSizeItem').off('change').on('change', function () {
            adminConfig.pageSize = $(this).val();
            adminController.loadData(true);
        });

        $('.btnDelete').off('click').on('click', function (e) {
            e.preventDefault = false;
            $('#id-item').text($(this).data('id'));
            $('#modal-delete').modal('show');
        });

        $('.Save-Delete').off('click').on('click', function (e) {
            var id = $('#id-item').text();
            adminController.deleteAdmin(id);
            $('#modal-delete').modal('hide');
        });
        $('.btnDetail').off('click').on('click', function (e) {
            e.preventDefault = false;
            adminController.getDetailAdmin($(this).data('id'));
            $('#modal-detailAdmin').modal('show');
        });

    },
    loadData: function (changePageSize) {
        var keyword = $('.txtSearchAdmin').val();

        $.ajax({
            url: '/Account/LoadData',
            type: 'GET',
            data: {
                keyword: keyword,
                page: adminConfig.pageIndex,
                pageSize: adminConfig.pageSize
            },
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            STT: i + 1,
                            Id: item.Id,
                            Image: item.Image,
                            UserName: item.UserName,
                            Name: item.Name,
                            Email: item.Email
                            //CreatedDate: item.CreatedDate == null ? "" : cadiController.formatDate((item.CreatedDate)),
                            //UpdatedDate: item.UpdatedDate == null ? "" : cadiController.formatDate(item.UpdatedDate)
                        });
                    });
                    $('#tblDataAdmin').html(html);
                    if (response.data.length == 0) {
                        $('#tblDataAdmin').hide();
                        $('.textEmpty').show();
                    } else {
                        $('.textEmpty').hide();
                        $('#tblDataAdmin').show();
                    }
                    adminController.pagination(response.totalRow, function () {
                        adminController.loadData();
                    }, changePageSize);
                    adminController.registerEvents();

                }
            }
        });
    },
    pagination: function (totalRow, changePageSize) {
        var totalPage = Math.ceil(totalRow / adminConfig.pageSize);
        console.log("totalpage: " + totalPage)
        //Unbind pagination if it existed or click change pageSize
        if ($('#pagination').length === 0 || changePageSize === true) {
            console.log("oke")
            $('#pagination').empty();
            $('#pagination').removeData('twbs-pagination');
            $('#pagination').unbind("page");
        }

        $('#pagination').twbsPagination({
            totalpages: totalPage,
            first: '<i class="fas fa-angle-double-left"></i>',
            prev: '<i class=" fas fa-angle-left"></i>',
            next: '<i class="fas fa-angle-right"></i>',
            last: '<i class="fas fa-angle-double-right"></i>',
            visiblePages: 8,
            onPageClick: function (event, page) {
                adminConfig.pageIndex = page;
            }
        });
    }
}

adminController.init();

