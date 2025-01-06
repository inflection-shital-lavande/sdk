using System;
using Newtonsoft.Json;
using Bogus;
using sdk.demo.src.api.animation.AnimationValidation;
using sdk.demo.src.api.animation.AnimationModel;

namespace sdk.demo.src.api.animation.AnimationService;
public class Animation
{
    private readonly APIClient _client;
    public Animation(APIClient client)
    {
        _client = client;
    }
    public Task<string> Create(AnimationCreateModel animation)
    {
        return _client.Request("/assets/animations", HttpMethod.Post, animation);
    }

    public Task<string> GetById(string animationId)
    {
        return _client.Request($"/assets/animations/{animationId}", HttpMethod.Get);
    }

    public Task<string> Search(AnimationSearchFilters searchFilters)
    {
        var queryParameters = new List<string>();

        if (!string.IsNullOrEmpty(searchFilters.Name))
            queryParameters.Add($"name={searchFilters.Name}");

        var queryString = string.Join("&", queryParameters);

        return _client.Request($"/assets/animations/search?{queryString}", HttpMethod.Get);
    }

    public Task<string> Update(string animationId, AnimationUpdateModel animation)
    {
        return _client.Request($"/assets/animations/{animationId}", HttpMethod.Put, animation);
    }

    public Task<string> Delete(string animationId)
    {
        return _client.Request($"/assets/animations/{animationId}", HttpMethod.Delete);
    }

    public async Task ExecuteAnimationOperations()
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
                Console.WriteLine("Validation error:" + error.ErrorMessage);
            }
            return;
        }

        var createResponse = await Create(animationCreateData);
        dynamic createdAnimation = JsonConvert.DeserializeObject(createResponse);
        Console.WriteLine("Create: " + JsonConvert.SerializeObject(createdAnimation, Formatting.Indented));

        string animationId = createdAnimation.Data.id.ToString();

        var retrievedResponse = await GetById(animationId);
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
                Console.WriteLine("Validation error:" + error.ErrorMessage);
            }
            return;
        }

        var updateResponse = await Update(animationId, animationupdatedData);
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
                Console.WriteLine("Validation error:" + error.ErrorMessage);
            }
            return;
        }

        var searchResponse = await Search(searchFiltersData);
        dynamic searchResults = JsonConvert.DeserializeObject(searchResponse);
        Console.WriteLine("Search Results: " + JsonConvert.SerializeObject(searchResults, Formatting.Indented));

        var deleteResponse = await Delete(animationId);
        dynamic deleted = JsonConvert.DeserializeObject(deleteResponse);
        Console.WriteLine("Delete: " + JsonConvert.SerializeObject(deleted, Formatting.Indented));
    }
}
