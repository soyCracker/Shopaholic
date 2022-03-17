

var ExceptUtil = new Vue({
    data:
    {

    },
    methods:
    {
        PostExceptionFuc: function (error) {
            if (error.response.status == 401) {
                sessionStorage.setItem('LoginReturnUrl', window.location.href);
                ToastUtil.RedirectAlert(error.response.status + ' 未登入', window.location.origin+'/Home/LoginPage')
            }
            else {
                ToastUtil.ErrorAlert(error.response.status + ' post error')
            }
        }
    }
});