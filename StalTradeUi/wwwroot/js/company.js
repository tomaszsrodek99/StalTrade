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
        window.location.href = '/CompanyUI/CreateCompanyView';
    });

    $('#add-contact-button').on('click', function () {
        window.location.href = '/ContactUI/CreateContactView/' + selectedRow[0];
    });

    $('#edit-company-button').on('click', function () {
        window.location.href = '/CompanyUI/CreateCompanyView/' + selectedRow[0];
    });

    $('#delete-company-button').on('click', function () {
        if (confirm('Czy na pewno chcesz usunąć rekord?')) {
            window.location.href = '/CompanyUI/RemoveCompany/' + selectedRow[0];
        }
    });

    $('#NIP').on('blur', function () {
        var nip = $('#NIP').val();
        var oldNip = $('#oldNip').val();
        var companyId = $('#companyId').val();

        if (companyId == undefined || null) {
            companyId = 0;
        }

        if (nip.length == 10 && oldNip != nip) {
            $.ajax({
                url: 'https://localhost:7090/CompanyUI/NipExists?nip=' + encodeURIComponent(nip) + '&companyId=' + companyId,
                type: "POST",
                success: function (result) {
                    if (result.success === true) {  
                        $('#uniqueNIPError').text('');
                        $('#save-company-button').prop('disabled', false);
                    } else {
                        $('#uniqueNIPError').text('Firma o podanym NIPie istnieje.');
                        $('#save-company-button').prop('disabled', true);
                    }
                },
                error: function (textStatus, errorThrown) {
                    alert('Wystąpił błąd podczas przetwarzania żadania.' + textStatus + ' ' + errorThrown);
                }
            });
        }
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
            '<a class="btn btn-secondary update-btn" href="/ContactUI/UpdateContactView/' + data[i].contactID + '" style="margin-right: 5px;">Edytuj</a>' +
            '<a class="btn btn-danger" href="/ContactUI/RemoveContact/' + data[i].contactID + '" onclick="return confirm(\'Czy na pewno chcesz usunąć kontakt?\')">Usuń</a>' +
            '</div>' +
            '</td>' +
            '</tr>';
    }

    table += '</tbody></table>';
    return table;
}





