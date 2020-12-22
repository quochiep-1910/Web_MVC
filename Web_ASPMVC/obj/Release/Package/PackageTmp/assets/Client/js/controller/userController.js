var user = {
    init: function () {

        user.loadProvince();
        user.registerEvent();
    },
    registerEvent: function () {
        $('#ddlProvince').off('change').on('change', function () {
            var id = $(this).val();//lấy ra id được chọn (tức là tỉnh thành)
            if (id != '') //id khác rỗng
            {
                user.loadDistrict(parseInt(id)); //truyền vào id cho nó để truyền về UserController
            }
            else //id rỗng thì huyện mặc định thành rỗng 
            {
                $('#ddlDistrict').html('');

            }
        });
    },
  
    loadProvince: function () {

        $.ajax({
            url: '/User/LoadProvince',
            type: "POST",
            dataType: "json",
            success: function (response) {
                if (response.status == true) {/* nếu thành công*/
                    var html = '<option value="">--Chọn tỉnh--</option>';
                    var data = response.data; //data này truyền về từ UserController

                    $.each(data, function (i, item) {
                        html += '<option value ="' + item.ID + '">' + item.Name + '</option>'
                    });
                    //gán vào ddlDistrict trong logout
                    $('#ddlProvince').html(html);
                }
            }
        })
    },
    loadDistrict: function (id) {

        $.ajax({
            url: '/User/LoadDistrict',
            type: "POST",
            data: { provinceID: id}, //truyền tham số id từ view vào 
            dataType: "json",
            success: function (response) {
                if (response.status == true) {/* nếu thành công*/
                    var html = '<option value="">--Chọn Quận Huyện--</option>';
                    var data = response.data; //data này truyền về từ UserController

                    $.each(data, function (i, item) {
                        html += '<option value ="' + item.ID + '">' + item.Name + '</option>'
                    });
                    //gán vào ddlDistrict trong logout
                    $('#ddlDistrict').html(html);
                }
            }
        })
    }
}
user.init();

