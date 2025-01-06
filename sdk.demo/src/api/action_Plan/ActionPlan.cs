using Newtonsoft.Json;
using Bogus;
using sdk.demo.src.api.action_plan.ActionPlanValidation;
using sdk.demo.src.api.action_plan.ActionPlanModel;

namespace sdk.demo.src.api.action_plan.ActionPlanService;

public class ActionPlan
{
    private readonly APIClient _client;
    public ActionPlan(APIClient client)
    {
        _client = client;
    }
    public Task<string> Create(ActionPlanCreateModel actionPlan)
    {
        return _client.Request("/assets/action-plans", HttpMethod.Post, actionPlan);
    }

    public Task<string> GetById(string actionPlanId)
    {
        return _client.Request($"/assets/action-plans/{actionPlanId}", HttpMethod.Get);
    }

    public Task<string> Search(ActionPlanSearchFilters searchFilters)
    {
        var queryParams = new List<string>();

        if (!string.IsNullOrEmpty(searchFilters.Name)) queryParams.Add($"name={searchFilters.Name}");

        var queryString = string.Join("&", queryParams);
        return _client.Request($"/assets/action-plans/search?{queryString}", HttpMethod.Get);
    }

    public Task<string> Update(string actionPlanId, ActionPlanUpdateModel actionPlan)
    {
        return _client.Request($"/assets/action-plans/{actionPlanId}", HttpMethod.Put, actionPlan);
    }

    public Task<string> Delete(string actionPlanId)
    {
        return _client.Request($"/assets/action-plans/{actionPlanId}", HttpMethod.Delete);
    }

    public async Task ExecuteActionPlanOperations()
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
        
        var createResponse = await Create(newActionPlan);
        dynamic createdActionPlan = JsonConvert.DeserializeObject(createResponse);
        Console.WriteLine("Create: " + JsonConvert.SerializeObject(createdActionPlan, Formatting.Indented));

        string actionPlanId = createdActionPlan.Data.id.ToString();

        var retrievedResponse = await GetById(actionPlanId);
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

        var searchResultsResponse = await Search(searchFilters);
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

        var updateResponse = await Update(actionPlanId, updatedActionPlan);
        dynamic updated = JsonConvert.DeserializeObject(updateResponse);
        Console.WriteLine("Update: " + JsonConvert.SerializeObject(updated, Formatting.Indented));

        var deleteResponse = await Delete(actionPlanId);
        dynamic deleted = JsonConvert.DeserializeObject(deleteResponse);
        Console.WriteLine("Delete: " + JsonConvert.SerializeObject(deleted, Formatting.Indented));
    }
}





