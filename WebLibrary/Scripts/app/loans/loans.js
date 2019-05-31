$(document).ready(function () {
    $('#loans').DataTable({
        "ajax": { url: "/api/Loan/GetLoans", dataSrc: "" },
        "paging": false,
        "ordering": false,
        "info": false,
        "searching": false,
        "columns": [
            {
                "data": "loanAmount",
                "render": function (data, type, row, meta) {
                    return data.toFixed(2).toLocaleString() + " PLN";
                }
            },
            {
                "data": "loanAmountLeft",
                "render": function (data, type, row, meta) {
                    return data.toFixed(2).toLocaleString() + " PLN";
                }
            },
            {
                "data": "installmentAmount",
                "render": function (data, type, row, meta) {
                    return data.toFixed(2).toLocaleString() + " PLN";
                }
            },
            { "data": "installmentsLeft" },
            {
                "data": "nextInstallmentDate", "render": function (data, type, row, meta) {

                    return data.substring(0, 10);
                }
            },
            {
                "data": "id",
                "render": function (data, type, row, meta) {
                    if (row.installmentsLeft == 0) {
                        return null;
                    }
                    return "<button type='button' class='btn btn-primary btn-block payInstallment' data-toggle='tooltip' data-placement='right' value='" + data + "'>Pay!</button>";
                }
            }

        ],
        "rowCallback": function (row, data, index) {
            var date = new Date().format('Y-m-d');
            if (data.installmentsLeft == 0) {
                $('td', row).css('background-color', '#006b24');
            } else if (data.nextInstallmentDate < date) {
                $('td', row).css('background-color', '#a01930');
                $('button', row).attr('title', "You're late!");
                $('button', row).attr('data-late', 'true');
                $(function () {
                    $('[data-toggle="tooltip"]').tooltip();
                });

            }

        }
    });


    $("#loans tbody").on("click", ".payInstallment", function (data) {

        var titleText = "Are you sure?";
        if (data.currentTarget.dataset.late === "true") {
            titleText = "You have to pay 10% extra as a penalty because you are late!";
        }
        swal.fire({
            title: titleText,
            text: "Are you sure that you want to pay installment?",
            type: "warning",
            showCancelButton: true,
            confirmButtonText: "Pay installment!",
            cancelButtonText: "No, cancel!"

        }).then((result) => {
            if (result.value) {
                var loanId = $(this).attr('value');
                $.ajax({
                    type: "POST",
                    url: "/api/Loan/PayInstallment",
                    data: {
                        loanId: loanId
                    }
                }).done(function (data) {
                    swal.fire('Success!', data.message, 'success');
                    var table = $("#loans").DataTable();
                    table.ajax.reload();
                }).fail(function (data) {

                    swal.fire('Oops!', data.responseJSON.message, 'error');

                });

            }
        })



        return false;
    });

});