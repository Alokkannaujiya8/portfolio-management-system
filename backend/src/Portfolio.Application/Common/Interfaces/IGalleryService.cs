using Portfolio.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Portfolio.Application.Common.Interfaces
{
    public interface IGalleryService
    {
        Task<List<Gallery>> GetAllGalleryItemsAsync(string? type = null);
        Task<List<string>> GetGalleryCategoriesAsync();
        Task<Gallery?> GetGalleryItemByIdAsync(int id);
        Task IncrementGalleryViewCountAsync(int id);
        Task IncrementGalleryDownloadCountAsync(int id);

        // Admin CRUD
        Task<int> AddGalleryItemAsync(Gallery item);
        Task UpdateGalleryItemAsync(Gallery item);
        Task DeleteGalleryItemAsync(int id);
    }
}
