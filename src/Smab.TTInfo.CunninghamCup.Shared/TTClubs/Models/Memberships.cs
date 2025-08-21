﻿namespace Smab.TTInfo.CunninghamCup.Shared.TTClubs.Models;


public sealed record Memberships
(
	int Page,
	int Size,
	int Total,
	int Pages,
	ImmutableList<MembershipsItem> Items,
	bool HasPreviousPage,
	bool HasNextPage
);
