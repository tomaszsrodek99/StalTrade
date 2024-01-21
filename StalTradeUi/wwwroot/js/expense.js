function loadCreateExpenseForm() {
    var form = document.getElementById("expenseForm");
    form.reset();
    document.getElementById("expenseForm").action = "AddExpense";
    var partialView = document.getElementById("partial-view");
    partialView.style.visibility = "visible";
    blur();
}

function loadUpdateExpenseForm(element) {
    var form = document.getElementById("expenseForm");
    form.reset();

    form.action = "PutExpense";

    var itemData = element.getAttribute("data-item");
    var itemObject = JSON.parse(itemData);



    var partialView = document.getElementById("partial-view");
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