// This file is required by the index.html file and will
// be executed in the renderer process for that window.
// All of the Node.js APIs are available in this process.
const remote = require('electron').remote;
const loadJsonFile = require('load-json-file');

const app = remote.app;
window.helloWorld = app.helloWorld;

console.log('helloWorld', app.helloWorld);
console.log('localFetch', app.localFetch);



window.boundAsync = {
    fetchLocalJson: (url) => {
        let myFirstPromise = new Promise((resolve, reject) => {
            loadJsonFile(url).then((data) => {
                var response = { value: data, statusCode: 200, statusMessage: "OK" };
                resolve(response);
            }).catch((e) => {
                console.log(e);
                var response = { value: null, statusCode: 404, statusMessage: e.message };
                resolve(response);
            });
        });
        return myFirstPromise;

        //        return loadJsonFile(url);
    },
    localFetch: app.localFetch
}
window.boundAsync.fetchLocalJson('file://does-not-exit.json').then(function(data) {
    console.log(data);
}).catch((e) => {
    console.log(e);
});
window.boundAsync.fetchLocalJson('config.json').then(function(data) {
    console.log(data);
}).catch((e) => {
    console.log(e);
});