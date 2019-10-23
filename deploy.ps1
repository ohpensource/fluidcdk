# Input Parameters
param (
    $localfolder = "",
    $awsprofile = "",
    $awsaccount = ""
);

if (!$localfolder) {
    $localfolder = Get-Location
}

if (!$awsprofile) {
    $awsprofile = $Env:AWS_PROFILE;
}

if (!$awsaccount) {
    $awsaccount = $Env:AWS_ACCOUNT;
}
# Helpers
function Write-ColorOutput($ForegroundColor)
{
    # save the current color
    $fc = $host.UI.RawUI.ForegroundColor

    # set the new color
    $host.UI.RawUI.ForegroundColor = $ForegroundColor

    # output
    if ($args) {
        Write-Output $args
    }
    else {
        $input | Write-Output
    }

    # restore the original color
    $host.UI.RawUI.ForegroundColor = $fc
}

Write-ColorOutput "green" "`n`r`tLocal folder: $localfolder`n`r`tProfile: $awsprofile`n`r`tAWS Account: $awsaccount"
Write-Host -NoNewLine 'Press any key to continue...';
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');
Write-Host ""

# Delete any previous Log
Set-Location $localfolder
del .\build\*.log

# Set Environment variables
$Env:ASSET_FOLDER = "$localfolder\build\assets";
$Env:AWS_ACCOUNT = "$awsaccount";

# Building & Packaging
Write-ColorOutput "cyan" "Building Solution..."
Set-Location $localfolder
dotnet build .\src\FluidCdk.sln > $localfolder\build\build.log

# Artifact Packaging
if ($?) 
{
    Write-ColorOutput "cyan" "  >> Packaging artifacts..."
    Set-Location .\src\examples\ImageTaggerWeb\ImageTagger.Lambda
    dotnet lambda package -f netcoreapp2.1 -c Release -o $localfolder\build\assets\ImageTagger.Lambda.zip > $localfolder\build\packaging.log
}
if ($?) 
{
    Set-Location $localfolder\src\examples\ImageTaggerWeb\ImageTagger
    dotnet lambda package -f netcoreapp2.1 -c Release -o $localfolder\build\assets\ImageTagger.Web.zip >> $localfolder\build\packaging.log
}

Set-Location $localfolder

if ($?) {
# Deploying Stack
    Write-ColorOutput "cyan" "Deploying stack..."
    Set-Location $localfolder\src\examples\ImageTaggerWeb\ImageTagger.Infra
    cdk deploy --profile $awsprofile --output cdk.out
}

if ($?) {
    # Everything OK
    Write-ColorOutput "green" "Done!"
}

if (!$?) {
    # Uh oh!
    Write-ColorOutput "red" "There was an error in the process ..."
    Write-ColorOutput "green" "Check the logs at $localfolder\build\"
        
}

# Cleanup
Remove-Item Env:\ASSET_FOLDER
Remove-Item Env:\AWS_ACCOUNT

Set-Location $localfolder
