var contact = {
    init: function () {
        contact.registerEvents();
    },
    registerEvents: function () {
        $('#btnSend').off('click').on('click', function () {
            var Name = $('#txtName').val();
            var Email = $('#txtEmail').val();
            var Phone = $('#txtPhone').val();
            var Address = $('#txtAddress').val();
            var Message = $('#txtMessage').val();

            $.ajax({
                url: '/Contact/Send',
                type: 'POST',
                dataType: 'json',
                data: {
                    Name: Name,
                    Email: Email,
                    Phone: Phone,
                    Address: Address,
                    Message: Message
                },
                success: function (res) {
                    if (res.status == true) {
                        window.alert('Gửi thành công!!');

                    }
                }
            });
        });

    }


}
contact.init();