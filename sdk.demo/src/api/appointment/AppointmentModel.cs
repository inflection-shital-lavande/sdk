using System.ComponentModel.DataAnnotations;

namespace sdk.demo.src.api.appointment.AppointmentModel;

public class AppointmentCreateModel
{
    [StringLength(100, MinimumLength = 1)]
    public string AssetCode { get; set; }= string.Empty;

    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Name { get; set; }= string.Empty;

    [StringLength(1000, MinimumLength = 1)]
    public string Description { get; set; }= string.Empty;

    public string AppointmentType { get; set; }= string.Empty;

    [StringLength(50, MinimumLength = 1)]
    public string Version { get; set; }= string.Empty;
}

public class AppointmentUpdateModel
{
    [StringLength(100, MinimumLength = 1)]
    public string AssetCode { get; set; }= string.Empty;

    [StringLength(200, MinimumLength = 1)]
    public string Name { get; set; }= string.Empty;

    [StringLength(1000, MinimumLength = 1)]
    public string Description { get; set; }= string.Empty;

    [Required]
    public string AppointmentType { get; set; }= string.Empty;

    [StringLength(200, MinimumLength = 1)]
    public string[]? Tags { get; set; }

    [StringLength(50, MinimumLength = 1)]
    public string Version { get; set; }= string.Empty;
}

public class AppointmentSearchFilters
{
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; set; }= string.Empty;
}
