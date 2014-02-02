using System;
using System.Collections.Generic;
using System.Text;

namespace MSRT {
    public class Dimension {
        int width;
        int height;

        public int Width {
            set { this.width = value; }
            get { return width; }
        }
        public int Height {
            set { this.height = value; }
            get { return height; }
        }
    }
}
