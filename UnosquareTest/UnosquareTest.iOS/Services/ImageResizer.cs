using System;
using UIKit;
using CoreGraphics;
using Xamarin.Forms;
using UnosquareTest.Services;

[assembly: Dependency(typeof(UnosquareTest.iOS.Services.ImageResizer))]
namespace UnosquareTest.iOS.Services
{
    public class ImageResizer : IResizeImageManager
    {
        public ImageResizer()
        {
        }

        public byte[] ResizeImage(byte[] imageData, float width, float height, string path)
        {
            return ResizeImageIOS(imageData, width, height);
        }


        public static byte[] ResizeImageIOS(byte[] imageData, float width, float height)
        {
            UIImage originalImage = ImageFromByteArray(imageData);
            UIImageOrientation orientation = originalImage.Orientation;

            //IMPORTANT : WE NEED TO ROTATE 90 DEGREES THE IMAGE UNTIL MEDIAPICKER.CAPTUREPHOTOASYNCH ORIENTATION GETS FIXED
            var degreesToRotate = 90;
            float Radians = degreesToRotate * (float)Math.PI / 180;

            UIView view = new UIView(frame: new CGRect(0, 0, width, height));
            CGAffineTransform t = CGAffineTransform.MakeRotation(Radians);
            view.Transform = t;
            CGSize size = view.Frame.Size;

            UIGraphics.BeginImageContext(size);
            CGContext context = UIGraphics.GetCurrentContext();

            context.TranslateCTM(size.Width / 2, size.Height / 2);
            context.RotateCTM(Radians);
            context.ScaleCTM(1, -1);

            context.DrawImage(new CGRect(-width / 2, -height / 2, width, height), originalImage.CGImage);

            UIImage imageCopy = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return imageCopy.AsJPEG().ToArray();
        }

        public static UIKit.UIImage ImageFromByteArray(byte[] data)
        {
            if (data == null)
            {
                return null;
            }

            UIKit.UIImage image;
            try
            {
                image = new UIKit.UIImage(Foundation.NSData.FromArray(data));
            }
            catch (Exception e)
            {
                Console.WriteLine("Image load failed: " + e.Message);
                return null;
            }
            return image;
        }
    }
}