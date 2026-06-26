using System;
using System.Collections.Generic;
using System.Linq;

namespace Portfolio.Domain.Entities
{
    public class Experience : BaseModel
    {
        public int ExperienceId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; } = string.Empty;

        // Display duration for timeline (e.g., "Jan 2023 - Present")
        public string SimpleDuration => EndDate.HasValue
            ? $"{StartDate:MMM yyyy} - {EndDate:MMM yyyy}"
            : $"{StartDate:MMM yyyy} - Present";

        // Calculate duration for a single experience (e.g., "2 years 3 months")
        public string Duration
        {
            get
            {
                var end = EndDate ?? DateTime.Now;
                var start = StartDate;

                int totalMonths = ((end.Year - start.Year) * 12) + end.Month - start.Month;

                if (end.Day < start.Day)
                {
                    totalMonths--;
                }

                if (totalMonths <= 0) return "Less than a month";

                int years = totalMonths / 12;
                int months = totalMonths % 12;

                if (years > 0 && months > 0)
                    return $"{years} year{(years > 1 ? "s" : "")} {months} month{(months > 1 ? "s" : "")}";
                else if (years > 0)
                    return $"{years} year{(years > 1 ? "s" : "")}";
                else
                    return $"{months} month{(months > 1 ? "s" : "")}";
            }
        }

        // Calculate total months for a single experience
        public int TotalMonths
        {
            get
            {
                var end = EndDate ?? DateTime.Now;
                var start = StartDate;

                int months = ((end.Year - start.Year) * 12) + end.Month - start.Month;

                if (end.Day < start.Day)
                {
                    months--;
                }

                return Math.Max(0, months);
            }
        }

        // ==================== STATIC METHODS FOR TOTAL EXPERIENCE ====================

        public static int GetTotalMonthsFromAll(List<Experience> experiences)
        {
            if (experiences == null || !experiences.Any())
                return 0;

            int totalMonths = 0;
            foreach (var exp in experiences)
            {
                totalMonths += exp.TotalMonths;
            }
            return totalMonths;
        }

        public static int GetTotalMonthsMerged(List<Experience> experiences)
        {
            if (experiences == null || !experiences.Any())
                return 0;

            var sortedExperiences = experiences
                .Where(e => e.IsActive)
                .OrderBy(e => e.StartDate)
                .ToList();

            var mergedRanges = new List<(DateTime Start, DateTime End)>();

            foreach (var exp in sortedExperiences)
            {
                DateTime start = exp.StartDate;
                DateTime end = exp.EndDate ?? DateTime.Now;

                if (!mergedRanges.Any())
                {
                    mergedRanges.Add((start, end));
                }
                else
                {
                    var last = mergedRanges.Last();
                    if (start <= last.End)
                    {
                        // Overlapping, merge
                        mergedRanges[mergedRanges.Count - 1] = (last.Start, last.End > end ? last.End : end);
                    }
                    else
                    {
                        mergedRanges.Add((start, end));
                    }
                }
            }

            int totalMonths = 0;
            foreach (var range in mergedRanges)
            {
                totalMonths += (int)((range.End - range.Start).TotalDays / 30.44);
            }

            return totalMonths;
        }

        public static double GetTotalYearsFromAll(List<Experience> experiences)
        {
            return Math.Round(GetTotalMonthsFromAll(experiences) / 12.0, 1);
        }

        public static double GetTotalYearsMerged(List<Experience> experiences)
        {
            return Math.Round(GetTotalMonthsMerged(experiences) / 12.0, 1);
        }

        public static string GetTotalExperienceString(List<Experience> experiences)
        {
            int totalMonths = GetTotalMonthsFromAll(experiences);
            return FormatDuration(totalMonths);
        }

        public static string GetTotalExperienceStringMerged(List<Experience> experiences)
        {
            int totalMonths = GetTotalMonthsMerged(experiences);
            return FormatDuration(totalMonths);
        }

