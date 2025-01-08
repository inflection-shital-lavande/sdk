using Newtonsoft.Json;
using Bogus;
using sdk.demo.src.api.appointment.AppointmentValidation;
using sdk.demo.src.api.appointment.AppointmentModel;

namespace sdk.demo.src.api.appointment.AppointmentService;

public class Appointment
{
    private readonly APIClient _client;

    public Appointment(APIClient client)
    {
        _client = client;
    }

    public Task<string> Create(AppointmentCreateModel appointment)
    {
        return _client.Request("/assets/appointments", HttpMethod.Post, appointment);
    }

    public Task<string> GetById(string appointmentId)
    {
        return _client.Request($"/assets/appointments/{appointmentId}", HttpMethod.Get);
    }

    public Task<string> Search(AppointmentSearchFilters searchQuery)
    {
        var queryParameters = new List<string>();

        if (!string.IsNullOrEmpty(searchQuery.Name))
            queryParameters.Add($"name={Uri.EscapeDataString(searchQuery.Name)}");

        var queryString = queryParameters.Any() ? $"?{string.Join("&", queryParameters)}" : string.Empty;

        return _client.Request($"/assets/appointments/search{queryString}", HttpMethod.Get);
    }

    public Task<string> Update(string appointmentId, AppointmentUpdateModel appointment)
    {
        return _client.Request($"/assets/appointments/{appointmentId}", HttpMethod.Put, appointment);
    }

    public Task<string> Delete(string appointmentId)
    {
        return _client.Request($"/assets/appointments/{appointmentId}", HttpMethod.Delete);
    }

    public async Task ExecuteAppointmentOperations()
    {
        var faker = new Faker("en");
        var createValidator = new AppointmentCreateModelValidator();

        var appointmentCreateData = new AppointmentCreateModel
        {
            AssetCode = faker.Random.AlphaNumeric(10),
            Name = faker.Commerce.ProductName(),
            Description = faker.Lorem.Paragraph(),
            AppointmentType = faker.PickRandom("Doctor", "Lab", "Physiotherapy", "Other"),
            Version = "1.0",
        };

        var createValidationResult = createValidator.Validate(appointmentCreateData);
        if (!createValidationResult.IsValid)
        {
            foreach (var error in createValidationResult.Errors)
            {
                Console.WriteLine("Validation error:" + error.ErrorMessage);
            }
            return;
        }

        var createResponse = await Create(appointmentCreateData);
        dynamic createdAppointment = JsonConvert.DeserializeObject(createResponse);
        Console.WriteLine("Create: " + JsonConvert.SerializeObject(createdAppointment, Formatting.Indented));

        string appointmentId = createdAppointment.Data.id.ToString();

        var retrievedResponse = await GetById(appointmentId);
        dynamic retrieved = JsonConvert.DeserializeObject(retrievedResponse);
        Console.WriteLine("GetById: " + JsonConvert.SerializeObject(retrieved, Formatting.Indented));

        var updateValidator = new AppointmentUpdateModelValidator();
        var appointmentUpdatedData = new AppointmentUpdateModel
        {
            AssetCode = faker.Random.AlphaNumeric(10),
            Name = faker.Name.FullName(),
            Description = faker.Lorem.Paragraph(),
            AppointmentType = faker.PickRandom("Doctor", "Lab", "Physiotherapy", "Other"),
            Tags = faker.Commerce.Categories(3).ToArray(),
            Version = "2.0",
        };

        var updateValidationResult = updateValidator.Validate(appointmentUpdatedData);
        if (!updateValidationResult.IsValid)
        {
            foreach (var error in updateValidationResult.Errors)
            {
                Console.WriteLine("Validation error:" + error.ErrorMessage);
            }
            return;
        }

        var updateResponse = await Update(appointmentId, appointmentUpdatedData);
        dynamic updated = JsonConvert.DeserializeObject(updateResponse);
        Console.WriteLine("Update: " + JsonConvert.SerializeObject(updated, Formatting.Indented));

        var searchValidator = new AppointmentSearchFiltersValidator();
        var searchFiltersData = new AppointmentSearchFilters
        {
            Name = appointmentCreateData.Name
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

        var deleteResponse = await Delete(appointmentId);
        dynamic deleted = JsonConvert.DeserializeObject(deleteResponse);
        Console.WriteLine("Delete: " + JsonConvert.SerializeObject(deleted, Formatting.Indented));
    }
}

