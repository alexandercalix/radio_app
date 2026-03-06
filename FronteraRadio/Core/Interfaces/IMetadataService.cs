using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FronteraRadio.Core.Interfaces
{
    public interface IMetadataService
    {
        public void UpdateMetadata(string title, string artist, string imageName);
    }
}