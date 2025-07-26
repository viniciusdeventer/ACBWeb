function abrirModalConta(idConta) {
    idContaSelecionado = idConta;
    fetch(`/Conta/BuscarContaPorId/${idConta}`)
        .then(response => response.text())
        .then(html => {
            document.querySelector('#modalConta .modal-body').innerHTML = html;

            setTimeout(() => {
                const btnConfirmar = document.getElementById('btnConfirmarPagamento');
                if (btnConfirmar) {
                    btnConfirmar.addEventListener('click', function () {
                        const situacao = document.getElementById('Situacao');
                        if (situacao) situacao.value = '1';

                        const confirmModal = bootstrap.Modal.getInstance(document.getElementById('confirmPagamentoModal'));
                        confirmModal.hide();

                        const form = document.getElementById('formConta');
                        const formData = new FormData(form);

                        fetch('/Conta/Salvar', { method: 'POST', body: formData })
                            .then(r => {
                                if (!r.ok) throw new Error('Erro ao salvar');
                                return r.text();
                            })
                            .then(() => {
                                const modalElement = document.getElementById('modalConta');
                                const modal = bootstrap.Modal.getInstance(modalElement);
                                modal.hide();

                                const clienteForm = document.getElementById('formCliente');
                                const idCliente = clienteForm.querySelector('[name="IdCliente"]').value;
                                return fetch(`/Cliente/BuscarPorId?id=${encodeURIComponent(idCliente)}`);
                            })
                            .then(r => r.text())
                            .then(html => {
                                document.querySelector('#formCliente').outerHTML = html;
                            })
                            .catch(err => console.error(err));
                    });
                }
            }, 0); 

            var modal = new bootstrap.Modal(document.getElementById('modalConta'));
            modal.show();
            pagamentoLiberado = false;
        });
}


let modalContaLimpo;
let idContaSelecionado = null;

document.addEventListener('DOMContentLoaded', function () {
    const modal = document.getElementById('modalConta');
    if (!modal) return;

    const modalBody = modal.querySelector('.modal-body');
    modalContaLimpo = modalBody.innerHTML;

    modal.addEventListener('hidden.bs.modal', function (e) {
        if (e.target.id === 'modalConta') {
            modalBody.innerHTML = modalContaLimpo;
        }
    });
});

document.addEventListener('shown.bs.modal', function (event) {
    const modal = event.target;

    if (modal.id === 'modalConta' && idClienteSelecionado) {
        const inputId = modal.querySelector('input[name="IdCliente"]');
        if (inputId) inputId.value = idClienteSelecionado;
    }
});

let pagamentoLiberado = false;

function habilitarCampos() {
    pagamentoLiberado = true;
    ['ValorPagamento', 'ObservacaoPagamento', 'DataPagamento'].forEach(id => {
        const el = document.getElementById(id);
        if (!el) return;
        el.removeAttribute('readonly');
        el.removeAttribute('tabindex');
        el.removeAttribute('onfocus');
    });
    const dataPag = document.getElementById('DataPagamento');
    if (dataPag && !dataPag.value) {
        const d = new Date();
        const localDate = new Date(d.getTime() - d.getTimezoneOffset() * 60000).toISOString().slice(0, 10);
        dataPag.value = localDate;
    }
    document.getElementById('ValorPagamento')?.focus();
}

