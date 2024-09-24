function TagDelete(Id) {
    console.log("TagDelete called with ID:", Id);
   
    Swal.fire({
        title: 'Do you want to Delete?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            // Make a POST request to delete the record
            $.ajax({
                url: "/EmpUI/DeleteEmp",
                method: 'POST',
                data: {
                    id: Id
                },
                success: function (data) {
                    console.log("Delete response received:", data);
                    if (data.msg === "success") {
                        Swal.fire("Done", "Record Deleted Successfully!", "success");
                    }
                    else {
                        Swal.fire("Oops!!!", "Please Contact Admin", "error");
                    }
                    // Reload the page after deletion
                    window.location.reload();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.error("Error occurred while deleting:", textStatus, errorThrown);
                    Swal.fire("Oops!!!", "An error occurred while deleting the record.", "error");
                }
            });
        } else if (result.isDenied) {
            Swal.fire('Welcome', '', 'info');
        }
    });
}


function EditEmp(id) {
    console.log("EditEmp called with ID:", id);
    $.getJSON("/EmpUI/EditEmp/" + id, function (d) {
        console.log("Data received:", d); // Log the data received

        // Check if data is correctly fetched
        if (d) {
            $("#Id").val(d.id);
            $("#FirstName").val(d.firstName);
            $("#LastName").val(d.lastName);
            $("#Email").val(d.email);
            $("#Salary").val(d.salary);


            // Display the modal
            $("#exampleModal").modal('show');

            $("#exampleModalLabel").html('Section Edit Form:: [Edit]');
        } else {
            console.error("No data received for editing.");
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.error("Failed to fetch data for editing:", textStatus, errorThrown);
    });
}



function Validation_TDS() {

    if (!$("#tds1").valid()) {
        $("#tds1").addClass("was-validated");
        //alert(1);
        return false;
    }
    if (!$("#tds2").valid()) {
        $("#tds2").addClass("was-validated");
        //alert(1);
        return false;
    }
    if (!$("#tds3").valid()) {
        $("#tds3").addClass("was-validated");
        //alert(1);
        return false;
    }
    if (!$("#tds4").valid()) {
        $("#tds4").addClass("was-validated");
        //alert(1);
        return false;
    }
    if (!$("#tds5").valid()) {
        $("#tds5").addClass("was-validated");
        //alert(1);
        return false;
    }

}





$('#btn_search').click(function () {

    if (Validation_TDS() != false) {
        debugger;
        $.ajax({
            url: "/EmpUI/Empdetails",
            type: 'GET',
            contentType: 'application/json,charset=uts-8',
            data: {
                FirstName: $('#FirstName').val(),
                LastName: $('#LastName').val(),
                Email: $('#Email').val(),
                Salary: $('#Salary').val(),
            },
            traditional: true,
            dataType: 'json',
            success: function (data) {
                // Parse the JSON data
                var employees = JSON.parse(data);

                // Clear existing table body content
                $('#TBody_Emp').empty();

                // Loop through the employees array and append rows to the table body
                employees.forEach(function (employee, index) {
                    var row = `<tr>
                      <td style="display:none">${employee.Id}</td>
                      <td>${index + 1}</td>
                      <td>${employee.FirstName}</td>
                      <td>${employee.LastName}</td>
                      <td>${employee.Email}</td>
                      <td>${employee.Salary}</td>
                   </tr>`;
                    $('#TBody_Emp').append(row);
                });

                // Initialize DataTable if it's not already initialized
                if (!$.fn.DataTable.isDataTable('#example')) {
                    $('#example').DataTable();
                } else {
                    $('#example').DataTable().clear().destroy();
                    $('#example').DataTable();
                }

                // Show the table container
                $('#tab_emptable').show();
            },


            error: function (e) {

            }

        })

    }


})