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

    var isCollapsed = true;
    $('.show-details').on('click', function () {
        var targetId = $(this).data('target');

        var targetElement = document.getElementById(targetId);

        if (isCollapsed) {
            targetElement.classList.remove('collapse');
        } else {
            targetElement.classList.add('collapse');
        }

        isCollapsed = !isCollapsed;
    });
});

function deleteDeposit(id) {
    if (confirm('Czy na pewno chcesz anulować wpłatę?')) {
        window.location.href = '/ExpenseUI/RemoveDeposit/' + id;
    }
}

function loadDepositForm() {
    var form = document.getElementById("depositListForm");
    var partialView = document.getElementById("partial-view-deposit");
    partialView.style.visibility = "visible";
    blur();
}

function loadDepositesList() {
    var partialView = document.getElementById("partial-view-deposit-list");
    partialView.style.visibility = "visible";
    blur();
}