using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models;

//цвет питомца
public class ColorPet 
{
    private ColorPet(string title)
    {
        Id = new Guid();
        Title = title;
    }
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    
    public static Result<ColorPet> Create(string title)
    {
        var colorPet = new ColorPet(title);

        return Result.Success(colorPet);
    } 
}
