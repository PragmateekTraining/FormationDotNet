using System.Data;

namespace SamplesAPI
{
    public static class DatabaseExtensions
    {
        public static int AddParameter(this IDbCommand command, string name, object value)
        {
            IDbDataParameter parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;

            return command.Parameters.Add(parameter);
        }
    }
}
