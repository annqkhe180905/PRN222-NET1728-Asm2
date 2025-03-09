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

    // Preview image functionality
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

    // Form submission handling
    form.addEventListener("submit", function (event) {
        event.preventDefault();

        const formData = new FormData(form);
        const isUpdate = document.getElementById("newsId").value !== "";
        const url = isUpdate ? "?handler=Update" : "?handler=Create";

        // Get the anti-forgery token
        const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

        fetch(url, {
            method: "POST",
            body: formData,
            headers: {
                "RequestVerificationToken": token
            }
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error("Network response was not ok");
                }
                return response.json();
            })
            .then(data => {
                alert(isUpdate ? "Update successfully!" : "Create successfully");
                window.location.reload();
            })
            .catch(error => {
                console.error("Error:", error);
                alert("An error occurred!");
            });
    });

    // Initialize event handlers for action buttons
    document.querySelectorAll(".btn-warning").forEach(button => {
        button.addEventListener("click", function () {
            const id = this.getAttribute("data-id");
            openEditModal(id);
        });
    });

    document.querySelectorAll(".btn-danger").forEach(button => {
        button.addEventListener("click", function () {
            const id = this.getAttribute("data-id");
            openDeleteModal(id);
        });
    });
});

// Define these functions outside the DOMContentLoaded handler
function openCreateModal() {
    document.getElementById("modalTitle").textContent = "Create News Article";
    document.getElementById("newsForm").reset();
    document.getElementById("newsId").value = "";
    document.getElementById("previewImage").style.display = "none";

    // Show the modal using Bootstrap
    $('#newsModal').modal('show');
}

function openEditModal(id) {
    fetch(`?handler=GetNews&id=${id}`)
        .then(response => response.json())
        .then(data => {
            document.getElementById("modalTitle").textContent = "Edit News Article";
            document.getElementById("newsId").value = data.newsArticleId;
            document.getElementById("newsTitle").value = data.newsTitle;
            document.getElementById("headline").value = data.headline;

            if (data.imgUrl) {
                document.getElementById("previewImage").src = data.imgUrl;
                document.getElementById("previewImage").style.display = "block";
                document.getElementById("newsImageUrl").value = data.imgUrl;
            }

            // Show the modal using Bootstrap
            $('#newsModal').modal('show');
        })
        .catch(error => {
            console.error("Error fetching news:", error);
            alert("Error loading news article data");
        });
}

function openDeleteModal(id) {
    if (confirm("Are you sure you want to delete this article?")) {
        deleteNews(id);
    }
}

function deleteNews(id) {
    // Get the anti-forgery token
    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

    fetch("?handler=Delete", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "RequestVerificationToken": token
        },
        body: JSON.stringify(id)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error("Failed to delete");
            }
            return response.json();
        })
        .then(() => {
            document.getElementById(`newsRow_${id}`).remove();
        })
        .catch(error => {
            console.error("Error deleting news:", error);
            alert("Error deleting news article");
        });
}

//FINISH UPLOAD IMAGE
