var candidateConfig = {
    pageSize: 1,
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

        $('.btnDelete').off('click').on('click', function () {

        });
    },
    deleteProduct: function (id) {
        //$.ajax({
        //    url: '/Candidate/Delete',
        //    type: 'POST',
        //    data: { id: id },
        //    dataType: 'json',
        //    success: function (response) {
        //        if (response.status == true) {
        //            toastr.error(response.message, response.title);
        //        } else {
        //            toastr.error(response.message, response.title);
        //        }
        //    }
        //});
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
                            Birthday: item.Birthday,
                            Address: item.Address,
                            Phone: item.Phone,
                            Sex: item.Sex == true ? 'Male' : 'FeMale',
                            CreatedDate: item.CreatedDate,
                            UpdatedDate: item.UpdatedDate
                        });
                    });
                    $('#tblDataCandidate').html(html);

                    cadiController.pagination(response.totalRow, function () {
                        cadiController.loadData();
                    }, changePageSize);
                    cadiController.registerEvents();
                }
            }
        });
    },
    pagination: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / candidateConfig.pageSize);

        //Unbind pagination if it existed or click change pageSize
        if ($('#pagination').length === 0 || changePageSize === true) {
            $('#pagination').empty();
            $('#pagination').removeData('twbs-pagination');
            $('#pagination').unbind("page");
        }
        $('#pagination li a').addClass('pagination__link');
        $('#pagination li a.active').addClass('pagination__link--active');

        $('#pagination').twbsPagination({
            totalPages: totalPage,
            first: `<i class="w-4 h-4" data-feather="chevrons-left"></i>`,
            prev: '<i class="w-4 h-4" data-feather="chevron-left"></i>',
            next: '<i class="w-4 h-4" data-feather="chevron-right"></i>',
            last: '<i class="w-4 h-4" data-feather="chevrons-right"></i>',
            visiblePages: 8,
            onPageClick: function (event, page) {
                candidateConfig.pageIndex = page;
                setTimeout(callback, 0);
            }
        });
        
    }
}

cadiController.init();