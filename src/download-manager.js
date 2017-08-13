const electron = require("electron");
const path = require('path')
const { app, BrowserWindow } = electron;
const loki = require('lokijs');
const lfsa = require('lokijs/src/loki-fs-structured-adapter');
const DownloadManager = require("electron-download-manager");


let db = null;
let downloadCollection = null;

function databaseInitialize() {
    downloadCollection = db.getCollection("download-collection");

    if (downloadCollection === null) {
        downloadCollection = db.addCollection("download-collection");
    }
}

function initializeDownloadManagerSync(userDataPath) {
    let adapter = new lfsa();
    let userDb = path.join(userDataPath, 'download-manager.db');
    let registerInit = { downloadFolder: path.join(userDataPath, 'download') };
    DownloadManager.register(registerInit);

    db = new loki(userDb, {
        adapter: adapter,
        autoload: true,
        autoloadCallback: databaseInitialize,
        autosave: true,
        autosaveInterval: 4000
    });
    databaseInitialize();
    console.log('downloadManager', db);
}

function postDownload(init) {
    let record = init.body;
    let result = downloadCollection.findOne({ key: record.key });
    if (result == null) {
        record.complete = false;
        downloadCollection.insert(record);
        result = record;
    }
    DownloadManager.download({
        url: record.url,
        onProgress: (progress, item) => {
            let r = result;
            r.progress = progress;
            downloadCollection.update(r);
        }
    }, function(error, url) {
        let r = result;
        if (error) {
            alert("ERROR: " + url);
            r.error = error;
            downloadCollection.update(r);
        } else {
            r.complete = true;
            downloadCollection.update(r);
        }
    });
}

/*
init:{
    headers:[],
    body:{}
}
*/
function getPage(init) {
    let query = init.body;
    let offset = query.offset;
    let count = query.count;
    var data = downloadCollection.data;
    if (offset < 0 || offset >= data.length) {
        return null;
    }
    var subset = data.slice(offset, count);
    return subset;
}

function getOne(init) {
    let query = init.body;
    var result = downloadCollection.findOne({ key: query.key });
    return result;
}

var downloadManager = {
    initializeDownloadManagerSync: initializeDownloadManagerSync
}

downloadManager.routes = {
    'v1/download-manager/page': {
        'GET': getPage
    },
    'v1/download-manager/one': {
        'GET': getOne
    },
    'v1/download-manager/download': {
        'POST': postDownload
    }
}

module.exports = downloadManager