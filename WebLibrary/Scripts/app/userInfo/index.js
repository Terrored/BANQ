$(document).ready(function () {
    $("#accountInfoModal").on("show.bs.modal", function () {
        $.ajax({
            url: "/api/moneytransfer/metadata",
            dataType: "json",
            method: "GET",
            success: function (metadata) {
                $("#js-transfers-sent").html(metadata.transfersSent);
                $("#js-transfers-received").html(metadata.transfersReceived);
                $("#js-total-money-sent").html(metadata.totalMoneySent);
                $("#js-total-money-received").html(metadata.totalMoneyReceived);


                var chartCanvas = $("#js-transfer-chart");
                var chart = new Chart(chartCanvas, {
                    type: "doughnut",
                    data: {
                        labels: ["Money sent (PLN)", "Money received (PLN)"],
                        datasets: [{
                            data: [metadata.totalMoneySent, metadata.totalMoneyReceived],
                            backgroundColor: ['rgb(214, 51, 51)', 'rgb(118, 158, 60)'],
                            hoverBackgroundColor: ['rgb(222, 92, 92)', 'rgb(145, 177, 99)'],
                            borderWidth: 2,
                            borderColor: "grey"
                        }]
                    },
                    options: {
                        animation: {
                            animateRotate: true
                        },
                        responsive: false,
                        legend: {
                            labels: {
                                fontColor: "rgb(248, 201, 160)"
                            }
                        }
                    }
                });
            }
        })
    });
});