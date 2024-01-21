function checkUnique() {
    var companyDrawingNumber = $('#CompanyDrawingNumber').val();
    var productId = $('#ProductId').val();
    console.log(companyDrawingNumber);
    $.ajax({
        url: 'https://localhost:7279/api/Product/IsProductUnique/' + productId + '?companyDrawingNumber=' + encodeURIComponent(companyDrawingNumber),
        type: 'GET',
        success: function (result) {
            if (!result) {
                $('#uniqueProductError').text('Taki produkt ju¿ istnieje.');
            } else {
                $('#uniqueProductError').text('');
            }
        }
    });
}

function loadCreateProductForm() {
    var form = document.getElementById("productForm");
    form.reset();
    document.getElementById("productForm").action = "AddProduct";
    var partialView = document.getElementById("partial-view");
    partialView.style.visibility = "visible";
    blur();
}

function loadUpdateProductForm(itemObject) {
    console.log(itemObject);
    var form = document.getElementById("productForm");
    form.reset();

    form.action = "PutProduct";

    //var itemData = element.getAttribute("data-item");
    //var itemObject = JSON.parse(itemData);

    document.getElementById("ProductId").value = itemObject.productId;
    document.getElementById("Name").value = itemObject.name;
    document.getElementById("CompanyDrawingNumber").value = itemObject.companyDrawingNumber;
    document.getElementById("CustomerDrawingNumber").value = itemObject.customerDrawingNumber;
    document.getElementById("UnitOfMeasure").value = itemObject.unitOfMeasure;
    document.getElementById("PurchaseVat").value = itemObject.purchaseVat;
    document.getElementById("SalesVat").value = itemObject.salesVat;
    document.getElementById("ConsumptionStandard").value = itemObject.consumptionStandard;
    document.getElementById("Weight").value = itemObject.weight;
    document.getElementById("ChargeProfile").value = itemObject.chargeProfile;
    document.getElementById("MaterialGrade").value = itemObject.materialGrade;
    document.getElementById("SubstituteGrade").value = itemObject.substituteGrade;

    var partialView = document.getElementById("partial-view");
    partialView.style.visibility = "visible";
    blur();
}