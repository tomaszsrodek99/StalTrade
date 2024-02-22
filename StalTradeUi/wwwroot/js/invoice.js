$(document).ready(function () {
    var selectedSRow = null;
    var currentSaleRow = null;
    var currentPurchaseRow = null;

    $('#delete-invoice-button').prop('disabled', true);

    $('#invoice-sale-table tbody').on('click', 'td.selectable-td', function (e) {
        e.stopPropagation();

        currentSaleRow = $(this).closest('tr');

        if (currentSaleRow.hasClass('selected')) {
            currentSaleRow.removeClass('selected');
            selectedRow = null;
            $('#delete-invoice-button').prop('disabled', true);
            inoicePurchaseTable.rows().deselect();
            currentPurchaseRow.addClass('selected');
        } else {
            invoiceSaleTable.rows().deselect();
            currentSaleRow.addClass('selected');
            selectedRow = invoiceSaleTable.row(currentSaleRow).data();
        }

        var isRowSelected = selectedSRow !== null;
        $('#delete-invoice-button').prop('disabled', !isRowSelected);

        $('#delete-invoice-button').attr('data-invoice-id', isRowSelected ? selectedRow[0] : null);
    });

    $('#invoice-purchase-table tbody').on('click', 'td.selectable-td', function (e) {
        e.stopPropagation();

        var currentPurchaseRow = $(this).closest('tr');

        if (currentPurchaseRow.hasClass('selected')) {
            currentPurchaseRow.removeClass('selected');
            selectedRow = null;
            $('#delete-invoice-button').prop('disabled', true);
            invoiceSaleTable.rows().deselect();
            currentSaleRow.addClass('selected');
        } else {
            inoicePurchaseTable.rows().deselect();
            currentPurchaseRow.addClass('selected');
            selectedRow = inoicePurchaseTable.row(currentPurchaseRow).data();
        }

        var isRowSelected = selectedSRow !== null;
        $('#delete-invoice-button').prop('disabled', !isRowSelected);

        $('#delete-invoice-button').attr('data-invoice-id', isRowSelected ? selectedRow[0] : null);
    });

    $('#add-invoice-purchase-button').on('click', function () {
        loadCreatePurchaseInvoiceForm();
    });

    $('#add-invoice-sale-button').on('click', function () {
        loadCreateSaleInvoiceForm();
    });

    $('#delete-invoice-button').on('click', function () {
        if (confirm('Czy na pewno chcesz usunąć produkt?')) {
            window.location.href = '/InvoiceUI/RemovePrice/' + selectedRow[0];
        }
    });

    $('#invoice-purchase-table tbody', '#invoice-sale-table tbody').on('click', 'button.show-products', function () {
        var tr = $(this).closest('tr');
        var row = table.row(tr);
        var productsData = $(this).data('item');
        if (row.child.isShown()) {
            row.child.hide();
            tr.removeClass('shown');
        } else {
            row.child(showProducts(productsData)).show();
            tr.addClass('shown');
        }
    });

    var invoiceSaleTable = new DataTable('#invoice-sale-table', {
        order: [[0, 'asc']],
        searching: false,
        paging: false,
        info: false,
        select: {
            info: false,
            selector: 'td.selectable-td',
            style: 'single'
        },
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.13.7/i18n/pl.json'
        },
        scrollY: '800px',
        scrollCollapse: true
    });

    var inoicePurchaseTable = new DataTable('#invoice-purchase-table', {
        order: [[0, 'asc']],
        searching: false,
        paging: false,
        info: false,
        select: {
            info: false,
            selector: 'td.selectable-td',
            style: 'single'
        },
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.13.7/i18n/pl.json'
        },
        scrollY: '800px',
        scrollCollapse: true
    });

    var productTable = new DataTable('#product-table', {
        order: [[0, 'asc']],
        searching: false,
        paging: false,
        info: false,
        select: false,
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.13.7/i18n/pl.json'
        },
        scrollY: '400px',
        scrollCollapse: true
    });
});

function loadCreatePurchaseInvoiceForm() {
    var form = document.getElementById("invoicePurchaseForm");
    form.reset();
    var partialView = document.getElementById("partial-view-purchase-form");
    partialView.style.visibility = "visible";
    blur();
}

