using QRCoder;

namespace Website.Services;

public class QrCodeService
{
    // Genererer en QR-kode ud fra en URL og returnerer den som et PNG-billede.
    public byte[] GenerateQrCode(string url)
    {
        // Opretter en generator til QR-koder.
        using var qrGenerator = new QRCodeGenerator();

        // Omdanner URL'en til QR-kodedata.
        // ECCLevel.Q angiver niveauet af fejlkorrektion.
        using var qrCodeData = qrGenerator.CreateQrCode(
            url,
            QRCodeGenerator.ECCLevel.Q
        );

        // Opretter et QR-kodebillede i PNG-format.
        using var qrCode = new PngByteQRCode(qrCodeData);

        // Returnerer billedet som bytes, så det kan sendes til browseren.
        return qrCode.GetGraphic(20);
    }
}