        public static TotalExperienceDetail GetTotalExperienceDetail(List<Experience> experiences)
        {
            var detail = new TotalExperienceDetail();

            if (experiences == null || !experiences.Any())
                return detail;

            detail.TotalMonths = GetTotalMonthsMerged(experiences);

            var currentJob = experiences.FirstOrDefault(e => !e.EndDate.HasValue && e.IsActive);
            if (currentJob != null)
            {
                detail.CurrentTotalMonths = currentJob.TotalMonths;
            }

            var pastJobs = experiences.Where(e => e.EndDate.HasValue && e.IsActive).ToList();
            var pastMonthsMerged = GetTotalMonthsMerged(pastJobs);
            detail.PastTotalMonths = pastMonthsMerged;

            return detail;
        }

        public static (int Years, int Months, int TotalMonths) GetCurrentExperience(List<Experience> experiences)
        {
            if (experiences == null || !experiences.Any())
                return (0, 0, 0);

            var currentJob = experiences.FirstOrDefault(e => !e.EndDate.HasValue && e.IsActive);

            if (currentJob == null)
                return (0, 0, 0);

            int totalMonths = currentJob.TotalMonths;
            return (totalMonths / 12, totalMonths % 12, totalMonths);
        }

        public static List<ExperienceDuration> GetExperiencesWithDuration(List<Experience> experiences)
        {
            if (experiences == null || !experiences.Any())
                return new List<ExperienceDuration>();

            return experiences
                .Where(e => e.IsActive)
                .Select(e => new ExperienceDuration
                {
                    CompanyName = e.CompanyName,
                    Role = e.Role,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    Duration = e.Duration,
                    DurationMonths = e.TotalMonths,
                    SimpleDuration = e.SimpleDuration
                })
                .OrderByDescending(e => e.StartDate)
                .ToList();
        }

        private static string FormatDuration(int totalMonths)
        {
            if (totalMonths <= 0) return "No experience";

            int years = totalMonths / 12;
            int months = totalMonths % 12;

            if (years > 0 && months > 0)
                return $"{years} year{(years > 1 ? "s" : "")} {months} month{(months > 1 ? "s" : "")}";
            else if (years > 0)
                return $"{years} year{(years > 1 ? "s" : "")}";
            else
                return $"{months} month{(months > 1 ? "s" : "")}";
        }
    }

    public class TotalExperienceDetail
    {
        public int TotalMonths { get; set; }
        public int CurrentTotalMonths { get; set; }
        public int PastTotalMonths { get; set; }

        public int Years => TotalMonths / 12;
        public int Months => TotalMonths % 12;

        public int CurrentYears => CurrentTotalMonths / 12;
        public int CurrentMonths => CurrentTotalMonths % 12;

        public int PastYears => PastTotalMonths / 12;
        public int PastMonths => PastTotalMonths % 12;

        public string Formatted => TotalMonths > 0
            ? $"{Years} year{(Years > 1 ? "s" : "")} {Months} month{(Months > 1 ? "s" : "")}"
            : "No experience";

        public string CurrentFormatted => CurrentTotalMonths > 0
            ? CurrentYears > 0
                ? $"{CurrentYears} year{(CurrentYears > 1 ? "s" : "")} {CurrentMonths} month{(CurrentMonths > 1 ? "s" : "")}"
                : $"{CurrentMonths} month{(CurrentMonths > 1 ? "s" : "")}"
            : "No current job";

        public string PastFormatted => PastTotalMonths > 0
            ? PastYears > 0
                ? $"{PastYears} year{(PastYears > 1 ? "s" : "")} {PastMonths} month{(PastMonths > 1 ? "s" : "")}"
                : $"{PastMonths} month{(PastMonths > 1 ? "s" : "")}"
            : "No past experience";

        public int YearsOnly => Years;
        public string DetailedBreakdown => $"{Formatted} total ({CurrentFormatted} current, {PastFormatted} past)";
    }

    public class ExperienceDuration
    {
        public string CompanyName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Duration { get; set; } = string.Empty;
        public int DurationMonths { get; set; }
        public string SimpleDuration { get; set; } = string.Empty;
        public bool IsCurrent => !EndDate.HasValue;
    }
}
