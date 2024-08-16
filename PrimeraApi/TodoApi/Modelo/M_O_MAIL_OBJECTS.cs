#nullable disable

namespace TodoApi.Modelo
{
    public class M_O_MAIL_OBJECTS
    {
        public string Usuario{ get; set; }
        public string MailObjectId{ get; set; }
        public string MailClassCd{ get; set; }
        public string SNm{ get; set; }
        public string RNm{ get; set; }
        public string GWgt{ get; set; }
        public string TrDat{ get; set; }
        public string TotCPVal{ get; set; }
        public string SSta{ get; set; }
        public string SCtr{ get; set; }
        public string RAdL1{ get; set; }
        public string RCty{ get; set; }

        public M_O_MAIL_OBJECTS()
        {
            MailObjectId = string.Empty;
        }
    }
}

