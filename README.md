# FluidCDK

FluidCDK is a [fluent interface](https://en.wikipedia.org/wiki/Fluent_interface) wrapper for [AWS CDK](https://aws.amazon.com/cdk/)  (CloudFormation Development Kit). It can drastically reduce the amount of code required to provision resources in AWS.

## Features

* Enhanced readability over YAML/JSON.
* Structures/props are not required.
* Easy permission grants.
* Tons of built-in day-to-day constructs.
* CDK native objects can still be accessed.
* SOLID design.

## Instructions

To run the ImageTagger demo you will need to:

* Have an active [Amazon Web Services (AWS) account](https://aws.amazon.com)
* Install [AWS CLI](https://docs.aws.amazon.com/cli/latest/userguide/install-windows.html).
* Install [NPM](https://www.npmjs.com/get-npm) if it's not yet installed in your system.
* Install [AWS Cloud Developemnt Kit (CDK)](https://docs.aws.amazon.com/cdk/latest/guide/getting_started.html).
* Configure your AWS CLI with a default profile.
* Clone this repository.
* From a PowerShell window, at the repository root, execute: `./deploy.ps1 -awsprofile "{profilename}" -awsaccount "{accountnumber}"` where `{profilename}` is the name of the profile you configured for the **AWS CLI** and `{accountnumber}` is your AWS account number.
* Once the application is deployed, you'll get in Output the URL for running it.
