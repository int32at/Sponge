Sponge
======

Sponge SharePoint Configuration &amp; Logging Framework.

The Idea behind Sponge is pretty simple - A consistant, centralized and easy to configurable Configuration and
Logging component based on top of SharePoint. Every developer has asked him/her-self the same questions:

- Where should i put the configuration of my application?
- Where should i log my exceptions to?

With Sponge i try to make this tasks very simple and straight forward. Let me explain.


####Configuration Framework
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



####Logging Framework
Not included in this version - work in progress! Check out the develop branch!
