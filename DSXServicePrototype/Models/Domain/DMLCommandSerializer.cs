using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSXServicePrototype.Models.Domain
{
    class DMLCommandSerializer : ICommandSerializer
    {
        private DSXCommand command;

        public DMLCommandSerializer(DSXCommand command)
        {
            this.command = command;            
        }

        public string Serialize()
        {
            var dataBuilder = new DMLData.DMLDataBuilder(command.IdLocGroupNumber, command.IdUdfFieldNumber, command.IdUdfFieldData)
                .OpenTable("Names")
                .AddField("FName", command.FirstName)
                .AddField("LName", command.LastName)
                .AddField("Company", command.Company)
                .AddField("Visitor", command.IsVisitor)
                .AddField("Trace", command.IsTrace)
                .AddField("Notes", command.NameNotes)
                .WriteToTable();

            if (command.UdfData.Count() > 0)
            {
                foreach (var udf in command.UdfData)
                {
                    dataBuilder
                        .OpenTable("UDF")
                        .AddField("UdfNum", udf.Key)
                        .AddField("UdfText", udf.Value)
                        .WriteToTable();
                }
            }

            if (command.ImageType != null && command.ImageFileName != null)
            {
                dataBuilder.OpenTable("Images")
                    .AddField("ImgType", command.ImageType)
                    .AddField("FileName", command.ImageFileName)
                    .WriteToTable();
            }

            dataBuilder.OpenTable("Cards")
                .AddField("Code", command.Code)
                .AddField("ReplaceCode", command.ReplacementCode)
                .AddField("PIN", command.Pin)
                .AddField("StartDate", command.StartDate)
                .AddField("StopDate", command.StopDate)
                .AddField("CardNum", command.CardNumber)
                .AddField("NumUses", command.NumberOfUses)
                .AddField("GTour", command.IsGuardTour)
                .AddField("APB", command.IsAntiPassBack);

            if (command.IsRevokeAllAccessLevels.HasValue)
            {
                if (command.IsRevokeAllAccessLevels.Value == true)
                    dataBuilder.AddField("ClearAcl", "", true);
            }

            if (command.IsRevokeAllTempAccessLevels.HasValue)
            {
                if (command.IsRevokeAllTempAccessLevels.Value == true)
                    dataBuilder.AddField("ClearTempAcl", "", true);
            }

            foreach (var acl in command.GrantAccessLevels)
            {
                dataBuilder
                    .AddField("AddAcl", acl);
            }

            foreach (var acl in command.GrantTempAccessLevels)
            {
                dataBuilder
                    .AddField("AddTempAcl", acl);
            }

            foreach (var acl in command.RevokeAccessLevels)
            {
                dataBuilder
                    .AddField("DelAcl", acl);
            }

            foreach (var acl in command.RevokeTempAccessLevels)
            {
                dataBuilder
                    .AddField("DelTempAcl", acl);
            }

            dataBuilder
                .AddField("AclStartDate", command.TempAccessStartDate)
                .AddField("AclStopDate", command.TempAccessStopDate)
                .AddField("Loc", command.Location)
                .AddField("OLL", command.OutputLinkingLevel)
                .AddField("Notes", command.CardNotes)
                .WriteToTable();

            var data = dataBuilder.Build();
            return data.WriteData();
        }
    }
}
