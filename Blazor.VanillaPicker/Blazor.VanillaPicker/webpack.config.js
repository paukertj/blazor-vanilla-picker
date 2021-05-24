const path = require("path");

module.exports = {
    entry: [
        './JsCompositor.js',
    ],
    module: {
        rules: [
            {
                test: /\.(js|jsx)$/,
                exclude: /node_modules/,
                use: {
                    loader: 'babel-loader'
                }
            },
            {
                test: /\.(css|scss)$/,
                use: ['style-loader', 'css-loader']
            }
        ]
    },
    output: {
        library: 'VanillaPicker',
        path: path.resolve(__dirname, 'wwwroot'),
        filename: 'VanillaPicker.js',
        libraryTarget: 'umd',
        libraryExport: 'default'
    }
};