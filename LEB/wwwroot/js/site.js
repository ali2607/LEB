function closeModal() {
    const modalElement = document.getElementById('myModal');
    const modalInstance = bootstrap.Modal.getInstance(modalElement);
    if (modalInstance) {
        modalInstance.hide();
    }
}

window.OpenModalWithAnimation = (modalId) => {
    var modalElement = document.querySelector(modalId);

    if (!modalElement) return; 

    var modal = new bootstrap.Modal(modalElement, {
        backdrop: true,
        keyboard: true
    });

    $(modalElement).removeClass('fade').addClass('fade');
    modal.show();
};

window.CloseModal = (modalId) => {
    var modalElement = document.querySelector(modalId);
    if (modalElement) {
        var modal = bootstrap.Modal.getInstance(modalElement);
        if (modal) {
            modal.hide();
        }
    }
}


