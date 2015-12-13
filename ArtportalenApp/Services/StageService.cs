using System;
using System.Collections.Generic;
using System.Linq;
using Artportalen.Model;
using ArtportalenApp.Helpers;
using ArtportalenApp.Models;

namespace ArtportalenApp.Services
{
    public class StageService
    {
        public List<Stage> GetAll()
        {
            var result = new List<Stage>();
            var enumValues = Enum.GetValues(typeof (StageEnum));

            foreach (StageEnum val in enumValues)
            {
                if (val != StageEnum.Undefined)
                {
                    result.Add(new Stage
                    {
                        Id = (int)val,
                        Name = CustomAttributeHelper.GetEnumDisplayValue(val),
                    });
                }
            }

            return result.OrderBy(x => x.Name).ToList();
        }
    }
}
