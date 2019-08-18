using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecoliisland.com.Models;
using ecoliisland.com.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace ecoliisland.com.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IGithubIssueService _githubIssueService;
        private readonly IObjectMapper _mapper;
        private readonly AppSettings _appSettings;
        
        public IndexModel(
            IGithubIssueService githubIssueService, 
            IObjectMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _githubIssueService = githubIssueService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        public string PageTitle => _appSettings.PageTitle ?? string.Empty;

        public IList<GithubIssue> GithubIssues { get; set; }
        
        public async Task OnGet(bool? disableCache)
        {
            var issueList = await _githubIssueService.GetIssuesAsync(disableCache ?? false);

            _mapper.Map(issueList, this);
        }
    }
}
