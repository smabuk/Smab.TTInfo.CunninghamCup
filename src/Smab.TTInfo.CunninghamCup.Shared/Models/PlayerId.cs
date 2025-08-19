namespace Smab.TTInfo.CunninghamCup.Shared.Models;

[JsonConverter(typeof(PlayerIdConverter))]
public readonly record struct PlayerId(string stringId)
{
	public override string ToString() => stringId;
	public static implicit operator string(PlayerId id) => id.stringId;
	public static explicit operator PlayerId(string value) => new(value);
}

/// <summary>
/// Converts a <see cref="PlayerId"/> object to its string representation and vice versa.
/// </summary>
/// <remarks>This converter is designed to facilitate serialization and deserialization of <see
/// cref="PlayerId"/>  objects by converting them to and from their string representations. It uses a creator
/// function to  instantiate <see cref="PlayerId"/> objects and an extractor function to retrieve their string
/// values.</remarks>
public class PlayerIdConverter : SingleValueConverter<PlayerId, string>
{
	public PlayerIdConverter() : base(creator => new PlayerId(creator!), extractor => extractor.ToString()) { }
}
