using System;
using System.Collections.Generic;

namespace NihongoLearning.Models;

public partial class LessonContent
{
    public int ContentId { get; set; }

    public int LessonId { get; set; }

    public int AlphabetId { get; set; }

    public int OrderIndex { get; set; }

    public virtual Alphabet Alphabet { get; set; } = null!;

    public virtual Lesson Lesson { get; set; } = null!;
}
