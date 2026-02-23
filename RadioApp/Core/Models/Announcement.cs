using System;

namespace RadioApp.Core.Models;

public class Announcement
{
    public string Title { get; set; }
    public string Message { get; set; }
    public DateTime PublishedAt { get; set; }
}