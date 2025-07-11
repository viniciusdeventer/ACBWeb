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