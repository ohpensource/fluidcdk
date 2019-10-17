# Input Parameters
param (
    $localFolder="C:\OhpenSource\FluidCdk\",
    $awsprofile = "",
    $awsaccount = ""
);

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

Write-ColorOutput "green" "`n`r`tLocal folder: $localFolder`n`r`tProfile: $awsprofile`n`r`tAWS Account: $awsaccount"
Write-Host -NoNewLine 'Press any key to continue...';
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');
Write-Host ""

# Delete any previous Log
Set-Location $localFolder
del .\build\*.log

# Set Environment variables
$Env:ASSET_FOLDER = "$localFolder\build\assets"

# Building & Packaging
Write-ColorOutput "cyan" "Building Solution..."
Set-Location $localFolder
dotnet build .\src\FluidCdk.sln > $localFolder\build\build.log

# Artifact Packaging
if ($?) 
{
    Write-ColorOutput "cyan" "  >> Packaging artifacts..."
    Set-Location .\src\examples\ImageTaggerWeb\ImageTagger.Lambda
    dotnet lambda package -f netcoreapp2.1 -c Release -o $localFolder\build\assets\ImageTagger.Lambda.zip > $localFolder\build\packaging.log
}
if ($?) 
{
    Set-Location $localfolder\src\examples\ImageTaggerWeb\ImageTagger
    dotnet lambda package -f netcoreapp2.1 -c Release -o $localFolder\build\assets\ImageTagger.Web.zip >> $localFolder\build\packaging.log
}

Set-Location $localFolder

if ($?) {
# Deploying Stack
    Write-ColorOutput "cyan" "Deploying stack..."
    Set-Location $localFolder\src\examples\ImageTaggerWeb\ImageTagger.Infra
    cdk deploy --profile $awsprofile --output cdk.out
}

if ($?) {
    # Everything OK
    Write-ColorOutput "green" "Done!"
}

if (!$?) {
    # Uh oh!
    Write-ColorOutput "red" "There was an error in the process ..."
    Write-ColorOutput "green" "Check the logs at $localFolder\build\"
        
}

# Cleanup
Remove-Item Env:\ASSET_FOLDER

Set-Location $localFolder
