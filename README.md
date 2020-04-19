# **Cloud.Core**

<div id="description">

Cross package functionality for all Cloud Core projects.  Contains the main interface contracts used as a top level API for other packages, this 
allows us to easily build cross-package functionality without creating a massive dependency chain.

</div>

## Design
The Interfaces held in this package allow for abstraction of services. Each service can have a "factory" implementation.  This is known as the 
_Abstract Factory_ design pattern, as covered in the link:  

- *Abstract Factory* - https://www.c-sharpcorner.com/article/abstract-factory-design-pattern-in-c-sharp/


## Usage
The Cloud.Core package contains the following interfaces:

#### Logging
- `ITelemetryLogger` - Contract for ILogger implementations. 

#### State / Features
- `IFeatureFlag` - Feature flag contract - can be used with services such as launch darkly.
- `IStateStorage` - State storage contract - implemented within Cloud Functionality providers to enable State save/load within the AppHost.
- `IAuditLogger` - Common audit logging contract.
- `INamedInstance` - Provides accessibility to use NamedInstanceFactory (see below).

#### Cloud Functionality
- `IAuthentication` - Contract for bearer token authentication implementations.
- `IIdentityProvider` - Contract for identity providers to supply identity information.
- `IBlobStorage` - Contract for a blob storage implementations.
- `ISecureVault` - Contract for a secure vault implementations.
- `IMessenger` & `IReactiveMessenger` - Contract for defining a message queuing implementations.
- `ITableStorage` - Contract for data table storage (not relational table storage).

Various extensions methods exist within this API.  Any general purpose reuable extension methods should be added to this package.

Cloud.Core specific exceptions also exist within this API.

## Code Samples

### DataAnnotations - Attribute Validation

Take this model:

```csharp
private class TestClass
{
	[Required]
	public string RequiredTest { get; set; }

	[StringLength(10)]
	public string StringLengthTest { get; set; }

	[Range(1, 100)]
	public int RangeTest { get; set; }

	[DataType(DataType.EmailAddress)]
	public string DataTypeTest { get; set; }

	[MaxLength(10)]
	public string MaxLengthTest { get; set; }

	// Alpha characters only (max length 25).
	[RegularExpression(@"^[a-zA-Z]{1,25}$")]
	public string RegExTest { get; set; }
}
```
We can see it's decorated using DataAnnotation attributes that can be used for validation in an Asp.Net MVC application. 
In MVC the model state (along with the controller base) will work out the valid state of any model passed to a controller.  A code sample as follows:

```csharp
[HttpPost]
public IActionResult Create(TestClass example)
{
	if (ModelState.IsValid == false)
	{
		return BadRequest(new ApiErrorResult(ModelState));
	}

	... do work
}
```

Unfortunately, its not possible to do this in normal applications without rolling your own implementation of the validation.
Cloud.Core has an easy to use base class that will do just that.  When the base class `AttributeValidator` is inherited by any model, it allows the same 
DataAnnotations validation using attributes to be carried out on any class (outside of MVC applications).  Here's a code sample...

Model updated to inherit from AttributeValidator:

```csharp
private class TestClass : AttributeValidator
{
	[Required]
	public string RequiredTest { get; set; }

	[StringLength(10)]
	public string StringLengthTest { get; set; }

	[Range(1, 100)]
	public int RangeTest { get; set; }

	[DataType(DataType.EmailAddress)]
	public string DataTypeTest { get; set; }

	[MaxLength(10)]
	public string MaxLengthTest { get; set; }

	// Alpha characters only (max length 25).
	[RegularExpression(@"^[a-zA-Z]{1,25}$")]
	public string RegExTest { get; set; }
}
```

Validation code is as follows:

```csharp
public void CheckModelIsValid(TestClass example)
{
	var validationResult = example.Validate();

	if (validationResult.IsValid == false) 
	{
		foreach(var err in validationResult.Errors) 
		{
			Console.WriteLine(err.Message);
		}
	}
	
	...
}
```

This now saves manually implementing validation methods in your POCO classes.
  

### Named Instance Factory

We regularly use IServiceCollection within Cloud.Core applications.  One problem we needed to solve was how to have multiple instances of the 
same class implementation of any of our interfaces.  It's a problem that the Factory design pattern can help us with.

Taking this example class:

```csharp
    public interface ITestInterface : INamedInstance {
    }

    public class TestInstance1: ITestInterface
    {
        public TestInstance1(string name) {
            Name = name;
        }
        public string Name { get; set; }
    }

    public class TestInstance2: ITestInterface
    {
        public TestInstance2(string name) {
            Name = name;
        }
        public string Name { get; set; }
    }

    public class TestInstance3: ITestInterface
    {
        public TestInstance3(string name) {
            Name = name;
        }
        public string Name { get; set; }
    }
```

We can use the factory in the following way:

```csharp
    var i1 = new TestInstance1("testInstance1");
    var i2 = new TestInstance2("testInstance2");
    var i3 = new TestInstance3("testInstance3");

    var factory = new NamedInstanceFactory<ITestInterface>(new List<ITestInterface> { i1, i2, i3 });
    var lookup1 = factory["testInstance1"];
    var lookup2 = factory["testInstance2"];
    var lookup3 = factory["testInstance3"];
```
Typically though this will all be used with .net's Dependency Injection (IServiceCollection), as follows:

```csharp

services.AddSingleton<TestInstance1>("x");
services.AddSingleton<TestInstance1>("y");
services.AddSingleton<TestInstance1>("z");
servuces.AddSingleton<NamedInstanceFactory<TestInterface>>();
```

> In the sample above with Dependency Injection, the NamedInstanceFactory will resolve instance X/Y/Z automatically, as they'll be injected into the constuctor during DI initialisation.


# Cloud.Core Docker Base Image
The base image for all of our docker files is found within this repo, in the **Cloud.Core.BaseImage** folder.  Update this as required - it's currently configured to run on Alpine Linux and .net Core 3.1 Runtime.

# Cloud.Core Project Templates
The Cloud.Core project templates can be found in the **Cloud.Core Templates** folder - [read more about modifying and using the templates here](TBC).

## Test Coverage
A threshold will be added to this package to ensure the test coverage is above 80% for branches, functions and lines.  If it's not above the required threshold 
(threshold that will be implemented on ALL of the core repositories to gurantee a satisfactory level of testing), then the build will fail.

## Compatibility
This package has has been written in .net Standard and can be therefore be referenced from a .net Core or .net Framework application. The advantage of utilising from a .net Core application, 
is that it can be deployed and run on a number of host operating systems, such as Windows, Linux or OSX.  Unlike referencing from the a .net Framework application, which can only run on 
Windows (or Linux using Mono).
 
## Setup
This package is built using .net Standard 2.1 and requires the .net Core 3.1 SDK, it can be downloaded here: 
https://www.microsoft.com/net/download/dotnet-core/

IDE of Visual Studio or Visual Studio Code, can be downloaded here:
https://visualstudio.microsoft.com/downloads/

## How to access this package
All of the Cloud.Core.* packages are published to a internal NuGet feed.  To consume this on your local development machine, please add the following feed to your feed sources in Visual Studio:
https://pkgs.dev.azure.com/cloudcoreproject/CloudCore/_packaging/Cloud.Core/nuget/v3/index.json
 
For help setting up, follow this article: https://docs.microsoft.com/en-us/vsts/package/nuget/consume?view=vsts


<img src="https://cloud1core.blob.core.windows.net/icons/cloud_core_small.PNG" />
