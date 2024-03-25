$(function() {
    // Open the new room type window
    function openNewRoomTypeForm(e) {
        e.preventDefault();
        $("#NewRoomTypeSection").toggle();
    }

    // Adding the new room type to the database
    function submitNewRoomTypeForm(e) {
        e.preventDefault();
        var form = $(this);

        // Clear previous errors
        $("#roomTypeErrors").empty();

        $.ajax({
            url: form.attr("action"),
            type: form.attr("method"),
            data: form.serialize(),
            success: function (data) {
                if (data.success) {
                    // Close the modal and add the new room type to the dropdown
                    $("#NewRoomTypeSection").toggle();
                    $("#RoomTypeId").append(new Option(data.roomType, data.roomTypeId, false, true));
                } else {
                    // Iterate through the error list and display them
                }
            },
        });
    }

    $("#AddNewRoomType").on("click", openNewRoomTypeForm); // Opens the New room type modal window
    $("#addRoomTypeForm").on("submit", submitNewRoomTypeForm); // Submits the New room type form
});