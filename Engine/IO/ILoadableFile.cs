using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynaStudios.IO
{
    public interface ILoadableFile : IEqualityComparer<ILoadableFile>
    {
        void load();
    }
}
