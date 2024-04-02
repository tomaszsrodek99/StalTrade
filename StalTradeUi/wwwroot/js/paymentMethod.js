$(document).ready(function () {
    $('#add-method-button').on('click', function () {
        loadCreateMethodForm();
        $('#save-method-button').prop('disabled', true);
    });
});

$(document).on('keyup', '#methodName', function () {
    var paymentMethod = document.getElementById("methodName").value;
    if (paymentMethod.length >= 2) {
        $.ajax({
            url: 'https://localhost:7090/CompanyUI/MethodExists?request=' + encodeURIComponent(paymentMethod),
            type: 'POST',
            success: function (result) {
                if (result.success === true) {                   
                    $('#save-method-button').prop('disabled', false);
                    $('#uniqueMethodError').text('');
                } else {
                    $('#save-method-button').prop('disabled', true);
                    $('#uniqueMethodError').text('Taka metoda płatności już istnieje.');
                }
            },
            error: function (textStatus, errorThrown) {
                alert('Wystąpił błąd podczas przetwarzania żadania.');
            }
        });
    }
});

function loadCreateMethodForm() {
    var form = document.getElementById("paymentMethodForm");
    form.reset();
    var partialView = document.getElementById("partial-view-method");
    partialView.style.visibility = "visible";
}

var table = new DataTable('#paymentMethodTable', {
    language: {
        url: 'https://cdn.datatables.net/plug-ins/1.13.7/i18n/pl.json'
    },
    searching: false,
    searching: false,
    paging: false,
    info: false,
    scrollY: '69vh',
    scrollCollapse: true
});
