﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ExamEntities : DbContext
    {
        public ExamEntities()
            : base("name=ExamEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<A_Rules> A_Rules { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Chapter> Chapters { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Level> Levels { get; set; }
        public virtual DbSet<MCQquestion> MCQquestions { get; set; }
        public virtual DbSet<P_Rules> P_Rules { get; set; }
        public virtual DbSet<Professor> Professors { get; set; }
        public virtual DbSet<Result> Results { get; set; }
        public virtual DbSet<Rule> Rules { get; set; }
        public virtual DbSet<ST_Rules> ST_Rules { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<TFquestion> TFquestions { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<ExamQuestion> ExamQuestions { get; set; }
        public virtual DbSet<E_Structures> E_Structures { get; set; }
    }
}
