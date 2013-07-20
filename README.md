Sponge
======

Sponge SharePoint Configuration &amp; Logging Framework.

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
Obviously, Sponge has to be installed on your SharePoint Farm first. See the Installation part of this readme! If this has been done, simply reference the Sponge.Common.dll from the GAC and include the same using to your Code file.

```c#
public void CreateAppServerSide()
{
    var app = "Sponge App";
    ConfigurationManager.CreateApplication(app);
}
```
#####Client Side
Same as the Server Side, Sponge has to be installed on your SharePoint Farm first. To use the Client Side API you have to reference the Sponge.Common.Configuration.dll and then add the same to your usings!

```c#
 public void CreateAppClientSide()
{
  var app = "Sponge App";
	using (var cfg = new Config("http://mysp"))
	{
		cfg.CreateApplication(app);
	}
}
```

###Logging Framework
Not included in this version - work in progress! Check out the develop branch!
