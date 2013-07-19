#load sponge configuration assembly
[System.Reflection.Assembly]::LoadWithPartialName('Sponge.Common.Configuration');
write-host "Songe Configuration has been loaded."

$applicationName = "Sponge Powershell App";

#create sponge application
[Sponge.Common.Configuration.ConfigurationManager]::CreateApplication($applicationName);
write-host "Application '$applicationName' created"

[Sponge.Common.Configuration.ConfigurationManager]::Set($applicationName, "SampleKey1", 4);
[Sponge.Common.Configuration.ConfigurationManager]::Set($applicationName, "SampleKey2", [guid]::NewGuid());
[Sponge.Common.Configuration.ConfigurationManager]::Set($applicationName, "SampleKey3", 4.33);
[Sponge.Common.Configuration.ConfigurationManager]::Set($applicationName, "SampleKey4", "admin@int32.at");
[Sponge.Common.Configuration.ConfigurationManager]::Set($applicationName, "SampleKey5", "5431235§$%");

write-host "Test Entries added."