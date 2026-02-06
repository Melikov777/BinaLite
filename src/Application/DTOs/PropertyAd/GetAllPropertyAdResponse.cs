using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.PropertyAd;

public class GetAllPropertyAdResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string? MainImageKey { get; set; }
}
