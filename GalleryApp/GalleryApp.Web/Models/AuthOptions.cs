using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GalleryApp.Web.Models
{
    public class AuthOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secret { get; set; }
        public int Lifetime { get; set; }
    }
}
