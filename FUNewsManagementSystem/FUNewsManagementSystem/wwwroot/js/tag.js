document.addEventListener('DOMContentLoaded', function () {
    toastr.options = {
        progressBar: true,
        positionClass: "toast-top-right",
        timeOut: 1000
    };

    // Display notification if stored in sessionStorage
    let notification = sessionStorage.getItem('notification');
    if (notification) {
        const { type, message } = JSON.parse(notification);
        if (type === 'success') {
            toastr.success(message);
        } else if (type === 'error') {
            toastr.error(message);
        }
        sessionStorage.removeItem('notification');
    }

    // Initialize search filter
    initSearchFilter();
});

// 🔹 Add Category
document.getElementById('addForm').addEventListener('submit', async function (e) {
    e.preventDefault();
    const formData = new FormData(this);

    try {
        const response = await fetch('?handler=Create', {
            method: 'POST',
            body: formData
        });
        const result = await response.json();

        if (result.success) {
            sessionStorage.setItem('notification', JSON.stringify({
                type: 'success',
                message: 'Tag added successfully'
            }));
            location.reload();
        } else {
            toastr.error('Failed to add tag');
        }
    } catch (error) {
        console.error('Error adding tag:', error);
        toastr.error('An error occurred while adding tag');
    }
});

// 🔹 Search Filter
function initSearchFilter() {
    const searchInput = document.getElementById('searchInput');
    const rows = document.querySelectorAll('#table-body-tag tr');

    searchInput.addEventListener('input', function () {
        const searchTerm = searchInput.value.toLowerCase();
        rows.forEach(row => {
            const name = row.querySelector('td:first-child').textContent.toLowerCase();
            row.style.display = name.includes(searchTerm) ? "" : "none";
        });
    });
}

// 🔹 Open Delete Confirmation Modal
let deleteId = null;
document.addEventListener('click', function (event) {
    if (event.target.classList.contains('btn-delete')) {
        deleteId = event.target.getAttribute('data-id');
        const deleteModal = new bootstrap.Modal(document.getElementById('confirmDeleteModal'));
        deleteModal.show();
    }
});

// 🔹 Delete Category
document.getElementById('confirmDeleteBtn').addEventListener('click', async function () {
    if (deleteId) {
        try {
            const response = await fetch(`?handler=Delete&id=${deleteId}`, {
                method: 'GET',
                headers: { 'Content-Type': 'application/json' }
            });

            const result = await response.json();
            if (result.success) {
                document.getElementById(`row-${deleteId}`)?.remove();
                toastr.success('tag deleted successfully');
            } else {
                toastr.error('Failed to delete tag');
            }
        } catch (error) {
            console.error('Error deleting category:', error);
            toastr.error('An error occurred while deleting');
        }
        deleteId = null;
    }
});

// 🔹 Open Update Modal and Pre-fill Data
document.getElementById('updateTagModal').addEventListener('show.bs.modal', function (e) {
    const button = e.relatedTarget;
    const updateId = button.getAttribute('data-id');
    const updateName = button.getAttribute('data-name');
    const updateDescription = button.getAttribute('data-note');
    this.querySelector('#updateTagId').value = updateId;
    this.querySelector('#updateTagName').value = updateName;
    this.querySelector('#updateNote').value = updateDescription;
});

// 🔹 Update Category
document.getElementById('updateForm').addEventListener('submit', async function (e) {
    e.preventDefault();
    const formData = new FormData(this);

    try {
        const response = await fetch('?handler=Update', {
            method: 'POST',
            body: formData
        });
        const result = await response.json();

        if (result.success) {
            sessionStorage.setItem('notification', JSON.stringify({
                type: 'success',
                message: 'Tag updated successfully'
            }));
            location.reload();
        } else {
            toastr.error('Failed to update tag');
        }
    } catch (error) {
        console.error('Error updating category:', error);
        toastr.error('An error occurred while updating');
    }
});
