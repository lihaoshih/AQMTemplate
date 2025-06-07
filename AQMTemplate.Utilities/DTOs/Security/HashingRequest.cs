using AQMTemplate.Domain.Enums.Security;

namespace Utilities.DTOs.Security;

public class HashingRequest
{
    public string InputText { get; set; } = null!;
    public HashingAlgorithm Algorithm { get; set; }
}