// Modal Login
$(document).ready(function () {

    $('.btn-login').click(function (e) {
        e.preventDefault();

        var $modal = $('#loginModal');
        var $modalDialog = $('.modal-dialog');
        var href = $(this).prop('href');

        // không cho phép tắt modal khi click bên ngoài modal
        var option = { backdrop: true };

        // load modal
        $modalDialog.load(href, function () {
            $modal.modal(option, 'show');
        });
    });

});

// Modal Register
$(document).ready(function () {

    $('.btn-register').click(function (e) {
        e.preventDefault();

        var $modal = $('#registerModal');
        var $modalDialog = $('.modal-dialog');
        var href = $(this).prop('href');

        // không cho phép tắt modal khi click bên ngoài modal
        var option = { backdrop: true };

        // load modal
        $modalDialog.load(href, function () {
            $modal.modal(option, 'show');
        });
    });

});

// Modal Minicart
$(document).ready(function () {

    $('.minicart-btn').click(function (e) {
        e.preventDefault();

        var $modal = $('#miniCartModal');
        var $modalDialog = $('.modal-dialog');
        var href = $(this).prop('href');

        // không cho phép tắt modal khi click bên ngoài modal
        var option = { backdrop: true };

        // load modal
        $modalDialog.load(href, function () {
            $modal.modal(option, 'show');
        });
    });

});
// Modal ChangePass
$(document).ready(function () {

    $('.btn-changePass').click(function (e) {
        e.preventDefault();

        var $modal = $('#changePassModal');
        var $modalDialog = $('.modal-dialog');
        var href = $(this).prop('href');

        // không cho phép tắt modal khi click bên ngoài modal
        var option = { backdrop: true };

        // load modal
        $modalDialog.load(href, function () {
            $modal.modal(option, 'show');
        });
    });

});
// Modal Info
$(document).ready(function () {

    $('.btn-information').click(function (e) {
        e.preventDefault();

        var $modal = $('#infoModal');
        var $modalDialog = $('.modal-dialog');
        var href = $(this).prop('href');

        // không cho phép tắt modal khi click bên ngoài modal
        var option = { backdrop: true };

        // load modal
        $modalDialog.load(href, function () {
            $modal.modal(option, 'show');
        });
    });

});