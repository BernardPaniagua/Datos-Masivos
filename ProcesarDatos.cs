using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;

namespace DatosMasivos.Functions;

public class ProcesarDatos
{
    private readonly ILogger<ProcesarDatos> _logger;
    private readonly string _connectionString;

    public ProcesarDatos(ILogger<ProcesarDatos> logger, IConfiguration config)
    {
        _logger = logger;
        _connectionString = config["SqlConnection"]
            ?? throw new InvalidOperationException("SqlConnection no configurado en Application Settings.");
    }

    [Function("ProcesarDatos")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
    {
        var aggregatedResults = new List<dynamic>();
        var response = req.CreateResponse();

        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            //Consulta SQL: limpieza, filtrado y agregación
            var query = @"
                SELECT VendorID, COUNT(*) AS Count
                FROM YellowTrips2019
                WHERE VendorID IS NOT NULL 
                  AND trip_distance IS NOT NULL
                  AND trip_distance > 2
                GROUP BY VendorID";

            using var cmd = new SqlCommand(query, connection);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                aggregatedResults.Add(new
                {
                    VendorID = reader["VendorID"].ToString(),
                    Count = (int)reader["Count"]
                });
            }

            response.StatusCode = HttpStatusCode.OK;
            await response.WriteAsJsonAsync(aggregatedResults);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al procesar datos: {ex}");
            response.StatusCode = HttpStatusCode.InternalServerError;
            await response.WriteStringAsync($"Error al procesar datos: {ex}");
        }

        return response;
    }
}
