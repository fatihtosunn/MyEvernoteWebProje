using MyBlog.BusinessLayer.Abstract;
using MyBlog.BusinessLayer.Results;
using MyBlog.Common.Helpers;
using MyBlog.DataAccessLayer.EntityFramework;
using MyBlog.Entities;
using MyBlog.Entities.Messages;
using MyBlog.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.BusinessLayer
{
    public class MyBlogUserManager : ManagerBase<MyBlogUser>
    {
        public BusinessLayerResult<MyBlogUser> RegisterUser(RegisterViewModel data)
        {
            MyBlogUser user = Find(x => x.Username == data.Username || x.Email == data.EMail);
            BusinessLayerResult<MyBlogUser> res = new BusinessLayerResult<MyBlogUser>();
            if(user != null)
            {
                if(user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı..");
                }
                if(user.Email == data.EMail)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-Posta adresi kayıtlı..");
                }
            }
            else
            {
                int dbResult = base.Insert(new MyBlogUser()
                {
                    Username = data.Username,
                    Email = data.EMail,
                    Password = data.Password,
                    ProfileImageFile = "user_boy.png",
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = false,
                    IsAdmin = false,
                    
                });

                if(dbResult > 0)
                {
                    res.Result = Find(x => x.Email == data.EMail && x.Username == data.Username);

                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/UserActivate/{res.Result.ActivateGuid}";
                    string body = $"Merhaba {res.Result.Username}; <br><br>Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank'>tıklayınız..</a>";

                    MailHelper.SendMail(body, res.Result.Email,"Hesap Aktifleştirme");

                }
            }
            return res;
        }

        public BusinessLayerResult<MyBlogUser> GetUserById(int id)
        {
            BusinessLayerResult<MyBlogUser> res = new BusinessLayerResult<MyBlogUser>();
            res.Result = Find(x => x.Id == id);

            if(res.Result == null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı bulunamadı");
            }
            return res;
        }

        public BusinessLayerResult<MyBlogUser> LoginUser(LoginViewModel data)
        {
            BusinessLayerResult<MyBlogUser> res = new BusinessLayerResult<MyBlogUser>();
            res.Result = Find(x => x.Username == data.Username && x.Password == data.Password);

            if (res.Result != null)
            {
                if (!res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserIsNotActive, "Kullanıcı aktifleştirilmemiştir..");
                    res.AddError(ErrorMessageCode.CheckYourEmail, "Lütfen E-Posta adresinizi kontrol ediniz..");
                }

            }
            else
            {
                res.AddError(ErrorMessageCode.UsernameOrPassWrong, "Kullanıcı adı yada şifre uyuşmuyor..");
            }
            return res;
        }

        public BusinessLayerResult<MyBlogUser> UpdateProfile(MyBlogUser data)
        {
            //EvernoteUser db_user = Find(x => x.Id != data.Id && (x.Username == data.Username || x.Email == data.Email));

            MyBlogUser db_user = Find(x => x.Id != data.Id && (x.Username == data.Username || x.Email == data.Email));
            BusinessLayerResult<MyBlogUser> res = new BusinessLayerResult<MyBlogUser>();

            if(db_user != null && db_user.Id != data.Id)
            {
                if(db_user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }
                if(db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-Posta adresi kayıtlı.");
                }
                return res;
            }
            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;

            if (string.IsNullOrEmpty(data.ProfileImageFile) == false)
            {
                res.Result.ProfileImageFile = data.ProfileImageFile;
            }
            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.ProfileClouldNotUpdated, "Profil Güncellenemedi");
            }
            return res;
        }

        public BusinessLayerResult<MyBlogUser> RemoveUserById(int id)
        {
            BusinessLayerResult<MyBlogUser> res = new BusinessLayerResult<MyBlogUser>();
            MyBlogUser user = Find(x => x.Id == id);
            if(user != null)
            {
                if(Delete(user) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı Silinemedi.");
                    return res;
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı bulunamadı.");
            }
            return res;
        }

        public BusinessLayerResult<MyBlogUser> ActivateUser(Guid activateId)
        {
            BusinessLayerResult<MyBlogUser> res = new BusinessLayerResult<MyBlogUser>();
            res.Result = Find(x => x.ActivateGuid == activateId);
            if(res.Result != null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı zaten aktif edilmiştir..");
                    return res;
                }
                res.Result.IsActive = true;
                Update(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExists, "Aktifleştirilecek kullanıcı bulunamadı..");
            }

            return res;
        }


        // method hiding : new ile method gizleme
        public new BusinessLayerResult<MyBlogUser> Insert(MyBlogUser data)
        {
            MyBlogUser user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<MyBlogUser> res = new BusinessLayerResult<MyBlogUser>();

            res.Result = data;

            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı..");
                }
                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-Posta adresi kayıtlı..");
                }
            }
            else
            {
                res.Result.ProfileImageFile = "user_boy.png";
                res.Result.ActivateGuid = Guid.NewGuid();

                if(base.Insert(res.Result) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotInserted, "Kullanıcı Eklenemedi");
                } 
            }
            return res;
        }

        // method hiding
        public new BusinessLayerResult<MyBlogUser> Update(MyBlogUser data)
        {
            MyBlogUser db_user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<MyBlogUser> res = new BusinessLayerResult<MyBlogUser>();
            res.Result = data;

            if (db_user != null && db_user.Id != data.Id)
            {
                if (db_user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }

                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;
            res.Result.IsActive = data.IsActive;
            res.Result.IsAdmin = data.IsAdmin;

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.UserCouldNotUpdated, "Kullanıcı güncellenemedi.");
            }

            return res;
        }
    }
}
