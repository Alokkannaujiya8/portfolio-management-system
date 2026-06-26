using Microsoft.Data.SqlClient;
using Portfolio_Management_System.Models;
using System.Data;

namespace Portfolio_Management_System.DAL
{
    public class DbHelper
    {
        private readonly string _connectionString;
        private readonly ILogger<DbHelper> _logger;

        public DbHelper(IConfiguration configuration, ILogger<DbHelper> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found.");
            _logger = logger;
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database connection failed");
                return false;
            }
        }
        // ==================== GALLERY METHODS ====================
        public async Task<List<GalleryModel>> GetAllGalleryItemsAsync(string? type = null)
        {
            var items = new List<GalleryModel>();
            try
            {
                using var conn = new SqlConnection(_connectionString);
                string query = "SELECT * FROM Gallery WHERE IsActive = 1";

                if (!string.IsNullOrEmpty(type))
                {
                    query += " AND MediaType = @Type";
                }
                query += " ORDER BY DisplayOrder, CreatedDate DESC";

                using var cmd = new SqlCommand(query, conn);
                if (!string.IsNullOrEmpty(type))
                {
                    cmd.Parameters.AddWithValue("@Type", type);
                }

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    items.Add(new GalleryModel
                    {
                        GalleryId = reader.GetInt32(0),
                        Title = reader.IsDBNull(1) ? null : reader.GetString(1),
                        Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                        MediaType = reader.IsDBNull(3) ? null : reader.GetString(3),
                        MediaPath = reader.IsDBNull(4) ? null : reader.GetString(4),
                        ThumbnailPath = reader.IsDBNull(5) ? null : reader.GetString(5),
                        VideoEmbedCode = reader.IsDBNull(6) ? null : reader.GetString(6),
                        Category = reader.IsDBNull(7) ? null : reader.GetString(7),
                        Tags = reader.IsDBNull(8) ? null : reader.GetString(8),
                        DisplayOrder = reader.GetInt32(9),
                        IsFeatured = reader.GetBoolean(10),
                        ViewCount = reader.GetInt32(11),
                        DownloadCount = reader.GetInt32(12),
                        IsActive = reader.GetBoolean(13),
                        CreatedDate = reader.GetDateTime(14),
                        UpdatedDate = reader.GetDateTime(15)
                    });
                }
                return items;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting gallery items");
                return items;
            }
        }

        public async Task<List<string>> GetGalleryCategoriesAsync()
        {
            var categories = new List<string>();
            try
            {
                using var conn = new SqlConnection(_connectionString);
                const string query = "SELECT DISTINCT Category FROM Gallery WHERE IsActive = 1 AND Category IS NOT NULL ORDER BY Category";

                using var cmd = new SqlCommand(query, conn);
                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    categories.Add(reader.GetString(0));
                }
                return categories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting gallery categories");
                return categories;
            }
        }

        public async Task<GalleryModel?> GetGalleryItemByIdAsync(int id)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                const string query = "SELECT * FROM Gallery WHERE GalleryId = @Id AND IsActive = 1";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new GalleryModel
                    {
                        GalleryId = reader.GetInt32(0),
                        Title = reader.IsDBNull(1) ? null : reader.GetString(1),
                        Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                        MediaType = reader.IsDBNull(3) ? null : reader.GetString(3),
                        MediaPath = reader.IsDBNull(4) ? null : reader.GetString(4),
                        ThumbnailPath = reader.IsDBNull(5) ? null : reader.GetString(5),
                        VideoEmbedCode = reader.IsDBNull(6) ? null : reader.GetString(6),
                        Category = reader.IsDBNull(7) ? null : reader.GetString(7),
                        Tags = reader.IsDBNull(8) ? null : reader.GetString(8),
                        DisplayOrder = reader.GetInt32(9),
                        IsFeatured = reader.GetBoolean(10),
                        ViewCount = reader.GetInt32(11),
                        DownloadCount = reader.GetInt32(12),
                        IsActive = reader.GetBoolean(13),
                        CreatedDate = reader.GetDateTime(14),
                        UpdatedDate = reader.GetDateTime(15)
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting gallery item by id");
                return null;
            }
        }
        public async Task IncrementGalleryViewCountAsync(int id)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                const string query = "UPDATE Gallery SET ViewCount = ViewCount + 1 WHERE GalleryId = @Id";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error incrementing view count");
            }
        }

        public async Task IncrementGalleryDownloadCountAsync(int id)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                const string query = "UPDATE Gallery SET DownloadCount = DownloadCount + 1 WHERE GalleryId = @Id";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error incrementing download count");
            }
        }
        // ==================== PROFILE METHODS ====================
        public async Task<ProfileModel?> GetProfileAsync()
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                const string query = "SELECT TOP 1 * FROM Profile WHERE IsActive = 1 ORDER BY ProfileId DESC";

                using var cmd = new SqlCommand(query, conn);
                await conn.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new ProfileModel
                    {
                        ProfileId = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Title = reader.GetString(2),
                        Description = reader.GetString(3),
                        Email = reader.GetString(4),
                        Phone = reader.GetString(5),
                        Address = reader.GetString(6),
                        LinkedIn = reader.IsDBNull(7) ? null : reader.GetString(7),
                        GitHub = reader.IsDBNull(8) ? null : reader.GetString(8),
                        Photo = reader.IsDBNull(9) ? null : reader.GetString(9),
                        ResumePath = reader.IsDBNull(10) ? null : reader.GetString(10),
                        IsActive = reader.GetBoolean(11),
                        CreatedDate = reader.GetDateTime(12),
                        UpdatedDate = reader.GetDateTime(13)
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting profile");
                return null;
            }
        }

        public async Task<int> UpdateProfileAsync(ProfileModel profile)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                const string query = @"
                    IF EXISTS(SELECT 1 FROM Profile WHERE IsActive = 1)
                        UPDATE Profile SET 
                            Name = @Name, 
                            Title = @Title, 
                            Description = @Description,
                            Email = @Email, 
                            Phone = @Phone, 
                            Address = @Address,
                            LinkedIn = @LinkedIn, 
                            GitHub = @GitHub,
                            Photo = @Photo, 
                            ResumePath = @ResumePath,
                            UpdatedDate = GETDATE()
                        WHERE ProfileId = (SELECT TOP 1 ProfileId FROM Profile WHERE IsActive = 1)
                    ELSE
                        INSERT INTO Profile (Name, Title, Description, Email, Phone, Address, LinkedIn, GitHub, Photo, ResumePath, IsActive, CreatedDate, UpdatedDate)
                        VALUES (@Name, @Title, @Description, @Email, @Phone, @Address, @LinkedIn, @GitHub, @Photo, @ResumePath, 1, GETDATE(), GETDATE());
                    SELECT SCOPE_IDENTITY();";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", profile.Name);
                cmd.Parameters.AddWithValue("@Title", profile.Title);
                cmd.Parameters.AddWithValue("@Description", profile.Description);
                cmd.Parameters.AddWithValue("@Email", profile.Email);
                cmd.Parameters.AddWithValue("@Phone", profile.Phone ?? "");
                cmd.Parameters.AddWithValue("@Address", profile.Address ?? "");
                cmd.Parameters.AddWithValue("@LinkedIn", profile.LinkedIn ?? "");
                cmd.Parameters.AddWithValue("@GitHub", profile.GitHub ?? "");
                cmd.Parameters.AddWithValue("@Photo", profile.Photo ?? "");
                cmd.Parameters.AddWithValue("@ResumePath", profile.ResumePath ?? "");

                await conn.OpenAsync();
                return Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating profile");
                throw;
            }
        }

        // ==================== SKILLS METHODS ====================
        public async Task<List<SkillModel>> GetAllSkillsAsync()
        {
            var skills = new List<SkillModel>();
            try
            {
                using var conn = new SqlConnection(_connectionString);
                const string query = "SELECT * FROM Skills WHERE IsActive = 1 ORDER BY SkillName";

                using var cmd = new SqlCommand(query, conn);
                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    skills.Add(new SkillModel
                    {
                        SkillId = reader.GetInt32(0),
                        SkillName = reader.GetString(1),
                        Percentage = reader.GetInt32(2),
                        IsActive = reader.GetBoolean(3),
                        CreatedDate = reader.GetDateTime(4),
                        UpdatedDate = reader.GetDateTime(5)
                    });
                }
                return skills;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting skills");
                return skills;
            }
        }

        public async Task<int> AddSkillAsync(SkillModel skill)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                const string query = @"
                    INSERT INTO Skills (SkillName, Percentage, IsActive, CreatedDate, UpdatedDate)
                    VALUES (@SkillName, @Percentage, 1, GETDATE(), GETDATE());
                    SELECT SCOPE_IDENTITY();";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SkillName", skill.SkillName);
                cmd.Parameters.AddWithValue("@Percentage", skill.Percentage);

                await conn.OpenAsync();
                return Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding skill");
                throw;
            }
        }

        public async Task UpdateSkillAsync(SkillModel skill)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                const string query = @"
                    UPDATE Skills 
                    SET SkillName = @SkillName, 
                        Percentage = @Percentage, 
                        UpdatedDate = GETDATE()
                    WHERE SkillId = @SkillId AND IsActive = 1";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SkillId", skill.SkillId);
                cmd.Parameters.AddWithValue("@SkillName", skill.SkillName);
                cmd.Parameters.AddWithValue("@Percentage", skill.Percentage);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating skill");
                throw;
            }
        }

        public async Task DeleteSkillAsync(int skillId)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                const string query = "UPDATE Skills SET IsActive = 0, UpdatedDate = GETDATE() WHERE SkillId = @SkillId";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SkillId", skillId);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting skill");
                throw;
            }
        }

        // ==================== PROJECTS METHODS ====================
        public async Task<List<ProjectModel>> GetAllProjectsAsync()
        {
            var projects = new List<ProjectModel>();
            try
            {
                using var conn = new SqlConnection(_connectionString);
                const string query = "SELECT * FROM Projects WHERE IsActive = 1 ORDER BY ProjectId DESC";

                using var cmd = new SqlCommand(query, conn);
                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    projects.Add(new ProjectModel
                    {
                        ProjectId = reader.GetInt32(0),
                        ProjectName = reader.GetString(1),
                        Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
                        ImagePath = reader.IsDBNull(3) ? null : reader.GetString(3),
                        GitHubLink = reader.IsDBNull(4) ? null : reader.GetString(4),
                        LiveLink = reader.IsDBNull(5) ? null : reader.GetString(5),
                        IsActive = reader.GetBoolean(6),
                        CreatedDate = reader.GetDateTime(7),
                        UpdatedDate = reader.GetDateTime(8)
                    });
                }
                return projects;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting projects");
                return projects;
            }
        }

        public async Task<int> AddProjectAsync(ProjectModel project)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                const string query = @"
                    INSERT INTO Projects (ProjectName, Description, ImagePath, GitHubLink, LiveLink, IsActive, CreatedDate, UpdatedDate)
                    VALUES (@ProjectName, @Description, @ImagePath, @GitHubLink, @LiveLink, 1, GETDATE(), GETDATE());
                    SELECT SCOPE_IDENTITY();";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ProjectName", project.ProjectName);
                cmd.Parameters.AddWithValue("@Description", project.Description);
                cmd.Parameters.AddWithValue("@ImagePath", project.ImagePath ?? "");
                cmd.Parameters.AddWithValue("@GitHubLink", project.GitHubLink ?? "");
                cmd.Parameters.AddWithValue("@LiveLink", project.LiveLink ?? "");

                await conn.OpenAsync();
                return Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding project");
                throw;
            }
        }

        public async Task UpdateProjectAsync(ProjectModel project)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                const string query = @"
                    UPDATE Projects SET
                        ProjectName = @ProjectName,
                        Description = @Description,
                        ImagePath = @ImagePath,
                        GitHubLink = @GitHubLink,
                        LiveLink = @LiveLink,
                        UpdatedDate = GETDATE()
                    WHERE ProjectId = @ProjectId AND IsActive = 1";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ProjectId", project.ProjectId);
                cmd.Parameters.AddWithValue("@ProjectName", project.ProjectName);
                cmd.Parameters.AddWithValue("@Description", project.Description);
                cmd.Parameters.AddWithValue("@ImagePath", project.ImagePath ?? "");
                cmd.Parameters.AddWithValue("@GitHubLink", project.GitHubLink ?? "");
                cmd.Parameters.AddWithValue("@LiveLink", project.LiveLink ?? "");

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating project");
                throw;
            }
        }

        public async Task DeleteProjectAsync(int projectId)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                const string query = "UPDATE Projects SET IsActive = 0, UpdatedDate = GETDATE() WHERE ProjectId = @ProjectId";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ProjectId", projectId);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting project");
                throw;
            }
        }

       
        // ==================== EDUCATION METHODS ====================
        public async Task<List<EducationModel>> GetAllEducationAsync()
        {
            var educations = new List<EducationModel>();
            try
            {
                using var conn = new SqlConnection(_connectionString);
                const string query = "SELECT * FROM Education WHERE IsActive = 1 ORDER BY Year DESC";

                using var cmd = new SqlCommand(query, conn);
                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    educations.Add(new EducationModel
                    {
                        EducationId = reader.GetInt32(0),
                        Degree = reader.GetString(1),
                        Institute = reader.GetString(2),
                        Year = reader.GetInt32(3),
                        Percentage = reader.IsDBNull(4) ? null : reader.GetDecimal(4),
                        IsActive = reader.GetBoolean(5),
                        CreatedDate = reader.GetDateTime(6),
                        UpdatedDate = reader.GetDateTime(7)
                    });
                }
                return educations;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting education");
                return educations;
            }
        }

        // ==================== CONTACT METHODS ====================
        public async Task<int> SaveContactMessageAsync(ContactMessageModel message)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                const string query = @"
                    INSERT INTO ContactMessages (Name, Email, Subject, Message, IPAddress, UserAgent, IsActive, CreatedDate, UpdatedDate)
                    VALUES (@Name, @Email, @Subject, @Message, @IPAddress, @UserAgent, 1, GETDATE(), GETDATE());
                    SELECT SCOPE_IDENTITY();";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", message.Name);
                cmd.Parameters.AddWithValue("@Email", message.Email);
                cmd.Parameters.AddWithValue("@Subject", message.Subject ?? "");
                cmd.Parameters.AddWithValue("@Message", message.Message);
                cmd.Parameters.AddWithValue("@IPAddress", message.IPAddress ?? "");
                cmd.Parameters.AddWithValue("@UserAgent", message.UserAgent ?? "");

                await conn.OpenAsync();
                return Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving message");
                throw;
            }
        }

        public async Task<List<ContactMessageModel>> GetAllMessagesAsync()
        {
            var messages = new List<ContactMessageModel>();
            try
            {
                using var conn = new SqlConnection(_connectionString);
                const string query = "SELECT * FROM ContactMessages WHERE IsActive = 1 ORDER BY CreatedDate DESC";

                using var cmd = new SqlCommand(query, conn);
                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    messages.Add(new ContactMessageModel
                    {
                        MessageId = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Email = reader.GetString(2),
                        Subject = reader.IsDBNull(3) ? "" : reader.GetString(3),
                        Message = reader.GetString(4),
                        IsRead = reader.GetBoolean(5),
                        IsReplied = reader.GetBoolean(6),
                        ReplyMessage = reader.IsDBNull(7) ? null : reader.GetString(7),
                        RepliedDate = reader.IsDBNull(8) ? null : reader.GetDateTime(8),
                        IPAddress = reader.IsDBNull(9) ? null : reader.GetString(9),
                        UserAgent = reader.IsDBNull(10) ? null : reader.GetString(10),
                        IsActive = reader.GetBoolean(11),
                        CreatedDate = reader.GetDateTime(12),
                        UpdatedDate = reader.GetDateTime(13)
                    });
                }
                return messages;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting messages");
                return messages;
            }
        }

        public async Task MarkMessageAsReadAsync(int messageId)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                const string query = "UPDATE ContactMessages SET IsRead = 1, UpdatedDate = GETDATE() WHERE MessageId = @MessageId";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MessageId", messageId);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking message as read");
                throw;
            }
        }

        // ==================== AUTH METHODS ====================
        public async Task<bool> ValidateAdminAsync(string username, string password)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                const string query = "SELECT COUNT(*) FROM Admin WHERE Username = @Username AND Password = @Password AND IsActive = 1";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                await conn.OpenAsync();
                var count = (int)await cmd.ExecuteScalarAsync();
                return count > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating admin");
                return false;
            }
        }


        public async Task<List<ResumeViewModel>> GetAllResumeViewsAsync()
        {
            var views = new List<ResumeViewModel>();
            try
            {
                using var conn = new SqlConnection(_connectionString);
                const string query = "SELECT * FROM ResumeViews WHERE IsActive = 1 ORDER BY ViewDate DESC";

                using var cmd = new SqlCommand(query, conn);
                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    views.Add(new ResumeViewModel
                    {
                        ViewId = reader.GetInt32(0),
                        VisitorName = reader.IsDBNull(1) ? null : reader.GetString(1),
                        VisitorEmail = reader.IsDBNull(2) ? null : reader.GetString(2),
                        CompanyName = reader.IsDBNull(3) ? null : reader.GetString(3),
                        Designation = reader.IsDBNull(4) ? null : reader.GetString(4),
                        ViewDate = reader.GetDateTime(5),
                        IPAddress = reader.IsDBNull(6) ? null : reader.GetString(6),
                        UserAgent = reader.IsDBNull(7) ? null : reader.GetString(7),
                        Country = reader.IsDBNull(8) ? null : reader.GetString(8),
                        City = reader.IsDBNull(9) ? null : reader.GetString(9),
                        IsDownloaded = reader.GetBoolean(10),
                        DownloadDate = reader.IsDBNull(11) ? null : reader.GetDateTime(11),
                        IsActive = reader.GetBoolean(12),
                        CreatedDate = reader.GetDateTime(13)
                    });
                }
                return views;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting resume views");
                return views;
            }
        }

        public async Task<List<BlogPostModel>> GetRecentBlogPostsAsync(int count)
        {
            var posts = new List<BlogPostModel>();
            try
            {
                using var conn = new SqlConnection(_connectionString);
                const string query = @"
                    SELECT TOP (@Count) PostId, Title, Slug, Excerpt, Content, FeaturedImage, CreatedDate, ViewCount
                    FROM BlogPosts WHERE IsActive = 1 AND IsPublished = 1 
                    ORDER BY CreatedDate DESC";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Count", count);

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    posts.Add(new BlogPostModel
                    {
                        PostId = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Slug = reader.GetString(2),
                        Excerpt = reader.IsDBNull(3) ? null : reader.GetString(3),
                        Content = reader.GetString(4),
                        FeaturedImage = reader.IsDBNull(5) ? null : reader.GetString(5),
                        CreatedDate = reader.GetDateTime(6),
                        ViewCount = reader.GetInt32(7)
                    });
                }
                return posts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting blog posts");
                return posts;
            }
        }


        public async Task<List<BlogPostModel>> GetPublishedBlogPostsAsync(int? categoryId = null, string? tag = null, string? search = null, int page = 1, int pageSize = 6)
        {
            var posts = new List<BlogPostModel>();
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var sql = @"
            SELECT p.*, c.CategoryName, c.Slug AS CategorySlug,
                   (SELECT COUNT(*) FROM BlogImages WHERE PostId = p.PostId AND IsActive = 1) AS ImageCount,
                   (SELECT COUNT(*) FROM BlogVideos WHERE PostId = p.PostId AND IsActive = 1) AS VideoCount,
                   (SELECT COUNT(*) FROM BlogComments WHERE PostId = p.PostId AND IsActive = 1 AND IsApproved = 1) AS ApprovedCommentCount
            FROM BlogPosts p
            LEFT JOIN BlogCategories c ON p.CategoryId = c.CategoryId
            WHERE p.IsActive = 1 AND p.IsPublished = 1";

                var parameters = new List<SqlParameter>();

                if (categoryId.HasValue)
                {
                    sql += " AND p.CategoryId = @CategoryId";
                    parameters.Add(new SqlParameter("@CategoryId", categoryId.Value));
                }

                if (!string.IsNullOrEmpty(tag))
                {
                    sql += " AND p.Tags LIKE @Tag";
                    parameters.Add(new SqlParameter("@Tag", $"%{tag}%"));
                }

                if (!string.IsNullOrEmpty(search))
                {
                    sql += " AND (p.Title LIKE @Search OR p.Content LIKE @Search OR p.Excerpt LIKE @Search)";
                    parameters.Add(new SqlParameter("@Search", $"%{search}%"));
                }

                sql += " ORDER BY p.IsFeatured DESC, p.PublishedDate DESC, p.CreatedDate DESC";
                sql += " OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

                parameters.Add(new SqlParameter("@Offset", (page - 1) * pageSize));
                parameters.Add(new SqlParameter("@PageSize", pageSize));

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddRange(parameters.ToArray());

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    posts.Add(MapBlogPost(reader));
                }
                return posts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting published blog posts");
                return posts;
            }
        }

        // Get total blog posts count for pagination
        public async Task<int> GetTotalBlogPostsCountAsync(int? categoryId = null, string? tag = null, string? search = null)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var sql = "SELECT COUNT(*) FROM BlogPosts WHERE IsActive = 1 AND IsPublished = 1";

                var parameters = new List<SqlParameter>();

                if (categoryId.HasValue)
                {
                    sql += " AND CategoryId = @CategoryId";
                    parameters.Add(new SqlParameter("@CategoryId", categoryId.Value));
                }

                if (!string.IsNullOrEmpty(tag))
                {
                    sql += " AND Tags LIKE @Tag";
                    parameters.Add(new SqlParameter("@Tag", $"%{tag}%"));
                }

                if (!string.IsNullOrEmpty(search))
                {
                    sql += " AND (Title LIKE @Search OR Content LIKE @Search OR Excerpt LIKE @Search)";
                    parameters.Add(new SqlParameter("@Search", $"%{search}%"));
                }

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddRange(parameters.ToArray());

                await conn.OpenAsync();
                return (int)await cmd.ExecuteScalarAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting blog posts count");
                return 0;
            }
        }


        // Get blog post by id
        public async Task<BlogPostModel?> GetBlogPostByIdAsync(int id)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var sql = @"
            SELECT p.*, c.CategoryName, c.Slug AS CategorySlug
            FROM BlogPosts p
            LEFT JOIN BlogCategories c ON p.CategoryId = c.CategoryId
            WHERE p.PostId = @PostId AND p.IsActive = 1";

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@PostId", id);

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return MapBlogPost(reader);
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting blog post by id: {Id}", id);
                return null;
            }
        }

        public async Task<List<BlogCategoryModel>> GetAllBlogCategoriesAsync()
        {
            var categories = new List<BlogCategoryModel>();
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var sql = "SELECT * FROM BlogCategories WHERE IsActive = 1 ORDER BY DisplayOrder, CategoryName";

                using var cmd = new SqlCommand(sql, conn);
                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    categories.Add(new BlogCategoryModel
                    {
                        CategoryId = reader.GetInt32(0),
                        CategoryName = reader.GetString(1),
                        Slug = reader.GetString(2),
                        Description = reader.IsDBNull(3) ? null : reader.GetString(3),
                        Icon = reader.IsDBNull(4) ? null : reader.GetString(4),
                        DisplayOrder = reader.GetInt32(5),
                        IsActive = reader.GetBoolean(6),
                        CreatedDate = reader.GetDateTime(7),
                        UpdatedDate = reader.GetDateTime(8)
                    });
                }
                return categories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting blog categories");
                return categories;
            }
        }

        // Get blog post count by category
        public async Task<int> GetBlogPostCountByCategoryAsync(int categoryId)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var sql = "SELECT COUNT(*) FROM BlogPosts WHERE CategoryId = @CategoryId AND IsActive = 1 AND IsPublished = 1";

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);

                await conn.OpenAsync();
                return (int)await cmd.ExecuteScalarAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting blog post count by category");
                return 0;
            }
        }

        // Get popular blog posts
        public async Task<List<BlogPostModel>> GetPopularBlogPostsAsync(int count)
        {
            var posts = new List<BlogPostModel>();
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var sql = @"
            SELECT TOP (@Count) * FROM BlogPosts 
            WHERE IsActive = 1 AND IsPublished = 1 
            ORDER BY ViewCount DESC, LikeCount DESC, CreatedDate DESC";

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Count", count);

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    posts.Add(MapBlogPost(reader));
                }
                return posts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting popular blog posts");
                return posts;
            }
        }

        // Get related blog posts
        public async Task<List<BlogPostModel>> GetRelatedBlogPostsAsync(int postId, int categoryId, int count)
        {
            var posts = new List<BlogPostModel>();
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var sql = @"
            SELECT TOP (@Count) * FROM BlogPosts 
            WHERE PostId != @PostId AND CategoryId = @CategoryId 
            AND IsActive = 1 AND IsPublished = 1
            ORDER BY CreatedDate DESC";

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@PostId", postId);
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                cmd.Parameters.AddWithValue("@Count", count);

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    posts.Add(MapBlogPost(reader));
                }
                return posts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting related blog posts");
                return posts;
            }
        }

        // Get blog images
        public async Task<List<BlogImageModel>> GetBlogImagesAsync(int postId)
        {
            var images = new List<BlogImageModel>();
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var sql = "SELECT * FROM BlogImages WHERE PostId = @PostId AND IsActive = 1 ORDER BY DisplayOrder, CreatedDate";

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@PostId", postId);

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    images.Add(new BlogImageModel
                    {
                        ImageId = reader.GetInt32(0),
                        PostId = reader.GetInt32(1),
                        ImagePath = reader.GetString(2),
                        ThumbnailPath = reader.IsDBNull(3) ? null : reader.GetString(3),
                        Caption = reader.IsDBNull(4) ? null : reader.GetString(4),
                        AltText = reader.IsDBNull(5) ? null : reader.GetString(5),
                        DisplayOrder = reader.GetInt32(6),
                        IsCover = reader.GetBoolean(7),
                        IsActive = reader.GetBoolean(8),
                        CreatedDate = reader.GetDateTime(9)
                    });
                }
                return images;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting blog images");
                return images;
            }
        }

        // Get blog videos
        public async Task<List<BlogVideoModel>> GetBlogVideosAsync(int postId)
        {
            var videos = new List<BlogVideoModel>();
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var sql = "SELECT * FROM BlogVideos WHERE PostId = @PostId AND IsActive = 1 ORDER BY DisplayOrder, CreatedDate";

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@PostId", postId);

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    videos.Add(new BlogVideoModel
                    {
                        VideoId = reader.GetInt32(0),
                        PostId = reader.GetInt32(1),
                        VideoTitle = reader.IsDBNull(2) ? null : reader.GetString(2),
                        VideoUrl = reader.IsDBNull(3) ? null : reader.GetString(3),
                        VideoEmbedCode = reader.IsDBNull(4) ? null : reader.GetString(4),
                        VideoType = reader.IsDBNull(5) ? null : reader.GetString(5),
                        ThumbnailPath = reader.IsDBNull(6) ? null : reader.GetString(6),
                        DisplayOrder = reader.GetInt32(7),
                        IsActive = reader.GetBoolean(8),
                        CreatedDate = reader.GetDateTime(9)
                    });
                }
                return videos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting blog videos");
                return videos;
            }
        }

        // Get blog comments
        public async Task<List<BlogCommentModel>> GetBlogCommentsAsync(int postId)
        {
            var comments = new List<BlogCommentModel>();
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var sql = @"
            SELECT c.*, vt.Country, vt.City 
            FROM BlogComments c
            LEFT JOIN VisitorTracking vt ON c.VisitorId = vt.VisitorId
            WHERE c.PostId = @PostId AND c.IsActive = 1 AND c.IsApproved = 1
            ORDER BY c.CreatedDate DESC";

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@PostId", postId);

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    comments.Add(new BlogCommentModel
                    {
                        CommentId = reader.GetInt32(0),
                        PostId = reader.GetInt32(1),
                        ParentCommentId = reader.IsDBNull(2) ? null : (int?)reader.GetInt32(2),
                        VisitorId = reader.IsDBNull(3) ? null : (int?)reader.GetInt32(3),
                        Name = reader.GetString(4),
                        Email = reader.IsDBNull(5) ? null : reader.GetString(5),
                        Website = reader.IsDBNull(6) ? null : reader.GetString(6),
                        Comment = reader.GetString(7),
                        IsApproved = reader.GetBoolean(8),
                        IsSpam = reader.GetBoolean(9),
                        IPAddress = reader.IsDBNull(10) ? null : reader.GetString(10),
                        LikeCount = reader.GetInt32(11),
                        IsActive = reader.GetBoolean(12),
                        CreatedDate = reader.GetDateTime(13),
                        Country = reader.IsDBNull(14) ? null : reader.GetString(14),
                        City = reader.IsDBNull(15) ? null : reader.GetString(15)
                    });
                }
                return comments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting blog comments");
                return comments;
            }
        }

        // Add blog comment
        public async Task<int> AddBlogCommentAsync(BlogCommentModel comment)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var sql = @"
            INSERT INTO BlogComments (PostId, ParentCommentId, VisitorId, Name, Email, Website, Comment, IPAddress, IsActive, CreatedDate)
            VALUES (@PostId, @ParentCommentId, @VisitorId, @Name, @Email, @Website, @Comment, @IPAddress, 1, GETDATE());
            SELECT SCOPE_IDENTITY();";

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@PostId", comment.PostId);
                cmd.Parameters.AddWithValue("@ParentCommentId", comment.ParentCommentId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@VisitorId", comment.VisitorId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Name", comment.Name);
                cmd.Parameters.AddWithValue("@Email", comment.Email ?? "");
                cmd.Parameters.AddWithValue("@Website", comment.Website ?? "");
                cmd.Parameters.AddWithValue("@Comment", comment.Comment);
                cmd.Parameters.AddWithValue("@IPAddress", comment.IPAddress ?? "");

                await conn.OpenAsync();
                return Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding blog comment");
                throw;
            }
        }


        private BlogPostModel MapBlogPost(SqlDataReader reader)
        {
            return new BlogPostModel
            {
                PostId = reader.GetInt32(0),
                CategoryId = reader.IsDBNull(1) ? null : (int?)reader.GetInt32(1),
                Title = reader.GetString(2),
                Slug = reader.GetString(3),
                Excerpt = reader.IsDBNull(4) ? null : reader.GetString(4),
                Content = reader.GetString(5),
                FeaturedImage = reader.IsDBNull(6) ? null : reader.GetString(6),
                VideoUrl = reader.IsDBNull(7) ? null : reader.GetString(7),
                VideoEmbedCode = reader.IsDBNull(8) ? null : reader.GetString(8),
                Tags = reader.IsDBNull(9) ? null : reader.GetString(9),

                // ✅ SEO Properties
                MetaTitle = reader.IsDBNull(10) ? null : reader.GetString(10),
                MetaDescription = reader.IsDBNull(11) ? null : reader.GetString(11),
                MetaKeywords = reader.IsDBNull(12) ? null : reader.GetString(12),

                ViewCount = reader.GetInt32(13),
                LikeCount = reader.GetInt32(14),
                CommentCount = reader.GetInt32(15),

                IsPublished = reader.GetBoolean(16),
                PublishedDate = reader.IsDBNull(17) ? null : (DateTime?)reader.GetDateTime(17),
                IsFeatured = reader.GetBoolean(18),

                IsActive = reader.GetBoolean(19),
                CreatedDate = reader.GetDateTime(20),
                UpdatedDate = reader.GetDateTime(21),

                Category = reader.FieldCount > 22 && !reader.IsDBNull(22) ? new BlogCategoryModel
                {
                    CategoryName = reader.GetString(22),
                    Slug = reader.GetString(23)
                } : null
            };
        }
        // ==================== DASHBOARD STATS - DYNAMIC ====================
        public async Task<DashboardViewModel> GetDashboardStatsAsync()
        {
            var dashboard = new DashboardViewModel();
            try
            {
                using var conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();

                // 1. Get Basic Counts
                var countQuery = @"
            SELECT 
                (SELECT COUNT(*) FROM Skills WHERE IsActive = 1) AS TotalSkills,
                (SELECT COUNT(*) FROM Projects WHERE IsActive = 1) AS TotalProjects,
                (SELECT COUNT(*) FROM Experience WHERE IsActive = 1) AS TotalExperience,
                (SELECT COUNT(*) FROM Education WHERE IsActive = 1) AS TotalEducation,
                (SELECT COUNT(*) FROM ContactMessages WHERE IsActive = 1) AS TotalMessages,
                (SELECT COUNT(*) FROM ContactMessages WHERE IsActive = 1 AND IsRead = 0) AS UnreadMessages,
                (SELECT COUNT(*) FROM ResumeViews WHERE IsActive = 1) AS TotalResumeViews,
                (SELECT COUNT(*) FROM ResumeViews WHERE IsDownloaded = 1) AS TotalDownloads,
                (SELECT COUNT(*) FROM VisitorTracking WHERE IsActive = 1) AS TotalVisitors,
                (SELECT COUNT(*) FROM PageVisits WHERE IsActive = 1) AS TotalPageViews,
                (SELECT COUNT(DISTINCT VisitorId) FROM PageVisits WHERE CAST(VisitTime AS DATE) = CAST(GETDATE() AS DATE)) AS TodayVisitors,
                (SELECT COUNT(DISTINCT VisitorId) FROM ChatMessages WHERE CreatedDate >= DATEADD(hour, -24, GETDATE())) AS ActiveChats,
                (SELECT COUNT(*) FROM BlogPosts WHERE IsActive = 1) AS TotalBlogPosts,
                (SELECT COUNT(*) FROM Gallery WHERE IsActive = 1) AS TotalGalleryItems,
                (SELECT COUNT(*) FROM BlogComments WHERE IsActive = 1 AND IsApproved = 0) AS PendingComments";

                using (var cmd = new SqlCommand(countQuery, conn))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        dashboard.TotalSkills = reader.GetInt32(0);
                        dashboard.TotalProjects = reader.GetInt32(1);
                        dashboard.TotalExperience = reader.GetInt32(2);
                        dashboard.TotalEducation = reader.GetInt32(3);
                        dashboard.TotalMessages = reader.GetInt32(4);
                        dashboard.UnreadMessages = reader.GetInt32(5);
                        dashboard.TotalResumeViews = reader.GetInt32(6);
                        dashboard.TotalDownloads = reader.GetInt32(7);
                        dashboard.TotalVisitors = reader.GetInt32(8);
                        dashboard.TotalPageViews = reader.GetInt32(9);
                        dashboard.TodayVisitors = reader.GetInt32(10);
                        dashboard.ActiveChats = reader.GetInt32(11);
                        dashboard.TotalBlogPosts = reader.GetInt32(12);
                        dashboard.TotalGalleryItems = reader.GetInt32(13);
                        dashboard.PendingComments = reader.GetInt32(14);
                    }
                }

                // 2. Get Recent Messages
                dashboard.RecentMessages = await GetRecentMessagesAsync(5);

                // 3. Get Recent Resume Views
                dashboard.RecentResumeViews = (await GetAllResumeViewsAsync()).Take(5).ToList();

                // 4. Get Recent Visitors
                dashboard.RecentVisitors = await GetRecentVisitorsAsync(5);

                // 5. Get Recent Blog Posts
                dashboard.RecentBlogPosts = await GetRecentBlogPostsForDashboardAsync(5);

                // 6. Get Visitors Chart Data (Last 7 days)
                var visitorChartQuery = @"
            SELECT 
                FORMAT(VisitTime, 'MMM dd') AS DateLabel,
                COUNT(*) AS VisitCount
            FROM PageVisits
            WHERE VisitTime >= DATEADD(day, -6, GETDATE())
            GROUP BY FORMAT(VisitTime, 'MMM dd'), CAST(VisitTime AS DATE)
            ORDER BY MIN(VisitTime)";

                using (var cmd = new SqlCommand(visitorChartQuery, conn))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    int index = 0;
                    while (await reader.ReadAsync() && index < 7)
                    {
                        dashboard.VisitorChartLabels[index] = reader.GetString(0);
                        dashboard.VisitorChartData[index] = reader.GetInt32(1);
                        index++;
                    }

                    // Fill remaining days with 0 if needed
                    while (index < 7)
                    {
                        dashboard.VisitorChartLabels[index] = DateTime.Now.AddDays(index - 6).ToString("MMM dd");
                        dashboard.VisitorChartData[index] = 0;
                        index++;
                    }
                }

                // 7. Get Traffic Sources
                var sourcesQuery = @"
            SELECT 
                CASE 
                    WHEN UserAgent LIKE '%bot%' THEN 'Bots'
                    WHEN UserAgent LIKE '%mobile%' THEN 'Mobile'
                    ELSE 'Desktop'
                END AS DeviceType,
                COUNT(*) AS Count
            FROM VisitorTracking
            WHERE CreatedDate >= DATEADD(day, -30, GETDATE())
            GROUP BY 
                CASE 
                    WHEN UserAgent LIKE '%bot%' THEN 'Bots'
                    WHEN UserAgent LIKE '%mobile%' THEN 'Mobile'
                    ELSE 'Desktop'
                END";

                var sources = new Dictionary<string, int>();
                using (var cmd = new SqlCommand(sourcesQuery, conn))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        sources[reader.GetString(0)] = reader.GetInt32(1);
                    }
                }

                dashboard.SourceLabels = new[] { "Desktop", "Mobile", "Bots" };
                dashboard.SourceData = new[]
                {
            sources.GetValueOrDefault("Desktop", 0),
            sources.GetValueOrDefault("Mobile", 0),
            sources.GetValueOrDefault("Bots", 0)
        };

                // 8. Get Resume Downloads Chart Data
                var resumeChartQuery = @"
            SELECT 
                FORMAT(ViewDate, 'MMM dd') AS DateLabel,
                COUNT(*) AS DownloadCount
            FROM ResumeViews
            WHERE ViewDate >= DATEADD(day, -6, GETDATE())
            GROUP BY FORMAT(ViewDate, 'MMM dd'), CAST(ViewDate AS DATE)
            ORDER BY MIN(ViewDate)";

                using (var cmd = new SqlCommand(resumeChartQuery, conn))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    int index = 0;
                    while (await reader.ReadAsync() && index < 7)
                    {
                        dashboard.ResumeChartLabels[index] = reader.GetString(0);
                        dashboard.ResumeChartData[index] = reader.GetInt32(1);
                        index++;
                    }

                    while (index < 7)
                    {
                        dashboard.ResumeChartLabels[index] = DateTime.Now.AddDays(index - 6).ToString("MMM dd");
                        dashboard.ResumeChartData[index] = 0;
                        index++;
                    }
                }

                // 9. Get Popular Pages
                var pagesQuery = @"
            SELECT TOP 5
                PageUrl,
                PageTitle,
                COUNT(*) AS ViewCount
            FROM PageVisits
            WHERE PageUrl IS NOT NULL
            GROUP BY PageUrl, PageTitle
            ORDER BY ViewCount DESC";

                using (var cmd = new SqlCommand(pagesQuery, conn))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        dashboard.PopularPages.Add(new PopularPageModel
                        {
                            PageName = reader.IsDBNull(1) ? "Unknown" : reader.GetString(1),
                            PageUrl = reader.GetString(0),
                            ViewCount = reader.GetInt32(2),
                            Trend = "+" + new Random().Next(1, 20) + "%" // Random trend for demo
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard stats");
            }
            return dashboard;
        }

        // Helper method to get recent messages
        private async Task<List<ContactMessageModel>> GetRecentMessagesAsync(int count)
        {
            var messages = new List<ContactMessageModel>();
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var query = @"
            SELECT TOP (@Count) * 
            FROM ContactMessages 
            WHERE IsActive = 1 
            ORDER BY CreatedDate DESC";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Count", count);

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    messages.Add(new ContactMessageModel
                    {
                        MessageId = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Email = reader.GetString(2),
                        Subject = reader.IsDBNull(3) ? "" : reader.GetString(3),
                        Message = reader.GetString(4),
                        IsRead = reader.GetBoolean(5),
                        CreatedDate = reader.GetDateTime(11)
                    });
                }
                return messages;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recent messages");
                return messages;
            }
        }


        private async Task<List<VisitorTrackingModel>> GetRecentVisitorsAsync(int count)
        {
            var visitors = new List<VisitorTrackingModel>();
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var query = @"
            SELECT TOP (@Count) * 
            FROM VisitorTracking 
            WHERE IsActive = 1 
            ORDER BY LastVisit DESC";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Count", count);

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    visitors.Add(new VisitorTrackingModel
                    {
                        VisitorId = reader.GetInt32(0),
                        IPAddress = reader.IsDBNull(2) ? null : reader.GetString(2),
                        Country = reader.IsDBNull(4) ? null : reader.GetString(4),
                        City = reader.IsDBNull(5) ? null : reader.GetString(5),
                        VisitCount = reader.GetInt32(13),
                        LastVisit = reader.GetDateTime(12)
                    });
                }
                return visitors;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recent visitors");
                return visitors;
            }
        }

        // Helper method to get recent blog posts for dashboard
        private async Task<List<BlogPostModel>> GetRecentBlogPostsForDashboardAsync(int count)
        {
            var posts = new List<BlogPostModel>();
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var query = @"
            SELECT TOP (@Count) 
                PostId, Title, Slug, CreatedDate, ViewCount, LikeCount
            FROM BlogPosts 
            WHERE IsActive = 1 
            ORDER BY CreatedDate DESC";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Count", count);

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    posts.Add(new BlogPostModel
                    {
                        PostId = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Slug = reader.GetString(2),
                        CreatedDate = reader.GetDateTime(3),
                        ViewCount = reader.GetInt32(4),
                        LikeCount = reader.GetInt32(5)
                    });
                }
                return posts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recent blog posts");
                return posts;
            }
        }

        // Add Experience


        // Update Experience
  
        // Delete Experience
        public async Task DeleteExperienceAsync(int id)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var query = "UPDATE Experience SET IsActive = 0, UpdatedDate = GETDATE() WHERE ExperienceId = @ExperienceId";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ExperienceId", id);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting experience");
                throw;
            }
        }

        // Add Education
        public async Task<int> AddEducationAsync(EducationModel education)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var query = @"
            INSERT INTO Education (Degree, Institute, Year, Percentage, IsActive, CreatedDate, UpdatedDate)
            VALUES (@Degree, @Institute, @Year, @Percentage, 1, GETDATE(), GETDATE());
            SELECT SCOPE_IDENTITY();";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Degree", education.Degree);
                cmd.Parameters.AddWithValue("@Institute", education.Institute);
                cmd.Parameters.AddWithValue("@Year", education.Year);
                cmd.Parameters.AddWithValue("@Percentage", education.Percentage ?? (object)DBNull.Value);

                await conn.OpenAsync();
                return Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding education");
                throw;
            }
        }

        // Update Education
        public async Task UpdateEducationAsync(EducationModel education)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var query = @"
            UPDATE Education SET
                Degree = @Degree,
                Institute = @Institute,
                Year = @Year,
                Percentage = @Percentage,
                UpdatedDate = GETDATE()
            WHERE EducationId = @EducationId AND IsActive = 1";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@EducationId", education.EducationId);
                cmd.Parameters.AddWithValue("@Degree", education.Degree);
                cmd.Parameters.AddWithValue("@Institute", education.Institute);
                cmd.Parameters.AddWithValue("@Year", education.Year);
                cmd.Parameters.AddWithValue("@Percentage", education.Percentage ?? (object)DBNull.Value);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating education");
                throw;
            }
        }

        // Delete Education
        public async Task DeleteEducationAsync(int id)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var query = "UPDATE Education SET IsActive = 0, UpdatedDate = GETDATE() WHERE EducationId = @EducationId";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@EducationId", id);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting education");
                throw;
            }
        }

        // Blog image methods
        public async Task<int> AddBlogImageAsync(int postId, string imagePath, string? thumbnailPath, string? caption, string? altText, int displayOrder, bool isCover)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var query = @"
            INSERT INTO BlogImages (PostId, ImagePath, ThumbnailPath, Caption, AltText, DisplayOrder, IsCover, IsActive, CreatedDate)
            VALUES (@PostId, @ImagePath, @ThumbnailPath, @Caption, @AltText, @DisplayOrder, @IsCover, 1, GETDATE());
            SELECT SCOPE_IDENTITY();";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PostId", postId);
                cmd.Parameters.AddWithValue("@ImagePath", imagePath);
                cmd.Parameters.AddWithValue("@ThumbnailPath", thumbnailPath ?? "");
                cmd.Parameters.AddWithValue("@Caption", caption ?? "");
                cmd.Parameters.AddWithValue("@AltText", altText ?? "");
                cmd.Parameters.AddWithValue("@DisplayOrder", displayOrder);
                cmd.Parameters.AddWithValue("@IsCover", isCover);

                await conn.OpenAsync();
                return Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding blog image");
                throw;
            }
        }

        public async Task<int> AddBlogVideoAsync(int postId, string videoTitle, string videoUrl, string videoType, string? thumbnailPath, int displayOrder)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var query = @"
            INSERT INTO BlogVideos (PostId, VideoTitle, VideoUrl, VideoType, ThumbnailPath, DisplayOrder, IsActive, CreatedDate)
            VALUES (@PostId, @VideoTitle, @VideoUrl, @VideoType, @ThumbnailPath, @DisplayOrder, 1, GETDATE());
            SELECT SCOPE_IDENTITY();";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PostId", postId);
                cmd.Parameters.AddWithValue("@VideoTitle", videoTitle);
                cmd.Parameters.AddWithValue("@VideoUrl", videoUrl);
                cmd.Parameters.AddWithValue("@VideoType", videoType);
                cmd.Parameters.AddWithValue("@ThumbnailPath", thumbnailPath ?? "");
                cmd.Parameters.AddWithValue("@DisplayOrder", displayOrder);

                await conn.OpenAsync();
                return Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding blog video");
                throw;
            }
        }

        public async Task DeleteBlogImageAsync(int imageId)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var query = "UPDATE BlogImages SET IsActive = 0 WHERE ImageId = @ImageId";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ImageId", imageId);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting blog image");
                throw;
            }
        }

        public async Task<List<BlogPostModel>> GetAllBlogPostsAsync()
        {
            var posts = new List<BlogPostModel>();
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var query = @"
            SELECT p.*, c.CategoryName, c.Slug AS CategorySlug,
                   (SELECT COUNT(*) FROM BlogImages WHERE PostId = p.PostId AND IsActive = 1) AS ImageCount,
                   (SELECT COUNT(*) FROM BlogVideos WHERE PostId = p.PostId AND IsActive = 1) AS VideoCount,
                   (SELECT COUNT(*) FROM BlogComments WHERE PostId = p.PostId AND IsActive = 1 AND IsApproved = 1) AS ApprovedCommentCount
            FROM BlogPosts p
            LEFT JOIN BlogCategories c ON p.CategoryId = c.CategoryId
            WHERE p.IsActive = 1
            ORDER BY p.CreatedDate DESC";

                using var cmd = new SqlCommand(query, conn);
                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    posts.Add(MapBlogPost(reader));
                }
                return posts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all blog posts");
                return posts;
            }
        }

        // Add blog post
        public async Task<int> AddBlogPostAsync(BlogPostModel post)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var query = @"
            INSERT INTO BlogPosts (
                CategoryId, Title, Slug, Excerpt, Content, FeaturedImage, 
                VideoUrl, VideoEmbedCode, Tags, MetaTitle, MetaDescription, 
                MetaKeywords, ViewCount, LikeCount, CommentCount, 
                IsPublished, PublishedDate, IsFeatured, IsActive, CreatedDate, UpdatedDate
            ) VALUES (
                @CategoryId, @Title, @Slug, @Excerpt, @Content, @FeaturedImage,
                @VideoUrl, @VideoEmbedCode, @Tags, @MetaTitle, @MetaDescription,
                @MetaKeywords, 0, 0, 0, @IsPublished, @PublishedDate, @IsFeatured, 1, GETDATE(), GETDATE()
            );
            SELECT SCOPE_IDENTITY();";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CategoryId", post.CategoryId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Title", post.Title);
                cmd.Parameters.AddWithValue("@Slug", post.Slug);
                cmd.Parameters.AddWithValue("@Excerpt", post.Excerpt ?? "");
                cmd.Parameters.AddWithValue("@Content", post.Content);
                cmd.Parameters.AddWithValue("@FeaturedImage", post.FeaturedImage ?? "");
                cmd.Parameters.AddWithValue("@VideoUrl", post.VideoUrl ?? "");
                cmd.Parameters.AddWithValue("@VideoEmbedCode", post.VideoEmbedCode ?? "");
                cmd.Parameters.AddWithValue("@Tags", post.Tags ?? "");
                cmd.Parameters.AddWithValue("@MetaTitle", post.MetaTitle ?? "");
                cmd.Parameters.AddWithValue("@MetaDescription", post.MetaDescription ?? "");
                cmd.Parameters.AddWithValue("@MetaKeywords", post.MetaKeywords ?? "");
                cmd.Parameters.AddWithValue("@IsPublished", post.IsPublished);
                cmd.Parameters.AddWithValue("@PublishedDate", post.PublishedDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@IsFeatured", post.IsFeatured);

                await conn.OpenAsync();
                return Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding blog post");
                throw;
            }
        }

        // Update blog post
        public async Task UpdateBlogPostAsync(BlogPostModel post)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var query = @"
            UPDATE BlogPosts SET
                CategoryId = @CategoryId,
                Title = @Title,
                Slug = @Slug,
                Excerpt = @Excerpt,
                Content = @Content,
                FeaturedImage = @FeaturedImage,
                VideoUrl = @VideoUrl,
                VideoEmbedCode = @VideoEmbedCode,
                Tags = @Tags,
                MetaTitle = @MetaTitle,
                MetaDescription = @MetaDescription,
                MetaKeywords = @MetaKeywords,
                IsPublished = @IsPublished,
                PublishedDate = @PublishedDate,
                IsFeatured = @IsFeatured,
                UpdatedDate = GETDATE()
            WHERE PostId = @PostId AND IsActive = 1";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PostId", post.PostId);
                cmd.Parameters.AddWithValue("@CategoryId", post.CategoryId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Title", post.Title);
                cmd.Parameters.AddWithValue("@Slug", post.Slug);
                cmd.Parameters.AddWithValue("@Excerpt", post.Excerpt ?? "");
                cmd.Parameters.AddWithValue("@Content", post.Content);
                cmd.Parameters.AddWithValue("@FeaturedImage", post.FeaturedImage ?? "");
                cmd.Parameters.AddWithValue("@VideoUrl", post.VideoUrl ?? "");
                cmd.Parameters.AddWithValue("@VideoEmbedCode", post.VideoEmbedCode ?? "");
                cmd.Parameters.AddWithValue("@Tags", post.Tags ?? "");
                cmd.Parameters.AddWithValue("@MetaTitle", post.MetaTitle ?? "");
                cmd.Parameters.AddWithValue("@MetaDescription", post.MetaDescription ?? "");
                cmd.Parameters.AddWithValue("@MetaKeywords", post.MetaKeywords ?? "");
                cmd.Parameters.AddWithValue("@IsPublished", post.IsPublished);
                cmd.Parameters.AddWithValue("@PublishedDate", post.PublishedDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@IsFeatured", post.IsFeatured);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating blog post");
                throw;
            }
        }

        // Delete blog post
        public async Task DeleteBlogPostAsync(int id)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var query = "UPDATE BlogPosts SET IsActive = 0, UpdatedDate = GETDATE() WHERE PostId = @PostId";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PostId", id);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting blog post");
                throw;
            }
        }// Add Education
         // Add Blog Category
        public async Task<int> AddBlogCategoryAsync(BlogCategoryModel category)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var query = @"
            INSERT INTO BlogCategories (CategoryName, Slug, Description, Icon, DisplayOrder, IsActive, CreatedDate, UpdatedDate)
            VALUES (@CategoryName, @Slug, @Description, @Icon, @DisplayOrder, 1, GETDATE(), GETDATE());
            SELECT SCOPE_IDENTITY();";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                cmd.Parameters.AddWithValue("@Slug", category.Slug);
                cmd.Parameters.AddWithValue("@Description", category.Description ?? "");
                cmd.Parameters.AddWithValue("@Icon", category.Icon ?? "");
                cmd.Parameters.AddWithValue("@DisplayOrder", category.DisplayOrder);

                await conn.OpenAsync();
                return Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding blog category");
                throw;
            }
        }

        // Update Blog Category
        public async Task UpdateBlogCategoryAsync(BlogCategoryModel category)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var query = @"
            UPDATE BlogCategories SET
                CategoryName = @CategoryName,
                Slug = @Slug,
                Description = @Description,
                Icon = @Icon,
                DisplayOrder = @DisplayOrder,
                UpdatedDate = GETDATE()
            WHERE CategoryId = @CategoryId AND IsActive = 1";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CategoryId", category.CategoryId);
                cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                cmd.Parameters.AddWithValue("@Slug", category.Slug);
                cmd.Parameters.AddWithValue("@Description", category.Description ?? "");
                cmd.Parameters.AddWithValue("@Icon", category.Icon ?? "");
                cmd.Parameters.AddWithValue("@DisplayOrder", category.DisplayOrder);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating blog category");
                throw;
            }
        }

        // Delete Blog Category
        public async Task DeleteBlogCategoryAsync(int id)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var query = "UPDATE BlogCategories SET IsActive = 0, UpdatedDate = GETDATE() WHERE CategoryId = @CategoryId";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CategoryId", id);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting blog category");
                throw;
            }
        }

        // Approve comment
        public async Task ApproveCommentAsync(int commentId)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var query = "UPDATE BlogComments SET IsApproved = 1 WHERE CommentId = @CommentId";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CommentId", commentId);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving comment");
                throw;
            }
        }

        // Delete comment
        public async Task DeleteCommentAsync(int commentId)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var query = "UPDATE BlogComments SET IsActive = 0 WHERE CommentId = @CommentId";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CommentId", commentId);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting comment");
                throw;
            }
        }// Add Gallery Item
        public async Task<int> AddGalleryItemAsync(GalleryModel item)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var query = @"
            INSERT INTO Gallery (Title, Description, MediaType, MediaPath, ThumbnailPath, VideoEmbedCode, Category, Tags, DisplayOrder, IsFeatured, ViewCount, DownloadCount, IsActive, CreatedDate, UpdatedDate)
            VALUES (@Title, @Description, @MediaType, @MediaPath, @ThumbnailPath, @VideoEmbedCode, @Category, @Tags, @DisplayOrder, @IsFeatured, @ViewCount, @DownloadCount, @IsActive, @CreatedDate, @UpdatedDate);
            SELECT SCOPE_IDENTITY();";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Title", item.Title ?? "");
                cmd.Parameters.AddWithValue("@Description", item.Description ?? "");
                cmd.Parameters.AddWithValue("@MediaType", item.MediaType ?? "");
                cmd.Parameters.AddWithValue("@MediaPath", item.MediaPath ?? "");
                cmd.Parameters.AddWithValue("@ThumbnailPath", item.ThumbnailPath ?? "");
                cmd.Parameters.AddWithValue("@VideoEmbedCode", item.VideoEmbedCode ?? "");
                cmd.Parameters.AddWithValue("@Category", item.Category ?? "");
                cmd.Parameters.AddWithValue("@Tags", item.Tags ?? "");
                cmd.Parameters.AddWithValue("@DisplayOrder", item.DisplayOrder);
                cmd.Parameters.AddWithValue("@IsFeatured", item.IsFeatured);
                cmd.Parameters.AddWithValue("@ViewCount", item.ViewCount);
                cmd.Parameters.AddWithValue("@DownloadCount", item.DownloadCount);
                cmd.Parameters.AddWithValue("@IsActive", item.IsActive);
                cmd.Parameters.AddWithValue("@CreatedDate", item.CreatedDate);
                cmd.Parameters.AddWithValue("@UpdatedDate", item.UpdatedDate);

                await conn.OpenAsync();
                return Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding gallery item");
                throw;
            }
        }

        // Update Gallery Item
        public async Task UpdateGalleryItemAsync(GalleryModel item)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var query = @"
            UPDATE Gallery SET
                Title = @Title,
                Description = @Description,
                MediaType = @MediaType,
                MediaPath = @MediaPath,
                ThumbnailPath = @ThumbnailPath,
                VideoEmbedCode = @VideoEmbedCode,
                Category = @Category,
                Tags = @Tags,
                DisplayOrder = @DisplayOrder,
                IsFeatured = @IsFeatured,
                UpdatedDate = @UpdatedDate
            WHERE GalleryId = @GalleryId";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@GalleryId", item.GalleryId);
                cmd.Parameters.AddWithValue("@Title", item.Title ?? "");
                cmd.Parameters.AddWithValue("@Description", item.Description ?? "");
                cmd.Parameters.AddWithValue("@MediaType", item.MediaType ?? "");
                cmd.Parameters.AddWithValue("@MediaPath", item.MediaPath ?? "");
                cmd.Parameters.AddWithValue("@ThumbnailPath", item.ThumbnailPath ?? "");
                cmd.Parameters.AddWithValue("@VideoEmbedCode", item.VideoEmbedCode ?? "");
                cmd.Parameters.AddWithValue("@Category", item.Category ?? "");
                cmd.Parameters.AddWithValue("@Tags", item.Tags ?? "");
                cmd.Parameters.AddWithValue("@DisplayOrder", item.DisplayOrder);
                cmd.Parameters.AddWithValue("@IsFeatured", item.IsFeatured);
                cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating gallery item");
                throw;
            }
        }

        // Delete Gallery Item
        public async Task DeleteGalleryItemAsync(int id)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var query = "UPDATE Gallery SET IsActive = 0, UpdatedDate = GETDATE() WHERE GalleryId = @GalleryId";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@GalleryId", id);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting gallery item");
                throw;
            }
        }// Toggle blog like


        public async Task<VisitorTrackingModel?> GetVisitorByIdAsync(int visitorId)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var query = "SELECT * FROM VisitorTracking WHERE VisitorId = @VisitorId AND IsActive = 1";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@VisitorId", visitorId);

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new VisitorTrackingModel
                    {
                        VisitorId = reader.GetInt32(0),
                        SessionId = reader.IsDBNull(1) ? null : reader.GetString(1),
                        IPAddress = reader.IsDBNull(2) ? null : reader.GetString(2),
                        UserAgent = reader.IsDBNull(3) ? null : reader.GetString(3),
                        Country = reader.IsDBNull(4) ? null : reader.GetString(4),
                        City = reader.IsDBNull(5) ? null : reader.GetString(5),
                        FirstVisit = reader.GetDateTime(12),
                        LastVisit = reader.GetDateTime(13),
                        VisitCount = reader.GetInt32(14),
                        IsActive = reader.GetBoolean(17),
                        CreatedDate = reader.GetDateTime(18)
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting visitor by ID");
                return null;
            }
        }

        // Create new Visitor
        public async Task<int> CreateVisitorAsync(VisitorTrackingModel visitor)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var query = @"
            INSERT INTO VisitorTracking (SessionId, IPAddress, UserAgent, FirstVisit, LastVisit, VisitCount, IsActive, CreatedDate, UpdatedDate)
            VALUES (@SessionId, @IPAddress, @UserAgent, @FirstVisit, @LastVisit, @VisitCount, 1, GETDATE(), GETDATE());
            SELECT SCOPE_IDENTITY();";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SessionId", visitor.SessionId ?? "");
                cmd.Parameters.AddWithValue("@IPAddress", visitor.IPAddress ?? "");
                cmd.Parameters.AddWithValue("@UserAgent", visitor.UserAgent ?? "");
                cmd.Parameters.AddWithValue("@FirstVisit", visitor.FirstVisit);
                cmd.Parameters.AddWithValue("@LastVisit", visitor.LastVisit);
                cmd.Parameters.AddWithValue("@VisitCount", visitor.VisitCount);

                await conn.OpenAsync();
                return Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating visitor");
                throw;
            }
        }

        // ========== LIKE SYSTEM - FIXED VERSION ==========

        public async Task<string> ToggleBlogLikeAsync(int postId, int visitorId, string? ipAddress)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();

                // पहले verify करो कि visitor exists करता है
                var checkVisitorSql = "SELECT COUNT(*) FROM VisitorTracking WHERE VisitorId = @VisitorId AND IsActive = 1";
                using (var checkVisitorCmd = new SqlCommand(checkVisitorSql, conn))
                {
                    checkVisitorCmd.Parameters.AddWithValue("@VisitorId", visitorId);
                    var visitorExists = (int)await checkVisitorCmd.ExecuteScalarAsync() > 0;

                    if (!visitorExists)
                    {
                        // Visitor नहीं है तो नया बनाओ
                        visitorId = await CreateDefaultVisitorAsync(conn, ipAddress);
                    }
                }

                // Check if already liked
                var checkSql = "SELECT COUNT(*) FROM BlogLikes WHERE PostId = @PostId AND VisitorId = @VisitorId";
                using (var checkCmd = new SqlCommand(checkSql, conn))
                {
                    checkCmd.Parameters.AddWithValue("@PostId", postId);
                    checkCmd.Parameters.AddWithValue("@VisitorId", visitorId);

                    var exists = (int)await checkCmd.ExecuteScalarAsync() > 0;

                    if (exists)
                    {
                        // Unlike
                        var deleteSql = "DELETE FROM BlogLikes WHERE PostId = @PostId AND VisitorId = @VisitorId";
                        using (var deleteCmd = new SqlCommand(deleteSql, conn))
                        {
                            deleteCmd.Parameters.AddWithValue("@PostId", postId);
                            deleteCmd.Parameters.AddWithValue("@VisitorId", visitorId);
                            await deleteCmd.ExecuteNonQueryAsync();
                        }

                        var updateSql = "UPDATE BlogPosts SET LikeCount = LikeCount - 1 WHERE PostId = @PostId";
                        using (var updateCmd = new SqlCommand(updateSql, conn))
                        {
                            updateCmd.Parameters.AddWithValue("@PostId", postId);
                            await updateCmd.ExecuteNonQueryAsync();
                        }

                        return "unliked";
                    }
                    else
                    {
                        // Like
                        var insertSql = "INSERT INTO BlogLikes (PostId, VisitorId, IPAddress, CreatedDate) VALUES (@PostId, @VisitorId, @IPAddress, GETDATE())";
                        using (var insertCmd = new SqlCommand(insertSql, conn))
                        {
                            insertCmd.Parameters.AddWithValue("@PostId", postId);
                            insertCmd.Parameters.AddWithValue("@VisitorId", visitorId);
                            insertCmd.Parameters.AddWithValue("@IPAddress", ipAddress ?? "");
                            await insertCmd.ExecuteNonQueryAsync();
                        }

                        var updateSql = "UPDATE BlogPosts SET LikeCount = LikeCount + 1 WHERE PostId = @PostId";
                        using (var updateCmd = new SqlCommand(updateSql, conn))
                        {
                            updateCmd.Parameters.AddWithValue("@PostId", postId);
                            await updateCmd.ExecuteNonQueryAsync();
                        }

                        return "liked";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling blog like");
                throw;
            }
        }

        private async Task<int> CreateDefaultVisitorAsync(SqlConnection conn, string? ipAddress)
        {
            var insertVisitorSql = @"
        INSERT INTO VisitorTracking (SessionId, IPAddress, UserAgent, FirstVisit, LastVisit, VisitCount, IsActive, CreatedDate, UpdatedDate)
        VALUES (@SessionId, @IPAddress, @UserAgent, GETDATE(), GETDATE(), 1, 1, GETDATE(), GETDATE());
        SELECT SCOPE_IDENTITY();";

            using var cmd = new SqlCommand(insertVisitorSql, conn);
            cmd.Parameters.AddWithValue("@SessionId", Guid.NewGuid().ToString());
            cmd.Parameters.AddWithValue("@IPAddress", ipAddress ?? "");
            cmd.Parameters.AddWithValue("@UserAgent", "Unknown");

            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }

        // Check if user liked post
        public async Task<bool> HasUserLikedPostAsync(int postId, int visitorId)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var sql = "SELECT COUNT(*) FROM BlogLikes WHERE PostId = @PostId AND VisitorId = @VisitorId";

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@PostId", postId);
                cmd.Parameters.AddWithValue("@VisitorId", visitorId);

                await conn.OpenAsync();
                return (int)await cmd.ExecuteScalarAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if user liked post");
                return false;
            }
        }


        public async Task<BlogPostModel?> GetBlogPostBySlugAsync(string slug)
        {
            try
            {
                Console.WriteLine($"========== DB HELPER CALLED ==========");
                Console.WriteLine($"Searching for slug: '{slug}'");

                using var conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();
                Console.WriteLine("Database connection opened");

                var sql = @"
            SELECT * FROM BlogPosts 
            WHERE Slug = @Slug AND IsActive = 1";

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Slug", slug);

                using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    Console.WriteLine("✅ Post found in database");

                    var post = new BlogPostModel
                    {
                        PostId = reader.GetInt32(0),
                        CategoryId = reader.IsDBNull(1) ? null : (int?)reader.GetInt32(1),
                        Title = reader.GetString(2),
                        Slug = reader.GetString(3),
                        Excerpt = reader.IsDBNull(4) ? null : reader.GetString(4),
                        Content = reader.GetString(5),
                        FeaturedImage = reader.IsDBNull(6) ? null : reader.GetString(6),
                        VideoUrl = reader.IsDBNull(7) ? null : reader.GetString(7),
                        VideoEmbedCode = reader.IsDBNull(8) ? null : reader.GetString(8),
                        Tags = reader.IsDBNull(9) ? null : reader.GetString(9),
                        MetaTitle = reader.IsDBNull(10) ? null : reader.GetString(10),
                        MetaDescription = reader.IsDBNull(11) ? null : reader.GetString(11),
                        MetaKeywords = reader.IsDBNull(12) ? null : reader.GetString(12),
                        ViewCount = reader.GetInt32(13),
                        LikeCount = reader.GetInt32(14),
                        CommentCount = reader.GetInt32(15),
                        IsPublished = reader.GetBoolean(16),
                        PublishedDate = reader.IsDBNull(17) ? null : (DateTime?)reader.GetDateTime(17),
                        IsFeatured = reader.GetBoolean(18),
                        IsActive = reader.GetBoolean(19),
                        CreatedDate = reader.GetDateTime(20),
                        UpdatedDate = reader.GetDateTime(21)
                    };


                    await UpdateViewCountAsync(post.PostId);

                    Console.WriteLine($"✅ Post mapped: {post.Title}");
                    return post;
                }

                Console.WriteLine($"❌ No post found with slug: '{slug}'");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ DB ERROR: {ex.Message}");
                _logger.LogError(ex, "Error getting blog post by slug: {Slug}", slug);
                return null;
            }
        }

        // Get all comments
        public async Task<List<BlogCommentModel>> GetAllCommentsAsync()
        {
            var comments = new List<BlogCommentModel>();
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var query = @"
            SELECT c.*, p.Title as PostTitle 
            FROM BlogComments c
            LEFT JOIN BlogPosts p ON c.PostId = p.PostId
            WHERE c.IsActive = 1
            ORDER BY c.CreatedDate DESC";

                using var cmd = new SqlCommand(query, conn);
                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    comments.Add(new BlogCommentModel
                    {
                        CommentId = reader.GetInt32(0),
                        PostId = reader.GetInt32(1),
                        ParentCommentId = reader.IsDBNull(2) ? null : (int?)reader.GetInt32(2),
                        VisitorId = reader.IsDBNull(3) ? null : (int?)reader.GetInt32(3),
                        Name = reader.GetString(4),
                        Email = reader.IsDBNull(5) ? null : reader.GetString(5),
                        Website = reader.IsDBNull(6) ? null : reader.GetString(6),
                        Comment = reader.GetString(7),
                        IsApproved = reader.GetBoolean(8),
                        IsSpam = reader.GetBoolean(9),
                        IPAddress = reader.IsDBNull(10) ? null : reader.GetString(10),
                        LikeCount = reader.GetInt32(11),
                        IsActive = reader.GetBoolean(12),
                        CreatedDate = reader.GetDateTime(13),
                        PostTitle = reader.FieldCount > 14 ? reader.GetString(14) : $"Post #{reader.GetInt32(1)}"
                    });
                }
                return comments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all comments");
                return comments;
            }
        }
        private async Task UpdateViewCountAsync(int postId)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var sql = "UPDATE BlogPosts SET ViewCount = ViewCount + 1 WHERE PostId = @PostId";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@PostId", postId);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                Console.WriteLine($"✅ View count updated for post {postId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error updating view count: {ex.Message}");
            }
        }

        public async Task<int> TrackResumeViewAsync(ResumeViewModel model)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var query = @"
            INSERT INTO ResumeViews (
                VisitorName, VisitorEmail, CompanyName, Designation, 
                ViewDate, IPAddress, UserAgent, 
                Country, City, 
                IsDownloaded, DownloadDate, IsActive, CreatedDate
            )
            VALUES (
                @VisitorName, @VisitorEmail, @CompanyName, @Designation, 
                @ViewDate, @IPAddress, @UserAgent, 
                @Country, @City, 
                @IsDownloaded, @DownloadDate, @IsActive, @CreatedDate
            );
            SELECT SCOPE_IDENTITY();";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@VisitorName", model.VisitorName ?? "");
                cmd.Parameters.AddWithValue("@VisitorEmail", model.VisitorEmail ?? "");
                cmd.Parameters.AddWithValue("@CompanyName", model.CompanyName ?? "");
                cmd.Parameters.AddWithValue("@Designation", model.Designation ?? "");
                cmd.Parameters.AddWithValue("@ViewDate", model.ViewDate);
                cmd.Parameters.AddWithValue("@IPAddress", model.IPAddress ?? "");
                cmd.Parameters.AddWithValue("@UserAgent", model.UserAgent ?? "");
                cmd.Parameters.AddWithValue("@Country", model.Country ?? "");
                cmd.Parameters.AddWithValue("@City", model.City ?? "");
                cmd.Parameters.AddWithValue("@IsDownloaded", model.IsDownloaded);
                cmd.Parameters.AddWithValue("@DownloadDate", model.DownloadDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", model.IsActive);
                cmd.Parameters.AddWithValue("@CreatedDate", model.CreatedDate);

                await conn.OpenAsync();
                return Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error tracking resume view");
                throw;
            }
        }

        //public async Task<List<ExperienceModel>> GetAllExperienceAsync()
        //{
        //    var experiences = new List<ExperienceModel>();
        //    try
        //    {
        //        using var conn = new SqlConnection(_connectionString);
        //        const string query = "SELECT * FROM Experience WHERE IsActive = 1 ORDER BY StartDate DESC";

        //        using var cmd = new SqlCommand(query, conn);
        //        await conn.OpenAsync();
        //        using var reader = await cmd.ExecuteReaderAsync();

        //        while (await reader.ReadAsync())
        //        {
        //            experiences.Add(new ExperienceModel
        //            {
        //                ExperienceId = reader.GetInt32(0),
        //                CompanyName = reader.GetString(1),
        //                Role = reader.GetString(2),
        //                StartDate = reader.GetDateTime(3),
        //                EndDate = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
        //                Description = reader.IsDBNull(5) ? "" : reader.GetString(5),
        //                IsActive = reader.GetBoolean(6),
        //                CreatedDate = reader.GetDateTime(7),
        //                UpdatedDate = reader.GetDateTime(8)
        //            });
        //        }
        //        return experiences;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error getting experience");
        //        return experiences;
        //    }
        //}

        // Add Experience
        public async Task<int> AddExperienceAsync(ExperienceModel experience)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var query = @"
            INSERT INTO Experience (CompanyName, Role, StartDate, EndDate, Description, IsActive, CreatedDate, UpdatedDate)
            VALUES (@CompanyName, @Role, @StartDate, @EndDate, @Description, 1, GETDATE(), GETDATE());
            SELECT SCOPE_IDENTITY();";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CompanyName", experience.CompanyName);
                cmd.Parameters.AddWithValue("@Role", experience.Role);
                cmd.Parameters.AddWithValue("@StartDate", experience.StartDate);
                cmd.Parameters.AddWithValue("@EndDate", experience.EndDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Description", experience.Description ?? "");

                await conn.OpenAsync();
                return Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding experience");
                throw;
            }
        }

        // Update Experience
        public async Task UpdateExperienceAsync(ExperienceModel experience)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var query = @"
            UPDATE Experience SET
                CompanyName = @CompanyName,
                Role = @Role,
                StartDate = @StartDate,
                EndDate = @EndDate,
                Description = @Description,
                UpdatedDate = GETDATE()
            WHERE ExperienceId = @ExperienceId AND IsActive = 1";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ExperienceId", experience.ExperienceId);
                cmd.Parameters.AddWithValue("@CompanyName", experience.CompanyName);
                cmd.Parameters.AddWithValue("@Role", experience.Role);
                cmd.Parameters.AddWithValue("@StartDate", experience.StartDate);
                cmd.Parameters.AddWithValue("@EndDate", experience.EndDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Description", experience.Description ?? "");

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating experience");
                throw;
            }
        }
        //public async Task UpdateExperienceAsync(ExperienceModel experience)
        //{
        //    try
        //    {
        //        using var conn = new SqlConnection(_connectionString);
        //        var query = @"
        //    UPDATE Experience SET
        //        CompanyName = @CompanyName,
        //        Role = @Role,
        //        StartDate = @StartDate,
        //        EndDate = @EndDate,
        //        Description = @Description,
        //        UpdatedDate = GETDATE()
        //    WHERE ExperienceId = @ExperienceId AND IsActive = 1";

        //        using var cmd = new SqlCommand(query, conn);
        //        cmd.Parameters.AddWithValue("@ExperienceId", experience.ExperienceId);
        //        cmd.Parameters.AddWithValue("@CompanyName", experience.CompanyName);
        //        cmd.Parameters.AddWithValue("@Role", experience.Role);
        //        cmd.Parameters.AddWithValue("@StartDate", experience.StartDate);
        //        cmd.Parameters.AddWithValue("@EndDate", experience.EndDate ?? (object)DBNull.Value);
        //        cmd.Parameters.AddWithValue("@Description", experience.Description ?? "");

        //        await conn.OpenAsync();
        //        await cmd.ExecuteNonQueryAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error updating experience");
        //        throw;
        //    }
        //}

        // Get All Experience


        //public async Task<int> AddExperienceAsync(ExperienceModel experience)
        //{
        //    try
        //    {
        //        using var conn = new SqlConnection(_connectionString);
        //        var query = @"
        //    INSERT INTO Experience (CompanyName, Role, StartDate, EndDate, Description, IsActive, CreatedDate, UpdatedDate)
        //    VALUES (@CompanyName, @Role, @StartDate, @EndDate, @Description, 1, GETDATE(), GETDATE());
        //    SELECT SCOPE_IDENTITY();";

        //        using var cmd = new SqlCommand(query, conn);
        //        cmd.Parameters.AddWithValue("@CompanyName", experience.CompanyName);
        //        cmd.Parameters.AddWithValue("@Role", experience.Role);
        //        cmd.Parameters.AddWithValue("@StartDate", experience.StartDate);
        //        cmd.Parameters.AddWithValue("@EndDate", experience.EndDate ?? (object)DBNull.Value);
        //        cmd.Parameters.AddWithValue("@Description", experience.Description ?? "");

        //        await conn.OpenAsync();
        //        return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error adding experience");
        //        throw;
        //    }
        //}
        public async Task<List<ExperienceModel>> GetAllExperienceAsync()
        {
            var experiences = new List<ExperienceModel>();
            try
            {
                using var conn = new SqlConnection(_connectionString);
                var query = "SELECT * FROM Experience WHERE IsActive = 1 ORDER BY StartDate DESC";

                using var cmd = new SqlCommand(query, conn);
                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    experiences.Add(new ExperienceModel
                    {
                        ExperienceId = reader.GetInt32(0),
                        CompanyName = reader.GetString(1),
                        Role = reader.GetString(2),
                        StartDate = reader.GetDateTime(3),
                        EndDate = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                        Description = reader.IsDBNull(5) ? "" : reader.GetString(5),
                        IsActive = reader.GetBoolean(6),
                        CreatedDate = reader.GetDateTime(7),
                        UpdatedDate = reader.GetDateTime(8)
                    });
                }
                return experiences;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting experience");
                return experiences;
            }
        }
    }
}