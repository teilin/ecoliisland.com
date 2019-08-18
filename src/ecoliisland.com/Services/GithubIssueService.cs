using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecoliisland.com.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Octokit;

namespace ecoliisland.com.Services
{
    public class GithubIssueService : IGithubIssueService
    {
        private const string CacheKey = nameof(GithubIssueService);

        private readonly IHostingEnvironment _env;
        private readonly IMemoryCache _cache;
        private readonly AppSettings _appSettings;

        public GithubIssueService(
            IHostingEnvironment env,
            IMemoryCache cache,
            IOptions<AppSettings> appSettings)
        {
            _env = env;
            _cache = cache;
            _appSettings = appSettings.Value;
        }

        public async Task<IssueList> GetIssuesAsync(bool disableCache)
        {
            if (string.IsNullOrEmpty(_appSettings.GithubApiKey))
            {
                return new IssueList() { GithubIssues = DesignData.Issues };
            }

            var result = _cache.Get<IssueList>(CacheKey);

            if (result == null)
            {
                result = await GetIssueList();

                _cache.Set(CacheKey, result, new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                });
            }

            return result;
        }

        private async Task<IssueList> GetIssueList()
        {
            var client = new GitHubClient(new ProductHeaderValue(_appSettings.GithubOwner));
            
            client.Credentials = new Credentials(_appSettings.GithubApiKey);

            var repos = await client.Repository.Get(_appSettings.GithubOwner, _appSettings.RepositoryName);

            var issues = await client.Issue.GetAllForRepository(repos.Id);
            
            var issueList = new IssueList();

            foreach (var issue in issues.Where(w => w.ClosedAt == null))
            {
                var tmp = new GithubIssue()
                {
                    Id = issue.Id,
                    Title = issue.Title,
                    Body = issue.Body,
                    Created = issue.CreatedAt,
                    IsClosed = issue.Locked
                };
                
                issueList.GithubIssues.Add(tmp);
            }

            return issueList;
        }

        private static class DesignData
        {
            public static readonly IList<GithubIssue> Issues = new List<GithubIssue>
            {
                new GithubIssue()
                {
                    Id = 1,
                    Title = "",
                    Body = "",
                    IsClosed = false
                }
            };
        }
    }
}