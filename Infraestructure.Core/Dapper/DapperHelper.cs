using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Core.Dapper
{
    public class DapperHelper
    {
        #region Singleton

        /// <summary>
        /// Atributo o proiedad para acceder a los metodos publicos cuando la clase esta instanciada.
        /// </summary>
        /// <value>The instancia.</value>
        public static DapperHelper Instancia
        {
            get
            {
                lock (Padlock)
                {
                    return _instancia ?? (_instancia = new DapperHelper());
                }
            }
        }

        #endregion Singleton

        #region Propiedades Privadas y Públicas
        // en segundos
        private readonly int bdTimeout = 2700;

        /// <summary>
        /// Propiedad estatica que almacena la instancia del objecto.
        /// </summary>
        private static DapperHelper _instancia;

        /// <summary>
        /// Atributo para validar el estado para instanciar el objeto
        /// </summary>
        private static readonly object Padlock = new object();

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString { get; set; }

        #endregion Propiedades Privadas y Públicas

        #region Métodos Públicos

        /// <summary>
        /// Ejecutar sentencia sql
        /// </summary>
        /// <typeparam name="T">Entidad para realizar el Mapeo</typeparam>
        /// <param name="query">Quesry SQL a ejecutar.</param>
        /// <param name="filter">Condición que se debe mapear a la sentencia sql server.</param>
        /// <returns>Task&lt;IEnumerable&lt;T&gt;&gt;.</returns>
        /// <exception cref="Exception">Manejo de errores</exception>
        public async Task<IEnumerable<T>> ExecuteQuerySelect<T>(string query, object filter = null)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                var result = filter == null
                    ? await conn.QueryAsync<T>(query, commandTimeout: bdTimeout).ConfigureAwait(false)
                    : await conn.QueryAsync<T>(query, filter, commandTimeout: bdTimeout).ConfigureAwait(false);
                conn.Close();
                conn.Dispose();
                return result;
            }
        }

        /// <summary>
        /// Executes the store procedure.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storeProcedure">The store procedure.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>Task&lt;IEnumerable&lt;T&gt;&gt;.</returns>
        public async Task<IEnumerable<T>> ExecuteStoreProcedure<T>(string storeProcedure, object filter = null) where T : class
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                var result = filter == null
                    ? await conn.QueryAsync<T>(storeProcedure, commandType: CommandType.StoredProcedure, commandTimeout: bdTimeout).ConfigureAwait(false)
                    : await conn.QueryAsync<T>(storeProcedure, filter, commandType: CommandType.StoredProcedure, commandTimeout: bdTimeout).ConfigureAwait(false);
                conn.Close();
                conn.Dispose();
                return result;
            }
        }

        /// <summary>
        /// Executes the query scalar.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>System.Int32.</returns>
        public int ExecuteQueryScalar(string query, object filter = null)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                var result = conn.ExecuteScalar<int>(query, filter, commandTimeout: bdTimeout);
                conn.Close();
                conn.Dispose();
                return result;
            }
        }

        /// <summary>
        /// Executes the store procedure scalar.
        /// </summary>
        /// <param name="storeProcedure">The store procedure.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>System.Int32.</returns>
        public int ExecuteStoreProcedureScalar(string storeProcedure, object filter = null)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                var result = filter == null
                    ? conn.ExecuteScalar<int>(storeProcedure, commandType: CommandType.StoredProcedure, commandTimeout: bdTimeout)
                    : conn.ExecuteScalar<int>(storeProcedure, filter, commandType: CommandType.StoredProcedure, commandTimeout: bdTimeout);
                conn.Close();
                conn.Dispose();
                return result;
            }
        }

        #endregion Métodos Públicos
    }

}
