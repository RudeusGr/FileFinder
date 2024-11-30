using System.Diagnostics;
using System.Text.Json;

namespace FileFinder;

public class Program
{
    static void Main()
    {
        ConfigurationJson? configurationJson = ReadConfigJson();
        if (configurationJson is null ||
            configurationJson.Extensions is null ||
            configurationJson.Months is null ||
            configurationJson.Urls is null)
        {
            Console.WriteLine("No se encontro algun parametro del archivo de configuracion");
            return;
        }
        Console.WriteLine("==================================================================");
        Console.WriteLine(@"
             _____ _ _      _____ _           _           
            |  ___(_) | ___|  ___(_)_ __   __| | ___ _ __ 
            | |_  | | |/ _ \ |_  | | '_ \ / _` |/ _ \ '__|
            |  _| | | |  __/  _| | | | | | (_| |  __/ |   
            |_|   |_|_|\___|_|   |_|_| |_|\__,_|\___|_|                                             
        ");
        Console.WriteLine("==================================================================");

        chooseOption(configurationJson);

        Console.WriteLine("==================================================================");
        Console.WriteLine("\t\tLa ventana se cerrara en un momento");
        Console.WriteLine("\t\tgracias por usar la aplicacion");
        Console.WriteLine("\t\tBy - Yosimar Zahid Aquino Sosa");
        Console.WriteLine("==================================================================");
    }

    static void chooseOption(ConfigurationJson configurationJson)
    {
        string? option;
        bool finallyProgram = true;
        do
        {
            Console.WriteLine("Por favor, seleccione de donde buscar los archivos:");
            Console.WriteLine("1...Generacion de archivos");
            Console.WriteLine("2...Recepcion de archivos");
            Console.WriteLine("3...Salir");
            Console.Write("-> ");
            option = Console.ReadLine();
            string fileName;
            switch (option)
            {
                case "1":
                    fileName = getFileName();
                    SearchFileGenerate(fileName, configurationJson.Extensions, configurationJson.Months, configurationJson.Urls["urlGenerateRemote"], configurationJson.Urls["urlGenerateLocal"]);
                    break;
                case "2":
                    fileName = getFileName();
                    SearchFileReception(fileName, configurationJson.Extensions, configurationJson.Months, configurationJson.Urls["urlReceptionRemote"], configurationJson.Urls["urlReceptionLocal"]);
                    break;
                case "3":
                    finallyProgram = false;
                    break;
                default:
                    Console.WriteLine("==================================================================");
                    Console.WriteLine("No fue seleccionado una opcion valida, intentelo nuevamente.");
                    Console.WriteLine("==================================================================");
                    break;
            }
        } while (finallyProgram);
    }

    static string getFileName()
    {
        string? fileName;
        do
        {
            Console.WriteLine("Por favor, escriba el nombre del archivo:");
            Console.Write("-> ");
            fileName = Console.ReadLine();
            if (fileName is null)
            {
                Console.WriteLine("El nombre proporcionado no es valido.");
                continue;
            }
            break;
        } while (true);
        return fileName;
    }

    static void SearchFileGenerate(string fileName, IList<string> extensions, Dictionary<string, string> months, string urlRemote, string urlLocal)
    {
        try
        {
            bool findFiles = false;
            foreach (string extension in extensions)
            {
                if (File.Exists(urlRemote + fileName + extension))
                {
                    Directory.CreateDirectory(urlLocal + fileName);
                    File.Copy(urlRemote + fileName + extension, urlLocal + fileName + "\\" + fileName + extension);
                    findFiles = true;
                }
            }

            if (!findFiles)
            {
                foreach (string extension in extensions)
                {
                    if (File.Exists(urlRemote + "RESPALDO\\" + fileName + extension))
                    {
                        Directory.CreateDirectory(urlLocal + fileName);
                        File.Copy(urlRemote + "RESPALDO\\" + fileName + extension, urlLocal + fileName + "\\" + fileName + extension);
                        findFiles = true;
                    }
                }
            }

            if (!findFiles)
            {
                string year = "20" + fileName.Substring(2, 2);
                string month = months[fileName.Substring(4, 2)];
                string deepDirectory = urlRemote + "RESPALDO" + "\\" + year + "\\" + month + "\\" + fileName;

                if (!Directory.Exists(deepDirectory))
                {
                    Console.WriteLine("==================================================================");
                    Console.WriteLine("\t\tNo existen archivos con este nombre");
                    Console.WriteLine("==================================================================");
                    return;
                }

                Directory.CreateDirectory(urlLocal + fileName);

                foreach (string pathFile in Directory.GetFiles(deepDirectory))
                {
                    string[] tempFileName = pathFile.Split('\\');
                    File.Copy(pathFile, urlLocal + fileName + "\\" + tempFileName[tempFileName.Length - 1]);
                }
            }

            Console.WriteLine("==================================================================");
            Console.WriteLine("\t\tArchivos encontrados, abriendo carpeta contenedora");
            Console.WriteLine("==================================================================");
            Process.Start("explorer.exe", urlLocal + fileName);
            return;
        }
        catch (IOException ioEx)
        {
            Console.WriteLine("==================================================================");
            Console.WriteLine($"Error al copiar el archivo.\nError: {ioEx.Message}");
            Console.WriteLine("==================================================================");
        }
    }

    static void SearchFileReception(string fileName, IList<string> extensions, Dictionary<string, string> months, string urlRemote, string urlLocal)
    {
        try
        {
            bool findFiles = false;
            foreach (string extension in extensions)
            {
                if (File.Exists(urlRemote + "RESPALDOA\\" + fileName + extension))
                {
                    Directory.CreateDirectory(urlLocal + fileName);
                    File.Copy(urlRemote + "RESPALDOA\\" + fileName + extension, urlLocal + fileName + "\\" + fileName + extension);
                    findFiles = true;
                }
            }

            if (!findFiles)
            {
                foreach (string extension in extensions)
                {
                    if (File.Exists(urlRemote + "RESPALDO\\" + fileName + extension))
                    {
                        Directory.CreateDirectory(urlLocal + fileName);
                        File.Copy(urlRemote + "RESPALDO\\" + fileName + extension, urlLocal + fileName + "\\" + fileName + extension);
                        findFiles = true;
                    }
                }
            }

            if (!findFiles)
            {
                string year = "20" + fileName.Substring(2, 2);
                string month = months[fileName.Substring(4, 2)];
                string deepDirectory = urlRemote + "RESPALDO" + "\\" + year + "\\" + month + "\\" + fileName;

                if (!Directory.Exists(deepDirectory))
                {
                    Console.WriteLine("==================================================================");
                    Console.WriteLine("\t\tNo existen archivos con este nombre");
                    Console.WriteLine("==================================================================");
                    return;
                }

                Directory.CreateDirectory(urlLocal + fileName);

                foreach (string pathFile in Directory.GetFiles(deepDirectory))
                {
                    string[] tempFileName = pathFile.Split('\\');
                    File.Copy(pathFile, urlLocal + fileName + "\\" + tempFileName[tempFileName.Length - 1]);
                }
            }

            Console.WriteLine("==================================================================");
            Console.WriteLine("\t\tArchivos encontrados, abriendo carpeta contenedora");
            Console.WriteLine("==================================================================");
            Process.Start("explorer.exe", urlLocal + fileName);
            return;
        }
        catch (IOException ioEx)
        {
            Console.WriteLine("==================================================================");
            Console.WriteLine($"Error al copiar el archivo.\nError: {ioEx.Message}");
            Console.WriteLine("==================================================================");
        }
    }

    static ConfigurationJson? ReadConfigJson()
    {
        try
        {
            string jsonString = File.ReadAllText("Config.json");
            ConfigurationJson? configurationJson = JsonSerializer.Deserialize<ConfigurationJson>(jsonString);
            return configurationJson;
        }
        catch (IOException ioEx)
        {
            Console.WriteLine("==================================================================");
            Console.WriteLine($"No se pudo leer un archivo\nError: {ioEx.Message}");
            Console.WriteLine("==================================================================");
            return null;
        }
        catch (ArgumentNullException ArgNullEx)
        {
            Console.WriteLine("==================================================================");
            Console.WriteLine($"Se esta pasando algun parametro nulo\nError: {ArgNullEx.Message}");
            Console.WriteLine("==================================================================");
            return null;
        }
    }
}
