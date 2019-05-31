$(document).ready(function () {
    var scriptTag = $("#installmentsScript");
    var creditId = scriptTag.data("creditid");

    $("#creditInstallments").DataTable({
        ajax: {
            url: "/api/credits/installments/" + creditId,
            dataSrc: ""
        },
        columns: [
            {
                data: "installmentAmount",
                render: function (data) {
                    return data + " PLN";
                }
            },
            {
                data: "paidOn",
                render: function (data) {
                    if (data == null)
                        return "This installment hasn't been paid yet"
                    else {
                        return moment(data).format("Do MMMM YYYY");
                    }

                }
            },
            {
                data: "paymentDeadline",
                render: function (data) {
                    return moment(data).format("Do MMMM YYYY");
                }
            },
            {
                data: "id"
            }
        ],
        rowCallback: function (row, data) {
            if (data.paidOn == null)
                $(row).children().eq(3).html("<button type = 'button' class = 'btn btn-primary btn-block' id='payInstallment' data-installment-id = " + data.id + ">Pay</button>");
            else
                $(row).children().eq(3).html("No actions available");
        },
        ordering: false,
        searching: false
    });

    var callAjaxForInstallmentPaying = function (installmentDto) {
        $.ajax({
            url: "/api/credits/payinstallment",
            data: installmentDto,
            method: "POST",
            success: function (d) {
                Swal.fire({
                    title: "Good job!",
                    text: d,
                    type: "success"
                }).then(function () {
                    $.ajax({
                        url: "/api/credits/isfullypaid/" + creditId,
                        method: "GET",
                        success: function (d) {
                            Swal.fire({
                                title: "Congratulations",
                                text: d,
                                type: "success"
                            });
                        },
                        error: function () { }
                    });
                });
                var table = $("#creditInstallments").DataTable();
                table.ajax.reload();
            },
            error: function (d) {
                Swal.fire({
                    title: "Error",
                    text: d.responseJSON.message,
                    type: "error"
                });
            }
        });
    };

    $("#creditInstallments").on("click", "#payInstallment", function () {
        var button = $(this);

        var deadlineString = $(this).parent().prev().html();
        var deadline = moment(deadlineString, "Do MMMM YYYY");
        var now = moment();
        var days = moment.duration(deadline.diff(now)).days();

        var installmentDto = {
            id: button.attr("data-installment-id"),
            creditId: creditId
        };

        if (days < 0) {
            days = Math.abs(days);

            $.ajax({
                url: "/api/credits/getpenalty",
                data: installmentDto,
                method: "GET",
                success: function (d) {
                    Swal.fire({
                        title: "Warning",
                        type: "warning",
                        text: "Because you are " + days + " day(s) late, you have to pay " + d.data.penaltyPercentage + "% extra as a penalty - " + d.data.amount + " PLN in total.",
                        buttons: true,
                        dangerMode: true
                    }).then(function (clicked) {
                        if (clicked) {
                            callAjaxForInstallmentPaying(installmentDto);
                        }
                    });
                }
            });
        }
        else {
            callAjaxForInstallmentPaying(installmentDto);
        }
    });
});