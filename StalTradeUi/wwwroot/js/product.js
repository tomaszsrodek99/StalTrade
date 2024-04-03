$(document).ready(function () {
    var selectedRow = null;

    $('#edit-product-button, #delete-product-button').prop('disabled', true);

    $('#search-table tbody').on('click', 'td.selectable-td', function (e) {
        e.stopPropagation();

        var currentRow = $(this).closest('tr');

        if (currentRow.hasClass('selected')) {
            currentRow.removeClass('selected');
            selectedRow = null;
            $('#edit-product-button, #delete-product-button').prop('disabled', true);
        } else {
            table.rows().deselect();
            currentRow.addClass('selected');
            selectedRow = table.row(currentRow).data();
        }

        var isRowSelected = selectedRow !== null;
        $('#edit-product-button, #delete-product-button').prop('disabled', !isRowSelected);

        $('#edit-product-button, #delete-product-button').attr('data-product-id', isRowSelected ? selectedRow[0] : null);
    });

    $('#add-product-button').on('click', function () {
        window.location.href = '/ProductUI/CreateProductView';
    });

    $('#edit-product-button').on('click', function () {
        window.location.href = '/ProductUI/CreateProductView/' + selectedRow[0];
    });

    $('#delete-product-button').on('click', function () {
            if (confirm('Czy na pewno chcesz usunąć produkt?')) {
                window.location.href = '/ProductUI/RemoveProduct/' + selectedRow[0];
        }
    });

    var table = new DataTable('#search-table', {
        order: [[3, 'asc']],
        searching: false,
        dom: 'Blrtip',
        buttons: [
            'print',
        ],
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.13.7/i18n/pl.json'
        },
        select: {
            info: false,
            selector: 'tr',
            style: 'single'
        },
        pageLength: 10,
        lengthMenu: [10, 25, 50],
        scrollY: '800px',
        scrollCollapse: true
    });
});

function checkUnique() {
    var companyDrawingNumber = $('#company-drawing-number').val();
    var productId = $('#productId').val();

    $.ajax({
        url: 'https://localhost:7090/ProductUI/ProductExists?productId=' + productId + '&companyDrawingNumber=' + encodeURIComponent(companyDrawingNumber),
        type: 'POST',
        success: function (result) {
            if (result.success === true) { 
                $('#uniqueProductError').text('');
            } else {
                $('#uniqueProductError').text('Taki produkt już istnieje.');
            }
        },
        error: function (textStatus, errorThrown) {
            alert('Wystąpił błąd podczas przetwarzania żadania.' + textStatus + ' ' + errorThrown);
        }
    });
}