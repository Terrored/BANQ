$(document).ready(function () {
    $("#credits").on("click", ".details", function () {
        var creditId = $(this).attr("data-credit-id");
        var paidString = $(this).attr("data-fully-repaid").toLowerCase();
        var redirect = (paidString == "false");

        if (redirect) {
            window.location.href = "/credits/installmentspage/" + creditId;
        }
        else {
            $.ajax({
                url: "/api/credits/getcreditinfo/" + creditId,
                method: "GET",
                success: function (d) {
                    Swal.fire({
                        title: "Your credit info",
                        type: "info",
                        html: "Credit amount: <strong>" + d.data.creditAmount + "</strong> PLN with <strong>" + d.data.percentageRate + "</strong>% rate." + "Payment was spread into <strong>" + d.data.installmentCount + "</strong> installments."
                    });
                },
                error: function (d) {
                    Swal.fire({
                        title: "Error",
                        type: "error",
                        text: d.responseJSON.message
                    });
                }
            });
        }

    });
});