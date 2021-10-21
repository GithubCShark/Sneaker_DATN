$(document).ready(function () {

    $('.btn-login').click(function (e) {
        e.preventDefault();

        var $modal = $('#loginModal');
        var $modalDialog = $('.modal-dialog');
        var href = $(this).prop('href');

        // không cho phép tắt modal khi click bên ngoài modal
        var option = { backdrop: true };

        // kiểm tra (logic, điều kiện...)

        // load modal
        $modalDialog.load(href, function () {
            $modal.modal(option, 'show');
        });
    });

}); // document ready

$(document).ready(function () {

    $('.btn-register').click(function (e) {
        e.preventDefault();

        var $modal = $('#registerModal');
        var $modalDialog = $('.modal-dialog');
        var href = $(this).prop('href');

        // không cho phép tắt modal khi click bên ngoài modal
        var option = { backdrop: true };

        // kiểm tra (logic, điều kiện...)

        // load modal
        $modalDialog.load(href, function () {
            $modal.modal(option, 'show');
        });
    });

}); // document ready