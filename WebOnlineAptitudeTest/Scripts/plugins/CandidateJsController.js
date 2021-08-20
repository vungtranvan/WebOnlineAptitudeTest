var candidateConfig = {
    pageSize: $('.pageSizeItem').val(),
    pageIndex: 1
}

var cadiController = {
    init: function () {
        cadiController.loadData();
    },
    registerEvents: function () {
        $('.txtSearchCandidate').off('input').on('input', function () {
            cadiController.loadData(true);
        });

        $('.pageSizeItem').off('change').on('change', function () {
            candidateConfig.pageSize = $(this).val();
            cadiController.loadData(true);
        });

        $('.btnDelete').off('click').on('click', function (e) {
            e.preventDefault = false;
            $('#id-item').text($(this).data('id'));
            $('#modal-delete').modal('show');
        });

        $('.Save-Delete').off('click').on('click', function (e) {
            var id = $('#id-item').text();
            cadiController.deleteCandidate(id);
            $('#modal-delete').modal('hide');
        });

    },
    deleteCandidate: function (id) {
        $.ajax({
            url: '/Candidate/Delete',
            type: 'POST',
            data: { id: id },
            dataType: 'json',
            success: function (response) {
                if (response.status == true) {
                    cadiController.loadData(true);
                    toastr.success(response.message, response.title);
                } else {
                    toastr.error(response.message, response.title);
                }
            }
        });
    },
    getDetailPro: function (id) {
        //$.ajax({
        //    url: '/Candidate/GetById',
        //    type: 'GET',
        //    data: { id: id },
        //    dataType: 'json',
        //    success: function (response) {
        //        if (response.status == true) {
        //            var data = response.data;
        //            $('#Id').val(data.Id);
        //            $('#Name').val(data.Name);
        //            $('#Price').val(data.Price);
        //            $('#CategoryId').val(data.CategoryId).attr("selected", "selected");
        //            if (data.Image == '' || data.Image == null) {
        //                $('#Click-Image').attr("src", "/Content/default-image.jpg");
        //            } else {
        //                $('#Click-Image').attr("src", '/userfiles/images/' + data.Image);
        //            }

        //            $('#Image').val(data.Image);
        //            $('#Dcription').val(data.Dcription);
        //            if (data.Status == true) {
        //                $('.Status label input:first').prop("checked", true);
        //            } else {
        //                $('.Status label input:last').prop("checked", true);
        //            }

        //        }
        //    }
        //});
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
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            STT: i + 1,
                            Id: item.Id,
                            UserName: item.UserName,
                            Name: item.Name,
                            Email: item.Email,
                            Birthday: item.Birthday == null ? "" : cadiController.formatDate(item.Birthday),
                            Address: item.Address,
                            Phone: item.Phone,
                            Sex: item.Sex == true ? 'Male' : 'FeMale',
                            CreatedDate: item.CreatedDate == null ? "" : cadiController.formatDate((item.CreatedDate)),
                            UpdatedDate: item.UpdatedDate == null ? "" : cadiController.formatDate(item.UpdatedDate)
                        });
                    });
                    $('#tblDataCandidate').html(html);
                    if (response.data.length == 0) {
                        $('#tableCandidate').hide();
                        $('.textEmpty').show();
                    } else {
                        $('.textEmpty').hide();
                        $('#tableCandidate').show();
                    }
                    cadiController.pagination(response.totalRow, function () {
                        cadiController.loadData();
                    }, changePageSize);
                    cadiController.registerEvents();

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

        //Unbind pagination if it existed or click change pageSize
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

cadiController.init();