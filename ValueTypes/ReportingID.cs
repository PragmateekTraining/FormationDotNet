namespace ValueTypesSamples
{
    struct ReportID
    {
        public string UserName { get; private set; }
        public int Folio { get; private set; }

        public ReportID(string userName, int folio)
            : this()
        {
            UserName = userName;
            Folio = folio;
        }
    }
}
