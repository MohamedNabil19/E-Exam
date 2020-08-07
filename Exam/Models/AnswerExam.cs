using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exam.Models
{
    public class AnswerExam
    {
      public  string Answer { get; set; }
       public MCQquestion McqQuestion { get; set; }
       public TFquestion TFQuestion { get; set; }
    }
}