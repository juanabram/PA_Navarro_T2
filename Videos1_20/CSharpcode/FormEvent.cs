using System;

namespace CSharpCode {
    public class FormEvent : EventArgs {
        private string name;
        private string occupation;
        private int ageCategory;
        private string empCat;
        private string taxId;
        private bool usCitizen;

        public FormEvent(object source, string name, string occupation, int ageCat, string empCat, string taxId, bool usCitizen) {
            this.name = name;
            this.occupation = occupation;
            this.ageCategory = ageCat;
            this.empCat = empCat;
            this.taxId = taxId;
            this.usCitizen = usCitizen;
        }

        public string getName() { return name; }
        public string getOccupation() { return occupation; }
        public int getAgeCategory() { return ageCategory; }
        public string getEmploymentCategory() { return empCat; }
        public string getTaxId() { return taxId; }
        public bool isUsCitizen() { return usCitizen; }
    }
}