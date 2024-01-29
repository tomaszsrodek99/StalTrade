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

        $('#edit-button, #delete-button').attr('data-expense-id', isRowSelected ? selectedRow.expenseId : null);
    });

    $('#edit-button').on('click', function () {
        if (selectedRow !== null) {
            loadUpdateExpenseForm(selectedRow);
        }
    });


    var table = new DataTable('#search-table', {
        data: data,
        columns: [
            { data: 'expenseId', title: 'Id', visible: false },
            {
                data: 'date',
                title: 'Data',
                render: function (data, type, row) {
                    if (type === 'display') {
                        var date = new Date(data);
                        return date.toLocaleDateString();
                    }
                    return data;
                }
            },
            { data: 'contractor', title: 'Dostawca' },
            { data: 'invoiceNumber', title: 'Nr. faktury klienta' },
            { data: 'description', title: 'Opis' },
            { data: 'netto', title: 'Netto' },
            { data: 'brutto', title: 'Brutto' },
            {
                data: 'dateOfPayment',
                title: 'Termin p³atnoœci',
                render: function (data, type, row) {
                    if (type === 'display') {
                        var dateOfPayment = new Date(data);
                        return dateOfPayment.toLocaleDateString();
                    }
                    return data;
                }
            },
            {
                data: 'paid',
                title: 'Op³acone',
                render: function (data, type, row) {
                    if (type === 'display') {
                        return '<input type="checkbox" ' + (data ? 'checked' : '') + ' disabled>';
                    }
                    return data;
                }
            },
            { data: 'paymentType', title: 'P³atnoœæ' },
            { data: 'eventType', title: 'Typ zdarzenia' }
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
function loadCreateExpenseForm() {
    var form = document.getElementById("expenseForm");
    form.reset();
    document.getElementById("expenseForm").action = "AddExpense";
    var partialView = document.getElementById("partial-view-expense");
    partialView.style.visibility = "visible";
    blur();
}

function loadUpdateExpenseForm(element) {
    var form = document.getElementById("expenseForm");
    form.reset();

    form.action = "PutExpense";

    var itemData = element.getAttribute("data-item");
    var itemObject = JSON.parse(itemData);



    var partialView = document.getElementById("partial-view-expense");
    partialView.style.visibility = "visible";
    blur();
}

$(function () {
    $(".autocomplete-contractor").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: 'https://localhost:7279/api/Expense/AutocompleteContractor',
                type: 'GET',
                dataType: 'json',
                data: {
                    term: request.term
                },
                success: function (data) {
                    response(data);
                }
            });
        },
        minLength: 2
    });
});

$(function () {
    $(".autocomplete-description").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: 'https://localhost:7279/api/Expense/AutocompleteDescription',
                type: "POST",
                dataType: "json",
                data: {
                    term: request.term
                },
                success: function (data) {
                    response(data);
                }
            });
        },
        minLength: 2
    });
});