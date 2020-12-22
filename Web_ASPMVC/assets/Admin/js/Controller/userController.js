//Ajax
//OOP

var user = {
    init: function () {
        user.registerEnvents();
    },
    registerEnvents: function () { //bắt sự kiện
        $('.btn-active').off('click').on('click', function (e)  //click lần đầu ta lấy được id(btn-active), click thứ 2 là gắn function mới
        {
            e.preventDefault(); //Event.preventDefault sẽ đảm bảo rằng form không bao giờ được gửi, và nó đã giành được quyền kiểm soát và ngăn chặn sự kiện đó khi click.
            var btn = $(this); //btn đại diện cho .btn-active
            var id = btn.data('id'); //lấy ra thuộc tính đã khai báo trong data-id
            $.ajax({
                url: "/Admin/User/ChangeStatus", //path: //controller/Action
                data: { id: id },
                datatype: "json",
                type: "POST",
                success: function(response)
                {
                    console.log(response);
                    if(response.status==true)
                    {
                        btn.text('Kích Hoạt');
                    }
                    else
                    {
                        btn.text('Khoá');
                    }
                }
            })
        })
    }
}
user.init()