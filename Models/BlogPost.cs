namespace BloggerApplication.Models
{
    class BlogPost
    {
        public string BlogPostId { get; set; }
        public string BlogPostCategory { get; set; }
        public string BlogPostTitle { get; set; }
        public string BlogPostContent { get; set; }
        public BlogPost(string blogId, string blogCategory, string blogPostTitle, string blogPostContent)
        {
            BlogPostId = blogId;
            BlogPostCategory = blogCategory;
            BlogPostTitle = blogPostTitle;
            BlogPostContent = blogPostContent;
        }
    }
}
