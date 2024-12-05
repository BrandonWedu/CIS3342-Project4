using System.Runtime.Serialization.Formatters.Binary;

namespace Project4.Models
{
    public class Serializer
    {
        public static byte[] SerializeData<T>(T data)
        {
            byte[] serializedData;
            using (MemoryStream stream = new MemoryStream())
            {
                // Disable the warning.
#pragma warning disable SYSLIB0011
                // Code that uses obsolete API.
                BinaryFormatter formatter = new BinaryFormatter();
                // Re-enable the warning.
#pragma warning restore SYSLIB0011
                formatter.Serialize(stream, data);
                serializedData = stream.ToArray();
            }
            return serializedData;
        }

        public static T DeserializeData<T>(byte[] byteArray)
        {
            T deserializedObject;
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                // Disable the warning.
#pragma warning disable SYSLIB0011
                // Code that uses obsolete API.
                BinaryFormatter formatter = new BinaryFormatter();
                // Re-enable the warning.
#pragma warning restore SYSLIB0011
                deserializedObject = (T)formatter.Deserialize(stream);
            }
            return deserializedObject;
        }
    }
}
