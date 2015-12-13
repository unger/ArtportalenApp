using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Artportalen.Model;
using ArtportalenApp.Helpers;
using ArtportalenApp.Interfaces;
using ArtportalenApp.Models;
using ArtportalenApp.ViewModels;

namespace ArtportalenApp.Services
{
    public class ProvinceService : IProvinceService
    {
        public List<Province> GetAll()
        {
            var result = new List<Province>();
            var enumValues = Enum.GetValues(typeof (ProvinceFeatureEnum));

            foreach (ProvinceFeatureEnum val in enumValues)
            {
                if (val != ProvinceFeatureEnum.Undefined)
                {
                    result.Add(new Province
                    {
                        Name = CustomAttributeHelper.GetEnumDisplayValue(val)
                    });
                }
            }

            return result;
        }
    }
}
