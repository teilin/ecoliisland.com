using System.Threading.Tasks;

namespace ecoliisland.com.Services
{
    public interface IGithubIssueService
    {
        Task<IssueList> GetIssuesAsync(bool disableCache);
    }
}