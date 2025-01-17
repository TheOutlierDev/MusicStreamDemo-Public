module.exports = {
  mode: 'jit',
  content: [
    "./Pages/**/*.{razor,cshtml}",
    "./Components/**/*.{razor,cshtml}",
    "./wwwroot/index.html",
    "./**/*.{razor,html,cshtml}"
  ],
  theme: {
    extend: {},
  },
  plugins: [],
};
