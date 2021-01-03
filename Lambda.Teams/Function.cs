using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Rest.Teams;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Lambda.Teams
{
    public class Function
    {
        public string FunctionHandler(ILambdaContext context)
        {
            var tokenService = new TokenService();
            var token = tokenService.obtenerToken();

            var teamsService = new TeamsService();
            var urlMeeting = teamsService.crearMeeting(token);
            
            return urlMeeting;
        }
    }
}
