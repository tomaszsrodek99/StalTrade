var formName;
$(document).ready(function () {
    var productTable = new DataTable('#product-table', {
        searching: false,
        paging: false,
        info: false,
        select: false,
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.13.7/i18n/pl.json'
        },
        scrollY: '345px',
        scrollCollapse: true
    });

    formName = document.getElementById('purchase-header');
    if (!formName) {
        console.log("działa");
        document.getElementById('actual-quantity-th').hidden = true;
    }

});

function enableCompany() {
    var companyInput = document.getElementById("invoice-companyId");
    companyInput.removeAttribute('readonly');
}

function updateBrutto(quantityInput) {
    var row = quantityInput.parentNode.parentNode;
    var nettoInput = row.querySelector('input[id^="netto-"]');
    //console.log(nettoInput);
    var vatInput = row.querySelector('td[id^="product-vat-"]');
    //console.log(vatInput);
    var bruttoInput = row.querySelector('input[name$=".Brutto"]');
    //console.log(bruttoInput);
    var sumValueInput = row.querySelector('input[name$=".Netto"]');
    //console.log(sumValueInput);
    var actualQuantity = row.querySelector('input[id^="product-actual-quantity-"]');

    if (quantityInput.value != 0) {
        actualQuantity.required = true;
        actualQuantity.disabled = false;
    } else {
        actualQuantity.required = false;
        actualQuantity.disabled = true;
    }

    var netto = parseFloat(nettoInput.value) || 0;
    var quantity = parseFloat(quantityInput.value) || 0;
    var vat = parseFloat(vatInput.textContent) || 0;

    var brutto = quantity * netto * (1 + vat / 100);
    var nettoSum = quantity * netto;

    bruttoInput.value = brutto.toFixed(2);
    sumValueInput.value = nettoSum.toFixed(2);
    updateInvoiceTotals();
}

function updateInvoiceTotals() {
    var table = document.getElementById("product-table");
    var bruttoPrices = table.querySelectorAll('input[name$=".Brutto"]');
    var nettoPrices = table.querySelectorAll('input[name$=".Netto"]');

    var nettoTotal = 0;
    var bruttoTotal = 0;

    bruttoPrices.forEach(function (product, index) {
        var brutto = parseFloat(product.value) || 0;
        bruttoTotal += brutto;
    });

    nettoPrices.forEach(function (product, index) {
        var netto = parseFloat(product.value) || 0;
        nettoTotal += netto;
    });

    document.getElementById('invoice-netto').value = nettoTotal.toFixed(2);
    document.getElementById('invoice-brutto').value = bruttoTotal.toFixed(2);
}

function getPaymentMethodById(companyId) {
    var company = companies.find(c => c.companyID == companyId);
    var paymentMethod = company.paymentMethod;
    var daysMatch = paymentMethod.match(/\d+/);
    var numberOfDays = daysMatch ? parseInt(daysMatch[0]) : null;

    if (numberOfDays != null && numberOfDays != undefined) {
        return numberOfDays;
    } else {
        return null;
    }
}

function filterProductsByCompanyId(companyId) {
    return products.map(product => {
        if (product.prices.length > 0) {
            var filteredPrices = product.prices.filter(price => price.companyId == companyId);
            return { ...product, prices: filteredPrices };
        } else {
            return product;
        }
    });
}

function updatePaymentDate() {
    var companyId = document.getElementById('invoice-companyId').value;
    var filteredProducts = filterProductsByCompanyId(companyId);
    console.log(filteredProducts);
    var paymentDate = document.getElementById('invoice-payment-date');
    paymentDate.setAttribute('readonly', 'readonly');
    var today = new Date();
    

    var paymentMethod = getPaymentMethodById(companyId);
    var company = companies.find(c => c.companyID == companyId);
    if (paymentMethod != null && paymentMethod != undefined) {
        var newDate = new Date(today.setDate(today.getDate() + paymentMethod));
        var formattedDate = newDate.toISOString().split('T')[0];
        paymentDate.value = formattedDate;
    }
    else {
        var formattedDate = today.toISOString().split('T')[0];
        paymentDate.value = formattedDate;
        paymentDate.removeAttribute('readonly');
    }

    document.getElementById('invoice-date-label').innerHTML = "Data płatności - " + company.paymentMethod;
 
    var productTableBody = $('#product-table tbody');
    productTableBody.empty();

    if (filteredProducts.length > 0) {
        for (var i = 0; i < filteredProducts.length; i++) {
            var product = filteredProducts[i];
            if (product.prices.length > 0) {
                var productId = product.productId;
                //console.log(product);
                var quantityInput = `<input id="product-quantity-${productId}" name="ProductsList[${i}].Quantity" type="number" oninput="updateBrutto(this)" class="form-control" min="0" step="1" placeholder="0" />`;
                var actualQuantityInput = `<td><input id="product-actual-quantity-${productId}" name="ProductsList[${i}].ActualQuantity" type="number" class="form-control" min="0" placeholder="0" step="1" disabled/></td>`;
                var nettoInput = `<input id="netto-${productId}" asp-for="ProductsList.ActualQuantity" value="${product.prices[0].netto}" type="number" class="form-control" min="0" step="0.01" placeholder="0" readonly />
                <span asp-validation-for="ProductsList.ActualQuantity" class="text-danger"></span>`;
                var productNettoInput = `<input id="product-netto-${productId}" name="ProductsList[${i}].Netto" type="number" class="form-control" min="0" step="0.01" placeholder="0" readonly />`;
                var productBruttoInput = `<input id="product-brutto-${productId}" name="ProductsList[${i}].Brutto" type="number" class="form-control" min="0" step="0.01" placeholder="0" readonly/>`;
                var productVat = `<td id="product-vat-${productId}"><p>${product.purchaseVat}</p></td>`;

                var quantityColumn = formName != null ? `<td>${quantityInput}</td>` : `<td><label>W magazynie ${product.stockStatus.inStock}</label>
            <input id="product-quantity-${productId}" name="ProductsList[${i}].Quantity" type="number" oninput="updateBrutto(this)" class="form-control" min="0" max="${product.stockStatus.inStock}" step="1" placeholder="0" /></td>`;
                var vatColumn = formName != null ? `${productVat}` : `<td id="product-vat-${productId}"><p>${product.salesVat}</p></td>`;
                var actualQuantityColumn = formName != null
                    ? `${actualQuantityInput}`
                    : `<td hidden><input id="product-actual-quantity-${productId}" name="ProductsList[${i}].ActualQuantity" type="number" class="form-control" value="0" /></td>`;         

                var tableRow = `
            <tr>
                <td hidden><input id="productId-${productId}" name="ProductsList[${i}].ProductId" class="form-control" type="number" value="${productId}" /></td>
                <td><p>${product.companyDrawingNumber} - ${product.name}</p></td>
                ${quantityColumn}
                ${actualQuantityColumn}
                <td><p>${product.unitOfMeasure}</p></td>
                <td>${nettoInput}</td>
                <td>${productNettoInput}</td>
                <td>${productBruttoInput}</td>
                ${vatColumn}
            </tr>
        `;

                productTableBody.append(tableRow);
            } else {
                productTableBody.append(`
        <tr>
        <td><p>${product.companyDrawingNumber} - ${product.name}</p></td>
            <td colspan="7" class="text-center">Brak ustalonej ceny cen dla danej firmy.</td>
        </tr>
    `);
            }
        }
    } else {
        productTableBody.append(`
        <tr>
            <td colspan="8" class="text-center">Brak ustalonych cen dla danej firmy.</td>
        </tr>
    `);
    }

}