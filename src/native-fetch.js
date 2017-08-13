const electron = require("electron");
const path = require('path')
let edge = require('electron-edge');


module.exports = class NativeFetch {
    // ..and an (optional) custom class constructor. If one is
    // not supplied, a default constructor is used instead:
    // constructor() { }
    constructor(rootFolder) {
        let self = this;
        self.routeRecords = {}
        self.fetchFunc = edge.func({
            assemblyFile: path.join(rootFolder, '\\MEF\\Hello.Core\\Hello\\bin\\Debug\\hello.dll'),
            typeName: 'Hello.Fetch'
        });
        self.nativeFetchPromise = (url, init) => {
            let myPromise = new Promise((resolve, reject) => {
                try {
                    let nativeInit = { url: url };
                    Object.assign(nativeInit, init);
                    this.fetchFunc(nativeInit, function(error, result) {
                        if (error) {
                            var response = { value: null, statusCode: 404, statusMessage: error.message };
                            resolve(response);
                        } else {
                            console.log(result);
                            resolve(result);
                        }
                    });
                } catch (e) {
                    reject(e.message);
                }
            });
            return myPromise;
        }
        self.localFetch = (url, init) => {
            try {
                let arr = url.split("//");
                let protocol = arr[0];
                let route = arr[1];
                let nodeRouteRecord = self.routeRecords[route];
                if (nodeRouteRecord == null) {
                    return self.nativeFetchPromise(url, init);
                } else {
                    let myPromise = new Promise((resolve, reject) => {
                        let action = nodeRouteRecord[init.method];
                        let response = { value: null, statusCode: 404, statusMessage: 'Not Found' };
                        if (action == null) {
                            Object.assign(response, { value: null, statusCode: 404, statusMessage: error.message });
                        } else {
                            switch (init.method) {
                                case 'GET':
                                    let data = action(init);
                                    Object.assign(response, { value: data, statusCode: 200, statusMessage: 'Success' });
                                    break;
                                case 'POST':
                                    action(init);
                                    Object.assign(response, { value: null, statusCode: 200, statusMessage: 'Success' });
                                    break;
                                default:
                                    Object.assign(response, { value: null, statusCode: 404, statusMessage: error.message });
                                    break;
                            }
                        }
                        resolve(response);
                    });
                    return myPromise;
                }
            } catch (e) {
                throw e;
            }
        }
        self.addRoutes = (records) => {
            Object.assign(self.routeRecords, records);
        }
    }
}