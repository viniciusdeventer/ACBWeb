const modalVendaEl = document.getElementById('modalVenda');

modalVendaEl.addEventListener('shown.bs.modal', () => {
    const $select = $('#IdItem');

    if ($select.length === 0) return;

    if ($select.hasClass('select2-hidden-accessible')) {
        $select.select2('destroy');
    }

    $select.select2({
        dropdownParent: $('#modalVenda .modal-content'),
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
            data: (params) => ({ term: params.term }),
            processResults: (data) => {
                return { results: data };
            },
            cache: true
        },
        minimumInputLength: 1
    });

    $select.on('select2:select', function (e) {
        const idItem = e.params.data.id;
        fetch(`/Produto/BuscarProdutoJson?id=${idItem}`)
            .then(resp => resp.json())
            .then(data => {
                if (data.sucesso) {
                    const valorStr = data.valorProduto.toFixed(2).replace('.', ',');
                    const inputValor = document.querySelector('input[name="ValorVenda"]');
                    if (inputValor) {
                        inputValor.value = valorStr;
                    }
                }
            })
            .catch(console.error);
    });

    const dataPag = document.getElementById('DataVenda');
    if (dataPag && !dataPag.value) {
        const d = new Date();
        const localDate = new Date(d.getTime() - d.getTimezoneOffset() * 60000).toISOString().slice(0, 10);
        dataPag.value = localDate;
    }
});

function abrirModalVenda(idVenda) {
    fetch(`/Venda/BuscarPorId/${idVenda}`)
        .then(response => response.text())
        .then(html => {
            const modalEl = document.getElementById('modalVenda');
            const modalBody = modalEl.querySelector('.modal-body');
            modalBody.innerHTML = html;

            const modal = new bootstrap.Modal(modalEl);
            modal.show();

            const $select = $('#IdItem');

            if ($select.hasClass('select2-hidden-accessible')) {
                $select.select2('destroy');
            }

            $select.select2({
                dropdownParent: $('#modalVenda .modal-content'),
                placeholder: 'Selecione um produto',
                allowClear: true,
                language: {
                    noResults: () => "Nenhum produto encontrado",
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
                            id: item.idItem,
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

let modalVendaLimpo;

document.addEventListener('DOMContentLoaded', () => {
    const modalEl = document.getElementById('modalVenda');
    if (!modalEl) return;

    const modalBody = modalEl.querySelector('.modal-body');
    modalVendaLimpo = modalBody.innerHTML;

    modalEl.addEventListener('hidden.bs.modal', () => {
        modalBody.innerHTML = modalVendaLimpo;
    });
});

document.addEventListener('submit', (e) => {
    const form = e.target;
    if (form.id === 'formVenda') {
        e.preventDefault();

        const formData = new FormData(form);
        fetch('/Venda/Salvar', {
            method: 'POST',
            body: formData
        })
            .then(response => {
                if (!response.ok) throw new Error('Erro ao salvar');
                return response.text();
            })
            .then(() => {
                const modalEl = document.getElementById('modalVenda');
                bootstrap.Modal.getInstance(modalEl)?.hide();

                const vendaForm = document.getElementById('formVenda');
                const idVenda = vendaForm.querySelector('[name="IdVenda"]').value;

                return fetch(`/Venda/Index`);
            })
            .then(response => response.text())
            .then(html => {
                const parser = new DOMParser();
                const doc = parser.parseFromString(html, 'text/html');
                const novaTabela = doc.querySelector('#tableVendas');
                if (novaTabela) {
                    document.querySelector('#tableVendas').innerHTML = novaTabela.innerHTML;
                }
            })
            .catch(console.error);
    }
});

document.addEventListener('DOMContentLoaded', function () {
    var input = document.getElementById('ValorVenda');
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

function excluirVenda(idVenda) {
    fetch(`/Venda/Excluir/${idVenda}`, { method: 'POST' })
        .then(response => {
            if (!response.ok) throw new Error("Erro ao excluir");
            return response.text();
        })
        .then(() => fetch(`/Venda/Index`))
        .then(response => response.text())
        .then(html => {
            const parser = new DOMParser();
            const doc = parser.parseFromString(html, 'text/html');
            const novaTabela = doc.querySelector('#tableVendas');
            if (novaTabela) {
                document.querySelector('#tableVendas').innerHTML = novaTabela.innerHTML;
            }

            const modalEl = document.getElementById('modalVenda');
            bootstrap.Modal.getInstance(modalEl)?.hide();
        })
        .catch(console.error);
}

document.addEventListener("DOMContentLoaded", function () {
    var calendarEl = document.getElementById('calendar');
    var calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth',
        locale: 'pt-br',
        headerToolbar: {
            left: 'prev,next',
        },
        events: '/Venda/Index',
        dateClick: function (info) {
            atualizarVendasPorData(info.dateStr);
            calendar.select(info.date);
        }
    });

    calendar.render();

    const hoje = new Date().toISOString().slice(0, 10);
    atualizarVendasPorData(hoje);
    calendar.select(new Date()); 
});

function atualizarVendasPorData(dataISO) {
    const url = `/Venda/Index?data=${dataISO}&pagina=1`; 
    fetch(url)
        .then(resp => resp.text())
        .then(html => {
            const parser = new DOMParser();
            const doc = parser.parseFromString(html, 'text/html');
            const novaTabela = doc.querySelector('#tableVendas');
            if (novaTabela) {
                document.querySelector('#tableVendas').innerHTML = novaTabela.innerHTML;
            }

            const tituloData = doc.querySelector('h4.text-warning');
            if (tituloData) {
                document.querySelector('h4.text-warning').innerHTML = tituloData.innerHTML;
            }
        })
        .catch(console.error);
}

document.addEventListener("DOMContentLoaded", function () {
    const vendasContainer = document.getElementById("tableVendas");

    function carregarPagina(pagina) {
        const dataSelecionada = document.querySelector('h4.text-warning')?.dataset?.data || new Date().toISOString().slice(0, 10);
        fetch(`/Venda/Index?pagina=${pagina}&data=${dataSelecionada}`)
            .then(r => r.text())
            .then(html => {
                const parser = new DOMParser();
                const doc = parser.parseFromString(html, "text/html");
                const novaTabela = doc.querySelector("#tableVendas");
                if (novaTabela) {
                    vendasContainer.innerHTML = novaTabela.innerHTML;
                    adicionarListenersPaginacao();
                }
            });
    }

    function adicionarListenersPaginacao() {
        const links = vendasContainer.querySelectorAll(".page-link");
        links.forEach(link => {
            link.addEventListener("click", function (e) {
                e.preventDefault();
                const pagina = this.getAttribute("data-pagina");
                carregarPagina(pagina);
            });
        });
    }

    adicionarListenersPaginacao();
});

