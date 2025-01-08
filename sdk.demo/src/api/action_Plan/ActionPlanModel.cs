using System.ComponentModel.DataAnnotations;

namespace sdk.demo.src.api.action_plan.ActionPlanModel;

public class ActionPlanCreateModel
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string AssetCode { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, MinimumLength = 2)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [StringLength(20, MinimumLength = 1)]
    public string Version { get; set; } = string.Empty;
}

public class ActionPlanUpdateModel
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string AssetCode { get; set; } = string.Empty;

    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, MinimumLength = 2)]
    public string Description { get; set; } = string.Empty;

    [MinLength(1), MaxLength(20)]
    public List<string>? Tags { get; set; }

    [StringLength(20, MinimumLength = 1)]
    public string Version { get; set; } = string.Empty;
}

public class ActionPlanSearchFilters
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;
}
