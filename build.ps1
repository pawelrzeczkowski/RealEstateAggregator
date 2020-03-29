$SolutionFile      = "RealEstateAggregator.sln"
$Configuration     = "Debug"
$Platform          = "Any CPU"
$msbuild = "C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe"

# Build the sln file
nuget restore $SolutionFile
dotnet restore $SolutionFile

try
{
	pushd src\RealEstateAggregator.Web
	#gulp less
}
finally
{
	popd
}

& $msbuild $SolutionFile /p:Configuration=$Configuration /p:Platform=$Platform /target:Rebuild
if ($LastExitCode -ne 0)
{
	throw "Building solution failed."
}
else
{
	Write-Host "Building solution complete."-ForegroundColor GREEN
}
