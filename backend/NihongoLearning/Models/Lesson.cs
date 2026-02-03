using System;
using System.Collections.Generic;

namespace NihongoLearning.Models;

public class Lesson
{
    public int LessonId { get; set; }
    public int NodeId { get; set; }
    public string Title { get; set; }
    public int BaseXP { get; set; }
}
