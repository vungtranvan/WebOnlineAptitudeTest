var candidateConfig = {
    pageSize: $('.pageSizeItem').val(),
    pageIndex: 1
}

var candiController = {
    init: function () {
        candiController.loadData();
    },
    registerEvents: function () {
        $('.txtSearchQuestion').off('input').on('input', function () {
            candiController.loadData(true);
        });

        $('.pageSizeItem').off('change').on('change', function () {
            candidateConfig.pageSize = $(this).val();
            candiController.loadData(true);
        });

        $('.btnDelete').off('click').on('click', function (e) {
            e.preventDefault();
            $('#id-item').text($(this).data('id'));
            $('#modal-delete').modal('show');
        });

        $('.Save-Delete').off('click').on('click', function (e) {
            e.preventDefault();
            var id = $('#id-item').text();
            candiController.deleteQuestion(id);
            $('#modal-delete').modal('hide');
        });
        $('.btnDetail').off('click').on('click', function (e) {
            e.preventDefault();
            candiController.getDetailQuestion($(this).data('id'));
            $('#modal-detailCadidate').modal('show');
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
                    candiController.loadData(true);
                    toastr.success(response.message, response.title);
                } else {
                    toastr.error(response.message, response.title);
                }
            }
        });
    },
    getDetailQuestion: function (id) {
        $.ajax({
            url: '/Question/Details',
            type: 'GET',
            data: { id: id },
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    var data = response.data;

                    $('#textUserName').text(data.UserName);
                    if (data.Image != '' || data.Image != null) {
                        $('#textImage').attr("src", data.Image);
                    }

                    $('#textDisplayName').html(data.Name);
                    $('#textEmail').html(data.Email);
                    $('#textBirthday').html(data.Birthday == null ? `&emsp;` : candiController.formatDate(data.Birthday));
                    $('#textAddress').html(data.Address == null ? `&emsp;` : data.Address);
                    $('#textPhone').html(data.Phone == null ? `&emsp;` : data.Phone);
                    $('#textEducation').html(data.Education == null ? `&emsp;` : data.Education);
                    $('#textWorkExperience').html(data.WorkExperience == null ? `&emsp;` : data.WorkExperience);
                    $('#textSex').html(data.Sex == true ? 'Male' : 'FeMale');
                    $('#textCreatedDate').html(data.CreatedDate == null ? `&emsp;` : candiController.formatDate(data.CreatedDate));
                    $('#textLastUpdatedDate').html(data.UpdatedDate == null ? `&emsp;` : candiController.formatDate(data.UpdatedDate));

                    //$('#textSex').val(data.CategoryId).attr("selected", "selected");
                    //if (data.Status == true) {
                    //    $('.Status label input:first').prop("checked", true);
                    //} else {
                    //    $('.Status label input:last').prop("checked", true);
                    //}

                } else {
                    $('#modal-detailCadidate').modal('hide');
                    toastr.error("Can not find Question", "Notification");
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
                page: candidateConfig.pageIndex,
                pageSize: candidateConfig.pageSize
            },
            dataType: 'json',
            success: function (response) {
                console.log(response.data);
                if (response.status == true) {
                    console.log(response.data);
                    var data = response.data;
                    var html = '';
                    $.each(data, function (i, item) {
                        html +=
                            ` <div class="card mb-0">
                                    <div class="card-header" id="headingOne">

                                        <div class="custom-accordion-title d-block">
                                            <span class="d-flex listQuest">
                                                <a class="qIcon collapsed" href="#collapseOne" data-toggle="collapse" aria-expanded="false" aria-controls="collapseOne"> </a>
                                                <span class="qId"> `+ item.Id + `</span>
                                                <span class="qContent">`+ item.Name + `</span>
                                                <span class="qCategory">`+ item.CategoryExamName +`</span>
                                                <span class="qMark">5 </span>
                                                <span class="qStatus">true</span>
                                                <span class="qDeleted">true</span>
                                                <a class="qEdit">Edit</a>

                                            </span>
                                        </div>

                                    </div>
                                    <div id="collapseOne" class="collapse" aria-labelledby="headingOne" data-parent="#accordion">
                                        <div class="card-body">
                                            <span class="d-flex answer-title">
                                                <span class="aId"> ID</span>
                                                <span class="aCorrect">Correct</span>
                                                <span class="aContent">Content</span>

                                            </span>
                                        </div>
                                        <div class="card-body">
                                            <span class="d-flex listQuest">
                                                <span class="aId"> 1</span>
                                                <span class="aCorrect">true</span>
                                                <span class="aContent">Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt.Cras dapibus. Vivamus elementum semper nisi</span>
                                            </span>
                                        </div>
    

                                    </div>
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
                    candiController.pagination(response.totalRow, function () {
                        candiController.loadData();
                    }, changePageSize);
                    candiController.registerEvents();

                }
            }
        });
    },
    formatDate: function (dateString) {
        var newDate = new Date(parseInt(dateString.replace('/Date(', '')));
        var dd = newDate.getDate();
        var mm = newDate.getMonth() + 1;
        var yyyy = newDate.getFullYear();
        if (dd < 10) {
            dd = '0' + dd;
        }
        if (mm < 10) {
            mm = '0' + mm;
        }
        newDate = mm + '/' + dd + '/' + yyyy;
        return newDate;
    },
    pagination: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / candidateConfig.pageSize);

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
                candidateConfig.pageIndex = page;
                setTimeout(callback, 0);
            }
        });
    }
}

candiController.init();