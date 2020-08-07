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
    using System.Web;

    public partial class Student
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Student()
        {
            this.Results = new HashSet<Result>();
            this.ST_Rules = new HashSet<ST_Rules>();
        }
    
        public int ST_id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string passwod { get; set; }
        public string mobile { get; set; }
        public string N_N { get; set; }
        public string rule { get; set; }
        public bool approval { get; set; }
        public int L_id { get; set; }
        public int Dep_id { get; set; }
        public double total { get; set; }
        public string ST_image { get; set; }
        public HttpPostedFileBase user_image_data { get; set; }
        public virtual Department Department { get; set; }
        public virtual Level Level { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Result> Results { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ST_Rules> ST_Rules { get; set; }
    }
}
