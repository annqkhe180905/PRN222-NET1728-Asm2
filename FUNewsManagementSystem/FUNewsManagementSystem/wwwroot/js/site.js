// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener('DOMContentLoaded', function () {
    console.log("Page loaded");
    const avatar = document.getElementById('avatar');
    const userInfoOverlay = document.getElementById('userInfo');

    if (avatar && userInfoOverlay) {
        console.log("Avatar and user info found");
        avatar.addEventListener('click', function () {
            console.log('Avatar clicked');
            userInfoOverlay.classList.toggle('d-none');
        });
    } else {
        console.log('Avatar or UserInfo overlay not found');
    }
});
