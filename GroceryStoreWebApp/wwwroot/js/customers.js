$(document).ready(function () {

    loadCustomersTable();

    document.getElementById('btnNew').addEventListener("click", addCustomer, false)

    function loadCustomersTable() {
        $("div.item").remove(); //clear old items

        $.ajax({
            url: WebApiBaseUrl + '/api/Groceries',
            type: 'GET',
            dataType: 'json',
            success: function (result) {
                for (i = 0; i < result.customers.length; i++) {
                    let myID = result.customers[i].id;
                    $("#divContainer").append('<div class="grid-item item"><button type="button" id="btnDelete' + i + '" value="' + myID + '">Delete</button>' + '</div>');
                    $("#divContainer").append('<div class="grid-item item">' + result.customers[i].id + '</div>');
                    $("#divContainer").append('<div class="grid-item item"><input type="text" value="' + result.customers[i].name + '" id="txtName' + i + '" name="' + myID + '"/></div>');

                    document.getElementById('btnDelete' + i).addEventListener("click", deleteCustomer, false);
                    document.getElementById('txtName' + i).addEventListener("change", updateCustomer, false);
                }
            },
            error: function (request, message, error) {
                console.log("Error: " + error);
            }
        });
    }

    function deleteCustomer(evt) {
        $.ajax({
            url: WebApiBaseUrl + '/api/Groceries/' + evt.target.value,
            type: 'DELETE',
            //dataType: 'json',
            success: function () {
                loadCustomersTable();
            },
            error: function (request, message, error) {
                console.log("Error: " + error);
            }
        });
    }

    function updateCustomer(evt) {
        $.ajax({
            url: WebApiBaseUrl + '/api/Groceries/' + evt.target.name + '/' + evt.target.value,
            type: 'PUT',
            dataType: 'json',
            success: function () {
                loadCustomersTable();
            },
            error: function (request, message, error) {
                console.log("Error: " + error);
            }
        });
    }

    function addCustomer() {
        var name = prompt("Enter new customer's name:", "");

        if (name != null && name != "") {
            $.ajax({
                url: WebApiBaseUrl + '/api/Groceries/' + name,
                type: 'POST',
                dataType: 'json',
                success: function () {
                    loadCustomersTable();
                },
                error: function (request, message, error) {
                    console.log("Error: " + error);
                }
            });
        }
    }

});


