#!/usr/bin/env bash

cd "${0%/*}"
rm ./build/*.log

# Set Environment variables
export ASSET_FOLDER "${0%/*}/build/assets"
export AWS_ACCOUNT $2

# Building & Packaging
echo "Building solution..."
cd "${0%/*}"
dotnet build ./src/FluidCdk.sln > ${0%/*}/build/build.log

# Artifact Packaging
echo "Packaging artifacts..."
cd ./src/examples/ImageTaggerWeb/ImageTagger.Lambda
dotnet lambda package -f netcoreapp2.1 -c Release -o ${0%/*}/build/assets/ImageTagger.Lambda.zip > ${0%/*}/build/packaging.log

cd ${0%/*}/src/examples/ImageTaggerWeb/ImageTagger
dotnet lambda package -f netcoreapp2.1 -c Release -o ${0%/*}/build/assets/ImageTagger.Web.zip >> ${0%/*}/build/packaging.log

cd "${0%/*}"
echo "Deploying stack..."
cd ${0%/*}/src/examples/ImageTaggerWeb/ImageTagger.Infra
cdk deploy --profile ${0%/*} --output cdk.out

unset ASSET_FOLDER
unset AWS_ACCOUNT

cd "${0%/*}"
