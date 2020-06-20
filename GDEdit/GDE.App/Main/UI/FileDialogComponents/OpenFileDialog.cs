namespace GDE.App.Main.UI.FileDialogComponents
{
    public class OpenFileDialog : FileDialog
    {
        protected override bool AllowInexistentFileNames => false;
        protected override string FileDialogActionName => "Open";

        public OpenFileDialog() : base() { }
        public OpenFileDialog(string defaultDirectory = null) : base(defaultDirectory) { }
    }
}
