var reportConfig = {
    pageSize: $('.pageSizeItem').val(),
    pageIndex: 1,
    fromDate: $('#FromDate').val(),
    toDate: $('#ToDate').val()
}

var reportController = {
    init: function () {
        reportController.loadData();
    },
    registerEvents: function () {

        $('.pageSizeItem').off('change').on('change', function () {
            reportConfig.pageSize = $(this).val();
            reportController.loadData(true);
        });

        $('#FromDate').off('change').on('change', function () {
            reportConfig.fromDate = $(this).val();
            reportController.loadData(true);
        });

        $('#ToDate').off('change').on('change', function () {
            reportConfig.toDate = $(this).val();
            reportController.loadData(true);
        });
    },
    loadData: function (changePageSize) {
        $.ajax({
            url: '/Report/LoadData',
            type: 'GET',
            data: {
                fromDate: reportConfig.fromDate,
                toDate: reportConfig.toDate,
                page: reportConfig.pageIndex,
                pageSize: reportConfig.pageSize
            },
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    var data = JSON.parse(response.data);

                    var html = '';
                    $.each(data, function (i, item) {

                        html +=
                            `<tr class="intro-x">
                                <td scope="col">${i + 1}</td>
                                <td>${item.TestSchedulesName}</td>
                                <td>${reportController.formatDate(item.TestSchedulesDateStart)}</td>
                                <td>${reportController.formatDate(item.TestSchedulesDateEnd)}</td>
                                <td>${item.CountCandidate}</td>
                                <td>${item.CountCandidateSuccess}</td>
                                <td>${item.CountCandidateOut}</td>
                                <td>${item.CountCandidatePass}</td>
                                <td>${item.CountCandidateNotPass}</td>
                             </tr>`;
                    });

                    $('#tblDataReport').html(html);

                    if (data.length == 0) {
                        $('#tableReport').hide();
                        $('.textEmpty').show();
                    } else {
                        $('.textEmpty').hide();
                        $('#tableReport').show();
                    }
                    reportController.pagination(response.totalRow, function () {
                        reportController.loadData();
                    }, changePageSize);
                    reportController.registerEvents();
                }
            }
        });
    },
    formatDate: function (dateString) {
        var newDate = new Date(dateString.replace('/Date(', ''));
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

        newDate = dd + '/' + mm + '/' + yyyy + ' ' + hours + ':' + minutes;
        return newDate;
    },
    pagination: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / reportConfig.pageSize);

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
                reportConfig.pageIndex = page;
                setTimeout(callback, 0);
            }
        });
    }
}

reportController.init();