using System;
using System.Collections.Generic;

namespace NihongoLearning.Models;

public partial class Topic
{
    public int TopicId { get; set; }

    public string TopicName { get; set; } = null!;

    public string? Description { get; set; }

    public string? IconUrl { get; set; }

    public int OrderIndex { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}
