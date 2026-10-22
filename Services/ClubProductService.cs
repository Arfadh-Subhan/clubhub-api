using ClubHub.Data;
using ClubHub.DTOs;
using ClubHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ClubHub.Services;

public class ClubProductService : IClubProductService
{
    private readonly AppDbContext _context;

    public ClubProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ClubProductResponse?> CreateAsync(CreateClubProductRequest request)
    {
        var clubProduct = new ClubProduct
        {
            ClubId = request.ClubId,
            Name = request.Name.Trim(),
            Description = request.Description.Trim(),
            Category = request.Category.Trim(),
            Price = request.Price,
            Image = request.Image,
            Stock = request.Stock,
            Status = "Pending",
            CreatedBy = request.CreatedBy,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.ClubProducts.Add(clubProduct);
        await _context.SaveChangesAsync();

        return MapToResponse(clubProduct);
    }

    public async Task<PageResult<ClubProductResponse>?> GetAllApprovedAsync(int pageNumber, int pageSize)
    {
        var query = _context.ClubProducts
            .AsNoTracking()
            .Where(clubProduct => clubProduct.Status == "Approved" && clubProduct.IsActive)
            .OrderByDescending(clubProduct => clubProduct.CreatedAt);

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(clubProduct => new ClubProductResponse
            {
                ClubProductId = clubProduct.ClubProductId,
                ClubId = clubProduct.ClubId,
                Name = clubProduct.Name,
                Description = clubProduct.Description,
                Category = clubProduct.Category,
                Price = clubProduct.Price,
                Image = clubProduct.Image,
                Stock = clubProduct.Stock,
                Status = clubProduct.Status,
                ReviewedBy = clubProduct.ReviewedBy,
                ReviewedAt = clubProduct.ReviewedAt,
                RejectionReason = clubProduct.RejectionReason,
                CreatedBy = clubProduct.CreatedBy,
                CreatedAt = clubProduct.CreatedAt,
                UpdatedAt = clubProduct.UpdatedAt,
                IsActive = clubProduct.IsActive
            })
            .ToListAsync();

        return new PageResult<ClubProductResponse>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<ClubProductResponse?> GetByIdAsync(int clubProductId)
    {
        return await _context.ClubProducts
            .AsNoTracking()
            .Where(clubProduct => clubProduct.ClubProductId == clubProductId)
            .Select(clubProduct => new ClubProductResponse
            {
                ClubProductId = clubProduct.ClubProductId,
                ClubId = clubProduct.ClubId,
                Name = clubProduct.Name,
                Description = clubProduct.Description,
                Category = clubProduct.Category,
                Price = clubProduct.Price,
                Image = clubProduct.Image,
                Stock = clubProduct.Stock,
                Status = clubProduct.Status,
                ReviewedBy = clubProduct.ReviewedBy,
                ReviewedAt = clubProduct.ReviewedAt,
                RejectionReason = clubProduct.RejectionReason,
                CreatedBy = clubProduct.CreatedBy,
                CreatedAt = clubProduct.CreatedAt,
                UpdatedAt = clubProduct.UpdatedAt,
                IsActive = clubProduct.IsActive
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<ClubProductResponse>?> GetByClubAsync(int clubId)
    {
        return await _context.ClubProducts
            .AsNoTracking()
            .Where(clubProduct => clubProduct.ClubId == clubId && clubProduct.Status == "Approved" && clubProduct.IsActive)
            .OrderByDescending(clubProduct => clubProduct.CreatedAt)
            .Select(clubProduct => new ClubProductResponse
            {
                ClubProductId = clubProduct.ClubProductId,
                ClubId = clubProduct.ClubId,
                Name = clubProduct.Name,
                Description = clubProduct.Description,
                Category = clubProduct.Category,
                Price = clubProduct.Price,
                Image = clubProduct.Image,
                Stock = clubProduct.Stock,
                Status = clubProduct.Status,
                ReviewedBy = clubProduct.ReviewedBy,
                ReviewedAt = clubProduct.ReviewedAt,
                RejectionReason = clubProduct.RejectionReason,
                CreatedBy = clubProduct.CreatedBy,
                CreatedAt = clubProduct.CreatedAt,
                UpdatedAt = clubProduct.UpdatedAt,
                IsActive = clubProduct.IsActive
            })
            .ToListAsync();
    }

    public async Task<List<ClubProductResponse>?> GetPendingAsync()
    {
        return await _context.ClubProducts
            .AsNoTracking()
            .Where(clubProduct => clubProduct.Status == "Pending" && clubProduct.IsActive)
            .OrderBy(clubProduct => clubProduct.CreatedAt)
            .Select(clubProduct => new ClubProductResponse
            {
                ClubProductId = clubProduct.ClubProductId,
                ClubId = clubProduct.ClubId,
                Name = clubProduct.Name,
                Description = clubProduct.Description,
                Category = clubProduct.Category,
                Price = clubProduct.Price,
                Image = clubProduct.Image,
                Stock = clubProduct.Stock,
                Status = clubProduct.Status,
                ReviewedBy = clubProduct.ReviewedBy,
                ReviewedAt = clubProduct.ReviewedAt,
                RejectionReason = clubProduct.RejectionReason,
                CreatedBy = clubProduct.CreatedBy,
                CreatedAt = clubProduct.CreatedAt,
                UpdatedAt = clubProduct.UpdatedAt,
                IsActive = clubProduct.IsActive
            })
            .ToListAsync();
    }

    public async Task<ClubProductResponse?> UpdateAsync(int clubProductId, UpdateClubProductRequest request)
    {
        var clubProduct = await _context.ClubProducts.FindAsync(clubProductId);

        if (clubProduct == null)
        {
            return null;
        }

        clubProduct.Name = request.Name.Trim();
        clubProduct.Description = request.Description.Trim();
        clubProduct.Category = request.Category.Trim();
        clubProduct.Price = request.Price;
        clubProduct.Image = request.Image;
        clubProduct.Stock = request.Stock;
        clubProduct.Status = "Pending";
        clubProduct.ReviewedBy = null;
        clubProduct.ReviewedAt = null;
        clubProduct.RejectionReason = null;
        clubProduct.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToResponse(clubProduct);
    }

    public async Task<ClubProductResponse?> ApproveAsync(int clubProductId, int reviewedBy)
    {
        var clubProduct = await _context.ClubProducts.FindAsync(clubProductId);

        if (clubProduct == null)
        {
            return null;
        }

        clubProduct.Status = "Approved";
        clubProduct.ReviewedBy = reviewedBy;
        clubProduct.ReviewedAt = DateTime.UtcNow;
        clubProduct.RejectionReason = null;

        await _context.SaveChangesAsync();

        return MapToResponse(clubProduct);
    }

    public async Task<ClubProductResponse?> RejectAsync(int clubProductId, int reviewedBy, string reason)
    {
        var clubProduct = await _context.ClubProducts.FindAsync(clubProductId);

        if (clubProduct == null)
        {
            return null;
        }

        clubProduct.Status = "Rejected";
        clubProduct.ReviewedBy = reviewedBy;
        clubProduct.ReviewedAt = DateTime.UtcNow;
        clubProduct.RejectionReason = reason;

        await _context.SaveChangesAsync();

        return MapToResponse(clubProduct);
    }

    public async Task<bool> DeleteAsync(int clubProductId)
    {
        var clubProduct = await _context.ClubProducts.FindAsync(clubProductId);

        if (clubProduct == null)
        {
            return false;
        }

        clubProduct.IsActive = false;
        clubProduct.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    private static ClubProductResponse MapToResponse(ClubProduct clubProduct)
    {
        return new ClubProductResponse
        {
            ClubProductId = clubProduct.ClubProductId,
            ClubId = clubProduct.ClubId,
            Name = clubProduct.Name,
            Description = clubProduct.Description,
            Category = clubProduct.Category,
            Price = clubProduct.Price,
            Image = clubProduct.Image,
            Stock = clubProduct.Stock,
            Status = clubProduct.Status,
            ReviewedBy = clubProduct.ReviewedBy,
            ReviewedAt = clubProduct.ReviewedAt,
            RejectionReason = clubProduct.RejectionReason,
            CreatedBy = clubProduct.CreatedBy,
            CreatedAt = clubProduct.CreatedAt,
            UpdatedAt = clubProduct.UpdatedAt,
            IsActive = clubProduct.IsActive
        };
    }
}