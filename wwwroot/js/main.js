// Captura o caminho da URL atual (ex: "/clientes.html")
const currentPath = window.location.pathname;

// Seleciona todos os itens de menu
document.querySelectorAll('.menu-item a').forEach(link => {
    if (link.getAttribute('href') === currentPath) {
    link.parentElement.classList.add('active');
    }
});
