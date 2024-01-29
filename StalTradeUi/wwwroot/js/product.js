$(document).ready(function () {
    var selectedRow = null;

    $('#edit-button, #delete-button').prop('disabled', true);

    $('#search-table tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {

            $(this).removeClass('selected');
            selectedRow = null;

            $('#edit-button, #delete-button');
        } else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
            selectedRow = table.row(this).data();
        }

        var isRowSelected = selectedRow !== null;
        $('#edit-button, #delete-button').prop('disabled', !isRowSelected);

        $('#edit-button, #delete-button').attr('data-product-id', isRowSelected ? selectedRow.productId : null);
    });

    $('#edit-button').on('click', function () {
        if (selectedRow !== null) {
            loadUpdateProductForm(selectedRow);
        }
    });

    var table = new DataTable('#search-table', {
        data: data,
        columns: [
            { data: 'productId', title: 'Id', visible: false },
            { data: 'name', title: 'Nazwa' },
            { data: 'companyDrawingNumber', title: 'Rysunek' },
            { data: 'customerDrawingNumber', title: 'Rysunek klienta' },
            { data: 'unitOfMeasure', title: 'Miara' },
            { data: 'purchaseVat', title: 'VAT zakupu' },
            { data: 'salesVat', title: 'VAT sprzeda¿y' },
            { data: 'consumptionStandard', title: 'Norma zu¿ycia' },
            { data: 'weight', title: 'Waga' },
            { data: 'chargeProfile', title: 'Profil wsadu' },
            { data: 'materialGrade', title: 'Gatunek materia³u' },
            { data: 'substituteGrade', title: 'Zamiennik' }
        ],
        searching: false,
        buttons: [
            'print'
        ],
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.13.7/i18n/pl.json'
        },
        select: {
            info: false,
            selector: 'tr',
            style: 'single'
        }
    });
});

function loadCreateProductForm() {
    var form = document.getElementById("productForm");
    form.reset();
    document.getElementById("productForm").action = "AddProduct";
    var partialView = document.getElementById("partial-view");
    partialView.style.visibility = "visible";
    blur();
}

function loadUpdateProductForm(itemObject) {
    var form = document.getElementById("productForm");
    form.reset();

    form.action = "PutProduct";

    document.getElementById("ProductId").value = itemObject.productId;
    document.getElementById("Name").value = itemObject.name;
    document.getElementById("CompanyDrawingNumber").value = itemObject.companyDrawingNumber;
    document.getElementById("CustomerDrawingNumber").value = itemObject.customerDrawingNumber;
    document.getElementById("UnitOfMeasure").value = itemObject.unitOfMeasure;
    document.getElementById("PurchaseVat").value = itemObject.purchaseVat;
    document.getElementById("SalesVat").value = itemObject.salesVat;
    document.getElementById("ConsumptionStandard").value = itemObject.consumptionStandard;
    document.getElementById("Weight").value = itemObject.weight;
    document.getElementById("ChargeProfile").value = itemObject.chargeProfile;
    document.getElementById("MaterialGrade").value = itemObject.materialGrade;
    document.getElementById("SubstituteGrade").value = itemObject.substituteGrade;

    var partialView = document.getElementById("partial-view");
    partialView.style.visibility = "visible";
    blur();
}

function checkUnique() {
    var companyDrawingNumber = $('#CompanyDrawingNumber').val();
    var productId = $('#ProductId').val();
    console.log(companyDrawingNumber);
    $.ajax({
        url: 'https://localhost:7279/api/Product/IsProductUnique/' + productId + '?companyDrawingNumber=' + encodeURIComponent(companyDrawingNumber),
        type: 'GET',
        success: function (result) {
            if (!result) {
                $('#uniqueProductError').text('Taki produkt ju¿ istnieje.');
            } else {
                $('#uniqueProductError').text('');
            }
        }
    });
}