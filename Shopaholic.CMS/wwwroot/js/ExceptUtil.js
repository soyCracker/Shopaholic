

var ExceptUtil = new Vue({
    data:
    {

    },
    methods:
    {
        PostExceptionFuc: function (error) {
            if (error.response) {
                console.log(error.response.data); // => the response payload 
                ToastUtil.ErrorAlert('Success:' + error.response.data.Success + '\n' +
                    'Msg:' + error.response.data.Msg)
            }
            else {
                ToastUtil.ErrorAlert('post error')
            }
        }
    }
});