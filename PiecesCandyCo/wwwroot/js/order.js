var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url:'/admin/order/getall'},
        "columns": [
            { data: 'id', "width": "5%" },
            { data: 'name', "width": "25%" },
            { data: 'phoneNumber', "width": "15%" },
            { data: 'applicationUser.email', "width": "20%" },
            { data: 'orderStatus', "width": "10%" },
            { data: 'orderTotal', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75" role="group">
                    <a href="/admin/order/details?orderId=${data}" class="btn btn-outline-primary btn-sm mx-2"> <i class="bi bi-binoculars-fill"></i></a>
                    </div>`

                    
                },
                "width": "5%"
            }
        ]
    });
}

