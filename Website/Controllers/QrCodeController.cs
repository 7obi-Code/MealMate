using Microsoft.AspNetCore.Mvc;
using Website.Services;

namespace Website.Controllers;

public class QrCodeController : Controller
{
    private readonly QrCodeService _qrCodeService;

    // Opretter en instans af QrCodeService, som bruges til at generere QR-koder.
    public QrCodeController()
    {
        _qrCodeService = new QrCodeService();
    }

    // HTTP GET endpoint, der genererer en QR-kode til MealMates landingpage.
    [HttpGet]
    public IActionResult LandingPage()
    {
        // Danner URL'en til HomeControllerens Index-metode.
        string url = Url.Action(
            "Index", "Home", null, Request.Scheme)!;

        // Kalder servicen, som genererer QR-koden ud fra URL'en.
        byte[] qrCode = _qrCodeService.GenerateQrCode(url);

        // Returnerer QR-koden som et PNG-billede til browseren.
        return File(qrCode, "image/png");
    }

    // HTTP GET endpoint, der genererer en QR-kode til en specifik restaurant.
    [HttpGet]
    public IActionResult Restaurant(int id)
    {
        // Danner URL'en til restaurantens side ud fra restaurantens ID.
        string url = Url.Action(
            "Index", "Restaurant", new { id }, Request.Scheme)!;

        // Genererer QR-koden med restaurantens URL.
        byte[] qrCode = _qrCodeService.GenerateQrCode(url);

        // Returnerer QR-koden som et PNG-billede til browseren.
        return File(qrCode, "image/png");
    }
}