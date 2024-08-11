﻿using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models;

//информация об оказании помощи
public class DetailsForAssistance 
{
    private DetailsForAssistance(string title, string description, string contactPhoneAssistance, string bankCardAssistance)
    {
        Id = new Guid();
        Title = title;
        Description = description;
        ContactPhoneAssistance = contactPhoneAssistance;
        BankCardAssistance = bankCardAssistance;
    }
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string ContactPhoneAssistance { get; private set; }
    public string BankCardAssistance { get; private set; }

    public static Result<DetailsForAssistance> Create(string title, string description, string contactPhoneAssistance,
        string bankCardAssistance)
    {
        if (contactPhoneAssistance.Length != 11 && IsDigitsOnly(contactPhoneAssistance))
            return Result.Failure<DetailsForAssistance>("Не верно указан контактный номер телефона");
        
        var detailsForAssistance = new DetailsForAssistance(title, description, contactPhoneAssistance, bankCardAssistance);

        return Result.Success(detailsForAssistance);
    }
    private static bool IsDigitsOnly(string checkString)
    {
        foreach (char c in checkString)
        {
            if (c < '0' || c > '9')
                return false;
        }
        return true;
    }
}