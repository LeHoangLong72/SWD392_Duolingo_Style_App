using System;
using System.Collections.Generic;

namespace NihongoLearning.Models;

public partial class UserLessonProgress
{
    public int ProgressId { get; set; }

    public int UserId { get; set; }

    public int LessonId { get; set; }

    public bool? IsCompleted { get; set; }

    public int? Score { get; set; }

    public int? Stars { get; set; }

    public DateTime? CompletedDate { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Lesson Lesson { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
