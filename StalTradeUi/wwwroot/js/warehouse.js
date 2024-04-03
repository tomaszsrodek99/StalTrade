var selectedProductRow = null;
var selectedPriceRow = null;
var currentPriceRow = null;
var chartId = null;
$(document).ready(function () {
    $('#edit-price-button, #delete-price-button, #add-purchase-price-button, #add-sale-price-button').prop('disabled', true);

    $('#product-table tbody').on('click', 'td.selectable-td', function (e) {
        e.stopPropagation();

        var currentProductRow = $(this).closest('tr');
        if (currentProductRow.hasClass('selected')) {
            currentProductRow.removeClass('selected');
            selectedProductRow = null;
            $('#add-price-button, #edit-price-button, #delete-price-button, #add-purchase-price-button, #add-sale-price-button').prop('disabled', true);
            var priceTableBody = $('#price-table tbody');
            priceTableBody.empty();
        } else {
            productTable.rows().deselect();
            currentProductRow.addClass('selected');
            selectedProductRow = productTable.row(currentProductRow).data();
            var latestPricesForSelectedProduct = latestPrices.filter(price => price.productId == selectedProductRow[0]);

            updatePriceTable(latestPricesForSelectedProduct);
            if (chartId != null)
                removeChart(chartId);
            $('#add-purchase-price-button, #add-sale-price-button').prop('disabled', false);
            $('#edit-price-button, #delete-price-button').prop('disabled', true);
            var isProductRowSelected = selectedProductRow != null;
            $('#add-purchase-price-button, #add-sale-price-button').attr('data-product-id', isProductRowSelected ? selectedProductRow[0] : null);
            var priceTableBody = $('#price-table tbody');
        }
    });

    $('#price-table tbody').on('click', 'tr', function (e) {
        e.stopPropagation();

        var currentPriceRow = $(this).closest('tr');
        if (currentPriceRow.hasClass('selected')) {
            currentPriceRow.removeClass('selected');
            $('#edit-price-button, #delete-price-button').prop('disabled', true);
            $('#edit-price-button, #delete-price-button').attr('data-price-id', null);

            if (chartId != null)
                removeChart(chartId);

            selectedPriceRow = null;
        } else {
            var availableCompanies = getAvailableCompanies();
            updateCompanyOptions(availableCompanies);

            $('#price-table tbody tr.selected').removeClass('selected');
            if (chartId != null)
                removeChart(chartId);
            selectedPriceRow = $(this).find('td').map(function () {
                return $(this).text();
            }).get();

            currentPriceRow.addClass('selected');
            $('#edit-price-button, #delete-price-button').attr('data-price-id', selectedPriceRow[2]);
            $('#edit-price-button, #delete-price-button').prop('disabled', false);

            //console.log(selectedProductRow);
            //console.log(selectedPriceRow);
            chartId = 'chart-' + selectedPriceRow[2];

            var chartData = prepareChartData(productList, selectedPriceRow);
            console.log(chartData);
            if (chartData.length > 1) {
                showDataTable(chartId, chartData);
            }
        }
    });

    $('#edit-price-button').on('click', function () {
        loadUpdatePriceForm(selectedPriceRow);
    }); $('#add-purchase-price-button').on('click', function () {
        var availableCompanies = getAvailableCompanies(true);
        updateCompanyOptions(availableCompanies);
        loadCreatePriceForm(selectedProductRow[0]);
        document.getElementById("is-purchase").checked = true;
    }); $('#add-sale-price-button').on('click', function () {
        var availableCompanies = getAvailableCompanies(false);
        updateCompanyOptions(availableCompanies);
        loadCreatePriceForm(selectedProductRow[0]);
        document.getElementById("is-purchase").checked = false;
    });

    $('#delete-price-button').on('click', function () {
        if (confirm('Czy na pewno chcesz usunąć cenę?')) {
            window.location.href = '/WarehouseUI/RemovePrice/' + selectedPriceRow[0];
        }
    });

    var table = new DataTable('#search-table', {
        order: [[1, 'asc']],
        searching: false,
        dom: 'Blrtip',
        buttons: [
            'print',
        ],
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.13.7/i18n/pl.json'
        },
        pageLength: 10,
        lengthMenu: [10, 25, 50],
        scrollY: '800px',
        scrollCollapse: true
    });

    var productTable = new DataTable('#product-table', {
        order: [[1, 'asc']],
        searching: false,
        paging: false,
        info: false,
        select: {
            info: false,
            selector: 'td.selectable-td',
            style: 'single'
        },
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.13.7/i18n/pl.json'
        },
        scrollY: '800px',
        scrollCollapse: true
    });

    priceTable = new DataTable('#price-table', {
        order: [[3, 'asc']],
        searching: false,
        paging: false,
        info: false,
        select: {
            info: false,
            selector: 'tr',
            style: 'single'
        },
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.13.7/i18n/pl.json'
        },
        scrollY: '800px',
        scrollCollapse: true
    });
});

