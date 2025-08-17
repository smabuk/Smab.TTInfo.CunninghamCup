namespace Smab.TTInfo.CunninghamCup.Shared.Models;

public record Match(
	PlayerEntry PlayerA,
	PlayerEntry PlayerB,
	DateTime? ScheduledTime,
	Result? Result
);
