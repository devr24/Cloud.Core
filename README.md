# **Cloud.Core** 
--------------

<div id="description">

Cross package functionality for all Cloud .net Core packages.  Contains the main interface contracts used as a top level API for other packages, this 
allows us to easily build cross-package functionality without creating a massive dependency chain.

</div>

## Design
The Interfaces held in this package allow for abstraction of services. Each service can have a "factory" implementation.  This is known as the 
_Abstract Factory_ design pattern, as covered in the link:  

- *Abstract Factory* - https://www.c-sharpcorner.com/article/abstract-factory-design-pattern-in-c-sharp/

You can read more on Design Patterns in our [Knowledge Share](https://ailimited.sharepoint.com/:f:/s/Engineering/EiNpbXiADjJCqPV1AA3Gu2ABh1Z1A3pxEmJU9joE59Vz-w?e=EFgFJ8)


## Usage
The Cloud.Core package contains the following interfaces:

- `IAuthentication` - Contract for bearer token authentication implementations.
- `IBlobStorage` - Contract for a blob storage implementations.
- `ISecureVault` - Contract for a secure vault implementations.
- `IMessenger` & `IReactiveMessenger` - Contract for defining a message queuing implementations.
- `ITelemetryLogger` - Contract for ILogger implementations.

## Test Coverage
A threshold will be added to this package to ensure the test coverage is above 80% for branches, functions and lines.  If it's not above the required threshold 
(threshold that will be implemented on ALL of the new core repositories going forward), then the build will fail.

## Compatibility
This package has has been written in .net Standard and can be therefore be referenced from a .net Core or .net Framework application. The advantage of utilising from a .net Core application, 
is that it can be deployed and run on a number of host operating systems, such as Windows, Linux or OSX.  Unlike referencing from the a .net Framework application, which can only run on 
Windows (or Linux using Mono).
 
## Setup
This package requires the .net Core 2.1 SDK, it can be downloaded here: 
https://www.microsoft.com/net/download/dotnet-core/2.1

IDE of Visual Studio or Visual Studio Code, can be downloaded here:
https://visualstudio.microsoft.com/downloads/

## How to access this package
All of the Cloud.Core.* packages are published to our internal NuGet feed.  To consume this on your local development machine, please add the following feed to your feed sources in Visual Studio:
TBC

For help setting up, follow this article: https://docs.microsoft.com/en-us/vsts/package/nuget/consume?view=vsts
