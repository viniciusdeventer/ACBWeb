function abrirModalConta(idConta) {
    fetch(`/Conta/BuscarContaPorId/${idConta}`)
        .then(response => response.text())
        .then(html => {
            document.querySelector('#modalConta .modal-body').innerHTML = html;
            var modal = new bootstrap.Modal(document.getElementById('modalConta'));
            modal.show();
        });
}

let modalContaLimpo;

document.addEventListener('DOMContentLoaded', function () {
    const modal = document.getElementById('modalConta');
    if (!modal) return;

    const modalBody = modal.querySelector('.modal-body');
    modalContaLimpo = modalBody.innerHTML;

    modal.addEventListener('hidden.bs.modal', function () {
        modalBody.innerHTML = modalContaLimpo;
    });
});

document.addEventListener('shown.bs.modal', function (event) {
    const modal = event.target;

    if (modal.id === 'modalConta' && idClienteSelecionado) {
        const inputId = modal.querySelector('input[name="IdCliente"]');
        if (inputId) inputId.value = idClienteSelecionado;
    }
});

document.addEventListener('submit', function (e) {
    const form = e.target;

    if (form.id === 'formConta') {
        e.preventDefault();

        const formData = new FormData(form);

        fetch('/Conta/Salvar', {
            method: 'POST',
            body: formData
        })
            .then(response => response.text())
            .then(html => {
                const modalElement = document.getElementById('modalConta');
                const modal = bootstrap.Modal.getInstance(modalElement);
                modal.hide();
            })
            .catch(error => {
                console.error('Erro ao salvar a conta:', error);
            });
    }
});