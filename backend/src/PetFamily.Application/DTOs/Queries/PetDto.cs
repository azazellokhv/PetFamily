namespace PetFamily.Application.DTOs.Queries;

public class PetDto
{
    public Guid PetId { get; init; }
    public Guid VolunteerId { get; init; }
    public string Nickname { get; init; } = string.Empty;
    public Guid BiologicalSpeciesId { get; init; }
    public Guid BreedId { get; init; }
    public string Description { get; init; } = string.Empty;
    public string Color { get; init; } = string.Empty;
    public bool IsHealthy { get; init; } 
    public string DescriptionDisease { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public string Locality { get; init; } = string.Empty;
    public string Street { get; init; } = string.Empty;
    public string BuildingNumber { get; init; } = string.Empty;
    public string Comments { get; init; } = string.Empty;
    public int Weight { get; init; }
    public int Height { get; init; }
    public string PhoneNumber { get; init; } = string.Empty;
    public bool IsNeutered { get; init; }
    public DateTime Birthday { get; init; }
    public bool IsVaccinated { get; init; }
    public string AssistanceStatus { get; init; } = string.Empty;
    public IEnumerable<DetailForAssistanceDto> DetailForAssistance { get; init; } = default!;
    public DateTime DateOfCreation { get; init; }
    public IEnumerable<PetPhotoDto> PetPhotos { get; init; } = default!;
    public int Position { get; init; }
}

public class PetPhotoDto
{
    public string FilePath { get; set; } = string.Empty;
    public bool IsMain { get; set; } = false;
}
