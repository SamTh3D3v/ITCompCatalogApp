//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ITCompCatalogueApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class Category
    {
        public Category()
        {
            this.Cours = new HashSet<Cour>();
        }
    
        public long C_id { get; set; }
        public string Code { get; set; }
        public string Intitule { get; set; }
        public long TechnologieID { get; set; }
    
        public virtual Technology Technology { get; set; }
        public virtual ICollection<Cour> Cours { get; set; }
    }
}
