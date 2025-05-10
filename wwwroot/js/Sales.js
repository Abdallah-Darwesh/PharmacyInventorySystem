const saleItems = [];

// Function to initialize search functionality
function initializeSearch() {
    const searchInput = document.getElementById('searchInput');
    if (!searchInput) return;

    searchInput.addEventListener('input', function () {
        let term = this.value;
        if (term.length < 2) {
            document.getElementById("searchResults").innerHTML = "";
            return;
        }

        fetch(`/Pharmacist/Sales/Search?term=${term}`)
            .then(res => res.json())
            .then(data => {
                let resultHTML = '';
                data.forEach(item => {
                    resultHTML += `<a href="#" class="list-group-item list-group-item-action"
                        onclick="addItem(${item.productID}, '${item.productName}', ${item.sUnitPrice}, ${item.sPPrice})">
                        <strong>${item.productName}</strong> | Unit: ${item.sUnitPrice} | Pack: ${item.sPPrice}
                    </a>`;
                });
                document.getElementById("searchResults").innerHTML = resultHTML;
            });
    });
}

// Initialize on page load
document.addEventListener('DOMContentLoaded', initializeSearch);

// Initialize after SPA navigation
if (window.initNewSaleInvoice) {
    window.initNewSaleInvoice();
} else {
    window.initNewSaleInvoice = function() {
        console.log('Initializing New Sale Invoice page');
        initializeSearch();
        // Clear any existing items
        saleItems.length = 0;
        renderTable();
    };
}

function addItem(id, name, unitPrice, packagePrice) {
    if (saleItems.find(x => x.productID === id)) return;

    const row = {
        productID: id,
        productName: name,
        saleType: 'Unit',
        quantity: 1,
        unitPrice: unitPrice,
        sUnitPrice: unitPrice,
        sPPrice: packagePrice
    };

    saleItems.push(row);
    renderTable();
    document.getElementById("searchResults").innerHTML = "";
    document.getElementById("searchInput").value = "";
}

function renderTable() {
    const tbody = document.getElementById("saleItemsTableBody");
    tbody.innerHTML = "";

    let total = 0;

    saleItems.forEach((item, index) => {
        let itemTotal = item.unitPrice * item.quantity;
        total += itemTotal;

        tbody.innerHTML += `
            <tr>
                <td>
                    ${item.productName}
                    <input type="hidden" name="SaleItems[${index}].ProductID" value="${item.productID}" />
                    <input type="hidden" name="SaleItems[${index}].ProductName" value="${item.productName}" />
                </td>
                <td>
                    <select name="SaleItems[${index}].SaleType" class="form-select"
                        onchange="changeType(${index}, this.value)">
                        <option value="Unit" ${item.saleType === "Unit" ? "selected" : ""}>Unit</option>
                        <option value="Package" ${item.saleType === "Package" ? "selected" : ""}>Package</option>
                    </select>
                </td>
                <td>
                    <input type="number" name="SaleItems[${index}].Quantity" value="${item.quantity}"
                        class="form-control" min="1" onchange="changeQuantity(${index}, this.value)" />
                </td>
                <td>
                    <input type="hidden" name="SaleItems[${index}].UnitPrice" value="${item.unitPrice}" />
                    ${item.unitPrice.toFixed(2)} EGP
                </td>
                <td>${itemTotal.toFixed(2)} EGP</td>
                <td><button type="button" class="btn btn-danger btn-sm" onclick="removeItem(${index})">🗑️</button></td>
            </tr>`;
    });

    document.getElementById("grandTotal").innerText = total.toFixed(2);
}

function changeType(index, type) {
    const item = saleItems[index];
    item.saleType = type;
    item.unitPrice = type === "Unit" ? item.sUnitPrice : item.sPPrice;
    renderTable();
}

function changeQuantity(index, quantity) {
    quantity = parseInt(quantity);
    if (quantity < 1) quantity = 1;
    saleItems[index].quantity = quantity;
    renderTable();
}

function removeItem(index) {
    saleItems.splice(index, 1);
    renderTable();
}

function validateSale() {
    if (saleItems.length === 0) {
        alert("⚠️ Please add at least one product to complete the sale.");
        return false;
    }

    const form = document.getElementById("saleForm");

    // امسح الحقول القديمة (لو موجودة)
    document.querySelectorAll(".dynamic-input").forEach(el => el.remove());

    // ضيف الحقول الجديدة
    saleItems.forEach((item, index) => {
        const inputs = [
            { name: `SaleItems[${index}].ProductID`, value: item.productID },
            { name: `SaleItems[${index}].ProductName`, value: item.productName },
            { name: `SaleItems[${index}].SaleType`, value: item.saleType },
            { name: `SaleItems[${index}].Quantity`, value: item.quantity },
            { name: `SaleItems[${index}].UnitPrice`, value: item.unitPrice }
        ];

        inputs.forEach(input => {
            const hiddenInput = document.createElement("input");
            hiddenInput.type = "hidden";
            hiddenInput.name = input.name;
            hiddenInput.value = input.value;
            hiddenInput.classList.add("dynamic-input");
            form.appendChild(hiddenInput);
        });
    });

    return true;
}

