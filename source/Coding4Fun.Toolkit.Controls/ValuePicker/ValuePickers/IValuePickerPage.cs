namespace Coding4Fun.Toolkit.Controls.Primitives
{
    public interface IValuePickerPage<T> where T : struct
    {
        /// <summary>
        /// Hooks up datasources
        /// </summary>
        void InitDataSource();

        /// <summary>
        /// Gets or sets the DateTime to show in the picker page and to set when the user makes a selection.
        /// </summary>
        T? Value { get; set; }

		string DialogTitle { get; set; }
    }
}
