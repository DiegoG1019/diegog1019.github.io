using QRCoder;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Resources;

namespace DiegoG.WebTools.Services;

public static class QRCodeHelpers
{
    public const int DefaultPPM = 8;
    public const bool DefaultForceUTF8 = false;
    public const bool DefaultUTF8BOM = false;
    public const int DefaultRequestedVersion = -1;
    public const QRCodeGenerator.ECCLevel DefaultECC = QRCodeGenerator.ECCLevel.Default;
    public const QRCodeGenerator.EciMode DefaultEciMode = QRCodeGenerator.EciMode.Default;

    public static async Task<byte[]> GenerateQrCode(
        string content, 
        QRCodeGenerator.ECCLevel ecc, 
        QRCodeGenerator.EciMode eciMode, 
        int ppm,
        bool forceUtf8,
        bool utf8BOM,
        int requestedVersion,
        Stream? logoBytes,
        ILogger log
    )
    {
        // I thoroughly despise this. Too many copies of stuff thanks to the QRCode gen, and another, final copy again because of this shenanigan
        // I wanted to pull
        await Task.Yield();

        log.LogDebug("Configuring QR Code encoder");
        using QRCodeGenerator qrGenerator = new();
        using QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, ecc, forceUtf8, utf8BOM, eciMode, requestedVersion);
        using PngByteQRCode qrCode = new(qrCodeData);

        if (logoBytes is not null and { Length: not 0 })
        {
            log.LogDebug("Loading logo image");
            using var logo = await Image.LoadAsync<Rgba32>(logoBytes);

            log.LogTrace("Configuring drawing options");
            var graphicsOptions = new DrawingOptions()
            {
                GraphicsOptions = new GraphicsOptions()
                {
                    AlphaCompositionMode = PixelAlphaCompositionMode.Src
                }
            };

            log.LogDebug("Obtaining QR Code graphic to modify");
            using var memstream = new MemoryStream(qrCode.GetGraphic(ppm));
            log.LogDebug("Loading QR Code graphic as an image");
            using var qrcodeImage = await Image.LoadAsync<Rgba32>(memstream);

            float threshold = 0.1F;
            Color sourceColor = Color.White;
            Color targetColor = Color.Transparent;

            log.LogTrace("Creating Recoloring Brush");
            var brush = new RecolorBrush(sourceColor, targetColor, threshold);

            log.LogTrace("Filling white space for logo");
            qrcodeImage.Mutate(x => x.Fill(graphicsOptions, brush));

            var newLogoSize = qrcodeImage.Size / 2;
            var logoPos = new Point(
                qrcodeImage.Size.Width / 2 - newLogoSize.Width / 2,
                qrcodeImage.Size.Height / 2 - newLogoSize.Height / 2
            );

            log.LogDebug(
                "Positioning logo image at x{posx},y{posy}, with size w{sizew},h{sizeh}", 
                logoPos.X, 
                logoPos.Y, 
                newLogoSize.Width, 
                newLogoSize.Height
            );

            log.LogTrace("Creating final image data");
            using var finalImage = new Image<Rgba32>(qrcodeImage.Size.Width, qrcodeImage.Size.Height);
            log.LogTrace("Resizing logo");
            logo.Mutate(o => o.Resize(newLogoSize));

            log.LogDebug("Drawing Logo over QR Code image in final image");
            // take the 2 source images and draw them onto the image
            finalImage.Mutate(o => o
                .Clear(Color.White)
                .DrawImage(qrcodeImage, new Point(0, 0), 1f) // draw the second next to it
                .DrawImage(logo, logoPos, 1f) // draw the first one top left
            );

            log.LogTrace("Creating output memory stream");
            using var outputStream = new MemoryStream();

            await finalImage.SaveAsPngAsync(outputStream);
            log.LogDebug("Converting memory stream data to array");
            return outputStream.ToArray();
        }
        else
        {
            log.LogDebug("Obtaining QR Code graphic");
            return qrCode.GetGraphic(ppm);
        }
    }

    public static bool TryGetECCFromChar(char? ecc, out QRCodeGenerator.ECCLevel code)
    {
        switch (ecc)
        {
            case null:
                code = QRCodeGenerator.ECCLevel.Default;
                return true;
            case 'H' or 'h':
                code = QRCodeGenerator.ECCLevel.H;
                return true;
            case 'Q' or 'q':
                code = QRCodeGenerator.ECCLevel.Q;
                return true;
            case 'L' or 'l':
                code = QRCodeGenerator.ECCLevel.L;
                return true;
            case 'M' or 'm':
                code = QRCodeGenerator.ECCLevel.M;
                return true;
            default:
                code = QRCodeGenerator.ECCLevel.Default;
                return false;
        }
    }
}
