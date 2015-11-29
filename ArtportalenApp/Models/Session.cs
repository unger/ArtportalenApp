using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtportalenApp.Models
{
    public class Session
    {
        public string Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }

        public string CreatedAtString
        {
            get
            {
                if (CreatedAt.HasValue)
                {
                    return CreatedAt.Value.ToString("yyyy-MM-dd HH:mm");
                }
                return string.Empty;
            }
        }

        public string ExpiresAtString
        {
            get
            {
                if (ExpiresAt.HasValue)
                {
                    return ExpiresAt.Value.ToString("yyyy-MM-dd HH:mm");
                }
                return string.Empty;
            }
        }

        public string DeviceInfo { get; set; }
    }
}
