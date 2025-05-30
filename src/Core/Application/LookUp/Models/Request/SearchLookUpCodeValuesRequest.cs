using Microsoft.Teams.Assist.Application.Common.Models;

namespace Microsoft.Teams.Assist.Application.LookUp.Models.Request;
public class SearchLookUpCodeValuesRequest : SearchRequestBaseClass
{
    public LookUpCodeTypes Type { get; set; }
}