function loadCreateSaleInvoiceForm() {
    var form = document.getElementById("invoiceSaleForm");
    form.reset();
    var partialView = document.getElementById("partial-view-sale-form");
    partialView.style.visibility = "visible";
    blur();
}

function showProducts(data) {
    console.log(data);
    /*var table = '<table class="table subtable table-info table-bordered table-hover">' +
        '<thead>' +
        '<tr>' +
        '<th>Nazwa</th>' +
        '<th>Jednostka miary</th>' +
        '<th>Ilość</th>' +
        '<th>Netto</th>' +
        '<th>Brutto</th>' +      
        '</tr>' +
        '</thead>' +
        '<tbody>';

    for (var i = 0; i < data.length; i++) {
        table += '<tr>' +
            '<td hidden>' + data[i].countryID + '</td>' +
            '<td>' + data[i].firstname + '</td>' +
            '<td>' + data[i].lastname + '</td>' +
            '<td>' + data[i].position + '</td>' +
            '<td>' + data[i].phone1 + '</td>' +
            '<td>' + (data[i].phone2 || '-') + '</td>' +
            '<td>' + data[i].email + '</td>' +
            '<td>' +
            '<div class="btn-group" role="group">' +
            '<a class="btn btn-secondary update-btn" onclick="loadUpdateContactForm(this)" data-item=\'' + JSON.stringify(data[i]) + '\' style="margin-right: 5px;">Edytuj</a>' +
            '<a class="btn btn-danger" href="RemoveContact/' + data[i].contactID + '" onclick="return confirm(\'Czy na pewno chcesz usunąć kontakt?\')">Usuń</a>' +
            '</div>' +
            '</td>' +
            '</tr>';
    }

    table += '</tbody></table>';*/
    //return table;
    return null;
}

//CreateInvoice
function enableCompany() {
    var companyInput = document.getElementById("invoice-companyId");
    companyInput.removeAttribute('disabled');
}

function updateBrutto(quantityInput) {
    var row = quantityInput.parentNode.parentNode;
    var nettoInput = row.querySelector('input[name$=".Netto"]');
    var vatInput = row.querySelector('td[id^="product-vat-"]');
    var bruttoInput = row.querySelector('input[name$=".Brutto"]');

    var netto = parseFloat(nettoInput.value) || 0;
    var quantity = parseFloat(quantityInput.value) || 0;
    var vat = parseFloat(vatInput.textContent) || 0;

    var brutto = quantity * netto * (1 + vat / 100);
    bruttoInput.value = brutto.toFixed(2);

    updateInvoiceTotals();
}

function updateInvoiceTotals() {
    var table = document.getElementById("product-table");
    var bruttoPrices = table.querySelectorAll('input[name$=".Brutto"]');
    var nettoPrices = table.querySelectorAll('input[name$=".Netto"]');
    var nettoTotal = 0;
    var bruttoTotal = 0;

    bruttoPrices.forEach(function (product, index) {
        var brutto = product.value;
        bruttoTotal += brutto;
    });

    nettoPrices.forEach(function (product, index) {
        var netto = product.value;
        nettoTotal += netto;
    });

    document.getElementById('invoice-netto').value = nettoTotal;
    document.getElementById('invoice-brutto').value = bruttoTotal;
}

function getPaymentMethodById(companyId) {
    var company = companies.find(c => c.companyID == companyId);
    var paymentMethod = company.paymentMethod;
    var daysMatch = paymentMethod.match(/\d+/);
    var numberOfDays = daysMatch ? parseInt(daysMatch[0]) : null;

    if (numberOfDays != null && numberOfDays != undefined) {
        return numberOfDays;
    } else {
        return null;
    }
}

function updatePaymentDate() {
    var paymentDate = document.getElementById('invoice-payment-date');
    paymentDate.setAttribute('disabled', 'disabled');
    var today = new Date();
    var companyId = document.getElementById('invoice-companyId').value;
    var paymentMethod = getPaymentMethodById(companyId);

    if (paymentMethod != null && paymentMethod != undefined) {
        var newDate = new Date(today.setDate(today.getDate() + paymentMethod));
        var formattedDate = newDate.toISOString().split('T')[0];
        paymentDate.value = formattedDate;
    }
    else {
        var formattedDate = today.toISOString().split('T')[0];
        paymentDate.value = formattedDate;
        paymentDate.removeAttribute('disabled');
    }
}