using System.Text.Json;

namespace MiTienda.Utilities
{
    public static class SessionExtensions
    {
        public static void SetCarrito<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T GetCarrito<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonSerializer.Deserialize<T>(value);
        }
    }
}
