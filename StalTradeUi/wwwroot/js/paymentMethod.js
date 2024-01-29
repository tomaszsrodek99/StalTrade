$(document).ready(function () {
    $('#add-method-button').on('click', function () {
        loadCreateMethodForm();
    });  
});

$(document).on('blur', '#methodName', function () {
    var paymentMethod = document.getElementById("methodName");
    $.ajax({
        url: 'https://localhost:7279/api/PaymentMethod/MethodExists/?request=' + paymentMethod.value,
        type: 'POST',
        success: function (result) {
            if (!result) {
                $('#uniqueMethodError').text('Taka metoda p³atnoœci ju¿ istnieje.');
                $('#save-method-button').prop('disabled', true);
            } else {
                $('#uniqueMethodError').text('');
                $('#save-method-button').prop('disabled', false);
            }
        },
        error: function (textStatus, errorThrown) {
            console.log('AJAX error:', textStatus, errorThrown);
            alert('Wyst¹pi³ b³¹d podczas przetwarzania ¿adania.');
        }
    });
});

function loadCreateMethodForm() {
    var form = document.getElementById("paymentMethodForm");
    form.reset();
    document.getElementById("paymentMethodForm").action = "AddMethod";
    var previousPartialView = document.getElementById("partial-view-company");
    var partialView = document.getElementById("partial-view-method");
    partialView.style.visibility = "visible";
    previousPartialView.style.visibility = "hidden";
}