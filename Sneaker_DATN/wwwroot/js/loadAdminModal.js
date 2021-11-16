// Modal Crete Size
$(document).ready(function () {

    $('.btn-create-size').click(function (e) {
        e.preventDefault();

        var $modal = $('#createSizeModal');
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

// Modal Edit Size
$(document).ready(function () {

    $('.btn-edit-size').click(function (e) {
        e.preventDefault();

        var $modal = $('#editSizeModal');
        var $modalDialog = $('.modal-dialog');
        var href = $(this).prop('href');
        var id = $(this).parent().find('.id').val();
        $.ajax({
            type: 'GET',
            url: 'Sizes/Edit/' + id,
            success: function (size) {
                $('#editSizeModal #id');
            }
        });
        // không cho phép tắt modal khi click bên ngoài modal
        var option = { backdrop: true };

        // load modal
        $modalDialog.load(href, function () {
            $modal.modal(option, 'show');
        });
    });

});

// Modal Delete Size
$(document).ready(function () {

    $('.btn-delete-size').click(function (e) {
        e.preventDefault();

        var $modal = $('#deleteSizeModal');
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

// Modal Crete Color
$(document).ready(function () {

    $('.btn-create-color').click(function (e) {
        e.preventDefault();

        var $modal = $('#createColorModal');
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

// Modal Edit Color
$(document).ready(function () {

    $('.btn-edit-color').click(function (e) {
        e.preventDefault();

        var $modal = $('#editColorModal');
        var $modalDialog = $('.modal-dialog');
        var href = $(this).prop('href');
        var id = $(this).parent().find('.id').val();
        $.ajax({
            type: 'GET',
            url: 'Colors/Edit/' + id,
            success: function (color) {
                $('#editColorModal #id');
            }
        });
        // không cho phép tắt modal khi click bên ngoài modal
        var option = { backdrop: true };

        // load modal
        $modalDialog.load(href, function () {
            $modal.modal(option, 'show');
        });
    });

});

// Modal Delete Color
$(document).ready(function () {

    $('.btn-delete-color').click(function (e) {
        e.preventDefault();

        var $modal = $('#deleteColorModal');
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

// Modal Edit Order
$(document).ready(function () {

    $('.btn-edit-order').click(function (e) {
        e.preventDefault();

        var $modal = $('#editOrderModal');
        var $modalDialog = $('.modal-dialog');
        var href = $(this).prop('href');
        var id = $(this).parent().find('.id').val();
        $.ajax({
            type: 'GET',
            url: 'Orders/Edit/' + id,
            success: function (color) {
                $('#editOrderModal #id');
            }
        });
        // không cho phép tắt modal khi click bên ngoài modal
        var option = { backdrop: true };

        // load modal
        $modalDialog.load(href, function () {
            $modal.modal(option, 'show');
        });
    });

});

// Modal Details Order
$(document).ready(function () {

    $('.btn-details-order').click(function (e) {
        e.preventDefault();

        var $modal = $('#detailsOrderModal');
        var $modalDialog = $('.modal-dialog');
        var href = $(this).prop('href');
        var id = $(this).parent().find('.id').val();
        $.ajax({
            type: 'GET',
            url: 'OrderDetails/Details/' + id,
            success: function (order) {
                $('#detailsOrderModal #id');
            }
        });

        // không cho phép tắt modal khi click bên ngoài modal
        var option = { backdrop: true };

        // load modal
        $modalDialog.load(href, function () {
            $modal.modal(option, 'show');
        });
    });

});



// Modal Crete Discount
$(document).ready(function () {

    $('.btn-create-discount').click(function (e) {
        e.preventDefault();

        var $modal = $('#createDiscountModal');
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

// Modal Edit Discount
$(document).ready(function () {

    $('.btn-edit-discount').click(function (e) {
        e.preventDefault();

        var $modal = $('#editDiscountModal');
        var $modalDialog = $('.modal-dialog');
        var href = $(this).prop('href');
        var id = $(this).parent().find('.id').val();
        $.ajax({
            type: 'GET',
            url: 'Colors/Edit/' + id,
            success: function (color) {
                $('#editColorModal #id');
            }
        });
        // không cho phép tắt modal khi click bên ngoài modal
        var option = { backdrop: true };

        // load modal
        $modalDialog.load(href, function () {
            $modal.modal(option, 'show');
        });
    });

});

// Modal Details Discount
$(document).ready(function () {

    $('.btn-details-discount').click(function (e) {
        e.preventDefault();

        var $modal = $('#detailsDiscountModal');
        var $modalDialog = $('.modal-dialog');
        var href = $(this).prop('href');
        var id = $(this).parent().find('.id').val();
        $.ajax({
            type: 'GET',
            url: 'Discount/Details/' + id,
            success: function (order) {
                $('#detailsDiscountModal #id');
            }
        });

        // không cho phép tắt modal khi click bên ngoài modal
        var option = { backdrop: true };

        // load modal
        $modalDialog.load(href, function () {
            $modal.modal(option, 'show');
        });
    });

});

// Modal Delete Discount
//$(document).ready(function () {

//    $('.btn-delete-color').click(function (e) {
//        e.preventDefault();

//        var $modal = $('#deleteColorModal');
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
