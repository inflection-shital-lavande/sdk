using Newtonsoft.Json;
using Bogus;
using sdk.demo.src.api.action_plan.ActionPlanValidation;
using sdk.demo.src.api.action_plan.ActionPlanModel;

namespace sdk.demo.src.api.action_plan.ActionPlanService;

public static class ActionPlan
{
    public static Task<string> Create(this APIClient client, ActionPlanCreateModel actionPlan)
    {
        return client.Request("/assets/action-plans", HttpMethod.Post, actionPlan);
    }

    public static Task<string> GetById(this APIClient client, string actionPlanId)
    {
        return client.Request($"/assets/action-plans/{actionPlanId}", HttpMethod.Get);
    }

    public static Task<string> Search(this APIClient client, ActionPlanSearchFilters searchFilters)
    {
        var queryParams = new List<string>();

        if (!string.IsNullOrEmpty(searchFilters.Name)) queryParams.Add($"name={searchFilters.Name}");

        var queryString = string.Join("&", queryParams);
        return client.Request($"/assets/action-plans/search?{queryString}", HttpMethod.Get);
    }

    public static Task<string> Update(this APIClient client, string actionPlanId, ActionPlanUpdateModel actionPlan)
    {
        return client.Request($"/assets/action-plans/{actionPlanId}", HttpMethod.Put, actionPlan);
    }

    public static Task<string> Delete(this APIClient client, string actionPlanId)
    {
        return client.Request($"/assets/action-plans/{actionPlanId}", HttpMethod.Delete);
    }

    public static async Task ExecuteActionPlanOperations(APIClient client)
    {
        var faker = new Faker("en");

        var createValidator = new ActionPlanCreateModelValidator();
        var newActionPlan = new ActionPlanCreateModel
        {
            AssetCode = faker.Random.AlphaNumeric(10),
            Name = faker.Commerce.ProductName(),
            Description = faker.Lorem.Paragraph(),
            Version = "1.0",
        };

        var createValidationResult = createValidator.Validate(newActionPlan);
        if (!createValidationResult.IsValid)
        {
            foreach (var error in createValidationResult.Errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }
            return;
        }

        if (!ValidationHelper.TryValidateModel(newActionPlan, out var validationResults))
        {
            foreach (var validationResult in validationResults)
            {
                Console.WriteLine(validationResult.ErrorMessage);
            }
            return;
        }
        var createResponse = await client.Create(newActionPlan);
        dynamic createdActionPlan = JsonConvert.DeserializeObject(createResponse);
        Console.WriteLine("Create: " + JsonConvert.SerializeObject(createdActionPlan, Formatting.Indented));

        string actionPlanId = createdActionPlan.Data.id.ToString();

        var retrievedResponse = await client.GetById(actionPlanId);
        dynamic retrieved = JsonConvert.DeserializeObject(retrievedResponse);
        Console.WriteLine("GetById: " + JsonConvert.SerializeObject(retrieved, Formatting.Indented));

        var searchValidator = new ActionPlanSearchFiltersValidator();
        var searchFilters = new ActionPlanSearchFilters
        {
           Name = newActionPlan.Name
        };

        var searchValidationResult = searchValidator.Validate(searchFilters);
        if (!searchValidationResult.IsValid)
        {
            foreach (var error in searchValidationResult.Errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }
            return;
        }

        var searchResultsResponse = await client.Search(searchFilters);
        dynamic searchResults = JsonConvert.DeserializeObject(searchResultsResponse);
        Console.WriteLine("Search: " + JsonConvert.SerializeObject(searchResults, Formatting.Indented));

        var updateValidator = new ActionPlanUpdateModelValidator();
        var updatedActionPlan = new ActionPlanUpdateModel
        {
            AssetCode = faker.Random.AlphaNumeric(10),
            Name = faker.Name.FullName(),
            Description = faker.Lorem.Paragraph(),
            Tags = faker.Lorem.Words(3).ToList(),
            Version = "1.0",
        };

        var updateValidationResult = updateValidator.Validate(updatedActionPlan);
        if (!updateValidationResult.IsValid)
        {
            foreach (var error in updateValidationResult.Errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }
            return;
        }

        var updateResponse = await client.Update(actionPlanId, updatedActionPlan);
        dynamic updated = JsonConvert.DeserializeObject(updateResponse);
        Console.WriteLine("Update: " + JsonConvert.SerializeObject(updated, Formatting.Indented));

        var deleteResponse = await client.Delete(actionPlanId);
        dynamic deleted = JsonConvert.DeserializeObject(deleteResponse);
        Console.WriteLine("Delete: " + JsonConvert.SerializeObject(deleted, Formatting.Indented));
    }
}





