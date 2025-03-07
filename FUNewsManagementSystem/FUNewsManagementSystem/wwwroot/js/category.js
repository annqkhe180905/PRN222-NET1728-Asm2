
var addForm = document.getElementById('addForm');
toastr.options = {
    progressBar: true,
    positionClass: "toast-top-right",
    timeOut: 1000
};

addForm.addEventListener('submit', async (e) => {
    e.preventDefault();
    var formData = new FormData(addForm);
    try {
        const response = await fetch('?handler=Create', {
            method: 'POST',
            body: formData,
        });
        if (response.ok) {
            const result = await response.json();
            if (result.success) {
                sessionStorage.setItem('notification', JSON.stringify({
                    type: 'success',
                    message: 'Thêm category thành công'
                }));
                location.reload();
            } else {
                toastr.error("Thêm category thất bại");
            }
        }
    } catch (error) {
        console.log(error);
    }
})

window.addEventListener('load', () => {
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
    initSearchFilter();
});
function initSearchFilter() {
    const searchInput = document.getElementById('searchInput');
    const rows = document.querySelectorAll('#table-body-category tr');
    searchInput.addEventListener('input', () => {
        const searchTerm = searchInput.value.toLowerCase();
        rows.forEach(row => {
            const name = row.cells[0].textContent.toLowerCase();
            if (name.includes(searchTerm)) {
                row.classList.remove('hidden');
            } else {
                row.classList.add('hidden');
            }
        })
    })

}

let deleteId = null;
const deleteModal = new bootstrap.Modal(document.getElementById('confirmDeleteModal'));
document.querySelectorAll('.btn-delete').forEach(button => {
    button.addEventListener('click', function () {
        deleteId = this.getAttribute('data-id');
        deleteModal.show();
    });
});


document.getElementById('updateCategoryModal').addEventListener('show.bs.modal', function (e) {
    const button = e.relatedTarget;
    const updateId = button.getAttribute('data-id');
    const updateName = button.getAttribute('data-name');
    const updateDescription = button.getAttribute('data-description');
    const updateActive = button.getAttribute('data-active');

    this.querySelector('#updateCategoryId').value = updateId;
    this.querySelector('#updateCategoryName').value = updateName;
    this.querySelector('#updateCategoryDescription').value = updateDescription;
    this.querySelector('#updateCategoryActive').checked = updateActive === 'True';
});

const updateForm = document.querySelector('#updateForm');
updateForm.addEventListener('submit', async (e) => {
    e.preventDefault();
    const formData = new FormData(updateForm);

    try {
        const response = await fetch('?handler=Update', {
            method: 'POST',
            body: formData,
        });
        const result = await response.json();

        if (result.success) {
            sessionStorage.setItem('notification', JSON.stringify({
                type: 'success',
                message: result.message
            }));
            location.reload();
        } else {
            toastr.error(result.message || 'Update failed!');
        }
    } catch (error) {
        console.error(error);
        toastr.error('An error occurred while updating');
    }
});



document.getElementById('confirmDeleteBtn').addEventListener('click', async function () {
    if (deleteId) {
        try {
            const response = await fetch(`?handler=Delete&id=${deleteId}`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            if (!response.ok) {
                throw new Error(`Server returned ${response.status}: ${response.statusText}`);
            }
            const result = await response.json();
            if (result.success) {
                document.querySelector(`i[data-id="${deleteId}"]`).closest('tr').remove();
                deleteModal.hide();
                deleteId = null;
                toastr.success('Xóa category thành công');
            } else {
                toastr.error(result.message || 'Xóa thất bại!');
            }
            deleteModal.hide();
        } catch (error) {
            console.error(error);
            alert('Có lỗi xảy ra khi xóa: ' + error.message);
        }
    }
});
