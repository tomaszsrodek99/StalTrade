var invoiceSaleTable, invoicePurchaseTable;
var selectedSaleRow = null;
var selectedPurchaseRow = null;
var currentSaleRow = null;
var currentPurchaseRow = null;
$(document).ready(function () {
    $('#delete-invoice-button').prop('disabled', true);

    $('#invoice-sale-table tbody').on('click', 'td.selectable-td', function (e) {
        e.stopPropagation();

        invoicePurchaseTable.rows().deselect();      

        currentSaleRow = $(this).closest('tr');

        if (currentSaleRow.hasClass('selected')) {
            currentSaleRow.removeClass('selected');
            selectedSaleRow = null;
            $('#delete-invoice-button').prop('disabled', true);
            invoicePurchaseTable.rows().deselect();           
        } else {
            invoiceSaleTable.rows().deselect();
            currentSaleRow.addClass('selected');
            selectedSaleRow = invoiceSaleTable.row(currentSaleRow).data();
        }

        var isRowSelected = selectedSaleRow !== null;
        $('#delete-invoice-button').prop('disabled', !isRowSelected);

        $('#delete-invoice-button').attr('data-invoice-id', isRowSelected ? selectedSaleRow[0] : null);
    });

    $('#invoice-purchase-table tbody').on('click', 'td.selectable-td', function (e) {
        e.stopPropagation();

        invoiceSaleTable.rows().deselect();      

        var currentPurchaseRow = $(this).closest('tr');

        if (currentPurchaseRow.hasClass('selected')) {
            currentPurchaseRow.removeClass('selected');
            selectedPurchaseRow = null;
            $('#delete-invoice-button').prop('disabled', true);
            invoicePurchaseTable.rows().deselect();           
        } else {
            invoicePurchaseTable.rows().deselect();
            currentPurchaseRow.addClass('selected');
            selectedPurchaseRow = invoicePurchaseTable.row(currentPurchaseRow).data();
        }

        var isRowSelected = selectedPurchaseRow !== null;
        $('#delete-invoice-button').prop('disabled', !isRowSelected);

        $('#delete-invoice-button').attr('data-invoice-id', isRowSelected ? selectedPurchaseRow[0] : null);
    });

    $('#add-invoice-purchase-button').on('click', function () {
        window.location.href = '/InvoiceUI/CreatePurchase/';
    });

    $('#add-invoice-sale-button').on('click', function () {
        window.location.href = '/InvoiceUI/CreateSale/';
    });

    $('#delete-invoice-button').on('click', function () {
        if (confirm('Czy na pewno chcesz usunąć fakturę?')) {
            if (selectedPurchaseRow)
                window.location.href = '/InvoiceUI/RemoveInvoice/' + selectedPurchaseRow[0];
            else
                window.location.href = '/InvoiceUI/RemoveInvoice/' + selectedSaleRow[0];
        }
    });

    $('#invoice-purchase-table tbody').on('click', 'button.show-products', function () {
        console.log("click");
        var tr = $(this).closest('tr');
        var row = invoicePurchaseTable.row(tr);
        var productsData = $(this).data('item');
        if (row.child.isShown()) {
            row.child.hide();
            tr.removeClass('shown');
        } else {
            row.child(showProducts(productsData)).show();
            tr.addClass('shown');
        }
    });

    $('#invoice-sale-table tbody').on('click', 'button.show-products', function () {
        console.log("click");
        var tr = $(this).closest('tr');
        var row = invoiceSaleTable.row(tr);
        var productsData = $(this).data('item');
        if (row.child.isShown()) {
            row.child.hide();
            tr.removeClass('shown');
        } else {
            row.child(showProducts(productsData)).show();
            tr.addClass('shown');
        }
    });

    invoiceSaleTable = new DataTable('#invoice-sale-table', {
        order: [[0, 'desc']],
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

    invoicePurchaseTable = new DataTable('#invoice-purchase-table', {
        order: [[0, 'desc']],
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
});

function showProducts(data) {
    console.log(data);
    var table = '<table class="table subtable table-info table-bordered table-hover">' +
        '<thead>' +
        '<tr>' +
        '<th>Nazwa</th>' +
        '<th>Ilość</th>' +
        '<th>Rzeczywista ilość</th>' +
        '<th>Netto</th>' +
        '<th>Brutto</th>' +
        '</tr>' +
        '</thead>' +
        '<tbody>';

    for (var i = 0; i < data.length; i++) {
        var actualQuantity = data[i].actualQuantity == 0 ? data[i].quantity : data[i].actualQuantity;

        table += '<tr>' +
            '<td hidden>' + data[i].invoiceId + '</td>' +
            '<td>' + data[i].product.name + ' ' + data[i].product.companyDrawingNumber + '</td>' +
            '<td>' + actualQuantity + '</td>' +
            '<td>' + data[i].actualQuantity + '</td>' +
            '<td>' + data[i].netto + '</td>' +
            '<td>' + (data[i].brutto || '-') + '</td>' +
            '</tr>';
    }

    table += '</tbody></table>';
    return table;
}