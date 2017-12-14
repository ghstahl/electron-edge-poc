# electron-edge-poc

## Requirements 
The electron-edge artifact was built using node 7.4, so you will need to have that installed.
[node 7.4 downloads](https://nodejs.org/download/release/v7.4.0/)

Visual Studio 2017
This is what I am using.
```
Microsoft Visual Studio Enterprise 2017 
Version 15.4.4
VisualStudio.15.Release/15.4.4+27004.2009
Microsoft .NET Framework
Version 4.7.02556
```

### Electron
```
cd .\electron-edge\poc\src  
npm install
```
### Native Apis


```
.\electron-edge-poc\src\MEF\Hello.Core\Hello.Core.sln
build the Hello project.

```

### Entry Point
[C# Fetch](./src/MEF/Hello.Core/Hello/Fetch.cs)  
This is where you will set your debug breakpoint  

This is a MEF Based api router that has the same feel as any web app REST router.  It is actually a reporposed console app command router.  

The following is a project that reads a json file and returns the data;  
[Command.FileLoader](./src/MEF/Hello.Core/Command.FileLoader)  


[Javascript on the Node side](./src/native-fetch.js)  
Since fetch has 2 arguments and native fetch only has one, this javascript exposes to the render.js side a nativeFetch implementation with 2 arguments.  

### Usage
1. From Node  
[Node side](./src/main.js)  
```
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
```

[Render side](./src/home.tag)  This is a riot tag.  

```
self.registerHeartBeat = () => {
        if(self.registrationResult == null){
            window.boundAsync.localFetch('local://v1/command-source/register-heart',
                {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'X-Symc-Fetch-App-Version': '1.0'
                    },
                    body: self.heartBeat
                }, function(error, result) {
                    if (error) throw error;
                    console.log(result);
                    self.registrationResult = result;
                });
        }
  	};
```
# Running
1. Make sure that the Hello project is built
2. run electron.
```
cd .\electron-edge\poc\src  
npm run start
```
3. Attach the debugger to a running process  
Here you will look for electron "Hello World"  

4. In Electron you can reload the page by using the "View" menu.  
