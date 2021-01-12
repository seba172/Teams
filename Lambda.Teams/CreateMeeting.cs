using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Domain.Teams;
using Rest.Teams;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Lambda.Teams
{
    public class CreateMeeting
    {
        protected string token;
        public string FunctionHandler(APIGatewayProxyRequest request,ILambdaContext context)
        {
            try
            {
                var body = JsonSerializer.Deserialize<Body>(request.Body);
                if (body == null || body.public_key==null)        
                    return "No se encuentra el public key, lo debe enviar en el body";
                //public_key = SO0I1V_NV5EtIEKIFgFsWf1E9Qv604XCekEK7RD9VfE
                Dictionary<string, string> configuracion = new ConfiguracionService().ObtenerConfiguracion();
                if (configuracion["config"] == "mongo")
                {
                    var result = new MongoService().ValidarPublicKey(body.public_key);
                    if (result != "ok")
                        return result;

                }
                var teamsService = new TeamsService();
                return teamsService.CrearMeeting(body.cliente,body.producto);
            }
            catch (Exception ex)
            {
                LambdaLogger.Log($"Error en CrearMeeting: {context.FunctionName}\n" + " Excepcion: " + ex.Message);
                throw new Exception(Constantes.ExcepcionGenerica);
            }
        }
    }
}
