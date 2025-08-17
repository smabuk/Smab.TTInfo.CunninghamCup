namespace Smab.TTInfo.CunninghamCup.Shared.Models;

public record Match(
	Player PlayerA,
	Player PlayerB,
	DateTime? ScheduledTime,
	Result? Result
);
