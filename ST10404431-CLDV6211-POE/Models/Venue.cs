#nullable disable
using System;
using System.Collections.Generic;

namespace ST10404431CLDV6211POE.Models;

public partial class Venue
{
    public string VenueID { get; set; }

    public string VenueName { get; set; }

    public string Location { get; set; }

    public int Capacity { get; set; }

    public string ImageURL { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}