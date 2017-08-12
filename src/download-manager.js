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
    db = new loki(userDb, {
        adapter: adapter,
        autoload: true,
        autoloadCallback: databaseInitialize,
        autosave: true,
        autosaveInterval: 4000
    });
    console.log('downloadManager', db);
}

function download(record) {
    var result = downloadCollection.findOne({ key: record.key });
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
            return;
        }
        r.complete = true;
        downloadCollection.update(r);
    });
}

function page(offset, count) {
    var data = downloadCollection.data;
    if (offset < 0 || offset >= data.length) {
        return null;
    }
    var subset = data.slice(offset, count);
    return subset;
}

var downloadManager = {
    initializeDownloadManagerSync: initializeDownloadManagerSync,
    download: download,
    page: page
}

module.exports = downloadManager