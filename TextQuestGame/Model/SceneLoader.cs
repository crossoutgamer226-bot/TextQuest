using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace TextQuestGame.Model
{
    public class SceneLoader
    {
        private const string DefaultImage = "Images/default.jpg";

        public static Dictionary<string, Scene> LoadScenes(string filePath)
        {
            try
            {
                if(!File.Exists(filePath))
                {
                    Console.WriteLine($"Файл сцен не найден: {filePath}. Используются сцены по умолчанию.");
                    return GetDefaultScenes();
                }

                var json = File.ReadAllText(filePath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var scenes = JsonSerializer.Deserialize<List<Scene>>(json, options);

                ValidateAndFixScenes(scenes);

                var sceneDict = new Dictionary<string, Scene>();
                foreach (var scene in scenes)
                {
                    if (sceneDict.ContainsKey(scene.Id))
                    {
                        Console.WriteLine($"Предупреждение: Дублирующаяся сцена с ID: {scene.Id}");
                    }
                    sceneDict[scene.Id] = scene;
                }

                Console.WriteLine($"Загружено сцен: {sceneDict.Count}");
                return sceneDict;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки сцен: {ex.Message}");
                return GetDefaultScenes();
            }
        }

        private static void ValidateAndFixScenes(List<Scene> scenes)
        {
            if (scenes == null)
                return;

            foreach (var scene in scenes)
            {
                scene.ImagePath = ValidateAndFixSceneImage(scene.Id, scene.ImagePath);

                foreach (var choice in scene.Choices ?? new List<Choice>())
                {
                    if (string.IsNullOrEmpty(choice.Text))
                    {
                        choice.Text = "Продолжить...";
                        Console.WriteLine($"Предупреждение: У сцены '{scene.Id}' есть выбор без текста");
                    }
                }
            }
        }

        private static string ValidateAndFixSceneImage(string sceneId, string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                Console.WriteLine($"Сцена '{sceneId}' без картинки. Используется картинка по умолчанию.");
                return DefaultImage;
            }

            var extension = Path.GetExtension(imagePath).ToLower();
            if (extension != ".jpg" && extension != ".jpeg")
            {
                Console.WriteLine($"Сцена '{sceneId}': не-JPG формат '{imagePath}'. Используется картинка по умолчанию.");
                return DefaultImage;
            }

            if (File.Exists(imagePath))
                return imagePath;

            var fileName = Path.GetFileName(imagePath);
            if (!string.IsNullOrEmpty(fileName))
            {
                var pathInImages = Path.Combine("Images", fileName);
                if (File.Exists(pathInImages))
                    return pathInImages;
            }

            Console.WriteLine($"Картинка для сцены '{sceneId}' не найдена: {imagePath}. Используется картинка по умолчанию.");
            return DefaultImage;
        }
        public static Dictionary<string, Scene> GetDefaultScenes()
        {
            Dictionary<string, Scene> scenes = new Dictionary<string, Scene>();

            scenes["start"] = new Scene
            {
                Id = "start",
                Text = "ScenceLoaderВы стоите в тёмной комнате. Перед вами две двери: левая и правая.",
                ImagePath = DefaultImage,
                Choices = new List<Choice>
                {
                    new Choice
                    {
                        Text = "Открыть левую дверь",
                        NextSceneId = "left_room",
                        Condition = "",
                        Effect = ""
                    },
                    new Choice
                    {
                        Text = "Открыть правую дверь",
                        NextSceneId = "right_room",
                        Condition = "",
                        Effect = ""
                    }
                }
            };

            scenes["left_room"] = new Scene
            {
                Id = "left_room",
                Text = "Вы вошли в библиотеку.",
                ImagePath = DefaultImage,
                Choices = new List<Choice>
                {
                    new Choice
                    {
                        Text = "Взять книгу",
                        NextSceneId = "book_taken",
                        Condition = "",
                        Effect = "AddItem:Древняя книга"
                    },
                    new Choice
                    {
                        Text = "Вернуться обратно",
                        NextSceneId = "start",
                        Condition = "",
                        Effect = ""
                    }
                }
            };

            scenes["right_room"] = new Scene
            {
                Id = "right_room",
                Text = "Комната заперта. Вам нужен ключ.",
                ImagePath = DefaultImage,
                Choices = new List<Choice>
                {
                    new Choice
                    {
                        Text = "Использовать ключ",
                        NextSceneId = "victory",
                        Condition = "",
                        Effect = ""
                    },
                    new Choice
                    {
                        Text = "Вернуться обратно",
                        NextSceneId = "start",
                        Condition = "",
                        Effect = ""
                    }
                }
            };

            Console.WriteLine("Используются сцены по умолчанию");
            return scenes;
        }
    }
}
