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
//$(document).ready(function () {

//    $('.btn-changePass').click(function (e) {
//        e.preventDefault();

//        var $modal = $('#changePassModal');
//        var $modalDialog = $('.modal-dialog');
//        var href = $(this).prop('href');

//        // không cho phép tắt modal khi click bên ngoài modal
//        var option = { backdrop: true };

//        // load modal
//        $modalDialog.load(href, function () {
//            $modal.modal(option, 'show');
//        });
//    });

//});
// Modal Info
//$(document).ready(function () {

//    $('.btn-information').click(function (e) {
//        e.preventDefault();

//        var $modal = $('#infoModal');
//        var $modalDialog = $('.modal-dialog');
//        var href = $(this).prop('href');

//        // không cho phép tắt modal khi click bên ngoài modal
//        var option = { backdrop: true };

//        // load modal
//        $modalDialog.load(href, function () {
//            $modal.modal(option, 'show');
//        });
//    });

//});

$(document).ready(function () {
    $('.btnLogin').click(function (e) {
        setTimeout(function () {
            $.ajax({
                type: "POST",
                url: '/Home/Login',
                success: function (result) {
                    Swal.fire({
                        position: 'center',
                        icon: 'success',
                        title: 'Đăng nhập thành công!',
                        showConfirmButton: false,
                        timer: 1000
                    })
                },
                error: function (e) {
                    Swal.fire({
                        position: 'center',
                        icon: 'error',
                        title: 'Đăng nhập thất bại!',
                        showConfirmButton: true,
                        timer: 1000
                    })
                }
            })
        }, 1000);
    })
});