var candidateConfig = {
    pageSize: $('.pageSizeItem').val(),
    pageIndex: 1
}

var candiController = {
    init: function () {
        candiController.loadData();
    },
    registerEvents: function () {
        $('.txtSearchCandidate').off('input').on('input', function () {
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
            candiController.deleteCandidate(id);
            $('#modal-delete').modal('hide');
        });
        $('.btnDetail').off('click').on('click', function (e) {
            e.preventDefault();
            candiController.getDetailCandidate($(this).data('id'));
            $('#modal-detailCadidate').modal('show');
        });

    },
    deleteCandidate: function (id) {
        $.ajax({
            url: '/Candidate/Locked',
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
    getDetailCandidate: function (id) {
        $.ajax({
            url: '/Candidate/Details',
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
                    toastr.error("Can not find Candidate", "Notification");
                }
            }
        });
    },
    loadData: function (changePageSize) {
        var keyword = $('.txtSearchCandidate').val();

        $.ajax({
            url: '/Candidate/LoadData',
            type: 'GET',
            data: {
                keyword: keyword,
                page: candidateConfig.pageIndex,
                pageSize: candidateConfig.pageSize
            },
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    var data = response.data;
                    var html = '';
                    $.each(data, function (i, item) {
                        html +=
                            `<tr class="intro-x">
                                <td scope="col">${i+1}</td>
                                <td><img src="${item.Image}" style="width:30px" alt="" /></td>
                                <td>${item.UserName}</td>
                                <td>${item.Name}</td>
                                <td>${item.Email}</td>
                                <td class="FlexIconAction">
                                        <a class="flex items-center text-theme-6 btnDetail" href="#" data-id="${item.Id}"> <i class="fas fa-search-plus"></i> Details </a>
                                        <a class="flex items-center mr-3" href="/Admin/Candidate/InsertOrUpdate/${item.Id}"> <i class="fas fa-edit"></i> Edit </a>
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