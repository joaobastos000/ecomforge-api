namespace EcomForge.Application.DTOs.Categories;

public sealed record CategoryDto(Guid Id, string Name, string? Description);
public sealed record CreateCategoryRequest(string Name, string? Description);
public sealed record UpdateCategoryRequest(string Name, string? Description);
