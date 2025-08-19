﻿namespace Smab.TTInfo.CunninghamCup.Shared.Helpers;

/// <summary>
/// Provides a custom JSON converter for a record type that maps a single value to a record instance.
/// </summary>
/// <remarks>This converter allows a record type to be serialized and deserialized as a single JSON value. The
/// <paramref name="creator"/> function is used to create a record instance from the single value during
/// deserialization, and the <paramref name="extractor"/> function is used to extract the single value from the record
/// during serialization.</remarks>
/// <typeparam name="TRecord">The type of the record being serialized or deserialized.</typeparam>
/// <typeparam name="TValue">The type of the single value used to represent the record in JSON.</typeparam>
/// <param name="creator">A function to create a record instance from a single value.</param>
/// <param name="extractor">A function to extract the single value from a record instance.</param>
public class SingleValueConverter<TRecord, TValue>(Func<TValue?, TRecord> creator, Func<TRecord, TValue?> extractor) : JsonConverter<TRecord>
{
	/// <summary>
	/// Reads and converts JSON data into an instance of the specified record type.
	/// </summary>
	/// <remarks>If the JSON token is <see cref="JsonTokenType.Null"/>, the method returns a default instance of
	/// <typeparamref name="TRecord"/>. Otherwise, the method deserializes the JSON data into an instance of <typeparamref
	/// name="TValue"/> and uses the creator function to create the corresponding <typeparamref name="TRecord"/>.</remarks>
	/// <param name="reader">The <see cref="Utf8JsonReader"/> used to read the JSON data.</param>
	/// <param name="typeToConvert">The type of the record to convert the JSON data into.</param>
	/// <param name="options">Options to customize the JSON serialization behavior.</param>
	/// <returns>An instance of <typeparamref name="TRecord"/> created from the JSON data.</returns>
	public override TRecord Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		// Handle nulls explicitly
		if (reader.TokenType == JsonTokenType.Null) {
			return creator(default);
		}

		TValue? value = JsonSerializer.Deserialize<TValue>(ref reader, options);
		return creator(value);
	}

	/// <summary>
	/// Writes the JSON representation of the specified record using the provided <see cref="Utf8JsonWriter"/>.
	/// </summary>
	/// <remarks>This method serializes the inner value of the specified record, as determined by the accessor
	/// function.</remarks>
	/// <param name="writer">The <see cref="Utf8JsonWriter"/> to which the JSON data will be written. This parameter cannot be <see
	/// langword="null"/>.</param>
	/// <param name="value">The record to serialize. This parameter cannot be <see langword="null"/>.</param>
	/// <param name="options">The <see cref="JsonSerializerOptions"/> to use during serialization. This parameter cannot be <see
	/// langword="null"/>.</param>
	public override void Write(Utf8JsonWriter writer, TRecord value, JsonSerializerOptions options)
	{
		TValue? innerValue = extractor(value);
		JsonSerializer.Serialize(writer, innerValue, options);
	}
}
