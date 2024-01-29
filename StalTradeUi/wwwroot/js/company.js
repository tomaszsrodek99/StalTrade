// -*- coding: utf-8 -*-
$(document).ready(function () {
    var selectedRow = null;
    $('#edit-company-button, #delete-company-button, #add-contact-button').prop('disabled', true);

    $('#search-table tbody').on('click', 'td.selectable-td', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
            selectedRow = null;
            $('#edit-company-button, #delete-company-button, #add-contact-button').prop('disabled', true);
        } else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
            selectedRow = table.row(this).data();
        }

        var isRowSelected = selectedRow !== null;
        $('#edit-company-button, #delete-company-button, #add-contact-button').prop('disabled', !isRowSelected);
        $('#edit-company-button, #delete-company-button, #add-contact-button').attr('data-company-id', isRowSelected ? selectedRow.companyID : null);
    });
    $('#search-table tbody').on('click', 'button.show-details', function () {
        var tr = $(this).closest('tr');
        var row = table.row(tr);

        if (row.child.isShown()) {
            row.child.hide();
            tr.removeClass('shown');
        } else {
            row.child(format(row.data())).show();
            tr.addClass('shown');
        }
    });
    $('#add-company-button').on('click', function () {
        loadCreateCompanyForm();
    });

    $('#add-contact-button').on('click', function () {
        if (selectedRow !== null) {
            loadCreateContactForm(selectedRow.companyID);
        }
    });

    $('#edit-company-button').on('click', function () {
        if (selectedRow !== null) {
            loadUpdateCompanyForm(selectedRow);
        }
    });

    $('#NIP').on('blur', function () {
        var nip = $('#NIP').val();
        var companyId = $('#CompanyID').val();
        $.ajax({
            url: 'https://localhost:7279/api/Company/IsNIPUnique/' + companyId + '?nip=' + encodeURIComponent(nip),
            type: 'GET',
            success: function (result) {
                if (!result) {
                    $('#uniqueNIPError').text('Firma o podanym NIPie istnieje.');
                    $('#save-company-button').prop('disabled', true);
                } else {
                    $('#uniqueNIPError').text('');
                    $('#save-company-button').prop('disabled', false);
                }
            },
            error: function (textStatus, errorThrown) {
                console.log('AJAX error:', textStatus, errorThrown);
                alert('Wyst¹pi³ b³¹d podczas przetwarzania ¿adania.');
            }
        });
    });
    
    var table = new DataTable('#search-table', {
        data: data,
        language: {
            url: '/js/pl.json'
        },
        columns: [
            { data: 'companyID', title: 'Id', visible: false },
            { data: 'name', title: 'Nazwa', class: 'selectable-td' },
            { data: 'shortName', title: 'Skrot', class: 'selectable-td' },
            { data: 'address', title: 'Adres firmy', class: 'selectable-td' },
            { data: 'city', title: 'Miasto', class: 'selectable-td' },
            { data: 'postalCode', title: 'Kod pocztowy', class: 'selectable-td' },
            { data: 'postOffice', title: 'Poczta', class: 'selectable-td' },
            { data: 'nip', title: 'NIP', class: 'selectable-td' },
            { data: 'paymentMethod', title: "Forma p³atnoœci", class: 'selectable-td' },
            {
                className: 'details-control', orderable: false, data: null, defaultContent: '', render: function (data, type, row) {
                    return row.contacts && row.contacts.length > 0
                        ? '<button class="show-details btn-info">Kontakty</button>'
                        : '';
                }
            }
        ],
        searching: false,
        buttons: [
            'print'
        ],       
        select: {
            info: false,
            selector: 'td.selectable-td',
            style: 'single'
        }
    });
});

function format(data) {
    var contacts = data.contacts;

    var table = '<table class="table subtable table-info table-bordered">' +
        '<thead>' +
        '<tr>' +
        '<th>Imiê</th>' +
        '<th>Nazwisko</th>' +
        '<th>Stanowisko</th>' +
        '<th>Telefon 1</th>' +
        '<th>Telefon 2</th>' +
        '<th>Email</th>' +
        '<th></th>' +
        '</tr>' +
        '</thead>' +
        '<tbody>';

    for (var i = 0; i < contacts.length; i++) {
        table += '<tr>' +
            '<td>' + contacts[i].firstname + '</td>' +
            '<td>' + contacts[i].lastname + '</td>' +
            '<td>' + contacts[i].position + '</td>' +
            '<td>' + contacts[i].phone1 + '</td>' +
            '<td>' + (contacts[i].phone2 || '-') + '</td>' +
            '<td>' + contacts[i].email + '</td>' +
            '<td>' +
            '<div class="btn-group" role="group">' +
            '<a class="btn btn-secondary update-btn" onclick="loadUpdateContactForm(this)" data-item=\'' + JSON.stringify(contacts[i]) + '\' style="margin-right: 5px;">Edytuj</a>' +
            '<a class="btn btn-danger" href="CompanyUI/RemoveContact/' + contacts[i].contactID + '" onclick="return confirm(\'Czy na pewno chcesz usun¹æ kontakt?\')">Usuñ</a>' +
            '</div>' +
            '</td>' +
            '</tr>';
    }

    table += '</tbody></table>';
    return table;
}

function loadCreateCompanyForm() {
    var form = document.getElementById("companyForm");
    form.reset();
    document.getElementById("companyForm").action = "AddCompany";
    var partialView = document.getElementById("partial-view-company");
    partialView.style.visibility = "visible";
    blur();
}

function loadUpdateCompanyForm(element) {
    var form = document.getElementById("companyForm");
    form.reset();

    form.action = "PutCompany";

    var itemData = $(element).attr("data-item");
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

    var partialView = document.getElementById("partial-view-company");
    partialView.style.visibility = "visible";
    blur();
}

function loadCreateContactForm(id) {
    var form = document.getElementById("contactForm");
    form.reset();
    document.getElementById("contactForm").action = "AddContact";
    document.getElementById("CompanyID").value = id;
    var partialView = document.getElementById("partial-view-contact");
    partialView.style.visibility = "visible";
    blur();
}

function loadUpdateContactForm(element) {
    var form = document.getElementById("contactForm");
    form.reset();

    form.action = "PutContact";

    var itemData = $(element).attr("data-item");
    var itemObject = JSON.parse(itemData);

    document.getElementById("CompanyID").value = itemObject.companyID;
    document.getElementById("ContactID").value = itemObject.contactID;
    document.getElementById("Firstname").value = itemObject.firstname;
    document.getElementById("Lastname").value = itemObject.lastname;
    document.getElementById("Position").value = itemObject.position;
    document.getElementById("Phone1").value = itemObject.phone1;
    document.getElementById("Phone2").value = itemObject.phone2;
    document.getElementById("Email").value = itemObject.email;

    var partialView = document.getElementById("partial-view-contact");
    partialView.style.visibility = "visible";
    blur();
}


