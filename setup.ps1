$ConfigTool = "src\RealEstateAggregator.IISConfigTool\bin\Debug\RealEstateAggregator.IISConfigTool.exe"

# Build the sln file
.\build.ps1

# Run IIS7 IISConfig tool
& $ConfigTool
if ($LastExitCode -ne 0)
{
	Write-Host "IISConfig Setup failed."-ForegroundColor RED
	exit 1
}
else
{
	Write-Host "IIS7 Setup complete."-ForegroundColor GREEN
}

Write-Host "Setup complete"-ForegroundColor GREEN
