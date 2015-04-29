using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.Domain
{
    public class DSXCommand : ICommand
    {
        private int IdLocGroupNumber { get; set; }
        private int IdUdfFieldNumber { get; set; }
        private string IdUdfFieldData { get; set; }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Company { get; private set; }
        public bool? IsVisitor { get; private set; }
        public bool? IsTrace { get; private set; }
        public string NameNotes { get; private set; }

        public IDictionary<int, string> UdfData { get; private set; }

        public int? ImageType { get; private set; }
        public string ImageFileName { get; private set; }

        public double? Code { get; private set; }
        public long? Pin { get; private set; }
        public DateTime? StartDate { get; private set; }
        public DateTime? StopDate { get; private set; }
        public string CardNumber { get; private set; }
        public long? NumberOfUses { get; private set; }
        public bool? IsGuardTour { get; private set; }
        public bool? IsAntiPassBack { get; private set; }
        public IList<string> GrantAccessLevels { get; private set; }
        public IList<string> GrantTempAccessLevels { get; private set; }
        public IList<string> RevokeAccessLevels { get; private set; }
        public IList<string> RevokeTempAccessLevels { get; private set; }
        public DateTime? TempAccessStartDate { get; private set; }
        public DateTime? TempAccessStopDate { get; private set; }
        public int? Location { get; private set; }
        public int? OutputLinkingLevel { get; private set; }
        public string CardNotes { get; private set; }

        public double? ReplacementCode { get; private set; }
        public bool? IsRevokeAllAccessLevels { get; set; }
        public bool? IsRevokeAllTempAccessLevels { get; set; }

        private DSXCommand(CommandBuilder builder)
        {
            IdLocGroupNumber = builder.IdLocGroupNumber;
            IdUdfFieldNumber = builder.IdUdfFieldNumber;
            IdUdfFieldData = builder.IdUdfFieldData;

            FirstName = builder.FirstName;
            LastName = builder.LastName;
            Company = builder.Company;
            IsVisitor = builder.IsVisitor;
            IsTrace = builder.IsTrace;
            NameNotes = builder.NameNotes;
            
            UdfData = builder.UdfData;

            Code = builder.Code;
            Pin = builder.Pin;
            StartDate = builder.StartDate;
            StopDate = builder.StopDate;
            CardNumber = builder.CardNumber;
            NumberOfUses = builder.NumberOfUses;
            IsGuardTour = builder.IsGuardTour;
            IsAntiPassBack = builder.IsAntiPassBack;
            GrantAccessLevels = builder.GrantAccessLevels;
            GrantTempAccessLevels = builder.GrantTempAccessLevels;
            RevokeAccessLevels = builder.RevokeAccessLevels;
            RevokeTempAccessLevels = builder.RevokeTempAccessLevels;
            Location = builder.Location;
            OutputLinkingLevel = builder.OutputLinkingLevel;
            CardNotes = builder.CardNotes;

            ReplacementCode = builder.ReplacementCode;
            IsRevokeAllAccessLevels = builder.IsRevokeAllAccessLevels;
            IsRevokeAllTempAccessLevels = builder.IsRevokeAllTempAccessLevels;
        }                

        public string WriteCommand()
        {
            var dataBuilder = new DSXData.DSXDataBuilder(IdLocGroupNumber, IdUdfFieldNumber, IdUdfFieldData)                
                .OpenTable("Names")
                .AddField("FName", FirstName)
                .AddField("LName", LastName)
                .AddField("Company", Company)
                .AddField("Visitor", IsVisitor)
                .AddField("Trace", IsTrace)
                .AddField("Notes", NameNotes)
                .WriteToTable();
            
            if (UdfData.Count() > 0)
            {                
                foreach (var udf in UdfData)
                {
                    dataBuilder
                        .OpenTable("UDF")
                        .AddField("UdfNum", udf.Key)
                        .AddField("UdfText", udf.Value)
                        .WriteToTable();
                }                                    
            }

            if(ImageType != null && ImageFileName != null)
            {
                dataBuilder.OpenTable("Images")
                    .AddField("ImgType", ImageType)
                    .AddField("FileName", ImageFileName)
                    .WriteToTable();
            }

            dataBuilder.OpenTable("Cards")
                .AddField("Code", Code)
                .AddField("ReplaceCode", ReplacementCode)
                .AddField("PIN", Pin)
                .AddField("StartDate", StartDate)
                .AddField("StopDate", StopDate)
                .AddField("CardNum", CardNumber)
                .AddField("NumUses", NumberOfUses)
                .AddField("GTour", IsGuardTour)
                .AddField("APB", IsAntiPassBack);

            if (IsRevokeAllAccessLevels.HasValue)
            {
                if(IsRevokeAllAccessLevels.Value == true)
                    dataBuilder.AddField("ClearAcl", "", true);
            }

            if(IsRevokeAllTempAccessLevels.HasValue)
            {
                if (IsRevokeAllTempAccessLevels.Value == true)
                    dataBuilder.AddField("ClearTempAcl", "", true);
            }

            foreach(var acl in GrantAccessLevels)
            {
                dataBuilder
                    .AddField("AddAcl", acl);
            }

            foreach(var acl in GrantTempAccessLevels)
            {
                dataBuilder
                    .AddField("AddTempAcl", acl);
            }

            foreach (var acl in RevokeAccessLevels)
            {
                dataBuilder
                    .AddField("DelAcl", acl);
            }

            foreach (var acl in RevokeTempAccessLevels)
            {
                dataBuilder
                    .AddField("DelTempAcl", acl);
            }

            dataBuilder   
                .AddField("AclStartDate", TempAccessStartDate)
                .AddField("AclStopDate", TempAccessStopDate)
                .AddField("Loc", Location)
                .AddField("OLL", OutputLinkingLevel)
                .AddField("Notes", CardNotes)
                .WriteToTable();

            // Build the product
            var data = dataBuilder.Build();
            return data.WriteData();
        }
        
        internal sealed class CommandBuilder
        {
            public int IdLocGroupNumber { get; private set; }
            public int IdUdfFieldNumber { get; private set; }
            public string IdUdfFieldData { get; private set; }

            public string FirstName { get; private set; }
            public string LastName { get; private set; }
            public string Company { get; private set; }
            public bool? IsVisitor { get; private set; }
            public bool? IsTrace { get; private set; }
            public string NameNotes { get; private set; }

            public IDictionary<int, string> UdfData { get; private set; }

            public int? ImageType { get; private set; }
            public string ImageFileName { get; private set; }

            public double? Code { get; private set; }
            public long? Pin { get; private set; }
            public DateTime? StartDate { get; private set; }
            public DateTime? StopDate { get; private set; }
            public string CardNumber { get; private set; }
            public long? NumberOfUses { get; private set; }
            public bool? IsGuardTour { get; private set; }
            public bool? IsAntiPassBack { get; private set; }
            public IList<string> GrantAccessLevels { get; private set; }
            public IList<string> GrantTempAccessLevels { get; private set; }
            public IList<string> RevokeAccessLevels { get; private set; }
            public IList<string> RevokeTempAccessLevels { get; private set; }
            public DateTime? TempAccessStartDate { get; private set; }
            public DateTime? TempAccessStopDate { get; private set; }
            public int? Location { get; private set; }
            public int? OutputLinkingLevel { get; private set; }
            public string CardNotes { get; private set; }

            public double? ReplacementCode { get; private set; }
            public bool? IsRevokeAllAccessLevels { get; set; }
            public bool? IsRevokeAllTempAccessLevels { get; set; }

            public CommandBuilder(string firstName, string lastName, string company, long code, int locGroupNumber, int udfFieldNumber, string udfFieldData)
            {
                // Identification data
                IdLocGroupNumber = locGroupNumber;
                IdUdfFieldNumber = udfFieldNumber;
                IdUdfFieldData = udfFieldData;

                // Names table data
                FirstName = firstName;
                LastName = lastName;
                Company = company;

                // Cards table data
                Code = code;

                UdfData = new Dictionary<int, string>();
                GrantAccessLevels = new List<string>();
                GrantTempAccessLevels = new List<string>();        
                RevokeAccessLevels = new List<string>();
                RevokeTempAccessLevels = new List<string>();
            }

            public CommandBuilder IsAVisitor()
            {
                IsVisitor = true;
                return this;
            }

            public CommandBuilder IsTraced()
            {
                IsTrace = true;
                return this;
            }

            public CommandBuilder WriteNoteForPerson(string note)
            {
                NameNotes = note;
                return this;
            }

            public CommandBuilder AddUdfData(int udfNum, string udfData)
            {
                UdfData.Add(new KeyValuePair<int, string>(udfNum, udfData));
                return this;
            }
            public CommandBuilder AddUdfData(IDictionary<int, string> dataSet)
            {
                foreach(var data in dataSet)
                {
                    AddUdfData(data.Key, data.Value);
                }
                return this;
            }

            public CommandBuilder SetImage(int imageType, string fileName)
            {
                ImageType = imageType;
                ImageFileName = fileName;
                return this;
            }

            public CommandBuilder SetPin(int pin)
            {
                Pin = pin;
                return this;
            }

            public CommandBuilder AccessBeginsOn(DateTime startDate)
            {
                StartDate = startDate;
                return this;
            }

            public CommandBuilder AccessStopsOn(DateTime stopDate)
            {
                StopDate = stopDate;
                return this;
            }

            public CommandBuilder SetCardNumber(string number)
            {
                CardNumber = number;
                return this;
            }

            public CommandBuilder SetMaximumUses(int number)
            {
                NumberOfUses = number;
                return this;
            }

            public CommandBuilder IsAGuardTourCard()
            {
                IsGuardTour = true;
                return this;
            }

            public CommandBuilder OverridesAntiPassBack()
            {
                IsAntiPassBack = true;
                return this;
            }

            public CommandBuilder GrantAccessLevel(string aclName)
            {
                if (!string.IsNullOrEmpty(aclName))
                    GrantAccessLevels.Add(aclName);    
                
                return this;
            }
            public CommandBuilder GrantAccessLevel(IEnumerable<string> aclSet)
            {
                foreach (var aclName in aclSet)
                {
                    GrantAccessLevel(aclName);
                }
                return this;
            }

            public CommandBuilder GrantTempAccessLevel(string aclName)
            {
                if (!string.IsNullOrEmpty(aclName))
                    GrantTempAccessLevels.Add(aclName);                
                
                return this;
            }
            public CommandBuilder GrantTempAccessLevel(IEnumerable<string> aclSet)
            {
                foreach (var aclName in aclSet)
                {
                    GrantTempAccessLevel(aclName);
                }
                return this;
            }

            public CommandBuilder RevokeAccessLevel(string aclName)
            {
                if (!string.IsNullOrEmpty(aclName))
                    RevokeAccessLevels.Add(aclName);

                return this;
            }
            public CommandBuilder RevokeAccessLevel(IEnumerable<string> aclSet)
            {
                foreach (var aclName in aclSet)
                {
                    RevokeAccessLevel(aclName);
                }
                return this;
            }

            public CommandBuilder RevokeTempAccessLevel(string aclName)
            {
                if (!string.IsNullOrEmpty(aclName))
                    GrantTempAccessLevels.Add(aclName);

                return this;
            }
            public CommandBuilder RevokeTempAccessLevel(IEnumerable<string> aclSet)
            {
                foreach (var aclName in aclSet)
                {
                    RevokeTempAccessLevel(aclName);
                }
                return this;
            }

            public CommandBuilder TemporaryAccessBeginsOn(DateTime startDate)
            {
                TempAccessStartDate = startDate;
                return this;
            }

            public CommandBuilder TemporaryAccessStopsOn(DateTime stopDate)
            {
                TempAccessStopDate = stopDate;
                return this;
            }

            public CommandBuilder SetLocationNumber(int number)
            {
                Location = number;
                return this;
            }

            public CommandBuilder SetOutputLinkingLevel(int number)
            {
                OutputLinkingLevel = number;
                return this;
            }

            public CommandBuilder WriteCardNote(string note)
            {
                CardNotes = note;
                return this;
            }

            public CommandBuilder RevokeAllAccessLevels()
            {
                IsRevokeAllAccessLevels = true;
                return this;
            }

            public CommandBuilder RevokeAllTempAccessLevels()
            {
                IsRevokeAllTempAccessLevels = true;
                return this;
            }

            public CommandBuilder ReplaceCode(double code)
            {
                ReplacementCode = code;
                return this;
            }

            public ICommand Build()
            {
                return new DSXCommand(this);
            }
        }
    }
}
