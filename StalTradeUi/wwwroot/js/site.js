

function CloseForm() {
    var elements = document.getElementsByClassName("partial-view");

    for (var i = 0; i < elements.length; i++) {
        var element = elements[i];
        element.style.visibility = "hidden";
    }

    $('#blurOverlay').hide();
    $('body').removeClass('blur-overlay-visible');
}

function blur() {
    $('#blurOverlay').show();
    $('body').addClass('blur-overlay-visible');
}

function PostalCodeFormat() {
    var postalCodeInput = document.getElementById('PostalCode');
    var value = postalCodeInput.value;
    if (value.length === 2 && !value.includes('-')) {
        postalCodeInput.value = value + '-';
    }
}

function searchByName() {
    var input, filter, table, tr, td, i, txtValue;
    input = document.getElementById("searchInput");
    filter = input.value.toUpperCase();
    console.log(filter);
    table = document.getElementById("search-table");
    console.log(table);
    tr = table.getElementsByTagName("tr");
    console.log(tr);
    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[0];
        if (td) {
            txtValue = td.textContent || td.innerText;
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}

function toggleSubMenu(id) {
    var submenu = document.getElementById(id);
    submenu.classList.toggle('show');
}

document.addEventListener('click', function (event) {
    var target = event.target;

    // Sprawdź, czy kliknięto na nagłówek tabeli
    if (target.tagName === 'TH' && target.parentNode.tagName === 'TR' && target.parentNode.parentNode.tagName === 'THEAD') {
        // Znajdź tabelę, do której należy kliknięty nagłówek
        var table = target.closest('table');

        // Pobierz indeks kolumny
        var columnIndex = Array.prototype.indexOf.call(target.parentNode.children, target);

        // Wywołaj funkcję sortującą dla znalezionej tabeli i indeksu kolumny
        if (table && columnIndex !== -1) {
            sortTable(table, columnIndex);
        }
    }
});

function sortTable(table, columnIndex) {
    var rows, switching, i, x, y, shouldSwitch;
    switching = true;

    // Ustal, czy sortować w porządku rosnącym czy malejącym
    var direction = "asc";
    if (table.getAttribute("data-sort-direction") === "asc") {
        direction = "desc";
    }

    // Zapisz aktualny kierunek sortowania w atrybucie
    table.setAttribute("data-sort-direction", direction);

    while (switching) {
        switching = false;
        rows = table.rows;

        for (i = 1; i < (rows.length - 1); i++) {
            shouldSwitch = false;

            // Pobierz wartości komórek do porównania
            x = rows[i].getElementsByTagName("td")[columnIndex];
            y = rows[i + 1].getElementsByTagName("td")[columnIndex];

            // Porównaj wartości, zależnie od typu danych
            if (direction === "asc") {
                if (isNaN(x.innerHTML)) {
                    shouldSwitch = x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase();
                } else {
                    shouldSwitch = parseFloat(x.innerHTML) > parseFloat(y.innerHTML);
                }
            } else if (direction === "desc") {
                if (isNaN(x.innerHTML)) {
                    shouldSwitch = x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase();
                } else {
                    shouldSwitch = parseFloat(x.innerHTML) < parseFloat(y.innerHTML);
                }
            }

            if (shouldSwitch) {
                // Zamień miejscami wiersze
                rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
                switching = true;
                break;
            }
        }
    }
}


