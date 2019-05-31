$(document).ready(function () {

    var scriptTag = $("#indexMoneyTransferScript");
    var userId = scriptTag.data("userid");
    $('#lastTransfers').DataTable({
        "ajax": { url: "/api/MoneyTransfer/GetLastFive", dataSrc: "" },
        "paging": false,
        "ordering": false,
        "info": false,
        "searching": false,
        "columns": [
            { "data": "name" },
            { "data": "id" },
            { "data": "from.userName" },
            { "data": "to.userName" },
            {
                "data": "cashAmount", "render": function (data, type, row, meta) {
                    if (row.from.id == userId) {
                        data = -data;
                    }

                    return data.toFixed(2).toLocaleString() + " PLN";
                }
            },
            {
                "data": "createdOn", "render": function (data, type, row, meta) {

                    return data.substring(0, 10);
                }
            }
        ],
        "rowCallback": function (row, data, index) {

            if (data.from.id === userId) {
                $('td', row).css('color', '#ff3232');
                $('td', row).css('font-weight', '500');
            } else {
                $('td', row).css('color', '#50C878');
                $('td', row).css('font-weight', '500');
            }
        }
    });
});