var FormTool = new Vue({
    el: '#FormTool',
    data:
    {
        orderSearchApi: baseurl + "/Order/api/Search",
        totalPages: 1,
        pageSize: 20,
        currentPage: 1,
        orderList: null,
        pageMaxDisplay: 5,
        pageDisplayArray: [],
        searchStr: '',
        minDate: '2000-01-01',
        maxDate: '',
        searchBeginDate: '2000-01-01',
        searchEndDate: ''
    },
    mounted: function () {
        var self = this;
        self.InitTime();
        self.RefreshUI(1);
    },
    methods:
    {
        RefreshUI: function (page) {
            var self = this;
            self.currentPage = page;
            self.orderList = null;
            var obj = {
                ['Page']: page,
                ['PageSize']: self.pageSize,
                ['SearchStr']: self.searchStr,
                ['BeginTime']: self.searchBeginDate,
                ['EndTime']: self.searchEndDate,
            };

            axios.post(self.orderSearchApi, obj)
                .then(function (result) {
                    if (result.data.Success) {
                        self.orderList = result.data.Data.OrderHeaderDTOs;
                        self.totalPages = result.data.Data.TotalPages;
                        self.pageDisplayArray = PageUtil.GetDisplayPageArray(page, self.pageMaxDisplay, self.totalPages);
                    }
                    else {
                        ToastUtil.ErrorAlert('Success:' + result.data.Success + '\n' +
                            'Msg:' + result.data.Msg)
                    }
                })
                .catch(function (error) {
                    ExceptUtil.PostExceptionFuc(error);
                });
        },

        ClearFilter: function () {
            var self = this;
            self.searchStr = '';
            self.RefreshUI(1);
        },

        InitTime: function () {
            var today = new Date();
            self.maxDate = today.getFullYear() + '-' + (today.getMonth()) + '-' + today.getDate();
            self.searchEndDate = today.getFullYear() + '-' + (today.getMonth()) + '-' + today.getDate();
        }
    }
});