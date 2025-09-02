using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Smab.TTInfo.CunninghamCup.Shared.Components;

public class CustomValidation : ComponentBase
{
	private ValidationMessageStore? _messageStore;

	[CascadingParameter]
	private EditContext? CurrentEditContext { get; set; }

	protected override void OnInitialized()
	{
		if (CurrentEditContext is null) {
			throw new InvalidOperationException(
				$"{nameof(CustomValidation)} requires a cascading " +
				$"parameter of type {nameof(EditContext)}. " +
				$"For example, you can use {nameof(CustomValidation)} " +
				$"inside an {nameof(EditForm)}.");
		}

		_messageStore = new(CurrentEditContext);

		CurrentEditContext.OnValidationRequested += (s, e) =>
			_messageStore?.Clear();
		CurrentEditContext.OnFieldChanged += (s, e) =>
			_messageStore?.Clear(e.FieldIdentifier);
	}

	public void DisplayErrors(Dictionary<string, List<string>> errors)
	{
		if (CurrentEditContext is not null) {
			foreach ((string key, List<string> value) in errors) {
				_messageStore?.Add(CurrentEditContext.Field(key), value);
			}

			CurrentEditContext.NotifyValidationStateChanged();
		}
	}

	public void ClearErrors()
	{
		_messageStore?.Clear();
		CurrentEditContext?.NotifyValidationStateChanged();
	}
}