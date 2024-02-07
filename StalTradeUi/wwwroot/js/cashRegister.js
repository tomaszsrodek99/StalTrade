$(document).ready(function () {
    $('.cash-register-table').each(function () {
        var table = new DataTable(this, {
            order: [[2, 'asc']],
            searching: false,
            dom: 'Blrtip',
            buttons: ['print'],
            language: {
                url: 'https://cdn.datatables.net/plug-ins/1.13.7/i18n/pl.json'
            },
            pageLength: 10,
            lengthMenu: [10, 25, 50],
            scrollY: '300px',
            scrollCollapse: true
        });
    });
});