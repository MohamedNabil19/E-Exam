//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Exam.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class A_Rules
    {
        public int id { get; set; }
        public int A_id { get; set; }
        public int R_id { get; set; }
    
        public virtual Admin Admin { get; set; }
        public virtual Rule Rule { get; set; }
    }
}