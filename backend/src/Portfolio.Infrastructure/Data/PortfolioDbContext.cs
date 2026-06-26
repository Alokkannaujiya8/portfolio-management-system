using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Common.Interfaces;
using Portfolio.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Portfolio.Infrastructure.Data
{
    public class PortfolioDbContext : DbContext, IPortfolioDbContext
    {
        public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options) : base(options)
        {
        }

        public DbSet<Admin> Admins => Set<Admin>();
        public DbSet<Profile> Profiles => Set<Profile>();
        public DbSet<Skill> Skills => Set<Skill>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<Experience> Experiences => Set<Experience>();
        public DbSet<Education> Educations => Set<Education>();
        public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();
        public DbSet<ResumeView> ResumeViews => Set<ResumeView>();
        public DbSet<BlogCategory> BlogCategories => Set<BlogCategory>();
        public DbSet<BlogPost> BlogPosts => Set<BlogPost>();
        public DbSet<BlogImage> BlogImages => Set<BlogImage>();
        public DbSet<BlogVideo> BlogVideos => Set<BlogVideo>();
        public DbSet<BlogComment> BlogComments => Set<BlogComment>();
        public DbSet<BlogLike> BlogLikes => Set<BlogLike>();
        public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
        public DbSet<VisitorTracking> VisitorTrackings => Set<VisitorTracking>();
        public DbSet<PageVisit> PageVisits => Set<PageVisit>();
        public DbSet<Gallery> Galleries => Set<Gallery>();
        public DbSet<GalleryAlbum> GalleryAlbums => Set<GalleryAlbum>();
        public DbSet<AlbumItem> AlbumItems => Set<AlbumItem>();

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Table mappings to match SQL Server schema exactly
            modelBuilder.Entity<Admin>().ToTable("Admin");
            modelBuilder.Entity<Profile>().ToTable("Profile");
            modelBuilder.Entity<Skill>().ToTable("Skills");
            modelBuilder.Entity<Project>().ToTable("Projects");
            modelBuilder.Entity<Experience>().ToTable("Experience");
            modelBuilder.Entity<Education>().ToTable("Education");
            modelBuilder.Entity<ContactMessage>().ToTable("ContactMessages");
            modelBuilder.Entity<ResumeView>().ToTable("ResumeViews");
            modelBuilder.Entity<BlogCategory>().ToTable("BlogCategories");
            modelBuilder.Entity<BlogPost>().ToTable("BlogPosts");
            modelBuilder.Entity<BlogImage>().ToTable("BlogImages");
            modelBuilder.Entity<BlogVideo>().ToTable("BlogVideos");
            modelBuilder.Entity<BlogComment>().ToTable("BlogComments");
            modelBuilder.Entity<BlogLike>().ToTable("BlogLikes");
            modelBuilder.Entity<ChatMessage>().ToTable("ChatMessages");
            modelBuilder.Entity<VisitorTracking>().ToTable("VisitorTracking");
            modelBuilder.Entity<PageVisit>().ToTable("PageVisits");
            modelBuilder.Entity<Gallery>().ToTable("Gallery");
            modelBuilder.Entity<GalleryAlbum>().ToTable("GalleryAlbums");
            modelBuilder.Entity<AlbumItem>().ToTable("AlbumItems");

            // Key configurations
            modelBuilder.Entity<Admin>().HasKey(a => a.AdminId);
            modelBuilder.Entity<Profile>().HasKey(p => p.ProfileId);
            modelBuilder.Entity<Skill>().HasKey(s => s.SkillId);
            modelBuilder.Entity<Project>().HasKey(p => p.ProjectId);
            modelBuilder.Entity<Experience>().HasKey(e => e.ExperienceId);
            modelBuilder.Entity<Education>().HasKey(e => e.EducationId);
            modelBuilder.Entity<ContactMessage>().HasKey(c => c.MessageId);
            modelBuilder.Entity<ResumeView>().HasKey(r => r.ViewId);
            modelBuilder.Entity<BlogCategory>().HasKey(b => b.CategoryId);
            modelBuilder.Entity<BlogPost>().HasKey(b => b.PostId);
            modelBuilder.Entity<BlogImage>().HasKey(b => b.ImageId);
            modelBuilder.Entity<BlogVideo>().HasKey(b => b.VideoId);
            modelBuilder.Entity<BlogComment>().HasKey(b => b.CommentId);
            modelBuilder.Entity<BlogLike>().HasKey(b => b.LikeId);
            modelBuilder.Entity<ChatMessage>().HasKey(c => c.ChatId);
            modelBuilder.Entity<VisitorTracking>().HasKey(v => v.VisitorId);
            modelBuilder.Entity<PageVisit>().HasKey(p => p.PageVisitId);
            modelBuilder.Entity<Gallery>().HasKey(g => g.GalleryId);
            modelBuilder.Entity<GalleryAlbum>().HasKey(g => g.AlbumId);
            modelBuilder.Entity<AlbumItem>().HasKey(a => a.AlbumItemId);

            // Configure Relationships
            modelBuilder.Entity<BlogPost>()
                .HasOne(b => b.Category)
                .WithMany(c => c.BlogPosts)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<BlogImage>()
                .HasOne(i => i.BlogPost)
                .WithMany(p => p.Images)
                .HasForeignKey(i => i.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BlogVideo>()
                .HasOne(v => v.BlogPost)
                .WithMany(p => p.Videos)
                .HasForeignKey(v => v.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BlogComment>()
                .HasOne(c => c.BlogPost)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BlogComment>()
                .HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BlogLike>()
                .HasOne(l => l.BlogPost)
                .WithMany()
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AlbumItem>()
                .HasOne(a => a.Album)
                .WithMany(al => al.AlbumItems)
                .HasForeignKey(a => a.AlbumId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AlbumItem>()
                .HasOne(a => a.Gallery)
                .WithMany(g => g.AlbumItems)
                .HasForeignKey(a => a.GalleryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Precision configurations for decimal fields
            modelBuilder.Entity<VisitorTracking>()
                .Property(v => v.Latitude)
                .HasPrecision(10, 8);

            modelBuilder.Entity<VisitorTracking>()
                .Property(v => v.Longitude)
                .HasPrecision(11, 8);

            modelBuilder.Entity<Education>()
                .Property(e => e.Percentage)
                .HasPrecision(5, 2);
        }
    }
}
