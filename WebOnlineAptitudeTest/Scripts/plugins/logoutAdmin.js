$(document).ready(function () {
    $('.btn-Logout').off('click').on('click', function (e) {
        e.preventDefault();
        $('#modal-logout').modal('show');
    });
});