using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Common.Interfaces;
using Portfolio.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Application.Services
{
    public class GalleryService : IGalleryService
    {
        private readonly IPortfolioDbContext _context;

        public GalleryService(IPortfolioDbContext context)
        {
            _context = context;
        }

        public async Task<List<Gallery>> GetAllGalleryItemsAsync(string? type = null)
        {
            var query = _context.Galleries.Where(g => g.IsActive);

            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(g => g.MediaType == type);
            }

            return await query.OrderBy(g => g.DisplayOrder).ToListAsync();
        }

        public async Task<List<string>> GetGalleryCategoriesAsync()
        {
            return await _context.Galleries
                .Where(g => g.IsActive && g.Category != null && g.Category != "")
                .Select(g => g.Category!)
                .Distinct()
                .ToListAsync();
        }

        public async Task<Gallery?> GetGalleryItemByIdAsync(int id)
        {
            return await _context.Galleries.FirstOrDefaultAsync(g => g.GalleryId == id && g.IsActive);
        }

        public async Task IncrementGalleryViewCountAsync(int id)
        {
            var item = await _context.Galleries.FindAsync(id);
            if (item != null)
            {
                item.ViewCount = (item.ViewCount ?? 0) + 1;
                await _context.SaveChangesAsync();
            }
        }

        public async Task IncrementGalleryDownloadCountAsync(int id)
        {
            var item = await _context.Galleries.FindAsync(id);
            if (item != null)
            {
                item.DownloadCount = (item.DownloadCount ?? 0) + 1;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> AddGalleryItemAsync(Gallery item)
        {
            item.CreatedDate = DateTime.UtcNow;
            item.UpdatedDate = DateTime.UtcNow;
            item.ViewCount = 0;
            item.DownloadCount = 0;
            item.IsActive = true;

            _context.Galleries.Add(item);
            await _context.SaveChangesAsync();
            return item.GalleryId;
        }

        public async Task UpdateGalleryItemAsync(Gallery item)
        {
            var existing = await _context.Galleries.FindAsync(item.GalleryId);
            if (existing != null)
            {
                existing.Title = item.Title;
                existing.Description = item.Description;
                existing.MediaType = item.MediaType;
                existing.Category = item.Category;
                existing.Tags = item.Tags;
                existing.DisplayOrder = item.DisplayOrder;
                existing.IsFeatured = item.IsFeatured;
                existing.UpdatedDate = DateTime.UtcNow;
                existing.IsActive = item.IsActive;

                if (!string.IsNullOrEmpty(item.MediaPath))
                {
                    existing.MediaPath = item.MediaPath;
                }

                if (!string.IsNullOrEmpty(item.ThumbnailPath))
                {
                    existing.ThumbnailPath = item.ThumbnailPath;
                }

                if (!string.IsNullOrEmpty(item.VideoEmbedCode))
                {
                    existing.VideoEmbedCode = item.VideoEmbedCode;
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteGalleryItemAsync(int id)
        {
            var item = await _context.Galleries.FindAsync(id);
            if (item != null)
            {
                item.IsActive = false;
                item.UpdatedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
