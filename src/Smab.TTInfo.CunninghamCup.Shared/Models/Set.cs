using System.ComponentModel.DataAnnotations;

namespace Smab.TTInfo.CunninghamCup.Shared.Models;

public record Set(
	[Range(0, 30)] int PlayerAScore,
	[Range(0, 30)] int PlayerBScore
);