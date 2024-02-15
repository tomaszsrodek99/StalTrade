$(document).ready(function () {
    var selectedRow = null;

    $('#edit-company-button, #delete-company-button, #add-contact-button').prop('disabled', true);

    $('#search-table tbody').on('click', 'td.selectable-td', function (e) {
        e.stopPropagation();

        var currentRow = $(this).closest('tr');

        if (currentRow.hasClass('selected')) {
            currentRow.removeClass('selected');
            selectedRow = null;
            $('#edit-company-button, #delete-company-button, #add-contact-button').prop('disabled', true);
        } else {
            table.rows().deselect();
            currentRow.addClass('selected');
            selectedRow = table.row(currentRow).data();
        }

        var isRowSelected = selectedRow !== null;
        $('#edit-company-button, #delete-company-button, #add-contact-button').prop('disabled', !isRowSelected);
        $('#edit-company-button, #delete-company-button, #add-contact-button').attr('data-company-id', isRowSelected ? selectedRow[0] : null);
    });
    $('#search-table tbody').on('click', 'button.show-contacts', function () {
        var tr = $(this).closest('tr');
        var row = table.row(tr);
        var contactsData = $(this).data('item');
        if (row.child.isShown()) {
            row.child.hide();
            tr.removeClass('shown');
        } else {
            row.child(showContacts(contactsData)).show();
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

    $('#delete-company-button').on('click', function () {
        if (selectedRow !== null) {
            if (confirm('Czy na pewno chcesz usunąć rekord?')) {
                window.location.href = '/CompanyUI/RemoveCompany/' + selectedRow.companyID;
            }
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
                alert('Wystąpił błąd podczas przetwarzania żadania.' + textStatus + ' ' + errorThrown);
            }
        });
    }); 

    var table = new DataTable('#search-table', {
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.13.7/i18n/pl.json'
        },
        searching: false,
        dom: 'Blrtip',
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
function showContacts(data) {
    var table = '<table class="table subtable table-info table-bordered table-hover">' +
        '<thead>' +
        '<tr>' +
        '<th>Imię</th>' +
        '<th>Nazwisko</th>' +
        '<th>Stanowisko</th>' +
        '<th>Telefon 1</th>' +
        '<th>Telefon 2</th>' +
        '<th>Email</th>' +
        '<th></th>' +
        '</tr>' +
        '</thead>' +
        '<tbody>';

    for (var i = 0; i < data.length; i++) {
        table += '<tr>' +
            '<td hidden>' + data[i].countryID + '</td>' +
            '<td>' + data[i].firstname + '</td>' +
            '<td>' + data[i].lastname + '</td>' +
            '<td>' + data[i].position + '</td>' +
            '<td>' + data[i].phone1 + '</td>' +
            '<td>' + (data[i].phone2 || '-') + '</td>' +
            '<td>' + data[i].email + '</td>' +
            '<td>' +
            '<div class="btn-group" role="group">' +
            '<a class="btn btn-secondary update-btn" onclick="loadUpdateContactForm(this)" data-item=\'' + JSON.stringify(data[i]) + '\' style="margin-right: 5px;">Edytuj</a>' +
            '<a class="btn btn-danger" href="RemoveContact/' + data[i].contactID + '" onclick="return confirm(\'Czy na pewno chcesz usunąć kontakt?\')">Usuń</a>' +
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
    document.getElementById("company-form-name").innerHTML = "Dodaj firmę";
    document.getElementById("companyForm").action = "AddCompany";
    var partialView = document.getElementById("partial-view-company");
    partialView.style.visibility = "visible";
    blur();
}

function loadUpdateCompanyForm(data) {
    var form = document.getElementById("companyForm");
    form.reset();

    form.action = "PutCompany";
    document.getElementById("company-form-name").innerHTML = "Edytuj firmę";

    document.getElementById("companyId").value = data[0];
    document.getElementById("name").value = data[1];
    document.getElementById("short-name").value = data[2];
    document.getElementById("address").value = data[3];
    document.getElementById("city").value = data[4];
    document.getElementById("postal-code").value = data[5];
    document.getElementById("post-office").value = data[6];
    document.getElementById("nip").value = data[7];
    document.getElementById("payment-method").value = data[8];

    var partialView = document.getElementById("partial-view-company");
    partialView.style.visibility = "visible";
    blur();
}

function loadCreateContactForm(id) {
    var form = document.getElementById("contactForm");
    form.reset();
    document.getElementById("contact-form-name").innerHTML = "Dodaj kontakt";
    document.getElementById("contactForm").action = "AddContact";
    document.getElementById("CompanyID").value = id;
    var partialView = document.getElementById("partial-view-contact");
    partialView.style.visibility = "visible";
    blur();
}

function loadUpdateContactForm(element) {
    var form = document.getElementById("contactForm");
    form.reset();
    document.getElementById("contact-form-name").innerHTML = "Edytuj kontakt";
    form.action = "PutContact";

    var itemData = $(element).attr("data-item");
    var itemObject = JSON.parse(itemData);

    document.getElementById("companyId").value = itemObject.companyID;
    document.getElementById("contactId").value = itemObject.contactID;
    document.getElementById("firstname").value = itemObject.firstname;
    document.getElementById("lastname").value = itemObject.lastname;
    document.getElementById("position").value = itemObject.position;
    document.getElementById("phone1").value = itemObject.phone1;
    document.getElementById("phone2").value = itemObject.phone2;
    document.getElementById("email").value = itemObject.email;

    var partialView = document.getElementById("partial-view-contact");
    partialView.style.visibility = "visible";
    blur();
}




