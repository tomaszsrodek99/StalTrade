$(document).ready(function () {
    var selectedRow = null;

    $('#edit-expense-button, #delete-expense-button').prop('disabled', true);

    $('#search-table tbody').on('click', 'td.selectable-td', function (e) {
        e.stopPropagation();

        var currentRow = $(this).closest('tr');

        if (currentRow.hasClass('selected')) {
            currentRow.removeClass('selected');
            selectedRow = null;
            $('#edit-expense-button, #delete-expense-button').prop('disabled', true);
        } else {
            table.rows().deselect();
            currentRow.addClass('selected');
            selectedRow = table.row(currentRow).data();
        }

        var isRowSelected = selectedRow !== null;
        $('#edit-expense-button, #delete-expense-button').prop('disabled', !isRowSelected);

        $('#edit-expense-button, #delete-expense-button').attr('data-expense-id', isRowSelected ? selectedRow[0] : null);
    });

    $('#edit-expense-button').on('click', function () {
        if (selectedRow !== null) {
            loadUpdateExpenseForm(selectedRow);
        }
    });

    $('#delete-expense-button').on('click', function () {
        if (selectedRow !== null) {
            if (confirm('Czy na pewno chcesz usunąć wydatek?')) {
                window.location.href = '/ExpenseUI/RemoveExpense/' + selectedRow[0];
            }
        }
    });

    var table = new DataTable('#search-table', {
        order: [[1, 'asc']],     
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
    document.getElementById("expense-form-name").innerHTML = "Dodaj wydatek";
    var partialView = document.getElementById("partial-view-expense");
    partialView.style.visibility = "visible";
    blur();
}

function loadUpdateExpenseForm(data) {
    console.log(data);
    var form = document.getElementById("expenseForm");
    form.reset();

    form.action = "PutExpense";
    document.getElementById("expense-form-name").innerHTML = "Edytuj wydatek";

    document.getElementById("date").value = formatDateForInput(data[2]);
    document.getElementById("expenseId").value = data[0];
    document.getElementById("contractor").value = data[3];
    document.getElementById("invoice-number").value = data[1];
    document.getElementById("expenseId").value = data[0];
    document.getElementById("description").value = data[4];
    document.getElementById("brutto").value = data[6].replace(',', '.');
    document.getElementById("netto").value = data[5].replace(',', '.');
    document.getElementById("date-of-payment").value = formatDateForInput(data[7]);
    document.getElementById("paid").checked = data[8].includes("checked");
    document.getElementById("payment-type").value = data[9];
    document.getElementById("event-type").value = data[10];

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

function formatDateForInput(dateString) {
    var parts = dateString.split('/');

    var formattedDate = new Date(parts[2], parts[1] - 1, parts[0]); 

    var formattedDateString = formattedDate.toISOString().split('T')[0];

    return formattedDateString;
}