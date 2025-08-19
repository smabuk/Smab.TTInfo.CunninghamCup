namespace Smab.TTInfo.CunninghamCup.Shared.Models;

[JsonConverter(typeof(MatchIdConverter))]
public readonly record struct MatchId(string stringId)
{
	public override string ToString() => stringId;
	public static implicit operator string(MatchId id) => id.stringId;
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
}
