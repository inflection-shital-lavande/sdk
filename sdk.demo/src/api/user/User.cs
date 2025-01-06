using Newtonsoft.Json;
using Bogus;
using sdk.demo.src.api.user.UserModel;
using sdk.demo.src.api.user.UserModelValidation;

namespace sdk.demo.src.api.user.UserService;
public static class User
{
    public static Task<string> Create(this APIClient client, UserCreateModel userData)
    {
        return client.Request("/users", HttpMethod.Post, userData);
    }

    public static Task<string> GetById(this APIClient client, Guid userId)
    {
        return client.Request($"/users/{userId}", HttpMethod.Get);
    }

    public static Task<string> Search(this APIClient client, UserSearchFilters filters)
    {
        var queryParameters = new List<string>();
        if (!string.IsNullOrEmpty(filters.FirstName))
            queryParameters.Add($"firstName={Uri.EscapeDataString(filters.FirstName)}");

        var queryString = queryParameters.Any() ? $"?{string.Join("&", queryParameters)}" : string.Empty;

        return client.Request($"/users/search{queryString}", HttpMethod.Get);
    }

    public static Task<string> Update(this APIClient client, Guid userId, UserUpdateModel userData)
    {
        return client.Request($"/users/{userId}", HttpMethod.Put, userData);
    }

    public static Task<string> Delete(this APIClient client, Guid userId)
    {
        return client.Request($"/users/{userId}", HttpMethod.Delete);
    }

    public static async Task ExecuteUserOperations(APIClient client)
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
                Console.WriteLine(error.ErrorMessage);
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

        var newUserResponse = await client.Create(userCreateData);
        dynamic newUser = JsonConvert.DeserializeObject(newUserResponse);
        Console.WriteLine("Create: " + JsonConvert.SerializeObject(newUser, Formatting.Indented));

        Guid userId;
        try
        {
            userId = Guid.Parse(newUser.Data.id.ToString());
        }
        catch (FormatException)
        {
            Console.WriteLine("An error occurred: The provided id is not a valid Guid.");
            return;
        }

        var retrievedUserResponse = await client.GetById(userId);
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
                Console.WriteLine(error.ErrorMessage);
            }
            return;
        }

        var searchResultsResponse = await client.Search(searchFiltersData);
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
                Console.WriteLine(error.ErrorMessage);
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

        var updatedUserResponse = await client.Update(userId, userUpdateData);
        dynamic updatedUser = JsonConvert.DeserializeObject(updatedUserResponse);
        Console.WriteLine("Update: " + JsonConvert.SerializeObject(updatedUser, Formatting.Indented));

        var deletedUserResponse = await client.Delete(userId);
        dynamic deletedUser = JsonConvert.DeserializeObject(deletedUserResponse);
        Console.WriteLine("Delete: " + JsonConvert.SerializeObject(deletedUser, Formatting.Indented));
    }
}
