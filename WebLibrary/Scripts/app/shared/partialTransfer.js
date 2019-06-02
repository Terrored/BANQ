$(document).ready(function () {
    $("#transferForm").validate({
        rules: {
            name: {
                required: true,
                minlength: 4
            },
            cashAmount: {
                required: true,
                min: 0 + Number.MIN_VALUE

            },
            accountNumber: "required"
        },
        messages: {
            name: "You have to provide at least 4 characters!",
            cashAmount: "Provide valid amount!",
            accountNumber: "Provide valid account number!"
        },
        tooltip_options: {

            name: { placement: 'right' },
            cashAmount: { placement: 'right' },
            accountNumber: { placement: 'right' },
        },
        submitHandler: function (form) {
            var dto = {
                name: $('#name').val(),
                cashAmount: $('#cashAmount').val(),
                to:
                {
                    id: $('#accountNumber').val()
                },
                from: {}
            };
            $.ajax({
                type: "POST",
                url: "/api/MoneyTransfer/Transfer",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(dto),
                dataType: "json"
            }).done(function (data) {
                Swal.fire('Great!', data, 'success');

                var table = $("#lastTransfers").DataTable();
                $("#transferForm")[0].reset();
                table.ajax.reload();

            }).fail(function (data) {
                Swal.fire('Oops!', data.responseJSON.message, 'error');
            });
        }
    });

});