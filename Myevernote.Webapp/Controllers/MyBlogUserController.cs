using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyBlog.BusinessLayer;
using MyBlog.BusinessLayer.Results;
using MyBlog.Entities;
using Myevernote.Webapp.Filters;

namespace Myevernote.Webapp.Controllers
{
    [Auth]
    [AuthAdmin]
    [Exc]
    public class MyBlogUserController : Controller
    {
        private MyBlogUserManager myblogUserManager = new MyBlogUserManager();

        public ActionResult Index()
        {
            return View(myblogUserManager.List());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyBlogUser myBlogUser = myblogUserManager.Find(x => x.Id == id.Value);
            if (myBlogUser == null)
            {
                return HttpNotFound();
            }
            return View(myBlogUser);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MyBlogUser myBlogUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<MyBlogUser> res = myblogUserManager.Insert(myBlogUser);
                if(res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(myBlogUser);
                }

                return RedirectToAction("Index");
            }

            return View(myBlogUser);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyBlogUser myBlogUser = myblogUserManager.Find(x => x.Id == id.Value);
            if (myBlogUser == null)
            {
                return HttpNotFound();
            }
            return View(myBlogUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MyBlogUser myBlogUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<MyBlogUser> res = myblogUserManager.Update(myBlogUser);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(myBlogUser);
                }

                return RedirectToAction("Index");
            }
            return View(myBlogUser);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyBlogUser myBlogUser = myblogUserManager.Find(x => x.Id == id.Value);
            if (myBlogUser == null)
            {
                return HttpNotFound();
            }
            return View(myBlogUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MyBlogUser myBlogUser = myblogUserManager.Find(x => x.Id == id);
            myblogUserManager.Delete(myBlogUser);

            return RedirectToAction("Index");
        }
    }
}
