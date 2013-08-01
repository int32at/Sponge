if ((Get-PSSnapin | ? { $_.Name -eq "Microsoft.SharePoint.PowerShell" }) -eq $null) {
    Add-PSSnapin Microsoft.SharePoint.PowerShell
}
write-host "SharePoint Snapin loaded"

function wait4timer($wspFileName) 
{
    $solution = Get-SPSolution | where-object {$_.Name -eq $wspFileName}
    if ($solution -ne $null) 
    {
        $counter = 1   
        
        Write-Host "Waiting to finish soultion timer job"
        while( ($solution.JobExists -eq $true ) -and ( $counter -lt 50 ) ) 
        {   
            Write-Host "Please wait..."
            sleep 2
            $counter++   
        }
        
        Write-Host "Finished the solution timer job"         
    }
}

###############################################################################################

$wspFileName = "Sponge.wsp"

$path = Join-Path $(get-location) $wspFileName

write-host "Adding solution $wspFileName to the farm"
add-spsolution -literalpath "$path"

write-host "Deploying solution $wspFileName"
install-spsolution -Identity "$wspFileName" -gacdeployment -allwebapplications

wait4timer($wspFileName)
write-host "Done adding"

$ca = Get-SPWebApplication -IncludeCentralAdministration | where {$_.DisplayName -eq "SharePoint Central Administration v4"} | select Url
$url =  $ca.Url + "Sponge/default.aspx"

Start-Process $url

