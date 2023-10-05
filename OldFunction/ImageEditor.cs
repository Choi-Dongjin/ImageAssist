using ImageAssist.SupportType;
using static ImageAssist.ImageAssistSystemDefault;

namespace ImageAssist.OldFunction
{
    public class ImageEditor
    {
        public ImageEditor(string filePath, LType? lType)
        {
            if (lType == null) { lType = LTypeDefault; }
        }

    }
}
