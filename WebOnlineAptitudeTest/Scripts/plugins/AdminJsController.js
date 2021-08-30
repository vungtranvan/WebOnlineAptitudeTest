$(function () {
    LoadData()
})

var pageSize = $('.pageSizeItem').val();
var pageIndex = 1;

$(".txtSearchAdmin").off('input').on('input', function () {
    LoadData(true)
});

$(".pageSizeItem").off('change').on('change', function () {
    pageSize = $(this).val();
    LoadData(true)
})

function LoadData(changePageSize) {
    var keysearch = $(".txtSearchAdmin").val()

    $.ajax({
        type: "GET",
        url: "/Admin/Account/LoadData",
        data: { keyword: keysearch, page: pageIndex, pageSize: pageSize },
        success: function (res) {

            if (res.status == true) {
                var data = res.data
                var html = ''
                $.each(data, function (key, item) {
                    html += '<tr>'
                    html += '<td scope="col">' + (key + 1) + '</td>'
                    html += '<td> <img src="' + item.Image + '" width="30" alt="" /></td>'
                    html += '<td>' + item.UserName + '</td>'
                    html += '<td>' + item.Email + '</td>'
                    html += '<td class="FlexIconAction">'
                    html += '<a class="flex items-center text-theme-6 btnDetail" href="javascript:void(0)" onclick="GetDetail(' + item.Id + ')"> <i class="fas fa-search-plus"></i> Details </a>'
                    html += '<a class="flex items-center mr-3" href="/Admin/Account/InsertOrUpdate/' + item.Id + '"> <i class="fas fa-edit"></i> Edit </a>'
                    html += '<a class="flex items-center text-theme-6 btnDelete" href="#" onclick="GetDelete(' + item.Id + ')"> <i class="fas fa-trash-alt"></i> Delete </a>'
                    html += '</td>'
                    html += '</tr>'
                });

                $("#tblDataAdmin").html(html)

                Paginate(res.totalRow, function () {
                    LoadData();
                }, changePageSize);
            }
        }
    })
}

function Paginate(totalRow, callback, changePageSize) {
    var totalPage = Math.ceil(totalRow / pageSize);

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
            pageIndex = page;
            setTimeout(callback, 0);
        }
    });
}

function GetDelete(id) {
    $('#modal-delete').modal('show');
    $(document).on("click", ".Save-Delete", function () {
        console.log("oke")
        $("#modal-delete").modal("hide")
        $.ajax({
            type: "GET",
            url: "/Admin/Account/Locked/" + id,
            success: function (res) {
                if (res.status == true) {
                    LoadData(true)
                    toastr.success(res.message, res.title);
                }
            }
        })
    })
}

function GetDetail(id) {

    $.ajax({
        type: "GET",
        url: "/Admin/Account/Details/" + id,
        success: function (res) {
            if (res.status == true) {
                var item = res.data
                $("#textUserName").html(item.UserName)
                $("#textDisplayName").html(item.DisplayName)
                $("#textEmail").html(item.Email)
                if (item.Sex == true) {
                    $("#textSex").html("Male")
                } else {
                    $("#textSex").html("Female")
                }
                $("#textImage").attr("src", item.Image)
                $("#textCreatedDate").html(res.createdDate)
                $("#textLastUpdatedDate").html(res.updatedDate)
                $("#modal-detailAccount").modal('show')
            }
        }
    })

}