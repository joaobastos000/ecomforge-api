using EcomForge.Application.Abstractions;
using EcomForge.Application.DTOs.Categories;
using EcomForge.Common.Results;
using EcomForge.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcomForge.Application.Services;

public sealed class CategoryService(IAppDbContext dbContext)
{
    public async Task<IReadOnlyCollection<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Categories.AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new CategoryDto(x.Id, x.Name, x.Description))
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<CategoryDto>> CreateAsync(CreateCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var category = new Category(request.Name, request.Description);
        dbContext.Categories.Add(category);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<CategoryDto>.Success(new CategoryDto(category.Id, category.Name, category.Description));
    }

    public async Task<Result> UpdateAsync(Guid id, UpdateCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var category = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (category is null)
        {
            return Result.Failure(new Error("Categories.NotFound", "Category not found."));
        }

        category.Update(request.Name, request.Description);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (category is null)
        {
            return Result.Failure(new Error("Categories.NotFound", "Category not found."));
        }

        dbContext.Categories.Remove(category);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
