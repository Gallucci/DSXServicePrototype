using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.Domain
{
    public class DSXRequest : IRequest
    {        
        public int IdLocGroupNumber { get; set; }
        public int IdUdfFieldNumber { get; set; }
        public string IdUdfFieldData { get; set; }

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

        private DSXRequest(RequestBuilder builder)
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

        public string WriteRequest(SerializerFormat format)
        {
            return RequestSerializerFactory.GetCommandSerializer(this, format).Serialize(); 
        }
        
        internal sealed class RequestBuilder
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

            public RequestBuilder(string firstName, string lastName, string company, long code, int locGroupNumber, int udfFieldNumber, string udfFieldData)
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

            public RequestBuilder IsAVisitor()
            {
                IsVisitor = true;
                return this;
            }

            public RequestBuilder IsTraced()
            {
                IsTrace = true;
                return this;
            }

            public RequestBuilder WriteNoteForPerson(string note)
            {
                NameNotes = note;
                return this;
            }

            public RequestBuilder AddUdfData(int udfNum, string udfData)
            {
                UdfData.Add(new KeyValuePair<int, string>(udfNum, udfData));
                return this;
            }
            public RequestBuilder AddUdfData(IDictionary<int, string> dataSet)
            {
                foreach(var data in dataSet)
                {
                    AddUdfData(data.Key, data.Value);
                }
                return this;
            }

            public RequestBuilder SetImage(int imageType, string fileName)
            {
                ImageType = imageType;
                ImageFileName = fileName;
                return this;
            }

            public RequestBuilder SetPin(long pin)
            {
                Pin = pin;
                return this;
            }

            public RequestBuilder AccessBeginsOn(DateTime startDate)
            {
                StartDate = startDate;
                return this;
            }

            public RequestBuilder AccessStopsOn(DateTime stopDate)
            {
                StopDate = stopDate;
                return this;
            }

            public RequestBuilder SetCardNumber(string number)
            {
                CardNumber = number;
                return this;
            }

            public RequestBuilder SetMaximumUses(int number)
            {
                NumberOfUses = number;
                return this;
            }

            public RequestBuilder IsAGuardTourCard()
            {
                IsGuardTour = true;
                return this;
            }

            public RequestBuilder OverridesAntiPassBack()
            {
                IsAntiPassBack = true;
                return this;
            }

            public RequestBuilder GrantAccessLevel(string aclName)
            {
                if (!string.IsNullOrEmpty(aclName))
                    GrantAccessLevels.Add(aclName);    
                
                return this;
            }
            public RequestBuilder GrantAccessLevel(IEnumerable<string> aclSet)
            {
                foreach (var aclName in aclSet)
                {
                    GrantAccessLevel(aclName);
                }
                return this;
            }

            public RequestBuilder GrantTempAccessLevel(string aclName)
            {
                if (!string.IsNullOrEmpty(aclName))
                    GrantTempAccessLevels.Add(aclName);                
                
                return this;
            }
            public RequestBuilder GrantTempAccessLevel(IEnumerable<string> aclSet)
            {
                foreach (var aclName in aclSet)
                {
                    GrantTempAccessLevel(aclName);
                }
                return this;
            }

            public RequestBuilder RevokeAccessLevel(string aclName)
            {
                if (!string.IsNullOrEmpty(aclName))
                    RevokeAccessLevels.Add(aclName);

                return this;
            }
            public RequestBuilder RevokeAccessLevel(IEnumerable<string> aclSet)
            {
                foreach (var aclName in aclSet)
                {
                    RevokeAccessLevel(aclName);
                }
                return this;
            }

            public RequestBuilder RevokeTempAccessLevel(string aclName)
            {
                if (!string.IsNullOrEmpty(aclName))
                    GrantTempAccessLevels.Add(aclName);

                return this;
            }
            public RequestBuilder RevokeTempAccessLevel(IEnumerable<string> aclSet)
            {
                foreach (var aclName in aclSet)
                {
                    RevokeTempAccessLevel(aclName);
                }
                return this;
            }

            public RequestBuilder TempAccessBeginsOn(DateTime startDate)
            {
                TempAccessStartDate = startDate;
                return this;
            }

            public RequestBuilder TempAccessStopsOn(DateTime stopDate)
            {
                TempAccessStopDate = stopDate;
                return this;
            }

            public RequestBuilder SetLocationNumber(int number)
            {
                Location = number;
                return this;
            }

            public RequestBuilder SetOutputLinkingLevel(int number)
            {
                OutputLinkingLevel = number;
                return this;
            }

            public RequestBuilder WriteCardNote(string note)
            {
                CardNotes = note;
                return this;
            }

            public RequestBuilder RevokeAllAccessLevels()
            {
                IsRevokeAllAccessLevels = true;
                return this;
            }

            public RequestBuilder RevokeAllTempAccessLevels()
            {
                IsRevokeAllTempAccessLevels = true;
                return this;
            }

            public RequestBuilder ReplaceCode(double code)
            {
                ReplacementCode = code;
                return this;
            }

            public IRequest Build()
            {
                return new DSXRequest(this);
            }
        }
    }
}
