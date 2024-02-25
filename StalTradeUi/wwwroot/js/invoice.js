var invoiceSaleTable, invoicePurchaseTable;
var selectedRow = null;
var currentSaleRow = null;
var currentPurchaseRow = null;
$(document).ready(function () {
    $('#delete-invoice-button').prop('disabled', true);

    $('#invoice-sale-table tbody').on('click', 'td.selectable-td', function (e) {
        e.stopPropagation();

        currentSaleRow = $(this).closest('tr');

        if (currentSaleRow.hasClass('selected')) {
            currentSaleRow.removeClass('selected');
            selectedRow = null;
            $('#delete-invoice-button').prop('disabled', true);
            invoicePurchaseTable.rows().deselect();
            if (currentPurchaseRow)
                currentPurchaseRow.addClass('selected');
        } else {
            invoiceSaleTable.rows().deselect();
            currentSaleRow.addClass('selected');
            selectedRow = invoiceSaleTable.row(currentSaleRow).data();
        }

        var isRowSelected = selectedRow !== null;
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
            invoicePurchaseTable.rows().deselect();
            if (currentSaleRow)
                currentSaleRow.addClass('selected');
        } else {
            invoicePurchaseTable.rows().deselect();
            currentPurchaseRow.addClass('selected');
            selectedRow = invoicePurchaseTable.row(currentPurchaseRow).data();
        }

        var isRowSelected = selectedRow !== null;
        $('#delete-invoice-button').prop('disabled', !isRowSelected);

        $('#delete-invoice-button').attr('data-invoice-id', isRowSelected ? selectedRow[0] : null);
    });

    $('#add-invoice-purchase-button').on('click', function () {
        window.location.href = '/InvoiceUI/CreatePurchase/';
    });

    $('#add-invoice-sale-button').on('click', function () {
        window.location.href = '/InvoiceUI/CreateSale/';
    });

    $('#delete-invoice-button').on('click', function () {
        if (confirm('Czy na pewno chcesz usunąć fakturę?')) {
            window.location.href = '/InvoiceUI/RemoveInvoice/' + selectedRow[0];
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

    invoicePurchaseTable = new DataTable('#invoice-purchase-table', {
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
        table += '<tr>' +
            '<td hidden>' + data[i].invoiceId + '</td>' +
            '<td>' + data[i].product.name + data[i].product.companyDrawingNumber + '</td>' +
            '<td>' + data[i].quantity + '</td>' +
            '<td>' + data[i].actualQuantity + '</td>' +
            '<td>' + data[i].netto + '</td>' +
            '<td>' + (data[i].brutto || '-') + '</td>' +     
            '</tr>';
    }

    table += '</tbody></table>';
    return table;
}