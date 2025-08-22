namespace Smab.TTInfo.CunninghamCup.Shared.Models;

[JsonConverter(typeof(MatchIdConverter))]
public readonly record struct MatchId(string StringId)
{
	public override string ToString() => StringId;
	public static implicit operator string(MatchId id) => id.StringId;
	public static explicit operator MatchId(string value) => new(value);
}

/// <summary>
/// Converts a <see cref="MatchId"/> object to its string representation and vice versa.
/// </summary>
/// <remarks>This converter is designed to facilitate serialization and deserialization of <see
/// cref="MatchId"/>  objects by converting them to and from their string representations. It uses a creator
/// function to  instantiate <see cref="MatchId"/> objects and an extractor function to retrieve their string
/// values.</remarks>
public class MatchIdConverter : SingleValueConverter<MatchId, string>
{
	public MatchIdConverter() : base(creator => new MatchId(creator!), extractor => extractor.ToString()) { }

	// Support dictionary key serialization
	public override MatchId ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		=> new(reader.GetString()!);

	public override void WriteAsPropertyName(Utf8JsonWriter writer, MatchId value, JsonSerializerOptions options)
		=> writer.WritePropertyName(value.StringId);

}
