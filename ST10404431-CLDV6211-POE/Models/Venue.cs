#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ST10404431CLDV6211POE.Models;

public partial class Venue
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string VenueID { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string VenueName { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string Location { get; set; }

    public int Capacity { get; set; }

    [Required]
    [StringLength(500)]
    [Unicode(false)]
    public string ImageURL { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Category { get; set; }

    [InverseProperty("Venue")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    [InverseProperty("Venue")]
    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}