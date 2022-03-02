if (!localStorage.getItem('isSignIn')) {
    console.log("isSignIn:未登入");
    location.href = window.location.origin + '/home/login';
}
else {
    console.log("isSignIn:登入");
}