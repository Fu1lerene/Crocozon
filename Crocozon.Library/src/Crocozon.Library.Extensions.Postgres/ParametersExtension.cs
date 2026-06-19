using Npgsql;

namespace Crocozon.Library.Extensions.Postgres;

public static class ParametersExtension
{
    public static void AddParameter<T>(this NpgsqlCommand command, T value)
    {
        command.Parameters.Add(new NpgsqlParameter<T> { TypedValue = value });
    }
}