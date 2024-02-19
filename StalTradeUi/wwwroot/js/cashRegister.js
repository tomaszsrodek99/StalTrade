$(document).ready(function () {
    $('.cash-register-table').each(function () {
        var table = new DataTable(this, {
            order: [[1, 'desc']],
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
        var chartId = 'chart-' + targetId.replace('details-', '');
        var targetElement = document.getElementById(targetId);

        if (isCollapsed) {
            targetElement.classList.remove('collapse');
            createChart(chartId, chartData);
        } else {
            var canvasContainer = document.getElementById(chartId);
            canvasContainer.remove();
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

function createChart(chartId, chartData) {
    var canvasContainer = document.getElementById("chart-container");

    var existingCanvas = document.getElementById(chartId);
    if (existingCanvas) {
        canvasContainer.removeChild(existingCanvas);
    }

    var canvas = document.createElement("canvas");
    canvas.id = chartId;
    canvas.width = 600;
    canvas.height = 400;
    canvasContainer.appendChild(canvas);

    var ctx = canvas.getContext('2d');
    new Chart(ctx, {
        type: 'line',
        data: chartData,
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
}