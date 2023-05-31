using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageAssist
{
    internal interface IImageEditor
    {
        public void Resize(int width, int height);
        public void Rotate(float angle);
        public void Crop(int startX, int startY, int width, int height);

    }
}
