﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageAssist
{
    public class LibraryType
    {
        public const LType LTypeDefault = LType.ImageSharp;
    }
    public enum LType
    {
        None,
        OpenCV,
        ImageSharp,
    }
}
