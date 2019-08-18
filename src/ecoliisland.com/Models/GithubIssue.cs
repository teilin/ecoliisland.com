using System;

namespace ecoliisland.com.Models
{
    public class GithubIssue
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTimeOffset Created { get; set; }
        public bool IsClosed { get; set; }
    }
}