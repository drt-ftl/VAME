using UnityEngine;
using System.IO;
using System.Collections;

public class SliInterpreter : MonoBehaviour
{

	void Start ()
    {	
	}

	void Update ()
    {	
	}

    public void LoadSli()
    {
        System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
        openFileDialog.InitialDirectory = Application.dataPath + "/Samples";
        var sel = "SLI Files (*.sli)|*.sli|";
        sel = sel.TrimEnd('|');
        openFileDialog.Filter = sel;
        openFileDialog.FilterIndex = 2;
        openFileDialog.RestoreDirectory = false;

        if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            try
            {
                var fileName = openFileDialog.FileName;
                if (fileName != null)
                {
                    var reader = new StreamReader(fileName);
                    string full = "";
                    while (!reader.EndOfStream)
                    {
                        full = reader.ReadLine();
                        System.Text.ASCIIEncoding Encoding = new System.Text.ASCIIEncoding();
                        var bytes = Encoding.GetBytes(full);
                        var text = Encoding.GetString(bytes);
                    }
                    reader.Close();
                }
            }
            catch { }

        }
    }
}
