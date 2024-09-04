namespace PetFamily.Domain.PetManagement.ValueObjects;

public record VolunteerDetailsList
{
    private VolunteerDetailsList() 
    {
    }
    public VolunteerDetailsList(IEnumerable<DetailForAssistance> detailForAssistance)
    {
        DetailsForAssistance = detailForAssistance.ToList();
    }
    public IReadOnlyList<DetailForAssistance> DetailsForAssistance { get; }
}