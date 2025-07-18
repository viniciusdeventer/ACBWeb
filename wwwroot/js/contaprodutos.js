const modalContaProdutosEl = document.getElementById('modalContaProdutos');

modalContaProdutosEl.addEventListener('shown.bs.modal', () => {
    const $select = $('#IdProduto');

    if ($select.length === 0) return;

    if ($select.hasClass('select2-hidden-accessible')) {
        $select.select2('destroy');
    }

    $select.select2({
        dropdownParent: $('#modalContaProdutos .modal-content'),
        placeholder: 'Selecione um produto',
        allowClear: true,
        language: {
            noResults: () => "Nenhum produto encontrado...",
            inputTooShort: (args) => `Digite o que gostaria de pesquisar...`,
            searching: () => "Buscando...",
            loadingMore: () => "Carregando mais resultados...",
            errorLoading: () => "Erro ao carregar resultados..."
        },
        ajax: {
            url: '/Produto/BuscarProdutosJson',
            dataType: 'json',
            delay: 250,
            data: (params) => ({ term: params.term }),
            processResults: (data) => {
                console.log('Dados recebidos do servidor:', data);
                return { results: data };
            },
            cache: true
        },
        minimumInputLength: 1
    });

    $select.on('select2:select', function (e) {
        const idProduto = e.params.data.id;
        fetch(`/Produto/BuscarProdutoJson?id=${idProduto}`)
            .then(resp => resp.json())
            .then(data => {
                if (data.sucesso) {
                    const valorStr = data.valorProduto.toFixed(2).replace('.', ',');
                    const inputValor = document.querySelector('input[name="ValorUnitario"]');
                    if (inputValor) {
                        inputValor.value = valorStr;
                    }
                }
            })
            .catch(console.error);
    });
});
function abrirModalContaProdutos(idItem) {
    fetch(`/ContaProdutos/BuscarContaProdutosPorId/${idItem}`)
        .then(response => response.text())
        .then(html => {
            const modalEl = document.getElementById('modalContaProdutos');
            const modalBody = modalEl.querySelector('.modal-body');
            modalBody.innerHTML = html;

            const modal = new bootstrap.Modal(modalEl);
            modal.show();

            const $select = $('#IdProduto');

            if ($select.hasClass('select2-hidden-accessible')) {
                $select.select2('destroy');
            }

            $select.select2({
                dropdownParent: $('#modalContaProdutos .modal-content'),
                placeholder: 'Selecione um produto',
                allowClear: true,
                language: {
                    noResults: () => "Nenhum produto encontrado",
                    inputTooShort: (args) => `Digite ao menos ${args.minimum} caractere(s)`,
                    searching: () => "Buscando...",
                    loadingMore: () => "Carregando mais resultados...",
                    errorLoading: () => "Erro ao carregar resultados"
                },
                ajax: {
                    url: '/Produto/BuscarProdutosJson',
                    dataType: 'json',
                    delay: 250,
                    data: (params) => ({ term: params.term }),
                    processResults: (data) => ({
                        results: data.map(item => ({
                            id: item.idProduto,
                            text: item.nome
                        }))
                    }),
                    cache: true
                },
                minimumInputLength: 1
            });

            const valorSelecionado = $select.data('value');
            const textoSelecionado = $select.data('text');

            if (valorSelecionado && textoSelecionado) {
                const option = new Option(textoSelecionado, valorSelecionado, true, true);
                $select.append(option).trigger('change');
            }
        });
}

let modalContaProdutosLimpo;

document.addEventListener('DOMContentLoaded', () => {
    const modalEl = document.getElementById('modalContaProdutos');
    if (!modalEl) return;

    const modalBody = modalEl.querySelector('.modal-body');
    modalContaProdutosLimpo = modalBody.innerHTML;

    modalEl.addEventListener('hidden.bs.modal', () => {
        modalBody.innerHTML = modalContaProdutosLimpo;
    });
});

document.addEventListener('submit', (e) => {
    const form = e.target;
    if (form.id === 'formContaProdutos') {
        e.preventDefault();

        const formData = new FormData(form);
        fetch('/ContaProdutos/Salvar', {
            method: 'POST',
            body: formData
        })
            .then(response => {
                if (!response.ok) throw new Error('Erro ao salvar');
                return response.text();
            })
            .then(() => {
                const modalEl = document.getElementById('modalContaProdutos');
                const modal = bootstrap.Modal.getInstance(modalEl);
                modal.hide();

                const contaForm = document.getElementById('formConta');
                const idConta = contaForm.querySelector('[name="IdConta"]').value;

                return fetch(`/Conta/BuscarContaPorId?id=${encodeURIComponent(idConta)}`);
            })
            .then(response => response.text())
            .then(html => {
                document.querySelector('#formConta').outerHTML = html;
            })
            .catch(console.error);
    }
});

document.addEventListener('DOMContentLoaded', function () {
    var input = document.getElementById('ValorUnitario');
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