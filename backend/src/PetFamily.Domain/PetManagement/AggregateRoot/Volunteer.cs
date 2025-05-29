using CSharpFunctionalExtensions;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Enum;
using PetFamily.Domain.Shared.Ids;

namespace PetFamily.Domain.PetManagement.AggregateRoot;

public sealed class Volunteer : Shared.Entity<VolunteerId>, ISoftDeletable
{
    private bool _isDeleted = false;
    private readonly List<Pet> _pets = [];

    //For EF Сore
    private Volunteer(VolunteerId id) : base(id)
    {
    }

    private Volunteer(
        VolunteerId volunteerId,
        FullName fullName,
        Description description,
        WorkExperience workExperience,
        PhoneNumber phoneNumber,
        SocialNetworksList socialNetworkList,
        VolunteerDetailsList volunteerDetailsList)
        : base(volunteerId)

    {
        FullName = fullName;
        Description = description;
        WorkExperience = workExperience;
        PhoneNumber = phoneNumber;
        SocialNetworkList = socialNetworkList;
        VolunteerDetailsList = volunteerDetailsList;
    }

    public FullName FullName { get; private set; }
    public Description Description { get; private set; }
    public WorkExperience WorkExperience { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public SocialNetworksList SocialNetworkList { get; private set; }
    public VolunteerDetailsList VolunteerDetailsList { get; private set; }
    public IReadOnlyList<Pet> Pets => _pets;


    public UnitResult<Error> AddPet(Pet pet)
    {
        var positionResult = Position.Create(_pets.Count + 1);
        if (positionResult.IsFailure)
            return positionResult.Error;

        pet.SetPosition(positionResult.Value);

        _pets.Add(pet);

        return Result.Success<Error>();
    }

    public UnitResult<Error> MovePet(Pet pet, Position newPosition)
    {
        var currentPosition = pet.Position;
        if (currentPosition == newPosition || _pets.Count == 1)
            return Result.Success<Error>();

        var adjustedPosition = AdjustNewPositionIfOutOfRange(newPosition);
        if (adjustedPosition.IsFailure)
            return adjustedPosition.Error;

        newPosition = adjustedPosition.Value;

        var moveResult = MovePetsBetweenPositions(currentPosition, newPosition);
        if (moveResult.IsFailure)
            return moveResult.Error;

        pet.Move(newPosition);
        
        return Result.Success<Error>();
    }

    private Result<Position, Error> AdjustNewPositionIfOutOfRange(Position newPosition)
    {
        if (newPosition.Value <= _pets.Count)
            return newPosition;

        var lastPosition = Position.Create(_pets.Count - 1);
        if (lastPosition.IsFailure)
            return lastPosition.Error;

        return lastPosition.Value;
    }

    private UnitResult<Error> MovePetsBetweenPositions(Position currentPosition, Position newPosition)
    {
        if (newPosition.Value < currentPosition.Value)
        {
            var petsToMove = _pets.Where(
                p => p.Position.Value >= newPosition.Value &&
                     p.Position.Value < currentPosition.Value);

            foreach (var petToMove in petsToMove)
            {
                var result = petToMove.MoveForward();
                if (result.IsFailure)
                    return result.Error;
            }
        }
        else if (newPosition.Value > currentPosition.Value)
        {
            var petsToMove = _pets.Where(
                p => p.Position.Value > currentPosition.Value &&
                     p.Position.Value <= newPosition.Value);

            foreach (var petToMove in petsToMove)
            {
                var result = petToMove.MoveBack();
                if (result.IsFailure)
                    return result.Error;
            }
        }
        
        return Result.Success<Error>();
    }


    public Result<Pet, Error> GetPetById(PetId petId)
    {
        var pet = _pets.FirstOrDefault(p => p.Id == petId);
        if (pet == null)
            return Errors.General.NotFound(petId.Value);

        return pet;
    }

    public void UpdateMainInfo(
        FullName fullName,
        Description description,
        WorkExperience workExperience,
        PhoneNumber phoneNumber)
    {
        FullName = fullName;
        Description = description;
        WorkExperience = workExperience;
        PhoneNumber = phoneNumber;
    }

    public void Delete()
    {
        if (_isDeleted == false)
            _isDeleted = true;
    }

    public void Restore()
    {
        if (_isDeleted == true)
            _isDeleted = false;
    }

    public int CountPetsNeedHelp() =>
        _pets.Count(p => p.AssistanceStatus == AssistanceStatus.NeedHelp);

    public int CountPetsFindHome() =>
        _pets.Count(p => p.AssistanceStatus == AssistanceStatus.FindHome);

    public int CountPetsTreated =>
        _pets.Count(p => p.AssistanceStatus == AssistanceStatus.FoundHome);

    public static Result<Volunteer, Error> Create(
        VolunteerId volunteerId,
        FullName fullName,
        Description description,
        WorkExperience workExperience,
        PhoneNumber phoneNumber,
        SocialNetworksList socialNetworkList,
        VolunteerDetailsList volunteerDetailsList)
    {
        return new Volunteer(
            volunteerId,
            fullName,
            description,
            workExperience,
            phoneNumber,
            socialNetworkList,
            volunteerDetailsList);
    }
}