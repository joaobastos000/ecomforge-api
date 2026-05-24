namespace EcomForge.Application.DTOs.Products;

public sealed record ProductDto(Guid Id, string Name, string Description, decimal Price, int Stock, Guid CategoryId, string? CategoryName);
public sealed record CreateProductRequest(string Name, string Description, decimal Price, int Stock, Guid CategoryId);
public sealed record UpdateProductRequest(string Name, string Description, decimal Price, int Stock, Guid CategoryId);
