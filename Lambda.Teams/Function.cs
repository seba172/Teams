using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Domain.Teams;
using Rest.Teams;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Lambda.Teams
{
    public class Function
    {
        protected string token;
        public string FunctionHandler(ILambdaContext context)
        {
            try
            {
                var teamsService = new TeamsService();
                return teamsService.CrearMeeting();
            }
            catch (Exception ex)
            {
                LambdaLogger.Log($"Error en CrearMeeting: {context.FunctionName}\n" + " Excepcion: " + ex.Message);
                throw new Exception(Constantes.ExcepcionGenerica);
            }
        }
    }
}
