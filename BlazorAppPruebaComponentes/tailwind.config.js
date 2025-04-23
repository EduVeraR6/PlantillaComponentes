/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        './Pages/**/*.{razor,cshtml}',
        './Components/**/*.{razor,cshtml}',
        '../PlantillaComponents/Components/**/*.{razor,cshtml}', // ruta biblioteca componentes
    ],
    theme: {
        extend: {},
    },
    plugins: [],
}
