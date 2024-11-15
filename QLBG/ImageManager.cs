using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Image = System.Drawing.Image;

namespace QLBG
{
    public static class ImageManager
    {
        private static string _productImagesFolder = Path.Combine(Directory.GetParent(Application.StartupPath).Parent.FullName, @"Resources\ProductImages");
        private static string _defaultProductImage = "default_product";
        private static Dictionary<string, Image> _imageReferences = new Dictionary<string, Image>();

        static ImageManager()
        {
            if (!Directory.Exists(_productImagesFolder))
            {
                Directory.CreateDirectory(_productImagesFolder);
            }
        }

        public static string AddProductImage(string sourceFilePath)
        {
            if (string.IsNullOrWhiteSpace(sourceFilePath) || !File.Exists(sourceFilePath))
            {
                throw new FileNotFoundException("File ảnh không tồn tại.");
            }

            string fileExtension = Path.GetExtension(sourceFilePath);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sourceFilePath); 
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            if (String.IsNullOrEmpty(fileNameWithoutExtension) || String.IsNullOrEmpty(fileExtension))
            {
                fileExtension = ".png";
                fileNameWithoutExtension = _defaultProductImage;
            }

            string newFileName = $"{fileNameWithoutExtension}_{timestamp}{fileExtension}";
            string destFilePath = Path.Combine(_productImagesFolder, newFileName);

            File.Copy(sourceFilePath, destFilePath, overwrite: true);

            return newFileName;
        }

        public static Image GetProductImage(string imageName)
        {
            string imagePath = GetProductImagePath(imageName);

            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
            {
                imagePath = GetProductImagePath(_defaultProductImage + ".png");
            }

            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
            {
                MessageBox.Show(imageName);
                throw new FileNotFoundException("Ảnh không tồn tại.");
            }

            if (!_imageReferences.ContainsKey(imageName))
            {
                _imageReferences[imageName] = Image.FromFile(imagePath);
            }

            return _imageReferences[imageName];
        }

        public static string GetProductImagePath(string imageName)
        {
            var files = Directory.GetFiles(_productImagesFolder, imageName);
            return files.Length > 0 ? files[0] : null;
        }

        public static void DeleteProductImage(string imageName)
        {
            if (imageName == _defaultProductImage + ".png")
            {
                return;
            }
            string imagePath = GetProductImagePath(imageName);

            if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
            {
                if (_imageReferences.ContainsKey(imageName))
                {
                    _imageReferences[imageName].Dispose();
                    _imageReferences.Remove(imageName);
                }

                File.Delete(imagePath);
            }
        }

        public static void ClearAllImages()
        {
            foreach (var image in _imageReferences.Values)
            {
                image.Dispose();
            }

            _imageReferences.Clear();

            foreach (var file in Directory.GetFiles(_productImagesFolder))
            {
                File.Delete(file);
            }
        }
    }
}
