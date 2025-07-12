const preview = document.getElementById('preview');
const input = document.getElementById('ImagemUpload');
const placeholder = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAMgAAADICAYAAACtWK6eAAAAFUlEQVR4nO3BMQEAAAgDoJvcf+FDMwAAAAAAAAAAwEsCdgAAXwscxQAAAABJRU5ErkJggg==";

function previewImagem(inputFile) {
    if (inputFile.files && inputFile.files[0]) {
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
    preview.src = placeholder;
    input.value = "";
}
function abrirModalProduto(idProduto) {
    fetch(`/Produto/Buscar/${idProduto}`)
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