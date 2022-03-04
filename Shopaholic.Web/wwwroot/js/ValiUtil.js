var ValiUtil = new Vue({
    data:
    {

    },
    methods:
    {
        IsNotEmptyOrNull: function (data) {
            if (data) {
                return true;
            }
            return false;
        },

        IsPhone: function (data) {
            const re = new RegExp('^09\\d{8}$');
            if (re.test(data)) {
                return true;
            }
            return false;
        }
    }
});