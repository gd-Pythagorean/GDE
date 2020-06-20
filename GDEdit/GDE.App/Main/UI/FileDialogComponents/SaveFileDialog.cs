namespace GDE.App.Main.UI.FileDialogComponents
{
    public class SaveFileDialog : FileDialog
    {
        protected override bool AllowInexistentFileNames => true;
        protected override string FileDialogActionName => "Save";

        public SaveFileDialog() : base() { }
        public SaveFileDialog(string defaultDirectory = null) : base(defaultDirectory) { }
    }
}
