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