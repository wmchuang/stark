using System.Text.Json;

namespace AIModule;

public interface IUtf8JsonSerializable
{
    void Write(Utf8JsonWriter writer);
}