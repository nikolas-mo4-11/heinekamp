using Heinekamp.Domain.AppConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Heinekamp.Controllers;

public class HomeController : Controller
{
    private readonly AppSettings _appSettings;
    
    public HomeController(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;
    }
    
    public IActionResult Index()
    {
        ViewBag.DocumentStorageDir = _appSettings.DocumentStorageDir;
        ViewBag.PreviewStorageDir = _appSettings.PreviewStorageDir;
        ViewBag.TypeIconDir = _appSettings.TypeIconDir;
        
        return View();
    }
}
