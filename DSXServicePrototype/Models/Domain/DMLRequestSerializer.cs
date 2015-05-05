using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.Domain
{
    class DMLRequestSerializer : IRequestSerializer
    {
        private DSXRequest request;

        public DMLRequestSerializer(DSXRequest request)
        {
            this.request = request;            
        }

        public string Serialize()
        {
            var dataBuilder = new DMLRequestData.DataBuilder(request.IdLocGroupNumber, request.IdUdfFieldNumber, request.IdUdfFieldData)
                .OpenTable("Names")
                .AddField("FName", request.FirstName)
                .AddField("LName", request.LastName)
                .AddField("Company", request.Company)
                .AddField("Visitor", request.IsVisitor)
                .AddField("Trace", request.IsTrace)
                .AddField("Notes", request.NameNotes)
                .CloseTableWithWrite();

            if (request.UdfData.Count() > 0)
            {
                foreach (var udf in request.UdfData)
                {
                    dataBuilder
                        .OpenTable("UDF")
                        .AddField("UdfNum", udf.Key)
                        .AddField("UdfText", udf.Value)
                        .CloseTableWithWrite();
                }
            }

            if (request.ImageType != null && request.ImageFileName != null)
            {
                dataBuilder.OpenTable("Images")
                    .AddField("ImgType", request.ImageType)
                    .AddField("FileName", request.ImageFileName)
                    .CloseTableWithWrite();
            }

            dataBuilder.OpenTable("Cards")
                .AddField("Code", request.Code)
                .AddField("ReplaceCode", request.ReplacementCode)
                .AddField("PIN", request.Pin)
                .AddField("StartDate", request.StartDate)
                .AddField("StopDate", request.StopDate)
                .AddField("CardNum", request.CardNumber)
                .AddField("NumUses", request.NumberOfUses)
                .AddField("GTour", request.IsGuardTour)
                .AddField("APB", request.IsAntiPassBack);

            if (request.IsRevokeAllAccessLevels.HasValue)
            {
                if (request.IsRevokeAllAccessLevels.Value == true)
                    dataBuilder.AddField("ClearAcl", "", true);
            }

            if (request.IsRevokeAllTempAccessLevels.HasValue)
            {
                if (request.IsRevokeAllTempAccessLevels.Value == true)
                    dataBuilder.AddField("ClearTempAcl", "", true);
            }

            foreach (var acl in request.GrantAccessLevels)
            {
                dataBuilder
                    .AddField("AddAcl", acl);
            }

            foreach (var acl in request.GrantTempAccessLevels)
            {
                dataBuilder
                    .AddField("AddTempAcl", acl);
            }

            foreach (var acl in request.RevokeAccessLevels)
            {
                dataBuilder
                    .AddField("DelAcl", acl);
            }

            foreach (var acl in request.RevokeTempAccessLevels)
            {
                dataBuilder
                    .AddField("DelTempAcl", acl);
            }

            dataBuilder
                .AddField("AclStartDate", request.TempAccessStartDate)
                .AddField("AclStopDate", request.TempAccessStopDate)
                .AddField("Loc", request.Location)
                .AddField("OLL", request.OutputLinkingLevel)
                .AddField("Notes", request.CardNotes)
                .CloseTableWithWrite();

            var data = dataBuilder.Build();
            return data.WriteData();
        }
    }
}
