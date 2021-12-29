using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace UnosquareTest.Behaviors
{
    public class NumbersValidationBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += OnEntryTextChanged;

            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= OnEntryTextChanged;

            base.OnDetachingFrom(bindable);
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                var isOnlyNumbers = Regex.IsMatch(e.NewTextValue, OnlyDigitsRegex);

                if (isOnlyNumbers)
                    ((Entry)sender).TextColor = Color.Black;
                else
                    ((Entry)sender).TextColor = Color.Red;

            }
        }

        private static readonly string OnlyDigitsRegex = @"^[0-9]+$";
    }
}
