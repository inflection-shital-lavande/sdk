using System;
using Newtonsoft.Json;
using Bogus;
using sdk.demo.src.api.animation.AnimationValidation;
using sdk.demo.src.api.animation.AnimationModel;

namespace sdk.demo.src.api.animation.AnimationService;
public static class Animation
{
    public static Task<string> Create(this APIClient client, AnimationCreateModel animation)
    {
        return client.Request("/assets/animations", HttpMethod.Post, animation);
    }

    public static Task<string> GetById(this APIClient client, string animationId)
    {
        return client.Request($"/assets/animations/{animationId}", HttpMethod.Get);
    }

    public static Task<string> Search(this APIClient client, AnimationSearchFilters searchFilters)
    {
        var queryParameters = new List<string>();

        if (!string.IsNullOrEmpty(searchFilters.Name))
            queryParameters.Add($"name={searchFilters.Name}");

        var queryString = string.Join("&", queryParameters);

        return client.Request($"/assets/animations/search?{queryString}", HttpMethod.Get);
    }

    public static Task<string> Update(this APIClient client, string animationId, AnimationUpdateModel animation)
    {
        return client.Request($"/assets/animations/{animationId}", HttpMethod.Put, animation);
    }

    public static Task<string> Delete(this APIClient client, string animationId)
    {
        return client.Request($"/assets/animations/{animationId}", HttpMethod.Delete);
    }

    public static async Task ExecuteAnimationOperations(APIClient client)
    {
        var faker = new Faker("en");

        var createValidator = new AnimationCreateModelValidator();
        var animationCreateData = new AnimationCreateModel
        {
            AssetCode = faker.Random.AlphaNumeric(10),
            Name = faker.Commerce.ProductName(),
            Transcript = faker.Lorem.Paragraph(),
            Url = faker.Internet.Url(),
            Version = "1.0",
        };

        var createValidationResult = createValidator.Validate(animationCreateData);
        if (!createValidationResult.IsValid)
        {
            foreach (var error in createValidationResult.Errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }
            return;
        }
        var createResponse = await client.Create(animationCreateData);
        dynamic createdAnimation = JsonConvert.DeserializeObject(createResponse);
        Console.WriteLine("Create: " + JsonConvert.SerializeObject(createdAnimation, Formatting.Indented));

        string animationId = createdAnimation.Data.id.ToString();

        var retrievedResponse = await client.GetById(animationId);
        dynamic retrieved = JsonConvert.DeserializeObject(retrievedResponse);
        Console.WriteLine("GetById: " + JsonConvert.SerializeObject(retrieved, Formatting.Indented));

        var updateValidator = new AnimationUpdateModelValidator();
        var animationupdatedData = new AnimationUpdateModel
        {
            AssetCode = faker.Random.AlphaNumeric(10),
            Name = faker.Name.FullName(),
            Transcript = faker.Lorem.Paragraph(),
            Url = faker.Internet.Url(),
            Tags = faker.Commerce.Categories(3).ToArray(),
            Version = "2.0",
        };

        var updateValidationResult = updateValidator.Validate(animationupdatedData);
        if (!updateValidationResult.IsValid)
        {
            foreach (var error in updateValidationResult.Errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }
            return;
        }

        var updateResponse = await client.Update(animationId, animationupdatedData);
        dynamic updated = JsonConvert.DeserializeObject(updateResponse);
        Console.WriteLine("Update: " + JsonConvert.SerializeObject(updated, Formatting.Indented));

        var searchValidator = new AnimationSearchFiltersValidator();
        var searchFiltersData = new AnimationSearchFilters
        {
            Name = animationCreateData.Name
        };

        var searchValidationResult = searchValidator.Validate(searchFiltersData);
        if (!searchValidationResult.IsValid)
        {
            foreach (var error in searchValidationResult.Errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }
            return;
        }

        var searchResponse = await client.Search(searchFiltersData);
        dynamic searchResults = JsonConvert.DeserializeObject(searchResponse);
        Console.WriteLine("Search Results: " + JsonConvert.SerializeObject(searchResults, Formatting.Indented));

        var deleteResponse = await client.Delete(animationId);
        dynamic deleted = JsonConvert.DeserializeObject(deleteResponse);
        Console.WriteLine("Delete: " + JsonConvert.SerializeObject(deleted, Formatting.Indented));
    }
}
