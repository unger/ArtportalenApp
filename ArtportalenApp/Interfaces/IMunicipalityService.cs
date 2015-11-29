using System.Collections.Generic;
using ArtportalenApp.Models;

namespace ArtportalenApp.Interfaces
{
    public interface IMunicipalityService
    {
        List<Municipality> GetAll();
    }
}