using System;
using System.Collections.Generic;

namespace NihongoLearning.Models;

public partial class Lesson
{
    public int LessonId { get; set; }

    public int TopicId { get; set; }

    public string LessonName { get; set; } = null!;

    public string? Description { get; set; }

    public string? LevelRequired { get; set; }

    public int? XpReward { get; set; }

    public int? GemsReward { get; set; }

    public int OrderIndex { get; set; }

    public int? Duration { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<LessonContent> LessonContents { get; set; } = new List<LessonContent>();

    public virtual Topic Topic { get; set; } = null!;

    public virtual ICollection<UserLessonProgress> UserLessonProgresses { get; set; } = new List<UserLessonProgress>();
}
