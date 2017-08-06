// This file is required by the index.html file and will
// be executed in the renderer process for that window.
// All of the Node.js APIs are available in this process.
const remote = require('electron').remote;
const loadJsonFile = require('load-json-file');

const app = remote.app;
window.helloWorld = app.helloWorld;
console.log('helloWorld', window.helloWorld);
window.boundAsync = {
    fetchLocalJson: (url) => {
        return loadJsonFile(url);
    }
}