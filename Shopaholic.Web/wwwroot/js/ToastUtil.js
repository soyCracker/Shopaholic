
var ToastUtil = new Vue({
    data:
    {

    },
    methods:
    {
        InfoFire: function (info) {
            Swal.fire({               
                icon: 'info',
                title: info,
                position: 'top-end',
                showConfirmButton: false,
                timer: 3000
            })
        },

        ErrorAlert: function (info) {
            Swal.fire({
                title: 'Error',
                text: info,
                icon: 'error',
                confirmButtonText: 'OK'
            })
        },

        SampleAlert: function (info) {
            Swal.fire({
                title: 'info!',
                text: info,
                icon: 'info',
                confirmButtonText: 'Cool'
            })
        },

        RedirectAlert: function (info, url) {
            Swal.fire({
                title: 'Redirect',
                text: info,
                icon: 'info',
                confirmButtonText: 'OK'
            }).then(function () {
                location.href = url;
            });
        },
    }
});
