using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

//информация об оказании помощи
public record DetailsForAssistance
{
    private DetailsForAssistance(
        string title,
        string description,
        string contactPhoneAssistance,
        string? bankCardAssistance)
    {
        Title = title;
        Description = description;
        ContactPhoneAssistance = contactPhoneAssistance;
        BankCardAssistance = bankCardAssistance;
    }

    public string Title { get; }
    public string Description { get; }
    public string ContactPhoneAssistance { get; }
    public string? BankCardAssistance { get; }

    public static Result<DetailsForAssistance> Create(
        string title,
        string description,
        string contactPhoneAssistance,
        string bankCardAssistance)
    {
        if (contactPhoneAssistance?.Length != Constants.LENGTH_PHONE_NUMBER && IsDigitsOnly(contactPhoneAssistance))
            return Result.Failure<DetailsForAssistance>("Не верно указан контактный номер телефона");

        if (string.IsNullOrWhiteSpace(contactPhoneAssistance))
        {
            return Result.Failure<DetailsForAssistance>("Не указан номер телефона");
        }

        return new DetailsForAssistance(title, description, contactPhoneAssistance, bankCardAssistance);
    }

    private static bool IsDigitsOnly(string? checkString)
    {
        if (checkString == null)
            return false;

        return checkString.All(c => c >= '0' && c <= '9');
    }
}