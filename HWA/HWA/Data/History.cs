using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

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
        public int ColorIndex { get; set; }
        public Color Color 
        {
            get
            {
                return StatusColor(ColorIndex);
            } 
        }
        private Color StatusColor(int index)
        {
            Color color = Color.White;
            if (index == 0) color = Color.FromHex("#B9F6CA");
            if (index == 1) color = Color.FromHex("#FF8A80");
            if (index == 2) color = Color.FromHex("#FF9800");
            if (index == 3) color = Color.FromHex("#BDBDBD");
            if (index == 4) color = Color.FromHex("#FFEE58");
            return color;
        }
    }
}
