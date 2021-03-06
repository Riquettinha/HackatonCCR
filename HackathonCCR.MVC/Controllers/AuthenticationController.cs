﻿using HackathonCCR.EDM.Models;
using HackathonCCR.MVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace HackathonCCR.MVC.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly Services.IAuthenticationService _authenticationService;
        private readonly Services.IUserService _userService;
        private readonly Services.ICategoryService _categoryService;

        public AuthenticationController(Services.IAuthenticationService authenticationService, Services.IUserService userService, Services.ICategoryService categoryService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _categoryService = categoryService;
        }


        [HttpGet]
        public ActionResult Login()
        {
            if (HttpContext.User != null && HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
            {
                return LogOff();
            }

            return View("Login", null);
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var user = _userService.Get(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Usuário não encontrado");
            }
            else
            {
                var confirmLogin = _authenticationService.ConfirmLogin(user, model);

                if (confirmLogin)
                {
                    Authenticate(user);
                    return UserDash(user);
                }
                else
                {
                    ModelState.AddModelError("Password", "Senha incorreta");
                }
            }

            return View(model);
        }

        private void Authenticate(EDM.Models.User user)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Role, ((int)user.Type).ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.Name));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim(ClaimTypes.GivenName, user.Name.Split(' ')[0]));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()));

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true,
                IssuedUtc = DateTime.UtcNow,
                RedirectUri = "/Home/Index"
            };

            var identity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                authProperties);
        }

        [HttpGet]
        public ActionResult RegisterDiscover()
        {
            if (HttpContext.User != null && HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
            {
                return LogOff();
            }

            return View();
        }

        [HttpPost]
        public ActionResult RegisterDiscover(RegisterDiscoverModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var user = _userService.Get(model.Email);
            if (user != null)
            {
                ModelState.AddModelError("Email", "Email já cadastrado");
            }
            else
            {
                user = _userService.Register(model);

                Authenticate(user);

                return UserDash(user);
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult RegisterMentor()
        {
            if (HttpContext.User != null && HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
            {
                return LogOff();
            }

            ViewBag.Graduations = _categoryService.GetCategorySelectList();
            return View();
        }

        [HttpPost]
        public ActionResult RegisterMentor(RegisterMentorModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Graduations = _categoryService.GetCategorySelectList();
                return View(model);
            }

            var user = _userService.Get(model.Email);
            if (user != null)
            {
                ModelState.AddModelError("Email", "Email já cadastrado");
            }
            else
            {
                user = _userService.Register(model);

                Authenticate(user);

                return UserDash(user);
            }

            ViewBag.Graduations = _categoryService.GetCategorySelectList();
            return View(model);
        }

        public string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                return Url.Action("Index", "Home");

            return returnUrl;
        }

        public ActionResult LogOff()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult UserDash(User user)
        {
            if (user?.Type == EDM.Enums.User.Type.Discover)
            {
                return RedirectToAction("DashStudent", "Home");
            }
            else if (user?.Type == EDM.Enums.User.Type.Mentor)
            {
                return RedirectToAction("DashMentor", "Home");
            }

            return LogOff();
        }
    }
}
