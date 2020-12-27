using MyBlog.Common;
using MyBlog.Entities;
using Myevernote.Webapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Myevernote.Webapp.Init
{
    public class WebCommon : ICommon
    {
        public string GetCurrentUsername()
        {
            MyBlogUser user = CurrentSession.User;

            if (user != null)
                return user.Username;
            else
                return "system";
        }
    }
}