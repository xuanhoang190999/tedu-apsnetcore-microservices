using Contracts.Common.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Infrastructure.Common
{
    public class SerializeService : ISerializeService
    {
        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                Converters = new List<JsonConverter>
                {
                    new StringEnumConverter // Nếu kiểu chuỗi (string) enum thì sẽ trả theo định dạng nào.
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    }
                }
            });
        }

        public string Serialize<T>(T obj, Type type)
        {
            return JsonConvert.SerializeObject(obj, type, new JsonSerializerSettings()); // Nhận vào kiểu T, parse ra kiểu type được nhận vào.
        }

        public T Deserialize<T>(string text)
        {
            return JsonConvert.DeserializeObject<T>(text);
        }
    }
}
