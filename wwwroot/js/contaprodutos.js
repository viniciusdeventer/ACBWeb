function abrirModalContaProdutos(idItem) {
    fetch(`/ContaProdutos/BuscarContaProdutosPorId/${idItem}`)
        .then(response => response.text())
        .then(html => {
            document.querySelector('#modalContaProdutos .modal-body').innerHTML = html;
            var modal = new bootstrap.Modal(document.getElementById('modalContaProdutos'));
            modal.show();

            // 🔧 Inicializa Select2 após inserir o HTML
            $('#IdProduto').select2({
                placeholder: 'Selecione um produto',
                allowClear: true,
                ajax: {
                    url: '/Produto/BuscarProdutosJson',
                    dataType: 'json',
                    delay: 250,
                    data: function (params) {
                        return { term: params.term };
                    },
                    processResults: function (data) {
                        return { results: data };
                    },
                    cache: true
                },
                minimumInputLength: 1
            });

            // 🔧 Reaplica valor se estiver editando
            const valorSelecionado = $('#IdProduto').data('value');
            const textoSelecionado = $('#IdProduto').data('text');

            if (valorSelecionado && textoSelecionado) {
                const option = new Option(textoSelecionado, valorSelecionado, true, true);
                $('#IdProduto').append(option).trigger('change');
            }
        });
}

let modalContaProdutosLimpo;

document.addEventListener('DOMContentLoaded', function () {
    const modal = document.getElementById('modalContaProdutos');
    if (!modal) return;

    const modalBody = modal.querySelector('.modal-body');
    modalContaProdutosLimpo = modalBody.innerHTML;

    modal.addEventListener('hidden.bs.modal', function () {
        modalBody.innerHTML = modalContaProdutosLimpo;
    });
});

document.addEventListener('submit', function (e) {
    const form = e.target;
    if (form.id === 'formContaProdutos') {
        e.preventDefault();
        const formData = new FormData(form);
        fetch('/ContaProdutos/Salvar', { method: 'POST', body: formData })
            .then(r => {
                if (!r.ok) throw new Error('Erro ao salvar');
                return r.text();
            })
            .then(() => {
                const modalElement = document.getElementById('modalContaProdutos');
                const modal = bootstrap.Modal.getInstance(modalElement);
                modal.hide();
                const contaForm = document.getElementById('formConta');
                const idConta = contaForm.querySelector('[name="IdConta"]').value;
                return fetch(`/Conta/BuscarContaPorId?id=${encodeURIComponent(idConta)}`);
            })
            .then(r => r.text())
            .then(html => {
                document.querySelector('#formConta').outerHTML = html;
            })
            .catch(err => console.error(err));
    }
});

$('#IdProduto').select2({
    placeholder: 'Selecione um produto',
    allowClear: true,
    ajax: {
        url: '/Produto/BuscarProdutosJson',
        dataType: 'json',
        delay: 250,
        data: function (params) {
            return { term: params.term };
        },
        processResults: function (data) {
            return { results: data };
        },
        cache: true
    },
    minimumInputLength: 1
});

// Reaplicar opção selecionada (caso edição)
var valorSelecionado = $('#IdProduto').data('value');
var textoSelecionado = $('#IdProduto').data('text');

if (valorSelecionado && textoSelecionado) {
    var option = new Option(textoSelecionado, valorSelecionado, true, true);
    $('#IdProduto').append(option).trigger('change');
}

