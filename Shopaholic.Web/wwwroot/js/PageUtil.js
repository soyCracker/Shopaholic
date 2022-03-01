

var PageUtil = new Vue({
    data:
    {

    },
    methods:
    {
        GetDisplayPageArray: function (page, pageMaxDisplay, totalPages) {
            let pageCount = pageMaxDisplay;
            if (totalPages < pageMaxDisplay) {
                pageCount = totalPages;
            }
            let pageDisplayArray = new Array()
            pageDisplayArray.push(page);
            let count = 1;
            for (let i = 1; i < pageCount; i++) {
                if (page - i > 0) {
                    pageDisplayArray.push(page - i);
                    count++;
                }
                if (page + i <= totalPages) {
                    pageDisplayArray.push(page + i);
                    count++;
                }
                if (count == pageCount) {
                    break;
                }
            }
            // array sort small to big
            return pageDisplayArray.sort(function (a, b) {
                if (a > b) return 1;
                if (a < b) return -1;
                return 0;
            });
        }
    }
});