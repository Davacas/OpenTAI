using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleFileBrowser;
using UnityEngine.SceneManagement; 
using TMPro; 

public class ConfigManager : MonoBehaviour {
    //Campos de texto con las rutas.
    public TMP_InputField rutaImagenes;
    public TMP_InputField rutaPalabras;
    public TMP_InputField rutaResultados;
    //Cuadros de texto con información.
    public TMP_Text infoImagenes;
    public TMP_Text infoPalabras;
    public TMP_Text infoResultados;
    //Máximo de imágenes que soporta el programa.
    const int MAX_IMG = 100;

    void Start() {
        if (PlayerPrefs.HasKey("rutaImagenes")) {
            rutaImagenes.text = PlayerPrefs.GetString("rutaImagenes");
            ValidarRutaImagenes();
        }
        else {
            infoImagenes.text = "> Información sobre la carpeta con las imágenes.";
        }

        if (PlayerPrefs.HasKey("rutaPalabras")) {
            rutaPalabras.text = PlayerPrefs.GetString("rutaPalabras");
            ValidarArchivoPalabras();
        }
        else {
            infoResultados.text = " > Información sobre el archivo con las palabras.";
        }

        if (PlayerPrefs.HasKey("rutaResultados")) {
            rutaResultados.text = PlayerPrefs.GetString("rutaResultados");
            ValidarRutaResultados();
        }
        else {
            infoResultados.text = " > Información sobre el archivo de resultados.";
        }        
    }

    public bool ValidarRutaImagenes() {
        string rutaImg = PlayerPrefs.GetString("rutaImagenes");
        int imagenes = 0;

        //Se busca la existencia de imágenes en la ruta proporcionada.
        for (int i = 1; i < MAX_IMG; i++) {
            if (System.IO.File.Exists(rutaImg + "\\" + i + ".jpg") ||
                System.IO.File.Exists(rutaImg + "\\" + i + ".jpeg") ||
                System.IO.File.Exists(rutaImg + "\\" + i + ".png")) {
                    imagenes ++;
            }
        }
        //Se notifica la cantidad de imágenes encontradas.
        if (imagenes > 0) {
            infoImagenes.text = " > Se encontraron " + imagenes + " imágenes.";
            infoImagenes.color = Color.green;
            PlayerPrefs.SetInt("numImg", imagenes);
            return true;
        }
        else {
            infoImagenes.text = " > No se encontraron imágenes. Consulta la sección 'blabla' del manual para más información.";
            infoImagenes.color = Color.red;
            return false;
        }
    }

    public bool ValidarArchivoPalabras() {
        string rutaPals = PlayerPrefs.GetString("rutaPalabras");
        string[] lineas; 

        //Validación de extensión del archivo.
        if (rutaPals.Contains(".txt")) {
            //Validación de existencia del archivo.
            if (System.IO.File.Exists(rutaPals)) { 
                lineas = System.IO.File.ReadAllLines(rutaPals);
                //Validación de cantidad de líneas del archivo. 
                if (lineas.Length > PlayerPrefs.GetInt("numImg")) {
                    infoPalabras.text = " > El archivo tiene más pares de palabras que imágenes.";
                    infoPalabras.color = Color.red;
                    return false;
                }
                else if (lineas.Length < PlayerPrefs.GetInt("numImg")) {
                    infoPalabras.text = " > El archivo tiene menos pares de palabras que imágenes.";
                    infoPalabras.color = Color.red;
                    return false;
                }
                //Validación del formato interno del archivo.
                else {
                    foreach (var cadena in lineas) {
                        if (!cadena.Contains(",")) {
                            infoPalabras.text = " > El contenido del archivo es incorrecto. Consulta la sección 'blabla' del manual para más información.";
                            infoPalabras.color = Color.red;
                            return false;
                        }
                    }
                }
                //Si se pasan todas las validaciones, es correcto.
                infoPalabras.text = " > Se encontró un archivo con pares de palabras.";
                infoPalabras.color = Color.green;
                return true;
            }
            //Si no existe un archivo con ese nombre, se notifica.
            else {
                infoPalabras.text = " > No existe un archivo con ese nombre.";
                infoPalabras.color = Color.red;
                return false;
            }
        }
        //Si no es de tipo TXT, se notifica.
        else {
            infoPalabras.text = " > El archivo debe ser de tipo TXT.";
            infoPalabras.color = Color.red;
            return false;
        }        
    }

    public bool ValidarRutaResultados() {
        string rutaRes = PlayerPrefs.GetString("rutaResultados");

        if (System.IO.File.Exists(rutaRes)) { 
            infoResultados.text = " > Se encontró un archivo de resultados. Se continuará escribiendo sobre él.";
            infoResultados.color = Color.green;
            return true;
        }
        else {
            try {
                System.IO.File.WriteAllText(PlayerPrefs.GetString("rutaResultados"), "");
            }
            catch {
                infoResultados.text = " > No se pudo crear el archivo de resultados. Intenta en otra ruta.";
                infoResultados.color = Color.red;
            }
            return false;
        }        
    }


    /*FUNCIONES PARA BOTONES*/
    public void ElegirRutaImagenes() {
        FileBrowser.ShowLoadDialog(
            (path) => {  
                //OnSuccess
                rutaImagenes.text = path[0];
                PlayerPrefs.SetString("rutaImagenes", path[0]);
                Debug.Log(PlayerPrefs.GetString("rutaImagenes"));
                ValidarRutaImagenes();
            }, 
            //OnCancel, folderMode, multiSelect, initialPath, title,                    buttonText
            null,       true,       false,          null,       "Seleccionar carpeta", "Seleccionar" );
    }

    public void ElegirArchivoPalabras() {
        FileBrowser.SetFilters( true, new FileBrowser.Filter( "Text", ".txt"));
        FileBrowser.ShowLoadDialog(
            (path) => {  
                //OnSuccess
                rutaPalabras.text = path[0];
                PlayerPrefs.SetString("rutaPalabras", path[0]);
                Debug.Log(PlayerPrefs.GetString("rutaPalabras"));
                ValidarArchivoPalabras();
            }, 
            //OnCancel, folderMode, multiSelect, initialPath, title,                    buttonText
            null,       false,       false,          null,       "Seleccionar archivo de palabras", "Seleccionar" );
    }

    public void ElegirRutaResultados() {
        //ShowLoadDialog( , OnCancel onCancel, bool folderMode = false, bool allowMultiSelection = false, string initialPath = null, string title = "Load", string loadButtonText = "Select" );
        FileBrowser.ShowSaveDialog(
            (path) => {  
                //OnSuccess
                if (!path[0].Contains(".csv")) {
                    path[0] += ".csv";
                }
                rutaResultados.text = path[0];
                PlayerPrefs.SetString("rutaResultados", path[0]);
                Debug.Log(PlayerPrefs.GetString("rutaResultados"));
                ValidarRutaResultados();
            }, 
            //OnCancel, folderMode, multiSelect, initialPath, title,                    buttonText
            null,       false,       false,          null,       "Guardar archivo de resultados", "Guardar" );
    }

    public void ValidarContinuar() {
        if (ValidarRutaImagenes() && ValidarArchivoPalabras() && ValidarRutaResultados()) {
            SceneManager.LoadScene("Test");
        }
    }

    public void Salir() {
        Application.Quit();
    }
}
