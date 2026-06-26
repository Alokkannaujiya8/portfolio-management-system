using Portfolio.Domain.Entities;
using Portfolio.Application.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Portfolio.Application.Common.Interfaces
{
    public interface IPortfolioService
    {
        Task<HomeViewModel> GetHomeDataAsync();
        
        Task<Profile?> GetProfileAsync();
        Task<int> UpdateProfileAsync(Profile profile);
        
        Task<List<Skill>> GetAllSkillsAsync();
        Task<int> AddSkillAsync(Skill skill);
        Task UpdateSkillAsync(Skill skill);
        Task DeleteSkillAsync(int id);
        
        Task<List<Project>> GetAllProjectsAsync();
        Task<int> AddProjectAsync(Project project);
        Task UpdateProjectAsync(Project project);
        Task DeleteProjectAsync(int id);
        
        Task<List<Experience>> GetAllExperiencesAsync();
        Task<int> AddExperienceAsync(Experience experience);
        Task UpdateExperienceAsync(Experience experience);
        Task DeleteExperienceAsync(int id);
        
        Task<List<Education>> GetAllEducationsAsync();
        Task<int> AddEducationAsync(Education education);
        Task UpdateEducationAsync(Education education);
        Task DeleteEducationAsync(int id);

        Task<int> SaveContactMessageAsync(ContactMessage message);
        Task<List<ContactMessage>> GetAllMessagesAsync();
        Task MarkMessageAsReadAsync(int messageId);
    }
}
