using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtportalenApp.Helpers;
using ArtportalenApp.Interfaces;
using ArtportalenApp.Models;
using ArtportalenApp.ViewModels;

namespace ArtportalenApp.Services
{
    public class MuncipalityService : IMunicipalityService
    {
        public List<Municipality> GetAll()
        {
            var result = new List<Municipality>();
            var enumValues = Enum.GetValues(typeof (MunicipalityFeatureEnum));

            foreach (MunicipalityFeatureEnum val in enumValues)
            {
                if (val != MunicipalityFeatureEnum.Undefined)
                {
                    result.Add(new Municipality
                    {
                        Name = CustomAttributeHelper.GetEnumDisplayValue(val)
                    });
                }
            }

            return result.OrderBy(x => x.Name).ToList();
        }
    }
}
