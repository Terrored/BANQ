$(document).ready(function () {

    $("#loanForm").validate({
        rules: {
            loanAmount: {
                required: true,
                min: 500 + Number.MIN_VALUE,
                max: 10000 + Number.MIN_VALUE
            }
        },
        messages: {
            loanAmount: "Min loan amount is 500 PLN and max is 10000 PLN!"
        },
        tooltip_options: {

            loanAmount: { placement: 'right' }

        },
        submitHandler: function (form) {
            var dataForm = {
                loanAmount: $('#loanAmount').val(),
                installmentAmount: $('#installmentAmount').val(),
                totalInstallments: $('#totalInstallments').val(),
                loanAmountLeft: ($('#totalInstallments').val() * $('#installmentAmount').val()).toFixed(2)
            };
            console.log(dataForm);
            $.ajax({
                type: "POST",
                url: "/api/Loan/TakeLoan",
                data: dataForm
            }).done(function (data) {
                console.log(data);
                Swal.fire('Great!', data.message, 'success');
                var table = $("#loans").DataTable();
                table.ajax.reload();
            }).fail(function (data) {

                swal.fire('Oops!', data.responseJSON.message, 'error');

            });




        }
    });



    $("#loanForm").change(function () {
        calculate();
        var installmentsNumber = $("#totalInstallments").val();
        $("#installmentInfo").empty();
        $("#installmentInfo").append(installmentsNumber + " installment(s)");
    });


    function calculate() {

        var loanAmount = $("#loanAmount").val();
        var totalInstallments = $("#totalInstallments").val();

        var inst = 0;

        for (var i = 1; i <= totalInstallments; i++) {

            inst = inst + (1 / Math.pow((1 + 0.10 / 2), i));
        }
        var installmentAmount = loanAmount / inst;
        $("#installmentAmount").val(installmentAmount.toFixed(2));

    }


});