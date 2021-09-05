
var questionConfig = {
    pageSize: $('.pageSizeItem').val(),
    pageIndex: 1,
    idCate: ($('select#DropDownLists option:selected').val() == '' ? 0 : $('select#DropDownLists option:selected').val())
}

var questController = {
    init: function () {
        questController.loadData();
    },
    registerEvents: function () {
        $('[data-toggle="tooltip"]').tooltip();
        $('.txtSearchQuestion').off('input').on('input', function () {
            questController.loadData(true);
        });

        $('.pageSizeItem').off('change').on('change', function () {
            questionConfig.pageSize = $(this).val();
            questController.loadData(true);
        });

        $('select#DropDownLists').off('change').on('change', function () {
            questionConfig.idCate = ($('select#DropDownLists option:selected').val() == '' ? 0 : $('select#DropDownLists option:selected').val());
            questController.loadData(true);
        });

        $('.btnDelete').off('click').on('click', function (e) {
            e.preventDefault();
            $('#id-item').text($(this).data('id'));
            $('#modal-delete').modal('show');
        });

        $('.Save-Delete').off('click').on('click', function (e) {
            e.preventDefault();
            var id = $('#id-item').text();
            questController.deleteQuestion(id);
            $('#modal-delete').modal('hide');
        });

    },
    deleteQuestion: function (id) {
        $.ajax({
            url: '/Question/Locked',
            type: 'POST',
            data: { id: id },
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    questController.loadData(true);
                    toastr.success(response.message, response.title);
                } else {
                    toastr.error(response.message, response.title);
                }
            }
        });
    },

    loadData: function (changePageSize) {
        var keyword = $('.txtSearchQuestion').val();

        $.ajax({
            url: '/Question/LoadData',
            type: 'GET',
            data: {
                keyword: keyword,
                idCate: questionConfig.idCate,
                page: questionConfig.pageIndex,
                pageSize: questionConfig.pageSize
            },
            dataType: 'json',
            success: function (response) {

                if (response.status == true) {

                    var data = JSON.parse(response.data);
                    
                    var html = '';
                    $.each(data, function (i, item) {
                        var status = `<i class="fa fa-circle text-success font-12" data-toggle="tooltip" title="Show"></i>`;
                        if (Boolean(item.Status) == false) {
                            status = `<i class="fa fa-circle text-danger font-12" data-toggle="tooltip" title="Hide"></i>`;
                        }
                        
                        html +=
                            ` <div class="card mb-0">
                                    <div class="card-header" id="headingOne">

                                        <div class="custom-accordion-title d-block">
                                            <span class="d-flex listQuest">
                                                <a class="qIcon collapsed" href="#collapse_`+ item.Id + `" data-toggle="collapse" aria-expanded="false" aria-controls="collapse_` + item.Id + `"> </a>
                                                <span class="qId"> `+ (i + 1) + `</span>
                                                <span class="qContent">`+ item.Name + `</span>
                                                <span class="qCategory">`+ item.CategoryExamName + `</span>
                                                <span class="qMark">`+ item.Mark + `</span>
                                                <span class="qStatus">`+ status + `</span>
                                                <span class="qAction">
                                                <a class="qEdit" href="Question/InsertOrUpdate/`+ item.Id + `" >Edit</a>
                                                <a class="qDelete btnDelete" href=":javascript" data-id=`+ item.Id + `  >Delete</a>

                                                </span>


                                                
                                            </span>
                                        </div>

                                    </div>
                                    <div id="collapse_`+ item.Id + `" class="collapse" aria-labelledby="headingOne" data-parent="#accordion">
                                        <div class="card-body">
                                            <span class="d-flex answer-title">
                                                <span class="aId"> ID</span>
                                                <span class="aCorrect">Correct</span>
                                                <span class="aContent">Content</span>

                                            </span>
                                        </div>`;

                        $.each(item.Answers, function (j, ans) {
                            html += ` <div class="card-body">
                                            <span class="d-flex listQuest">
                                                <span class="aId"> `+ (j + 1) + `</span>
                                                <span class="aCorrect">`+ ans.Correct + `</span>
                                                <span class="aContent">`+ ans.Name + `</span>
                                            </span>
                                        </div>`;

                        });

                        html += `</div>
                                </div>`;
                    });

                    $('#questTableContent').html(html);

                    if (response.data.length == 0) {
                        $('#tableQuestion').hide();
                        $('.textEmpty').show();
                    } else {
                        $('.textEmpty').hide();
                        $('#tableQuestion').show();
                    }
                    questController.pagination(response.totalRow, function () {
                        questController.loadData();
                    }, changePageSize);
                    questController.registerEvents();

                }
            }
        });
    },
    pagination: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / questionConfig.pageSize);

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
                questionConfig.pageIndex = page;
                setTimeout(callback, 0);
            }
        });
    }
}

questController.init();