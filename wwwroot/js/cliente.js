function abrirModalCliente(idCliente) {
    idClienteSelecionado = idCliente;
    fetch(`/Cliente/BuscarPorId/${idCliente}`)
        .then(response => response.text())
        .then(html => {
            document.querySelector('#modalCliente .modal-body').innerHTML = html;
            var modal = new bootstrap.Modal(document.getElementById('modalCliente'));
            modal.show();
            initSituacaoChange();
        });
}

function initSituacaoChange() {
    const selectSituacao = document.querySelector('#Situacao');
    if (!selectSituacao) {
        return;
    }

    selectSituacao.addEventListener('change', function () {
        const situacao = this.value;
        fetch(`/Cliente/BuscarPorId/${idClienteSelecionado}?situacao=${situacao}`)
            .then(response => response.text())
            .then(html => {
                document.querySelector('#modalCliente .modal-body').innerHTML = html;
                initSituacaoChange();
            });
    });
}

let modalClienteLimpo;
let idClienteSelecionado = null;

document.addEventListener('DOMContentLoaded', function () {
    const modal = document.getElementById('modalCliente');
    if (!modal) return;

    const modalBody = modal.querySelector('.modal-body');
    modalClienteLimpo = modalBody.innerHTML;

    modal.addEventListener('hidden.bs.modal', function () {
        modalBody.innerHTML = modalClienteLimpo;
    });
});

document.addEventListener("DOMContentLoaded", function () {
    const btnPesquisar = document.getElementById("btn-pesquisar");
    const termoInput = document.getElementById("pesquisa-termo");
    const clientesContainer = document.getElementById("lista-clientes") || document.getElementById("clientes-container");

    function carregarPagina(pagina) {
        const termo = termoInput.value;
        fetch(`/Cliente/Pesquisar?termo=${encodeURIComponent(termo)}&pagina=${pagina}`)
            .then(r => r.text())
            .then(html => {
                clientesContainer.innerHTML = html;
                adicionarListenersPaginacao();
            });
    }

    function adicionarListenersPaginacao() {
        const links = clientesContainer.querySelectorAll(".page-link");
        links.forEach(link => {
            link.addEventListener("click", function (e) {
                e.preventDefault();
                const pagina = this.getAttribute("data-pagina");
                carregarPagina(pagina);
            });
        });
    }

    btnPesquisar.addEventListener("click", () => carregarPagina(1));

    termoInput.addEventListener("keyup", (e) => {
        if (e.key === "Enter") carregarPagina(1);
    });

    adicionarListenersPaginacao();
});

document.addEventListener('DOMContentLoaded', function () {
    const input = document.getElementById('Telefone');

    if (input) {
        input.addEventListener('input', function (e) {
            let v = e.target.value.replace(/\D/g, '');

            if (v.length > 11) v = v.slice(0, 11); 

            if (v.length >= 2 && v.length <= 6)
                v = `(${v.slice(0, 2)}) ${v.slice(2)}`;
            else if (v.length >= 7)
                v = `(${v.slice(0, 2)}) ${v.slice(2, 7)}-${v.slice(7)}`;

            e.target.value = v;
        });
    }
});
