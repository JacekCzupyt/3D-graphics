using MathNet.Numerics.LinearAlgebra;
using Newtonsoft.Json;
using System;

namespace _3D_graphics
{
    class JsonMatrixConverter : JsonConverter<Matrix<double>>
    {
        public override Matrix<double> ReadJson(JsonReader reader, Type objectType, Matrix<double> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var val = serializer.Deserialize<double[,]>(reader);
            return Matrix<double>.Build.DenseOfArray(val);
        }

        public override void WriteJson(JsonWriter writer, Matrix<double> value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToArray());
        }
    }
}
