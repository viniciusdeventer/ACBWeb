const preview = document.getElementById('preview');
const input = document.getElementById('ImagemUpload');
const placeholder = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAMgAAADICAYAAACtWK6eAAAAFUlEQVR4nO3BMQEAAAgDoJvcf+FDMwAAAAAAAAAAwEsCdgAAXwscxQAAAABJRU5ErkJggg==";
function previewImagem(inputFile) {
    const preview = document.getElementById('preview');
    if (inputFile.files && inputFile.files[0] && preview) {
        const reader = new FileReader();
        reader.onload = function (e) {
            preview.src = e.target.result;
        };
        reader.readAsDataURL(inputFile.files[0]);
    } else {
        limparImagem();
    }
}

function limparImagem() {
    const preview = document.getElementById('preview');
    const input = document.getElementById('ImagemUpload');

    if (preview && input) {
        preview.src = placeholder;
        input.value = "";
    }
}
function abrirModalProduto(idProduto) {
    fetch(`/Produto/BuscarPorId/${idProduto}`)
        .then(response => response.text())
        .then(html => {
            document.querySelector('#modal .modal-body').innerHTML = html;
            var modal = new bootstrap.Modal(document.getElementById('modal'));
            modal.show();
        });
}

document.addEventListener('DOMContentLoaded', function () {
    var input = document.getElementById('ValorProduto');
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

let modalLimpo;

document.addEventListener('DOMContentLoaded', function () {
    const modal = document.getElementById('modal');
    const modalBody = modal.querySelector('.modal-body');

    modalLimpo = modalBody.innerHTML;

    modal.addEventListener('hidden.bs.modal', function () {
        modalBody.innerHTML = modalLimpo;
    });
});

document.addEventListener("DOMContentLoaded", function () {
    const btnPesquisar = document.getElementById("btn-pesquisar");
    const termoInput = document.getElementById("pesquisa-termo");
    const produtosContainer = document.getElementById("lista-produtos") || document.getElementById("produtos-container");

    function carregarPagina(pagina) {
        const termo = termoInput.value;
        fetch(`/Produto/Pesquisar?termo=${encodeURIComponent(termo)}&pagina=${pagina}`)
            .then(r => r.text())
            .then(html => {
                produtosContainer.innerHTML = html;
                adicionarListenersPaginacao(); 
            });
    }

    function adicionarListenersPaginacao() {
        const links = produtosContainer.querySelectorAll(".page-link");
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

let produtosSelecionados = new Set();

document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll('.select').forEach(card => {
        card.addEventListener('click', function (e) {
            // Ignora duplo clique
            if (e.detail === 2) return;

            const id = this.getAttribute('data-id');
            if (produtosSelecionados.has(id)) {
                produtosSelecionados.delete(id);
                this.classList.remove('selected');
            } else {
                produtosSelecionados.add(id);
                this.classList.add('selected');
            }
        });
    });

    document.getElementById("btnExcluir").addEventListener("click", function () {
        if (produtosSelecionados.size === 0) {
            alert("Selecione ao menos um produto para excluir.");
            return;
        }

        if (!confirm("Tem certeza que deseja excluir os produtos selecionados?")) return;

        fetch('/Produto/Excluir', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('[name="__RequestVerificationToken"]').value
            },
            body: JSON.stringify(Array.from(produtosSelecionados))
        })
            .then(response => {
                if (response.ok) {
                    location.reload();
                } else {
                    alert("Erro ao excluir produtos.");
                }
            });
    });
});