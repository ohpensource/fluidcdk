# Input Parameters
param (
    $profile="personal",
    $stackname="joan-cdk-test",
    $s3Bucket="joan-cdk-test-codebucket",
    $localFolder="C:\OhpenGit\FluidCdk\",
    $build="true"
);

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

Write-ColorOutput "green" "Parameters: `n`r`tProfile: '$profile'`n`r`tStackName: '$stackname'`n`r`tS3 Bucket: '$s3Bucket'`n`r`tLocal folder: $localFolder"
Write-Host -NoNewLine 'Press any key to continue...';
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');
Write-Host ""

# Building & Packaging
Write-ColorOutput "cyan" "Building Solution..."
Set-Location $localFolder
dotnet build .\src\FluidCdk.sln 
if ($?) 
{
    Write-ColorOutput "cyan" "  >> Packaging artifacts..."
    Set-Location .\src\examples\ImageTaggerWeb\ImageTagger.Lambda
    dotnet lambda package -f netcoreapp2.1 -c Release  --force-upload
}
if ($?) 
{
    Set-Location $localfolder\src\examples\ImageTaggerWeb\ImageTagger
    dotnet lambda package -f netcoreapp2.1 -c Release  --force-upload
}

Set-Location $localFolder

# Upload Artifacts
if ($?) {
    Write-ColorOutput "cyan" "Uploading Artifacts..."
    $zipnametagger = "ImageTagger.Lambda.zip"
    $zippathtagger = "$localFolder\src\examples\ImageTaggerWeb\ImageTagger.Lambda\bin\Release\netcoreapp2.1\$zipnametagger"
    $zipnameweb = "ImageTagger.zip"
    $zippathweb = "$localFolder\src\examples\ImageTaggerWeb\ImageTagger\bin\Release\netcoreapp2.1\$zipnameweb"
    $s3pathtagger = "s3://$s3Bucket/$stackname-$zipnametagger"
    $s3pathweb = "s3://$s3Bucket/$stackname-$zipnameweb"

    Write-ColorOutput "cyan" "  >> Uploading $zipnametagger lambda code to $s3pathtagger..."
    aws s3 cp $zippathtagger $s3pathtagger --profile $profile
    Write-ColorOutput "cyan" "  >> Uploading $zipnameweb lambda code to $s3pathweb..."
    aws s3 cp $zippathweb $s3pathweb --profile $profile
}

if ($?) {
# Generating CloudFormation
    Write-ColorOutput "cyan" "Synthing Cloudformation..."
    Set-Location $localFolder\src\examples\ImageTaggerWeb\ImageTagger.Infra
    cdk synth > $localFolder\output.tmp.txt
}

if ($?) {
    # Deploying to AWS
    Write-ColorOutput "cyan" "Deploying to AWS..."
    aws cloudformation deploy --template-file .\cdk.out\image-tagger-stack.template.json --stack-name $stackname --profile $profile --capabilities CAPABILITY_IAM 
}

if ($?) {
    # End
    Write-ColorOutput "green" "Done!"
    del $localFolder\output.tmp.txt
}

if (!$?) {
    Write-ColorOutput "red" "There was an error in the process ..."
    cat $localFolder\output.tmp.txt
        
}
Set-Location $localFolder
