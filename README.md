# FluidCDK

FluidCDK is a [fluent interface](https://en.wikipedia.org/wiki/Fluent_interface) wrapper for [AWS CDK](https://aws.amazon.com/cdk/)  (CloudFormation Development Kit). It can drastically reduce the amount of code required to provision resources in AWS.

## Features

* Enhanced readability over YAML/JSON.
* Structures/props are not required.
* Easy permission grants.
* Tons of built-in day-to-day constructs.
* CDK native objects can still be accessed.
* SOLID design.

## ImageTagger demo

### Prerequisites

* [.NET Core 2.0 or higher](https://dotnet.microsoft.com/download)
* [AWS CLI](https://docs.aws.amazon.com/cli/latest/userguide/install-windows.html)
* [AWS Cloud Development Kit (CDK)](https://docs.aws.amazon.com/cdk/latest/guide/getting_started.html)
* [npm](https://www.npmjs.com/get-npm)
* An active [Amazon Web Services (AWS) account](https://aws.amazon.com)

### Instructions

1. Clone this repository to the machine where AWS CLI is installed.
2. Configure AWS CLI with a default profile (for instructions, [see the AWS documentation](https://docs.aws.amazon.com/cli/latest/userguide/cli-chap-configure.html)).

#### Windows 

3. Open a PowerShell session and change directory to the repository root folder.
4. Run `./deploy.ps1 -awsprofile {profilename} -awsaccount {accountnumber}` where
  * `{profilename}` is the name of the profile you configured for the AWS CLI and
  * `{accountnumber}` is your AWS account number.
5. Once the application is deployed, you'll get its URL.

#### Linux / macOS 

3. Open the terminal and change directory to the repository root folder.
4. Run `./deploy.sh {profilename} {accountnumber}` where
  * `{profilename}` is the name of the profile you configured for the AWS CLI and
  * `{accountnumber}` is your AWS account number.
5. Once the application is deployed, you'll get its URL.