document.addEventListener('submit', function (e) {
    const form = e.target;
    if (form.id !== 'formConta') return;

    const situacao = document.getElementById('Situacao');
    if (pagamentoLiberado === true && form.querySelector(':focus')?.type === 'submit' && situacao && situacao.value === '0') {
        e.preventDefault();
        const confirmModal = new bootstrap.Modal(document.getElementById('confirmPagamentoModal'));
        confirmModal.show();
        return;
    }

    e.preventDefault();
    const formData = new FormData(form);
    fetch('/Conta/Salvar', { method: 'POST', body: formData })
        .then(r => {
            if (!r.ok) throw new Error('Erro ao salvar');
            return r.text();
        })
        .then(() => {
            const modalElement = document.getElementById('modalConta');
            const modal = bootstrap.Modal.getInstance(modalElement);
            modal.hide();
            const clienteForm = document.getElementById('formCliente');
            const idCliente = clienteForm.querySelector('[name="IdCliente"]').value;
            return fetch(`/Cliente/BuscarPorId?id=${encodeURIComponent(idCliente)}`);
        })
        .then(r => r.text())
        .then(html => {
            document.querySelector('#formCliente').outerHTML = html;
            setTimeout(initSituacaoChange, 0);
        })
        .catch(err => console.error(err));
});

document.getElementById('btnConfirmarPagamento')?.addEventListener('click', function () {
    const situacao = document.getElementById('Situacao');
    if (situacao) situacao.value = '1';

    const confirmModal = bootstrap.Modal.getInstance(document.getElementById('confirmPagamentoModal'));
    confirmModal.hide();

    const form = document.getElementById('formConta');
    const formData = new FormData(form);

    fetch('/Conta/Salvar', { method: 'POST', body: formData })
        .then(r => {
            if (!r.ok) throw new Error('Erro ao salvar');
            return r.text();
        })
        .then(() => {
            const modalElement = document.getElementById('modalConta');
            const modal = bootstrap.Modal.getInstance(modalElement);
            modal.hide();

            const clienteForm = document.getElementById('formCliente');
            const idCliente = clienteForm.querySelector('[name="IdCliente"]').value;
            return fetch(`/Cliente/BuscarPorId?id=${encodeURIComponent(idCliente)}`);
        })
        .then(r => r.text())
        .then(html => {
            document.querySelector('#formCliente').outerHTML = html;
            initSituacaoChange();
        })
        .catch(err => console.error(err));
});

document.addEventListener('DOMContentLoaded', function () {
    var input = document.getElementById('ValorPagamento');
    if (input) {
        input.addEventListener('input', function (e) {
            let v = e.target.value.replace(/\D/g, '');
            v = (parseInt(v, 10) / 100).toFixed(2) + '';
            v = v.replace('.', ',');
            v = v.replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1.');
            e.target.value = v;
        });
    }
});

function parseValorProduto(valorStr) {
    valorStr = valorStr.replace(".", "").replace(",", ".");
    return parseFloat(valorStr);
}
function excluirConta(idConta) {
    fetch(`/Conta/Excluir/${idConta}`, {
        method: 'POST'
    })
        .then(response => {
            if (!response.ok) throw new Error("Erro ao excluir");
            return response.text();
        })
        .then(() => {
            const modalEl = document.getElementById('modalConta');
            const modal = bootstrap.Modal.getInstance(modalEl);
            modal.hide();

            const contaForm = document.getElementById('formCliente');
            const idCliente = contaForm.querySelector('[name="IdCliente"]').value;
            return fetch(`/Cliente/BuscarPorId?id=${encodeURIComponent(idCliente)}`);
        })
        .then(response => response.text())
        .then(html => {
            document.querySelector('#formCliente').outerHTML = html;
            initSituacaoChange();
        })
        .catch(console.error);
}

document.addEventListener("click", function (e) {
    const link = e.target.closest("#contas-container .page-link");
    if (!link) return;

    e.preventDefault();

    const pagina = link.getAttribute("data-pagina");
    const idClientenput = document.querySelector('#modalCliente [name="IdCliente"]');
    if (!idClientenput) return;

    const idCliente = idClientenput.value;

    fetch(`/Cliente/BuscarPorId?id=${encodeURIComponent(idCliente)}&pagina=${pagina}`)
        .then(r => {
            if (!r.ok) throw new Error("Erro ao paginar contas");
            return r.text();
        })
        .then(html => {
            const formCliente = document.getElementById("formCliente");
            if (formCliente) formCliente.outerHTML = html;
            initSituacaoChange();
        })
        .catch(console.error);
});
