﻿

function CloseForm() {
    var partialView = document.getElementById("partial-view");
    partialView.style.visibility = "hidden";
    $('#blurOverlay').hide();
    $('body').removeClass('blur-overlay-visible');
}

function blur() {
    $('#blurOverlay').show();
    $('body').addClass('blur-overlay-visible');
}

function CloseModal() {
    $('#modal').modal('hide');
    $('#blurOverlay').hide();
    $('body').removeClass('blur-overlay-visible');
}

function PostalCodeFormat() {
    var postalCodeInput = document.getElementById('PostalCode');
    var value = postalCodeInput.value;
    if (value.length === 2 && !value.includes('-')) {
        postalCodeInput.value = value + '-';
    }
}

function searchByName() {
    var input, filter, table, tr, td, i, txtValue;
    input = document.getElementById("searchInput");
    filter = input.value.toUpperCase();
    console.log(filter);
    table = document.getElementById("search-table");
    console.log(table);
    tr = table.getElementsByTagName("tr");
    console.log(tr);
    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[1];
        if (td) {
            txtValue = td.textContent || td.innerText;
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}