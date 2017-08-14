const electron = require('electron')
const downloadManager = require('./download-manager')
const localFetchJson = require('./local-fetch-json')
const NativeFetch = require('./native-fetch')
var edge = require('electron-edge');

// Module to control application life.
const app = electron.app
    // Module to create native browser window.
const BrowserWindow = electron.BrowserWindow

const path = require('path')
const url = require('url')

// Keep a global reference of the window object, if you don't, the window will
// be closed automatically when the JavaScript object is garbage collected.
let mainWindow
let heartBeat = {
    heart: (data, callback) => {
        console.log(data);
        callback(null, data.key);
    }
}

let nativeFetch = new NativeFetch(__dirname);

downloadManager.initializeDownloadManagerSync(app.getPath('userData'));
nativeFetch.addRoutes(downloadManager.routes);
nativeFetch.addRoutes(localFetchJson.routes);

app.localFetch = nativeFetch.localFetch;

function createWindow() {
    // Create the browser window.
    mainWindow = new BrowserWindow({ width: 800, height: 600 })



    // and load the index.html of the app.
    mainWindow.loadURL(url.format({
        pathname: path.join(__dirname, 'index.html'),
        protocol: 'file:',
        slashes: true
    }))

    var helloWorld = edge.func({
        assemblyFile: path.join(__dirname, '\\MEF\\Hello.Core\\Hello\\bin\\Debug\\hello.dll'),
        typeName: 'Hello.Startup'
    });


    helloWorld({
        url: 'local://v1/programs/is-installed',
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'X-Symc-Fetch-App-Version': '1.0'
        },
        body: {
            displayName: 'Norton Internet Security'
        }
    }, function(error, result) {
        if (error) throw error;
        console.log(result);
    });


    var payload = {
        a: 2,
        b: 3,
        add: function(data, callback) {
            callback(null, data.a + data.b);
        }
    };
    /*
    app.localFetch('local://v1/command-source/immediate-callback', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'X-Symc-Fetch-App-Version': '1.0'
        },
        body: payload
    }, function(error, result) {
        if (error) throw error;
        console.log(result);
    });
*/


    app.localFetch('local://v1/json-file/config', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'X-Symc-Fetch-App-Version': '1.0'
        },
        body: { rootFolder: __dirname }
    }).then((data) => {
        console.log('json-file/config', data);
    }).catch((e) => {
        console.log('json-file/config', e);
    });

    app.localFetch('local://v1/command-source/immediate-callback', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'X-Symc-Fetch-App-Version': '1.0'
        },
        body: payload
    }).then((data) => {
        console.log('immediate-callback', data);
    }).catch((e) => {
        console.log('immediate-callback', e);
    });

    app.localFetch('local://v1/command-source/register-heart', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'X-Symc-Fetch-App-Version': '1.0'
        },
        body: heartBeat
    }).then((data) => {
        console.log('register heartbeat', data);
    }).catch((e) => {
        console.log('register heartbeat', e);
    });

    app.localFetch('local://v1/download-manager/download', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'X-Symc-Fetch-App-Version': '1.0'
        },
        body: {
            key: 'CuVQGg3',
            url: 'http://i.imgur.com/CuVQGg3.jpg'
        }
    }).then((data) => {
        console.log('download', data);
    }).catch((e) => {
        console.log('download', e);
    });
    app.localFetch('local://v1/local-json/load', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'X-Symc-Fetch-App-Version': '1.0'
        },
        body: {
            url: '/config.json'
        }
    }).then((data) => {
        console.log('local-json', data);
    }).catch((e) => {
        console.log('local-json', e);
    });



    app.helloWorld = helloWorld;

    // Open the DevTools.
    // mainWindow.webContents.openDevTools()

    // Emitted when the window is closed.
    mainWindow.on('closed', function() {
        // Dereference the window object, usually you would store windows
        // in an array if your app supports multi windows, this is the time
        // when you should delete the corresponding element.
        mainWindow = null
    })
}

// This method will be called when Electron has finished
// initialization and is ready to create browser windows.
// Some APIs can only be used after this event occurs.
app.on('ready', createWindow)

// Quit when all windows are closed.
app.on('window-all-closed', function() {
    // On OS X it is common for applications and their menu bar
    // to stay active until the user quits explicitly with Cmd + Q
    if (process.platform !== 'darwin') {
        app.quit()
    }
})

app.on('activate', function() {
    // On OS X it's common to re-create a window in the app when the
    // dock icon is clicked and there are no other windows open.
    if (mainWindow === null) {
        createWindow()
    }
})

// In this file you can include the rest of your app's specific main process
// code. You can also put them in separate files and require them here.