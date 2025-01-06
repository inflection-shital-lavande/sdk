# SDK Demo: Creating and Testing a .NET SDK Package

This guide walks you through the steps to create a .NET SDK, build it, package it as a NuGet `.nupkg` file, and test it locally.

## Project Structure

plaintext
sdk.demo/
│
├── src/
│   ├── api/
│   │   ├── action.plan/
│   │   │   ├── ActionPlanService.cs
│   │   │   ├── ActionPlanModel.cs
│   │   │   └── ActionPlanModelValidation.cs
│   │   ├── animation/
│   │   │   ├── AnimationService.cs
│   │   │   ├── AnimationModel.cs
│   │   │   └── AnimationModelValidation.cs
│   │   ├── appointment/
│   │   │   ├── AppointmentService.cs
│   │   │   ├── AppointmentModel.cs
│   │   │   └── AppointmentModelValidation.cs
│   │   ├── user/
│   │   │   ├── UserService.cs
│   │   │   ├── UserModel.cs
│   │   │   └── UserModelValidation.cs
|   |   ├──ApiClient.cs
|   |   ├──AuthentiationService.cs
|   |   ├──SDK.cs
├── common/
│   │   │   ├── CacheUtils.cs
│   │   │   ├── Exceptions.cs
│   │   │   └── ValidationHelper.cs
├── appsettings.json
├── Program.cs
├── sdk.demo.csproj   
│   
├── sdk.demo.sln    # Solution file
└── README.md       # Documentation

### Step 01: Create the Project Directory Structure
sdk.demo/
│
├── src/
│   ├── api/
│   ├── common/
├── appsettings.json
├── Program.cs
├── sdk.demo.csproj
├── sdk.demo.sln
└── README.md

### Step 02: Create a New Class Library
Syntax-
		dotnet new classlib -n <ProjectName>
		cd <ProjectName>

Command-
		dotnet new classlib -n sdk.demo
		cd sdk.demo

### Step 03: Add Code to Your SDK

. ActionPlanService.cs — Contains logic for handling action plans.
. AnimationService.cs — Logic for managing animations.
. UserService.cs — Logic for user management.

### Step 04: Update the csproj File
<PropertyGroup>
	  <OutputType>Exe</OutputType>
	  <TargetFramework>net8.0</TargetFramework>
	  <ImplicitUsings>enable</ImplicitUsings>
	  <Nullable>enable</Nullable>
	  <RootNamespace>sdk.demo</RootNamespace>
	  <GeneratePackageOnBuild>true</GeneratePackageOnBuild>
	  <PackageId>sdk.demo</PackageId>
	  <Version>1.0.0</Version>
	  <Authors>Your Name</Authors>
	  <Description>A demo SDK package for testing</Description>
	  <PackageLicenseExpression>MIT</PackageLicenseExpression>
	  <RepositoryUrl>https://github.com/yourrepo</RepositoryUrl>
  </PropertyGroup>

### Step 05: Build and Test the SDK

dotnet build
dotnet run

### Step 06: Pack the SDK into a NuGet Package

dotnet build --configuration Release   
dotnet pack --configuration Release --output ./Package

### Step 07: Test the Package Locally

dotnet nuget list source
dotnet nuget add source ./Package --name LocalPackage

### Step 08: Create a new project to test the package:

Syntax-
		dotnet new console -n <ProjectName>
		cd <ProjectName>
Command-
		dotnet new console -n test.sdk.package
		cd test.sdk.package

Syntax add package -
dotnet add package <package-name> --version <version> --source <source-path>

Command-
1)dotnet add package sdk.demo --version 1.0.0 --source ../Package OR
  dotnet add package sdk.demo --version 1.0.0 --source "F:\InflectionProject\dotnet_sdk\sdk.demo\sdk.demo\Package" OR
  dotnet add package sdk.demo --version 1.0.0 --source LocalPackage

2)write test package code in -> program.cs file
3)dotnet build
4)dotnet run

### Step 09:Uninstall the Package
syntax- 
		dotnet remove package <package-name>
cmd-
		dotnet remove package sdk.demo

### Step 10:Clear NuGet Cache

a) If there are any issues with the package, you can clear the NuGet cache:
	dotnet nuget locals all --clear
	

### Install Required NuGet Packages -> use in -Step 3
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Configuration.UserSecrets
dotnet add package Microsoft.Extensions.DependencyInjection
dotnet add package Bogus
dotnet add package Newtonsoft.Json

### Initialize User Secrets for Local Development -> Use in SDK Test SDKDemo project
dotnet user-secrets init
dotnet user-secrets set "USER_NAME" "admin"
dotnet user-secrets set "PASSWORD" "uHqLYqjh"
	
### Remove Local Package Source (Optional)
dotnet nuget remove source LocalPackage

