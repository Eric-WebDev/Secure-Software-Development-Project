using System;
using System.Collections.Generic;
using System.Text;

namespace BloggerApplication.Models
{
    class BlogPost
    {
        public string BlogPostId { get; set; }
        public string BlogPostCategory { get; set; }
        public string BlogPostTitle { get; set; }
        public string BlogPostContent { get; set; }


        public BlogPost(string blogId, string blogCategory, string blogTitle, string content)
        {
            BlogPostId = blogId;
            BlogPostCategory = blogCategory;
            BlogPostTitle = blogTitle;
            BlogPostContent = content;
        }
    }
}
