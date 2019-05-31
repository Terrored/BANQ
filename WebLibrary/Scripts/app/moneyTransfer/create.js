$(document).ready(function () {
    $("button#submitTransfer").on("click", function () {
        var dataForm = {
            cashAmount: $('#cashAmount').val(),
            toId: $('#accountNumber').val()
        };
        $.ajax({
            type: "POST",
            url: "/MoneyTransfer/Transfer",
            data: dataForm
        }).done(function (data) {
            if (data.message == 'Transfer has been successful') {
                Swal.fire('Great!', data.message, 'success');
            } else {
                Swal.fire('Oops!', data.message, 'error');
            }

        });
    });
});