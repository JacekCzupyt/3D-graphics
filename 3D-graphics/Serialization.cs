using System.Collections.Generic;
using _3D_graphics.Objects;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace _3D_graphics
{
    public partial class MainWindow : System.Windows.Window
    {
        private void SaveScene(object sender, System.Windows.RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Scene|*.json";
            dialog.Title = "Save the scene";
            dialog.ShowDialog();

            if (dialog.FileName != "")
            {
                using (System.IO.FileStream fs =
                    (System.IO.FileStream)dialog.OpenFile())
                {

                    string json = JsonConvert.SerializeObject(
                        Scene, 
                        new JsonSerializerSettings() {
                            Formatting = Formatting.Indented, 
                            TypeNameHandling = TypeNameHandling.Auto
                        }
                    );

                    using (var writer = new System.IO.StreamWriter(fs))
                    {
                        writer.Write(json);
                    }
                }
            }
        }

        private void LoadScene(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Scene|*.json";
            dialog.Title = "Open a scene";
            dialog.ShowDialog();

            if (dialog.FileName != "")
            {
                using (System.IO.FileStream fs =
                    (System.IO.FileStream)dialog.OpenFile())
                {
                    using (var reader = new System.IO.StreamReader(fs))
                    {
                        string json = reader.ReadToEnd();
                        List<IWireframeObject> obj = JsonConvert.DeserializeObject<List<IWireframeObject>>(
                            json,
                            new JsonSerializerSettings()
                            {
                                Formatting = Formatting.Indented,
                                TypeNameHandling = TypeNameHandling.Auto
                            }
                        );

                        Scene = obj as List<IWireframeObject>;
                    }
                }
            }
        }
    }
}
