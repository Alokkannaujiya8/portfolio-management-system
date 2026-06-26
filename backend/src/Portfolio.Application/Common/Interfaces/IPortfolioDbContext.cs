using Microsoft.EntityFrameworkCore;
using Portfolio.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Portfolio.Application.Common.Interfaces
{
    public interface IPortfolioDbContext
    {
        DbSet<Admin> Admins { get; }
        DbSet<Profile> Profiles { get; }
        DbSet<Skill> Skills { get; }
        DbSet<Project> Projects { get; }
        DbSet<Experience> Experiences { get; }
        DbSet<Education> Educations { get; }
        DbSet<ContactMessage> ContactMessages { get; }
        DbSet<ResumeView> ResumeViews { get; }
        DbSet<BlogCategory> BlogCategories { get; }
        DbSet<BlogPost> BlogPosts { get; }
        DbSet<BlogImage> BlogImages { get; }
        DbSet<BlogVideo> BlogVideos { get; }
        DbSet<BlogComment> BlogComments { get; }
        DbSet<BlogLike> BlogLikes { get; }
        DbSet<ChatMessage> ChatMessages { get; }
        DbSet<VisitorTracking> VisitorTrackings { get; }
        DbSet<PageVisit> PageVisits { get; }
        DbSet<Gallery> Galleries { get; }
        DbSet<GalleryAlbum> GalleryAlbums { get; }
        DbSet<AlbumItem> AlbumItems { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
