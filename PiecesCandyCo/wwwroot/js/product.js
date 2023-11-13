﻿var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url:'/admin/product/getall'},
        "columns": [
            { data: 'name', "width": "20%" },
            { data: 'price', "width": "10%" },
            { data: 'price10', "width": "10%" },
            { data: 'category.name', "width": "15%" },
            { data: 'description', "width": "20%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75" role="group">
                    <a href="/admin/product/upsert?id=${data}" class="btn btn-outline-primary btn-sm mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                    <a onClick=Delete('/admin/product/delete/${data}') class="btn btn-outline-danger btn-sm mx-2"><i class="bi bi-trash3-fill"></i> Delete</a>
                    </div>`

                    
                },
                "width": "20%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}

