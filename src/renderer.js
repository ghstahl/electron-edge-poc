// This file is required by the index.html file and will
// be executed in the renderer process for that window.
// All of the Node.js APIs are available in this process.
const remote = require('electron').remote;
const loadJsonFile = require('load-json-file');

const app = remote.app;
window.helloWorld = app.helloWorld;

console.log('helloWorld', app.helloWorld);
console.log('localFetch', app.localFetch);

let heartBeat = {
    heart: (data, callback) => {
        var hb = heartBeat;
        console.log(data);
        callback(null, data.key);
    }
}

window.boundAsync = {
    fetchLocalJson: (url) => {
        return loadJsonFile(url);
    },
    localFetch: app.localFetch
}

app.localFetch({
    url: 'local://v1/command-source/register-heart',
    method: 'POST',
    headers: {
        'Content-Type': 'application/json',
        'X-Symc-Fetch-App-Version': '1.0'
    },
    body: heartBeat
}, function(error, result) {
    if (error) throw error;
    console.log(result);
});