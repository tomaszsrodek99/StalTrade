function loadCreateCompanyForm() {
    var form = document.getElementById("companyForm");
    form.reset();
    document.getElementById("companyForm").action = "AddCompany";
    var partialView = document.getElementById("partial-view");
    partialView.style.visibility = "visible";
}
function loadUpdateCompanyForm(element) {
    var form = document.getElementById("companyForm");
    form.reset();

    form.action = "PutCompany";

    var itemData = element.getAttribute("data-item");
    var itemObject = JSON.parse(itemData);

    document.getElementById("CompanyID").value = itemObject.companyID;
    document.getElementById("Name").value = itemObject.name;
    document.getElementById("ShortName").value = itemObject.shortName;
    document.getElementById("Address").value = itemObject.address;
    document.getElementById("City").value = itemObject.city;
    document.getElementById("PostalCode").value = itemObject.postalCode;
    document.getElementById("PostOffice").value = itemObject.postOffice;
    document.getElementById("NIP").value = itemObject.nip;
    document.getElementById("PaymentMethod").value = itemObject.paymentMethod;

    var partialView = document.getElementById("partial-view");
    partialView.style.visibility = "visible";
}

function loadCreateContactForm(id) {
    var contactRow = document.getElementById(id);
    var url = '/Company/CreateAddContactForm?companyId=' + id;

    var xhr = new XMLHttpRequest();
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {
            var createContactContainer = contactRow.getElementsByClassName('text-center-contact')[0];
            createContactContainer.innerHTML = xhr.responseText;
        }
    };

    xhr.open('GET', url, true);
    xhr.send();
}

function loadUpdateContactForm(element) {
    var itemData = element.getAttribute("data-item");
    var itemObject = JSON.parse(itemData);
    var contactRow = document.getElementById(itemObject.companyID);
    var url = '/Company/CreateUpdateContactForm?contactId=' + itemObject.contactID;

    var xhr = new XMLHttpRequest();
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {
            var createContactContainer = contactRow.getElementsByClassName('text-center-contact')[0];
            createContactContainer.innerHTML = xhr.responseText;
        }
    };

    xhr.open('GET', url, true);
    xhr.send();

}

function toggleContacts(id) {
    var contactRow = document.getElementById(id);
    document.getElementById('CompanyID').value = id;
    if (contactRow.style.display === "none") {
        contactRow.style.display = "";
    } else
        contactRow.style.display = "none";
}

function CloseForm() {
    var partialView = document.getElementById("partial-view");
    partialView.style.visibility = "hidden";
}

function CloseModal() {
    $('#modal').modal('hide');
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