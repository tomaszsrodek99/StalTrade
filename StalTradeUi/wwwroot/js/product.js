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

    $('#edit-product-button').on('click', function () {
        if (selectedRow !== null) {
            loadUpdateProductForm(selectedRow);
        }
    });

    $('#delete-product-button').on('click', function () {
        if (selectedRow !== null) {
            if (confirm('Czy na pewno chcesz usunąć produkt?')) {
                window.location.href = '/ProductUI/RemoveProduct/' + selectedRow.productId;
            }
        }
    });

    var table = new DataTable('#search-table', {
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

function loadCreateProductForm() {
    var form = document.getElementById("productForm");
    form.reset();
    document.getElementById("productForm").action = "AddProduct";
    document.getElementById("product-form-name").innerHTML = "Dodaj produkt";
    var partialView = document.getElementById("partial-view");
    partialView.style.visibility = "visible";
    blur();
}

function loadUpdateProductForm(data) {
    console.log(data);
    var form = document.getElementById("productForm");
    form.reset();

    form.action = "PutProduct";
    document.getElementById("product-form-name").innerHTML = "Edytuj product";

    document.getElementById("productId").value = data[0];
    document.getElementById("name").value = data[1];
    document.getElementById("company-drawing-number").value = data[2];
    document.getElementById("customer-drawing-number").value = data[3];
    document.getElementById("unit-of-measure").value = data[4];
    document.getElementById("purchase-vat").value = data[5];
    document.getElementById("sales-vat").value = data[6];
    document.getElementById("consumption-standard").value = data[7].replace(',', '.');
    document.getElementById("weight").value = data[8].replace(',', '.');
    document.getElementById("charge-profile").value = data[9];
    document.getElementById("material-grade").value = data[10];
    document.getElementById("substitute-grade").value = data[11];

    var partialView = document.getElementById("partial-view");
    partialView.style.visibility = "visible";
    blur();
}

function checkUnique() {
    var companyDrawingNumber = $('#company-drawing-number').val();
    var productId = $('#productId').val();
    $.ajax({
        url: 'https://localhost:7279/api/Product/IsProductUnique/' + productId + '?companyDrawingNumber=' + encodeURIComponent(companyDrawingNumber),
        type: 'GET',
        success: function (result) {
            if (!result) {
                $('#uniqueProductError').text('Taki produkt już istnieje.');
            } else {
                $('#uniqueProductError').text('');
            }
        }
    });
}