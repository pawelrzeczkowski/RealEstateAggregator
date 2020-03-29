
# App Pool setup

$appcmd = "C:\Windows\System32\inetsrv\appcmd"

$appName = "RealEstateAggregator"

$appPoolExists = (&$appcmd list apppool /name:$appName) -ne $null

if ($appPoolExists -eq $true)
{
	Write-Host "App pool $appName starting"
	&$appcmd stop apppool /apppool.name:$appName
}
else
{
    Write-Host "App pool $appName does not exists, run IisConfigTool in order to debug app locally"
}

# Site setup

$siteExists = (&$appcmd list sites /name:$appName) -ne $null

if ($siteExists -eq $true)
{
	Write-Host "Site $appName starting"
	&$appcmd stop sites /site.name:$appName
}
else
{
    Write-Host "Site $appName does not exists, run IisConfigTool in order to debug app locally"
}
