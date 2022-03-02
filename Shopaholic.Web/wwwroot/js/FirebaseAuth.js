/*if (!localStorage.getItem('signInId')) {
    console.log("signInId:未登入");
    location.href = window.location.origin + '/home/login';
}
else {
    console.log("signInId:登入");
}*/

var provider = new firebase.auth.GoogleAuthProvider();

// 登入狀態變換
firebase.auth().onAuthStateChanged(function (user) {
    if (!user) {
        location.href = window.location.origin + '/home';
    }
});