var cart = {
    init: function () {
        cart.regEvents();
    },
    regEvents: function () { //off là clear đi tất cả lệnh của click. //on là thiết lập lại sự kiện click
        $('#btnContinue').off('click').on('click', function () {
            window.location.href = "/";
        });
        $('#btnUpdate').off('click').on('click', function () {
            var listProduct = $('#txtquantity');
            var cartlist = [];
            $.each(listProduct, function (i, item) { //vòng lập của jquery
                cartlist.push({
                    Quantity: $(item).val(),
                    Product: {
                        ID: $(item).data('id')
                    }
                });
            });
            $.ajax({
                url: '/Cart/Update',
                data: { cartModel: JSON.stringify(cartlist) },//tạo đối tượng json sang chuỗi (stringify)
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if(res.status ==true)
                    {
                        window.location.href = "/gio-hang";
                    }   
                }
            })
        });
    }
}
cart.init();