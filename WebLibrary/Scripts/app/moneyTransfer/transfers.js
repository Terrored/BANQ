$(document).ready(function () {
    var scriptTag = $("#transfersScript");
    var userId = scriptTag.data("userid");

    $('#allTransfers').DataTable({
        "ajax": { url: "/api/MoneyTransfer/GetAllTransfers", dataSrc: "" },
        "paging": false,
        "order": [[5, "desc"]],
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