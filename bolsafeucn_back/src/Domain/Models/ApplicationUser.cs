using bolsafeucn_back.src.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace bolsafeucn.src.Domain.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public required GeneralUser GeneralUser { get; set; }
    }
}
