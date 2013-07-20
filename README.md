Sponge (beta)
======

#####Sponge SharePoint Configuration &amp; Logging Framework.

The Idea behind Sponge is pretty simple - A consistant, centralized and easy to configurable Configuration and
Logging component based on top of SharePoint. Every developer has asked him/her-self the same questions:

- Where should i put the configuration of my application?
- Where should i log my exceptions to?

With Sponge i try to make this tasks very simple and straight forward. Let me explain.


###Configuration Framework

####Overview
There are several good and bad ways of storing your configuration in and around SharePoint.

| Method        | Result           |
| ------------- |:-------------:|
| Hard Coded    | very bad - that's like no configuration at all!
| web.config     | better but still very bad at large farms, even when used with SPWebConfigModification 
| SP Lists | ok, but you have to define where to store (Web, Site, WebApp?) it and save that link somehwere...
| SPPersistedObject | ok, but quite complex. you have to create your own object for  every application, and sometimes Update Conflicts in large farms will happen if not done correctly
| Sponge | awesome! stores your configs in the Central Admin and makes it accessible via API and Web Services!

Sponge creates a Site with multiple lists in your Central Administration **http://myadmin/Sponge**:

- **ConfigApplications**: The name's of your applications you want to configure. This gives you a nice grouping of the config entries later on!
- **ConfigItems**: This is where the actual config goes to. Add your Key/Value pairs to your application and you are ready to go!

####Usage
At first, you have to decide which application you want to configure - is it an application that "lives" on SharePoint e.g. WebPart, Timer Job, etc. or is it a client application like WinForms or even an Windows Phone App!

#####Server Side
Obviously, Sponge has to be installed on your SharePoint Farm first. See the Installation part of this readme! If this has been done, simply reference the `Sponge.Common.dll` from the GAC and include the `Sponge.Common.Configuration` Namespace to your usings. `ConfigurationManager` allows you to interact with your config store.

```c#
public void CreateAppServerSide()
{
    //create application
    var app = "Sponge App";
    ConfigurationManager.CreateApplication(app);
	
	//set the MyKey value to 3 in app
	//if exists, it will be updated; if not exists it will be created
	ConfigurationManager.Set(app, "MyKey", 3);
	
	//retreive the value we just created
	var myValue = ConfigurationManager.Get<int>(app, "MyKey");
	
	foreach(var item in ConfigurationManager.GetAll(app))
	{
		Console.WriteLine("{0} - {1}", item.Key, item.Value);
	}
}
```
#####Client Side
Same as the Server Side, Sponge has to be installed on your SharePoint Farm first. To use the Client Side API you have to reference the `Sponge.Common.Configuration.dll` and then add the same to your usings! Your config entries can be managed using the `ClientConfigurationManager` class.

```c#
public void CreateAppClientSide()
{
    var app = "Sponge App";
    
	//init connection to the config service
    using (var cfg = new ClientConfigurationManager("http://mysp"))
    {
		//create app
        cfg.CreateApplication(app);
		
		cfg.Set(app, "MyKey", Guid.NewGuid.ToString());
		
		var myValue = cfg.Get(app, "MyKey");
		
		foreach(var item in cfg.GetAll(app))
		{
			Console.WriteLine("{0} - {1}", item.Key, item.Value);
		}
    }
}
```

That's it. The Client Side API will call the Sponge Config Web Service in order to retrieve values from the config store. Instead of using the `ConfigurationManager` class, the Client Side API uses the `Config` class. If you prefer to use the Config Web Service directly, feel free to do so! It's available at `http://mysp/_layouts/Sponge/ConfigService.asmx`.

####Supported API

- **CreateApplication(appName)**: Creates an entry in the Config Applications list.
- **ApplicationExists(appName)**: Whether an entry exists in the Config Applications list.
- **Get<T>(appName, key)**: Gets an entry for the specified app and key and cast it to T.
- **Get(appName, key)**: Same as Get<T> where T = object, so it can be used i.e. with Powershell.
- **GetAll(appName)**: Gets all Key/Value's for the specified app.
- **Set(appName, key, value)**: Sets the value of the entry for the specified app and key.

All of these functions are available in both the Server and Client Side API!
###Logging Framework
Not included in this version - work in progress! Check out the develop branch!


###Installation

1.  Download `Sponge.wsp` and `install.ps1` in a folder (or clone this repo)
2.  Run `install.ps1` with PowerShell (with the most-privileged account possible)
3.  Browser will open and take you directly to Sponge.
4.  Read the `Usage` Sections on how to interact with Sponge from Code!
