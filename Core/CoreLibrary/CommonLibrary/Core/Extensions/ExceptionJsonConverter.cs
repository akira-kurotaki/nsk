using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CoreLibrary.Core.Extensions
{
    /// <summary>
    /// ExceptionのプロパティをJSONに書き込む。
    /// 読み込みには対応していない。取り出す際は、System.Dynamic.ExpandoObjectとして取得する。
    /// </summary>
    public class ExceptionJsonConverter : JsonConverter<Exception>
    {
        /// <summary>
        /// 未実装。NotImplementedExceptionをスローする。
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Exception? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// ExceptionのプロパティをJSONに書き込む。
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, Exception value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            var exceptionType = value.GetType();
            writer.WriteString("ClassName", exceptionType.FullName);
            var properties = exceptionType.GetProperties()
                .Where(e => e.PropertyType != typeof(Type))
                .Where(e => e.PropertyType.Namespace != typeof(MemberInfo).Namespace)
                .ToList();
            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(value, null);
                if (options.DefaultIgnoreCondition == JsonIgnoreCondition.WhenWritingNull && propertyValue == null)
                {
                    continue;
                }
                writer.WritePropertyName(property.Name);
                JsonSerializer.Serialize(writer, propertyValue, property.PropertyType, options);
            }
            writer.WriteEndObject();
        }
    }
}
