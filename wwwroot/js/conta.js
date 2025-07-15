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

