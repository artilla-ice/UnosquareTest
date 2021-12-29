using System.IO;
using Android.Graphics;
using AndroidX.ExifInterface.Media;
using UnosquareTest.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(UnosquareTest.Droid.Services.ImageResizer))]
namespace UnosquareTest.Droid.Services
{
    public class ImageResizer : IResizeImageManager
    {
        public ImageResizer()
        {
        }

        public byte[] ResizeImage(byte[] imageData, float width, float height, string path)
        {
            return ResizeImageAndroid ( imageData, width, height, path);
        }

        public static byte[] ResizeImageAndroid (byte[] imageData, float width, float height, string path)
        {
            // Load the bitmap
            Bitmap originalImage = BitmapFactory.DecodeByteArray (imageData, 0, imageData.Length);

            var matrix = new Matrix();
            var scaleWidth = ((float)width) / originalImage.Width;
            var scaleHeight = ((float)height) / originalImage.Height;

            ExifInterface ei = new ExifInterface(path);
            var orientation = ei.GetAttributeInt(ExifInterface.TagOrientation, ExifInterface.OrientationNormal);

            switch (orientation)
            {
                case ExifInterface.OrientationRotate90:
                    matrix.PostRotate(90);
                    break;
                case ExifInterface.OrientationRotate180:
                    matrix.PostRotate(180);
                    break;
                case ExifInterface.OrientationRotate270:
                    matrix.PostRotate(270);
                    break;
                default:
                    break;
            }

            matrix.PreScale(scaleWidth, scaleHeight);
            Bitmap resizedImage = Bitmap.CreateBitmap(originalImage, 0, 0, originalImage.Width, originalImage.Height, matrix, true);

            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Compress (Bitmap.CompressFormat.Jpeg, 100, ms);
                return ms.ToArray ();
            }
        }
    }
}