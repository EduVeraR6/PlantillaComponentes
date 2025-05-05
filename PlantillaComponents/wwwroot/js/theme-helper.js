window.setThemeClass = function (theme) {
    const root = document.documentElement;
    if (theme === 'dark') {
        root.classList.add('dark');
        root.classList.remove('light');
    } else {
        root.classList.add('light');
        root.classList.remove('dark');
    }
}

function MostrarSpinner() {

    const loadingElement = document.getElementById('app');
    loadingElement.classList.remove('hidden');

}

function OcultarSpinner() {
    const loadingElement = document.getElementById('app');
    loadingElement.classList.add('hidden');
}
