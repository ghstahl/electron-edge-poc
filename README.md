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
