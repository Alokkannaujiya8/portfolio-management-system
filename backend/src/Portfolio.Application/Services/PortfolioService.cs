using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Common.Interfaces;
using Portfolio.Application.Common.Models;
using Portfolio.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Application.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IPortfolioDbContext _context;

        public PortfolioService(IPortfolioDbContext context)
        {
            _context = context;
        }

        public async Task<HomeViewModel> GetHomeDataAsync()
        {
            var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.IsActive);
            var skills = await _context.Skills.Where(s => s.IsActive).ToListAsync();
            var projects = await _context.Projects.Where(p => p.IsActive).ToListAsync();
            var experience = await _context.Experiences.Where(e => e.IsActive).OrderByDescending(e => e.StartDate).ToListAsync();
            var education = await _context.Educations.Where(e => e.IsActive).OrderByDescending(e => e.Year).ToListAsync();
            
            // Get 3 recent published blog posts
            var blogPosts = await _context.BlogPosts
                .Include(b => b.Category)
                .Where(b => b.IsActive && b.IsPublished == true)
                .OrderByDescending(b => b.PublishedDate)
                .Take(3)
                .ToListAsync();

            return new HomeViewModel
            {
                Profile = profile,
                Skills = skills,
                Projects = projects,
                Experience = experience,
                Education = education,
                BlogPosts = blogPosts
            };
        }

        public async Task<Profile?> GetProfileAsync()
        {
            return await _context.Profiles.FirstOrDefaultAsync(p => p.IsActive);
        }

        public async Task<int> UpdateProfileAsync(Profile profile)
        {
            var existing = await _context.Profiles.FirstOrDefaultAsync(p => p.ProfileId == profile.ProfileId);
            if (existing == null)
            {
                profile.CreatedDate = DateTime.UtcNow;
                profile.UpdatedDate = DateTime.UtcNow;
                _context.Profiles.Add(profile);
            }
            else
            {
                existing.Name = profile.Name;
                existing.Title = profile.Title;
                existing.Description = profile.Description;
                existing.Email = profile.Email;
                existing.Phone = profile.Phone;
                existing.Address = profile.Address;
                existing.LinkedIn = profile.LinkedIn;
                existing.GitHub = profile.GitHub;
                existing.IsActive = profile.IsActive;
                existing.UpdatedDate = DateTime.UtcNow;
                
                if (!string.IsNullOrEmpty(profile.Photo))
                {
                    existing.Photo = profile.Photo;
                }
                
                if (!string.IsNullOrEmpty(profile.ResumePath))
                {
                    existing.ResumePath = profile.ResumePath;
                }
            }

            await _context.SaveChangesAsync();
            return profile.ProfileId;
        }

        public async Task<List<Skill>> GetAllSkillsAsync()
        {
            return await _context.Skills.Where(s => s.IsActive).ToListAsync();
        }

        public async Task<int> AddSkillAsync(Skill skill)
        {
            skill.CreatedDate = DateTime.UtcNow;
            skill.UpdatedDate = DateTime.UtcNow;
            _context.Skills.Add(skill);
            await _context.SaveChangesAsync();
            return skill.SkillId;
        }

        public async Task UpdateSkillAsync(Skill skill)
        {
            var existing = await _context.Skills.FindAsync(skill.SkillId);
            if (existing != null)
            {
                existing.SkillName = skill.SkillName;
                existing.Percentage = skill.Percentage;
                existing.IsActive = skill.IsActive;
                existing.UpdatedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteSkillAsync(int id)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill != null)
            {
                skill.IsActive = false;
                skill.UpdatedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Project>> GetAllProjectsAsync()
        {
            return await _context.Projects.Where(p => p.IsActive).ToListAsync();
        }

        public async Task<int> AddProjectAsync(Project project)
        {
            project.CreatedDate = DateTime.UtcNow;
            project.UpdatedDate = DateTime.UtcNow;
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return project.ProjectId;
        }

        public async Task UpdateProjectAsync(Project project)
        {
            var existing = await _context.Projects.FindAsync(project.ProjectId);
            if (existing != null)
            {
                existing.ProjectName = project.ProjectName;
                existing.Description = project.Description;
                existing.GitHubLink = project.GitHubLink;
                existing.LiveLink = project.LiveLink;
                existing.IsActive = project.IsActive;
                existing.UpdatedDate = DateTime.UtcNow;

                if (!string.IsNullOrEmpty(project.ImagePath))
                {
                    existing.ImagePath = project.ImagePath;
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteProjectAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                project.IsActive = false;
                project.UpdatedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Experience>> GetAllExperiencesAsync()
        {
            return await _context.Experiences.Where(e => e.IsActive).OrderByDescending(e => e.StartDate).ToListAsync();
        }

        public async Task<int> AddExperienceAsync(Experience experience)
        {
            experience.CreatedDate = DateTime.UtcNow;
            experience.UpdatedDate = DateTime.UtcNow;
            _context.Experiences.Add(experience);
            await _context.SaveChangesAsync();
            return experience.ExperienceId;
        }

        public async Task UpdateExperienceAsync(Experience experience)
        {
            var existing = await _context.Experiences.FindAsync(experience.ExperienceId);
            if (existing != null)
            {
                existing.CompanyName = experience.CompanyName;
                existing.Role = experience.Role;
                existing.StartDate = experience.StartDate;
                existing.EndDate = experience.EndDate;
                existing.Description = experience.Description;
                existing.IsActive = experience.IsActive;
                existing.UpdatedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteExperienceAsync(int id)
        {
            var experience = await _context.Experiences.FindAsync(id);
            if (experience != null)
            {
                experience.IsActive = false;
                experience.UpdatedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Education>> GetAllEducationsAsync()
        {
            return await _context.Educations.Where(e => e.IsActive).OrderByDescending(e => e.Year).ToListAsync();
        }

        public async Task<int> AddEducationAsync(Education education)
        {
            education.CreatedDate = DateTime.UtcNow;
            education.UpdatedDate = DateTime.UtcNow;
            _context.Educations.Add(education);
            await _context.SaveChangesAsync();
            return education.EducationId;
        }

        public async Task UpdateEducationAsync(Education education)
        {
            var existing = await _context.Educations.FindAsync(education.EducationId);
            if (existing != null)
            {
                existing.Degree = education.Degree;
                existing.Institute = education.Institute;
                existing.Year = education.Year;
                existing.Percentage = education.Percentage;
                existing.IsActive = education.IsActive;
                existing.UpdatedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteEducationAsync(int id)
        {
            var education = await _context.Educations.FindAsync(id);
            if (education != null)
            {
                education.IsActive = false;
                education.UpdatedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> SaveContactMessageAsync(ContactMessage message)
        {
            message.CreatedDate = DateTime.UtcNow;
            message.UpdatedDate = DateTime.UtcNow;
            message.IsActive = true;
            message.IsRead = false;
            message.IsReplied = false;
            
            _context.ContactMessages.Add(message);
            await _context.SaveChangesAsync();
            return message.MessageId;
        }

        public async Task<List<ContactMessage>> GetAllMessagesAsync()
        {
            return await _context.ContactMessages.Where(m => m.IsActive == true).OrderByDescending(m => m.CreatedDate).ToListAsync();
        }

        public async Task MarkMessageAsReadAsync(int messageId)
        {
            var message = await _context.ContactMessages.FindAsync(messageId);
            if (message != null)
            {
                message.IsRead = true;
                message.UpdatedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
