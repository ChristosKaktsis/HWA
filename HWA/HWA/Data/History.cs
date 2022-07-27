using System;
using System.Collections.Generic;
using System.Text;

namespace HWA.Data
{
    public class History
    {
        /*
         * this is what the webservice will return
           {
              "ClaimDate":"2020-09-01T14:40:55.453",
              "AppointmentDate":"2020-09-02T21:40:55.447",
              "Status":"Closed",
              "Provider":"EUROMEDICA ΔΥΤΙΚΗΣ ΘΕΣΣΑΛΟΝΙΚΗΣ",
              "ExamGroup":"CU5 - CHECK UP ΑΝΔΡΩΝ - ΑΝΕΞΑΡΤΗΤΩΣ ΗΛΙΚΙΑΣ -NEW",
              "ExtraInfo":"ΕΔΩ ΘΑ ΜΠΟΥ ΠΟΛΛΕΣ ΠΛΗΡΟΦΟΡΙΕΣ ΚΑΙ ΘΕΛΥΟΜΕ ΤΟΥΛΑΧΙΣΤΟΝ 2 - 3 ΓΡΑΜΜΕΣ ΓΙΑ ΝΑ ΤΙΣ ΒΛΕΠΟΥΜΕ"
           }
        */
        public DateTime ClaimDate { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; }
        public string Provider { get; set; }
        public string ExamGroup { get; set; }
        public string ExtraInfo { get; set; }
    }
}
