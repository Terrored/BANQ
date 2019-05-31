$(document).ready(function () {

    $("#obtainCreditForm").on("input", function () {

        var creditDto = {
            creditAmount: $("#formCreditAmount").val()
        };

        $.ajax({
            url: "/api/credits/getpercentagerate",
            method: "GET",
            data: creditDto,
            success: function (d) {
                $("#displayPercentageRate").html(d.data + " %");
            },
            error: function (d) {
                $("#displayPercentageRate").html(d.responseJSON.message);
            }
        });
    });

    $.validator.addMethod("regex", function (value, element, regexpr) {
        return this.optional(element) || value.match(typeof regexpr == "string" ? new RegExp(regexpr) : regexpr);
    }, "Please provide a valid input");

    $("#obtainCreditForm").validate({
        rules: {
            formCreditAmount: {
                required: true,
                regex: /^\d*[.,]?\d{0,2}$/
            }
        },
        messages: {
            formCreditAmount: {
                required: "This field is required",
                regex: "This field must be a real positive number"
            }
        },
        submitHandler: function () {
            var creditDto = {
                creditAmount: $("#formCreditAmount").val(),
                installmentCount: $("#formPeriodInput").val() * 12
            };

            $.ajax({
                method: "POST",
                url: "/api/credits/obtaincredit",
                data: creditDto,
                success: function (data) {
                    Swal.fire({
                        title: "Success",
                        text: data,
                        type: "success",
                        button: "OK"
                    });
                },
                error: function (data) {
                    Swal.fire({
                        title: "Error",
                        text: data.responseJSON.message,
                        type: "error",
                        button: "OK"
                    });
                }
            });
        },
        errorPlacement: function (error, element) {
            error.appendTo(element.parent());
        }
    });

    $("#calculateCredit").validate({
        rules: {
            calculatorCreditAmount: {
                required: true,
                regex: /^\d*[.,]?\d{0,2}$/
            },
            calculatorPercentageRate: {
                required: true,
                regex: /^\d*[.,]?\d{0,2}$/
            }
        },
        messages: {
            calculatorCreditAmount: {
                required: "This field is required",
                regex: "This field must be a real positive number"
            },
            calculatorPercentageRate: {
                required: "This field is required",
                regex: "This field must be a real positive number"
            }
        },
        submitHandler: function () {
            var amount = $("#calculatorCreditAmount").val();
            var rate = $("#calculatorPercentageRate").val();
            var installments = $("#calculatorPeriodInput").val() * 12;

            var creditDto = {
                creditAmount: amount,
                percentageRate: rate,
                installmentCount: installments
            };

            $.ajax({
                method: "GET",
                url: "/api/credits/calculate",
                data: creditDto,
                success: function (d) {
                    $("#creditInfoError").hide();
                    $("#creditInfoSuccess").html("<strong>" + installments + "</strong>" + " installments, " + "<strong>" + (parseFloat(d.data)).toFixed(2) + "</strong>" + " each. Total amount: " + "<strong>" + (d.data * installments).toFixed(2) + "</strong>");
                    $("#creditInfoSuccess").slideDown();
                },
                error: function (d) {
                    $("#creditInfoSuccess").hide();
                    $("#creditInfoError").html("<strong>" + d.responseJSON.message + "</strong>");
                    $("#creditInfoError").slideDown();
                }
            });
        },
        errorPlacement: function (error, element) {
            error.appendTo(element.parent());
        }
    });

    var getYearSpelling = function (value) {
        if (value == 1)
            return " year";
        else
            return " years";
    };

    var adjustNumberOfYears = function (elementToAdjust) {
        var currentPeriod = $(elementToAdjust + "Input").val();
        $(elementToAdjust + "Value").html(currentPeriod + getYearSpelling(currentPeriod));
    };

    (function () {
        adjustNumberOfYears("#formPeriod");
        adjustNumberOfYears("#calculatorPeriod");

        $("#formPeriodInput").on("input", function () {
            var name = $(this).attr("id").replace("Input", "");
            adjustNumberOfYears("#" + name);
        });

        $("#calculatorPeriodInput").on("input", function () {
            var name = $(this).attr("id").replace("Input", "");
            adjustNumberOfYears("#" + name);
        });

        $("#creditInfoSuccess").hide();
        $("#creditInfoError").hide();

    })();

    $("#calculatorModal").on("hidden.bs.modal", function () {
        $("#creditInfoSuccess").hide();
        $("#creditInfoError").hide();
    });

});