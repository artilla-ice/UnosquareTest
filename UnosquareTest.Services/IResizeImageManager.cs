
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UnosquareTest.Services
{
    public interface IResizeImageManager
    {
        byte[] ResizeImage(byte[] imageData, float width, float height, string path);
    }
}
