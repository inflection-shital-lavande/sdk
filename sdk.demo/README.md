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

### Step 1: Create the Project Directory Structure
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

### Step 2: Create a New Class Library
dotnet new classlib -n SDKDemo
cd SDKDemo

### Step 3: Add Code to Your SDK
. ActionPlanService.cs — Contains logic for handling action plans.
. AnimationService.cs — Logic for managing animations.
. UserService.cs — Logic for user management.

### Step 4: Update the csproj File
<PropertyGroup>
  <TargetFramework>net8.0</TargetFramework>
  <PackageId>SDKDemo</PackageId>
  <Version>1.0.0</Version>
  <Authors>YourName</Authors>
  <Description>A demo SDK for showcasing NuGet package creation.</Description>
  <PackageLicenseExpression>MIT</PackageLicenseExpression>
  <RepositoryUrl>https://github.com/your-repo/SDKDemo</RepositoryUrl>
</PropertyGroup>

### Step 5: Build and Test the SDK
dotnet build
dotnet run

### Step 6: Pack the SDK into a NuGet Package
dotnet pack --configuration Release --output ./Package

### Step 7: Test the Package Locally
dotnet nuget add source ./Package --name LocalSDKDemo

### Step 8: Create a new project to test the package:

dotnet new console -n TestSDKDemo
cd TestSDKDemo
dotnet add package SDKDemo --version 1.0.0 --source ../Package

1)write test package code 
2)dotnet build
3)dotnet run

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

### Clear NuGet Cache
a) If there are any issues with the package, you can clear the NuGet cache:
	dotnet nuget locals all --clear
	
### Remove Local Package Source (Optional)
dotnet nuget remove source LocalSDKDemo
















