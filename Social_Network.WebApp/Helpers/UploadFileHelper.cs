namespace Social_Network.Presentation.WebApp.Helpers
{
    public static class UploadFileHelper
    {
        public static string SaveFile(IFormFile file, string userId, string folderName, bool isEditing = false, string imagePath = "")
        {
            if (isEditing && file == null)
                return imagePath;
            if (file == null || file.Length == 0)
                return "";
            string basePath = $"Images/{folderName}/{userId}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/{basePath}");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            Guid guid = Guid.NewGuid();
            FileInfo fileInfo = new FileInfo(file.FileName);
            string fileName = $"{guid}{fileInfo.Extension}";
            string filePath = Path.Combine(path, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            if(isEditing && !String.IsNullOrEmpty(imagePath))
            {
                string[] olldImageParts = imagePath.Split('/');
                string oldFileName = olldImageParts[^1];
                string oldFilePath = Path.Combine(path, oldFileName);
                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                }
            }

            return $"{basePath}/{fileName}";

        }
    }
}
