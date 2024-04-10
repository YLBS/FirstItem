using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("[controller]/[action]")]
    public class UploadFileController : Controller
    {
        
        [HttpPost]
        
        public ActionResult UploadFile2(List<IFormFile> files1)   //
        {
            IFormFileCollection files = Request.Form.Files;
            
            string tyle = Request.Form["type"];
            string filePath = "";
            int j = 0;
            foreach (IFormFile file in files) {
                j++;
                if (file == null || file.Length <= 0)
                    return BadRequest();
                string header = $"File";
                var fileName = $"\\car\\{DateTime.Now:yyyyMMddHHmmss}"+j+ tyle + Path.GetFileName(Path.GetExtension(file.FileName));

                var pathToSave = Directory.GetCurrentDirectory() + "\\" + header + fileName; // 设置存储文件的目标路径
                try {
                    using (var stream = new FileStream(pathToSave, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }catch (Exception ex)
                {
                    fileName= UploadFile1(file);
                }
                filePath += fileName + ";";
            }

            
            return Ok($"{filePath}");
        }

        [HttpPost]
        public ActionResult UploadFile(IFormFile file)
        {
            if (file == null || file.Length <= 0)
                return BadRequest();
            string header = $"File";
            var fileName = $"\\car\\{DateTime.Now:yyyyMMddHHmmss}" + Path.GetFileName(Path.GetExtension(file.FileName));

            var pathToSave = Directory.GetCurrentDirectory() + "\\" + header + fileName; // 设置存储文件的目标路径
            using (var stream = new FileStream(pathToSave, FileMode.Create))
            {
                file.CopyTo(stream);
            }


            return Ok($"{fileName}");
        }
        public static string UploadFile1(IFormFile file)
        {
            if (file == null || file.Length <= 0)
                return "没有文件";
            string header = $"File";
            var fileName = $"\\car\\{DateTime.Now:yyyyMMddHHmmss}" + Path.GetFileName(Path.GetExtension(file.FileName));

            var pathToSave = Directory.GetCurrentDirectory() + "\\" + header + fileName; // 设置存储文件的目标路径
            using (var stream = new FileStream(pathToSave, FileMode.Create))
            {
                file.CopyTo(stream);
            }


            return fileName;
        }

    }
}
