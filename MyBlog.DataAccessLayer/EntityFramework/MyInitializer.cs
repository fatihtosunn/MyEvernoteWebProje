using MyBlog.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.DataAccessLayer.EntityFramework
{
    public class MyInitializer : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            // Adding admin user..
            MyBlogUser admin = new MyBlogUser()
            {
                Name = "Fatih",
                Surname = "Tosunoğlu",
                Email = "ftosunoglu0@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                Username = "ftosunoglu",
                Password = "26081905gs",
                ProfileImageFile = "user_boy.png",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "ftosunoglu"
            };
            // Adding standart user..
            MyBlogUser standartuser = new MyBlogUser()
            {
                Name = "Fatih",
                Surname = "Tosun",
                Email = "ftosunoglu03@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                Username = "ftosunoglu2",
                Password = "26081905gs",
                ProfileImageFile = "user_boy.png",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "ftosunoglu2"
            };
            context.MyBlogUsers.Add(admin);
            context.MyBlogUsers.Add(standartuser);

            // ADDİNG FAKE USER
            //for (int i = 0; i < 8; i++)
            //{
            //    MyBlogUser user = new MyBlogUser()
            //    {
            //        Name = FakeData.NameData.GetFirstName(),
            //        Surname = FakeData.NameData.GetSurname(),
            //        Email = FakeData.NetworkData.GetEmail(),
            //        ActivateGuid = Guid.NewGuid(),
            //        IsActive = true,
            //        IsAdmin = false,
            //        Username = $"user{i}",
            //        Password = "123",
            //        ProfileImageFile = "user_boy.png",
            //        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1),DateTime.Now),
            //        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
            //        ModifiedUsername = $"user{i}"
            //    };

            //    context.MyBlogUsers.Add(user);
            //}
            //context.SaveChanges();

            //List<MyBlogUser> userlist = context.MyBlogUsers.ToList();

            // Adding fake categories
            //for (int i = 0; i < 10; i++)
            //{
            //    Category cat = new Category()
            //    {
            //        Title = FakeData.PlaceData.GetStreetName(),
            //        Description = FakeData.PlaceData.GetAddress(),
            //        CreatedOn = DateTime.Now,
            //        ModifiedOn = DateTime.Now,
            //        ModifiedUsername = "ftosunoglu"
            //    };
            //    context.Categories.Add(cat);

            //    // Adding fake notes
            //    for (int k = 0; k < FakeData.NumberData.GetNumber(5,9); k++)
            //    {
            //        MyBlogUser owner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];
            //        Note note = new Note()
            //        {
            //            Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
            //            Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
            //            IsDraft = false,
            //            LikeCount = FakeData.NumberData.GetNumber(1,9),
            //            Owner = owner,
            //            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1),DateTime.Now),
            //            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
            //            ModifiedUsername = owner.Username,
            //        };
            //        context.Notes.Add(note);

            //        // Adding fake comments
            //        for (int j = 0; j < FakeData.NumberData.GetNumber(3,5); j++)
            //        {
            //            MyBlogUser comment_owner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];
            //            Comment comment = new Comment()
            //            {
            //                Text = FakeData.TextData.GetSentence(),
            //                Owner = comment_owner,
            //                CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
            //                ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
            //                ModifiedUsername = comment_owner.Username
            //            };
            //            context.Comments.Add(comment);
            //        }

            //        // Adding fake liked

            //        for (int m = 0; m < note.LikeCount; m++)
            //        {
            //            Liked liked = new Liked()
            //            {
            //                LikedUser = userlist[m]
            //            };
            //            note.Likes.Add(liked);
            //        }
            //    }
            //}
        }
    }
}
