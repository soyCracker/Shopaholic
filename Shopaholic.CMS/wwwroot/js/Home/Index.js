var FlowChartApp = new Vue({
    el: '#FlowChartApp',
    data: {
        dateArray: new Array(),
        flowCountArray: new Array(),
        getMonthFlowApi: baseurl + "/Home/api/GetMonthFlow",
        isShow:false
    },
    mounted: function () {
        var self = this;
        self.GetMonthFlow();
    },
    methods: {
        GetMonthFlow: function () {
            let self = this;
            axios.post(self.getMonthFlowApi)
                .then(function (result) {
                    if (result.data.Success) {
                        self.isShow = true;
                        self.SetChart(result.data.Data);
                    }
                })
                .catch(function (error) {
                    ExceptUtil.PostExceptionFuc(error);
                });
        },

        SetChart: function (flows) {
            let self = this;
            for (let i = 0; i < flows.length; i++) {
                self.dateArray.push(flows[i].FlowDate);
                self.flowCountArray.push(flows[i].Count);
            }

            var options = {
                chart: {
                    type: 'bar',
                    height: '300%',
                    width:'100%'
                },
                series: [
                    {
                        name: '瀏覽量',
                        data: self.flowCountArray
                    }
                ],
                xaxis: {
                    categories: self.dateArray
                },
                fill: {
                    colors: ['#bbded4']
                }
            }

            var chart = new ApexCharts(document.querySelector('#chart'), options)
            chart.render()
        }
    }
});