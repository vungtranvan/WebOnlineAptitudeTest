
var historyConfig = {
    pageSize: $('.pageSizeItem').val(),
    pageIndex: 1,
    idTeschedule: ($('select#DropDownLists option:selected').val() == '' ? 0 : $('select#DropDownLists option:selected').val())
}

var historyController = {
    init: function () {
        historyController.loadData();
    },
    registerEvents: function () {
        $('[data-toggle="tooltip"]').tooltip();
        $('.txtSearch').off('input').on('input', function () {
            historyController.loadData(true);
        });

        $('.pageSizeItem').off('change').on('change', function () {
            historyConfig.pageSize = $(this).val();
            historyController.loadData(true);
        });

        $('select#DropDownLists').off('change').on('change', function () {
            historyConfig.idTeschedule = ($('select#DropDownLists option:selected').val() == '' ? 0 : $('select#DropDownLists option:selected').val());
            historyController.loadData(true);
        });

        $('.btnDelete').off('click').on('click', function (e) {
            e.preventDefault();
            $('#id-item').text($(this).data('id'));
            $('#modal-delete').modal('show');
        });

        $('.Save-Delete').off('click').on('click', function (e) {
            e.preventDefault();
            var id = $('#id-item').text();
            historyController.deleteHistory(id);
            $('#modal-delete').modal('hide');
        });

    },
    deleteHistory: function (id) {
        $.ajax({
            url: '/HistoryTests/Locked',
            type: 'POST',
            data: { id: id },
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    historyController.loadData(true);
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
            url: '/HistoryTests/LoadData',
            type: 'GET',
            data: {
                keyword: keyword,
                idTeschedule: historyConfig.idTeschedule,
                page: historyConfig.pageIndex,
                pageSize: historyConfig.pageSize
            },
            dataType: 'json',
            success: function (response) {

                if (response.status == true) {

                    var data = JSON.parse(response.data);

                    var html = '';
                    $.each(data, function (i, item) {

                        var status = `<i class="fa fa-circle text-primary font-12" data-toggle="tooltip" title="New"></i>`;
                        if (item.Status == 1) {
                            status = `<i class="fa fa-circle text-secondary font-12" data-toggle="tooltip" title="Undone"></i>`;
                        } else if (item.Status == 2) {
                            status = `<i class="fa fa-circle text-warning font-12" data-toggle="tooltip" title="Scheduled"></i>`;
                        } else if (item.Status == 3) {
                            status = `<i class="fa fa-circle text-info font-12" data-toggle="tooltip" title="In Progress"></i>`;
                        } else if (item.Status == 4) {
                            status = `<i class="fa fa-circle text-success font-12" data-toggle="tooltip" title="Done"></i>`;
                        } else if (item.Status == 5) {
                            status = `<i class="fa fa-circle text-danger font-12" data-toggle="tooltip" title="Quit"></i>`;
                        }
                        let totalMarks = item.ToTalMark.toFixed(2);
                        html +=
                            ` <div class="card mb-0">
                                    <div class="card-header" id="headingOne">

                                        <div class="custom-accordion-title d-block">
                                            <span class="d-flex listQuest">
                                                <a class="qIcon collapsed" href="#collapse_`+ item.Id + `" data-toggle="collapse" aria-expanded="false" aria-controls="collapse_` + item.Id + `"> </a>
                                                <span class="qId"> `+ (i + 1) + `</span>
                                                <span class="qContent">`+ item.Name + `</span>
                                                <span class="qCategory">`+ item.UserName + `</span>
                                                <span class="qMark">`+ totalMarks + ` %</span>
                                                <span class="qStatus">`+ status + `</span>
                                                <span class="qAction">
                                                <a class="qDelete btnDelete" href=":javascript" data-id=`+ item.Id + `  >Delete</a>
                                                </span>
                                            </span>
                                        </div>

                                    </div>
                                    <div id="collapse_`+ item.Id + `" class="collapse" aria-labelledby="headingOne" data-parent="#accordion">
                                        <div class="card-body">
                                            <span class="d-flex answer-title">
                                                <span class="aId"> #</span>
                                                <span class="aCorrect">CategoryExamId</span>
                                                <span class="aCorrect">TestScheduleId</span>
                                                <span class="aCorrect">DateStart</span>
                                                <span class="aCorrect">DateEnd</span>
                                                <span class="aCorrect">Correct</span>
                                                <span class="aCorrect">TimeTest</span>
                                                <span class="aCorrect">TotalMark</span>
                                                <span class="aCorrect">Status</span>
                                            </span>
                                        </div>`;

                        $.each(item.HistoryTests, function (j, ans) {

                            var statusSub = `<i class="fa fa-circle text-primary font-12" data-toggle="tooltip" title="Undone"></i>`;
                            if (ans.Status == 1) {
                                statusSub = `<i class="fa fa-circle text-danger font-12" data-toggle="tooltip" title="In Progress"></i>`;
                            } else if (ans.Status == 2) {
                                statusSub = `<i class="fa fa-circle text-success font-12" data-toggle="tooltip" title="Done"></i>`;
                            }

                            let seconTime = ans.TimeTest - (Math.floor(ans.TimeTest / 60) * 60);
                            if (seconTime < 10) {
                                seconTime = '0' + seconTime;
                            }

                            let timespent = ans.TimeTest == 0 ? '' : Math.floor(ans.TimeTest / 60) + ":" + seconTime + 's';
                            let dateTimeStart = ans.DateStartTest == null ? '' : historyController.formatDate(ans.DateStartTest);
                            let datetimeEnd = ans.DateEndTest == null ? '' : historyController.formatDate(ans.DateEndTest);

                            html += ` <div class="card-body">
                                            <span class="d-flex listQuest">
                                                <span class="aId"> `+ (j + 1) + `</span>
                                                <span class="aCorrect">`+ ans.CategoryExamId + `</span>
                                                <span class="aCorrect">`+ ans.TestScheduleId + `</span>
                                                <span class="aCorrect">`+ dateTimeStart + `</span>
                                                <span class="aCorrect">`+ datetimeEnd + `</span>
                                                <span class="aCorrect">`+ ans.CorectMark + `/` + ans.TotalMark + `</span>
                                                <span class="aCorrect">`+ timespent + `</span>
                                                <span class="aCorrect">`+ ans.PercentMark + `%</span>
                                                <span class="aCorrect">`+ statusSub + `</span>
                                            </span>
                                        </div>`;

                        });

                        html += `</div>
                                </div>`;
                    });

                    $('#questTableContent').html(html);

                    if (data.length == 0) {
                        $('#tableQuestion').hide();
                        $('.textEmpty').show();
                    } else {
                        $('.textEmpty').hide();
                        $('#tableQuestion').show();
                    }
                    historyController.pagination(response.totalRow, function () {
                        historyController.loadData();
                    }, changePageSize);
                    historyController.registerEvents();

                }
            }
        });
    },
    pagination: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / historyConfig.pageSize);

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
                historyConfig.pageIndex = page;
                setTimeout(callback, 0);
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
        let secon = newDate.getSeconds();
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
        if (secon < 10) {
            secon = '0' + secon;
        }

        newDate = dd + '/' + mm + '/' + yyyy + ' ' + hours + ':' + minutes + ':' + secon;
        return newDate;
    }
}

historyController.init();
