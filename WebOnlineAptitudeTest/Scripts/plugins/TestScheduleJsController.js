var testScheduleConfig = {
    pageSize: $('.pageSizeItem').val(),
    pageIndex: 1
}

var testScheduleController = {
    init: function () {
        testScheduleController.loadData();
    },
    registerEvents: function () {
        $('[data-toggle="tooltip"]').tooltip();

        $('.txtSearch').off('input').on('input', function () {
            testScheduleController.loadData(true);
        });

        $('.pageSizeItem').off('change').on('change', function () {
            testScheduleConfig.pageSize = $(this).val();
            testScheduleController.loadData(true);
        });

        $('.btnDelete').off('click').on('click', function (e) {
            e.preventDefault();
            $('#id-item').text($(this).data('id'));
            $('#modal-delete').modal('show');
        });

        $('.Save-Delete').off('click').on('click', function (e) {
            e.preventDefault();
            var id = $('#id-item').text();
            testScheduleController.deleteCandidate(id);
            $('#modal-delete').modal('hide');
        });
    },
    deleteCandidate: function (id) {
        $.ajax({
            url: '/TestSchedule/Locked',
            type: 'POST',
            data: { id: id },
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    testScheduleController.loadData(true);
                    toastr.success(response.message, response.title);
                } else {
                    toastr.error(response.message, response.title);
                }
            }
        });
    },
    loadData: function (changePageSize) {
        var keyword = $('.txtSearch').val();

        $.ajax({
            url: '/TestSchedule/LoadData',
            type: 'GET',
            data: {
                keyword: keyword,
                page: testScheduleConfig.pageIndex,
                pageSize: testScheduleConfig.pageSize
            },
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    var data = response.data;
                    var html = '';
                    $.each(data, function (i, item) {

                        var status = `<i class="fa fa-circle text-primary font-12" data-toggle="tooltip" title="Undone"></i>`;
                        if (item.Status == 1) {
                            status = `<i class="fa fa-circle text-danger font-12" data-toggle="tooltip" title="In Progress"></i>`;
                        } else if (item.Status == 2) {
                            status = `<i class="fa fa-circle text-success font-12" data-toggle="tooltip" title="Done"></i>`;
                        }

                        html +=
                            `<tr class="intro-x">
                                <td scope="col">${i + 1}</td>
                                <td>${item.Name}</td>
                                <td>${testScheduleController.formatDate(item.DateStart)}</td>
                                <td>${testScheduleController.formatDate(item.DateEnd)}</td>
                                <td class="text-center">${status}</td>
                                <td class="FlexIconAction">
                                        <a class="flex items-center mr-3" href="/Admin/TestSchedule/InsertOrUpdate/${item.Id}"> <i class="fas fa-edit"></i> Edit </a>
                                        <a class="flex items-center text-theme-6 btnDelete" href="#" data-id="${item.Id}"> <i class="fas fa-trash-alt"></i> Delete </a>
                                </td>
                             </tr>`;
                    });

                    $('#tblDataTestSchedule').html(html);

                    if (response.data.length == 0) {
                        $('#tableTestSchedule').hide();
                        $('.textEmpty').show();
                    } else {
                        $('.textEmpty').hide();
                        $('#tableTestSchedule').show();
                    }
                    testScheduleController.pagination(response.totalRow, function () {
                        testScheduleController.loadData();
                    }, changePageSize);
                    testScheduleController.registerEvents();

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
        var totalPage = Math.ceil(totalRow / testScheduleConfig.pageSize);

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
                testScheduleConfig.pageIndex = page;
                setTimeout(callback, 0);
            }
        });
    }
}

testScheduleController.init();