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

    $('#add-expense-button').on('click', function () {
        window.location.href = '/ExpenseUI/CreateExpenseView';
    });
    $('#edit-expense-button').on('click', function () {
        window.location.href = '/ExpenseUI/EditExpenseView/' + selectedRow[0];
    });

    $('#delete-expense-button').on('click', function () {
            if (confirm('Czy na pewno chcesz usunąć wydatek?')) {
                window.location.href = '/ExpenseUI/RemoveExpense/' + selectedRow[0];
            }
    });

    var table = new DataTable('#search-table', {
        order: [[7, 'desc']],     
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

$(function () {
    $(".autocomplete-contractor").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: 'https://localhost:7090/ExpenseUI/AutocompleteContractor?term=' + encodeURIComponent(request.term),
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    response(data.data);
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
                url: 'https://localhost:7090/ExpenseUI/AutocompleteDescription?term=' + encodeURIComponent(request.term),
                type: "GET",
                dataType: 'json',
                success: function (data) {
                    response(data.data);
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
                url: 'https://localhost:7090/ExpenseUI/AutocompleteEventType?term=' + encodeURIComponent(request.term),
                type: "GET",
                dataType: 'json',
                success: function (data) {
                    response(data.data);
                }
            });
        },
        minLength: 2
    });
});