var TableTool = new Vue({
    el: '#TableTool',
    data:
    {
        categoryName: '',
        deleteApi: baseurl + "/Category/api/Delete"
    },
    methods:
    {
        DeleteCategory: function (id) {
            var self = this;
            if (confirm("確定刪除?")) {
                var obj = {
                    ['Id']: id
                };
                self.PostContent(obj);
            }
        },

        PostContent: function (obj) {
            var self = this;
            var url = self.deleteApi;
            console.log(url + ' post');
            axios.post(url, obj)
                .then(function (result) {
                    console.log(result);
                    console.log('Success:' + result.data.Success);
                    console.log('Msg:' + result.data.Msg);
                    console.log('Data:');
                    console.log(result.data.data);
                    if (result.data.Success) {
                        location.href = "/Category";
                    }
                })
                .catch(function (error) {
                    ExceptUtil.PostExceptionFuc(error);
                });
        }
    }
});