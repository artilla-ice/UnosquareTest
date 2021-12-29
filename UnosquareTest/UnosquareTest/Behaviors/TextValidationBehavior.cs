using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace UnosquareTest.Behaviors
{
    public class TextValidationBehavior : Behavior<Entry>
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
            if(!string.IsNullOrEmpty(e.NewTextValue))
            {
                var isOnlyText = Regex.IsMatch(e.NewTextValue, OnlyTextRegex);

                if (isOnlyText)
                    ((Entry)sender).TextColor = Color.Black;
                else
                    ((Entry)sender).TextColor = Color.Red;
            }
        }

        //adding \s to allow spaces for full name
        private static readonly string OnlyTextRegex = @"^[a-zA-Z\s]+$";
    }
}
