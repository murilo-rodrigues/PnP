﻿using OfficeDevPnP.PowerShell.CmdletHelpAttributes;
using OfficeDevPnP.PowerShell.Commands.Base;
using OfficeDevPnP.PowerShell.Commands.Base.PipeBinds;
using Microsoft.SharePoint.Client;
using System;
using System.Management.Automation;

namespace OfficeDevPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Set, "SPOTaxonomyFieldValue")]
    [CmdletHelp("Sets a taxonomy term value in a listitem field")]
    [CmdletExample(Code = @"
PS:> Set-SPOTaxonomyFieldValue -ListItem $item -InternalFieldName 'Department' -Label 'HR'
    ")]
    [CmdletExample(Code = @"
PS:> Set-SPOTaxonomyFieldValue -ListItem $item -InternalFieldName 'Department' -TermPath 'CORPORATE|DEPARTMENTS|HR'
    ")]
    public class SetTaxonomyFieldValue : SPOCmdlet
    {
        [Parameter(Mandatory = true, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "The list item to set the field value to")]
        public ListItem ListItem;

        [Parameter(Mandatory = true, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "The internal name of the field")]
        public string InternalFieldName;

        [Parameter(Mandatory = true, ParameterSetName = "ITEM", HelpMessage = "The Label value of the term")]
        public string Label;

        [Parameter(Mandatory = true, ParameterSetName = "ITEM", HelpMessage = "The Id of the Term")]
        public GuidPipeBind TermId;

        [Parameter(Mandatory = true, ParameterSetName = "PATH", HelpMessage = "A path in the form of GROUPLABEL|TERMSETLABEL|TERMLABEL")]
        public string TermPath;

        protected override void ExecuteCmdlet()
        {
            Field field = ListItem.ParentList.Fields.GetByInternalNameOrTitle(InternalFieldName);
            ClientContext.Load(field);
            ClientContext.ExecuteQueryRetry();

            switch (ParameterSetName)
            {
                case "ITEM":
                    {
                        ListItem.SetTaxonomyFieldValue(field.Id, Label, TermId.Id);
                        break;
                    }
                case "PATH":
                    {
                        ListItem.SetTaxonomyFieldValueByTermPath(TermPath, field.Id);
                        break;
                    }
                case "ID":
                    {
                        WriteError(new ErrorRecord(new Exception("Not implemented"), "0", ErrorCategory.NotImplemented, null));
                        break;
                    }
            }
        }
    }

}
