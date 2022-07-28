const path = require("path");

module.exports = {
    entry: './wwwroot/js/Vue/index.js',
    output: {
        filename: "bundle.js",
        path: path.resolve(__dirname, "wwwroot")
    },
    mode: "development",
    devtool: "source-map",
}