# SelectelSharp
### Selectel Cloud Storage .NET SDK 

SelectelSharp is .net SDK for Selectel Cloud Storage written on C# in Async style.
At this moment most of API methods are realised, but some of them are still in development.

For more information see: 
[Selectel.com](https://selectel.com/)

## Basic usage

**Create client**

Everything is starts with SelectelClient initialization:

```cs
var client = new SelectelClient();
```

If you working behind a network Proxy, you should pass proxy parameters to the constructor, something like that:

```cs
var client = new SelectelClient("myproxy.com:8080", "domain\\whoami", "pa$$w0rd");
```

**Authorize it**

Almost every method in API needs _token_ to perform I/O operations under your storage.
You should call AuthorizeAsync method to obtain it. Pass your client id and storage password to this method.

```cs
await client.AuthorizeAsync("clientKey", "pa$$w0rd");
```
If authorization was successful, client will recieve authrization token. In other case it will throw WebException.

**Call API methods**

Now you could could Api methods, forexample:
```cs
var result = await client.CreateContainerAsync("new-container");
```

## Realized client methods

**Container methods**
* GetContainersListAsync
* GetContainerInfoAsync
* GetContainerFilesAsync
* CreateContainerAsync
* SetContainerMetaAsync
* SetContainerToGalleryAsync
* DeleteContainerAsync

**File methods**
* GetFileAsync
* UploadFileAsync
* SetFileMetaAsync
* DeleteFileAsync

**Archive methods**
* UploadArchiveAsync

_More methods coming soon..._
