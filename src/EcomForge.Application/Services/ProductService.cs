using EcomForge.Application.Abstractions;
using EcomForge.Application.DTOs.Products;
using EcomForge.Common.Results;
using EcomForge.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcomForge.Application.Services;

public sealed class ProductService(IAppDbContext dbContext)
{
    public async Task<IReadOnlyCollection<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Products
            .AsNoTracking()
            .Include(x => x.Category)
            .OrderBy(x => x.Name)
            .Select(x => new ProductDto(x.Id, x.Name, x.Description, x.Price, x.Stock, x.CategoryId, x.Category!.Name))
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<ProductDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await dbContext.Products.AsNoTracking().Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return product is null
            ? Result<ProductDto>.Failure(new Error("Products.NotFound", "Product not found."))
            : Result<ProductDto>.Success(new ProductDto(product.Id, product.Name, product.Description, product.Price, product.Stock, product.CategoryId, product.Category!.Name));
    }

    public async Task<Result<ProductDto>> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        var categoryExists = await dbContext.Categories.AnyAsync(x => x.Id == request.CategoryId, cancellationToken);
        if (!categoryExists)
        {
            return Result<ProductDto>.Failure(new Error("Categories.NotFound", "Category not found."));
        }

        var product = new Product(request.Name, request.Description, request.Price, request.Stock, request.CategoryId);
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(product.Id, cancellationToken);
    }

    public async Task<Result> UpdateAsync(Guid id, UpdateProductRequest request, CancellationToken cancellationToken = default)
    {
        var product = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (product is null)
        {
            return Result.Failure(new Error("Products.NotFound", "Product not found."));
        }

        product.Update(request.Name, request.Description, request.Price, request.Stock, request.CategoryId);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (product is null)
        {
            return Result.Failure(new Error("Products.NotFound", "Product not found."));
        }

        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
