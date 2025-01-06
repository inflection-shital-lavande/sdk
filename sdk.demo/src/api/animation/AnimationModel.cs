using System.ComponentModel.DataAnnotations;

namespace sdk.demo.src.api.animation.AnimationModel;

public class AnimationCreateModel
{
    [StringLength(100, MinimumLength = 1)]
    public string AssetCode { get; set; } = string.Empty;

    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000, MinimumLength = 1)]
    public string Transcript { get; set; } = string.Empty;

    [StringLength(500, MinimumLength = 1)]
    public string Url { get; set; } = string.Empty;

    [StringLength(50, MinimumLength = 1)]
    public string Version { get; set; } = string.Empty;
}

public class AnimationUpdateModel
{
    [StringLength(100, MinimumLength = 1)]
    public string AssetCode { get; set; } = string.Empty;

    [StringLength(200, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000, MinimumLength = 1)]
    public string Transcript { get; set; } = string.Empty;

    [StringLength(500, MinimumLength = 1)]
    public string Url { get; set; } = string.Empty;

    [StringLength(200, MinimumLength = 1)]
    public string[]? Tags { get; set; }

    [StringLength(50, MinimumLength = 1)]
    public string Version { get; set; } = string.Empty;
}

public class AnimationSearchFilters
{
    [StringLength(200, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;
}
