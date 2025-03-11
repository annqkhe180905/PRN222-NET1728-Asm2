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

// UPLOAD IMAGE
document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("newsForm");
    const fileInput = document.getElementById("newsImage");
    const previewImage = document.getElementById("previewImage");

    // Hiển thị ảnh xem trước
    fileInput.addEventListener("change", function () {
        const file = fileInput.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function (e) {
                previewImage.src = e.target.result;
                previewImage.style.display = "block";
            };
            reader.readAsDataURL(file);
        }
    });

    // Xử lý submit form (tạo hoặc cập nhật bài viết)
    form.addEventListener("submit", function (event) {
        event.preventDefault(); // Ngăn form submit mặc định

        const formData = new FormData(form);
        const file = fileInput.files[0];

        if (file) {
            formData.append("newsImage", file);
        }

        const isUpdate = form.dataset.action === "update"; // Kiểm tra đang tạo hay cập nhật
        const url = isUpdate ? "/News/OnPostUpdateAsync" : "/News/OnPostCreateAsync";

        fetch(url, {
            method: "POST",
            body: formData
        })
            .then(response => response.json())
            .then(data => {
                if (data) {
                    alert(isUpdate ? "Update successfully!" : "Create successfully");
                    window.location.reload(); // Reload lại trang sau khi xử lý xong
                } else {
                    alert("An error occured!");
                }
            })
            .catch(error => console.error("Error sending data:", error));
    });
});

