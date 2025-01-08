using Newtonsoft.Json;
using Bogus;
using sdk.demo.src.api.user.UserModel;
using sdk.demo.src.api.user.UserModelValidation;

namespace sdk.demo.src.api.user.UserService;

public class User
{
    private readonly APIClient _client;

    public User(APIClient client)
    {
        _client = client;
    }

    public Task<string> Create(UserCreateModel userData)
    {
        return _client.Request("/users", HttpMethod.Post, userData);
    }

    public Task<string> GetById(string userId)
    {
        return _client.Request($"/users/{userId}", HttpMethod.Get);
    }

    public Task<string> Search(UserSearchFilters filters)
    {
        var queryParameters = new List<string>();
        if (!string.IsNullOrEmpty(filters.FirstName))
            queryParameters.Add($"firstName={Uri.EscapeDataString(filters.FirstName)}");

        var queryString = queryParameters.Any() ? $"?{string.Join("&", queryParameters)}" : string.Empty;

        return _client.Request($"/users/search{queryString}", HttpMethod.Get);
    }

    public Task<string> Update(string userId, UserUpdateModel userData)
    {
        return _client.Request($"/users/{userId}", HttpMethod.Put, userData);
    }

    public Task<string> Delete(string userId)
    {
        return _client.Request($"/users/{userId}", HttpMethod.Delete);
    }

    public async Task ExecuteUserOperations()
    {
        var faker = new Faker("en");

        var createValidator = new UserCreateModelValidator();
        var userCreateData = new UserCreateModel
        {
            RoleId = faker.Random.Int(1, 5),
            FirstName = faker.Name.FirstName(),
            LastName = faker.Name.LastName(),
            Email = faker.Internet.Email(),
            Gender = faker.PickRandom(new[] { "Male", "Female", "Other" }),
            CountryCode = faker.PickRandom(new[] { "+1", "+91", "+44" }),
            Phone = faker.Phone.PhoneNumber("##########"),
            Password = faker.Internet.Password(8, false, "\\w", "P@"),
        };

        var createValidationResult = createValidator.Validate(userCreateData);
        if (!createValidationResult.IsValid)
        {
            foreach (var error in createValidationResult.Errors)
            {
                Console.WriteLine("Validation error:" + error.ErrorMessage);
            }
            return;
        }

        if (!ValidationHelper.TryValidateModel(userCreateData, out var validationResults))
        {
            foreach (var validationResult in validationResults)
            {
                Console.WriteLine(validationResult.ErrorMessage);
            }
            return;
        }

        var newUserResponse = await Create(userCreateData);
        dynamic newUser = JsonConvert.DeserializeObject(newUserResponse);
        Console.WriteLine("Create: " + JsonConvert.SerializeObject(newUser, Formatting.Indented));

        string userId = newUser.Data.id.ToString();

        var retrievedUserResponse = await GetById(userId);
        dynamic retrievedUser = JsonConvert.DeserializeObject(retrievedUserResponse);
        Console.WriteLine("GetById: " + JsonConvert.SerializeObject(retrievedUser, Formatting.Indented));

        var searchValidator = new UserSearchFiltersValidator();
        var searchFiltersData = new UserSearchFilters
        {
            FirstName = userCreateData.FirstName
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

        var searchResultsResponse = await Search(searchFiltersData);
        dynamic searchResults = JsonConvert.DeserializeObject(searchResultsResponse);
        Console.WriteLine("Search: " + JsonConvert.SerializeObject(searchResults, Formatting.Indented));

        var updateValidator = new UserUpdateModelValidator();
        var userUpdateData = new UserUpdateModel
        {
            FirstName = faker.Name.FirstName(),
            LastName = faker.Name.LastName(),
            Email = faker.Internet.Email(),
            Gender = faker.PickRandom(new[] { "Male", "Female", "Other" }),
            CountryCode = faker.PickRandom(new[] { "+1", "+91", "+44" }),
            Phone = faker.Phone.PhoneNumber("##########"),
            Password = faker.Internet.Password(8, false, "\\w", "P@"),
        };

        var updateValidationResult = updateValidator.Validate(userUpdateData);
        if (!updateValidationResult.IsValid)
        {
            foreach (var error in updateValidationResult.Errors)
            {
                Console.WriteLine("Validation error:" + error.ErrorMessage);
            }
            return;
        }

        if (!ValidationHelper.TryValidateModel(userUpdateData, out var updateValidationResults))
        {
            foreach (var validationResult in updateValidationResults)
            {
                Console.WriteLine(validationResult.ErrorMessage);
            }
            return;
        }

        var updatedUserResponse = await Update(userId, userUpdateData);
        dynamic updatedUser = JsonConvert.DeserializeObject(updatedUserResponse);
        Console.WriteLine("Update: " + JsonConvert.SerializeObject(updatedUser, Formatting.Indented));

        var deletedUserResponse = await Delete(userId);
        dynamic deletedUser = JsonConvert.DeserializeObject(deletedUserResponse);
        Console.WriteLine("Delete: " + JsonConvert.SerializeObject(deletedUser, Formatting.Indented));
    }
}
