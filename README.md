# Open Source Template

This template is based on [Google's new-project template](https://github.com/google/new-project).

## Instructions

1. Check it out from Bitbucket.
2. Create a new local repository and copy the files from this repo into it.
3. Modify README.md using the template in [README Instructions](#markdown-header-readme-instructions).
4. Modify the LICENSE file to add the year and copyright holder.
5. Modify the CODE-OF-CONDUCT file to add your email.
6. If needed, consider adding a separate CONTRIBUTE.md file with instructions for contributors.

### Set up your opensource repo using Git

``` shell
git clone git@bitbucket.org:ohpen-dev/opensource-template.git
mkdir opensource-project
cd opensource-project
git init
cp ../project-code/* .
git add *
git commit -a -m 'License and README for open source'
```

## Add the license to the source code headers

ALL files containing source code must include copyright and license information, including JS/CSS/YAML files.

Apache header:

    Copyright 2019 Google LLC

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

        https://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.

# README Instructions #

This README would normally document whatever steps are necessary to get your application up and running.

### What is this repository for? ###

* Quick summary
* Version
* [Learn Markdown](https://bitbucket.org/tutorials/markdowndemo)

### How do I get set up? ###

* Summary of set up
* Configuration
* Dependencies
* Database configuration
* How to run tests
* Deployment instructions

### Contribution guidelines ###

* Writing tests
* Code review
* Other guidelines

### Who do I talk to? ###

* Repo owner or admin
* Other community or team contact