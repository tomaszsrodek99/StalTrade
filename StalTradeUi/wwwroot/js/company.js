$(document).ready(function () {
    $('#NIP').on('blur', function () {
        var nip = $('#NIP').val();
        var companyId = $('#CompanyID').val();

        $.ajax({
            url: 'https://localhost:7279/api/Company/IsNIPUnique/' + companyId + '?nip=' + encodeURIComponent(nip),
            type: 'GET',
            success: function (result) {
                if (!result) {
                    $('#uniqueNIPError').text('Firma o podanym NIPie ju¿ istnieje.');
                } else {
                    $('#uniqueNIPError').text('');
                }
            }
        });
    });
});

function loadCreateCompanyForm() {
    var form = document.getElementById("companyForm");
    form.reset();
    document.getElementById("companyForm").action = "AddCompany";
    var partialView = document.getElementById("partial-view");
    partialView.style.visibility = "visible";
    blur();
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
    blur();
}

function loadCreateContactForm(id) {
    var contactRow = document.getElementById(id);
    var url = '/CompanyUI/CreateAddContactForm?companyId=' + id;

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
    var url = '/CompanyUI/CreateUpdateContactForm?contactId=' + itemObject.contactID;

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