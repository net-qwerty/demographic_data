using Microsoft.AspNetCore.Authorization.Infrastructure;

public class FormItem
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public string? Birthday { get; set; }

    public string? Gender { get; set; }

    public string? PlaceOfBirth { get; set; }

    public string? Education { get; set; }

    public string? FamilyStatus { get; set; }

    public string? Employment { get; set; }

    public string? IncomeLevel { get; set; }

    public string? Revenue { get; set; }

}
