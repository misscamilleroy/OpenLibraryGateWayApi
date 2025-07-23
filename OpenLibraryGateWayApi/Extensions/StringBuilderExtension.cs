using System.Text;
using OpenLibraryGateWayApi.Enums;

namespace OpenLibraryGateWayApi.Extensions;

public static class StringBuilderExtensions
{
    /// <summary>
    /// AddQueryString will begin  the querystringparameters by prepending the "?"
    /// </summary>
    /// <param name="sb">this StringBuilder/param>
    /// <param name="parameterName">Querystring parameter name, do not include the equals sign</param>
    /// <param name="parameterValue">The parameter value object will be converted to a string</param>
    public static void AddQueryString(this StringBuilder sb, UriParameterName parameterName, object parameterValue)
    {
        sb.Append('?').Append(parameterName.ToString()).Append('=').Append(Convert.ToString(parameterValue));
    }

    /// <summary>
    /// AppendQueryString will continue the querystringparameters by prepending the "&"
    /// </summary>
    /// <param name="sb">this StringBuilder /param>
    /// <param name="parameterName">Querystring parameter name, do not include the equals sign</param>
    /// <param name="parameterValue">The parameter value object will be converted to a string</param>
    public static void AppendQueryString(this StringBuilder sb, UriParameterName parameterName, object parameterValue)
    {
        sb.Append('&').Append(parameterName.ToString()).Append('=').Append(Convert.ToString(parameterValue));
    }

}