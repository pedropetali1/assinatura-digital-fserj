using EmailSignatureApp.Models;
using EmailSignatureApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmailSignatureApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly AdService _adService;
        private readonly SignatureService _signatureService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(AdService adService, SignatureService signatureService, ILogger<HomeController> logger)
        {
            _adService = adService;
            _signatureService = signatureService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var username = User.Identity?.Name ?? string.Empty;

            var user = _adService.GetUserByUsername(username);

            if (user == null)
            {
                ViewBag.Error = "Não foi possível carregar seus dados do Active Directory. Entre em contato com o suporte de TI.";
                return View(new SignatureViewModel());
            }

            return View(new SignatureViewModel
            {
                DisplayName   = user.DisplayName,
                Mail          = user.Mail,
                Description   = user.Description,
                Office        = user.Office,
                SignatureHtml = _signatureService.GenerateHtml(user)
            });
        }
    }
}