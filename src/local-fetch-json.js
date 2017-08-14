const loadJsonFile = require('load-json-file');

function loadJsonFilePromise(init) {
    let query = init.body;
    return loadJsonFile(query.url);
}

var localFetchJson = {}

localFetchJson.routes = {
    'v1/local-json/load': {
        'GET': { promise: loadJsonFilePromise }
    }
}
module.exports = localFetchJson