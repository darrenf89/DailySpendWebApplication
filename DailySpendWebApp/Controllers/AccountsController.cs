using DailySpendBudgetWebApp.Data;
using DailySpendBudgetWebApp.Models;
using DailySpendBudgetWebApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DailySpendBudgetWebApp.Controllers
{
    public class AccountsController : Controller
    {
        private readonly ApplicationDBContext _db;

        private readonly ISecurityHelper _sh;

        private readonly IEmailSenderCustom _es;

        public AccountsController(ApplicationDBContext db, ISecurityHelper sh, IEmailSenderCustom es)
        {
            _db = db;
            _sh = sh;
            _es = es;
        }

        //GET
        public IActionResult Register()
        {
            
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterModel obj, string ReturnUrl)
        {
            UserAccounts? A = new();

            if (ModelState.IsValid)
            {
                if (obj.isAgreedToTerms == true)
                {
                    if (_sh.CheckUniqueEmail(obj.Email))
                    {

                        if (obj.Password == obj.ConfirmPassword)
                        {
                            if (!obj.isAgreedToTerms)
                            {
                                ModelState.AddModelError("isAgreedToTerms", "* Hey! you have to agree to our terms...don't be rude!");
                            }
                            else
                            {

                                obj = _sh.CreateUserSecurityDetails(obj);

                                A.Email = obj.Email;
                                A.Password = obj.Password;
                                A.isDPAPermissions = obj.isDPAPermissions;
                                A.isAgreedToTerms = obj.isAgreedToTerms;
                                A.Salt = obj.Salt;
                                A.AccountCreated = DateTime.Now;
                                A.LastLoggedOn = DateTime.Now;
                                A.NickName = obj.NickName;
                                A.isEmailVerified = false;

                                _db.UserAccounts.Add(A);
                                _db.SaveChanges();

                                //string Token = _sh.GenerateEmailToken(A);

                                //string EmailMessage = _es.GenerateVerifyEmail(Token);
                                //_es.SendEmailAsync(A.Email, "Verify your dBudget Email", EmailMessage);

                                HttpContext.SignOutAsync();
                                return Redirect(ReturnUrl == null ? "/Home" : ReturnUrl);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("ConfirmPassword", "* Whoops it would seem you provided two different passwords, please try again dummy!");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("isEmailUnique", "* Whoops someone has already used this email. Your email? try logging in. Not your email? ... dont use other people emails that's rude!");

                    }
                }
                else
                {
                    ModelState.AddModelError("isAgreedToTerms", "You have to agree to our Terms, its the Terms sorry!");
                }
            }
            return View(obj);
        }

        //GET
        public IActionResult Logon()
        {
            try
            {
                string AlreadyLoggedIn = User.FindFirst(ClaimTypes.Name).Value;
                return RedirectToAction("Index", "Home");

            }
            catch
            {
                return View();
            }
            
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logon(LogonPageModel obj, string ReturnUrl)
        {
            if (_sh.CheckEmailVerified(obj.Email))
            {

                if (_sh.CheckPassword(obj.Password, obj.Email))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, obj.Email)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "Login");

                    var AuthProperties = new AuthenticationProperties
                    {
                        IssuedUtc = DateTime.UtcNow,
                        RedirectUri = "/Accounts/Home"
                    };

                    if (obj.isRememberMe == true)
                    {
                        AuthProperties.AllowRefresh = false;
                        AuthProperties.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(10);
                        AuthProperties.IsPersistent = true;
                    }
                    else
                    {
                        AuthProperties.AllowRefresh = true;
                        AuthProperties.ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20);
                        AuthProperties.IsPersistent = false;
                    }


                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), AuthProperties);

                    try
                    {
                        var Accounts = _db.UserAccounts
                            .Where(x => x.Email == obj.Email);
                        var Account = Accounts.First();
                        Account.LastLoggedOn = DateTime.UtcNow;
                        _db.UserAccounts.Update(Account);
                        _db.SaveChanges();
                    }
                    catch
                    {
                        throw (new Exception("Not Found"));
                    }

                    if(ReturnUrl == null)
                    {
                        ReturnUrl = "/Home";
                    }
                    if (ReturnUrl == "/Accounts/Home")
                    {
                        ReturnUrl = "/Home";
                    }

                    return RedirectToAction("UpdateBudget", "Home", new {ReturnURL = ReturnUrl ?? "/Home"});
                    
                }
                else
                {
                    ModelState.AddModelError("IncorrectLogin", "Those aren't the correct details, use the right details next time!");
                    return View(obj);
                }
            }
            else
            {
                ModelState.AddModelError("EmailVerified", "You haven't verified your Email! Did you use a fake one .. tut tut");
                return View(obj);
            }
        }

        public IActionResult Home()
        {
            try
            {
                string AlreadyLoggedIn = User.FindFirst(ClaimTypes.Name).Value;
                return RedirectToAction("Index", "Home");

            }
            catch
            {
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Home", "Accounts");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult VerifyEmail(string Token)
        {
            if (_sh.ValidateEmailToken(Token))
            {
                string claimdetails = _sh.GetClaim(Token, "nameid");
                //Show a congrate messages and ask them if they want to validate account or delete account.
                //get the person ID and set the validate Email Flag to true
            }
            else
            {
                //if it fails ask them if they want to generate a new validation email.
            }

            return View();
        }

        //public IActionResult Logout()
        //{
        //    return View();
        //}

    }
}
