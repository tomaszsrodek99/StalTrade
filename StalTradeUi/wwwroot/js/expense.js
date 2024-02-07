$(document).ready(function () {
    var selectedRow = null;

    $('#edit-button, #delete-expense-button').prop('disabled', true);

    $('#search-table tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {

            $(this).removeClass('selected');
            selectedRow = null;

            $('#edit-button, #delete-expense-button');
        } else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
            selectedRow = table.row(this).data();
        }

        var isRowSelected = selectedRow !== null;
        $('#edit-button, #delete-expense-button').prop('disabled', !isRowSelected);

        $('#edit-button, #delete-expense-button').attr('data-expense-id', isRowSelected ? selectedRow.expenseId : null);
    });

    $('#edit-button').on('click', function () {
        if (selectedRow !== null) {
            loadUpdateExpenseForm(selectedRow);
        }
    });

    $('#delete-expense-button').on('click', function () {
        if (selectedRow !== null) {
            if (confirm('Czy na pewno chcesz usunąć rekord?')) {
                window.location.href = '/ExpenseUI/RemoveExpense/' + selectedRow.expenseId;
            }
        }
    });

    var table = new DataTable('#search-table', {
        order: [[2, 'asc']],
        data: data,
        columns: [
            { data: 'expenseId', title: 'Id', visible: false },
            { data: 'invoiceNumber', title: 'Nr. faktury klienta', class: 'selectable-td' },
            { data: 'date', title: 'Data',
                render: function (data, type, row) {
                    if (type === 'display') {
                        var date = new Date(data);
                        return date.toLocaleDateString();
                    }
                    return data;
                }
                , class: 'selectable-td'
            },
            { data: 'contractor', title: 'Dostawca', class: 'selectable-td' },
            { data: 'description', title: 'Opis', class: 'selectable-td' },
            { data: 'netto', title: 'Netto', class: 'selectable-td' },
            { data: 'brutto', title: 'Brutto', class: 'selectable-td' },
            {
                data: 'dateOfPayment',
                title: 'Termin płatności',
                render: function (data, type, row) {
                    if (type === 'display') {
                        var date = new Date(data);
                        return date.toLocaleDateString();
                    }
                    return data;
                }
                , class: 'selectable-td'
            },
            {
                data: 'paid',
                title: 'Zapłacone?',
                render: function (data, type, row) {
                    if (type === 'display') {
                        return '<input type="checkbox" id="paid-checkbox-' + row.expenseId + '" value="' + row.expenseId + '" ' + (row.paid ? 'checked' : '') + ' disabled>';
                    }
                    return data;
                },
                class: 'selectable-td'
            },
            { data: 'paymentType', title: 'Płatność', class: 'selectable-td' },
            { data: 'eventType', title: 'Typ zdarzenia', class: 'selectable-td' },
            {
                className: 'payment-control',
                orderable: false,
                data: null,
                defaultContent: '',
                render: function (data, type, row) {
                    if (!row.paid) {
                        return '<button id="mark-as-paid-button-' + row.expenseId + '" class="change-status btn btn-info" onclick="updateStatus(' + row.expenseId + ')">Oznacz jako opłacone</button>';
                    } else {
                        return '';
                    }
                }
            },
        ],
        searching: false,
        dom: 'Blrtip',
        buttons: ['print'],
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.13.7/i18n/pl.json'
        },
        select: {
            info: false,
            selector: 'td.selectable-td',
            style: 'single'
        },
        pageLength: 10,
        lengthMenu: [10, 25, 50],
        scrollY: '800px',
        scrollCollapse: true
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

function loadUpdateExpenseForm(itemObject) {
    var form = document.getElementById("expenseForm");
    form.reset();

    form.action = "PutExpense";

    document.getElementById("expenseId").value = itemObject.expenseId;
    document.getElementById("date").value = itemObject.date;
    document.getElementById("contractor-input").value = itemObject.contractor;
    document.getElementById("invoice-number").value = itemObject.invoiceNumber;
    document.getElementById("description-input").value = itemObject.description;
    document.getElementById("netto").value = itemObject.netto;
    document.getElementById("brutto").value = itemObject.brutto;  
    document.getElementById("date-of-payment").value = itemObject.dateOfPayment;
    document.getElementById("paid").checked = itemObject.paid;
    document.getElementById("payment-type").value = itemObject.paymentType;
    document.getElementById("event-type-input").value = itemObject.eventType;

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
                type: "GET",
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

$(function () {
    $(".autocomplete-event-type").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: 'https://localhost:7279/api/Expense/AutocompleteEventType',
                type: "GET",
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

function updateStatus(id) {
    $.ajax({
        url: 'https://localhost:7279/api/Expense/ChangePaidStatus/' + id,
        type: 'POST',
        contentType: 'application/json',
        success: function (response) {
            $('#mark-as-paid-button-' + id).hide();

            $('#paid-checkbox-' + id).prop('checked', true);
        },
        error: function (error) {
            alert('Wystąpił błąd podczas przetwarzania żądania.' + error);
        }
    });
}