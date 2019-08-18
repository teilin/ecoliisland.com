using System.Collections.Generic;
using ecoliisland.com.Models;

namespace ecoliisland.com.Services
{
    public class IssueList
    {
        public IList<GithubIssue> GithubIssues { get; set; } = new List<GithubIssue>();
    }
}