Vue.component('validation-provider', VeeValidate.ValidationProvider);
Vue.component('validation-observer', VeeValidate.ValidationObserver);

var FormTool = new Vue({
    el: '#FormTool',
    data:
    {
        categoryName: '',
        createApi: baseurl + "/Category/api/Add",
        categoryIndexUrl: "/Category"
    },
    methods:
    {
        SubmitBtn: async function () {
            var self = this;
            console.log('submitBtn click');
            //click觸發驗證
            var isValid = await self.$refs.validObserver.validate();
            if (isValid) {
                ToastUtil.InfoFire('處理中')
                self.PostContent(self.GetPostObj(), self.createApi);
            }
            else {
                ToastUtil.ErrorAlert('validate error')
                console.log('validate error');
            }
        },

        GetPostObj: function () {
            var self = this;

            var obj = {
                ['Name']: self.categoryName
            };

            return obj;
        },

        PostContent: function (obj, url) {
            var self = this;
            console.log(url + ' post');
            axios.post(url, obj)
                .then(function (result) {
                    console.log('Success:' + result.data.success);
                    console.log('Msg:' + result.data.msg);
                    console.log('Data:');
                    console.log(result.data.data);
                    if (result.data.Success) {
                        location.href = self.categoryIndexUrl;
                    }
                    else {
                        ToastUtil.ErrorAlert('Success:' + result.data.Success + '\n' +
                            'Msg:' + result.data.Msg);
                    }
                })
                .catch(function (error) {
                    ExceptUtil.PostExceptionFuc(error);
                });
        }
    }
});