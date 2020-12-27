using MyBlog.BusinessLayer;
using MyBlog.BusinessLayer.Results;
using MyBlog.Entities;
using MyBlog.Entities.Messages;
using MyBlog.Entities.ValueObjects;
using Myevernote.Webapp.Filters;
using Myevernote.Webapp.Models;
using Myevernote.Webapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Myevernote.Webapp.Controllers
{
    [Exc]
    public class HomeController : Controller
    {
        private NoteManager notemanager = new NoteManager();
        private CategoryManager categorymanager = new CategoryManager();
        private MyBlogUserManager eum = new MyBlogUserManager();

        // GET: Home
        public ActionResult Index()
        {
            return View(notemanager.ListQueryable().Where(x => x.IsDraft == false).OrderByDescending(x => x.ModifiedOn).ToList());
            //return View(notemanager.ListQueryable().OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category cat = categorymanager.Find(x => x.Id == id.Value);

            if (cat == null)
            {
                return HttpNotFound();
            }
            return View("Index", cat.Notes.Where(x => x.IsDraft == false).OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult MostLiked()
        {
            return View("Index",notemanager.ListQueryable().Where(x => x.IsDraft == false).OrderByDescending(x => x.LikeCount).ToList());
        }

        public ActionResult About()
        {
            return View();
        }

        [Auth]
        public ActionResult ShowProfile()
        {

            BusinessLayerResult<MyBlogUser> res = eum.GetUserById(CurrentSession.User.Id);

            if(res.Errors.Count > 0)
            {
                ErrorViewModel ErrorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors

                };
                return View("Error", ErrorNotifyObj);
            }

            return View(res.Result);
        }

        [Auth]
        public ActionResult EditProfile()
        {

            BusinessLayerResult<MyBlogUser> res = eum.GetUserById(CurrentSession.User.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel ErrorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors

                };
                return View("Error", ErrorNotifyObj);
            }

            return View(res.Result);

        }

        [Auth]
        [HttpPost]
        public ActionResult EditProfile(MyBlogUser model, HttpPostedFile ProfileImage)
        {
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                if (ProfileImage != null &&
                (ProfileImage.ContentType == "image/jpeg" ||
                ProfileImage.ContentType == "image/jpg" ||
                ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";

                    ProfileImage.SaveAs(Server.MapPath($"~/Images/{filename}"));
                    model.ProfileImageFile = filename;
                }

                BusinessLayerResult<MyBlogUser> res = eum.UpdateProfile(model);

                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Profil Güncellenemedi",
                        RedirectingUrl = "/Home/EditProfile"
                    };

                    return View("Error", errorNotifyObj);
                }
                CurrentSession.Set<MyBlogUser>("login", res.Result);
                return RedirectToAction("ShowProfile");
            }
            return View(model);
        }

        [Auth]
        public ActionResult DeleteProfile()
        {

            BusinessLayerResult<MyBlogUser> res = eum.RemoveUserById(CurrentSession.User.Id);

            if(res.Errors.Count > 0)
            {
                ErrorViewModel messages = new ErrorViewModel() {
                    Items = res.Errors,
                    Title = "Profil Silinemedi",
                    RedirectingUrl = "/Home/ShowProfile"

                };
                return View("Error", messages);
            }
            Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult Login()
        {
             return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<MyBlogUser> res = eum.LoginUser(model);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                CurrentSession.Set<MyBlogUser>("login", res.Result); //Session'a kullanıcı bilgi saklama..
                return RedirectToAction("Index"); // yönlendirme

            }

            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                BusinessLayerResult<MyBlogUser> res = eum.RegisterUser(model);

                if(res.Errors.Count > 0)
                {
                    if(res.Errors.Find(x => x.Code == ErrorMessageCode.UserIsNotActive) != null)
                    {
                        ViewBag.SetLink = "http://Home/Activate/1242-4254-57345";
                    }

                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));

                    return View(model);
                }
                OkViewModel notifyObj = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/Login",

                };
                notifyObj.Items.Add("Lütfen E-posta adresinize gönderdiğimiz aktivasyon link'ine tıklayarak hesabınızı aktive edin. Eğer hesabınızı aktive ettirmezseniz not ekleyemez ve beğeni yapamazsınız.");
                return View("Ok",notifyObj);
            }

            return View(model);
        }

        public ActionResult UserActivate(Guid id)
        {
            BusinessLayerResult<MyBlogUser> res = eum.ActivateUser(id);

            if(res.Errors.Count > 0)
            {
                ErrorViewModel ErrorNotifyObj = new ErrorViewModel() {
                    Title = "Hesap Bulunamadı..",
                    RedirectingUrl = "/Home/Index",
                    Items = res.Errors

                };
                return View("Error", ErrorNotifyObj);
            }
            OkViewModel okNotifyObj = new OkViewModel()
            {
                Title = "Hesabınız aktifleştirildi..",
                RedirectingUrl = "/Home/Login"
            };
            okNotifyObj.Items.Add("Hesabınız aktifleştirilmiştir. Ana sayfaya yönlendiriliyorsunuz..");

            return View("Ok",okNotifyObj);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult HasError()
        {
            return View();
        }
    }
}