function updatePriceTable(prices) {
    var priceTableBody = $('#price-table tbody');
    priceTableBody.empty();
    if (prices.length > 0) {
        prices.forEach(price => {
            //            console.log(price);
            priceTableBody.append(`
                <tr>
                    <td id="priceId-${price.priceId}" hidden>${price.priceId}</td>
                    <td id="productId-${price.productId}" hidden>${price.productId}</td>
                    <td id="companyId-${price.companyId}" hidden>${price.companyId}</td>
                    <td id="company-name-${price.company.name}">${price.company.name}</td>
                    <td id="price-data-${price.date}">${new Date(price.date).toLocaleDateString()}</td>
                    <td id="netto-${price.netto}">${price.netto}</td>
                    <td id="is-purchase-${price.isPurchase}">${price.isPurchase ? 'Zakup' : 'Sprzedaż'}</td>
                </tr>
            `);
        });
    } else {
        priceTableBody.append(`
            <tr>
                <td colspan="7" class="text-center">Dodaj pierwszą cenę dla wybranego produktu.</td>
            </tr>
        `);
    }
}

function loadCreatePriceForm(data) {
    var form = document.getElementById("priceForm");
    form.reset();
    document.getElementById("product-id").value = data;
    document.getElementById("price-form-name").innerHTML = "Dodaj cenę";
    var partialView = document.getElementById("partial-view-price");
    partialView.style.visibility = "visible";
    blur();
}

function loadUpdatePriceForm(data) {
    //console.log(data);
    var form = document.getElementById("priceForm");
    form.reset();
    form.action = "PutPrice";
    document.getElementById("price-form-name").innerHTML = "Edytuj cenę dla " + data[3];

    var companySelect = $('#company-id');
    companySelect.empty();
    //console.log(companySelect);
    companySelect.append($('<option>', {
        value: data[2],
        text: data[3]
    }));
    var companyName = data[3];
    var selectElement = document.getElementById("company-id");
    var option = Array.from(selectElement.options).find(option => option.text == companyName);
    if (option) {
        option.selected = true;
    }
    companySelect.prop('disabled', true);
    document.getElementById("price-id").value = data[0];
    document.getElementById("product-id").value = data[1];
    document.getElementById("company-id").value = data[2];
    document.getElementById("netto").value = data[5];
    document.getElementById("is-purchase").value = data[6];

    var partialView = document.getElementById("partial-view-price");
    partialView.style.visibility = "visible";
    blur();
}

function updateCompanyOptions(companies) {
    var companySelect = $('#company-id');
    companySelect.empty();

    for (var i = 0; i < companies.length; i++) {
        companySelect.append($('<option>', {
            value: companies[i].companyID,
            text: companies[i].name
        }));
    }
}

function getAvailableCompanies(isPurchase) {
    //console.log("wszystkie " + allCompanies);

    var usedCompanyIds = getUsedCompanyIds(isPurchase);
    //console.log("usedIds " + usedCompanyIds);
    if (usedCompanyIds.length > 0) {
        var availableCompanies = allCompanies.filter(function (company) {
            return !usedCompanyIds.includes(company.companyID);
        });
        //console.log("dostepne " + availableCompanies);
        return availableCompanies;
    } else
        return allCompanies;
}

function getUsedCompanyIds(isPurchase) {
    var usedCompanyIds = [];

    $('#price-table tbody tr').each(function () {
        var isPurchaseId = $(this).find('td:last').attr('id');
        if (isPurchaseId && isPurchaseId.endsWith(`is-purchase-${isPurchase}`)) {
            var companyId = $(this).find('td:eq(2)').text();
            usedCompanyIds.push(parseInt(companyId));
        }
    });

    return usedCompanyIds;
}
function removeChart(chartId) {
    var canvasContainer = document.getElementById("chart-container");
    var existingTable = document.getElementById("chart-table");
    if (existingTable) {
        canvasContainer.removeChild(existingTable);
    }

    var existingButton = document.getElementById("details-price-button");
    if (existingButton) {
        existingButton.parentNode.removeChild(existingButton);
    }

    var existingChart = document.getElementById(chartId);
    if (existingChart) {
        existingChart.remove();
    }
}
function prepareChartData(productList, selectedPriceRow) {
    //console.log(selectedPriceRow);
    selectedPriceRow[6] = selectedPriceRow[6] === 'Zakup' ? true : false;
    var selectedProduct = productList.find(product => product.productId == selectedPriceRow[1]);
    //console.log(selectedProduct);
    var selectedPrices = null;
    selectedPrices = selectedProduct.prices.filter(price => price.companyId == selectedPriceRow[2] && price.isPurchase === selectedPriceRow[6]);
    selectedPrices.sort((a, b) => new Date(b.date) - new Date(a.date));
    //console.log(selectedPrices);
    var data = selectedPrices.map(price => ({
        date: new Date(price.date).toLocaleDateString(),
        netto: price.netto
    }));
    //console.log(data)
    return data;
} 

function showDataTable(chartId, chartData) {
    var canvasContainer = document.getElementById("chart-container");
    var existingCanvas = document.querySelector("canvas");
    if (existingCanvas) {
        canvasContainer.removeChild(existingCanvas);
    }

    var table = document.createElement("table");
    table.className = "table subtable table-info table-bordered table-hover data-table";
    table.id = "chart-table";

    var thead = document.createElement("thead");
    thead.className = "thead-dark";
    var headerRow = thead.insertRow();
    headerRow.insertCell().innerText = "Data";
    headerRow.insertCell().innerText = "Netto";

    var tbody = document.createElement("tbody");
    //console.log(table);
    //console.log(chartData);
    chartData.forEach(entry => {
        var row = tbody.insertRow();
        row.insertCell().innerText = entry.date;
        row.insertCell().innerText = entry.netto;
    });

    table.appendChild(thead);
    table.appendChild(tbody);
    canvasContainer.appendChild(table);
}
