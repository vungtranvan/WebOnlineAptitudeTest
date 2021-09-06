var transferConfig = {
    pageSize: $('.pageSizeItem').val(),
    pageIndex: 1
}

var transferController = {
    init: function () {
        transferController.loadData();
    },
    registerEvents: function () {
        $('[data-toggle="tooltip"]').tooltip();

        $('.txtSearchCandidate').off('input').on('input', function () {
            transferController.loadData(true);
        });

        $('.pageSizeItem').off('change').on('change', function () {
           transferConfig.pageSize = $(this).val();
            transferController.loadData(true);
        });

        $('.btnDelete').off('click').on('click', function (e) {
            e.preventDefault();
            $('#id-item').text($(this).data('id'));
            $('#modal-delete').modal('show');
        });

        $('.Save-Delete').off('click').on('click', function (e) {
            e.preventDefault();
            var id = $('#id-item').text();
            transferController.deleteCandidate(id);
            $('#modal-delete').modal('hide');
        });

    },
    deleteCandidate: function (id) {
        $.ajax({
            url: '/Transfer/Locked',
            type: 'POST',
            data: { id: id },
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    transferController.loadData(true);
                    toastr.success(response.message, response.title);
                } else {
                    toastr.error(response.message, response.title);
                }
            }
        });
    },
    loadData: function (changePageSize) {
        var keyword = $('.txtSearchCandidate').val();

        $.ajax({
            url: '/Transfer/LoadData',
            type: 'GET',
            data: {
                keyword: keyword,
                page: transferConfig.pageIndex,
                pageSize: transferConfig.pageSize
            },
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    var data = response.data;
                    var html = '';
                    $.each(data, function (i, item) {

                        html +=
                            `<tr class="intro-x">
                                <td scope="col">${i + 1}</td>
                                <td><img src="${item.Image}" style="width:30px" alt="" /></td>
                                <td>${item.UserName}</td>
                                <td>${item.Name}</td>
                                <td>${item.Email}</td>
                                <td>${candiController.formatDate(item.CreatedDate)}</td >
                                <td class="FlexIconAction">
                                      <a class="flex items-center text-theme-6 btnDelete" href="#" data-id="${item.Id}"> <i class="fas fa-trash-alt"></i> Delete </a>
                                </td>
                             </tr>`;
                    });

                    $('#tblDataCandidate').html(html);

                    if (response.data.length == 0) {
                        $('#tableCandidate').hide();
                        $('.textEmpty').show();
                    } else {
                        $('.textEmpty').hide();
                        $('#tableCandidate').show();
                    }
                    transferController.pagination(response.totalRow, function () {
                        transferController.loadData();
                    }, changePageSize);
                    transferController.registerEvents();

                }
            }
        });
    },
    formatDate: function (dateString) {
        var newDate = new Date(parseInt(dateString.replace('/Date(', '')));
        var dd = newDate.getDate();
        var mm = newDate.getMonth() + 1;
        var yyyy = newDate.getFullYear();
        let hours = newDate.getHours();
        let minutes = newDate.getMinutes();

        if (dd < 10) {
            dd = '0' + dd;
        }
        if (mm < 10) {
            mm = '0' + mm;
        }
        if (hours < 10) {
            hours = '0' + hours;
        }
        if (minutes < 10) {
            minutes = '0' + minutes;
        }

        newDate = mm + '/' + dd + '/' + yyyy + ' ' + hours + ':' + minutes;
        return newDate;
    },
    pagination: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / transferConfig.pageSize);

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
                transferConfig.pageIndex = page;
                setTimeout(callback, 0);
            }
        });
    }
}

transferController.init();