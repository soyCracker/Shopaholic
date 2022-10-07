var FormTool = new Vue({
    el: '#FormTool',
    data:
    {
        getOrderDataApi: baseurl + '/Order/api/GetOrderData',
        pickupConfirmApi: baseurl + '/Order/api/PickupConfirm',
        returnConfirmApi: baseurl + '/Order/api/ReturnConfirm',
        manualFinishApi: baseurl + '/Order/api/ManunlFinish',
        progressNum: 0,
        orderId: '',
        orderAllData: null,
        pickupAble: false,
        returnAble: false,
        finishAble: false,
    },
    mounted: function () {
        var self = this;
        self.GetOrderId();
        self.RefreshUI();
    },
    methods:
    {
        GetOrderId: function () {
            var self = this;
            temp = window.location.pathname.split('/');
            self.orderId = temp[temp.length - 1];
        },

        RefreshUI: function () {
            var self = this;

            ToastUtil.InfoFire('處理中')

            var obj = {
                ['OrderId']: self.orderId,
            };

            axios.post(self.getOrderDataApi, obj)
                .then(function (result) {
                    if (result.data.Success) {
                        self.orderAllData = result.data.Data;
                        self.SetProcessNum(self.orderAllData);
                        self.PickupBtnAble(self.orderAllData);
                        self.ReturnBtnAble(self.orderAllData);
                        self.FinishBtnAble(self.orderAllData);
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

        PickupConfirm: function () {
            var self = this;

            ToastUtil.InfoFire('處理中')

            var obj = {
                ['OrderId']: self.orderId,
            };

            axios.post(self.pickupConfirmApi, obj)
                .then(function (result) {
                    if (result.data.Data) {
                        self.RefreshUI();
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

        ReturnConfirm: function () {
            var self = this;

            ToastUtil.InfoFire('處理中')

            var obj = {
                ['OrderId']: self.orderId,
            };

            axios.post(self.returnConfirmApi, obj)
                .then(function (result) {
                    if (result.data.Data) {
                        self.RefreshUI();
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

        ManualFinish: function () {
            var self = this;

            ToastUtil.InfoFire('處理中')

            var obj = {
                ['OrderId']: self.orderId,
            };

            axios.post(self.manualFinishApi, obj)
                .then(function (result) {
                    if (result.data.Data) {
                        self.RefreshUI();
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

        SetProcessNum: function (orderAllData) {
            var self = this;
            if (orderAllData.IsFinish) {
                self.progressNum = 100;
            }
            else if (orderAllData.Status == 0) {
                self.progressNum = 0;
            }
            else if (orderAllData.Status == 1) {
                self.progressNum = 20;
            }
            else if (orderAllData.Status == 2) {
                self.progressNum = 40;
            }
            else if (orderAllData.Status == 3) {
                self.progressNum = 60;
            }
            else if (orderAllData.Status == 4) {
                self.progressNum = 80;
            }
        },

        PickupBtnAble: function (orderAllData) {
            var self = this;
            if (orderAllData.Status == 1 && orderAllData.FailCode == 0) {
                self.pickupAble = true;
            }
            else {
                self.pickupAble = false;
            }
        },

        ReturnBtnAble: function (orderAllData) {
            var self = this;
            if (orderAllData.Status == 4 && orderAllData.FailCode == 1) {
                self.returnAble = true;
            }
            else {
                self.returnAble = false;
            }
        },

        FinishBtnAble: function (orderAllData) {
            var self = this;
            if (!orderAllData.IsFinish) {
                self.finishAble = true;
            }
            else {
                self.finishAble = false;
            }
        }
    }
});